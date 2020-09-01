using System;
using System.Threading.Tasks;
using AutoCare.Product.Application.RepositoryServices;
using AutoCare.Product.Infrastructure.ExceptionHandler;
using AutoCare.Product.Infrastructure.RepositoryService;
using AutoCare.Product.Infrastructure.Serializer;
using AutoCare.Product.Vcdb.Model;
using System.Text;
using System.Collections.Generic;
using System.Linq.Expressions;
using AutoCare.Product.Application.Models.BusinessModels;
using AutoCare.Product.Application.Infrastructure.ExceptionHelpers;
using System.Linq;
using AutoCare.Product.VcdbSearch.Indexing.ApplicationService;

namespace AutoCare.Product.Application.BusinessServices.Vcdb
{
    public class VehicleToBrakeConfigBusinessService : VcdbBusinessService<VehicleToBrakeConfig>, IVehicleToBrakeConfigBusinessService
    {
        private readonly IRepositoryService<VehicleToBrakeConfig> _vehicleToBrakeConfigRepositoryService = null;
        private readonly IVehicleToBrakeConfigIndexingService _vehicleToBrakeConfigIndexingService = null;
        public VehicleToBrakeConfigBusinessService(IVcdbUnitOfWork vcdbRepositoryService,
            IVcdbChangeRequestBusinessService vcdbChangeRequestBusinessService,
            ITextSerializer serializer, IVehicleToBrakeConfigIndexingService vehicleToBrakeConfigIndexingService = null)
            : base(vcdbRepositoryService, vcdbChangeRequestBusinessService, serializer)
        {
            _vehicleToBrakeConfigRepositoryService = Repositories.GetRepositoryService<VehicleToBrakeConfig>();
            _vehicleToBrakeConfigIndexingService = vehicleToBrakeConfigIndexingService;
        }

        public override async Task<long> SubmitAddChangeRequestAsync(VehicleToBrakeConfig entity, string requestedBy,
            List<ChangeRequestItemStaging> changeRequestItemStagings = null, CommentsStagingModel comment = null,
            List<AttachmentsModel> attachmentsStagingModels = null, string changeContent = null)
        {
            if (entity.VehicleId.Equals(default(int))
                || entity.BrakeConfigId.Equals(default(int)))
            {
                throw new ArgumentException(nameof(entity));
            }

            changeRequestItemStagings = changeRequestItemStagings ?? new List<ChangeRequestItemStaging>();
            // Validation check for insert of new vehicle to brake config

            StringBuilder validationErrorMessage = new StringBuilder();
            await ValidateVehicleToBrakeConfigHasNoChangeReqeuest(entity);
            await ValidateVehicleToBrakeConfigLookUpHasNoChangeRequest(entity);

            changeRequestItemStagings.Add(new ChangeRequestItemStaging()
            {
                ChangeType = ChangeType.Add,
                EntityId = entity.Id.ToString(),
                CreatedDateTime = DateTime.UtcNow,
                Entity = typeof(VehicleToBrakeConfig).Name,
                Payload = base.Serializer.Serialize(entity)
            });

            var vehicleRepositoryService = Repositories.GetRepositoryService<Vehicle>() as IVcdbSqlServerEfRepositoryService<Vehicle>;
            var brakeConfigRepositoryService = Repositories.GetRepositoryService<BrakeConfig>() as IVcdbSqlServerEfRepositoryService<BrakeConfig>;

            Vehicle vehicle = null;
            BrakeConfig brakeConfig = null;

            if (vehicleRepositoryService != null && brakeConfigRepositoryService != null)
            {
                var vehicles = await vehicleRepositoryService.GetAsync(m => m.Id == entity.VehicleId && m.DeleteDate == null, 1);
                if (vehicles != null && vehicles.Any())
                {
                    vehicle = vehicles.First();
                }

                var brakeConfigs = await brakeConfigRepositoryService.GetAsync(m => m.Id == entity.BrakeConfigId && m.DeleteDate == null, 1);
                if (brakeConfigs != null && brakeConfigs.Any())
                {
                    brakeConfig = brakeConfigs.First();
                }
            }

            changeContent = string.Format("{0} / {1} / {2} / {3} / {4} => \n{5} / {6} / {7} / {8}"
                , vehicle.BaseVehicle.YearId
                , vehicle.BaseVehicle.Make.Name
                , vehicle.BaseVehicle.Model.Name
                , vehicle.SubModel.Name
                , vehicle.Region.Name
                , brakeConfig.FrontBrakeType.Name
                , brakeConfig.RearBrakeType.Name
                , brakeConfig.BrakeABS.Name
                , brakeConfig.BrakeSystem.Name);

            // NOTE: change-request-comments-staging perfomed on base

            return await base.SubmitAddChangeRequestAsync(entity, requestedBy, changeRequestItemStagings, comment, attachmentsStagingModels, changeContent);
        }

        public override async Task<long> SubmitUpdateChangeRequestAsync<TId>(VehicleToBrakeConfig entity, TId id, string requestedBy,
            ChangeType changeType = ChangeType.None,
            List<ChangeRequestItemStaging> changeRequestItemStagings = null, CommentsStagingModel comment = null, List<AttachmentsModel> attachments = null, string changeContent = null)
        {
            //Note: Existing Payload in not required as there is no Modify Method
            throw new Exception("This method should not be invoked");
        }

        public override async Task<long> SubmitDeleteChangeRequestAsync<TId>(VehicleToBrakeConfig entity, TId id, string requestedBy, List<ChangeRequestItemStaging> changeRequestItemStagings = null, CommentsStagingModel comment = null, List<AttachmentsModel> attachmentsStaging = null, string changeContent = null)
        {
            if (id.Equals(default(TId)))
            {
                throw new ArgumentNullException(nameof(id));
            }

            var vehicleToBrakeConfigFromDb = await FindAsync(id);
            if (vehicleToBrakeConfigFromDb == null)
            {
                throw new NoRecordFound("No Vehicle to Brake Config exist");
            }

            var vehicleToBrakeConfigSubmit = new VehicleToBrakeConfig()
            {
                Id = vehicleToBrakeConfigFromDb.Id,
                VehicleId = vehicleToBrakeConfigFromDb.VehicleId,
                BrakeConfigId = vehicleToBrakeConfigFromDb.BrakeConfigId
            };

            changeRequestItemStagings = changeRequestItemStagings ?? new List<ChangeRequestItemStaging>();

            await ValidateVehicleToBrakeConfigLookUpHasNoChangeRequest(vehicleToBrakeConfigFromDb);

            changeRequestItemStagings.Add(new ChangeRequestItemStaging()
            {
                ChangeType = ChangeType.Delete,
                EntityId = vehicleToBrakeConfigFromDb.Id.ToString(),
                CreatedDateTime = DateTime.UtcNow,
                Entity = typeof(VehicleToBrakeConfig).Name,
                // Payload = base.Serializer.Serialize(vehicleToBrakeConfigFromDb)
                Payload = base.Serializer.Serialize(new VehicleToBrakeConfig
                {
                    VehicleId = vehicleToBrakeConfigFromDb.VehicleId,
                    BrakeConfigId = vehicleToBrakeConfigFromDb.BrakeConfigId,
                    Id = vehicleToBrakeConfigFromDb.Id,
                })
            });

            changeContent = string.Format("{0} / {1} / {2} / {3} / {4} => \n{5} / {6} / {7} / {8}"
                , vehicleToBrakeConfigFromDb.Vehicle.BaseVehicle.YearId
                , vehicleToBrakeConfigFromDb.Vehicle.BaseVehicle.Make.Name
                , vehicleToBrakeConfigFromDb.Vehicle.BaseVehicle.Model.Name
                , vehicleToBrakeConfigFromDb.Vehicle.SubModel.Name
                , vehicleToBrakeConfigFromDb.Vehicle.Region.Name
                , vehicleToBrakeConfigFromDb.BrakeConfig.FrontBrakeType.Name
                , vehicleToBrakeConfigFromDb.BrakeConfig.RearBrakeType.Name
                , vehicleToBrakeConfigFromDb.BrakeConfig.BrakeABS.Name
                , vehicleToBrakeConfigFromDb.BrakeConfig.BrakeSystem.Name);

            var changeRequestId = await base.SubmitDeleteChangeRequestAsync(vehicleToBrakeConfigSubmit, id, requestedBy, changeRequestItemStagings, comment, attachmentsStaging, changeContent);

            vehicleToBrakeConfigFromDb.ChangeRequestId = changeRequestId;
            _vehicleToBrakeConfigRepositoryService.Update(vehicleToBrakeConfigFromDb);
            Repositories.SaveChanges();

            await _vehicleToBrakeConfigIndexingService.UpdateVehicleToBrakeConfigChangeRequestIdAsync(vehicleToBrakeConfigFromDb.Id, changeRequestId);

            return changeRequestId;
        }

        public override async Task<ChangeRequestStagingModel<VehicleToBrakeConfig>> GetChangeRequestStaging<TId>(
           TId changeRequestId)
        {
            ChangeRequestStagingModel<VehicleToBrakeConfig> staging =
                await
                    ChangeRequestBusinessService.GetChangeRequestStagingByChangeRequestIdAsync<VehicleToBrakeConfig, TId>(
                        changeRequestId);
            //if (
            //    !staging.StagingItem.ChangeType.Equals(ChangeType.Add.ToString(),
            //        StringComparison.CurrentCultureIgnoreCase))
            //{
            //    VehicleToBrakeConfig currentVehicleToBrakeConfig = await FindAsync(staging.StagingItem.EntityId);
            //    staging.EntityCurrent = currentVehicleToBrakeConfig;
            //}
            return staging;
        }

        public override async Task<List<VehicleToBrakeConfig>> GetAsync(Expression<Func<VehicleToBrakeConfig, bool>> whereCondition, int topCount = 0)
        {
            var result = await _vehicleToBrakeConfigRepositoryService
                .GetAsync(whereCondition, 100000,
                    "BrakeConfig.BrakeABS",
                    "BrakeConfig.BrakeSystem",
                    "BrakeConfig.FrontBrakeType",
                    "BrakeConfig.RearBrakeType",
                    "Vehicle.BaseVehicle.Make",
                    "Vehicle.BaseVehicle.Model",
                    "Vehicle.BaseVehicle.Year",
                    "Vehicle.SubModel");

            return result.Where(x => x.DeleteDate == null).ToList();
        }

        private async Task ValidateVehicleToBrakeConfigHasNoChangeReqeuest(VehicleToBrakeConfig vehicleToBrakeConfig)
        {
            // 1. Validate that there is no vehicle to brake config with same configuration
            IList<VehicleToBrakeConfig> vehicleToBrakeConfigFromDb =
                await _vehicleToBrakeConfigRepositoryService.GetAsync(x => x.VehicleId.Equals(vehicleToBrakeConfig.VehicleId)
                        && x.BrakeConfigId.Equals(vehicleToBrakeConfig.BrakeConfigId)
                        && x.DeleteDate == null);

            if (vehicleToBrakeConfigFromDb != null && vehicleToBrakeConfigFromDb.Count > 0)
            {
                throw new RecordAlreadyExist("Vehicle to Brake Config with same configuration already exists.");
            }

            // 2. Validate that there is no open CR with same configuration

            var ChangeRequestId = await
             ChangeRequestBusinessService.ChangeRequestExist<VehicleToBrakeConfig>(
                 x => x.VehicleId.Equals(vehicleToBrakeConfig.VehicleId)
                      && x.BrakeConfigId.Equals(vehicleToBrakeConfig.BrakeConfigId));

            if (ChangeRequestId > 0)
            {
                throw new ChangeRequestExistException($"There is already an open CR ID {ChangeRequestId} with same vehicle to brake configuration.");
            }
        }

        private async Task ValidateVehicleToBrakeConfigLookUpHasNoChangeRequest(
            VehicleToBrakeConfig vehicleToBrakeConfig)
        {
            var changerrequestid =
                await
                    ChangeRequestBusinessService.ChangeRequestExist(typeof(Vehicle).Name,
                        vehicleToBrakeConfig.VehicleId);

            if (changerrequestid > 0)
            {
                throw new ChangeRequestExistException($"There is already an open CR ID {changerrequestid} for Vehicle Id : {vehicleToBrakeConfig.VehicleId}.");
            }

            var changerequestID =
                await
                    ChangeRequestBusinessService.ChangeRequestExist(typeof(BrakeConfig).Name,
                        vehicleToBrakeConfig.BrakeConfigId);
            if (changerequestID > 0)
            {
                throw new ChangeRequestExistException($"There is already an open CR ID {changerequestID} for Brake Config ID : {vehicleToBrakeConfig.BrakeConfigId}.");
            }
        }

        public override AssociationCount AssociatedCount(string payload)
        {
            AssociationCount count = new AssociationCount();
            return count;
        }

        public override async Task ClearChangeRequestId(long changeRequestId)
        {
            var entities = await _vehicleToBrakeConfigRepositoryService.GetAsync(m => m.ChangeRequestId == changeRequestId && m.DeleteDate == null);

            if (entities != null && entities.Any())
            {
                var entity = entities.First();   //can only be a single item
                entity.ChangeRequestId = null;
                _vehicleToBrakeConfigRepositoryService.Update(entity);
                Repositories.SaveChanges();
            }
        }

        /// <summary>
        /// Find a single record which is not marked as deleted.
        /// </summary>
        /// <typeparam name="TKey">any</typeparam>
        /// <param name="id">the key property</param>
        /// <returns>Single entity object if condition matches, null if not found.</returns>
        private async Task<VehicleToBrakeConfig> FindAsync<TKey>(TKey id)
        {
            var entityId = Convert.ToInt32(id);
            var entities = await _vehicleToBrakeConfigRepositoryService.GetAsync(m => m.Id == entityId && m.DeleteDate == null, 1);
            if (entities != null && entities.Any())
            {
                return entities.First();
            }
            return null;
        }
    }
}
