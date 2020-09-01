using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoCare.Product.Application.BusinessServices.Event;
using AutoCare.Product.Infrastructure.RepositoryService;
using AutoCare.Product.Infrastructure.Serializer;
using AutoCare.Product.Vcdb.Model;

namespace AutoCare.Product.Application.BusinessServices.EventHandler
{
    public abstract class ChangeRequestReviewEventHandler<T>
        where T : class, IDomainEntity
    {
        protected IUnitOfWork Repositories;
        protected readonly ITextSerializer Serializer;
        protected readonly IDocumentIndexer DocumentIndexer;
        protected ISqlServerEfRepositoryService<T> RepositoryService;
        private readonly IDictionary<ChangeType, Func<ApprovedEvent<T>, Task>> _approvedEventActions = null;
        private readonly IDictionary<ChangeType, Func<RejectedEvent<T>, Task>> _rejectedEventActions = null;
        private readonly IDictionary<ChangeType, Func<DeletedEvent<T>, Task>> _deletedEventActions = null;

        protected ChangeRequestReviewEventHandler(IUnitOfWork repositories,
            ITextSerializer serializer,
            IDocumentIndexer documentIndexer)
        {
            Repositories = repositories;
            Serializer = serializer;
            DocumentIndexer = documentIndexer;
            _approvedEventActions = new Dictionary<ChangeType, Func<ApprovedEvent<T>, Task>>();
            _rejectedEventActions = new Dictionary<ChangeType, Func<RejectedEvent<T>, Task>>();
            _deletedEventActions = new Dictionary<ChangeType, Func<DeletedEvent<T>, Task>>();
            RepositoryService = Repositories.GetRepositoryService<T>() as ISqlServerEfRepositoryService<T>;
            RegisterEventHandlers();
        }

        private void RegisterEventHandlers()
        {
            _approvedEventActions.Add(ChangeType.Add, ApprovedAddChangeRequestAction);
            _approvedEventActions.Add(ChangeType.Modify, ApprovedModifyChangeRequestAction);
            _approvedEventActions.Add(ChangeType.Replace, ApprovedReplaceChangeRequestAction);
            _approvedEventActions.Add(ChangeType.Delete, ApprovedDeleteChangeRequestAction);

            _rejectedEventActions.Add(ChangeType.Add, RejectedAddChangeRequestAction);
            _rejectedEventActions.Add(ChangeType.Modify, RejectedModifyChangeRequestAction);
            _rejectedEventActions.Add(ChangeType.Replace, RejectedReplaceChangeRequestAction);
            _rejectedEventActions.Add(ChangeType.Delete, RejectedDeleteChangeRequestAction);

            _deletedEventActions.Add(ChangeType.Add, DeletedAddChangeRequestAction);
            _deletedEventActions.Add(ChangeType.Modify, DeletedModifyChangeRequestAction);
            _deletedEventActions.Add(ChangeType.Replace, DeletedReplaceChangeRequestAction);
            _deletedEventActions.Add(ChangeType.Delete, DeletedDeleteChangeRequestAction);
        }

        protected async Task RaiseApprovedEventAction(ApprovedEvent<T> approvedEvent)
        {
            Func<ApprovedEvent<T>, Task> approvedEventAction;
            if (_approvedEventActions.TryGetValue(approvedEvent.ChangeType, out approvedEventAction))
            {
                await approvedEventAction(approvedEvent);
            }
        }

        protected async Task RaiseRejectedEventAction(RejectedEvent<T> rejectedEvent)
        {
            Func<RejectedEvent<T>, Task> rejectedEventAction;
            if (_rejectedEventActions.TryGetValue(rejectedEvent.ChangeType, out rejectedEventAction))
            {
                await rejectedEventAction(rejectedEvent);
            }
        }

        protected async Task RaiseDeletedEventAction(DeletedEvent<T> deletedEvent)
        {
            Func<DeletedEvent<T>, Task> deletedEventAction;
            if (_deletedEventActions.TryGetValue(deletedEvent.ChangeType, out deletedEventAction))
            {
                await deletedEventAction(deletedEvent);
            }
        }

        protected virtual async Task ApprovedAddChangeRequestAction(ApprovedEvent<T> approvedEvent)
        {
            var deserializedEntity = Serializer.Deserialize<T>(approvedEvent.Payload);
            deserializedEntity.Id = RepositoryService.GetMax(x => x.Id) + 1;
            deserializedEntity.ChangeRequestId = approvedEvent.ChangeRequestId;
            deserializedEntity.InsertDate = DateTime.UtcNow;
            deserializedEntity.LastUpdateDate = DateTime.UtcNow;
            //this.UpdateEntity(ref deserializedEntity, ChangeType.Add, approvedEvent.ChangeRequestId);

            RepositoryService.Add(deserializedEntity);
            if (await Repositories.SaveChangesAsync() > 0)
            {
                await DocumentIndexer.AddChangeRequestIndexerAsync(approvedEvent.ChangeRequestId);
                await this.ClearChangeRequestId<T>(approvedEvent.ChangeRequestId);
            }
        }

        protected virtual async Task ApprovedModifyChangeRequestAction(ApprovedEvent<T> approvedEvent)
        {
            var deserializedEntity = Serializer.Deserialize<T>(approvedEvent.Payload);
            deserializedEntity.ChangeRequestId = approvedEvent.ChangeRequestId;
            deserializedEntity.LastUpdateDate = DateTime.UtcNow;

            RepositoryService.Update(deserializedEntity);

            if (await Repositories.SaveChangesAsync() > 0)
            {
                await
                    DocumentIndexer.ModifyChangeRequestIndexerAsync(approvedEvent.ChangeRequestId);
                await this.ClearChangeRequestId<T>(approvedEvent.ChangeRequestId);
            }
        }

        protected virtual async Task ApprovedReplaceChangeRequestAction(ApprovedEvent<T> approvedEvent)
        {
            var deserializedEntity = Serializer.Deserialize<T>(approvedEvent.Payload);
            deserializedEntity.ChangeRequestId = approvedEvent.ChangeRequestId;
            deserializedEntity.LastUpdateDate = DateTime.UtcNow;

            RepositoryService.Update(deserializedEntity);
            if (await Repositories.SaveChangesAsync() > 0)
            {
                await
                    DocumentIndexer.ReplaceChangeRequestIndexerAsync(
                        approvedEvent.ChangeRequestId);
                await this.ClearChangeRequestId<T>(approvedEvent.ChangeRequestId);
            }
        }

        protected virtual async Task ApprovedDeleteChangeRequestAction(ApprovedEvent<T> approvedEvent)
        {
            var existingEntity = await RepositoryService.FindAsync(Convert.ToInt32(approvedEvent.EntityId));

            existingEntity.ChangeRequestId = approvedEvent.ChangeRequestId;
            existingEntity.DeleteDate = DateTime.UtcNow;

            RepositoryService.Update(existingEntity);
            if (await Repositories.SaveChangesAsync() > 0)
            {
                await
                    DocumentIndexer.DeleteChangeRequestIndexerAsync(
                        approvedEvent.ChangeRequestId);

                await this.ClearChangeRequestId<T>(approvedEvent.ChangeRequestId);
            }
        }

        protected virtual async Task RejectedAddChangeRequestAction(RejectedEvent<T> rejectedEvent)
        {
            //note: during reject of add request, nothing is to be done at transaction tables
            // do nothing
            return;
        }

        protected virtual async Task RejectedModifyChangeRequestAction(RejectedEvent<T> rejectedEvent)
        {
            var existingEntity = RepositoryService.FindAsync(Convert.ToInt32(rejectedEvent.EntityId)).Result;
            // note: change request id is required for documentIndexer. this will eventually be cleared after indexer processing.
            existingEntity.ChangeRequestId = rejectedEvent.ChangeRequestId;

            RepositoryService.Update(existingEntity);
            if (await Repositories.SaveChangesAsync() > 0)
            {
                // clear change request id in azure
                await DocumentIndexer.RejectChangeRequestIndexerAsync(rejectedEvent.ChangeRequestId);
                // clear change request id in transaction table
                await this.ClearChangeRequestId<T>(rejectedEvent.ChangeRequestId);
            }
        }

        protected virtual async Task RejectedReplaceChangeRequestAction(RejectedEvent<T> rejectedEvent)
        {
            // note: this has same effect as modify in transaction table
            await this.RejectedModifyChangeRequestAction(rejectedEvent);
        }

        protected virtual async Task RejectedDeleteChangeRequestAction(RejectedEvent<T> rejectedEvent)
        {
            // note: this has same effect as modify in transaction table
            await this.RejectedModifyChangeRequestAction(rejectedEvent);
        }

        private async Task DeletedAddChangeRequestAction(DeletedEvent<T> deletedEvent)
        {
            //note: during delete CR of add request, nothing is to be done at transaction tables
            // do nothing
            return;
        }

        private async Task DeletedDeleteChangeRequestAction(DeletedEvent<T> deletedEvent)
        {
            // note: this has same effect as modify in transaction table
            await this.DeletedModifyChangeRequestAction(deletedEvent);
        }

        protected virtual async Task DeletedReplaceChangeRequestAction(DeletedEvent<T> deletedEvent)
        {
            // note: this has same effect as modify in transaction table
            await this.DeletedModifyChangeRequestAction(deletedEvent);
        }

        private async Task DeletedModifyChangeRequestAction(DeletedEvent<T> deletedEvent)
        {
            var existingEntity = RepositoryService.FindAsync(Convert.ToInt32(deletedEvent.EntityId)).Result;
            // note: change request id is required for documentIndexer. this will eventually be cleared after indexer processing.
            existingEntity.ChangeRequestId = deletedEvent.ChangeRequestId;

            RepositoryService.Update(existingEntity);
            if (await Repositories.SaveChangesAsync() > 0)
            {
                // clear change request id in azure
                //TODO: rename RejectChangeRequestIndexerAsync to RejectOrDeleteChangeRequestIndexerAsync
                await DocumentIndexer.RejectChangeRequestIndexerAsync(deletedEvent.ChangeRequestId);
                // clear change request id in transaction table
                await this.ClearChangeRequestId<T>(deletedEvent.ChangeRequestId);
            }
        }

        //protected virtual void UpdateEntity(ref T entity, ChangeType changeType, long changeRequestId)
        //{
        //    // TODO: refactoring required for update property value on model.
        //    switch (changeType)
        //    {
        //        case ChangeType.None:
        //            break;
        //        case ChangeType.Delete:
        //            entity.ChangeRequestId = changeRequestId;
        //            entity.DeleteDate = DateTime.UtcNow;
        //            // note: soft delete
        //            break;
        //        case ChangeType.Add:
        //            entity.Id = RepositoryService.GetMax(x => x.Id) + 1;
        //            entity.ChangeRequestId = changeRequestId;
        //            entity.InsertDate = DateTime.UtcNow;
        //            entity.LastUpdateDate = DateTime.UtcNow;
        //            //// TODO: set id according to modelType profile.
        //            //PropertyInfo pi = typeof(T).GetProperties()
        //            //    .FirstOrDefault(p => p.GetCustomAttributes(typeof(IdentityProperty), true).Any());
        //            //pi?.SetValue(entity, (RepositoryService.GetMax(pi) + 1));
        //            //typeof(T).GetProperties()
        //            //    .FirstOrDefault(p => p.GetCustomAttributes(typeof(ChangeRequestProperty), true).Any())?
        //            //    .SetValue(entity, changeRequestId);
        //            //typeof(T).GetProperties()
        //            //    .FirstOrDefault(p => p.GetCustomAttributes(typeof(InsertedOnProperty), true).Any())?
        //            //    .SetValue(entity, DateTime.UtcNow);
        //            //typeof(T).GetProperties()
        //            //    .FirstOrDefault(p => p.GetCustomAttributes(typeof(UpdatedOnProperty), true).Any())?
        //            //    .SetValue(entity, DateTime.UtcNow);
        //            break;
        //        case ChangeType.Modify:
        //        case ChangeType.Replace:
        //            entity.ChangeRequestId = changeRequestId;
        //            entity.LastUpdateDate = DateTime.UtcNow;
        //            //typeof(T).GetProperties()
        //            //    .FirstOrDefault(p => p.GetCustomAttributes(typeof(ChangeRequestProperty), true).Any())?
        //            //    .SetValue(entity, changeRequestId);
        //            //typeof(T).GetProperties()
        //            //    .FirstOrDefault(p => p.GetCustomAttributes(typeof(UpdatedOnProperty), true).Any())?
        //            //    .SetValue(entity, DateTime.UtcNow);
        //            break;
        //    }
        //}

        protected async Task ClearChangeRequestId<TEntity>(long changeRequestId)
            where TEntity : class, IDomainEntity
        {
            if (typeof(TEntity) != typeof(T))
            {
                var replaceRepositoryService = Repositories.GetRepositoryService<TEntity>() as ISqlServerEfRepositoryService<TEntity>;

                var replaceEntities =
                await
                    replaceRepositoryService.GetAsync(m => m.ChangeRequestId == changeRequestId && m.DeleteDate == null);

                if (replaceEntities != null && replaceEntities.Any())
                {
                    var replaceEntity = replaceEntities.First();   //can only be a single item
                    replaceEntity.ChangeRequestId = null;
                    replaceRepositoryService.Update(replaceEntity);

                    Repositories.SaveChanges();
                }
            }
            else
            {
                var entities =
                    await
                        RepositoryService.GetAsync(m => m.ChangeRequestId == changeRequestId && m.DeleteDate == null);

                if (entities != null && entities.Any())
                {
                    var entity = entities.First();   //can only be a single item
                    entity.ChangeRequestId = null;
                    RepositoryService.Update(entity);

                    Repositories.SaveChanges();
                }
            }
        }
    }
}

