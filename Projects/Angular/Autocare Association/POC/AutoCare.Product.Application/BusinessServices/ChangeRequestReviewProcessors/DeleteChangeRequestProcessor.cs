using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoCare.Product.Application.BusinessServices.Event;
using AutoCare.Product.Application.RepositoryServices;
using AutoCare.Product.Infrastructure.Bus.EventBus;
using AutoCare.Product.Infrastructure.RepositoryService;
using AutoCare.Product.Infrastructure.Serializer;
using AutoCare.Product.Vcdb.Model;
using AutoCare.Product.VcdbSearch.Indexing.ApplicationService;

namespace AutoCare.Product.Application.BusinessServices.ChangeRequestReviewProcessors
{
    public abstract class DeleteChangeRequestProcessor : ChangeRequestReviewProcessor, IDeleteChangeRequestProcessor
    {
        private readonly IAzureFileStorageRepositoryService _azureFileStorageRepositoryService;
        private readonly ITextSerializer _serializer;
        private readonly IEventBus _eventBus;
        private readonly string _containerName;
        private readonly IUnitOfWork _unitOfWork;

        protected DeleteChangeRequestProcessor(
            IChangeRequestCommentsBusinessService<CommentsStaging> commentsStagingBusinessService,
            IChangeRequestAttachmentBusinessService<AttachmentsStaging> attachmentStagingBusinessService,
            IChangeRequestItemBusinessService<ChangeRequestItemStaging> changeRequestItemStagingBusinessService,
            IAzureFileStorageRepositoryService azureFileStorageRepositoryService,
            ITextSerializer serializer,
            IUnitOfWork unitofWork,
            IEventBus eventBus,
            IChangeRequestIndexingService changeRequestIndexingService,
            string containerName)
            : base(commentsStagingBusinessService, attachmentStagingBusinessService,
                  changeRequestItemStagingBusinessService, azureFileStorageRepositoryService, 
                  unitofWork, changeRequestIndexingService)
        {
            _azureFileStorageRepositoryService = azureFileStorageRepositoryService;
            _serializer = serializer;
            _eventBus = eventBus;
            _containerName = containerName;
            _unitOfWork = unitofWork;
        }

        public async Task<bool> ProcessAsync(long changeRequestId, string deletedBy,
            CommentsStaging commentsStaging = null, List<AttachmentsStaging> attachments = null)
        {
            long reviewCommentId = 0;
            try
            {
                reviewCommentId = await AddCommentsAsync(changeRequestId, commentsStaging, deletedBy);
                var changeRequestStaging = await UpdateChangeRequestStatusAsync(changeRequestId);
                await SaveAttachmentsAsync(attachments, changeRequestStaging);
                var changeRequestStore =
                    await AddToChangeRequestStoreAsync(changeRequestId, ChangeRequestStatus.Deleted, deletedBy);
                var removedChangeRequestStaging = await RemoveFromChangeRequestStagingAsync(changeRequestId);

                if (changeRequestStore != null && removedChangeRequestStaging != null)
                {
                    var isTansactionalTableUpdated = await this.UpdateToTransactionalTableAsync(changeRequestStore, changeRequestId);
                    if (isTansactionalTableUpdated)
                    {
                        if (await this.Repositories.SaveChangesAsync() > 0)
                        {
                            await UpdateChangeRequestStatusToChangeRequestIndexAsync(changeRequestId,
                                ChangeRequestStatus.Deleted, reviewCommentId);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                if (reviewCommentId > 0)
                {
                    await this.CommentsStagingBusinessService.RemoveAsync(reviewCommentId);
                    await this.CommentsStagingBusinessService.SaveChangesAsync();
                }
                throw;
            }
            return true;
        }

        public async Task<long> AddCommentsAsync(long changeRequestId, CommentsStaging commentsStaging,
            string deletedBy)
        {
            long reviewCommentId = 0;
            if (commentsStaging != null)
            {
                CommentsStaging changeRequestCommentsStaging = new CommentsStaging()
                {
                    Id = 0,
                    Comment = commentsStaging.Comment,
                    ChangeRequestId = Convert.ToInt64(changeRequestId.ToString()),
                    AddedBy = deletedBy,
                    CreatedDatetime = DateTime.UtcNow
                };

                this.CommentsStagingBusinessService.Add(changeRequestCommentsStaging);

                if (await this.CommentsStagingBusinessService.SaveChangesAsync() > 0)
                {
                    reviewCommentId = changeRequestCommentsStaging.Id;
                }
            }
            return reviewCommentId;
        }

        public async Task SaveAttachmentsAsync(List<AttachmentsStaging> attachmentsStaging,
            ChangeRequestStaging changeRequestStaging)
        {
            if (attachmentsStaging != null && attachmentsStaging.Count > 0)
            {
                var folderPath = GetFolderPath(changeRequestStaging.Id);
                foreach (var attachment in attachmentsStaging)
                {
                    if (attachment.FileStatus == FileStatus.Deleted)
                    {
                        continue;
                    }

                    await SaveFileToTempLocation(attachment);
                }

                changeRequestStaging.AttachmentsStagings = changeRequestStaging.AttachmentsStagings ??
                                                           new List<AttachmentsStaging>();

                foreach (var attachment in attachmentsStaging)
                {
                    if (attachment.FileStatus == FileStatus.Deleted)
                    {
                        await
                            _azureFileStorageRepositoryService.DeleteAsync(attachment.AzureContainerName,
                                attachment.FileName, attachment.DirectoryPath);

                        if (attachment.AttachmentId > 0)
                        {
                            // note: remove from staging table.
                            await this.AttachmentStagingBusinessService.RemoveAsync(attachment.AttachmentId);
                        }
                        continue;
                    }

                    var newUri = await
                        MoveFileFromTempLocationToPermanent(attachment.AzureContainerName, _containerName,
                            attachment.FileName, folderPath);
                    attachment.DirectoryPath = folderPath;
                    attachment.AzureContainerName = _containerName;
                    attachment.ChangeRequestId = changeRequestStaging.Id;

                    this.AttachmentStagingBusinessService.Add(attachment);
                }
            }

            await this.AttachmentStagingBusinessService.SaveChangesAsync();
            // note: saveChangesAsync() will be performed on repective method call.
            //await this._vcdbUnitOfWork.SaveChangesAsync();
        }

        public Task<bool> UpdateToTransactionalTableAsync(ChangeRequestStore changeRequestStore, long changeRequestId)
        {
            return Task.Run(async () =>
            {
                long id = Convert.ToInt64(changeRequestId);

                IList<ChangeRequestItem> changeRequestItems =
                    changeRequestStore.ChangeRequestItems.Where(x => x.ChangeRequestId == id).ToList();

                foreach (ChangeRequestItem item in changeRequestItems)
                {
                    if (String.IsNullOrWhiteSpace(item.EntityFullName))
                    {
                        // note: all modelType share common namesapce
                        string namespaceName = typeof(Make).Namespace;
                        //string namespaceName = typeof(MakeBusinessService).Namespace;
                        // note: all modelType sharte common assembly
                        string assemblyFullName = typeof(Make).Assembly.FullName;
                        //string assemblyFullName = typeof(MakeBusinessService).Assembly.FullName;

                        string entityName = item.Entity;
                        //string entityName = item.Entity + "BusinessService";
                        item.EntityFullName = $"{namespaceName}.{entityName},{assemblyFullName}";
                    }

                    Type deletedEventType =
                    typeof(DeletedEvent<>).MakeGenericType(Type.GetType(item.EntityFullName));

                    var deleteEventInstance = (ChangeRequestReviewEvent)Activator.CreateInstance(deletedEventType);
                    deleteEventInstance.Entity = item.Entity;
                    deleteEventInstance.EntityId = item.EntityId;
                    deleteEventInstance.Payload = item.Payload;
                    deleteEventInstance.ChangeType = changeRequestStore.ChangeType;    //NOTE: for capturing Replace type
                    //deleteEventInstance.ChangeRequestStatus = changeRequestStore.Status;
                    deleteEventInstance.ChangeRequestId = id;

                    await _eventBus.SendAsync(Convert.ChangeType(deleteEventInstance, deletedEventType) as IEvent);
                }

                return true;
            });
        }

        private async Task<ChangeRequestStaging> UpdateChangeRequestStatusAsync(long changeRequestId)
        {
            ChangeRequestStaging changeRequestStaging =
                    await this.ChangeRequestStagingRepositoryService.FindAsync(changeRequestId);
            changeRequestStaging.Status = ChangeRequestStatus.Deleted;

            this.ChangeRequestStagingRepositoryService.Update(changeRequestStaging);
            return changeRequestStaging;
        }
    }
}
