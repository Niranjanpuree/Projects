using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AutoCare.Product.Application.RepositoryServices;
using AutoCare.Product.Infrastructure.ExceptionHandler;
using AutoCare.Product.Infrastructure.RepositoryService;
using AutoCare.Product.Infrastructure.Serializer;
using AutoCare.Product.Vcdb.Model;
using System.Collections.Generic;
using AutoCare.Product.Application.Infrastructure.ExceptionHelpers;
using AutoCare.Product.Application.Models.BusinessModels;

namespace AutoCare.Product.Application.BusinessServices.Vcdb
{
    public class VehicleTypeBusinessService : VcdbBusinessService<VehicleType>, IVehicleTypeBusinessService
    {
        private readonly ISqlServerEfRepositoryService<VehicleType> _vehicleTypeRepositoryService = null;

        public VehicleTypeBusinessService(IVcdbUnitOfWork vcdbRepositoryService,
            IVcdbChangeRequestBusinessService vcdbChangeRequestBusinessService,
            ITextSerializer serializer)
            : base(vcdbRepositoryService, vcdbChangeRequestBusinessService, serializer)
        {
            _vehicleTypeRepositoryService = Repositories.GetRepositoryService<VehicleType>() as ISqlServerEfRepositoryService<VehicleType>;
        }

        public override async Task<List<VehicleType>> GetAllAsync(int topCount = 0)
        {
            return await _vehicleTypeRepositoryService.GetAsync(m => m.DeleteDate == null, topCount);
        }

        public override async Task<VehicleType> GetAsync<TKey>(TKey id)
        {
            if (id.Equals(default(TKey)))
            {
                throw new ArgumentNullException(nameof(id));
            }

            var vehicleType = await FindAsync(id);

            if (vehicleType == null)
            {
                throw new NoRecordFound("Record Not Found");
            }

            vehicleType.ModelCount = await _vehicleTypeRepositoryService.GetCountAsync(vehicleType, x => x.Models, y => y.DeleteDate == null);

            var vehicleTypeId = Convert.ToInt32(id);
            vehicleType.BaseVehicleCount = _vehicleTypeRepositoryService.GetAllQueryable()
                .Where(x => x.Id == vehicleTypeId).Include("Models.BaseVehicles")
                .SelectMany(x => x.Models)
                .SelectMany(x => x.BaseVehicles).Count(y => y.DeleteDate == null);

            vehicleType.VehicleCount = _vehicleTypeRepositoryService.GetAllQueryable()
                .Where(x => x.Id == vehicleTypeId).Include("Models.BaseVehicles.Vehicles")
                .SelectMany(x => x.Models)
                .SelectMany(x => x.BaseVehicles)
                .SelectMany(x => x.Vehicles).Count(y => y.DeleteDate == null);

            return vehicleType;
        }

        public override async Task<long> SubmitAddChangeRequestAsync(VehicleType entity, string requestedBy, 
            List<ChangeRequestItemStaging> changeRequestItemStagings = null, CommentsStagingModel comment = null,
            List<AttachmentsModel> attachmentsStagingModels = null, string changeContent = null)
        {
            if (String.IsNullOrWhiteSpace(entity?.Name))
            {
                throw new ArgumentNullException(nameof(entity));
            }

            changeRequestItemStagings = changeRequestItemStagings ?? new List<ChangeRequestItemStaging>();

            await ValidateVehicleTypeHasNoChangeRequest(entity);
            await ValidateVehicleGroupTypeHasNoChangeRequest(entity);

            changeRequestItemStagings.Add(new ChangeRequestItemStaging()
            {
                ChangeType = ChangeType.Add,
                EntityId = entity.Id.ToString(),
                CreatedDateTime = DateTime.UtcNow,
                Entity = typeof(VehicleType).Name,
                Payload = base.Serializer.Serialize(entity)
            });

            changeContent = string.Format("{0}"
                , entity.Name);

            // NOTE: change-request-comments-staging perfomed on base

            return await base.SubmitAddChangeRequestAsync(entity, requestedBy, changeRequestItemStagings, comment, attachmentsStagingModels, changeContent);
        }

        public override async Task<long> SubmitUpdateChangeRequestAsync<TId>(VehicleType entity, TId id, string requestedBy, 
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
                throw new ArgumentException("Invalid Vehicle Type Id");
            }

            var vehicleTypeFromDb = await FindAsync(id);
            if (vehicleTypeFromDb == null)
            {
                throw new NoRecordFound("No Vehicle Type exist");
            }
            changeRequestItemStagings = changeRequestItemStagings ?? new List<ChangeRequestItemStaging>();
            await ValidateVehicleTypeHasNoChangeRequest(entity);
            await ValidateVehicleGroupTypeHasNoChangeRequest(entity);
            await ValidateVehicleTypeLookUpHasNoChangeRequest(entity);

            entity.InsertDate = vehicleTypeFromDb.InsertDate;

            var existingEntity = new VehicleType()
            {
                Id = vehicleTypeFromDb.Id,
                Name =  vehicleTypeFromDb.Name,
                VehicleTypeGroupId = vehicleTypeFromDb.VehicleTypeGroupId,
                VehicleTypeGroup = new VehicleTypeGroup()
                {
                   Name = vehicleTypeFromDb.VehicleTypeGroup.Name
                },
                DeleteDate = vehicleTypeFromDb.DeleteDate,
                InsertDate = vehicleTypeFromDb.InsertDate,
                LastUpdateDate = vehicleTypeFromDb.LastUpdateDate
            };

            changeRequestItemStagings.Add(new ChangeRequestItemStaging()
            {
                ChangeType = ChangeType.Modify,
                EntityId = entity.Id.ToString(),
                CreatedDateTime = DateTime.UtcNow,
                Entity = typeof(VehicleType).Name,
                Payload = base.Serializer.Serialize(entity),
                ExistingPayload = base.Serializer.Serialize(existingEntity)
            });

            changeContent = string.Format("{0} > {1}"
                , vehicleTypeFromDb.Name
                , entity.Name);

            var changeRequestId = await base.SubmitUpdateChangeRequestAsync(entity, id, requestedBy, ChangeType.Modify, changeRequestItemStagings, comment, attachments, changeContent);

            vehicleTypeFromDb.ChangeRequestId = changeRequestId;
            _vehicleTypeRepositoryService.Update(vehicleTypeFromDb);
            Repositories.SaveChanges();
            return changeRequestId;
        }

        public override async Task<long> SubmitDeleteChangeRequestAsync<TId>(VehicleType entity, TId id, string requestedBy, List<ChangeRequestItemStaging> changeRequestItemStagings = null,CommentsStagingModel comment=null, List<AttachmentsModel> attachmentsStaging = null, string changeContent = null)
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

            await ValidateVehicleGroupTypeHasNoChangeRequest(modelFromDb);
            await ValidateVehicleTypeLookUpHasNoChangeRequest(modelFromDb);

            changeRequestItemStagings.Add(new ChangeRequestItemStaging()
            {
                ChangeType = ChangeType.Delete,
                EntityId = modelFromDb.Id.ToString(),
                CreatedDateTime = DateTime.UtcNow,
                Entity = typeof(VehicleType).Name,

                Payload = base.Serializer.Serialize(new VehicleType()
                {
                    Id = modelFromDb.Id,
                    Name = modelFromDb.Name,
                    VehicleTypeGroupId = modelFromDb.VehicleTypeGroupId
                })
            });

            changeContent = string.Format("{0}"
                , entity.Name);

            var changeRequestId = await base.SubmitDeleteChangeRequestAsync(modelFromDb, id, requestedBy, changeRequestItemStagings, comment, attachmentsStaging, changeContent);

            modelFromDb.ChangeRequestId = changeRequestId;
            _vehicleTypeRepositoryService.Update(modelFromDb);
            Repositories.SaveChanges();

            return changeRequestId;
        }

        public override async Task<ChangeRequestStagingModel<VehicleType>> GetChangeRequestStaging<TId>(TId changeRequestId)
        {
            ChangeRequestStagingModel<VehicleType> staging = await
                ChangeRequestBusinessService.GetChangeRequestStagingByChangeRequestIdAsync<VehicleType, TId>(changeRequestId);

            // business specific task.
            // fill value of "EntityCurrent"
            //if (!staging.StagingItem.ChangeType.Equals(ChangeType.Add.ToString(), StringComparison.CurrentCultureIgnoreCase))
            //{
            //    VehicleType currentVehicleTypeModel = await FindAsync(staging.StagingItem.EntityId);
            //    staging.EntityCurrent = currentVehicleTypeModel;
            //}

            return staging;
        }

        private async Task ValidateVehicleTypeHasNoChangeRequest(VehicleType vehicleType)
        {
            IList<VehicleType> vehicleTypesFromDb = await _vehicleTypeRepositoryService.GetAsync(x => x.Name.ToLower().Equals(vehicleType.Name.ToLower())&&x.VehicleTypeGroupId==vehicleType.VehicleTypeGroupId && x.DeleteDate == null);

            if (vehicleTypesFromDb != null && vehicleTypesFromDb.Any())
            {
                throw new RecordAlreadyExist("Vehicle Type already exists");
            }

            var changerequestid =
                await
                    ChangeRequestBusinessService.ChangeRequestExist<VehicleType>(
                        x => x.Name.ToLower().Equals(vehicleType.Name.ToLower()));
            if(changerequestid>0)
            {
                throw new ChangeRequestExistException($"There is already an open CR ID {changerequestid} with same Vehicle Type Name.");
            }
        }

        private async Task ValidateVehicleGroupTypeHasNoChangeRequest(VehicleType vehicleType)
        {
            var changerequestid =
                await
                    ChangeRequestBusinessService.ChangeRequestExist(typeof (VehicleTypeGroup).Name,
                        vehicleType.VehicleTypeGroupId);
            if(changerequestid>0)
            {
                throw new RecordAlreadyExist("Selected Vehicle Type Group exists in CR with CR ID "+ changerequestid + ".");
            }

        }

        private async Task ValidateVehicleTypeLookUpHasNoChangeRequest(VehicleType vehicleType)
        {
            var changerequestid =
                await
                    ChangeRequestBusinessService.ChangeRequestExist<Model>(x => x.VehicleTypeId.Equals(vehicleType.Id));

            if(changerequestid>0){
                throw new ChangeRequestExistException($"There is already an open CR ID {changerequestid} for base Model using this Vehicle Type.");
            }
        }

        public override AssociationCount AssociatedCount(string payload)
        {
            AssociationCount count = new AssociationCount();
            if (payload != null)
            {
                VehicleType deserializedEntry = Serializer.Deserialize<VehicleType>(payload);
                count.ModelCount = deserializedEntry.ModelCount;
                count.BaseVehicleCount = deserializedEntry.BaseVehicleCount;
                count.VehicleCount = deserializedEntry.VehicleCount;
            }
            return count;
        }

        public override async Task ClearChangeRequestId(long changeRequestId)
        {
            var entities = await _vehicleTypeRepositoryService.GetAsync(m => m.ChangeRequestId == changeRequestId && m.DeleteDate == null);

            if (entities != null && entities.Any())
            {
                var entity = entities.First();   //can only be a single item
                entity.ChangeRequestId = null;
                _vehicleTypeRepositoryService.Update(entity);
                Repositories.SaveChanges();
            }
        }

        /// <summary>
        /// Find a single record which is not marked as deleted.
        /// </summary>
        /// <typeparam name="TKey">any</typeparam>
        /// <param name="id">the key property</param>
        /// <returns>Single entity object if condition matches, null if not found.</returns>
        private async Task<VehicleType> FindAsync<TKey>(TKey id)
        {
            var entityId = Convert.ToInt32(id);
            var entities = await _vehicleTypeRepositoryService.GetAsync(m => m.Id == entityId && m.DeleteDate == null, 1);
            if (entities != null && entities.Any())
            {
                return entities.First();
            }
            return null;
        }
    }
}
