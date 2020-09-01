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
    public class VehicleToBodyStyleConfigBusinessService : VcdbBusinessService<VehicleToBodyStyleConfig>, IVehicleToBodyStyleConfigBusinessService
    {
        private readonly IRepositoryService<VehicleToBodyStyleConfig> _vehicleToBodyConfigRepositoryService = null;
        private readonly IVehicleToBodyStyleConfigIndexingService _vehicleToBodyStyleConfigIndexingService = null;
        public VehicleToBodyStyleConfigBusinessService(IVcdbUnitOfWork vcdbRepositoryService,
            IVcdbChangeRequestBusinessService vcdbChangeRequestBusinessService
            ,
            ITextSerializer serializer,
            IVehicleToBodyStyleConfigIndexingService vehicleToBodyConfigConfigIndexingService = null
            )
            : base(vcdbRepositoryService, vcdbChangeRequestBusinessService, serializer)
        {
            _vehicleToBodyConfigRepositoryService = Repositories.GetRepositoryService<VehicleToBodyStyleConfig>();
            _vehicleToBodyStyleConfigIndexingService = vehicleToBodyConfigConfigIndexingService;
        }

        public override async Task<long> SubmitAddChangeRequestAsync(VehicleToBodyStyleConfig entity, string requestedBy,
            List<ChangeRequestItemStaging> changeRequestItemStagings = null, CommentsStagingModel comment = null,
            List<AttachmentsModel> attachmentsStagingModels = null, string changeContent = null)
        {
            if (entity.VehicleId.Equals(default(int))
                || entity.BodyStyleConfigId.Equals(default(int)))
            {
                throw new ArgumentException(nameof(entity));
            }

            changeRequestItemStagings = changeRequestItemStagings ?? new List<ChangeRequestItemStaging>();
            // Validation check for insert of new vehicle to Bed config
            StringBuilder validationErrorMessage = new StringBuilder();
            await ValidateVehicleToBodyConfigHasNoChangeReqeuest(entity);
            await ValidateVehicleToBodyConfigLookUpHasNoChangeRequest(entity);

            changeRequestItemStagings.Add(new ChangeRequestItemStaging()
            {
                ChangeType = ChangeType.Add,
                EntityId = entity.Id.ToString(),
                CreatedDateTime = DateTime.UtcNow,
                Entity = typeof(VehicleToBodyStyleConfig).Name,
                Payload = base.Serializer.Serialize(entity)
            });

            var vehicleRepositoryService = Repositories.GetRepositoryService<Vehicle>() as IVcdbSqlServerEfRepositoryService<Vehicle>;
            var bodyStyleConfigRepositoryService = Repositories.GetRepositoryService<BodyStyleConfig>() as IVcdbSqlServerEfRepositoryService<BodyStyleConfig>;

            Vehicle vehicle = null;
            BodyStyleConfig bodyStyleConfig = null;

            if (vehicleRepositoryService != null && bodyStyleConfigRepositoryService != null)
            {
                var vehicles = await vehicleRepositoryService.GetAsync(m => m.Id == entity.VehicleId && m.DeleteDate == null, 1);
                if (vehicles != null && vehicles.Any())
                {
                    vehicle = vehicles.First();
                }

                var bodyStyleConfigs = await bodyStyleConfigRepositoryService.GetAsync(m => m.Id == entity.BodyStyleConfigId && m.DeleteDate == null, 1);
                if (bodyStyleConfigs != null && bodyStyleConfigs.Any())
                {
                    bodyStyleConfig = bodyStyleConfigs.First();
                }
            }

            changeContent = string.Format("{0} / {1} / {2} / {3} / {4} => \n{5} / {6}"
                , vehicle.BaseVehicle.YearId
                , vehicle.BaseVehicle.Make.Name
                , vehicle.BaseVehicle.Model.Name
                , vehicle.SubModel.Name
                , vehicle.Region.Name
                , bodyStyleConfig.BodyNumDoors.NumDoors
                , bodyStyleConfig.BodyType.Name);

            // NOTE: change-request-comments-staging perfomed on base

            return await base.SubmitAddChangeRequestAsync(entity, requestedBy, changeRequestItemStagings, comment, attachmentsStagingModels, changeContent);
        }

        //this method is not invoked since there is no update for Vehicle To Body config
        public override async Task<long> SubmitUpdateChangeRequestAsync<TId>(VehicleToBodyStyleConfig entity, TId id, string requestedBy,
            ChangeType changeType = ChangeType.None,
            List<ChangeRequestItemStaging> changeRequestItemStagings = null, CommentsStagingModel comment = null, List<AttachmentsModel> attachments = null, string changeContent = null)
        {
            //Note: Existing Payload in not required as there is no Modify Method
            throw new Exception("This method should not be invoked");
        }

        public override async Task<long> SubmitDeleteChangeRequestAsync<TId>(VehicleToBodyStyleConfig entity, TId id, string requestedBy, List<ChangeRequestItemStaging> changeRequestItemStagings = null, CommentsStagingModel comment = null, List<AttachmentsModel> attachmentsStaging = null, string changeContent = null)
        {
            if (id.Equals(default(TId)))
            {
                throw new ArgumentNullException(nameof(id));
            }

            var vehicleToBodyConfigFromDb = await FindAsync(id);
            if (vehicleToBodyConfigFromDb == null)
            {
                throw new NoRecordFound("No Vehicle to Body Config exist");
            }

            var vehicleToBodyConfigSubmit = new VehicleToBodyStyleConfig()
            {
                Id = vehicleToBodyConfigFromDb.Id,
                VehicleId = vehicleToBodyConfigFromDb.VehicleId,
                BodyStyleConfigId = vehicleToBodyConfigFromDb.BodyStyleConfigId
            };

            changeRequestItemStagings = changeRequestItemStagings ?? new List<ChangeRequestItemStaging>();

            await ValidateVehicleToBodyConfigLookUpHasNoChangeRequest(vehicleToBodyConfigFromDb);

            changeRequestItemStagings.Add(new ChangeRequestItemStaging()
            {
                ChangeType = ChangeType.Delete,
                EntityId = vehicleToBodyConfigFromDb.Id.ToString(),
                CreatedDateTime = DateTime.UtcNow,
                Entity = typeof(VehicleToBodyStyleConfig).Name,
                // Payload = base.Serializer.Serialize(vehicleToBedConfigFromDb)
                Payload = base.Serializer.Serialize(new VehicleToBodyStyleConfig
                {
                    VehicleId = vehicleToBodyConfigFromDb.VehicleId,
                    BodyStyleConfigId = vehicleToBodyConfigFromDb.BodyStyleConfigId,
                    Id = vehicleToBodyConfigFromDb.Id,
                })
            });

            changeContent = string.Format("{0} / {1} / {2} / {3} / {4} => \n{5} / {6}"
                , vehicleToBodyConfigFromDb.Vehicle.BaseVehicle.YearId
                , vehicleToBodyConfigFromDb.Vehicle.BaseVehicle.Make.Name
                , vehicleToBodyConfigFromDb.Vehicle.BaseVehicle.Model.Name
                , vehicleToBodyConfigFromDb.Vehicle.SubModel.Name
                , vehicleToBodyConfigFromDb.Vehicle.Region.Name
                , vehicleToBodyConfigFromDb.BodyStyleConfig.BodyNumDoors.NumDoors
                , vehicleToBodyConfigFromDb.BodyStyleConfig.BodyType.Name);

            var changeRequestId = await base.SubmitDeleteChangeRequestAsync(vehicleToBodyConfigSubmit, id, requestedBy, changeRequestItemStagings, comment, attachmentsStaging, changeContent);

            vehicleToBodyConfigFromDb.ChangeRequestId = changeRequestId;
            _vehicleToBodyConfigRepositoryService.Update(vehicleToBodyConfigFromDb);
            Repositories.SaveChanges();

            await _vehicleToBodyStyleConfigIndexingService.UpdateVehicleToBodyStyleConfigChangeRequestIdAsync(vehicleToBodyConfigFromDb.Id, changeRequestId);

            return changeRequestId;
        }

        public override async Task<ChangeRequestStagingModel<VehicleToBodyStyleConfig>> GetChangeRequestStaging<TId>(
           TId changeRequestId)
        {
            ChangeRequestStagingModel<VehicleToBodyStyleConfig> staging =
                await
                    ChangeRequestBusinessService.GetChangeRequestStagingByChangeRequestIdAsync<VehicleToBodyStyleConfig, TId>(
                        changeRequestId);
            return staging;
        }

        public override async Task<List<VehicleToBodyStyleConfig>> GetAsync(Expression<Func<VehicleToBodyStyleConfig, bool>> whereCondition, int topCount = 0)
        {
            var result = await _vehicleToBodyConfigRepositoryService
                .GetAsync(whereCondition, 100000,
                    "BodyStyleConfig.BodyType",
                    "BodyStyleConfig.BodyNumDoors",
                    "Vehicle.BaseVehicle.Make",
                    "Vehicle.BaseVehicle.Model",
                    "Vehicle.BaseVehicle.Year",
                    "Vehicle.SubModel");

            return result.Where(x => x.DeleteDate == null).ToList();
        }

        private async Task ValidateVehicleToBodyConfigHasNoChangeReqeuest(VehicleToBodyStyleConfig vehicleToBodyConfig)
        {
            // 1. Validate that there is no vehicle to Bed config with same configuration
            IList<VehicleToBodyStyleConfig> vehicleToBodyConfigFromDb =
                await _vehicleToBodyConfigRepositoryService.GetAsync(x => x.VehicleId.Equals(vehicleToBodyConfig.VehicleId)
                        && x.BodyStyleConfigId.Equals(vehicleToBodyConfig.BodyStyleConfigId)
                        && x.DeleteDate == null);

            if (vehicleToBodyConfigFromDb != null && vehicleToBodyConfigFromDb.Count > 0)
            {
                throw new RecordAlreadyExist("Vehicle to Body Config with same configuration already exists.");
            }

            // 2. Validate that there is no open CR with same configuration

            var ChangeRequestId = await
             ChangeRequestBusinessService.ChangeRequestExist<VehicleToBodyStyleConfig>(
                 x => x.VehicleId.Equals(vehicleToBodyConfig.VehicleId)
                      && x.BodyStyleConfigId.Equals(vehicleToBodyConfig.BodyStyleConfigId));

            if (ChangeRequestId > 0)
            {
                throw new ChangeRequestExistException($"There is already an open CR ID {ChangeRequestId} with same vehicle to Body configuration.");
            }
        }

        private async Task ValidateVehicleToBodyConfigLookUpHasNoChangeRequest(
            VehicleToBodyStyleConfig vehicleToBodyConfig)
        {
            var changerrequestid =
                await
                    ChangeRequestBusinessService.ChangeRequestExist(typeof(Vehicle).Name,
                        vehicleToBodyConfig.VehicleId);

            if (changerrequestid > 0)
            {
                throw new ChangeRequestExistException($"There is already an open CR ID {changerrequestid} for Vehicle ID : {vehicleToBodyConfig.VehicleId}.");
            }

            var changerequestID =
                await
                    ChangeRequestBusinessService.ChangeRequestExist(typeof(BodyStyleConfig).Name,
                        vehicleToBodyConfig.BodyStyleConfigId);
            if (changerequestID > 0)
            {
                throw new ChangeRequestExistException($"There is already an open CR ID {changerequestID} for Body Style Config ID : {vehicleToBodyConfig.BodyStyleConfigId}.");
            }
        }

        public override AssociationCount AssociatedCount(string payload)
        {
            AssociationCount count = new AssociationCount();
            return count;
        }

        public override async Task ClearChangeRequestId(long changeRequestId)
        {
            var entities = await _vehicleToBodyConfigRepositoryService.GetAsync(m => m.ChangeRequestId == changeRequestId && m.DeleteDate == null);

            if (entities != null && entities.Any())
            {
                var entity = entities.First();   //can only be a single item
                entity.ChangeRequestId = null;
                _vehicleToBodyConfigRepositoryService.Update(entity);
                Repositories.SaveChanges();
            }
        }

        /// <summary>
        /// Find a single record which is not marked as deleted.
        /// </summary>
        /// <typeparam name="TKey">any</typeparam>
        /// <param name="id">the key property</param>
        /// <returns>Single entity object if condition matches, null if not found.</returns>
        private async Task<VehicleToBodyStyleConfig> FindAsync<TKey>(TKey id)
        {
            var entityId = Convert.ToInt32(id);
            var entities = await _vehicleToBodyConfigRepositoryService.GetAsync(m => m.Id == entityId && m.DeleteDate == null, 1);
            if (entities != null && entities.Any())
            {
                return entities.First();
            }
            return null;
        }
    }
}
