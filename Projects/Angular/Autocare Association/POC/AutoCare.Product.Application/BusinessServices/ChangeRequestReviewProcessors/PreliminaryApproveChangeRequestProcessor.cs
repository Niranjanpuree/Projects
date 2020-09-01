using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoCare.Product.Application.RepositoryServices;
using AutoCare.Product.Infrastructure.Bus.EventBus;
using AutoCare.Product.Infrastructure.RepositoryService;
using AutoCare.Product.Infrastructure.Serializer;
using AutoCare.Product.Vcdb.Model;
using AutoCare.Product.VcdbSearch.Indexing.ApplicationService;

namespace AutoCare.Product.Application.BusinessServices.ChangeRequestReviewProcessors
{
    public abstract class PreliminaryApproveChangeRequestProcessor : ChangeRequestReviewProcessor, IPreliminaryApproveChangeRequestProcessor
    {
        private readonly IAzureFileStorageRepositoryService _azureFileStorageRepositoryService;
        private readonly ITextSerializer _serializer;
        private readonly IEventBus _eventBus;
        private readonly string _containerName;

        protected PreliminaryApproveChangeRequestProcessor(
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
        }

        public async Task<bool> ProcessAsync(long changeRequestId, string approvedBy,
            CommentsStaging commentsStaging = null, List<AttachmentsStaging> attachments = null)
        {
            long reviewCommentId = 0;
            try
            {
                reviewCommentId = await AddCommentsAsync(changeRequestId, commentsStaging, approvedBy);
                var changeRequestStaging = await UpdateChangeRequestStatusAsync(changeRequestId);
                await SaveAttachmentsAsync(attachments, changeRequestStaging);
                if (await this.Repositories.SaveChangesAsync() > 0)
                {
                    await UpdateChangeRequestStatusToChangeRequestIndexAsync(changeRequestId,
                                ChangeRequestStatus.PreliminaryApproved, reviewCommentId);
                }
            }
            catch(Exception e)
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
            string approvedBy)
        {
            long reviewCommentId = 0;
            if (commentsStaging != null)
            {
                CommentsStaging changeRequestCommentsStaging = new CommentsStaging()
                {
                    Id = 0,
                    Comment = commentsStaging.Comment,
                    ChangeRequestId = Convert.ToInt64(changeRequestId.ToString()),
                    AddedBy = approvedBy,
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

        public async Task<ChangeRequestStaging> UpdateChangeRequestStatusAsync(long changeRequestId)
        {
            ChangeRequestStaging changeRequestStaging =
                    await this.ChangeRequestStagingRepositoryService.FindAsync(changeRequestId);
            changeRequestStaging.Status = ChangeRequestStatus.PreliminaryApproved;

            this.ChangeRequestStagingRepositoryService.Update(changeRequestStaging);
            return changeRequestStaging;
        }
    }
}
