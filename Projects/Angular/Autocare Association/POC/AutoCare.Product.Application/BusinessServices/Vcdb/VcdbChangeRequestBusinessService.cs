using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoCare.Product.Application.BusinessServices.ChangeRequestReviewProcessors;
using AutoCare.Product.Application.BusinessServices.Event;
using AutoCare.Product.Application.Infrastructure.ExceptionHelpers;
using AutoCare.Product.Application.Models.BusinessModels;
using AutoCare.Product.Application.RepositoryServices;
using AutoCare.Product.Infrastructure;
using AutoCare.Product.Infrastructure.Serializer;
using AutoCare.Product.Vcdb.Model;
using AutoCare.Product.VcdbSearch.Indexing.ApplicationService;
using AutoMapper;
using AutoCare.Product.Infrastructure.ExceptionHandler;
using AutoCare.Product.VcdbSearchIndex.Model;
using AutoCare.Product.VcdbSearch.ApplicationService;

namespace AutoCare.Product.Application.BusinessServices.Vcdb
{
    public class VcdbChangeRequestBusinessService : ChangeRequestBusinessService<ChangeRequestStaging,
        ChangeRequestItemStaging, CommentsStaging, AttachmentsStaging>, IVcdbChangeRequestBusinessService
    {
        private readonly ITextSerializer _serializer;
        private readonly IVcdbUnitOfWork _vcdbUnitOfWork;
        private readonly IVcdbSqlServerEfRepositoryService<ChangeRequestStaging> _changeRequestStagingRepositoryService;
        // todo: use business service instead
        private readonly IVcdbSqlServerEfRepositoryService<ChangeRequestStore> _changeRequestStoreRepositoryService;
        private readonly IVcdbChangeRequestItemBusinessService _vcdbChangeRequestItemBusinessService;
        private readonly IVcdbChangeRequestCommentsBusinessService _vcdbChangeRequestCommentsBusinessService;
        private readonly IVcdbChangeRequestAttachmentBusinessService _vcdbChangeRequestAttachmentBusinessService;
        private readonly IChangeRequestIndexingService _changeRequestIndexingService;
        private readonly IVehicleIndexingService _vehicleIndexingService = null;
        private readonly IVehicleToBrakeConfigIndexingService _vehicleToBrakeConfigIndexingService = null;
        private readonly IVehicleSearchService _vehicleSearchService = null;
        private readonly IVehicleToBrakeConfigSearchService _vehicletoBrakeConfigSearchService = null;
        private readonly IAzureFileStorageRepositoryService _azureFileStorageRepositoryService;
        private readonly IApproveChangeRequestProcessor _approveChangeRequestProcessor;
        private readonly IRejectChangeRequestProcessor _rejectChangeRequestProcessor;
        private readonly IPreliminaryApproveChangeRequestProcessor _preliminaryApproveChangeRequestProcessor;
        private readonly IDeleteChangeRequestProcessor _deleteChangeRequestProcessor;
        private readonly IVcdbSqlServerEfRepositoryService<LikesStaging> _likeStagingRepositoryService;
        private readonly IVcdbSqlServerEfRepositoryService<CommentsStaging> _CommentsStagingRepositoryService;
        private readonly IVcdbSqlServerEfRepositoryService<Comments> _CommentsRepositoryService;
        private readonly IVcdbSqlServerEfRepositoryService<ChangeRequestAssignmentStaging>
            _changeRequestAssignmentRepositoryService;

        private const string VcdbContainerName = "vcdbattachments";

        public VcdbChangeRequestBusinessService(IVcdbUnitOfWork vcdbUnitOfWork, IMapper autoMapper,
            ITextSerializer serializer,
            IVcdbChangeRequestItemBusinessService vcdbChangeRequestItemBusinessService,
            IVcdbChangeRequestCommentsBusinessService vcdbchangeRequestCommentsBusinessService,
            IVcdbChangeRequestAttachmentBusinessService vcdbChangeRequestAttachmentBusinessService,
            IChangeRequestIndexingService changeRequestIndexingService,
            IVehicleIndexingService vehicleIndexingService,
            IVehicleToBrakeConfigIndexingService vehicleToBrakeConfigIndexingService,
            IVehicleSearchService vehicleSearchService,
            IVehicleToBrakeConfigSearchService vehicletoBrakeConfigSearchService,
            IAzureFileStorageRepositoryService azureFileStorageRepositoryService,
            IVcdbApproveChangeRequestProcessor approveChangeRequestProcessor,
            IVcdbRejectChangeRequestProcessor rejectChangeRequestProcessor,
            IVcdbPreliminaryApproveChangeRequestProcessor preliminaryApproveChangeRequestProcessor,
            IVcdbDeleteChangeRequestProcessor deleteChangeRequestProcessor)
            : base(vcdbUnitOfWork, autoMapper, serializer, vcdbChangeRequestItemBusinessService,
                vcdbchangeRequestCommentsBusinessService, changeRequestIndexingService)
        {
            _serializer = serializer;
            _vcdbUnitOfWork = vcdbUnitOfWork;

            _changeRequestStagingRepositoryService =
                vcdbUnitOfWork.GetRepositoryService<ChangeRequestStaging>() as
                    IVcdbSqlServerEfRepositoryService<ChangeRequestStaging>;
            // todo: use business service instead
            _changeRequestStoreRepositoryService =
                vcdbUnitOfWork.GetRepositoryService<ChangeRequestStore>() as
                    IVcdbSqlServerEfRepositoryService<ChangeRequestStore>;

            _vcdbChangeRequestItemBusinessService = vcdbChangeRequestItemBusinessService;
            _vcdbChangeRequestCommentsBusinessService = vcdbchangeRequestCommentsBusinessService;
            _vcdbChangeRequestAttachmentBusinessService = vcdbChangeRequestAttachmentBusinessService;
            _changeRequestIndexingService = changeRequestIndexingService;
            _vehicleIndexingService = vehicleIndexingService;
            _vehicleToBrakeConfigIndexingService = vehicleToBrakeConfigIndexingService;
            _vehicleSearchService = vehicleSearchService;
            _vehicletoBrakeConfigSearchService = vehicletoBrakeConfigSearchService;
            _azureFileStorageRepositoryService = azureFileStorageRepositoryService;
            _approveChangeRequestProcessor = approveChangeRequestProcessor;
            _rejectChangeRequestProcessor = rejectChangeRequestProcessor;
            _preliminaryApproveChangeRequestProcessor = preliminaryApproveChangeRequestProcessor;
            _deleteChangeRequestProcessor = deleteChangeRequestProcessor;
            // TODO: use separate business services later
            _likeStagingRepositoryService = vcdbUnitOfWork.GetRepositoryService<LikesStaging>() as
                IVcdbSqlServerEfRepositoryService<LikesStaging>;
            _changeRequestAssignmentRepositoryService =
                vcdbUnitOfWork.GetRepositoryService<ChangeRequestAssignmentStaging>() as
                    IVcdbSqlServerEfRepositoryService<ChangeRequestAssignmentStaging>;
            _CommentsStagingRepositoryService = vcdbUnitOfWork.GetRepositoryService<CommentsStaging>() as
                IVcdbSqlServerEfRepositoryService<CommentsStaging>;
            _CommentsRepositoryService = vcdbUnitOfWork.GetRepositoryService<Comments>() as
                IVcdbSqlServerEfRepositoryService<Comments>;
        }

        public override async Task<long> SubmitAsync<TEntity, TId>(TEntity entity, TId id, string requestedBy,
            ChangeType changeType = ChangeType.None,
            List<ChangeRequestItemStaging> changeRequestItemStagings = null,
            CommentsStaging changeRequestCommentsStaging = null,
            List<AttachmentsStaging> attachmentsStaging = null,
            string changeContent = null)
        {
            // validation
            if (changeType != ChangeType.Add)
            {
                //1. Check if entity + entity-id exists in ChangeRequestStaging.
                var existingChangeRequest = await
                    this._changeRequestStagingRepositoryService.GetAsync(
                        item => item.Entity.Equals(typeof(TEntity).Name, StringComparison.CurrentCultureIgnoreCase)
                                && item.EntityId.Equals(id.ToString(), StringComparison.InvariantCultureIgnoreCase));
                if (existingChangeRequest != null && existingChangeRequest.Any())
                {
                    throw new ChangeRequestExistException(
                        $"Request already exists in Change Request with change request id {existingChangeRequest.First().Id}.");
                }

                // 2. Check if entity + entity-id exists in ChangeRequestItemStaging.
                // NOTE: This check can already be done in ChangeRequestItemBusinessServer.Make()
                var existingChangeRequestItems =
                    await this._vcdbChangeRequestItemBusinessService.GetChangeRequestItemsAsync(item =>
                        item.Entity.Equals(typeof(TEntity).Name, StringComparison.CurrentCultureIgnoreCase)
                        && item.EntityId.Equals(id.ToString()));
                if (existingChangeRequestItems != null && existingChangeRequestItems.Any())
                {
                    throw new ChangeRequestExistException(
                        $"Request already exists in Change Request item with change request id {existingChangeRequestItems.First().ChangeRequestId}.");
                }
            }

            var changeRequestStaging = new ChangeRequestStaging
            {
                ChangeRequestTypeId = default(short),
                ChangeType = changeType,
                CreatedDateTime = DateTime.UtcNow,
                Entity = typeof(TEntity).Name,
                EntityId = id.ToString(),
                RequestedBy = requestedBy,
                ChangeRequestItemStagings = new List<ChangeRequestItemStaging>(),
                CommentsStagings = new List<CommentsStaging>(),
                AttachmentsStagings = new List<AttachmentsStaging>()
            };
            if (changeRequestItemStagings != null)
            {
                foreach (ChangeRequestItemStaging changeRequestItemStaging in changeRequestItemStagings)
                {
                    //NOTE: added the EntityFullNamein Item Staging that was moved from Change Request Staging to Item Staging
                    if (String.IsNullOrEmpty(changeRequestItemStaging.EntityFullName))
                    {
                        changeRequestItemStaging.EntityFullName = typeof(TEntity).AssemblyQualifiedName;
                    }
                    changeRequestStaging.ChangeRequestItemStagings.Add(changeRequestItemStaging);
                }
            }

            // Comments Stagging
            if (changeRequestCommentsStaging != null && !string.IsNullOrEmpty(changeRequestCommentsStaging.Comment))
            {
                changeRequestStaging.CommentsStagings.Add(changeRequestCommentsStaging);
            }
        
            this._changeRequestStagingRepositoryService.Add(changeRequestStaging);

            if (await this._vcdbUnitOfWork.SaveChangesAsync() > 0)
            {
                // upload to azure search
                var document = new ChangeRequestDocument()
                {
                    ChangeRequestId = changeRequestStaging.Id.ToString(),
                    ChangeType = changeType.ToString(),
                    ChangeRequestTypeId = changeRequestStaging.ChangeRequestTypeId,
                    RequestedBy = changeRequestStaging.RequestedBy,
                    Status = (short)changeRequestStaging.Status,
                    StatusText = changeRequestStaging.Status.ToString(),
                    SubmittedDate = changeRequestStaging.CreatedDateTime,
                    UpdatedDate = changeRequestStaging.CreatedDateTime,
                    Likes = default(int),
                    Source = "",
                    Entity = changeRequestStaging.Entity,
                    ChangeContent = changeContent
                };
                //upload if change request have comments or not to azure
                int commentCount = _CommentsStagingRepositoryService.GetCountAsync(x => x.ChangeRequestId == changeRequestStaging.Id);
                if (commentCount > 0)
                {
                    document.CommentExists = true;
                }
                else
                {
                    document.CommentExists = false;
                }
                await this._changeRequestIndexingService.UploadDocumentAsync(document);
            }
            await SaveAttachments(attachmentsStaging, changeRequestStaging);
            await this._vcdbUnitOfWork.SaveChangesAsync();
          
            return changeRequestStaging.Id;
        }

        private async Task SaveAttachments(List<AttachmentsStaging> attachmentsStaging,
            ChangeRequestStaging changeRequestStaging)
        {
            if (attachmentsStaging != null && attachmentsStaging.Count > 0)
            {
                var folderPath = GetFolderPath(changeRequestStaging.Id);
                foreach (var attachment in attachmentsStaging)
                {
                    //Need to be tested
                    if (attachment.FileStatus == FileStatus.Deleted)
                    {
                        // todo: get directory path.

                        await
                            _azureFileStorageRepositoryService.DeleteAsync(attachment.AzureContainerName,
                                attachment.FileName, attachment.DirectoryPath);
                        continue;
                    }
                    await SaveFileToTempLocation(attachment);
                }

                changeRequestStaging.AttachmentsStagings = changeRequestStaging.AttachmentsStagings ??
                                                           new List<AttachmentsStaging>();
                foreach (var attachment in attachmentsStaging)
                {
                    //Need to be tested
                    if (attachment.FileStatus == FileStatus.Deleted)
                    {
                        if (attachment.AttachmentId > 0)
                        {
                            // note: remove from staging table.
                            await this._vcdbChangeRequestAttachmentBusinessService.RemoveAsync(attachment.AttachmentId);
                        }
                        continue;
                    }

                    var newUri = await
                        MoveFileFromTempLocationToPermanent(attachment.AzureContainerName, VcdbContainerName,
                            attachment.FileName, folderPath);
                    attachment.DirectoryPath = folderPath;
                    attachment.AzureContainerName = VcdbContainerName;
                    attachment.ChangeRequestId = changeRequestStaging.Id;

                    this._vcdbChangeRequestAttachmentBusinessService.Add(attachment);
                }
            }

            await this._vcdbChangeRequestAttachmentBusinessService.SaveChangesAsync();
            // note: saveChangesAsync() will be performed on repective method call.
            //await this._vcdbUnitOfWork.SaveChangesAsync();
        }

        private async Task<Uri> MoveFileFromTempLocationToPermanent(string sourceContainer, string targetContainer,
            string sourceBlobName,
            string targetDirectoryPath)
        {
            return await _azureFileStorageRepositoryService.MoveFileAsync(sourceContainer, targetContainer,
                sourceBlobName, targetDirectoryPath);
        }

        private async Task SaveFileToTempLocation(AttachmentsStaging attachment)
        {
            await _azureFileStorageRepositoryService.SaveFileAsync(new AzureFileModel
            {
                BlobName = attachment.FileName,
                ContainerName = attachment.AzureContainerName,
                ContentType = attachment.ContentType,
                ChunkIdList = attachment.BlocksIdList
            });
        }

        private string GetFolderPath(long key, int maxFiles = 1000, int depth = 2)
        {
            var parts = new List<string>();
            long current = key;

            for (int i = depth; i >= 0; i--)
            {
                long q = Convert.ToInt64(Math.Pow(maxFiles, i));
                long level = current / q;

                parts.Add($"{level:0000}");

                current = current % q;
            }

            //parts.Add(string.Format("{0:0000}{1}", current, extension));

            string separator = Path.DirectorySeparatorChar.ToString(CultureInfo.InvariantCulture);
            string path = string.Join(separator, parts);

            return path;
        }

        public override async Task<bool> PreliminaryApproveAsync<TId>(TId changeRequestId, string approvedBy,
            CommentsStaging commentsStaging = null, List<AttachmentsStaging> attachments = null)
        {
            return
                await
                    _preliminaryApproveChangeRequestProcessor.ProcessAsync(Convert.ToInt64(changeRequestId), approvedBy,
                        commentsStaging, attachments);
            //long reviewCommentId = 0;

            //// add comments staging
            //if (commentsStaging != null)
            //{
            //    CommentsStaging changeRequestCommentsStaging = new CommentsStaging()
            //    {
            //        Id = 0,
            //        Comment = commentsStaging.Comment,
            //        ChangeRequestId = Convert.ToInt64(changeRequestId.ToString()),
            //        AddedBy = approvedBy,
            //        CreatedDatetime = DateTime.UtcNow
            //    };
            //    this._vcdbChangeRequestCommentsBusinessService.Add(changeRequestCommentsStaging);

            //    if (await this._vcdbChangeRequestCommentsBusinessService.SaveChangesAsync() > 0)
            //    {
            //        reviewCommentId = changeRequestCommentsStaging.Id;
            //    }
            //    else
            //    {
            //        return false;
            //    }
            //}

            //try
            //{
            //    // update change request staging to preliminary approve status
            //    ChangeRequestStaging changeRequestStaging =
            //        await this._changeRequestStagingRepositoryService.FindAsync(changeRequestId);
            //    changeRequestStaging.Status = ChangeRequestStatus.PreliminaryApproved;

            //    this._changeRequestStagingRepositoryService.Update(changeRequestStaging);

            //    await SaveAttachments(attachments, changeRequestStaging);

            //    // todo: implement who was responsible for approval

            //    return await this._vcdbUnitOfWork.SaveChangesAsync() > 0;
            //}
            //catch
            //{
            //    // remove review comment
            //    if (reviewCommentId > 0)
            //    {
            //        await this._vcdbChangeRequestCommentsBusinessService.RemoveAsync(reviewCommentId);
            //        await this._vcdbChangeRequestCommentsBusinessService.SaveChangesAsync();
            //    }

            //    // throw exception
            //    throw;
            //}
        }

        public override async Task<bool> SubmitAsync<TKey>(TKey changeRequestId, string submittedBy,
            CommentsStaging commentsStaging = null,
            List<AttachmentsStaging> attachments = null)
        {
            long reviewCommentId = 0;

            // add comments staging
            if (commentsStaging != null)
            {
                CommentsStaging changeRequestCommentsStaging = new CommentsStaging()
                {
                    Id = 0,
                    Comment = commentsStaging.Comment,
                    ChangeRequestId = Convert.ToInt64(changeRequestId.ToString()),
                    AddedBy = submittedBy,
                    CreatedDatetime = DateTime.UtcNow
                };
                this._vcdbChangeRequestCommentsBusinessService.Add(changeRequestCommentsStaging);

                if (await this._vcdbChangeRequestCommentsBusinessService.SaveChangesAsync() > 0)
                {
                    reviewCommentId = changeRequestCommentsStaging.Id;
                    if (reviewCommentId > 0)
                    {
                        var document = new ChangeRequestDocument()
                        {
                            ChangeRequestId = changeRequestId.ToString(),
                            CommentExists = true
                        };

                        await this._changeRequestIndexingService.UpdateDocumentAsync(document);
                    }
                }
                else
                {
                    return false;
                }
            }

            try
            {
                // update change request staging to preliminary approve status
                ChangeRequestStaging changeRequestStaging =
                    await this._changeRequestStagingRepositoryService.FindAsync(changeRequestId);
                changeRequestStaging.Status = ChangeRequestStatus.Submitted;

                this._changeRequestStagingRepositoryService.Update(changeRequestStaging);

                await SaveAttachments(attachments, changeRequestStaging);

                // todo: implement who was responsible for approval

                return await this._vcdbUnitOfWork.SaveChangesAsync() > 0;
            }
            catch
            {
                // remove review comment
                if (reviewCommentId > 0)
                {
                    await this._vcdbChangeRequestCommentsBusinessService.RemoveAsync(reviewCommentId);
                    await this._vcdbChangeRequestCommentsBusinessService.SaveChangesAsync();
                }

                // throw exception
                throw;
            }
        }

        public override async Task<bool> ApproveAsync<TKey>(TKey changeRequestId, string approvedBy,
            CommentsStaging commentsStaging = null, List<AttachmentsStaging> attachments = null)
        {
            return
                await
                    _approveChangeRequestProcessor.ProcessAsync(Convert.ToInt64(changeRequestId), approvedBy,
                        commentsStaging, attachments);

            //long reviewCommentId = 0;

            //// add comments staging
            //if (commentsStaging != null)
            //{
            //    CommentsStaging changeRequestCommentsStaging = new CommentsStaging()
            //    {
            //        Id = 0,
            //        Comment = commentsStaging.Comment,
            //        ChangeRequestId = Convert.ToInt64(changeRequestId.ToString()),
            //        AddedBy = approvedBy,
            //        CreatedDatetime = DateTime.UtcNow
            //    };
            //    this._vcdbChangeRequestCommentsBusinessService.Add(changeRequestCommentsStaging);

            //    if (await this._vcdbChangeRequestCommentsBusinessService.SaveChangesAsync() > 0)
            //    {
            //        reviewCommentId = changeRequestCommentsStaging.Id;
            //    }
            //    else
            //    {
            //        return false;
            //    }
            //}

            //try
            //{
            //    // update change request staging to approve status
            //    ChangeRequestStaging changeRequestStaging =
            //        await this._changeRequestStagingRepositoryService.FindAsync(changeRequestId);
            //    changeRequestStaging.Status = ChangeRequestStatus.Approved;

            //    this._changeRequestStagingRepositoryService.Update(changeRequestStaging);

            //    await SaveAttachments(attachments, changeRequestStaging);

            //    // todo: implement who was responsible for approval

            //    // copy staging to store
            //    ChangeRequestStore changeRequestStore =
            //        await this.AddToChangeRequestStore(changeRequestId, ChangeRequestStatus.Approved, approvedBy);
            //    // remove staging
            //    ChangeRequestStaging changeRequestStagingReviewed =
            //        await this.RemoveFromChangeRequestStagingAsync(changeRequestId);

            //    if (changeRequestStore != null && changeRequestStagingReviewed != null)
            //    {
            //        // 1. Transaction table: Update change request id to NULL
            //        // 2. IF NEW: Insert record into transaction table (note: primary key is custom generated). 
            //        // 2. IF UPDATE: Update record at transaction table.
            //        // 2. IF DELETE: Soft Delete record at transaction table.
            //        // 2. IF REPLACE: TODO TODO TODO.
            //        // todo: 3. TODO TODO TODO log to history table. TODO TODO TODO
            //        if (this.UpdateToTransactionalTable(changeRequestStore, changeRequestId))
            //        {
            //            var isSaved = await this._vcdbUnitOfWork.SaveChangesAsync() > 0;

            //            return isSaved;
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    // remove review comment
            //    if (reviewCommentId > 0)
            //    {
            //        await this._vcdbChangeRequestCommentsBusinessService.RemoveAsync(reviewCommentId);
            //        await this._vcdbChangeRequestCommentsBusinessService.SaveChangesAsync();
            //    }

            //    // throw exception
            //    throw ex;
            //}

            //return false;
        }

        public override async Task<bool> RejectAsync<TKey>(TKey changeRequestId, string rejectedBy,
            CommentsStaging commentsStaging = null, List<AttachmentsStaging> attachments = null)
        {
            return
                await
                    _rejectChangeRequestProcessor.ProcessAsync(Convert.ToInt64(changeRequestId), rejectedBy,
                        commentsStaging, attachments);

            //long reviewCommentId = 0;

            //// add comments staging
            //if (commentsStaging != null)
            //{
            //    CommentsStaging changeRequestCommentsStaging = new CommentsStaging()
            //    {
            //        Id = 0,
            //        Comment = commentsStaging.Comment,
            //        ChangeRequestId = Convert.ToInt64(changeRequestId.ToString()),
            //        AddedBy = rejectedBy,
            //        CreatedDatetime = DateTime.UtcNow
            //    };
            //    this._vcdbChangeRequestCommentsBusinessService.Add(changeRequestCommentsStaging);

            //    if (await this._vcdbChangeRequestCommentsBusinessService.SaveChangesAsync() > 0)
            //    {
            //        reviewCommentId = changeRequestCommentsStaging.Id;
            //    }
            //    else
            //    {
            //        return false;
            //    }
            //}

            //try
            //{
            //    // update change request staging to reject status
            //    ChangeRequestStaging changeRequestStaging =
            //        await this._changeRequestStagingRepositoryService.FindAsync(changeRequestId);
            //    changeRequestStaging.Status = ChangeRequestStatus.Rejected;

            //    this._changeRequestStagingRepositoryService.Update(changeRequestStaging);

            //    await SaveAttachments(attachments, changeRequestStaging);

            //    // todo: implement who was responsible for approval

            //    // copy staging to store
            //    ChangeRequestStore changeRequestStore =
            //        await this.AddToChangeRequestStore(changeRequestId, ChangeRequestStatus.Rejected, rejectedBy);
            //    // remove staging

            //    ChangeRequestStaging changeRequestStagingReviewed =
            //        await this.RemoveFromChangeRequestStagingAsync(changeRequestId);

            //    if (changeRequestStore != null && changeRequestStagingReviewed != null)
            //    {
            //        // todo: 1. Transaction table: Update change request id = null 
            //        if (this.UpdateToTransactionalTable(changeRequestStore, changeRequestId))
            //        {
            //            return await this._vcdbUnitOfWork.SaveChangesAsync() > 0;
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    // remove review comment
            //    if (reviewCommentId > 0)
            //    {
            //        await this._vcdbChangeRequestCommentsBusinessService.RemoveAsync(reviewCommentId);
            //        await this._vcdbChangeRequestCommentsBusinessService.SaveChangesAsync();
            //    }

            //    // throw exception
            //    throw ex;
            //}

            //return false;
        }

        public override async Task<bool> SubmitReviewAsync<TId>(TId changeRequestId, string reviewedBy,
            ChangeRequestStatus reviewStatus,
            CommentsStaging commentsStaging = null, List<AttachmentsStaging> attachments = null)
        {
            bool result = false;
            // check review status
            switch (reviewStatus)
            {
                case ChangeRequestStatus.Submitted: // never executed
                    result = await this.SubmitAsync(changeRequestId, reviewedBy, commentsStaging, attachments);
                    break;
                case ChangeRequestStatus.Deleted: // if review status .equals cancel
                    result = await this.DeleteAsync(changeRequestId, reviewedBy, commentsStaging, attachments);
                    break;
                case ChangeRequestStatus.PreliminaryApproved: // if review status .equals preliminary approve
                    result =
                        await this.PreliminaryApproveAsync(changeRequestId, reviewedBy, commentsStaging, attachments);
                    break;
                case ChangeRequestStatus.Approved: // if review status .equals approve
                    result = await this.ApproveAsync(changeRequestId, reviewedBy, commentsStaging, attachments);
                    break;
                case ChangeRequestStatus.Rejected: // if review status .equals reject
                    result = await this.RejectAsync(changeRequestId, reviewedBy, commentsStaging, attachments);
                    break;
            }

            //if (result)
            //{
            //    // upload to azure search
            //    var document = new ChangeRequestDocument()
            //    {
            //        ChangeRequestId = changeRequestId.ToString(),
            //        Status = (short)reviewStatus,
            //        UpdatedDate = DateTime.UtcNow
            //    };

            //    await this._changeRequestIndexingService.UpdateDocumentAsync(document);
            //}

            return result;
        }

        public override async Task<List<ChangeRequestItemStaging>> GetChangeRequestItemStagingsAsync(
            Expression<Func<ChangeRequestItemStaging, bool>> whereCondition, int topCount = 0)
        {
            return await this._vcdbChangeRequestItemBusinessService.GetChangeRequestItemsAsync(whereCondition, topCount);
        }

        public override async Task<List<ChangeRequestStaging>> GetChangeRequestsAsync(
            Expression<Func<ChangeRequestStaging, bool>> whereCondition, int topCount = 0)
        {
            return await this._changeRequestStagingRepositoryService
                .GetAsync(whereCondition, topCount, "ChangeRequestItemStagings", "CommentsStagings");
        }

        public override async Task<bool> DeleteAsync<TKey>(TKey changeRequestId, string deletedBy
            , CommentsStaging commentsStaging = null, List<AttachmentsStaging> attachments = null)
        {
            return
                await
                    _deleteChangeRequestProcessor.ProcessAsync(Convert.ToInt64(changeRequestId), deletedBy,
                        commentsStaging, attachments);
        }

        public override async Task<ChangeRequestStagingModel<TEntity>> GetChangeRequestStagingByChangeRequestIdAsync
            <TEntity, TId>(TId changeRequestId)
        {
            long id = Convert.ToInt64(changeRequestId);

            // validation
            //TODO: this.ChangeRequestRepositoryService.FindAsync(TId) can be sufficient
            var changeRequestStagings = await this._changeRequestStagingRepositoryService.GetAsync(item =>
                item.Id.Equals(id) && item.Entity.Equals(typeof(TEntity).Name));
            if (changeRequestStagings == null || changeRequestStagings.Count == 0)
            {
                // note: if no records in staging the get from store.
                var changeRequestStore =
                    await this.GetChangeRequestStoreByChangeRequestIdAsync<TEntity, TId>(changeRequestId);
                if (changeRequestStore == null)
                {
                    // if no records in staging + store, then records doesn't exists.
                    throw new NoRecordFound(
                        $"There are no {typeof(ChangeRequestStaging).Name} with Id : {id.ToString()} for {typeof(TEntity).Name}.");
                }
                else
                {
                    return changeRequestStore;
                }
            }
            else
            {
                ChangeRequestStaging changeRequestStaging = changeRequestStagings.First();

                // get change-request-item-staging
                var changeRequestItems =
                    await this._vcdbChangeRequestItemBusinessService.GetChangeRequestItemsAsync(item =>
                        item.ChangeRequestId == changeRequestStaging.Id
                        && item.Entity.Equals(changeRequestStaging.Entity)
                        //TODO: in case of replace base vehicle, changeRequestStaging.Entity="BaseVehicle", and item.Entity="Vehicle"
                        && item.EntityId.Equals(changeRequestStaging.EntityId));

                ChangeRequestItemStaging changeRequestItem = null;
                TEntity itemStaging = null;
                TEntity existingItem = null;

                if (changeRequestItems != null && changeRequestItems.Count > 0)
                {
                    changeRequestItem = changeRequestItems.First();
                    itemStaging = this._serializer.Deserialize<TEntity>(changeRequestItem.Payload);

                    // get existing payload as well from item staging.
                    if (!String.IsNullOrWhiteSpace(changeRequestItem.ExistingPayload))
                    {
                        existingItem = this._serializer.Deserialize<TEntity>(changeRequestItem.ExistingPayload);
                    }
                }

                // get comments
                IList<CommentsStaging> commentsStagings =
                    await this._vcdbChangeRequestCommentsBusinessService.GetChangeRequestCommentsAsync(item =>
                        item.ChangeRequestId == changeRequestStaging.Id);
                IList<CommentsStagingModel> comments = null;
                if (commentsStagings.Any())
                {
                    comments = commentsStagings.Select(item => new CommentsStagingModel()
                    {
                        Comment = item?.Comment,
                        AddedBy = item?.AddedBy,
                        CreatedDatetime = item?.CreatedDatetime
                    }).OrderByDescending(x=>x.CreatedDatetime).ToList();
                }

                //All comments merged into one
                // get requestor comment
                //IList<CommentsStaging> commentsStagingsRequestor =
                //    commentsStagings.Where(item => item.AddedBy.Equals(changeRequestStaging.RequestedBy)).ToList();
                //IList<CommentsStagingModel> commentsRequestor = null;
                //if (commentsStagingsRequestor.Any())
                //{
                //    commentsRequestor = commentsStagingsRequestor.Select(item => new CommentsStagingModel()
                //    {
                //        Comment = item?.Comment,
                //        AddedBy = item?.AddedBy,
                //        CreatedDatetime = item?.CreatedDatetime
                //    }).ToList();
                //}

                // get reviewer comment
                //IList<CommentsStaging> commentStagingsReviewer =
                //    commentsStagings.Where(item => !item.AddedBy.Equals(changeRequestStaging.RequestedBy)).ToList();
                //IList<CommentsStagingModel> commentsReviewer = null;
                //if (commentStagingsReviewer.Any())
                //{
                //    commentsReviewer = commentStagingsReviewer.Select(item => new CommentsStagingModel()
                //    {
                //        Comment = item?.Comment,
                //        AddedBy = item?.AddedBy,
                //        CreatedDatetime = item?.CreatedDatetime
                //    }).ToList();
                //}
                // get reviewer attachment
                IList<AttachmentsModel> attachments = null;
                var attachmentStagings =
                    await this._vcdbChangeRequestAttachmentBusinessService.GetChangeRequestAttachmentsAsync(item =>
                        item.ChangeRequestId == changeRequestStaging.Id);

                if (attachmentStagings != null && attachmentStagings.Any())
                {
                    attachments = attachmentStagings.Select(item => new AttachmentsModel
                    {
                        AttachmentId = item.AttachmentId,
                        AttachedBy = item.AttachedBy,
                        ContainerName = item.AzureContainerName,
                        //ChunksIdList = item.
                        ContentType = item.ContentType,
                        CreatedDatetime = item.CreatedDateTime,
                        FileExtension = item.FileExtension,
                        FileName = item.FileName,
                        DirectoryPath = item.DirectoryPath,
                        FileUri =
                            _azureFileStorageRepositoryService.GetReadOnlyFileUrl(item.AzureContainerName, item.FileName,
                                item.DirectoryPath).AbsoluteUri,
                        FileSize = item.FileSize
                    }).ToList();
                }

                // convert to change request business model
                ChangeRequestStagingReviewModel stagingReviewModel = new ChangeRequestStagingReviewModel()
                {
                    ChangeRequestId = changeRequestStaging.Id,
                    ChangeType = changeRequestStaging.ChangeType.ToString(),
                    Status = changeRequestStaging.Status.ToString(),
                    SubmittedBy = changeRequestStaging.RequestedBy,
                    CreatedDateTime = changeRequestStaging.CreatedDateTime,
                    EntityName = changeRequestStaging.Entity,
                    EntityId = changeRequestStaging.EntityId
                };

                ChangeRequestStagingModel<TEntity> changeRequestStagingModel = new ChangeRequestStagingModel<TEntity>()
                {
                    StagingItem = stagingReviewModel,
                    EntityStaging = itemStaging,
                    //EntityCurrent = makeCurrent,
                    EntityCurrent = existingItem, // fill existing entity
                    //RequestorComments = commentsRequestor,
                    //ReviewerComments = commentsReviewer,
                    Comments = comments,
                    Attachments = attachments
                };

                return changeRequestStagingModel;
            }
        }

        public override async Task<List<ChangeEntityFacet>> GetAllChangeTypes()
        {
            var changeRequests = (await GetAllAsync()).GroupBy(x => x.Entity)
                .Select(t => t.First()).ToArray();
            List<string> entityList = changeRequests.Select(changeRequestStaging => changeRequestStaging.Entity).ToList();
            var changeRequestStores = (await _changeRequestStoreRepositoryService.GetAllAsync()).GroupBy(x => x.Entity)
                .Select(t => t.First()).ToArray();

            var distinctEntityTypes = (from o in changeRequestStores
                                       where !(from k in changeRequests select k.Entity).Contains((o.Entity))
                                       select o).ToList();
            if (distinctEntityTypes != null && distinctEntityTypes.Any())
            {
                entityList.AddRange(distinctEntityTypes.Select(distinctEntityType => distinctEntityType.Entity));
            }
            
            List<ChangeEntityFacet> changeTypes = new List<ChangeEntityFacet>();
            
            changeTypes.AddRange(entityList.Select(entity => new ChangeEntityFacet()
            {
                Name = entity
            }));

            return changeTypes;
        }

        public override async Task<ChangeRequestStagingModel<TEntity>> GetChangeRequestStoreByChangeRequestIdAsync
            <TEntity, TId>
            (TId changeRequestId)
        {
            long id = Convert.ToInt64(changeRequestId);
            // todo: create and use busines-sservice instead of directly accessing repository-service.
            IVcdbSqlServerEfRepositoryService<ChangeRequestStore> changeRequestStoreRepositoryService =
                this._vcdbUnitOfWork.GetRepositoryService<ChangeRequestStore>() as
                    IVcdbSqlServerEfRepositoryService<ChangeRequestStore>;
            IVcdbSqlServerEfRepositoryService<ChangeRequestItem> changeRequestItemRepositoryService =
                this._vcdbUnitOfWork.GetRepositoryService<ChangeRequestItem>() as
                    IVcdbSqlServerEfRepositoryService<ChangeRequestItem>;
            IVcdbSqlServerEfRepositoryService<Comments> changeRequestCommentsRepositoryService =
                this._vcdbUnitOfWork.GetRepositoryService<Comments>() as
                    IVcdbSqlServerEfRepositoryService<Comments>;
            IVcdbSqlServerEfRepositoryService<Attachments> changeRequestAttachmentsRepositoryService =
                this._vcdbUnitOfWork.GetRepositoryService<Attachments>() as
                    IVcdbSqlServerEfRepositoryService<Attachments>;

            if (changeRequestStoreRepositoryService == null) return null;

            // validation
            var changeRequestStore = await changeRequestStoreRepositoryService.GetAsync(item =>
                item.Id.Equals(id) && item.Entity.Equals(typeof(TEntity).Name));
            if (changeRequestStore == null || changeRequestStore.Count == 0)
            {
                throw new NoRecordFound(
                    $"There are no {typeof(ChangeRequestStore).Name} with Id : {id.ToString()} for {typeof(TEntity).Name}.");
            }
            ChangeRequestStore changeRequestStaging = changeRequestStore.First();

            // get change-request-item-staging
            if (changeRequestItemRepositoryService == null) return null;

            var changeRequestItems =
                await changeRequestItemRepositoryService.GetAsync(item =>
                    item.ChangeRequestId == changeRequestStaging.Id
                    && item.Entity.Equals(changeRequestStaging.Entity)
                    && item.EntityId.Equals(changeRequestStaging.EntityId));

            //if (changeRequestItems == null || changeRequestItems.Count == 0)
            //{
            //    throw new NoRecordFound($"There are no {typeof(ChangeRequestItem).Name} with Change Request Id : {id.ToString()} for {typeof(TEntity).Name}.");
            //}

            ChangeRequestItem changeRequestItem = null;
            TEntity makeStaging = null;
            TEntity existingItem = null;

            // de-serialize payload
            if (changeRequestItems != null && changeRequestItems.Count > 0)
            {
                changeRequestItem = changeRequestItems.First();
                makeStaging = this._serializer.Deserialize<TEntity>(changeRequestItem.Payload);

                if (!String.IsNullOrWhiteSpace(changeRequestItem.ExistingPayload))
                {
                    existingItem = this._serializer.Deserialize<TEntity>(changeRequestItem.ExistingPayload);
                }
            }

            // get comments
            if (changeRequestCommentsRepositoryService == null) return null;

            List<Comments> commentsStagings = await changeRequestCommentsRepositoryService.GetAsync(item =>
                item.ChangeRequestId == changeRequestStaging.Id);

            IList<CommentsStagingModel> comments = null;
            if (commentsStagings.Any())
            {
                comments = commentsStagings.Select(item => new CommentsStagingModel()
                {
                    Comment = item?.Comment,
                    AddedBy = item?.AddedBy,
                    CreatedDatetime = item?.CreatedDatetime
                }).OrderByDescending(x => x.CreatedDatetime).ToList();
            }

            // get requestor comment
            //IList<Comments> commentsStoreRequestor =
            //    commentsStagings.Where(item => item.AddedBy.Equals(changeRequestStaging.RequestedBy)).ToList();
            //IList<CommentsStagingModel> commentsRequestor = null;
            //if (commentsStoreRequestor.Any())
            //{
            //    commentsRequestor = commentsStoreRequestor.Select(item => new CommentsStagingModel()
            //    {
            //        Comment = item?.Comment,
            //        AddedBy = item?.AddedBy,
            //        CreatedDatetime = item?.CreatedDatetime
            //    }).ToList();
            //}

            // get reviewer comment
            //IList<Comments> commentStoreReviewer =
            //    commentsStagings.Where(item => !item.AddedBy.Equals(changeRequestStaging.RequestedBy)).ToList();
            //IList<CommentsStagingModel> commentsReviewer = null;
            //if (commentStoreReviewer.Any())
            //{
            //    commentsReviewer = commentStoreReviewer.Select(item => new CommentsStagingModel()
            //    {
            //        Comment = item?.Comment,
            //        AddedBy = item?.AddedBy,
            //        CreatedDatetime = item?.CreatedDatetime
            //    }).ToList();
            //}

            // get attachments
            if (changeRequestAttachmentsRepositoryService == null) return null;

            IList<AttachmentsModel> attachments = null;
            var attachmentStores = await changeRequestAttachmentsRepositoryService.GetAsync(item =>
                item.ChangeRequestId == changeRequestStaging.Id);

            if (attachmentStores != null && attachmentStores.Any())
            {
                attachments = attachmentStores.Select(item => new AttachmentsModel
                {
                    AttachedBy = item.AttachedBy,
                    ContainerName = item.AzureContainerName,
                    //ChunksIdList = item.
                    ContentType = item.ContentType,
                    CreatedDatetime = item.CreatedDateTime,
                    FileExtension = item.FileExtension,
                    FileName = item.FileName,
                    DirectoryPath = item.DirectoryPath,
                    FileUri =
                        _azureFileStorageRepositoryService.GetReadOnlyFileUrl(item.AzureContainerName, item.FileName,
                            item.DirectoryPath).AbsoluteUri,
                    FileSize = item.FileSize
                }).ToList();
            }

            // convert to change-request-make-model
            ChangeRequestStagingReviewModel stagingReviewModel = new ChangeRequestStagingReviewModel()
            {
                ChangeRequestId = changeRequestStaging.Id,
                ChangeType = changeRequestStaging.ChangeType.ToString(),
                Status = changeRequestStaging.Status.ToString(),
                SubmittedBy = changeRequestStaging.RequestedBy,
                CreatedDateTime = changeRequestStaging.RequestedDateTime,
                EntityName = changeRequestStaging.Entity,
                EntityId = changeRequestStaging.EntityId
            };

            ChangeRequestStagingModel<TEntity> changeRequestStagingModel = new ChangeRequestStagingModel<TEntity>()
            {
                StagingItem = stagingReviewModel,
                EntityStaging = makeStaging,
                //EntityCurrent = makeCurrent,
                EntityCurrent = existingItem, // fill existing entity
                //RequestorComments = commentsRequestor,
                //ReviewerComments = commentsReviewer,
                Comments = comments,
                Attachments = attachments
            };

            return changeRequestStagingModel;
        }

        public override async Task<AssociationCount> GetAssociatedCount(
            List<ChangeRequestStaging> selectedChangeRequestStagings)
        {
            AssociationCount associationCount = new AssociationCount();
            foreach (ChangeRequestStaging changeRequest in selectedChangeRequestStagings)
            {
                List<ChangeRequestItemStaging> changeRequestItem =
                    await _vcdbChangeRequestItemBusinessService.GetChangeRequestItemsAsync(item =>
                        item.Entity.Equals(changeRequest.Entity, StringComparison.CurrentCultureIgnoreCase)
                        && item.ChangeRequestId.Equals(changeRequest.Id), 1);
                string namespaceName = typeof(MakeBusinessService).Namespace;
                string assemblyFullName = typeof(MakeBusinessService).Assembly.FullName;
                string entityName = changeRequest.Entity + "BusinessService";
                string entity = $"{namespaceName}.{entityName},{assemblyFullName}";

                Type serviceType = Type.GetType(entity);
                if (serviceType == null)
                    return null;

                IChangeRequestReviewEventHandler associatedCountEventHandler;

                try
                {
                    associatedCountEventHandler =
                        Activator.CreateInstance(serviceType, new object[] { _vcdbUnitOfWork, this, _serializer })
                            as
                            IChangeRequestReviewEventHandler;
                }
                catch
                {
                    associatedCountEventHandler =
                        Activator.CreateInstance(serviceType, new object[] { _vcdbUnitOfWork, this, _serializer, null })
                            as
                            IChangeRequestReviewEventHandler;
                }
                if (changeRequestItem.Count > 0)
                {
                    if (associatedCountEventHandler != null)
                    {
                        var singleCount = associatedCountEventHandler.AssociatedCount(changeRequestItem[0].Payload);

                        associationCount.BaseVehicleCount += singleCount.BaseVehicleCount;
                        associationCount.VehicleCount += singleCount.VehicleCount;
                        associationCount.BrakeConfigCount += singleCount.BrakeConfigCount;
                        associationCount.VehicleToBrakeConfigCount += singleCount.VehicleToBrakeConfigCount;
                        associationCount.EngineConfigCount += singleCount.EngineConfigCount;
                        associationCount.VehicleToEngineConfigCount += singleCount.VehicleToEngineConfigCount;
                        associationCount.VehicleToBedConfigCount += singleCount.VehicleToBedConfigCount;
                        associationCount.VehicleToBodyStyleConfigCount += singleCount.VehicleToBodyStyleConfigCount;
                        associationCount.ModelCount += singleCount.ModelCount;
                        associationCount.VehicleTypeCount += singleCount.VehicleTypeCount;
                        associationCount.FrontBrakeConfigCount += singleCount.FrontBrakeConfigCount;
                        associationCount.RearBrakeConfigCount += singleCount.RearBrakeConfigCount;
                        associationCount.VehicleToMfrBodyCodeCount += singleCount.VehicleToMfrBodyCodeCount;
                        associationCount.VehicleToDriveTypeCount += singleCount.VehicleToDriveTypeCount;
                        associationCount.VehicleToWheelBaseCount += singleCount.VehicleToWheelBaseCount;
                        associationCount.VehicleToSteeringConfigCount += singleCount.VehicleToSteeringConfigCount;
                        associationCount.BodyStyleConfigCount += singleCount.BodyStyleConfigCount;
                        associationCount.BedConfigCount += singleCount.BedConfigCount;
                        associationCount.SteeringConfigCount += singleCount.SteeringConfigCount;
                    }
                }
            }
            return associationCount;
        }

        public override async Task<bool> AssignReviewer(AssignReviewerBusinessModel assignReviewerBusinessModel)
        {
            List<ChangeRequestAssignmentStaging> changeRequestAssignmentStagings =
                assignReviewerBusinessModel.ChangeRequestIds.Select(
                    changeRequest => new ChangeRequestAssignmentStaging()
                    {
                        ChangeRequestId = changeRequest,
                        AssignedByUserId = assignReviewerBusinessModel.AssignedByUserId,
                        AssignedToRoleId = assignReviewerBusinessModel.AssignedToRoleId,
                        AssignedToUserId = assignReviewerBusinessModel.AssignedToUserId,
                        Comments = "",
                        CreatedDateTime = DateTime.UtcNow
                    }).ToList();
            _changeRequestAssignmentRepositoryService.AddAll(changeRequestAssignmentStagings);
            bool status = await this._vcdbUnitOfWork.SaveChangesAsync() > 0;

            if (status)
            {
                List<ChangeRequestDocument> documents =
                    assignReviewerBusinessModel.ChangeRequestIds.Select(
                        changeRequest => new ChangeRequestDocument()
                        {
                            ChangeRequestId = changeRequest.ToString(),
                            Assignee =
                                assignReviewerBusinessModel.AssignedToUserId ??
                                assignReviewerBusinessModel.AssignedToRoleId.ToString()
                        }).ToList();

                await this._changeRequestIndexingService.UpdateDocumentAsync(documents);
            }
            return status;
        }

        public async Task<List<ChangeRequestItem>> GetChangeRequestItemAsync(Expression<Func<ChangeRequestItem, bool>> whereCondition, int topCount = 0)
        {
            return
                await this._vcdbUnitOfWork.GetRepositoryService<ChangeRequestItem>().GetAsync(whereCondition, topCount);
        }

        private bool UpdateToTransactionalTable<TKey>(ChangeRequestStore changeRequestStore, TKey changeRequestId)
        {
            bool isSuccess = false;

            long id = Convert.ToInt64(changeRequestId);

            IList<ChangeRequestItem> changeRequestItems =
                changeRequestStore.ChangeRequestItems.Where(x => x.ChangeRequestId == id).ToList();

            foreach (ChangeRequestItem item in changeRequestItems)
            {
                // note: all modelType share common namesapce
                string namespaceName = typeof(Make).Namespace;
                //string namespaceName = typeof(MakeBusinessService).Namespace;
                // note: all modelType sharte common assembly
                string assemblyFullName = typeof(Make).Assembly.FullName;
                //string assemblyFullName = typeof(MakeBusinessService).Assembly.FullName;

                string entityName = item.Entity;
                //string entityName = item.Entity + "BusinessService";
                string entity = $"{namespaceName}.{entityName},{assemblyFullName}";

                //Push changes to transaction table
                IChangeRequestReviewEventHandler changeRequestReviewEventHandler =
                    Utility.GetGenericTypeInstance(typeof(VcdbBusinessService<>),
                        new Type[] { Type.GetType(entity) },
                        new object[] { _vcdbUnitOfWork, this, _serializer }) as IChangeRequestReviewEventHandler;

                // ALTERNATIVE: get instance of modelType-Business Service
                //Type serviceType = Type.GetType(entity);
                //if (serviceType == null)
                //    return isSuccess;
                //IChangeRequestReviewEventHandler changeRequestApprovedEventHandler =
                //    Activator.CreateInstance(serviceType, new object[] {_vcdbUnitOfWork, this, _serializer}) as
                //        IChangeRequestReviewEventHandler;

                if (changeRequestReviewEventHandler == null)
                {
                    return false;
                }

                isSuccess = changeRequestReviewEventHandler.Handle(new ChangeRequestApprovedEvent
                {
                    Entity = item.Entity,
                    EntityId = item.EntityId,
                    Payload = item.Payload,
                    ChangeType = item.ChangeType,
                    ChangeRequestStatus = changeRequestStore.Status,
                    ChangeRequestId = id
                });
            }

            return isSuccess;
        }

        private async Task<ChangeRequestStore> AddToChangeRequestStore<TKey>(TKey changeRequestId,
            ChangeRequestStatus status, string decisionBy)
        {
            try
            {
                long id = Convert.ToInt64(changeRequestId);

                ChangeRequestStaging staging = await this._changeRequestStagingRepositoryService.FindAsync(id);
                List<ChangeRequestItemStaging> items =
                    await _vcdbChangeRequestItemBusinessService.GetChangeRequestItemsAsync(
                        item => item.ChangeRequestId == id);
                List<CommentsStaging> comments =
                    await _vcdbChangeRequestCommentsBusinessService.GetChangeRequestCommentsAsync(
                        item => item.ChangeRequestId == id);
                List<AttachmentsStaging> attachments =
                    await _vcdbChangeRequestAttachmentBusinessService.GetChangeRequestAttachmentsAsync(
                        item => item.ChangeRequestId == id);
                List<ChangeRequestAssignmentStaging> assignments =
                    await _changeRequestAssignmentRepositoryService.GetAsync(
                        item => item.ChangeRequestId == id);

                staging.ChangeRequestItemStagings = items;
                staging.CommentsStagings = comments;
                staging.AttachmentsStagings = attachments;
                staging.ChangeRequestAssignmentStagings = assignments;

                // todo: use automapper for map ChangeRequestStaging to ChangeRequesStore
                ChangeRequestStore changeRequestStoreItem = this.MapToChangeRequestStore(staging);
                changeRequestStoreItem.DecisionBy = decisionBy;
                changeRequestStoreItem.Status = status;
                changeRequestStoreItem.CompletedDateTime = DateTime.UtcNow;

                this._changeRequestStoreRepositoryService.Add(changeRequestStoreItem);
                return changeRequestStoreItem;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task<ChangeRequestStaging> RemoveFromChangeRequestStagingAsync<TKey>(TKey changeRequestId)
        {
            return await this._changeRequestStagingRepositoryService.RemoveAsync(changeRequestId);
        }

        private ChangeRequestStore MapToChangeRequestStore(ChangeRequestStaging staging)
        {
            ChangeRequestStore store = new ChangeRequestStore()
            {
                Id = staging.Id,
                EntityId = staging.EntityId,
                ChangeRequestTypeId = staging.ChangeRequestTypeId,
                TaskControllerId = staging.TaskControllerId,
                Entity = staging.Entity,
                ChangeType = staging.ChangeType,
                Status = staging.Status,
                RequestedBy = staging.RequestedBy,
                RequestedDateTime = staging.CreatedDateTime // // note: requested on store equals created on staging
            };

            // map item
            List<ChangeRequestItem> storeItems = staging.ChangeRequestItemStagings?.Select(item
                => new ChangeRequestItem()
                {
                    ChangeRequestItemId = item.Id,
                    ChangeRequestId = item.ChangeRequestId,
                    EntityId = item.EntityId,
                    Entity = item.Entity,
                    Payload = item.Payload,
                    EntityFullName = String.Empty, //temporary fix since it is required field
                    ChangeType = item.ChangeType,
                    CreatedDateTime = item.CreatedDateTime
                }).ToList();
            // map comment
            List<Comments> comments = staging.CommentsStagings?.Select(item
                => new Comments()
                {
                    Id = item.Id,
                    ChangeRequestId = item.ChangeRequestId,
                    Comment = item.Comment,
                    AddedBy = item.AddedBy,
                    CreatedDatetime = item.CreatedDatetime
                }).ToList();
            // map attachment
            List<Attachments> attachments = staging.AttachmentsStagings?.Select(item
                => new Attachments()
                {
                    AttachmentId = item.AttachmentId,
                    ChangeRequestId = item.ChangeRequestId,
                    DirectoryPath = item.DirectoryPath,
                    FileName = item.FileName,
                    FileExtension = item.FileExtension,
                    AttachedBy = item.AttachedBy,
                    CreatedDateTime = item.CreatedDateTime,
                    AzureContainerName = item.AzureContainerName,
                    ContentType = item.ContentType,
                    FileSize = item.FileSize
                }).ToList();

            List<ChangeRequestAssignment> assignments = staging.ChangeRequestAssignmentStagings?.Select(item
                => new ChangeRequestAssignment()
                {
                    Id = item.Id,
                    ChangeRequestId = item.ChangeRequestId,
                    AssignedByUserId = item.AssignedByUserId,
                    AssignedToUserId = item.AssignedToUserId,
                    AssignedToRoleId = item.AssignedToRoleId,
                    CreatedDateTime = item.CreatedDateTime,
                    Comments = item.Comments
                }).ToList();

            // todo: map taskcontroller


            // fill up children/ details
            store.ChangeRequestItems = storeItems;
            store.Comments = comments;
            store.Attachments = attachments;
            store.ChangeRequestAssignments = assignments;
            // todo: fill taskcontoller

            return store;
        }

        public override async Task<bool> SubmitLikeAsync(long crId, string likedBy, string likedStatus)
        {
            LikesStaging like = new LikesStaging
            {
                ChangeRequestId = crId,
                LikedBy = likedBy,
                LikeStatus = (byte)(LikeStatusType)Enum.Parse(typeof(LikeStatusType), likedStatus),
                CreatedDatetime = DateTime.UtcNow,
                UpdatedDatetime = DateTime.UtcNow
            };
            if ((byte)LikeStatusType.Like == (byte)(LikeStatusType)Enum.Parse(typeof(LikeStatusType), likedStatus))
            {
                _likeStagingRepositoryService.Add(like);
            }
            else if ((byte)LikeStatusType.Unlike ==
                     (byte)(LikeStatusType)Enum.Parse(typeof(LikeStatusType), likedStatus))
            {
                //LikesStaging Original=new LikesStaging();
                //  Original=_likeStagingRepositoryService.FindAsync()           
                _likeStagingRepositoryService.Update(like);
            }
            bool status = await this._vcdbUnitOfWork.SaveChangesAsync() > 0;
            // upload to azure search
            List<LikesStaging> likesStagings =
                await _likeStagingRepositoryService.GetAsync(x => x.ChangeRequestId == crId);
            int likesCount = likesStagings.Count();
            if (status)
            {
                var document = new ChangeRequestDocument()
                {
                    ChangeRequestId = crId.ToString(),
                    Likes = likesCount
                };

                await this._changeRequestIndexingService.UpdateDocumentAsync(document);
            }
            return status;
        }

        public override async Task<List<CommentsStagingModel>> GetRequestorComments(string changeRequestId,
            string reviewStatus)
        {
            List<CommentsStagingModel> commentsStagingModels = new List<CommentsStagingModel>();
            long id = Convert.ToInt64(changeRequestId);
            ChangeRequestStatus status = (ChangeRequestStatus)Enum.Parse(typeof(ChangeRequestStatus), reviewStatus);
            if (status == ChangeRequestStatus.PreliminaryApproved || status == ChangeRequestStatus.Submitted)
            {
                //Requestor comments from CommentsStaging Table
                //var changeRequestStagings = await this._changeRequestStagingRepositoryService.GetAsync(item =>
                //     item.Id.Equals(id));
                //ChangeRequestStaging changeRequest = changeRequestStagings.First();

                //All comments from CommentsStaging table of particular change request
                var commentsStaging = await this._CommentsStagingRepositoryService.GetAsync(item =>
               item.ChangeRequestId.Equals(id));
                commentsStagingModels.AddRange(commentsStaging.Select(item => new CommentsStagingModel
                {
                    AddedBy = item.AddedBy,
                    Comment = item.Comment,
                    CreatedDatetime = item.CreatedDatetime
                }));
            }
            else
            {
                //Requestor comments from Comments Table
                //var changeRequestStores =
                //    await this._changeRequestStoreRepositoryService.GetAsync(item =>
                //    item.Id.Equals(id));
                //ChangeRequestStore changeRequestStore = changeRequestStores.First();

                //All comments from Comments table of particular change request
                var commentsStaging = await this._CommentsRepositoryService.GetAsync(item =>
              item.ChangeRequestId.Equals(id));
                commentsStagingModels.AddRange(commentsStaging.Select(item => new CommentsStagingModel
                {
                    AddedBy = item.AddedBy,
                    Comment = item.Comment,
                    CreatedDatetime = item.CreatedDatetime
                }));
            }
            commentsStagingModels= commentsStagingModels.OrderByDescending(x => x.CreatedDatetime).ToList();
            return commentsStagingModels;
        }
    }
}
