using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AutoCare.Product.Application.Infrastructure.ExceptionHelpers;
using AutoCare.Product.Application.Models.BusinessModels;
using AutoCare.Product.Application.RepositoryServices;
using AutoCare.Product.Infrastructure.ExceptionHandler;
using AutoCare.Product.Infrastructure.RepositoryService;
using AutoCare.Product.Infrastructure.Serializer;
using AutoCare.Product.Vcdb.Model;
using AutoCare.Product.VcdbSearch.Indexing.ApplicationService;

namespace AutoCare.Product.Application.BusinessServices.Vcdb
{
    public class VehicleBusinessService : VcdbBusinessService<Vehicle>, IVehicleBusinessService
    {
        private readonly IRepositoryService<Vehicle> _vehicleRepositoryService = null;
        private readonly IVehicleIndexingService _vehicleIndexingService = null;

        public VehicleBusinessService(IVcdbUnitOfWork vcdbRepositoryService,
            IVcdbChangeRequestBusinessService vcdbChangeRequestBusinessService,
            ITextSerializer serializer, IVehicleIndexingService vehicleIndexingService)
            : base(vcdbRepositoryService, vcdbChangeRequestBusinessService, serializer)
        {
            _vehicleRepositoryService = Repositories.GetRepositoryService<Vehicle>();
            _vehicleIndexingService = vehicleIndexingService;
        }

        public override async Task<Vehicle> GetAsync<TKey>(TKey id)
        {
            if (id.Equals(default(TKey)))
            {
                throw new ArgumentNullException(nameof(id));
            }

            var vehicle = await FindAsync(id);

            if (vehicle == null)
            {
                throw new NoRecordFound("Record Not Found");
            }

            vehicle.VehicleToBrakeConfigCount = await _vehicleRepositoryService.GetCountAsync(vehicle, x => x.VehicleToBrakeConfigs, y => y.DeleteDate == null);
            vehicle.VehicleToBedConfigCount = await _vehicleRepositoryService.GetCountAsync(vehicle, x => x.VehicleToBedConfigs, y => y.DeleteDate == null);
            vehicle.VehicleToBodyStyleConfigCount = await _vehicleRepositoryService.GetCountAsync(vehicle, x => x.VehicleToBodyStyleConfigs, y => y.DeleteDate == null);
            vehicle.VehicleToWheelBaseCount = await _vehicleRepositoryService.GetCountAsync(vehicle, x => x.VehicleToWheelBases, y => y.DeleteDate == null);
            vehicle.VehicleToDriveTypeCount = await _vehicleRepositoryService.GetCountAsync(vehicle, x => x.VehicleToDriveTypes, y => y.DeleteDate == null);
            vehicle.VehicleToMfrBodyCodeCount = await _vehicleRepositoryService.GetCountAsync(vehicle, x => x.VehicleToMfrBodyCodes, y => y.DeleteDate == null);
            return vehicle;
        }

        public override async Task<long> SubmitUpdateChangeRequestAsync<TId>(Vehicle entity, TId id, string requestedBy,
            ChangeType changeType = ChangeType.None,
            List<ChangeRequestItemStaging> changeRequestItemStagings = null, CommentsStagingModel comment = null, List<AttachmentsModel> attachments = null, string changeContent = null)
        {
            if (entity.SubModelId.Equals(default(int))
                || entity.RegionId.Equals(default(int)))
            {
                throw new ArgumentException(nameof(entity));
            }

            if (id.Equals(default(TId)))
            {
                throw new ArgumentNullException(nameof(id));
            }

            if (!entity.Id.Equals(id))
            {
                throw new ArgumentException("Invalid Vehicle Id");
            }

            var vehicleFromDb = await FindAsync(id);
            if (vehicleFromDb == null)
            {
                throw new NoRecordFound("No Vehicle exist");
            }

            var existingEntry = new Vehicle()
            {
                Id = vehicleFromDb.Id,
                BaseVehicleId = vehicleFromDb.BaseVehicleId,
                SubModelId = vehicleFromDb.SubModelId,
                SourceName = vehicleFromDb.SourceName,
                SourceId = vehicleFromDb.SourceId,
                RegionId = vehicleFromDb.RegionId,
                PublicationStageId = vehicleFromDb.PublicationStageId,
                PublicationStageSource = vehicleFromDb.PublicationStageSource,
                PublicationStageDate = vehicleFromDb.PublicationStageDate,
                PublicationEnvironment = vehicleFromDb.PublicationEnvironment,
                ChangeRequestId = vehicleFromDb.ChangeRequestId,
                InsertDate = vehicleFromDb.InsertDate,
                DeleteDate = vehicleFromDb.DeleteDate,
                LastUpdateDate = vehicleFromDb.LastUpdateDate,
                VehicleToBrakeConfigCount = vehicleFromDb.VehicleToBrakeConfigCount,
                VehicleToBedConfigCount = vehicleFromDb.VehicleToBedConfigCount,
                VehicleToBodyStyleConfigCount = vehicleFromDb.VehicleToBodyStyleConfigCount
            };             

            changeType = vehicleFromDb.BaseVehicleId == entity.BaseVehicleId ? ChangeType.Modify : ChangeType.Replace;
            changeRequestItemStagings = changeRequestItemStagings ?? new List<ChangeRequestItemStaging>();

            // Validation check for insert of new vehicle
            await ValidateVehicleHasNoChangeRequest(entity);
            await ValidateVehicleLookupsHasNoChangeRequest(entity);
            await ValidateBaseVehicleHasNoChangeRequest(entity);
            // 5. validate there is no open CR for vehicle to brake system configuration
            // brake system configuration
            var changerequestid = await
                ChangeRequestBusinessService.ChangeRequestExist<VehicleToBrakeConfig>(
                    x => x.VehicleId == entity.Id);
            if (changerequestid > 0)
            {
                throw new ChangeRequestExistException($"There is already an open CR ID {changerequestid} for Vehicle to Brake System Configuration.");
            }
            // 6. validate there is no open CR to replace vehicle that rely on this vehicle
            {
                // NOTE: No vehicle replacement functionality implemented, yet.
            }
            //<Raja>: I don't see the difference between the above base vehicle validation and below base vehicle validation
            // 7. Validate that there are no open CR to the existing base vehicle.
            // Applicable only during Base vehicle replace
            if (changeType.Equals(ChangeType.Replace))
            {
                await ValidateBaseVehicleHasNoChangeRequest(vehicleFromDb);
            }

            //Fill in the existing values to avoid being overwritten when final approve in change review screen.
            entity.InsertDate = vehicleFromDb.InsertDate;
            entity.PublicationEnvironment = vehicleFromDb.PublicationEnvironment;
            entity.PublicationStageId = vehicleFromDb.PublicationStageId;
            entity.PublicationStageDate = vehicleFromDb.PublicationStageDate;
            entity.PublicationStageSource = vehicleFromDb.PublicationStageSource;
            entity.SourceId = vehicleFromDb.SourceId;
            entity.SourceName = vehicleFromDb.SourceName;
            changeRequestItemStagings.Add(new ChangeRequestItemStaging()
            {
                ChangeType = changeType,
                EntityId = entity.Id.ToString(),
                CreatedDateTime = DateTime.UtcNow,
                Entity = typeof(Vehicle).Name,
                Payload = base.Serializer.Serialize(entity),
                ExistingPayload = base.Serializer.Serialize(existingEntry)
            });

            changeContent = string.Format("{0} / {1} / {2} / {3} / {4}"
                   , vehicleFromDb.BaseVehicle.YearId
                   , vehicleFromDb.BaseVehicle.Make.Name
                   , vehicleFromDb.BaseVehicle.Model.Name
                   , vehicleFromDb.SubModel.Name
                   , vehicleFromDb.Region.Name);

            // NOTE: change-request-comments-staging perfomed on base

            var changeRequestId = await base.SubmitUpdateChangeRequestAsync(entity, id, requestedBy, changeType, changeRequestItemStagings, comment, attachments, changeContent);

            vehicleFromDb.ChangeRequestId = changeRequestId;
            _vehicleRepositoryService.Update(vehicleFromDb);
            Repositories.SaveChanges();

            await _vehicleIndexingService.UpdateVehicleChangeRequestIdAsync(entity.Id, changeRequestId);

            return changeRequestId;
        }

        public override async Task<long> SubmitAddChangeRequestAsync(Vehicle entity, string requestedBy,
            List<ChangeRequestItemStaging> changeRequestItemStagings = null, CommentsStagingModel comment = null,
            List<AttachmentsModel> attachmentsStagingModels = null, string changeContent = null)
        {
            if (entity.SubModelId.Equals(default(int)) || entity.RegionId.Equals(default(int)))
            {
                throw new ArgumentException(nameof(entity));
            }

            if (entity.BaseVehicleId.Equals(default(int)))
            {

                throw new ArgumentException("Invalid BaseVehicle Id ");
            }

            changeRequestItemStagings = changeRequestItemStagings ?? new List<ChangeRequestItemStaging>();
            // Validation check for insert of new vehicle

            await ValidateVehicleHasNoChangeRequest(entity);

            var baseVehicleRepositoryService = Repositories.GetRepositoryService<BaseVehicle>() as IVcdbSqlServerEfRepositoryService<BaseVehicle>;
            BaseVehicle baseVehicle = null;
            if (baseVehicleRepositoryService != null)
            {
                var baseVehicles = await baseVehicleRepositoryService.GetAsync(m => m.Id == entity.BaseVehicleId && m.DeleteDate == null, 1);
                if (baseVehicles != null && baseVehicles.Any())
                {
                    baseVehicle = baseVehicles.First();
                }
                await ValidateBaseVehicleLookupsHasNoChangeRequest(baseVehicle);
            }

            await ValidateVehicleLookupsHasNoChangeRequest(entity);
            await ValidateBaseVehicleHasNoChangeRequest(entity);

            //NOTE: Using Change Request Item Business Service's AddAsync function instead of creating new object, since validation is checked there.

            //NOTE: save default values
            entity.PublicationEnvironment = "";
            entity.PublicationStageId = 1;
            entity.PublicationStageDate = DateTime.UtcNow;
            entity.PublicationStageSource = "";
            entity.SourceId = 1;
            entity.SourceName = null;

            changeRequestItemStagings.Add(new ChangeRequestItemStaging()
            {
                ChangeType = ChangeType.Add,
                EntityId = entity.Id.ToString(),
                CreatedDateTime = DateTime.UtcNow,
                Entity = typeof(Vehicle).Name,
                Payload = base.Serializer.Serialize(entity)
            });
            //changeRequestItemStagings.Add(await base.ChangeRequestItemBusinessService.AddAsync(entity, entity.Id, requestedBy, ChangeType.Add));

            var subModelRepositoryService = Repositories.GetRepositoryService<SubModel>() as IVcdbSqlServerEfRepositoryService<SubModel>;
            var regionRepositoryService = Repositories.GetRepositoryService<Region>() as IVcdbSqlServerEfRepositoryService<Region>;

            SubModel subModel = null;
            Region region = null;

            if (baseVehicle != null && subModelRepositoryService != null && regionRepositoryService != null)
            {
                var subModels = await subModelRepositoryService.GetAsync(m => m.Id == entity.SubModelId && m.DeleteDate == null, 1);
                if (subModels != null && subModels.Any())
                {
                    subModel = subModels.First();
                }
                var regions = await regionRepositoryService.GetAsync(m => m.Id == entity.RegionId && m.DeleteDate == null, 1);
                if (regions != null && regions.Any())
                {
                    region = regions.First();
                }

                changeContent = string.Format("{0} / {1} / {2} / {3} / {4}"
                , baseVehicle.YearId
                , baseVehicle.Make.Name
                , baseVehicle.Model.Name
                , subModel.Name
                , region.Name);
            }
            // NOTE: change-request-comments-staging perfomed on base

            var changeRequestId = await base.SubmitAddChangeRequestAsync(entity, requestedBy, changeRequestItemStagings, comment, attachmentsStagingModels, changeContent);

            return changeRequestId;
        }

        private async Task ValidateBaseVehicleLookupsHasNoChangeRequest(BaseVehicle baseVehicle)
        {
            long changerequestid = 0;
            //make
            changerequestid =
                  await ChangeRequestBusinessService.ChangeRequestExist(typeof(Make).Name, baseVehicle.MakeId);

            if (changerequestid > 0)
            {
                throw new ChangeRequestExistException(
                    $"There is already an open CR ID {changerequestid} for Make Name : {baseVehicle.Make.Name}.");
            }
            //Model
            changerequestid =
               await ChangeRequestBusinessService.ChangeRequestExist(typeof(Model).Name, baseVehicle.ModelId);
            if (changerequestid > 0)
            {
                {
                    throw new ChangeRequestExistException(
                        $"There is already an open CR ID {changerequestid} for Model Name: {baseVehicle.Model.Name}.");
                }
            }
        }

        private async Task ValidateVehicleLookupsHasNoChangeRequest(Vehicle entity)
        {
            long changerequestID = 0;
            // submodel
            changerequestID =
               await ChangeRequestBusinessService.ChangeRequestExist(typeof(SubModel).Name, entity.SubModelId);
            if (changerequestID > 0)
            {
                throw new ChangeRequestExistException(
                    $"There is already an open CR ID {changerequestID} for Submodel ID : {entity.SubModelId}.");
            }
            // region
            changerequestID =
                await ChangeRequestBusinessService.ChangeRequestExist(typeof(Region).Name, entity.RegionId);

            if (changerequestID > 0)
            {
                throw new ChangeRequestExistException(
                    $"There is already an open CR ID {changerequestID} for Region ID : {entity.RegionId}.");
            }
        }

        private async Task ValidateBaseVehicleHasNoChangeRequest(Vehicle entity)
        {
            var changerequestid = await ChangeRequestBusinessService.ChangeRequestExist(typeof(BaseVehicle).Name,
                entity.BaseVehicleId);
            if (changerequestid > 0)
            {
                throw new ChangeRequestExistException(
                    $"There is already an open CR ID {changerequestid} for Base Vehicle ID : {entity.BaseVehicleId}.");
            }
        }

        private async Task ValidateVehicleHasNoChangeRequest(Vehicle entity)
        {
            // 1. Validate that there is no vehicle with same configuration
            IList<Vehicle> existingVehicle =
                await _vehicleRepositoryService.GetAsync(item => item.BaseVehicleId == entity.Id
                && item.SubModelId == entity.SubModelId
                && item.RegionId == entity.RegionId
                && item.DeleteDate == null);

            if (existingVehicle != null && existingVehicle.Any())
            {
                throw new RecordAlreadyExist("Vehicle with same configuration already exists");
            }

            // 2. Validate that there is no open CR with same configuration
            // vehicle
            var changerequestid = await
                ChangeRequestBusinessService.ChangeRequestExist<Vehicle>(
                    x =>
                        x.BaseVehicleId == entity.BaseVehicleId && x.SubModelId == entity.SubModelId &&
                        x.RegionId == entity.RegionId);

            if (changerequestid > 0)
            {
                throw new ChangeRequestExistException($"There is already an open CR ID {changerequestid} with same vehicle configuration.");
            }
        }

        public override async Task<long> SubmitDeleteChangeRequestAsync<TId>(Vehicle entity, TId id, string requestedBy, List<ChangeRequestItemStaging> changeRequestItemStagings = null, CommentsStagingModel comment = null, List<AttachmentsModel> attachmentsStaging = null, string changeContent = null)
        {
            if (id.Equals(default(TId)))
            {
                throw new ArgumentNullException(nameof(id));
            }

            Vehicle vehicleFromDb = await FindAsync(id);
            if (vehicleFromDb == null)
            {
                throw new NoRecordFound("No Vehicle exist");
            }
            // Serialize issue: To prevent cyclic reference.
            Vehicle currentVehicle = new Vehicle()
            {
                Id = vehicleFromDb.Id,
                BaseVehicleId = vehicleFromDb.BaseVehicleId,
                SubModelId = vehicleFromDb.SubModelId,
                SourceId = vehicleFromDb.SourceId,
                RegionId = vehicleFromDb.RegionId
            };

            if (currentVehicle.BaseVehicleId.Equals(default(int)))
            {
                throw new ArgumentException(nameof(currentVehicle.BaseVehicle));
            }

            if (!currentVehicle.Id.Equals(id))
            {
                throw new ArgumentException("Invalid Vehicle to Brake Config Id");
            }

            changeRequestItemStagings = changeRequestItemStagings ?? new List<ChangeRequestItemStaging>();
            // validation check for delete of vehicle
            try
            {
                long changerequestid = 0;
                StringBuilder validationErrorMessage = new StringBuilder();
                // 1. Validate that there is no open CR with same configuration
                changerequestid = await
                     ChangeRequestBusinessService.ChangeRequestExist<Vehicle>(
                         x =>
                             x.BaseVehicleId == vehicleFromDb.BaseVehicleId && x.SubModelId == vehicleFromDb.SubModelId &&
                             x.RegionId == vehicleFromDb.RegionId);
                if (changerequestid > 0)
                {
                    throw new ChangeRequestExistException($"There is already an open CR ID {changerequestid} with same vehicle configuration.");
                }

                // 2. Validate that there are no open CR to base vehicle.
                await ValidateBaseVehicleHasNoChangeRequest(currentVehicle);

                changeRequestItemStagings.Add(new ChangeRequestItemStaging()
                {
                    ChangeType = ChangeType.Delete,
                    EntityId = currentVehicle.Id.ToString(),
                    CreatedDateTime = DateTime.UtcNow,
                    Entity = typeof(Vehicle).Name,
                    Payload = base.Serializer.Serialize(currentVehicle)
                });

                // 3. validate there is no open CR for vehicle to brake system configuration
                changerequestid = await
                   ChangeRequestBusinessService.ChangeRequestExist<VehicleToBrakeConfig>(
                       x =>
                           x.VehicleId == currentVehicle.Id);

                if (changerequestid > 0)
                {
                    throw new ChangeRequestExistException($"There is already an open CR Id {changerequestid} for Vehicle to Brake System Configuration.");
                }

                //4. Validate there is no existing record with the same configuration
                IList<VehicleToBrakeConfig> existingVehicleToBrakeConfigs =
                            await
                                base.Repositories.GetRepositoryService<VehicleToBrakeConfig>()
                                    .GetAsync(item => item.VehicleId.Equals(currentVehicle.Id) && item.DeleteDate == null);
                if (existingVehicleToBrakeConfigs != null && existingVehicleToBrakeConfigs.Count > 0)
                {
                    changeRequestItemStagings.AddRange(existingVehicleToBrakeConfigs.Select(vehicleToBrakeConfig => new ChangeRequestItemStaging()
                    {
                        ChangeType = ChangeType.Delete,
                        EntityId = vehicleToBrakeConfig.Id.ToString(),
                        CreatedDateTime = DateTime.UtcNow,
                        Entity = typeof(VehicleToBrakeConfig).Name,
                        Payload = base.Serializer.Serialize(new VehicleToBrakeConfig()
                        {
                            Id = vehicleToBrakeConfig.Id,
                            BrakeConfigId = vehicleToBrakeConfig.BrakeConfigId,
                            VehicleId = vehicleToBrakeConfig.VehicleId
                        })
                    }));
                }

                // 4. validate there is no open CR to replace vehicle that rely on this vehicle
                {
                    // NOTE: No vehicle replacement functionality implemented, yet.
                }

                // throw custom validation failed exception
                if (validationErrorMessage.Length > 0)
                {
                    throw new Exception(validationErrorMessage.ToString());
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error occured when validating.\n" + ex.Message);
            }

            changeContent = string.Format("{0} / {1} / {2} / {3} / {4}"
                   , vehicleFromDb.BaseVehicle.YearId
                   , vehicleFromDb.BaseVehicle.Make.Name
                   , vehicleFromDb.BaseVehicle.Model.Name
                   , vehicleFromDb.SubModel.Name
                   , vehicleFromDb.Region.Name);

            var changeRequestId = await base.SubmitDeleteChangeRequestAsync(currentVehicle, id, requestedBy, changeRequestItemStagings, comment, attachmentsStaging, changeContent);

            vehicleFromDb.ChangeRequestId = changeRequestId;
            _vehicleRepositoryService.Update(vehicleFromDb);
            Repositories.SaveChanges();

            await _vehicleIndexingService.UpdateVehicleChangeRequestIdAsync(currentVehicle.Id, changeRequestId);

            return changeRequestId;
        }

        public override async Task<ChangeRequestStagingModel<Vehicle>> GetChangeRequestStaging<TId>(TId changeRequestId)
        {
            ChangeRequestStagingModel<Vehicle> staging = await
                ChangeRequestBusinessService.GetChangeRequestStagingByChangeRequestIdAsync<Vehicle, TId>(changeRequestId);

            return staging;
        }

        public override AssociationCount AssociatedCount(string payload)
        {
            AssociationCount count = new AssociationCount();
            if (payload != null)
            {
                Vehicle deserializedEntry = Serializer.Deserialize<Vehicle>(payload);
                count.VehicleToBrakeConfigCount = deserializedEntry.VehicleToBrakeConfigCount;
                count.VehicleToBedConfigCount = deserializedEntry.VehicleToBedConfigCount;
                count.VehicleToBodyStyleConfigCount = deserializedEntry.VehicleToBodyStyleConfigCount;
            }
            return count;
        }

        public override async Task ClearChangeRequestId(long changeRequestId)
        {
            var entities = await _vehicleRepositoryService.GetAsync(m => m.ChangeRequestId == changeRequestId && m.DeleteDate == null);

            if (entities != null && entities.Any())
            {
                var entity = entities.First();   //can only be a single item
                entity.ChangeRequestId = null;
                _vehicleRepositoryService.Update(entity);
                Repositories.SaveChanges();
            }
        }

        public override async Task<List<Vehicle>> GetPendingAddChangeRequests(Expression<Func<Vehicle, bool>> whereCondition = null, int topCount = 0)
        {
            var existingChangeRequestItemStagings = await ChangeRequestBusinessService.GetChangeRequestItemStagingsAsync(
                x => x.Entity.Equals(typeof(Vehicle).Name, StringComparison.CurrentCultureIgnoreCase)
                && x.ChangeType == ChangeType.Add);

            List<Vehicle> pendingVehicles = new List<Vehicle>();

            foreach (var existingChangeRequestItemStaging in existingChangeRequestItemStagings)
            {
                var pendingVehicle = Serializer.Deserialize<Vehicle>(existingChangeRequestItemStaging.Payload);
                pendingVehicle.ChangeRequestId = existingChangeRequestItemStaging.ChangeRequestId;
                pendingVehicles.Add(pendingVehicle);
            }

            if (whereCondition != null)
            {
                var predicate = whereCondition.Compile();

                pendingVehicles = pendingVehicles.Where(predicate).ToList();
            }

            return pendingVehicles;
        }

        /// <summary>
        /// Find a single record which is not marked as deleted.
        /// </summary>
        /// <typeparam name="TKey">any</typeparam>
        /// <param name="id">the key property</param>
        /// <returns>Single entity object if condition matches, null if not found.</returns>
        private async Task<Vehicle> FindAsync<TKey>(TKey id)
        {
            var entityId = Convert.ToInt32(id);
            var entities = await _vehicleRepositoryService.GetAsync(m => m.Id == entityId && m.DeleteDate == null, 1);
            if (entities != null && entities.Any())
            {
                return entities.First();
            }
            return null;
        }
    }
}
