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
    public class VehicleToMfrBodyCodeBusinessService : VcdbBusinessService<VehicleToMfrBodyCode>, IVehicleToMfrBodyCodeBusinessService
    {
        private readonly IRepositoryService<VehicleToMfrBodyCode> _vehicleToMfrBodyCodeRepositoryService = null;
        private readonly IVehicleToMfrBodyCodeIndexingService _vehicleToMfrBodyCodeIndexingService = null;
        public VehicleToMfrBodyCodeBusinessService(IVcdbUnitOfWork vcdbRepositoryService,
          IVcdbChangeRequestBusinessService vcdbChangeRequestBusinessService,
          ITextSerializer serializer, IVehicleToMfrBodyCodeIndexingService vehicleToMfrBodyCodeIndexingService = null)
            : base(vcdbRepositoryService, vcdbChangeRequestBusinessService, serializer)
        {
            _vehicleToMfrBodyCodeRepositoryService = Repositories.GetRepositoryService<VehicleToMfrBodyCode>();
            _vehicleToMfrBodyCodeIndexingService = vehicleToMfrBodyCodeIndexingService;
        }
        public override async Task<long> SubmitAddChangeRequestAsync(VehicleToMfrBodyCode entity, string requestedBy,
       List<ChangeRequestItemStaging> changeRequestItemStagings = null, CommentsStagingModel comment = null,
       List<AttachmentsModel> attachmentsStagingModels = null, string changeContent = null)
        {
            if (entity.VehicleId.Equals(default(int))
                || entity.MfrBodyCodeId.Equals(default(int)))
            {
                throw new ArgumentException(nameof(entity));
            }

            changeRequestItemStagings = changeRequestItemStagings ?? new List<ChangeRequestItemStaging>();
            // Validation check for insert of new vehicle to Mfr Body code

            StringBuilder validationErrorMessage = new StringBuilder();
            await ValidateVehicleToMfrBodyCodeHasNoChangeReqeuest(entity);
            await ValidateVehicleToMfrBodyCodeLookUpHasNoChangeRequest(entity);

            changeRequestItemStagings.Add(new ChangeRequestItemStaging()
            {
                ChangeType = ChangeType.Add,
                EntityId = entity.Id.ToString(),
                CreatedDateTime = DateTime.UtcNow,
                Entity = typeof(VehicleToMfrBodyCode).Name,
                Payload = base.Serializer.Serialize(entity)
            });

            var vehicleRepositoryService = Repositories.GetRepositoryService<Vehicle>() as IVcdbSqlServerEfRepositoryService<Vehicle>;
            var MfrBodyCodeRepositoryService = Repositories.GetRepositoryService<MfrBodyCode>() as IVcdbSqlServerEfRepositoryService<MfrBodyCode>;

            Vehicle vehicle = null;
            MfrBodyCode MfrBodyCode = null;

            if (vehicleRepositoryService != null && MfrBodyCodeRepositoryService != null)
            {
                var vehicles = await vehicleRepositoryService.GetAsync(m => m.Id == entity.VehicleId && m.DeleteDate == null, 1);
                if (vehicles != null && vehicles.Any())
                {
                    vehicle = vehicles.First();
                }

                var MfrBodyCodes = await MfrBodyCodeRepositoryService.GetAsync(m => m.Id == entity.MfrBodyCodeId && m.DeleteDate == null, 1);
                if (MfrBodyCodes != null && MfrBodyCodes.Any())
                {
                    MfrBodyCode = MfrBodyCodes.First();
                }
            }

            changeContent = string.Format("{0} / {1} / {2} / {3} / {4} => \n{5}"
                , vehicle.BaseVehicle.YearId
                , vehicle.BaseVehicle.Make.Name
                , vehicle.BaseVehicle.Model.Name
                , vehicle.SubModel.Name
                , vehicle.Region.Name
                , MfrBodyCode.Name);

            // NOTE: change-request-comments-staging perfomed on base

            return await base.SubmitAddChangeRequestAsync(entity, requestedBy, changeRequestItemStagings, comment, attachmentsStagingModels, changeContent);
        }

        public override async Task<long> SubmitUpdateChangeRequestAsync<TId>(VehicleToMfrBodyCode entity, TId id, string requestedBy,
          ChangeType changeType = ChangeType.None,
          List<ChangeRequestItemStaging> changeRequestItemStagings = null, CommentsStagingModel comment = null, List<AttachmentsModel> attachments = null, string changeContent = null)
        {
            //Note: Existing Payload in not required as there is no Modify Method
            throw new Exception("This method should not be invoked");
        }

        public override async Task<long> SubmitDeleteChangeRequestAsync<TId>(VehicleToMfrBodyCode entity, TId id, string requestedBy, List<ChangeRequestItemStaging> changeRequestItemStagings = null, CommentsStagingModel comment = null, List<AttachmentsModel> attachmentsStaging = null, string changeContent = null)
        {
            if (id.Equals(default(TId)))
            {
                throw new ArgumentNullException(nameof(id));
            }

            var vehicleToMfrBodyCodeFromDb = await FindAsync(id);
            if (vehicleToMfrBodyCodeFromDb == null)
            {
                throw new NoRecordFound("No Vehicle to Mfr Body Code exist");
            }

            var vehicleToMfrBodyCodeSubmit = new VehicleToMfrBodyCode()
            {
                VehicleId = vehicleToMfrBodyCodeFromDb.VehicleId,
                MfrBodyCodeId = vehicleToMfrBodyCodeFromDb.MfrBodyCodeId
            };

            changeRequestItemStagings = changeRequestItemStagings ?? new List<ChangeRequestItemStaging>();

            await ValidateVehicleToMfrBodyCodeLookUpHasNoChangeRequest(vehicleToMfrBodyCodeFromDb);

            changeRequestItemStagings.Add(new ChangeRequestItemStaging()
            {
                ChangeType = ChangeType.Delete,
                EntityId = vehicleToMfrBodyCodeFromDb.Id.ToString(),
                CreatedDateTime = DateTime.UtcNow,
                Entity = typeof(VehicleToMfrBodyCode).Name,
                // Payload = base.Serializer.Serialize(vehicleToMfrBodyCodeFromDb)
                Payload = base.Serializer.Serialize(new VehicleToMfrBodyCode
                {
                    VehicleId = vehicleToMfrBodyCodeFromDb.VehicleId,
                    MfrBodyCodeId = vehicleToMfrBodyCodeFromDb.MfrBodyCodeId,
                    Id = vehicleToMfrBodyCodeFromDb.Id,
                })
            });

            changeContent = string.Format("{0} / {1} / {2} / {3} / {4} => \n{5}"
                , vehicleToMfrBodyCodeFromDb.Vehicle.BaseVehicle.YearId
                , vehicleToMfrBodyCodeFromDb.Vehicle.BaseVehicle.Make.Name
                , vehicleToMfrBodyCodeFromDb.Vehicle.BaseVehicle.Model.Name
                , vehicleToMfrBodyCodeFromDb.Vehicle.SubModel.Name
                , vehicleToMfrBodyCodeFromDb.Vehicle.Region.Name
                , vehicleToMfrBodyCodeFromDb.MfrBodyCode.Name);

            var changeRequestId = await base.SubmitDeleteChangeRequestAsync(vehicleToMfrBodyCodeSubmit, id, requestedBy, changeRequestItemStagings, comment, attachmentsStaging, changeContent);

            vehicleToMfrBodyCodeFromDb.ChangeRequestId = changeRequestId;
            _vehicleToMfrBodyCodeRepositoryService.Update(vehicleToMfrBodyCodeFromDb);
            Repositories.SaveChanges();

            await _vehicleToMfrBodyCodeIndexingService.UpdateVehicleToMfrBodyCodeChangeRequestIdAsync(vehicleToMfrBodyCodeFromDb.Id, changeRequestId);

            return changeRequestId;
        }

        public override async Task<ChangeRequestStagingModel<VehicleToMfrBodyCode>> GetChangeRequestStaging<TId>(
       TId changeRequestId)
        {
            ChangeRequestStagingModel<VehicleToMfrBodyCode> staging =
                await
                    ChangeRequestBusinessService.GetChangeRequestStagingByChangeRequestIdAsync<VehicleToMfrBodyCode, TId>(
                        changeRequestId);
            //if (
            //    !staging.StagingItem.ChangeType.Equals(ChangeType.Add.ToString(),
            //        StringComparison.CurrentCultureIgnoreCase))
            //{
            //    VehicleToMfrBodyCode currentVehicleToMfrBodyCode = await FindAsync(staging.StagingItem.EntityId);
            //    staging.EntityCurrent = currentVehicleToMfrBodyCode;
            //}
            return staging;
        }

        public override async Task<List<VehicleToMfrBodyCode>> GetAsync(Expression<Func<VehicleToMfrBodyCode, bool>> whereCondition, int topCount = 0)
        {
            var result = await _vehicleToMfrBodyCodeRepositoryService
                .GetAsync(whereCondition, 100000,
                    "MfrBodyCode",
                    "Vehicle.BaseVehicle.Make",
                    "Vehicle.BaseVehicle.Model",
                    "Vehicle.BaseVehicle.Year",
                    "Vehicle.SubModel");

            return result.Where(x => x.DeleteDate == null).ToList();
        }

        private async Task ValidateVehicleToMfrBodyCodeHasNoChangeReqeuest(VehicleToMfrBodyCode vehicleToMfrBodyCode)
        {
            // 1. Validate that there is no vehicle to Mfr Body Code with same configuration
            IList<VehicleToMfrBodyCode> vehicleToMfrBodyCodeFromDb =
                await _vehicleToMfrBodyCodeRepositoryService.GetAsync(x => x.VehicleId.Equals(vehicleToMfrBodyCode.VehicleId)
                        && x.MfrBodyCodeId.Equals(vehicleToMfrBodyCode.MfrBodyCodeId)
                        && x.DeleteDate == null);

            if (vehicleToMfrBodyCodeFromDb != null && vehicleToMfrBodyCodeFromDb.Count > 0)
            {
                throw new RecordAlreadyExist("Vehicle to Mfr Body Code with same configuration already exists.");
            }

            // 2. Validate that there is no open CR with same configuration

            var ChangeRequestId = await
             ChangeRequestBusinessService.ChangeRequestExist<VehicleToMfrBodyCode>(
                 x => x.VehicleId.Equals(vehicleToMfrBodyCode.VehicleId)
                      && x.MfrBodyCodeId.Equals(vehicleToMfrBodyCode.MfrBodyCodeId));

            if (ChangeRequestId > 0)
            {
                throw new ChangeRequestExistException($"There is already an open CR ID {ChangeRequestId} with same vehicle to Mfr Body Code.");
            }
        }

        private async Task ValidateVehicleToMfrBodyCodeLookUpHasNoChangeRequest(
            VehicleToMfrBodyCode vehicleToMfrBodyCode)
        {
            var changerrequestid =
                await
                    ChangeRequestBusinessService.ChangeRequestExist(typeof(Vehicle).Name,
                        vehicleToMfrBodyCode.VehicleId);

            if (changerrequestid > 0)
            {
                throw new ChangeRequestExistException($"There is already an open CR ID {changerrequestid} for Vehicle Id : {vehicleToMfrBodyCode.VehicleId}.");
            }

            var changerequestID =
                await
                    ChangeRequestBusinessService.ChangeRequestExist(typeof(MfrBodyCode).Name,
                        vehicleToMfrBodyCode.MfrBodyCodeId);
            if (changerequestID > 0)
            {
                throw new ChangeRequestExistException($"There is already an open CR ID {changerequestID} for Mfr Body Code ID : {vehicleToMfrBodyCode.MfrBodyCodeId}.");
            }
        }

        public override AssociationCount AssociatedCount(string payload)
        {
            AssociationCount count = new AssociationCount();
            return count;
        }

        public override async Task ClearChangeRequestId(long changeRequestId)
        {
            var entities = await _vehicleToMfrBodyCodeRepositoryService.GetAsync(m => m.ChangeRequestId == changeRequestId && m.DeleteDate == null);

            if (entities != null && entities.Any())
            {
                var entity = entities.First();   //can only be a single item
                entity.ChangeRequestId = null;
                _vehicleToMfrBodyCodeRepositoryService.Update(entity);
                Repositories.SaveChanges();
            }
        }

        /// <summary>
        /// Find a single record which is not marked as deleted.
        /// </summary>
        /// <typeparam name="TKey">any</typeparam>
        /// <param name="id">the key property</param>
        /// <returns>Single entity object if condition matches, null if not found.</returns>
        private async Task<VehicleToMfrBodyCode> FindAsync<TKey>(TKey id)
        {
            var entityId = Convert.ToInt32(id);
            var entities = await _vehicleToMfrBodyCodeRepositoryService.GetAsync(m => m.Id == entityId && m.DeleteDate == null, 1);
            if (entities != null && entities.Any())
            {
                return entities.First();
            }
            return null;
        }


    }
}
