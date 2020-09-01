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
    public class VehicleToDriveTypeBusinessService : VcdbBusinessService<VehicleToDriveType>, IVehicleToDriveTypeBusinessService
    {
        private readonly IRepositoryService<VehicleToDriveType> _vehicleToDriveTypeRepositoryService = null;
        private readonly IVehicleToDriveTypeIndexingService _vehicleToDriveTypeIndexingService = null;
        public VehicleToDriveTypeBusinessService(IVcdbUnitOfWork vcdbRepositoryService,
          IVcdbChangeRequestBusinessService vcdbChangeRequestBusinessService,
          ITextSerializer serializer, IVehicleToDriveTypeIndexingService vehicleToDriveTypeIndexingService = null)
            : base(vcdbRepositoryService, vcdbChangeRequestBusinessService, serializer)
        {
            _vehicleToDriveTypeRepositoryService = Repositories.GetRepositoryService<VehicleToDriveType>();
            _vehicleToDriveTypeIndexingService = vehicleToDriveTypeIndexingService;
        }
        public override async Task<long> SubmitAddChangeRequestAsync(VehicleToDriveType entity, string requestedBy,
       List<ChangeRequestItemStaging> changeRequestItemStagings = null, CommentsStagingModel comment = null,
       List<AttachmentsModel> attachmentsStagingModels = null, string changeContent = null)
        {
            if (entity.VehicleId.Equals(default(int))
                || entity.DriveTypeId.Equals(default(int)))
            {
                throw new ArgumentException(nameof(entity));
            }

            changeRequestItemStagings = changeRequestItemStagings ?? new List<ChangeRequestItemStaging>();
            // Validation check for insert of new vehicle to Mfr Body code

            StringBuilder validationErrorMessage = new StringBuilder();
            await ValidateVehicleToDriveTypeHasNoChangeReqeuest(entity);
            await ValidateVehicleToDriveTypeLookUpHasNoChangeRequest(entity);

            changeRequestItemStagings.Add(new ChangeRequestItemStaging()
            {
                ChangeType = ChangeType.Add,
                EntityId = entity.Id.ToString(),
                CreatedDateTime = DateTime.UtcNow,
                Entity = typeof(VehicleToDriveType).Name,
                Payload = base.Serializer.Serialize(entity)
            });

            var vehicleRepositoryService = Repositories.GetRepositoryService<Vehicle>() as IVcdbSqlServerEfRepositoryService<Vehicle>;
            var driveTypeRepositoryService = Repositories.GetRepositoryService<DriveType>() as IVcdbSqlServerEfRepositoryService<DriveType>;

            Vehicle vehicle = null;
            DriveType driveType = null;

            if (vehicleRepositoryService != null && driveTypeRepositoryService != null)
            {
                var vehicles = await vehicleRepositoryService.GetAsync(m => m.Id == entity.VehicleId && m.DeleteDate == null, 1);
                if (vehicles != null && vehicles.Any())
                {
                    vehicle = vehicles.First();
                }

                var driveTypes = await driveTypeRepositoryService.GetAsync(m => m.Id == entity.DriveTypeId && m.DeleteDate == null, 1);
                if (driveTypes != null && driveTypes.Any())
                {
                    driveType = driveTypes.First();
                }
            }

            changeContent =
                $"{vehicle.BaseVehicle.YearId} / {vehicle.BaseVehicle.Make.Name} / {vehicle.BaseVehicle.Model.Name} / {vehicle.SubModel.Name} / {vehicle.Region.Name} / {driveType.Name}";

            // NOTE: change-request-comments-staging perfomed on base

            return await base.SubmitAddChangeRequestAsync(entity, requestedBy, changeRequestItemStagings, comment, attachmentsStagingModels, changeContent);
        }

        public override async Task<long> SubmitUpdateChangeRequestAsync<TId>(VehicleToDriveType entity, TId id, string requestedBy,
          ChangeType changeType = ChangeType.None,
          List<ChangeRequestItemStaging> changeRequestItemStagings = null, CommentsStagingModel comment = null, List<AttachmentsModel> attachments = null, string changeContent = null)
        {
            //Note: Existing Payload in not required as there is no Modify Method
            throw new Exception("This method should not be invoked");
        }

        public override async Task<long> SubmitDeleteChangeRequestAsync<TId>(VehicleToDriveType entity, TId id, string requestedBy, List<ChangeRequestItemStaging> changeRequestItemStagings = null, CommentsStagingModel comment = null, List<AttachmentsModel> attachmentsStaging = null, string changeContent = null)
        {
            if (id.Equals(default(TId)))
            {
                throw new ArgumentNullException(nameof(id));
            }

            var vehicleToDriveTypeFromDb = await FindAsync(id);
            if (vehicleToDriveTypeFromDb == null)
            {
                throw new NoRecordFound("No Vehicle to Drive Type exist");
            }

            var vehicleToDriveTypeSubmit = new VehicleToDriveType()
            {
                VehicleId = vehicleToDriveTypeFromDb.VehicleId,
                DriveTypeId = vehicleToDriveTypeFromDb.DriveTypeId
            };

            changeRequestItemStagings = changeRequestItemStagings ?? new List<ChangeRequestItemStaging>();

            await ValidateVehicleToDriveTypeLookUpHasNoChangeRequest(vehicleToDriveTypeFromDb);

            changeRequestItemStagings.Add(new ChangeRequestItemStaging()
            {
                ChangeType = ChangeType.Delete,
                EntityId = vehicleToDriveTypeFromDb.Id.ToString(),
                CreatedDateTime = DateTime.UtcNow,
                Entity = typeof(VehicleToDriveType).Name,
                // Payload = base.Serializer.Serialize(vehicleToDriveTypeFromDb)
                Payload = base.Serializer.Serialize(new VehicleToDriveType
                {
                    VehicleId = vehicleToDriveTypeFromDb.VehicleId,
                    DriveTypeId = vehicleToDriveTypeFromDb.DriveTypeId,
                    Id = vehicleToDriveTypeFromDb.Id,
                })
            });

            changeContent =
                $"{vehicleToDriveTypeFromDb.Vehicle.BaseVehicle.YearId} / {vehicleToDriveTypeFromDb.Vehicle.BaseVehicle.Make.Name} / {vehicleToDriveTypeFromDb.Vehicle.BaseVehicle.Model.Name} / {vehicleToDriveTypeFromDb.Vehicle.SubModel.Name} / {vehicleToDriveTypeFromDb.Vehicle.Region.Name} / {vehicleToDriveTypeFromDb.DriveType.Name}";

            var changeRequestId = await base.SubmitDeleteChangeRequestAsync(vehicleToDriveTypeSubmit, id, requestedBy, changeRequestItemStagings, comment, attachmentsStaging, changeContent);

            vehicleToDriveTypeFromDb.ChangeRequestId = changeRequestId;
            _vehicleToDriveTypeRepositoryService.Update(vehicleToDriveTypeFromDb);
            Repositories.SaveChanges();

            await _vehicleToDriveTypeIndexingService.UpdateVehicleToDriveTypeChangeRequestIdAsync(vehicleToDriveTypeFromDb.Id, changeRequestId);

            return changeRequestId;
        }

        public override async Task<ChangeRequestStagingModel<VehicleToDriveType>> GetChangeRequestStaging<TId>(
       TId changeRequestId)
        {
            ChangeRequestStagingModel<VehicleToDriveType> staging =
                await
                    ChangeRequestBusinessService.GetChangeRequestStagingByChangeRequestIdAsync<VehicleToDriveType, TId>(
                        changeRequestId);
            //if (
            //    !staging.StagingItem.ChangeType.Equals(ChangeType.Add.ToString(),
            //        StringComparison.CurrentCultureIgnoreCase))
            //{
            //    VehicleToMfrBodyCode currentVehicleToMfrBodyCode = await FindAsync(staging.StagingItem.EntityId);
            //    staging.EntityCurrent = currentVehicleToBodyCode;
            //}
            return staging;
        }

        public override async Task<List<VehicleToDriveType>> GetAsync(Expression<Func<VehicleToDriveType, bool>> whereCondition, int topCount = 0)
        {
            var result = await _vehicleToDriveTypeRepositoryService
                .GetAsync(whereCondition, 100000,
                    "DriveType",
                    "Vehicle.BaseVehicle.Make",
                    "Vehicle.BaseVehicle.Model",
                    "Vehicle.BaseVehicle.Year",
                    "Vehicle.SubModel");

            return result.Where(x => x.DeleteDate == null).ToList();
        }

        private async Task ValidateVehicleToDriveTypeHasNoChangeReqeuest(VehicleToDriveType vehicleToDriveType)
        {
            // 1. Validate that there is no vehicle to  Body Code with same configuration
            IList<VehicleToDriveType> vehicleToDriveTypeFromDb =
                await _vehicleToDriveTypeRepositoryService.GetAsync(x => x.VehicleId.Equals(vehicleToDriveType.VehicleId)
                        && x.DriveTypeId.Equals(vehicleToDriveType.DriveTypeId)
                        && x.DeleteDate == null);

            if (vehicleToDriveTypeFromDb != null && vehicleToDriveTypeFromDb.Count > 0)
            {
                throw new RecordAlreadyExist("Vehicle to Drive Type with same configuration already exists.");
            }

            // 2. Validate that there is no open CR with same configuration

            var ChangeRequestId = await
             ChangeRequestBusinessService.ChangeRequestExist<VehicleToDriveType>(
                 x => x.VehicleId.Equals(vehicleToDriveType.VehicleId)
                      && x.DriveTypeId.Equals(vehicleToDriveType.DriveTypeId));

            if (ChangeRequestId > 0)
            {
                throw new ChangeRequestExistException($"There is already an open CR ID {ChangeRequestId} with same vehicle to Drive Type ID.");
            }
        }

        private async Task ValidateVehicleToDriveTypeLookUpHasNoChangeRequest(
            VehicleToDriveType vehicleToDriveType)
        {
            var changerrequestid =
                await
                    ChangeRequestBusinessService.ChangeRequestExist(typeof(Vehicle).Name,
                        vehicleToDriveType.VehicleId);

            if (changerrequestid > 0)
            {
                throw new ChangeRequestExistException($"There is already an open CR ID {changerrequestid} for Vehicle Id : {vehicleToDriveType.VehicleId}.");
            }

            var changerequestID =
                await
                    ChangeRequestBusinessService.ChangeRequestExist(typeof(DriveType).Name,
                        vehicleToDriveType.DriveTypeId);
            if (changerequestID > 0)
            {
                throw new ChangeRequestExistException($"There is already an open CR ID {changerequestID} for Drive Type ID : {vehicleToDriveType.DriveTypeId}.");
            }
        }

        public override AssociationCount AssociatedCount(string payload)
        {
            AssociationCount count = new AssociationCount();
            return count;
        }

        public override async Task ClearChangeRequestId(long changeRequestId)
        {
            var entities = await _vehicleToDriveTypeRepositoryService.GetAsync(m => m.ChangeRequestId == changeRequestId && m.DeleteDate == null);

            if (entities != null && entities.Any())
            {
                var entity = entities.First();   //can only be a single item
                entity.ChangeRequestId = null;
                _vehicleToDriveTypeRepositoryService.Update(entity);
                Repositories.SaveChanges();
            }
        }

        /// <summary>
        /// Find a single record which is not marked as deleted.
        /// </summary>
        /// <typeparam name="TKey">any</typeparam>
        /// <param name="id">the key property</param>
        /// <returns>Single entity object if condition matches, null if not found.</returns>
        private async Task<VehicleToDriveType> FindAsync<TKey>(TKey id)
        {
            var entityId = Convert.ToInt32(id);
            var entities = await _vehicleToDriveTypeRepositoryService.GetAsync(m => m.Id == entityId && m.DeleteDate == null, 1);
            if (entities != null && entities.Any())
            {
                return entities.First();
            }
            return null;
        }


    }
}
