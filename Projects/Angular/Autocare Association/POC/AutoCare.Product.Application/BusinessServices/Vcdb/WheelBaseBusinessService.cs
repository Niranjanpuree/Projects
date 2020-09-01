using AutoCare.Product.Application.Infrastructure.ExceptionHelpers;
using AutoCare.Product.Application.Models.BusinessModels;
using AutoCare.Product.Application.RepositoryServices;
using AutoCare.Product.Infrastructure.ExceptionHandler;
using AutoCare.Product.Infrastructure.Serializer;
using AutoCare.Product.Vcdb.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoCare.Product.VcdbSearch.Indexing.ApplicationService;
using AutoCare.Product.VcdbSearch.ApplicationService;

namespace AutoCare.Product.Application.BusinessServices.Vcdb
{
    public class WheelBaseBusinessService : VcdbBusinessService<WheelBase>, IWheelBaseBusinessService
    {
        private readonly IVcdbSqlServerEfRepositoryService<WheelBase> _wheelBaseRepositoryService = null;
        // todo: needs indexing service
        private readonly IVehicleToWheelBaseIndexingService _vehicleToWheelBaseIndexingService = null;
        private readonly IVehicleToWheelBaseSearchService  _vehicleToWheelBaseSearchService = null;

        public WheelBaseBusinessService(IVcdbUnitOfWork vcdbUnitOfWork, IVcdbChangeRequestBusinessService vcdbChangeRequestBusinessService, 
            ITextSerializer serializer, IVehicleToWheelBaseIndexingService vehicleToWheelBaseIndexingService, IVehicleToWheelBaseSearchService vehicleToWheelBaseSearchService) 
            : base(vcdbUnitOfWork, vcdbChangeRequestBusinessService, serializer)
        {
            _wheelBaseRepositoryService = vcdbUnitOfWork.GetRepositoryService<WheelBase>() as IVcdbSqlServerEfRepositoryService<WheelBase>;
            // todo: needs indexing service
            _vehicleToWheelBaseIndexingService = vehicleToWheelBaseIndexingService;
            _vehicleToWheelBaseSearchService = vehicleToWheelBaseSearchService;
        }

        public override async Task<WheelBase> GetAsync<TKey>(TKey id)
        {
            if (id.Equals(default(TKey)))
            {
                throw new ArgumentNullException(nameof(id));
            }

            var wheelBase = await FindAsync(id);

            if (wheelBase == null)
            {
                throw new NoRecordFound("Record Not Found");
            }
           wheelBase.VehicleToWheelBaseCount = await _wheelBaseRepositoryService.GetCountAsync(wheelBase, x => x.VehicleToWheelBases, y => y.DeleteDate == null);
            return wheelBase;
        }
        public override async Task<long> SubmitAddChangeRequestAsync(WheelBase entity, string requestedBy,
          List<ChangeRequestItemStaging> changeRequestItemStagings = null, CommentsStagingModel comment = null,
          List<AttachmentsModel> attachmentsStagingModels = null, string changeContent = null)
        {
            if (String.IsNullOrWhiteSpace(entity?.Base))
            {
                throw new ArgumentNullException(nameof(entity));
            }
            if (String.IsNullOrWhiteSpace(entity?.WheelBaseMetric))
            {
                throw new ArgumentNullException(nameof(entity));
            }

            changeRequestItemStagings = changeRequestItemStagings ?? new List<ChangeRequestItemStaging>();

            await ValidateWheelBaseHasNoChangeRequest(entity);

            changeRequestItemStagings.Add(new ChangeRequestItemStaging()
            {
                ChangeType = ChangeType.Add,
                EntityId = entity.Id.ToString(),
                Entity = typeof(WheelBase).Name,
                CreatedDateTime = DateTime.UtcNow,
                Payload = Serializer.Serialize(entity)
            });

            changeContent = string.Format("{0} / {1}"
                , entity.Base, entity.WheelBaseMetric);

            // NOTE: change-request-comments-staging perfomed on base
            return await base.SubmitAddChangeRequestAsync(entity, requestedBy, changeRequestItemStagings, comment, attachmentsStagingModels, changeContent);
        }

        public override async Task<long> SubmitUpdateChangeRequestAsync<TId>(WheelBase entity, TId id, string requestedBy,
          ChangeType changeType = ChangeType.None,
          List<ChangeRequestItemStaging> changeRequestItemStagings = null, CommentsStagingModel comment = null, List<AttachmentsModel> attachments = null, string changeContent = null)
        {
            if (String.IsNullOrWhiteSpace(entity?.Base))
            {
                throw new ArgumentNullException(nameof(entity));
            }
            if (String.IsNullOrWhiteSpace(entity?.WheelBaseMetric))
            {
                throw new ArgumentNullException(nameof(entity));
            }

            if (id.Equals(default(TId)))
            {
                throw new ArgumentNullException(nameof(id));
            }

            if (!entity.Id.Equals(id))
            {
                throw new ArgumentException("Invalid Wheel Base Id");
            }

            var wheelBaseFromDb = await FindAsync(id);
            if (wheelBaseFromDb == null)
            {
                throw new NoRecordFound("No WheelBase exist");
            }

            changeRequestItemStagings = changeRequestItemStagings ?? new List<ChangeRequestItemStaging>();
            await ValidateWheelBaseHasNoChangeRequest(entity);
            
            entity.InsertDate = wheelBaseFromDb.InsertDate;
            var existingEntity = new WheelBase()
            {
                Id = wheelBaseFromDb.Id,
                Base = wheelBaseFromDb.Base,
                WheelBaseMetric = wheelBaseFromDb.WheelBaseMetric,
                ChangeRequestId = wheelBaseFromDb.ChangeRequestId,
                VehicleToWheelBaseCount = wheelBaseFromDb.VehicleToWheelBaseCount,
                DeleteDate = wheelBaseFromDb.DeleteDate,
                InsertDate = wheelBaseFromDb.InsertDate,
                LastUpdateDate = wheelBaseFromDb.LastUpdateDate
            };
            changeRequestItemStagings.Add(new ChangeRequestItemStaging()
            {
                ChangeType = ChangeType.Modify,
                EntityId = entity.Id.ToString(),
                CreatedDateTime = DateTime.UtcNow,
                Entity = typeof(WheelBase).Name,
                Payload = Serializer.Serialize(entity),
                ExistingPayload = Serializer.Serialize(existingEntity)
            });

            changeContent = string.Format("{0} / {1} > {2} / {3}"
                , wheelBaseFromDb.Base, wheelBaseFromDb.WheelBaseMetric
                , entity.Base, entity.WheelBaseMetric);

            var changeRequestId = await base.SubmitUpdateChangeRequestAsync(entity, id, requestedBy, ChangeType.Modify, changeRequestItemStagings, comment, attachments, changeContent);

            wheelBaseFromDb.ChangeRequestId = changeRequestId;
            _wheelBaseRepositoryService.Update(wheelBaseFromDb);
            Repositories.SaveChanges();

            IList<VehicleToWheelBase> existingVehicleToWheelBases =
                await
                    base.Repositories.GetRepositoryService<VehicleToWheelBase>()
                        .GetAsync(item => item.WheelBaseId.Equals(wheelBaseFromDb.Id) && item.DeleteDate == null);
            if (existingVehicleToWheelBases == null || existingVehicleToWheelBases.Count == 0)
            {
                var vehicleToWheelBaseSearchResult = await _vehicleToWheelBaseSearchService.SearchAsync(null, $"wheelBaseId eq {wheelBaseFromDb.Id}");
                var existingVehicleToWheelBaseDocuments = vehicleToWheelBaseSearchResult.Documents;
                if (existingVehicleToWheelBaseDocuments != null && existingVehicleToWheelBaseDocuments.Any())
                {
                    foreach (var existingVehicleToWheelBaseDocument in existingVehicleToWheelBaseDocuments)
                    {
                        //existingVehicleDocument.VehicleId must be a GUID string
                        await _vehicleToWheelBaseIndexingService.UpdateWheelBaseChangeRequestIdAsync(existingVehicleToWheelBaseDocument.VehicleToWheelBaseId, changeRequestId);
                    }
                }
            }
            else
            {
                //NOTE: wheelBaseFromDb.VehicleToBrakeConfigs will be null because it is not lazy loaded
                foreach (var vehicleToWheelBase in existingVehicleToWheelBases)
                {
                    await _vehicleToWheelBaseIndexingService.UpdateWheelBaseChangeRequestIdAsync(vehicleToWheelBase.Id.ToString(), changeRequestId);
                }
            }
            return changeRequestId;
        }


        public override async Task<long> SubmitDeleteChangeRequestAsync<TId>(WheelBase entity, TId id, string requestedBy, List<ChangeRequestItemStaging> changeRequestItemStagings = null, CommentsStagingModel comment = null, List<AttachmentsModel> attachmentsStaging = null, string changeContent = null)
        {
            // validation
            if (id.Equals(default(TId)))
            {
                throw new ArgumentNullException(nameof(id));
            }
            var wheelBaseFromDb = await FindAsync(id);
            if (wheelBaseFromDb == null)
            {
                throw new NoRecordFound("No Wheel Base exist");
            }

            changeRequestItemStagings = changeRequestItemStagings ?? new List<ChangeRequestItemStaging>();
            changeRequestItemStagings.Add(new ChangeRequestItemStaging()
            {
                EntityId = wheelBaseFromDb.Id.ToString(),
                Entity = typeof(WheelBase).Name,
                ChangeType = ChangeType.Delete,
                CreatedDateTime = DateTime.UtcNow,
                Payload = Serializer.Serialize(new WheelBase()
                {
                    Id = wheelBaseFromDb.Id,
                    Base = wheelBaseFromDb.Base,
                    WheelBaseMetric = wheelBaseFromDb.WheelBaseMetric
                })
            });

            changeContent = string.Format("{0} / {1}"
                , wheelBaseFromDb.Base, wheelBaseFromDb.WheelBaseMetric);

            var changeRequestId = await base.SubmitDeleteChangeRequestAsync(wheelBaseFromDb, id, requestedBy, changeRequestItemStagings, comment, attachmentsStaging, changeContent);

            wheelBaseFromDb.ChangeRequestId = changeRequestId;
            _wheelBaseRepositoryService.Update(wheelBaseFromDb);
            Repositories.SaveChanges();

            IList<VehicleToWheelBase> existingVehicleToWheelBases =
            await
                base.Repositories.GetRepositoryService<VehicleToWheelBase>()
                    .GetAsync(item => item.WheelBaseId.Equals(wheelBaseFromDb.Id) && item.DeleteDate == null);
            if (existingVehicleToWheelBases == null || existingVehicleToWheelBases.Count == 0)
            {
                var vehicleToWheelBaseSearchResult = await _vehicleToWheelBaseSearchService.SearchAsync(null, $"wheelBaseId eq {wheelBaseFromDb.Id}");
                var existingVehicleToWheelBaseDocuments = vehicleToWheelBaseSearchResult.Documents;
                if (existingVehicleToWheelBaseDocuments != null && existingVehicleToWheelBaseDocuments.Any())
                {
                    foreach (var existingVehicleToWheelBaseDocument in existingVehicleToWheelBaseDocuments)
                    {
                        //existingVehicleDocument.VehicleId must be a GUID string
                        await _vehicleToWheelBaseIndexingService.UpdateWheelBaseChangeRequestIdAsync(existingVehicleToWheelBaseDocument.VehicleToWheelBaseId, changeRequestId);
                    }
                }
            }
            else
            {
                foreach (var vehicleToWheelBase in existingVehicleToWheelBases)
                {
                    await _vehicleToWheelBaseIndexingService.UpdateVehicleToWheelBaseChangeRequestIdAsync(vehicleToWheelBase.Id.ToString(), changeRequestId);
                }
            }
            return changeRequestId;

            
        }
        public override async Task<long> SubmitReplaceChangeRequestAsync<TId>(WheelBase entity, TId id, string requestedBy, 
            List<ChangeRequestItemStaging> changeRequestItemStagings = null, CommentsStagingModel comment = null, 
            List<AttachmentsModel> attachments = null, string changeContent = null)
        {
            // validation
            if (id.Equals(default(TId)))
            {
                throw new ArgumentNullException(nameof(id));
            }

            if (!entity.Id.Equals(id))
            {
                throw new ArgumentException("Invalid Wheel Base Id.");
            }

            // get current item
            var wheelBaseFromDb = await this.FindAsync(id);
            if (wheelBaseFromDb == null)
            {
                throw new NoRecordFound("No Wheel Base exist.");
            }

            changeRequestItemStagings = changeRequestItemStagings ?? new List<ChangeRequestItemStaging>();

            await this.ValidateWheelBaseLookupsHasNoChangeRequest(entity);
            await this.ValidateWheelBaseDependentHasNoChangeRequest(entity);

            //Fill in the existing values to avoid being overwritten when final approve in change review screen.
            var vehicleToWheelBaseRepository = base.Repositories.GetRepositoryService<VehicleToWheelBase>();
            foreach (var vehicleToBrakeConfig in entity.VehicleToWheelBases)
            {
                // todo: if eager loading was setup, then retrieve items not marked as delete by DeleteDate.
                var vehicleToWheelBaseFromDb = await vehicleToWheelBaseRepository.GetAsync(x => x.Id == vehicleToBrakeConfig.Id && x.DeleteDate == null);
                if (vehicleToWheelBaseFromDb == null || !vehicleToWheelBaseFromDb.Any()) continue;

                vehicleToBrakeConfig.InsertDate = vehicleToWheelBaseFromDb.First().InsertDate;
            }

            changeRequestItemStagings.AddRange(entity.VehicleToWheelBases.Select(vehicle => new ChangeRequestItemStaging()
            {
                ChangeType = ChangeType.Modify,
                EntityId = vehicle.Id.ToString(),
                CreatedDateTime = DateTime.UtcNow,
                Entity = typeof(VehicleToWheelBase).Name,
                Payload = base.Serializer.Serialize(vehicle),
                EntityFullName = typeof(VehicleToWheelBase).AssemblyQualifiedName
            }));

            changeContent = $"{wheelBaseFromDb.Base} / {wheelBaseFromDb.WheelBaseMetric}";
            var changeRequestId = await base.SubmitReplaceChangeRequestAsync(entity, id, requestedBy, changeRequestItemStagings, comment, attachments, changeContent);

            wheelBaseFromDb.ChangeRequestId = changeRequestId;
            this._wheelBaseRepositoryService.Update(wheelBaseFromDb);
            base.Repositories.SaveChanges();

            //NOTE: all vehicle-to-wheelbase linked to the wheelbase need to be updated with WheelBase.ChangeRequestId = CR
            var linkedVehicleToWheelBases = await vehicleToWheelBaseRepository.GetAsync(x => x.WheelBaseId == wheelBaseFromDb.Id && x.DeleteDate == null);
            foreach (var vehicleToWheelBase in linkedVehicleToWheelBases)
            {
                // todo: needs indexing service
                //await this._vehicleToWheelBaseIndexingService.UpdateBaseVehicleChangeRequestIdAsync(vehicleToWheelBase.Id, changeRequestId);
            }

            return changeRequestId;
        }

        //public override async Task<ChangeRequestStagingModel<WheelBase>> GetChangeRequestStaging<TId>(TId changeRequestId)
        //{
        //    ChangeRequestStagingModel<WheelBase> staging =
        //      await ChangeRequestBusinessService.GetChangeRequestStagingByChangeRequestIdAsync<WheelBase, TId>(changeRequestId);
        //    return staging;
        //}
        private async Task<WheelBase> FindAsync<TKey>(TKey id)
        {
            var entityId = Convert.ToInt32(id);
            var entities = await _wheelBaseRepositoryService.GetAsync(m => m.Id == entityId && m.DeleteDate == null, 1);
            if (entities != null && entities.Any())
            {
                return entities.First();
            }
            return null;
        }


        public new async Task<WheelBaseChangeRequestStagingModel> GetChangeRequestStaging<TId>(TId changeRequestId)
        {
            var result = await
                ChangeRequestBusinessService.GetChangeRequestStagingByChangeRequestIdAsync<WheelBase, TId>(changeRequestId);

            List<VehicleToWheelBase> vehicleToWheelBases = null;
            if (result.StagingItem.ChangeType == ChangeType.Replace.ToString())
            {
                result.EntityStaging = result.EntityCurrent;
                var changeRequestIdLong = Convert.ToInt64(changeRequestId);
                var vehicleToWheelBaseChangeRequestItems = await this.ChangeRequestBusinessService.GetChangeRequestItemStagingsAsync(item => item.ChangeRequestId == changeRequestIdLong);

                if (vehicleToWheelBaseChangeRequestItems != null && vehicleToWheelBaseChangeRequestItems.Any())
                {
                    var vehicleToWheelBaseIds = vehicleToWheelBaseChangeRequestItems.Select(item => Convert.ToInt32(item.EntityId)).ToList();
                    vehicleToWheelBases = await base.Repositories.GetRepositoryService<VehicleToWheelBase>()
                        .GetAsync(item => vehicleToWheelBaseIds.Any(id => id == item.Id) && item.DeleteDate == null);

                    //1. Extract the replacement WheelBase to WheelBaseid from the first deserialized vehicleToWheelBaseChangeRequestItems
                    var vehicleToWheelBase = Serializer.Deserialize<VehicleToWheelBase>(vehicleToWheelBaseChangeRequestItems[0].Payload);

                    //2. fill result.EntityStaging with the replacement WheelBase details
                    result.EntityStaging = await FindAsync(vehicleToWheelBase.WheelBaseId);

                    // 3. fill currentEntity
                    result.EntityCurrent = await FindAsync(result.StagingItem.EntityId);
                }
                else
                {
                    var vehicleToWheelBaseChangeRequestStoreItems = await this.ChangeRequestBusinessService.GetChangeRequestItemAsync(item =>
                        item.ChangeRequestId == changeRequestIdLong);

                    if (vehicleToWheelBaseChangeRequestStoreItems != null && vehicleToWheelBaseChangeRequestStoreItems.Any())
                    {
                        //note: deleted records may be needed to be fetched to show in review screen so commented item.deleteddate==null
                        var vehicleToWheelBaseIds = vehicleToWheelBaseChangeRequestStoreItems.Select(item => Convert.ToInt32(item.EntityId)).ToList();
                        vehicleToWheelBases =
                            await
                                base.Repositories.GetRepositoryService<VehicleToWheelBase>()
                                    .GetAsync(item => vehicleToWheelBaseIds.Any(id => id == item.Id /* && item.DeleteDate == null*/));

                        //1. Extract the replacement base vehicle id from the first deserialized vehicleChangeRequestItems
                        var vehicleToWheelBase = Serializer.Deserialize<VehicleToWheelBase>(vehicleToWheelBaseChangeRequestStoreItems[0].Payload);

                        //2. fill result.EntityStaging with the replacement base vehicle details
                        result.EntityStaging = await FindAsync(vehicleToWheelBase.WheelBaseId);

                        // 3. fill currentEntity
                        result.EntityCurrent = await FindAsync(result.StagingItem.EntityId);
                    }
                }
            }

            WheelBaseChangeRequestStagingModel staging = new WheelBaseChangeRequestStagingModel
            {
                EntityCurrent = result.EntityCurrent,
                EntityStaging = result.EntityStaging,
                //RequestorComments = result.RequestorComments,
                //ReviewerComments = result.ReviewerComments,
                Comments = result.Comments,
                StagingItem = result.StagingItem,
                ReplacementVehicleToWheelBases = vehicleToWheelBases,
                Attachments = result.Attachments
            };
            return staging;
        }
        private async Task ValidateWheelBaseLookupsHasNoChangeRequest(WheelBase wheelBase)
        {
            // wheelBase has no lookup items.
        }

        private async Task ValidateWheelBaseDependentHasNoChangeRequest(WheelBase wheelBase)
        {
            // validate no open CR for vehicle to wheel base.
            var changerequestid = await ChangeRequestBusinessService.ChangeRequestExist<VehicleToWheelBase>(x =>
                x.WheelBaseId.Equals(wheelBase.Id));
            if (changerequestid > 0)
            {
                throw new ChangeRequestExistException("There is already an open CR ID " + changerequestid + " for Vehicle to Wheel Base using this wheel base.");
            }
        }
      
        private async Task ValidateWheelBaseHasNoChangeRequest(WheelBase wheelBase)
        {
            IList<WheelBase> wheelBaseFromDb = await _wheelBaseRepositoryService.GetAsync(x => x.Base.Equals(wheelBase.Base) && x.WheelBaseMetric.Equals(wheelBase.WheelBaseMetric) && x.DeleteDate == null);

            if (wheelBaseFromDb != null && wheelBaseFromDb.Any())
            {
                throw new RecordAlreadyExist("Wheel Base already exists");
            }

            var changerequestid =
                await
                    ChangeRequestBusinessService.ChangeRequestExist<WheelBase>(
                        x => x.Base.Equals(wheelBase.Base) && x.WheelBaseMetric.Equals(wheelBase.WheelBaseMetric));
            if (changerequestid > 0)
            {
                throw new ChangeRequestExistException("There is already an open CR ID " + changerequestid + " with same Wheel Base and Wheel Base Metric.");
            }
        }
    }
}
