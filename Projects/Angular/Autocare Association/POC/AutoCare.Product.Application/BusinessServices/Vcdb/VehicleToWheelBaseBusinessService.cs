using AutoCare.Product.Application.Models.BusinessModels;
using AutoCare.Product.Application.RepositoryServices;
using AutoCare.Product.Infrastructure.Serializer;
using AutoCare.Product.Vcdb.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AutoCare.Product.Application.Infrastructure.ExceptionHelpers;
using AutoCare.Product.Infrastructure.ExceptionHandler;
using AutoCare.Product.VcdbSearch.Indexing.ApplicationService;

namespace AutoCare.Product.Application.BusinessServices.Vcdb
{
    public class VehicleToWheelBaseBusinessService : VcdbBusinessService<VehicleToWheelBase>, IVehicleToWheelBaseBusinessService
    {
        private readonly IVcdbSqlServerEfRepositoryService<VehicleToWheelBase> _vehicleToWheelBaseRepositoryService = null;
        private readonly IVcdbChangeRequestBusinessService _vcdbChangeRequestBusinessService = null;
        // todo: needs indexing service
        private readonly IVehicleToWheelBaseIndexingService _vehicleToWheelBaseIndexingService = null;

        public VehicleToWheelBaseBusinessService(IVcdbUnitOfWork vcdbUnitOfWork, 
            IVcdbChangeRequestBusinessService vcdbChangeRequestBusinessService, 
            ITextSerializer serializer, IVehicleToWheelBaseIndexingService vehicleToWheelBaseIndexingService = null) 
            : base(vcdbUnitOfWork, vcdbChangeRequestBusinessService, serializer)
        {
            _vehicleToWheelBaseRepositoryService = vcdbUnitOfWork.GetRepositoryService<VehicleToWheelBase>() as IVcdbSqlServerEfRepositoryService<VehicleToWheelBase>;
            _vcdbChangeRequestBusinessService = vcdbChangeRequestBusinessService;
            _vehicleToWheelBaseIndexingService = vehicleToWheelBaseIndexingService;
        }

        public override AssociationCount AssociatedCount(string payload)
        {
            AssociationCount count = new AssociationCount();
            return count;
        }

        public override async Task<long> SubmitAddChangeRequestAsync(VehicleToWheelBase entity, string requestedBy,
            List<ChangeRequestItemStaging> changeRequestItemStagings = null, CommentsStagingModel comment = null,
            List<AttachmentsModel> attachmentsStagingModels = null, string changeContent = null)
        {
            // validation
            if (entity.VehicleId.Equals(default(int))
                || entity.WheelBaseId.Equals(default(int)))
            {
                throw new ArgumentException(nameof(entity));
            }

            changeRequestItemStagings = changeRequestItemStagings ?? new List<ChangeRequestItemStaging>();

            StringBuilder validationErrorMessage = new StringBuilder();
            await this.ValidateVehicleToWheelBaseHasNoChangeReqeuest(entity);
            await this.ValidateVehicleToWheelBaseLookUpHasNoChangeRequest(entity);

            changeRequestItemStagings.Add(new ChangeRequestItemStaging()
            {
                ChangeType = ChangeType.Add,
                EntityId = entity.Id.ToString(),
                CreatedDateTime = DateTime.UtcNow,
                Entity = typeof(VehicleToWheelBase).Name,
                Payload = base.Serializer.Serialize(entity)
            });

            var vehicleRepositoryService = base.Repositories.GetRepositoryService<Vehicle>() as IVcdbSqlServerEfRepositoryService<Vehicle>;
            var wheelBaseConfigRepositoryService = base.Repositories.GetRepositoryService<WheelBase>() as IVcdbSqlServerEfRepositoryService<WheelBase>;

            Vehicle vehicle = null;
            WheelBase wheelBase = null;
            if (vehicleRepositoryService != null && wheelBaseConfigRepositoryService != null)
            {
                var vehicles = await vehicleRepositoryService.GetAsync(m => m.Id == entity.VehicleId && m.DeleteDate == null, 1);
                if (vehicles != null && vehicles.Any())
                {
                    vehicle = vehicles.First();
                }

                var wheelBases = await wheelBaseConfigRepositoryService.GetAsync(m => m.Id == entity.WheelBaseId && m.DeleteDate == null, 1);
                if (wheelBases != null && wheelBases.Any())
                {
                    wheelBase = wheelBases.First();
                }
            }

            changeContent =
                $"{vehicle?.BaseVehicle.YearId} / {vehicle?.BaseVehicle.Make.Name} / {vehicle?.BaseVehicle.Model.Name} / {vehicle?.SubModel.Name} / {vehicle?.Region.Name} => \n{wheelBase?.Base} / {wheelBase?.WheelBaseMetric}";

            return await base.SubmitAddChangeRequestAsync(entity, requestedBy, changeRequestItemStagings, comment, attachmentsStagingModels, changeContent);
        }

        // this method is not invoked since there is no update for vehicle to wheel base
        public override async Task<long> SubmitUpdateChangeRequestAsync<TId>(VehicleToWheelBase entity, TId id, string requestedBy,
            ChangeType changeType = ChangeType.None,
            List<ChangeRequestItemStaging> changeRequestItemStagings = null, CommentsStagingModel comment = null,
            List<AttachmentsModel> attachments = null, string changeContent = null)
        {
            //Note: Existing Payload in not required as there is no Modify Method
            throw new Exception("This method should not be invoked");
        }

        public override async Task<long> SubmitDeleteChangeRequestAsync<TId>(VehicleToWheelBase entity, TId id, 
            string requestedBy, List<ChangeRequestItemStaging> changeRequestItemStagings = null, 
            CommentsStagingModel comment = null, List<AttachmentsModel> attachmentsStaging = null, string changeContent = null)
        {
            if (id.Equals(default(TId)))
            {
                throw new ArgumentNullException(nameof(id));
            }

            var vehicleToWheelBaseFromDb = await this.FindAsync(id);
            if (vehicleToWheelBaseFromDb == null)
            {
                throw new NoRecordFound("No Vehicle to Wheel Base exist");
            }

            var vehicleToWheelBaseSubmit = new VehicleToWheelBase()
            {
                Id = vehicleToWheelBaseFromDb.Id,
                VehicleId = vehicleToWheelBaseFromDb.VehicleId,
                WheelBaseId = vehicleToWheelBaseFromDb.WheelBaseId
            };

            changeRequestItemStagings = changeRequestItemStagings ?? new List<ChangeRequestItemStaging>();

            // validate no open CR on loopup items
            await this.ValidateVehicleToWheelBaseLookUpHasNoChangeRequest(vehicleToWheelBaseFromDb);

            changeRequestItemStagings.Add(new ChangeRequestItemStaging()
            {
                ChangeType = ChangeType.Delete,
                EntityId = vehicleToWheelBaseFromDb.Id.ToString(),
                CreatedDateTime = DateTime.UtcNow,
                Entity = typeof(VehicleToWheelBase).Name,
                Payload = base.Serializer.Serialize(new VehicleToWheelBase
                {
                    VehicleId = vehicleToWheelBaseFromDb.VehicleId,
                    WheelBaseId = vehicleToWheelBaseFromDb.WheelBaseId,
                    Id = vehicleToWheelBaseFromDb.Id,
                })
            });

            changeContent =
                $"{vehicleToWheelBaseFromDb.Vehicle.BaseVehicle.YearId} / {vehicleToWheelBaseFromDb.Vehicle.BaseVehicle.Make.Name} / {vehicleToWheelBaseFromDb.Vehicle.BaseVehicle.Model.Name} / {vehicleToWheelBaseFromDb.Vehicle.SubModel.Name} / {vehicleToWheelBaseFromDb.Vehicle.Region.Name} => \n{vehicleToWheelBaseFromDb.WheelBase.Base} / {vehicleToWheelBaseFromDb.WheelBase.WheelBaseMetric}";

            var changeRequestId = await base.SubmitDeleteChangeRequestAsync(vehicleToWheelBaseSubmit, id, requestedBy, changeRequestItemStagings, comment, attachmentsStaging, changeContent);

            vehicleToWheelBaseFromDb.ChangeRequestId = changeRequestId;
            _vehicleToWheelBaseRepositoryService.Update(vehicleToWheelBaseFromDb);
            Repositories.SaveChanges();

            // todo: needs indexing service
            await _vehicleToWheelBaseIndexingService.UpdateVehicleToWheelBaseChangeRequestIdAsync(vehicleToWheelBaseFromDb.Id.ToString(), changeRequestId);

            return changeRequestId;
        }

        public override async Task<ChangeRequestStagingModel<VehicleToWheelBase>> GetChangeRequestStaging<TId>(
           TId changeRequestId)
        {
            ChangeRequestStagingModel<VehicleToWheelBase> staging =
                await _vcdbChangeRequestBusinessService.GetChangeRequestStagingByChangeRequestIdAsync<VehicleToWheelBase, TId>(changeRequestId);
            return staging;
        }

        public override async Task<List<VehicleToWheelBase>> GetAsync(Expression<Func<VehicleToWheelBase, bool>> whereCondition, int topCount = 0)
        {
            var result = await _vehicleToWheelBaseRepositoryService
                .GetAsync(whereCondition, 100000,
                    "WheelBase",
                    "Vehicle.BaseVehicle.Make",
                    "Vehicle.BaseVehicle.Model",
                    "Vehicle.BaseVehicle.Year",
                    "Vehicle.SubModel");
            return result.Where(x => x.DeleteDate == null).ToList();
        }

        public override async Task ClearChangeRequestId(long changeRequestId)
        {
            var entities = await _vehicleToWheelBaseRepositoryService.GetAsync(m => m.ChangeRequestId == changeRequestId && m.DeleteDate == null);
            if (entities != null && entities.Any())
            {
                var entity = entities.First();   //can only be a single item
                entity.ChangeRequestId = null;
                _vehicleToWheelBaseRepositoryService.Update(entity);
                Repositories.SaveChanges();
            }
        }

        private async Task ValidateVehicleToWheelBaseHasNoChangeReqeuest(VehicleToWheelBase vehicleToWheelBase)
        {
            // 1. Validate that there is no vehicle to wheel base with same configuration
            IList<VehicleToWheelBase> vehicleToWheelBaseFromDb =
                await _vehicleToWheelBaseRepositoryService.GetAsync(x => 
                    x.VehicleId.Equals(vehicleToWheelBase.VehicleId) && 
                    x.WheelBaseId.Equals(vehicleToWheelBase.WheelBaseId) && 
                    x.DeleteDate == null);

            if (vehicleToWheelBaseFromDb != null && vehicleToWheelBaseFromDb.Count > 0)
            {
                throw new RecordAlreadyExist("Vehicle to Wheel Base with same configuration already exists.");
            }

            // 2. Validate that there is no open CR with same configuration
            var changeRequestId = await
             ChangeRequestBusinessService.ChangeRequestExist<VehicleToWheelBase>(
                 x => x.VehicleId.Equals(vehicleToWheelBase.VehicleId)
                      && x.WheelBaseId.Equals(vehicleToWheelBase.WheelBaseId));
            if (changeRequestId > 0)
            {
                throw new ChangeRequestExistException($"There is already an open CR ID {changeRequestId} with same vehicle to wheel base configuration.");
            }
        }

        private async Task ValidateVehicleToWheelBaseLookUpHasNoChangeRequest(VehicleToWheelBase vehicleToWheelBase)
        {
            var changerRequestId =
                await ChangeRequestBusinessService.ChangeRequestExist(typeof(Vehicle).Name,vehicleToWheelBase.VehicleId);
            if (changerRequestId > 0)
            {
                throw new ChangeRequestExistException($"There is already an open CR ID {changerRequestId} for Vehicle Id : {vehicleToWheelBase.VehicleId}.");
            }

            var changerequestId =
                await
                    ChangeRequestBusinessService.ChangeRequestExist(typeof(WheelBase).Name,
                        vehicleToWheelBase.WheelBaseId);
            if (changerequestId > 0)
            {
                throw new ChangeRequestExistException($"There is already an open CR ID {changerequestId} for Wheel Base ID : {vehicleToWheelBase.WheelBaseId}.");
            }
        }

        private async Task<VehicleToWheelBase> FindAsync<TKey>(TKey id)
        {
            var entityId = Convert.ToInt32(id);
            var entities = await _vehicleToWheelBaseRepositoryService.GetAsync(m => m.Id == entityId && m.DeleteDate == null, 1);
            if (entities != null && entities.Any())
            {
                return entities.First();
            }
            return null;
        }
    }
}
