using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using AutoCare.Product.Application.BusinessServices.Event;
using AutoCare.Product.Application.Models.BusinessModels;
using AutoCare.Product.Application.RepositoryServices;
using AutoCare.Product.Infrastructure.Serializer;
using AutoCare.Product.Vcdb.Model;

namespace AutoCare.Product.Application.BusinessServices.Vcdb
{
    public class VcdbBusinessService<T> : BusinessServiceBase<T>, IVcdbBusinessService<T>
        where T : class
    {
        private readonly VcdbSqlServerEfRepositoryService<T> _repositoryService;
        // todo: make this private. and in all derived business service use created private service instead.
        //private readonly IVcdbChangeRequestBusinessService _vcdbChangeRequestBusinessService;
        protected readonly IVcdbChangeRequestBusinessService ChangeRequestBusinessService;

        public VcdbBusinessService(IVcdbUnitOfWork vcdbUnitOfWork,
            IVcdbChangeRequestBusinessService vcdbChangeRequestBusinessService,
            ITextSerializer serializer)
            : base(vcdbUnitOfWork, serializer)
        {
            _repositoryService = vcdbUnitOfWork.GetRepositoryService<T>() as VcdbSqlServerEfRepositoryService<T>;
            ChangeRequestBusinessService = vcdbChangeRequestBusinessService;
        }

        public override bool Handle(ChangeRequestApprovedEvent changeRequestApprovedEvent)
        {
            if (_repositoryService != null && changeRequestApprovedEvent != null)
            {
                T deserializedEntity = Serializer.Deserialize<T>(changeRequestApprovedEvent.Payload);

                switch (changeRequestApprovedEvent.ChangeRequestStatus)
                {
                    case ChangeRequestStatus.Submitted: // note: this is never executed.
                    case ChangeRequestStatus.PreliminaryApproved: // note: this is never executed.
                        break;
                    case ChangeRequestStatus.Deleted:
                    case ChangeRequestStatus.Rejected:
                        switch (changeRequestApprovedEvent.ChangeType)
                        {
                            case ChangeType.Delete:
                            case ChangeType.Modify:
                            case ChangeType.Replace:
                                T existingEntity = _repositoryService.FindAsync(Convert.ToInt32(changeRequestApprovedEvent.EntityId)).Result;
                                typeof(T).GetProperties()
                                    .FirstOrDefault(p => p.GetCustomAttributes(typeof(ChangeRequestProperty), true).Any())?
                                    .SetValue(existingEntity, null);
                                _repositoryService.Update(existingEntity);
                                break;
                        }
                        break;
                    case ChangeRequestStatus.Approved:
                        switch (changeRequestApprovedEvent.ChangeType)
                        {
                            case ChangeType.None: // note: this is never executed.
                                break;
                            case ChangeType.Delete:
                                // note: soft delete only
                                T existingEntity = _repositoryService.FindAsync(Convert.ToInt32(changeRequestApprovedEvent.EntityId)).Result;
                                this.UpdateEntity(ref existingEntity, ChangeType.Delete, changeRequestApprovedEvent.ChangeRequestId);
                                _repositoryService.Update(existingEntity);

                                break;
                            case ChangeType.Add:
                                this.UpdateEntity(ref deserializedEntity, ChangeType.Add, changeRequestApprovedEvent.ChangeRequestId);
                                _repositoryService.Add(deserializedEntity);
                                break;
                            case ChangeType.Modify:
                                this.UpdateEntity(ref deserializedEntity, ChangeType.Modify, changeRequestApprovedEvent.ChangeRequestId);
                                _repositoryService.Update(deserializedEntity);
                                break;
                            case ChangeType.Replace:
                                this.UpdateEntity(ref deserializedEntity, ChangeType.Replace, changeRequestApprovedEvent.ChangeRequestId);
                                _repositoryService.Update(deserializedEntity);
                                break;
                        }

                        break;
                    default:
                        throw new Exception("Status type not found.");
                }
                // note: savechanges is performed at change request business service
            }

            return true;
        }

        public override bool Handle(ChangeRequestRejectedEvent changeRequestRejectedEvent)
        {
            if (changeRequestRejectedEvent == null)
            {
                throw new ArgumentNullException(nameof(changeRequestRejectedEvent));
            }

            switch (changeRequestRejectedEvent.ChangeType)
            {
                case ChangeType.Delete:
                case ChangeType.Modify:
                case ChangeType.Replace:
                    T existingEntity =
                        _repositoryService.FindAsync(Convert.ToInt32(changeRequestRejectedEvent.EntityId)).Result;
                    typeof (T).GetProperties()
                        .FirstOrDefault(p => p.GetCustomAttributes(typeof (ChangeRequestProperty), true).Any())?
                        .SetValue(existingEntity, null);
                    _repositoryService.Update(existingEntity);
                    break;
            }

            return true;
        }

        public override bool Handle(ChangeRequestPrelimApprovedEvent changeRequestPrelimApprovedEvent)
        {
            throw new NotImplementedException();
        }

        public override async Task<bool> SubmitLikeAsync(long crId, string likedBy, string likedStatus)
        {
            return await ChangeRequestBusinessService.SubmitLikeAsync(crId, likedBy, likedStatus);
        }

        public override AssociationCount AssociatedCount(string payLoad)
        {
            throw new NotImplementedException();
        }

        public override Task ClearChangeRequestId(long changeRequestId)
        {
            throw new NotImplementedException();
        }

        private void UpdateEntity(ref T entity, ChangeType changeType, long changeRequestId)
        {
            // TODO: refactoring required for update property value on model.
            switch (changeType)
            {
                case ChangeType.None:
                    break;
                case ChangeType.Delete:
                    typeof(T).GetProperties()
                        .FirstOrDefault(p => p.GetCustomAttributes(typeof(ChangeRequestProperty), true).Any())?
                        .SetValue(entity, changeRequestId);
                    // note: soft delete
                    typeof(T).GetProperties()
                        .FirstOrDefault(p => p.GetCustomAttributes(typeof(DeletedOnProperty), true).Any())?
                        .SetValue(entity, DateTime.UtcNow);
                    break;
                case ChangeType.Add:
                    // TODO: set id according to modelType profile.
                    PropertyInfo pi = typeof(T).GetProperties()
                        .FirstOrDefault(p => p.GetCustomAttributes(typeof(IdentityProperty), true).Any());
                    pi?.SetValue(entity, (this._repositoryService.GetMax(pi) + 1));
                    typeof(T).GetProperties()
                        .FirstOrDefault(p => p.GetCustomAttributes(typeof(ChangeRequestProperty), true).Any())?
                        .SetValue(entity, changeRequestId);
                    typeof(T).GetProperties()
                        .FirstOrDefault(p => p.GetCustomAttributes(typeof(InsertedOnProperty), true).Any())?
                        .SetValue(entity, DateTime.UtcNow);
                    typeof(T).GetProperties()
                        .FirstOrDefault(p => p.GetCustomAttributes(typeof(UpdatedOnProperty), true).Any())?
                        .SetValue(entity, DateTime.UtcNow);
                    break;
                case ChangeType.Modify:
                case ChangeType.Replace:
                    typeof(T).GetProperties()
                        .FirstOrDefault(p => p.GetCustomAttributes(typeof(ChangeRequestProperty), true).Any())?
                        .SetValue(entity, changeRequestId);
                    typeof(T).GetProperties()
                        .FirstOrDefault(p => p.GetCustomAttributes(typeof(UpdatedOnProperty), true).Any())?
                        .SetValue(entity, DateTime.UtcNow);
                    break;
            }
        }

        protected CommentsStaging MakeCommentsStaging(CommentsStagingModel comment, String addedBy)
        {
            // make change request comments staging.
            CommentsStaging changeRequestCommentsStaging = new CommentsStaging()
            {
                Id = 0,
                Comment = comment.Comment,
                AddedBy = addedBy,
                CreatedDatetime = DateTime.UtcNow
            };
            return changeRequestCommentsStaging;
        }

        protected List<AttachmentsStaging> MakeAttachmentsStaging(List<AttachmentsModel> attachments, String attachedBy)
        {
            // make change request comments staging.
            List<AttachmentsStaging> attachmentsStaging = new List<AttachmentsStaging>();

            if (attachments == null || attachments.Count == 0)
            {
                return attachmentsStaging;
            }

            AttachmentsStaging attachmentStaging;
            attachments.ForEach(attachment =>
            {
                attachmentStaging = new AttachmentsStaging
                {
                    AttachmentId = attachment.AttachmentId,
                    AttachedBy = attachedBy,
                    FileExtension = attachment.FileExtension,
                    FileName = attachment.FileName,
                    DirectoryPath = attachment.DirectoryPath,
                    CreatedDateTime = DateTime.UtcNow,
                    AzureContainerName = attachment.ContainerName,
                    ContentType = attachment.ContentType,
                    BlocksIdList = attachment.ChunksIdList?.Split(',').ToList(),
                    FileSize = attachment.FileSize,
                    FileStatus = attachment.FileStatus
                };

                attachmentsStaging.Add(attachmentStaging);
            });

            return attachmentsStaging;
        }


        public virtual async Task<long> SubmitAddChangeRequestAsync(T entity, string requestedBy, List<ChangeRequestItemStaging> changeRequestItemStagings = null,
            CommentsStagingModel comment = null, List<AttachmentsModel> attachments = null, string changeContent = null)
        {
            // make change request comments stagings.
            CommentsStaging changeRequestCommentsStaging = null;
            if (comment != null)
            {
                changeRequestCommentsStaging = this.MakeCommentsStaging(comment, requestedBy);
            }

            return
                await
                    ChangeRequestBusinessService.SubmitAsync(entity, 0, requestedBy, ChangeType.Add,
                        changeRequestItemStagings, changeRequestCommentsStaging,
                        MakeAttachmentsStaging(attachments, requestedBy), changeContent);
        }

        public virtual async Task<long> SubmitUpdateChangeRequestAsync<TId>(T entity, TId id, string requestedBy, ChangeType changeType = ChangeType.None,
            List<ChangeRequestItemStaging> changeRequestItemStagings = null, CommentsStagingModel comment = null, List<AttachmentsModel> attachments = null, string changeContent = null)
        {
            // make change request comments stagings.
            CommentsStaging changeRequestCommentsStaging = null;
            List<AttachmentsStaging> changeRequestAttachmentStaging = null;
            if (comment != null)
            {
                changeRequestCommentsStaging = this.MakeCommentsStaging(comment, requestedBy);
            }
            if (attachments != null)
            {
                changeRequestAttachmentStaging = this.MakeAttachmentsStaging(attachments, requestedBy);
            }

            return
                await
                    ChangeRequestBusinessService.SubmitAsync(entity, id, requestedBy, changeType,
                        changeRequestItemStagings, changeRequestCommentsStaging, changeRequestAttachmentStaging, changeContent);
        }

        public virtual async Task<long> SubmitDeleteChangeRequestAsync<TId>(T entity, TId id, string requestedBy, List<ChangeRequestItemStaging> changeRequestItemStagings = null,
            CommentsStagingModel comment = null, List<AttachmentsModel> attachments = null, string changeContent = null)
        {
            CommentsStaging changeRequestCommentsStaging = null;
            List<AttachmentsStaging> changeRequestAttachmentStaging = null;
            if (comment != null)
            {
                changeRequestCommentsStaging = this.MakeCommentsStaging(comment, requestedBy);
            }
            if (attachments != null)
            {
                changeRequestAttachmentStaging = this.MakeAttachmentsStaging(attachments, requestedBy);
            }

            return
                await
                    ChangeRequestBusinessService.SubmitAsync(entity, id, requestedBy, ChangeType.Delete,
                        changeRequestItemStagings, changeRequestCommentsStaging, changeRequestAttachmentStaging, changeContent);
        }

        public override async Task<long> ChangeRequestExist<TId>(T entity, TId id)
        {
            return await ChangeRequestBusinessService.ChangeRequestExist(entity, id);
        }

        public virtual async Task<long> SubmitReplaceChangeRequestAsync<TId>(T entity, TId id, string requestedBy, List<ChangeRequestItemStaging> changeRequestItemStagings = null,
            CommentsStagingModel comment = null, List<AttachmentsModel> attachments = null, string changeContent = null)
        {
            CommentsStaging changeRequestCommentsStaging = null;
            List<AttachmentsStaging> changeRequestAttachmentStaging = null;
            if (comment != null)
            {
                changeRequestCommentsStaging = this.MakeCommentsStaging(comment, requestedBy);
            }
            if (attachments != null)
            {
                changeRequestAttachmentStaging = this.MakeAttachmentsStaging(attachments, requestedBy);
            }

            return await ChangeRequestBusinessService.SubmitAsync(entity, id, requestedBy,
                ChangeType.Replace, changeRequestItemStagings, changeRequestCommentsStaging, changeRequestAttachmentStaging, changeContent);
        }

        public virtual async Task<AssociationCount> GetAssociatedCount(List<ChangeRequestStaging> selectedChangeRequestStagings)
        {
            return await this.ChangeRequestBusinessService.GetAssociatedCount(selectedChangeRequestStagings);
        }

        public override async Task<ChangeRequestStagingModel<T>> GetChangeRequestStaging<TId>(TId changeRequestId)
        {
            return
                await
                    this.ChangeRequestBusinessService.GetChangeRequestStagingByChangeRequestIdAsync<T, TId>(changeRequestId);
        }

        public override Task<List<T>> GetPendingAddChangeRequests(Expression<Func<T, bool>> whereCondition = null, int topCount = 0)
        {
            throw new NotImplementedException();
        }

        public override async Task<bool> SubmitChangeRequestReviewAsync<TId>(TId changeRequestId, ChangeRequestReviewModel review)
        {
            // make CommentStaging
            CommentsStaging changeRequestCommentsStaging = null;
            List<AttachmentsStaging> attachments = null;
            if (!string.IsNullOrEmpty(review.ReviewComment.Comment))
            {
                changeRequestCommentsStaging = this.MakeCommentsStaging(review.ReviewComment, review.ReviewedBy);
            }

            if (review.Attachments != null)
            {
                attachments = this.MakeAttachmentsStaging(review.Attachments, review.ReviewedBy);
            }

            return await this.ChangeRequestBusinessService.SubmitReviewAsync(changeRequestId, review.ReviewedBy,
                review.ReviewStatus, changeRequestCommentsStaging, attachments);
        }

        public override async Task<List<ChangeEntityFacet>> GetAllChangeTypes()
        {
            return
                await
                    this.ChangeRequestBusinessService.GetAllChangeTypes();
        }
    }
}
