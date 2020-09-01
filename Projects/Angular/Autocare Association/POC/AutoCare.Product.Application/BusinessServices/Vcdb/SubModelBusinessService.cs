using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoCare.Product.Application.Infrastructure.ExceptionHelpers;
using AutoCare.Product.Application.Models.BusinessModels;
using AutoCare.Product.Application.RepositoryServices;
using AutoCare.Product.Infrastructure.ExceptionHandler;
using AutoCare.Product.Infrastructure.RepositoryService;
using AutoCare.Product.Infrastructure.Serializer;
using AutoCare.Product.Vcdb.Model;

namespace AutoCare.Product.Application.BusinessServices.Vcdb
{
    public class SubModelBusinessService : VcdbBusinessService<SubModel>, ISubModelBusinessService
    {
        private readonly ISqlServerEfRepositoryService<SubModel> _subModelRepositoryService = null;

        public SubModelBusinessService(IVcdbUnitOfWork vcdbRepositoryService,
            IVcdbChangeRequestBusinessService vcdbChangeRequestBusinessService,
            ITextSerializer serializer)
            : base(vcdbRepositoryService, vcdbChangeRequestBusinessService, serializer)
        {
            _subModelRepositoryService = Repositories.GetRepositoryService<SubModel>() as ISqlServerEfRepositoryService<SubModel>;
        }

        public override async Task<List<SubModel>> GetAllAsync(int topCount = 0)
        {
            return await _subModelRepositoryService.GetAsync(m => m.DeleteDate == null, topCount);
        }

        public override async Task<SubModel> GetAsync<TKey>(TKey id)
        {
            if (id.Equals(default(TKey)))
            {
                throw new ArgumentNullException(nameof(id));
            }

            var subModel = await FindAsync(id);

            if (subModel == null)
            {
                throw new NoRecordFound("Record Not Found");
            }

            subModel.VehicleCount = await _subModelRepositoryService.GetCountAsync(subModel, x => x.Vehicles, y => y.DeleteDate == null);

            return subModel;
        }

        public override async Task<long> SubmitAddChangeRequestAsync(SubModel entity, string requestedBy, 
            List<ChangeRequestItemStaging> changeRequestItemStagings = null, CommentsStagingModel comment = null,
            List<AttachmentsModel> attachmentsStagingModels = null, string changeContent = null)
        {
            if (String.IsNullOrWhiteSpace(entity?.Name))
            {
                throw new ArgumentNullException(nameof(entity));
            }

            changeRequestItemStagings = changeRequestItemStagings ?? new List<ChangeRequestItemStaging>();

            await ValidateSubModelHasNoChangeRequest(entity);

            changeRequestItemStagings.Add(new ChangeRequestItemStaging()
            {
                EntityId = entity.Id.ToString(),
                Entity = typeof(SubModel).Name,
                ChangeType = ChangeType.Add,
                CreatedDateTime = DateTime.UtcNow,
                Payload = Serializer.Serialize(entity)
            });

            changeContent = string.Format("{0}"
                , entity.Name);

            // NOTE: change-request-comments-staging perfomed on base

            return await base.SubmitAddChangeRequestAsync(entity, requestedBy, changeRequestItemStagings, comment, attachmentsStagingModels, changeContent);
        }

        public override async Task<long> SubmitUpdateChangeRequestAsync<TId>(SubModel entity, TId id, string requestedBy,
            ChangeType changeType = ChangeType.None, 
            List<ChangeRequestItemStaging> changeRequestItemStagings = null, CommentsStagingModel comment = null, List<AttachmentsModel> attachments = null, string changeContent = null)
        {
            if (String.IsNullOrWhiteSpace(entity?.Name))
            {
                throw new ArgumentNullException(nameof(entity));
            }

            if (id.Equals(default(TId)))
            {
                throw new ArgumentNullException(nameof(id));
            }

            if (!entity.Id.Equals(id))
            {
                throw new ArgumentException("Invalid Sub Model Id");
            }

            var subModelFromDb = await FindAsync(id);

            if (subModelFromDb == null)
            {
                throw new NoRecordFound("No Sub Model exist");
            }

            changeRequestItemStagings = changeRequestItemStagings ?? new List<ChangeRequestItemStaging>();

            await ValidateSubModelHasNoChangeRequest(entity);
            await ValidateSubModelLookUpHasNoChangeRequest(entity);

            entity.InsertDate = subModelFromDb.InsertDate;

            // to eliminate circular reference during serialize,
            var existingEntity = new SubModel()
            {
                Id = subModelFromDb.Id,
                Name = subModelFromDb.Name,
                ChangeRequestId = subModelFromDb.ChangeRequestId,
                VehicleCount = subModelFromDb.VehicleCount,
                DeleteDate = subModelFromDb.DeleteDate,
                InsertDate = subModelFromDb.InsertDate,
                LastUpdateDate = subModelFromDb.LastUpdateDate
            };

            changeRequestItemStagings.Add(new ChangeRequestItemStaging()
            {
                EntityId = entity.Id.ToString(),
                Entity = typeof(SubModel).Name,
                ChangeType = ChangeType.Modify,
                CreatedDateTime = DateTime.UtcNow,
                Payload = Serializer.Serialize(entity),
                ExistingPayload = base.Serializer.Serialize(existingEntity) // also add existing record as ExistingPayload
            });

            changeContent = string.Format("{0} > {1}"
                , subModelFromDb.Name
                , entity.Name);

            var changeRequestId = await base.SubmitUpdateChangeRequestAsync(entity, id, requestedBy, ChangeType.Modify, changeRequestItemStagings, comment, attachments, changeContent);

            subModelFromDb.ChangeRequestId = changeRequestId;
            _subModelRepositoryService.Update(subModelFromDb);
            Repositories.SaveChanges();
            return changeRequestId;
        }

        public override async Task<long> SubmitDeleteChangeRequestAsync<TId>(SubModel entity, TId id, string requestedBy, List<ChangeRequestItemStaging> changeRequestItemStagings = null,CommentsStagingModel comment=null, List<AttachmentsModel> attachmentsStaging = null, string changeContent = null)
        {
            if (id.Equals(default(TId)))
            {
                throw new ArgumentNullException(nameof(id));
            }

            var subModelsFromDb = await FindAsync(id);
            if (subModelsFromDb == null)
            {
                throw new NoRecordFound("No Sub-Model exist");
            }

            changeRequestItemStagings = changeRequestItemStagings ?? new List<ChangeRequestItemStaging>();

            await ValidateSubModelLookUpHasNoChangeRequest(subModelsFromDb);
            changeRequestItemStagings.Add(new ChangeRequestItemStaging()
            {
                EntityId = subModelsFromDb.Id.ToString(),
                Entity = typeof(SubModel).Name,
                ChangeType = ChangeType.Delete,
                CreatedDateTime = DateTime.UtcNow,
                Payload = Serializer.Serialize(new Make()
                {
                    Id = subModelsFromDb.Id,
                    Name = subModelsFromDb.Name
                })
            });

            changeContent = string.Format("{0}"
                , entity.Name);

            var changeRequestId = await base.SubmitDeleteChangeRequestAsync(subModelsFromDb, id, requestedBy, changeRequestItemStagings, comment, attachmentsStaging, changeContent);

            subModelsFromDb.ChangeRequestId = changeRequestId;
            _subModelRepositoryService.Update(subModelsFromDb);
            Repositories.SaveChanges();

            return changeRequestId;
        }

        public override async Task<ChangeRequestStagingModel<SubModel>> GetChangeRequestStaging<TId>(TId changeRequestId)
        {
            ChangeRequestStagingModel<SubModel> staging = await
                ChangeRequestBusinessService.GetChangeRequestStagingByChangeRequestIdAsync<SubModel, TId>(changeRequestId);

            // business specific task.
            // fill value of "EntityCurrent"
            //if (!staging.StagingItem.ChangeType.Equals(ChangeType.Add.ToString(), StringComparison.CurrentCultureIgnoreCase))
            //{
            //    SubModel currentSubModel = await FindAsync(staging.StagingItem.EntityId);
            //    staging.EntityCurrent = currentSubModel;
            //}

            return staging;
        }

        private async Task ValidateSubModelHasNoChangeRequest(SubModel subModel)
        {
            IList<SubModel> subModelsFromDb = await _subModelRepositoryService.GetAsync(x => x.Name.ToLower().Equals(subModel.Name.ToLower()) && x.DeleteDate == null);

            if (subModelsFromDb != null && subModelsFromDb.Any())
            {
                throw new RecordAlreadyExist("Sub Model already exists");
            }

            var changerequestid=
                   await ChangeRequestBusinessService.ChangeRequestExist<SubModel>(
                        x => x.Name.ToLower().Equals(subModel.Name.ToLower()));

            if(changerequestid>0)
            {
                throw new ChangeRequestExistException("There is already an open CR ID "+ changerequestid + " with same Sub Model Name.");
            }
        }

        private async Task ValidateSubModelLookUpHasNoChangeRequest(SubModel subModel)
        {
            var changerequestid =
                await
                    ChangeRequestBusinessService.ChangeRequestExist<Vehicle>(x => x.SubModelId.Equals(subModel.Id));
            if(changerequestid>0)
            {
                throw new ChangeRequestExistException($"There is already an open CR  ID {changerequestid} for Vehicle using this Sub Model.");
            }
        }

        public override AssociationCount AssociatedCount(string payload)
        {
            AssociationCount count = new AssociationCount();
            if (payload != null)
            {
                SubModel deserializedEntry = Serializer.Deserialize<SubModel>(payload);
                count.VehicleCount = deserializedEntry.VehicleCount;
            }
            return count;
        }

        public override async Task ClearChangeRequestId(long changeRequestId)
        {
            var entities = await _subModelRepositoryService.GetAsync(m => m.ChangeRequestId == changeRequestId && m.DeleteDate == null);

            if (entities != null && entities.Any())
            {
                var entity = entities.First();   //can only be a single item
                entity.ChangeRequestId = null;
                _subModelRepositoryService.Update(entity);
                Repositories.SaveChanges();
            }
        }

        /// <summary>
        /// Find a single record which is not marked as deleted.
        /// </summary>
        /// <typeparam name="TKey">any</typeparam>
        /// <param name="id">the key property</param>
        /// <returns>Single entity object if condition matches, null if not found.</returns>
        private async Task<SubModel> FindAsync<TKey>(TKey id)
        {
            var entityId = Convert.ToInt32(id);
            var entities = await _subModelRepositoryService.GetAsync(m => m.Id == entityId && m.DeleteDate == null, 1);
            if (entities != null && entities.Any())
            {
                return entities.First();
            }
            return null;
        }
    }
}
