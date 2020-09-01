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
    public class VehicleToBedConfigBusinessService : VcdbBusinessService<VehicleToBedConfig>, IVehicleToBedConfigBusinessService
    {
        private readonly IRepositoryService<VehicleToBedConfig> _vehicleToBedConfigRepositoryService = null;
        private readonly IVehicleToBedConfigIndexingService _vehicleToBedConfigIndexingService = null;
        public VehicleToBedConfigBusinessService(IVcdbUnitOfWork vcdbRepositoryService,
            IVcdbChangeRequestBusinessService vcdbChangeRequestBusinessService
            ,
            ITextSerializer serializer,
            IVehicleToBedConfigIndexingService vehicleToBedConfigIndexingService = null
            )
            : base(vcdbRepositoryService, vcdbChangeRequestBusinessService, serializer)
        {
            _vehicleToBedConfigRepositoryService = Repositories.GetRepositoryService<VehicleToBedConfig>();
            _vehicleToBedConfigIndexingService = vehicleToBedConfigIndexingService;
        }

        public override async Task<long> SubmitAddChangeRequestAsync(VehicleToBedConfig entity, string requestedBy,
            List<ChangeRequestItemStaging> changeRequestItemStagings = null, CommentsStagingModel comment = null,
            List<AttachmentsModel> attachmentsStagingModels = null, string changeContent = null)
        {
            if (entity.VehicleId.Equals(default(int))
                || entity.BedConfigId.Equals(default(int)))
            {
                throw new ArgumentException(nameof(entity));
            }

            changeRequestItemStagings = changeRequestItemStagings ?? new List<ChangeRequestItemStaging>();
            // Validation check for insert of new vehicle to Bed config

            StringBuilder validationErrorMessage = new StringBuilder();
            await ValidateVehicleToBedConfigHasNoChangeReqeuest(entity);
            await ValidateVehicleToBedConfigLookUpHasNoChangeRequest(entity);

            changeRequestItemStagings.Add(new ChangeRequestItemStaging()
            {
                ChangeType = ChangeType.Add,
                EntityId = entity.Id.ToString(),
                CreatedDateTime = DateTime.UtcNow,
                Entity = typeof(VehicleToBedConfig).Name,
                Payload = base.Serializer.Serialize(entity)
            });

            var vehicleRepositoryService = Repositories.GetRepositoryService<Vehicle>() as IVcdbSqlServerEfRepositoryService<Vehicle>;
            var bedConfigRepositoryService = Repositories.GetRepositoryService<BedConfig>() as IVcdbSqlServerEfRepositoryService<BedConfig>;

            Vehicle vehicle = null;
            BedConfig bedConfig = null;

            if (vehicleRepositoryService != null && bedConfigRepositoryService != null)
            {
                var vehicles = await vehicleRepositoryService.GetAsync(m => m.Id == entity.VehicleId && m.DeleteDate == null, 1);
                if (vehicles != null && vehicles.Any())
                {
                    vehicle = vehicles.First();
                }

                var bedConfigs = await bedConfigRepositoryService.GetAsync(m => m.Id == entity.BedConfigId && m.DeleteDate == null, 1);
                if (bedConfigs != null && bedConfigs.Any())
                {
                    bedConfig = bedConfigs.First();
                }
            }

            changeContent = string.Format("{0} / {1} / {2} / {3} / {4} => \n{5} / {6} / {7}"
                , vehicle.BaseVehicle.YearId
                , vehicle.BaseVehicle.Make.Name
                , vehicle.BaseVehicle.Model.Name
                , vehicle.SubModel.Name
                , vehicle.Region.Name
                , bedConfig.BedLength.Length
                , bedConfig.BedLength.BedLengthMetric
                , bedConfig.BedType.Name);

            // NOTE: change-request-comments-staging perfomed on base

            return await base.SubmitAddChangeRequestAsync(entity, requestedBy, changeRequestItemStagings, comment, attachmentsStagingModels, changeContent);
        }

        public override async Task<long> SubmitUpdateChangeRequestAsync<TId>(VehicleToBedConfig entity, TId id, string requestedBy,
            ChangeType changeType = ChangeType.None,
            List<ChangeRequestItemStaging> changeRequestItemStagings = null, CommentsStagingModel comment = null, List<AttachmentsModel> attachments = null, string changeContent = null)
        {
            //Note: Existing Payload in not required as there is no Modify Method
            throw new Exception("This method should not be invoked");
        }

        public override async Task<long> SubmitDeleteChangeRequestAsync<TId>(VehicleToBedConfig entity, TId id, string requestedBy, List<ChangeRequestItemStaging> changeRequestItemStagings = null, CommentsStagingModel comment = null, List<AttachmentsModel> attachmentsStaging = null, string changeContent = null)
        {
            if (id.Equals(default(TId)))
            {
                throw new ArgumentNullException(nameof(id));
            }

            var vehicleToBedConfigFromDb = await FindAsync(id);
            if (vehicleToBedConfigFromDb == null)
            {
                throw new NoRecordFound("No Vehicle to Bed Config exist");
            }

            var vehicleToBedConfigSubmit = new VehicleToBedConfig()
            {
                Id = vehicleToBedConfigFromDb.Id,
                VehicleId = vehicleToBedConfigFromDb.VehicleId,
                BedConfigId = vehicleToBedConfigFromDb.BedConfigId
            };

            changeRequestItemStagings = changeRequestItemStagings ?? new List<ChangeRequestItemStaging>();

            await ValidateVehicleToBedConfigLookUpHasNoChangeRequest(vehicleToBedConfigFromDb);

            changeRequestItemStagings.Add(new ChangeRequestItemStaging()
            {
                ChangeType = ChangeType.Delete,
                EntityId = vehicleToBedConfigFromDb.Id.ToString(),
                CreatedDateTime = DateTime.UtcNow,
                Entity = typeof(VehicleToBedConfig).Name,
                // Payload = base.Serializer.Serialize(vehicleToBedConfigFromDb)
                Payload = base.Serializer.Serialize(new VehicleToBedConfig
                {
                    VehicleId = vehicleToBedConfigFromDb.VehicleId,
                    BedConfigId = vehicleToBedConfigFromDb.BedConfigId,
                    Id = vehicleToBedConfigFromDb.Id,
                })
            });

            changeContent = string.Format("{0} / {1} / {2} / {3} / {4} => \n{5} / {6} / {7}"
                , vehicleToBedConfigFromDb.Vehicle.BaseVehicle.YearId
                , vehicleToBedConfigFromDb.Vehicle.BaseVehicle.Make.Name
                , vehicleToBedConfigFromDb.Vehicle.BaseVehicle.Model.Name
                , vehicleToBedConfigFromDb.Vehicle.SubModel.Name
                , vehicleToBedConfigFromDb.Vehicle.Region.Name
                , vehicleToBedConfigFromDb.BedConfig.BedLength.Length
                , vehicleToBedConfigFromDb.BedConfig.BedLength.BedLengthMetric
                , vehicleToBedConfigFromDb.BedConfig.BedType.Name);

            var changeRequestId = await base.SubmitDeleteChangeRequestAsync(vehicleToBedConfigSubmit, id, requestedBy, changeRequestItemStagings, comment, attachmentsStaging, changeContent);

            vehicleToBedConfigFromDb.ChangeRequestId = changeRequestId;
            _vehicleToBedConfigRepositoryService.Update(vehicleToBedConfigFromDb);
            Repositories.SaveChanges();

            await _vehicleToBedConfigIndexingService.UpdateVehicleToBedConfigChangeRequestIdAsync(vehicleToBedConfigFromDb.Id, changeRequestId);

            return changeRequestId;
        }

        public override async Task<ChangeRequestStagingModel<VehicleToBedConfig>> GetChangeRequestStaging<TId>(
           TId changeRequestId)
        {
            ChangeRequestStagingModel<VehicleToBedConfig> staging =
                await
                    ChangeRequestBusinessService.GetChangeRequestStagingByChangeRequestIdAsync<VehicleToBedConfig, TId>(
                        changeRequestId);
            //if (
            //    !staging.StagingItem.ChangeType.Equals(ChangeType.Add.ToString(),
            //        StringComparison.CurrentCultureIgnoreCase))
            //{
            //    VehicleToBedConfig currentVehicleToBedConfig = await FindAsync(staging.StagingItem.EntityId);
            //    staging.EntityCurrent = currentVehicleToBedConfig;
            //}
            return staging;
        }

        public override async Task<List<VehicleToBedConfig>> GetAsync(Expression<Func<VehicleToBedConfig, bool>> whereCondition, int topCount = 0)
        {
            var result = await _vehicleToBedConfigRepositoryService
                .GetAsync(whereCondition, 100000,
                    "BedConfig.BedLength",
                    "BedConfig.BedType",
                    "Vehicle.BaseVehicle.Make",
                    "Vehicle.BaseVehicle.Model",
                    "Vehicle.BaseVehicle.Year",
                    "Vehicle.SubModel");

            return result.Where(x => x.DeleteDate == null).ToList();
        }

        private async Task ValidateVehicleToBedConfigHasNoChangeReqeuest(VehicleToBedConfig vehicleToBedConfig)
        {
            // 1. Validate that there is no vehicle to Bed config with same configuration
            IList<VehicleToBedConfig> vehicleToBedConfigFromDb =
                await _vehicleToBedConfigRepositoryService.GetAsync(x => x.VehicleId.Equals(vehicleToBedConfig.VehicleId)
                        && x.BedConfigId.Equals(vehicleToBedConfig.BedConfigId)
                        && x.DeleteDate == null);

            if (vehicleToBedConfigFromDb != null && vehicleToBedConfigFromDb.Count > 0)
            {
                throw new RecordAlreadyExist("Vehicle to Bed Config with same configuration already exists.");
            }

            // 2. Validate that there is no open CR with same configuration

            var ChangeRequestId = await
             ChangeRequestBusinessService.ChangeRequestExist<VehicleToBedConfig>(
                 x => x.VehicleId.Equals(vehicleToBedConfig.VehicleId)
                      && x.BedConfigId.Equals(vehicleToBedConfig.BedConfigId));

            if (ChangeRequestId > 0)
            {
                throw new ChangeRequestExistException($"There is already an open CR ID {ChangeRequestId} with same vehicle to Bed configuration.");
            }
        }

        private async Task ValidateVehicleToBedConfigLookUpHasNoChangeRequest(
            VehicleToBedConfig vehicleToBedConfig)
        {
            var changerrequestid =
                await
                    ChangeRequestBusinessService.ChangeRequestExist(typeof(Vehicle).Name,
                        vehicleToBedConfig.VehicleId);

            if (changerrequestid > 0)
            {
                throw new ChangeRequestExistException($"There is already an open CR ID {changerrequestid} for Vehicle ID : {vehicleToBedConfig.VehicleId}.");
            }

            var changerequestID =
                await
                    ChangeRequestBusinessService.ChangeRequestExist(typeof(BedConfig).Name,
                        vehicleToBedConfig.BedConfigId);
            if (changerequestID > 0)
            {
                throw new ChangeRequestExistException($"There is already an open CR ID {changerequestID} for Bed Config ID : {vehicleToBedConfig.BedConfigId}.");
            }
        }

        public override AssociationCount AssociatedCount(string payload)
        {
            AssociationCount count = new AssociationCount();
            return count;
        }

        public override async Task ClearChangeRequestId(long changeRequestId)
        {
            var entities = await _vehicleToBedConfigRepositoryService.GetAsync(m => m.ChangeRequestId == changeRequestId && m.DeleteDate == null);

            if (entities != null && entities.Any())
            {
                var entity = entities.First();   //can only be a single item
                entity.ChangeRequestId = null;
                _vehicleToBedConfigRepositoryService.Update(entity);
                Repositories.SaveChanges();
            }
        }

        /// <summary>
        /// Find a single record which is not marked as deleted.
        /// </summary>
        /// <typeparam name="TKey">any</typeparam>
        /// <param name="id">the key property</param>
        /// <returns>Single entity object if condition matches, null if not found.</returns>
        private async Task<VehicleToBedConfig> FindAsync<TKey>(TKey id)
        {
            var entityId = Convert.ToInt32(id);
            var entities = await _vehicleToBedConfigRepositoryService.GetAsync(m => m.Id == entityId && m.DeleteDate == null, 1);
            if (entities != null && entities.Any())
            {
                return entities.First();
            }
            return null;
        }
    }
}
