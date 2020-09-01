using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AutoCare.Product.Application.Models.BusinessModels;
using AutoCare.Product.Application.Infrastructure.ExceptionHelpers;
using AutoCare.Product.Application.RepositoryServices;
using AutoCare.Product.Infrastructure.ExceptionHandler;
using AutoCare.Product.Infrastructure.RepositoryService;
using AutoCare.Product.Infrastructure.Serializer;
using AutoCare.Product.Vcdb.Model;

namespace AutoCare.Product.Application.BusinessServices.Vcdb
{
    public class ModelBusinessService : VcdbBusinessService<Model>, IModelBusinessService
    {
        //private readonly IModelSqlServerEfRepositoryService _modelRepositoryService = null;
        private readonly ISqlServerEfRepositoryService<Model> _modelRepositoryService = null;
        private readonly ITextSerializer _serializer;
        public ModelBusinessService(IVcdbUnitOfWork vcdbRepositoryService,
            IVcdbChangeRequestBusinessService vcdbChangeRequestBusinessService,
            ITextSerializer serializer)
            : base(vcdbRepositoryService, vcdbChangeRequestBusinessService, serializer)
        {
            //_modelRepositoryService = Repositories.GetRepositoryService<IModelSqlServerEfRepositoryService, Model>();
            _modelRepositoryService = Repositories.GetRepositoryService<Model>() as ISqlServerEfRepositoryService<Model>;
            _serializer = serializer;
        }

        public override async Task<List<Model>> GetAllAsync(int topCount = 0)
        {
            return await _modelRepositoryService.GetAsync(m => m.DeleteDate == null, topCount);
        }

        public override async Task<Model> GetAsync<TKey>(TKey id)
        {
            if (id.Equals(default(TKey)))
            {
                throw new ArgumentNullException(nameof(id));
            }

            var model = await FindAsync(id);

            if (model == null)
            {
                throw new NoRecordFound("Record Not Found");
            }

            model.BaseVehicleCount = await _modelRepositoryService.GetCountAsync(model, x => x.BaseVehicles, y => y.DeleteDate == null);

            var modelId = Convert.ToInt32(id);
            model.VehicleCount = await _modelRepositoryService
                .GetAllQueryable()
                .Where(x => x.Id == modelId).Include("BaseVehicles.Vehicles")
                .SelectMany(x => x.BaseVehicles)
                .SelectMany(x => x.Vehicles).CountAsync(y => y.DeleteDate == null);

            return model;
        }

        public override async Task<long> SubmitAddChangeRequestAsync(Model entity, string requestedBy,
            List<ChangeRequestItemStaging> changeRequestItemStagings = null, CommentsStagingModel comment = null,
            List<AttachmentsModel> attachmentsStagingModels = null, string changeContent = null)

        {
            if (String.IsNullOrWhiteSpace(entity?.Name))
            {
                throw new ArgumentNullException(nameof(entity));
            }

            await ValidateModelHasNoChangeRequest(entity);
            await ValidateVehicleTypeHasNoChangeRequest(entity);

            changeRequestItemStagings = changeRequestItemStagings ?? new List<ChangeRequestItemStaging>();

            changeRequestItemStagings.Add(new ChangeRequestItemStaging()
            {
                ChangeType = ChangeType.Add,
                EntityId = entity.Id.ToString(),
                CreatedDateTime = DateTime.UtcNow,
                Entity = typeof(Model).Name,
                Payload = base.Serializer.Serialize(entity)
            });

            changeContent = string.Format("{0}"
                , entity.Name);

            // NOTE: change-request-comments-staging perfomed on base

            return await base.SubmitAddChangeRequestAsync(entity, requestedBy, changeRequestItemStagings, comment, attachmentsStagingModels, changeContent);
        }

        public override async Task<long> SubmitUpdateChangeRequestAsync<TId>(Model entity, TId id, string requestedBy,
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
                throw new ArgumentException("Invalid Model Id");
            }

            var modelFromDb = await FindAsync(id);

            if (modelFromDb == null)
            {
                throw new NoRecordFound("No Model exist");
            }

            if (modelFromDb.Name.Equals(entity.Name) && modelFromDb.VehicleTypeId==entity.VehicleTypeId)
            {
                throw new RecordAlreadyExist("Proposed change is identical to existing value(s)");
            }

            changeRequestItemStagings = changeRequestItemStagings ?? new List<ChangeRequestItemStaging>();

            await ValidateModelHasNoChangeRequest(entity);
            await ValidateVehicleTypeHasNoChangeRequest(entity);
            await ValidateModelLookUpHasNoChangeRequest(entity);

            entity.InsertDate = modelFromDb.InsertDate;

            // to eliminate circular reference during serialize,
            var existingEntity = new Model()
            {
                Id = modelFromDb.Id,
                Name = modelFromDb.Name,
                VehicleTypeId = modelFromDb.VehicleTypeId,
                VehicleType = new VehicleType()
               {
                  Name = modelFromDb.VehicleType.Name
               },
               
                ChangeRequestId = modelFromDb.ChangeRequestId,
                VehicleCount = modelFromDb.VehicleCount,
                BaseVehicleCount = modelFromDb.BaseVehicleCount,
                DeleteDate = modelFromDb.DeleteDate,
                InsertDate = modelFromDb.InsertDate,
                LastUpdateDate = modelFromDb.LastUpdateDate
            };

            changeRequestItemStagings.Add(new ChangeRequestItemStaging()
            {
                ChangeType = ChangeType.Modify,
                EntityId = entity.Id.ToString(),
                CreatedDateTime = DateTime.UtcNow,
                Entity = typeof(Model).Name,
                Payload = base.Serializer.Serialize(entity),
                ExistingPayload = base.Serializer.Serialize(existingEntity)
            });

            changeContent = string.Format("{0} > {1}"
                , modelFromDb.Name
                , entity.Name);

            // NOTE: change-request-comments-staging perfomed on base

            var changeRequestId = await base.SubmitUpdateChangeRequestAsync(entity, id, requestedBy, ChangeType.Modify, changeRequestItemStagings, comment, attachments, changeContent);

            modelFromDb.ChangeRequestId = changeRequestId;
            _modelRepositoryService.Update(modelFromDb);
            Repositories.SaveChanges();
            return changeRequestId;
        }

        public override async Task<long> SubmitDeleteChangeRequestAsync<TId>(Model entity, TId id, string requestedBy, List<ChangeRequestItemStaging> changeRequestItemStagings = null, CommentsStagingModel comment = null, List<AttachmentsModel> attachmentsStaging = null, string changeContent = null)
        {
            if (id.Equals(default(TId)))
            {
                throw new ArgumentNullException(nameof(id));
            }

            var modelFromDb = await FindAsync(id);
            if (modelFromDb == null)
            {
                throw new NoRecordFound("No Brake ABS exist");
            }

            changeRequestItemStagings = changeRequestItemStagings ?? new List<ChangeRequestItemStaging>();

            await ValidateModelLookUpHasNoChangeRequest(modelFromDb);

            changeRequestItemStagings.Add(new ChangeRequestItemStaging()
            {
                ChangeType = ChangeType.Delete,
                EntityId = modelFromDb.Id.ToString(),
                CreatedDateTime = DateTime.UtcNow,
                Entity = typeof(Model).Name,

                Payload = base.Serializer.Serialize(new Model()
                {
                    Id = modelFromDb.Id,
                    Name = modelFromDb.Name,
                    VehicleTypeId=modelFromDb.VehicleTypeId,
                })
            });

            changeContent = string.Format("{0}"
                , entity.Name);

            var changeRequestId = await base.SubmitDeleteChangeRequestAsync(modelFromDb, id, requestedBy, changeRequestItemStagings,comment, attachmentsStaging, changeContent);

            modelFromDb.ChangeRequestId = changeRequestId;
            _modelRepositoryService.Update(modelFromDb);
            Repositories.SaveChanges();

            return changeRequestId;
        }

        public override AssociationCount AssociatedCount(string payload)
        {
            AssociationCount count = new AssociationCount();
            if (payload != null)
            {
                Model deserializedEntry = Serializer.Deserialize<Model>(payload);
                count.BaseVehicleCount = deserializedEntry.BaseVehicleCount;
                count.VehicleCount = deserializedEntry.VehicleCount;
            }
            return count;
        }

        public override async Task ClearChangeRequestId(long changeRequestId)
        {
            var entities = await _modelRepositoryService.GetAsync(m => m.ChangeRequestId == changeRequestId && m.DeleteDate == null);

            if (entities != null && entities.Any())
            {
                var entity = entities.First();   //can only be a single item
                entity.ChangeRequestId = null;
                _modelRepositoryService.Update(entity);
                Repositories.SaveChanges();
            }
        }

        public override async Task<ChangeRequestStagingModel<Model>> GetChangeRequestStaging<TId>(TId changeRequestId)
        {
            ChangeRequestStagingModel<Model> staging = await
                ChangeRequestBusinessService.GetChangeRequestStagingByChangeRequestIdAsync<Model, TId>(changeRequestId);

            // business specific task.
            // fill value of "EntityCurrent"
            //if (!staging.StagingItem.ChangeType.Equals(ChangeType.Add.ToString(), StringComparison.CurrentCultureIgnoreCase))
            //{
            //    Model currentModel = await FindAsync(staging.StagingItem.EntityId);
            //    staging.EntityCurrent = currentModel;
            //}

            return staging;
        }

        private async Task ValidateModelHasNoChangeRequest(Model model)
        {
            IList<Model> modelsFromDb = await _modelRepositoryService.GetAsync(x => x.Name.ToLower().Equals(model.Name.ToLower(), StringComparison.CurrentCultureIgnoreCase) && x.VehicleTypeId==model.VehicleTypeId && x.DeleteDate == null);

            if (modelsFromDb != null && modelsFromDb.Any())
            {
                throw new RecordAlreadyExist("Model: " + model.Name + " already exists");
            }
            var changerequestid =
                await
                    ChangeRequestBusinessService.ChangeRequestExist<Model>(
                        x => x.Name.ToLower().Equals(model.Name.ToLower()) && x.VehicleTypeId==model.VehicleTypeId);
            if(changerequestid>0)
            {
                throw new ChangeRequestExistException("There is already an open CR ID "+ changerequestid + " with  same Model Name.");
            }
        }

        private async Task ValidateVehicleTypeHasNoChangeRequest(Model model)
        {
            var changerequestid =
                await ChangeRequestBusinessService.ChangeRequestExist(typeof (VehicleType).Name, model.VehicleTypeId);
            if (changerequestid>0)
            {
                throw new RecordAlreadyExist("Selected Vehicle Type exists in CR with CR ID "+ changerequestid + ".");
            }
         }

        private async Task ValidateModelLookUpHasNoChangeRequest(Model model)
        {
            var changeRequestId = await
                ChangeRequestBusinessService.ChangeRequestExist<BaseVehicle>(x => x.ModelId.Equals(model.Id));
            if (changeRequestId >0)
               
            {
               throw new ChangeRequestExistException($"There is already an open CR ID {changeRequestId} for base vehicle using this model.");
            }
        }

        /// <summary>
    /// Find a single record which is not marked as deleted.
    /// </summary>
    /// <typeparam name="TKey">any</typeparam>
    /// <param name="id">the key property</param>
    /// <returns>Single entity object if condition matches, null if not found.</returns>
        private async Task<Model> FindAsync<TKey>(TKey id)
        {
            var entityId = Convert.ToInt32(id);
            var entities = await _modelRepositoryService.GetAsync(m => m.Id == entityId && m.DeleteDate == null, 1);
            if (entities != null && entities.Any())
            {
                return entities.First();
            }
            return null;
        }
    }
}

