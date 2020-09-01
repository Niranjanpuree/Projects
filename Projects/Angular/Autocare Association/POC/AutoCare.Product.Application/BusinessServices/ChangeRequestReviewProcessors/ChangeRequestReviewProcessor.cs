using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoCare.Product.Application.Models.BusinessModels;
using AutoCare.Product.Application.RepositoryServices;
using AutoCare.Product.Infrastructure.RepositoryService;
using AutoCare.Product.Vcdb.Model;
using AutoCare.Product.VcdbSearch.Indexing.ApplicationService;
using AutoCare.Product.VcdbSearchIndex.Model;

namespace AutoCare.Product.Application.BusinessServices.ChangeRequestReviewProcessors
{
    public abstract class ChangeRequestReviewProcessor
    {
        protected readonly IChangeRequestCommentsBusinessService<CommentsStaging> CommentsStagingBusinessService;
        protected readonly IChangeRequestAttachmentBusinessService<AttachmentsStaging> AttachmentStagingBusinessService;
        private readonly IAzureFileStorageRepositoryService _azureFileStorageRepositoryService;
        protected readonly IUnitOfWork Repositories;
        private readonly IChangeRequestIndexingService _changeRequestIndexingService;

        protected readonly IChangeRequestItemBusinessService<ChangeRequestItemStaging>
            ChangeRequestItemStagingBusinessService;

        protected readonly IRepositoryService<ChangeRequestAssignmentStaging> AssignmentStagingRepositoryService;
        protected readonly IRepositoryService<ChangeRequestStaging> ChangeRequestStagingRepositoryService;
        protected readonly IRepositoryService<ChangeRequestStore> ChangeRequestStoreRepositoryService;
        protected readonly IRepositoryService<LikesStaging> LikesStagingRepositoryService;

        protected ChangeRequestReviewProcessor(
            IChangeRequestCommentsBusinessService<CommentsStaging> commentsStagingBusinessService,
            IChangeRequestAttachmentBusinessService<AttachmentsStaging> attachmentStagingBusinessService,
            IChangeRequestItemBusinessService<ChangeRequestItemStaging> changeRequestItemStagingBusinessService,
            IAzureFileStorageRepositoryService azureFileStorageRepositoryService,
            IUnitOfWork repositories,
            IChangeRequestIndexingService changeRequestIndexingService)
        {
            CommentsStagingBusinessService = commentsStagingBusinessService;
            AttachmentStagingBusinessService = attachmentStagingBusinessService;
            _azureFileStorageRepositoryService = azureFileStorageRepositoryService;
            Repositories = repositories;
            _changeRequestIndexingService = changeRequestIndexingService;
            ChangeRequestItemStagingBusinessService = changeRequestItemStagingBusinessService;
            AssignmentStagingRepositoryService =Repositories.GetRepositoryService<ChangeRequestAssignmentStaging>();
            LikesStagingRepositoryService = Repositories.GetRepositoryService<LikesStaging>();
            ChangeRequestStagingRepositoryService = Repositories.GetRepositoryService<ChangeRequestStaging>();
            ChangeRequestStoreRepositoryService = Repositories.GetRepositoryService<ChangeRequestStore>();
        }

        public async Task<ChangeRequestStore> AddToChangeRequestStoreAsync(long changeRequestId,
            ChangeRequestStatus status,
            string decisionBy)
        {
            try
            {
                long id = Convert.ToInt64(changeRequestId);

                ChangeRequestStaging staging = await this.ChangeRequestStagingRepositoryService.FindAsync(id);
                List<ChangeRequestItemStaging> items =
                    await ChangeRequestItemStagingBusinessService.GetChangeRequestItemsAsync(
                        item => item.ChangeRequestId == id);
                List<CommentsStaging> comments =
                    await CommentsStagingBusinessService.GetChangeRequestCommentsAsync(
                        item => item.ChangeRequestId == id);
                List<AttachmentsStaging> attachments =
                    await AttachmentStagingBusinessService.GetChangeRequestAttachmentsAsync(
                        item => item.ChangeRequestId == id);
                List<ChangeRequestAssignmentStaging> assignments =
                    await AssignmentStagingRepositoryService.GetAsync(
                        item => item.ChangeRequestId == id);
                List<LikesStaging> likes =
                    await LikesStagingRepositoryService.GetAsync(item => item.ChangeRequestId == id);

                staging.ChangeRequestItemStagings = items;
                staging.CommentsStagings = comments;
                staging.AttachmentsStagings = attachments;
                staging.ChangeRequestAssignmentStagings = assignments;
                staging.LikesStagings = likes;

                // todo: use automapper for map ChangeRequestStaging to ChangeRequesStore
                ChangeRequestStore changeRequestStoreItem = this.MapToChangeRequestStore(staging);
                changeRequestStoreItem.DecisionBy = decisionBy;
                changeRequestStoreItem.Status = status;
                changeRequestStoreItem.CompletedDateTime = DateTime.UtcNow;

                this.ChangeRequestStoreRepositoryService.Add(changeRequestStoreItem);
                return changeRequestStoreItem;
            }
            catch (Exception)
            {
                throw;
            }

        }

        public async Task UpdateChangeRequestStatusToChangeRequestIndexAsync(long changeRequestId,
            ChangeRequestStatus changeRequestStatus, long reviewCommentId)
        {
            var document = new ChangeRequestDocument()
            {
                ChangeRequestId = changeRequestId.ToString(),
                Status = (short) changeRequestStatus,
                StatusText = changeRequestStatus.ToString(),
                UpdatedDate = DateTime.UtcNow
            };
            if (reviewCommentId > 0)
            {
                document.CommentExists = true;
            }

            await this._changeRequestIndexingService.UpdateDocumentAsync(document);
        }

        public async Task<ChangeRequestStaging> RemoveFromChangeRequestStagingAsync(long changeRequestId)
        {
            return await this.ChangeRequestStagingRepositoryService.RemoveAsync(changeRequestId);
        }

        protected string GetFolderPath(long key, int maxFiles = 1000, int depth = 2)
        {
            var parts = new List<string>();
            long current = key;

            for (int i = depth; i >= 0; i--)
            {
                long q = Convert.ToInt64(Math.Pow(maxFiles, i));
                long level = current/q;

                parts.Add($"{level:0000}");

                current = current%q;
            }

            //parts.Add(string.Format("{0:0000}{1}", current, extension));

            string separator = Path.DirectorySeparatorChar.ToString(CultureInfo.InvariantCulture);
            string path = string.Join(separator, parts);

            return path;
        }

        protected async Task SaveFileToTempLocation(AttachmentsStaging attachment)
        {
            await _azureFileStorageRepositoryService.SaveFileAsync(new AzureFileModel
            {
                BlobName = attachment.FileName,
                ContainerName = attachment.AzureContainerName,
                ContentType = attachment.ContentType,
                ChunkIdList = attachment.BlocksIdList
            });
        }

        protected async Task<Uri> MoveFileFromTempLocationToPermanent(string sourceContainer, string targetContainer,
            string sourceBlobName,
            string targetDirectoryPath)
        {
            return await _azureFileStorageRepositoryService.MoveFileAsync(sourceContainer, targetContainer,
                sourceBlobName, targetDirectoryPath);
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
                    EntityFullName = "",
                    Payload = item.Payload,
                    ExistingPayload = item.ExistingPayload, // existing payload
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
            List<Likes> likes = staging.LikesStagings?.Select(item
                => new Likes()
                {
                    Id = item.Id,
                    ChangeRequestId = item.ChangeRequestId,
                    LikeStatus = item.LikeStatus,
                    LikedBy = item.LikedBy,
                    UpdatedDatetime = item.UpdatedDatetime,
                    CreatedDatetime = item.CreatedDatetime
                }).ToList();

            // todo: map taskcontroller


            // fill up children/ details
            store.ChangeRequestItems = storeItems;
            store.Comments = comments;
            store.Attachments = attachments;
            store.ChangeRequestAssignments = assignments;
            store.Likes = likes;
            // todo: fill taskcontoller

            return store;
        }
    }
}
