using System;
using System.Linq;
using System.Threading.Tasks;
using AutoCare.Product.Application.Infrastructure.ExceptionHelpers;
using AutoCare.Product.Application.RepositoryServices;
using AutoCare.Product.Infrastructure.ExceptionHandler;
using AutoCare.Product.Infrastructure.RepositoryService;
using AutoCare.Product.Infrastructure.Serializer;
using AutoCare.Product.Vcdb.Model;
using System.Collections.Generic;
using System.Linq.Expressions;
using AutoCare.Product.Application.Models.BusinessModels;
using AutoCare.Product.VcdbSearch.Indexing.ApplicationService;
using AutoCare.Product.VcdbSearch.ApplicationService;

namespace AutoCare.Product.Application.BusinessServices.Vcdb
{
    public class BaseVehicleBusinessService : VcdbBusinessService<BaseVehicle>, IBaseVehicleBusinessService
    {
        private readonly ISqlServerEfRepositoryService<BaseVehicle> _baseVehicleRepositoryService = null;
        private readonly IVehicleIndexingService _vehicleIndexingService = null;
        private readonly IVehicleSearchService _vehicleSearchService;

        public BaseVehicleBusinessService(IVcdbUnitOfWork vcdbRepositoryService,
            IVcdbChangeRequestBusinessService vcdbChangeRequestBusinessService,
            ITextSerializer serializer,
            IVehicleIndexingService vehicleIndexingService = null,
            IVehicleSearchService vehicleSearchService = null)
            : base(vcdbRepositoryService, vcdbChangeRequestBusinessService, serializer)
        {
            _baseVehicleRepositoryService = Repositories.GetRepositoryService<BaseVehicle>() as ISqlServerEfRepositoryService<BaseVehicle>;
            _vehicleIndexingService = vehicleIndexingService;
            _vehicleSearchService = vehicleSearchService;
        }

        public override async Task<List<BaseVehicle>> GetAllAsync(int topCount = 0)
        {
            return await _baseVehicleRepositoryService.GetAsync(x => x.DeleteDate == null, topCount);
        }

        public override async Task<BaseVehicle> GetAsync<TKey>(TKey id)
        {
            if (id.Equals(default(TKey)))
            {
                throw new ArgumentNullException(nameof(id));
            }

            var baseVehicle = await FindAsync(id);

            if (baseVehicle == null)
            {
                throw new NoRecordFound("Record Not Found");
            }

            baseVehicle.VehicleCount = await _baseVehicleRepositoryService.GetCountAsync(baseVehicle, x => x.Vehicles, y => y.DeleteDate == null);

            return baseVehicle;
        }

        public override async Task<long> SubmitAddChangeRequestAsync(BaseVehicle entity, string requestedBy,
            List<ChangeRequestItemStaging> changeRequestItemStagings = null, CommentsStagingModel comment = null,
            List<AttachmentsModel> attachmentsStagingModels = null, string changeContent = null)
        {
            if (entity.MakeId.Equals(default(int))
                || entity.ModelId.Equals(default(int))
                || entity.YearId == default(int))
            {
                throw new ArgumentException(nameof(entity));
            }

            changeRequestItemStagings = changeRequestItemStagings ?? new List<ChangeRequestItemStaging>();

            // validation check on insert of new base vehicle
            await ValidateConfigurationDoesNotMatchWithExistingBaseVehicle(entity);
            await ValidateNoChangeRequestExistsWithSameConfiguration(entity);
            await ValidateBaseVehicleLookupsHasNoChangeRequest(entity);

            changeRequestItemStagings.Add(new ChangeRequestItemStaging()
            {
                ChangeType = ChangeType.Add,
                EntityId = entity.Id.ToString(),
                CreatedDateTime = DateTime.UtcNow,
                Entity = typeof(BaseVehicle).Name,
                Payload = base.Serializer.Serialize(entity)
                // note: existingpaylod is not required on Add.
            });

            var makeRepositoryService = Repositories.GetRepositoryService<Make>() as IVcdbSqlServerEfRepositoryService<Make>;
            var modelRepositoryService = Repositories.GetRepositoryService<Model>() as IVcdbSqlServerEfRepositoryService<Model>;

            Make make = null;
            Model model = null;

            if (makeRepositoryService != null && modelRepositoryService != null)
            {
                var makes = await makeRepositoryService.GetAsync(m => m.Id == entity.MakeId && m.DeleteDate == null, 1);
                if (makes != null && makes.Any())
                {
                    make = makes.First();
                }
                var models = await modelRepositoryService.GetAsync(m => m.Id == entity.ModelId && m.DeleteDate == null, 1);
                if (models != null && models.Any())
                {
                    model = models.First();
                }
            }

            changeContent = string.Format("{0} / {1} / {2}"
            , entity.YearId
            , make.Name
            , model.Name);

            // NOTE: change-request-comments-staging perfomed on base

            return await base.SubmitAddChangeRequestAsync(entity, requestedBy, changeRequestItemStagings, comment, attachmentsStagingModels, changeContent);
        }

        public override async Task<long> SubmitUpdateChangeRequestAsync<TId>(BaseVehicle entity, TId id, string requestedBy, ChangeType changeType = ChangeType.None,
            List<ChangeRequestItemStaging> changeRequestItemStagings = null, CommentsStagingModel comment = null, List<AttachmentsModel> attachments = null, string changeContent = null)
        {
            if (entity.MakeId.Equals(default(int))
                || entity.ModelId.Equals(default(int))
                || entity.YearId == default(int))
            {
                throw new ArgumentException(nameof(entity));
            }

            if (id.Equals(default(TId)))
            {
                throw new ArgumentNullException(nameof(id));
            }

            if (!entity.Id.Equals(id))
            {
                throw new ArgumentException("Invalid Base Vehicle Id");
            }

            var baseVehicleFromDb = await FindAsync(id);
            if (baseVehicleFromDb == null)
            {
                throw new NoRecordFound("No Base Vehicle exist");
            }

            changeRequestItemStagings = changeRequestItemStagings ?? new List<ChangeRequestItemStaging>();

            // validation check on insert of new base vehicle

            await ValidateConfigurationDoesNotMatchWithExistingBaseVehicle(entity);
            await ValidateNoChangeRequestExistsWithSameConfiguration(entity);
            await ValidateBaseVehicleLookupsHasNoChangeRequest(entity);
            await ValidateBaseVehicleDependentHasNoChangeRequest(entity);
            await ValidateBaseVehicleHasNoReplacementChangeRequest(entity);

            entity.InsertDate = baseVehicleFromDb.InsertDate;

            // to eliminate circular reference during serialize,
            var existingEntity = new BaseVehicle()
            {
                Id = baseVehicleFromDb.Id,
                ChangeRequestId = baseVehicleFromDb.ChangeRequestId,
                MakeId = baseVehicleFromDb.MakeId,
                ModelId = baseVehicleFromDb.ModelId,
                YearId = baseVehicleFromDb.YearId,
                VehicleCount = baseVehicleFromDb.VehicleCount,
                DeleteDate = baseVehicleFromDb.DeleteDate,
                InsertDate = baseVehicleFromDb.InsertDate,
                LastUpdateDate = baseVehicleFromDb.LastUpdateDate
            };

            changeRequestItemStagings.Add(new ChangeRequestItemStaging()
            {
                ChangeType = ChangeType.Modify,
                EntityId = entity.Id.ToString(),
                CreatedDateTime = DateTime.UtcNow,
                Entity = typeof(BaseVehicle).Name,
                Payload = base.Serializer.Serialize(entity),
                ExistingPayload = base.Serializer.Serialize(existingEntity) // also add existing record as ExistingPayload
            });

            changeContent = string.Format("{0} / {1} / {2}"
            , baseVehicleFromDb.YearId
            , baseVehicleFromDb.Make.Name
            , baseVehicleFromDb.Model.Name);

            // NOTE: change-request-comments-staging perfomed on base
            var changeRequestId = await base.SubmitUpdateChangeRequestAsync(entity, id, requestedBy, ChangeType.Modify, changeRequestItemStagings, comment, attachments, changeContent);

            baseVehicleFromDb.ChangeRequestId = changeRequestId;
            _baseVehicleRepositoryService.Update(baseVehicleFromDb);
            Repositories.SaveChanges();

            IList<Vehicle> vehicles = await base.Repositories.GetRepositoryService<Vehicle>()
                .GetAsync(item => item.BaseVehicleId == baseVehicleFromDb.Id && item.DeleteDate == null);

            if (vehicles == null || vehicles.Count == 0)
            {
                var vehicleSearchResult = await _vehicleSearchService.SearchAsync(null, $"baseVehicleId eq {baseVehicleFromDb.Id}");
                var existingVehicleDocuments = vehicleSearchResult.Documents;
                if (existingVehicleDocuments != null && existingVehicleDocuments.Any())
                {
                    foreach (var existingVehicleDocument in existingVehicleDocuments)
                    {
                        //existingVehicleDocument.VehicleId must be a GUID string
                        await _vehicleIndexingService.UpdateBaseVehicleChangeRequestIdAsync(existingVehicleDocument.VehicleId, changeRequestId);
                    }
                }
            }
            else
            {
                foreach (var vehicle in baseVehicleFromDb.Vehicles)
                {
                    await _vehicleIndexingService.UpdateBaseVehicleChangeRequestIdAsync(vehicle.Id.ToString(), changeRequestId);
                }
            }

            return changeRequestId;
        }

        public override async Task<long> SubmitDeleteChangeRequestAsync<TId>(BaseVehicle entity, TId id, string requestedBy, List<ChangeRequestItemStaging> changeRequestItemStagings = null, CommentsStagingModel comment = null, List<AttachmentsModel> attachments = null, string changeContent = null)
        {
            if (id.Equals(default(TId)))
            {
                throw new ArgumentNullException(nameof(id));
            }

            var baseVehicleFromDb = await FindAsync(id);
            if (baseVehicleFromDb == null)
            {
                throw new NoRecordFound("No Base Vehicle exist");
            }

            var baseVehicleSubmit = new BaseVehicle()
            {
                Id = baseVehicleFromDb.Id,
                MakeId = baseVehicleFromDb.MakeId,
                ModelId = baseVehicleFromDb.ModelId,
                YearId = baseVehicleFromDb.YearId,
                VehicleCount = entity.VehicleCount
            };

            if (!baseVehicleSubmit.Id.Equals(id))
            {
                throw new ArgumentException("Invalid Base Vehicle Id");
            }

            if (baseVehicleSubmit.MakeId.Equals(default(int))
               || baseVehicleSubmit.ModelId.Equals(default(int))
               || baseVehicleSubmit.YearId == default(int))
            {
                throw new ArgumentException(nameof(baseVehicleSubmit));
            }

            changeRequestItemStagings = changeRequestItemStagings ?? new List<ChangeRequestItemStaging>();
            // validation check on insert of new base vehicle
            await ValidateNoChangeRequestExistsWithSameConfiguration(baseVehicleFromDb);
            await ValidateBaseVehicleDependentHasNoChangeRequest(baseVehicleFromDb);
            await ValidateBaseVehicleHasNoReplacementChangeRequest(baseVehicleFromDb);

            changeRequestItemStagings.Add(new ChangeRequestItemStaging()
            {
                ChangeType = ChangeType.Delete,
                EntityId = id.ToString(),
                CreatedDateTime = DateTime.UtcNow,
                Entity = typeof(BaseVehicle).Name,
                Payload = base.Serializer.Serialize(baseVehicleSubmit)
                // note: no need for existingpayload during delete.
            });

            IList<Vehicle> vehicles = await base.Repositories.GetRepositoryService<Vehicle>()
                .GetAsync(item => item.BaseVehicleId == baseVehicleSubmit.Id && item.DeleteDate == null);

            if (vehicles != null && vehicles.Count > 0)
            {
                foreach (Vehicle vehicle in vehicles)
                {
                    changeRequestItemStagings.Add(new ChangeRequestItemStaging()
                    {
                        ChangeType = ChangeType.Delete,
                        EntityId = vehicle.Id.ToString(),
                        CreatedDateTime = DateTime.UtcNow,
                        Entity = typeof(Vehicle).Name,
                        Payload = base.Serializer.Serialize(new Vehicle()
                        {
                            Id = vehicle.Id,
                            BaseVehicleId = vehicle.BaseVehicleId,
                            SubModelId = vehicle.SubModelId,
                            SourceId = vehicle.SourceId,
                            RegionId = vehicle.RegionId,
                            PublicationStageId = vehicle.PublicationStageId
                        })
                    });

                    IList<VehicleToBrakeConfig> vehicleToBrakeConfigs =
                        await
                            base.Repositories.GetRepositoryService<VehicleToBrakeConfig>()
                                .GetAsync(item => item.VehicleId.Equals(vehicle.Id));

                    if (vehicleToBrakeConfigs != null && vehicleToBrakeConfigs.Count > 0)
                    {
                        changeRequestItemStagings.AddRange(
                            vehicleToBrakeConfigs.Select(vehicleToBrakeConfig => new ChangeRequestItemStaging()
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
                }
            }

            changeContent = string.Format("{0} / {1} / {2}"
            , baseVehicleFromDb.YearId
            , baseVehicleFromDb.Make.Name
            , baseVehicleFromDb.Model.Name);

            var changeRequestId = await base.SubmitDeleteChangeRequestAsync(baseVehicleSubmit, id, requestedBy, changeRequestItemStagings, comment, attachments, changeContent);

            baseVehicleFromDb.ChangeRequestId = changeRequestId;
            _baseVehicleRepositoryService.Update(baseVehicleFromDb);
            Repositories.SaveChanges();

            //NOTE: updating change request id in child dependent tables is not valid

            if (vehicles == null || vehicles.Count == 0)
            {
                var vehicleSearchResult = await _vehicleSearchService.SearchAsync(null, $"baseVehicleId eq {baseVehicleFromDb.Id}");
                var existingVehicleDocuments = vehicleSearchResult.Documents;
                if (existingVehicleDocuments != null && existingVehicleDocuments.Any())
                {
                    foreach (var existingVehicleDocument in existingVehicleDocuments)
                    {
                        //existingVehicleDocument.VehicleId must be a GUID string
                        await _vehicleIndexingService.UpdateBaseVehicleChangeRequestIdAsync(existingVehicleDocument.VehicleId, changeRequestId);
                    }
                }
            }
            else
            {
                foreach (var vehicle in vehicles)
                {
                    await _vehicleIndexingService.UpdateBaseVehicleChangeRequestIdAsync(vehicle.Id.ToString(), changeRequestId);
                }
            }

            return changeRequestId;
        }

        private async Task ValidateBaseVehicleLookupsHasNoChangeRequest(BaseVehicle baseVehicle)
        {
            //Make
            var ChangeRequestId = await ChangeRequestBusinessService.ChangeRequestExist(typeof(Make).Name,
                baseVehicle.MakeId);
            if (ChangeRequestId > 0)
            {
                throw new ChangeRequestExistException(
                    $"There is already an open CR ID {ChangeRequestId} for Make ID : {baseVehicle.MakeId}.");
            }
            //Model
            var changerequestid = await ChangeRequestBusinessService.ChangeRequestExist(typeof(Model).Name,
                baseVehicle.ModelId);
            if (changerequestid > 0)
            {
                throw new ChangeRequestExistException(
                    $"There is already an open CR ID {changerequestid} for Model ID : {baseVehicle.ModelId}.");
            }

            // year
            var changerequestID =
                await ChangeRequestBusinessService.ChangeRequestExist(typeof(Year).Name, baseVehicle.YearId);
            if (changerequestID > 0)
            {
                throw new ChangeRequestExistException(
                    $"There is already an open CR ID {changerequestID} for Year ID : {baseVehicle.YearId}.");
            }
        }

        private async Task ValidateConfigurationDoesNotMatchWithExistingBaseVehicle(BaseVehicle baseVehicle)
        {
            // 1. Validate that there is no base vehicle with same configuration
            IList<BaseVehicle> existingVehicle =
                await _baseVehicleRepositoryService.GetAsync(item => item.MakeId == baseVehicle.MakeId
                && item.ModelId == baseVehicle.ModelId
                && item.Year == baseVehicle.Year
                && item.DeleteDate == null);

            if (existingVehicle != null && existingVehicle.Any())
            {
                throw new RecordAlreadyExist("Base Vehicle with same configuration already exists.");
            }
        }

        private async Task ValidateNoChangeRequestExistsWithSameConfiguration(BaseVehicle baseVehicle)
        {
            var changeRequestId =
               await ChangeRequestBusinessService.ChangeRequestExist<BaseVehicle>(
                    item => item.MakeId == baseVehicle.MakeId &&
                            item.ModelId == baseVehicle.ModelId &&
                            item.YearId == baseVehicle.YearId);
            if (changeRequestId > 0)

            {


                throw new ChangeRequestExistException($"There is already an open CR ID {changeRequestId} for  vehicle using this Basevehicle.");
            }

            //await ChangeRequestBusinessService.ChangeRequestExist<VehicleToBrakeConfig>(item =>
            //    item.VehicleId.Equals(vehicle.Id))
            //if (await
            //    ChangeRequestBusinessService.ChangeRequestExist<BaseVehicle>(
            //        item => item.MakeId == baseVehicle.MakeId &&
            //                item.ModelId == baseVehicle.ModelId &&
            //                item.YearId == baseVehicle.YearId))
            //{
            //    throw new ChangeRequestExistException("There is already an open CR with same base vehicle configuration.");
            //}
        }

        private async Task ValidateBaseVehicleDependentHasNoChangeRequest(BaseVehicle baseVehicle)
        {
            // 5. validate there is no open CR vehicle that uses this base vehicle.
            // vehicle
            var changeRequestId =
                await ChangeRequestBusinessService.ChangeRequestExist<Vehicle>(item =>
                         item.BaseVehicleId.Equals((baseVehicle.Id)));
            if (changeRequestId > 0)

            {


                throw new ChangeRequestExistException($"There is already an open CR ID {changeRequestId} for  vehicle using this Basevehicle.");
            }

            // 6. validate there is no open CR vehicle to brake system configuration that rely on this base vehicle.
            IList<Vehicle> vehicles = await base.Repositories.GetRepositoryService<Vehicle>()
                .GetAsync(item => item.BaseVehicleId == baseVehicle.Id && item.DeleteDate == null);
            // vehicle to brake configuration
            foreach (var vehicle in vehicles)
            {
                var changeRequestId1 = await ChangeRequestBusinessService.ChangeRequestExist<VehicleToBrakeConfig>(item =>
                    item.VehicleId.Equals(vehicle.Id));
                if (changeRequestId1 > 0)
                {
                    throw new ChangeRequestExistException($"There is already an open CR ID {changeRequestId1} for  Vehicle to Brake Configuration for Vehicle that uses this Basevehicle.");
                }
            }
            // vehicle to bed configuration
            foreach (var vehicle in vehicles)
            {
                var changeRequestId1 = await ChangeRequestBusinessService.ChangeRequestExist<VehicleToBedConfig>(item =>
                    item.VehicleId.Equals(vehicle.Id));
                if (changeRequestId1 > 0)
                {
                    throw new ChangeRequestExistException($"There is already an open CR ID {changeRequestId1} for  Vehicle to Bed Configuration for Vehicle that uses this Basevehicle.");
                }
            }
            // vehicle to body style configuration
            foreach (var vehicle in vehicles)
            {
                var changeRequestId1 = await ChangeRequestBusinessService.ChangeRequestExist<VehicleToBodyStyleConfig>(item =>
                    item.VehicleId.Equals(vehicle.Id));
                if (changeRequestId1 > 0)
                {
                    throw new ChangeRequestExistException($"There is already an open CR ID {changeRequestId1} for  Vehicle to Body Style Configuration for Vehicle that uses this Basevehicle.");
                }
            }
        }

        private async Task ValidateBaseVehicleHasNoReplacementChangeRequest(BaseVehicle baseVehicle)
        {
            // 4. validate there is no open CR for replace of base vehicle involving this base vehicle.
            {
                // validate the baseVehicle is not being replaced
                var changerequestid = await ChangeRequestBusinessService.ChangeRequestExist(baseVehicle, baseVehicle.Id);

                if (changerequestid > 0)
                {
                    throw new ChangeRequestExistException("There is already an open CR ID " + changerequestid + " for replacement of this base vehicle.");
                }
            }
        }

        public override async Task<long> SubmitReplaceChangeRequestAsync<TId>(BaseVehicle entity, TId id, string requestedBy, List<ChangeRequestItemStaging> changeRequestItemStagings = null, CommentsStagingModel comment = null, List<AttachmentsModel> attachments = null, string changeContent = null)
        {
            if (entity.MakeId.Equals(default(int))
              || entity.ModelId.Equals(default(int))
              || entity.YearId == default(int))
            {
                throw new ArgumentException(nameof(entity));
            }
            if (id.Equals(default(TId)))
            {
                throw new ArgumentNullException(nameof(id));
            }

            if (!entity.Id.Equals(id))
            {
                throw new ArgumentException("Invalid Base Vehicle Id");
            }
            var baseVehicleFromDb = await FindAsync(id);
            if (baseVehicleFromDb == null)
            {
                throw new NoRecordFound("No Base Vehicle exist");
            }

            changeRequestItemStagings = changeRequestItemStagings ?? new List<ChangeRequestItemStaging>();

            await ValidateBaseVehicleLookupsHasNoChangeRequest(entity);
            await ValidateBaseVehicleDependentHasNoChangeRequest(entity);
            await ValidateBaseVehicleHasNoReplacementChangeRequest(entity);

            //Fill in the existing values to avoid being overwritten when final approve in change review screen.
            var vehicleRepository = base.Repositories.GetRepositoryService<Vehicle>();
            foreach (var vehicle in entity.Vehicles)
            {
                //var vehicleFromDb = await vehicleRepository.FindAsync(vehicle.Id);
                var vehicleFromDb = await vehicleRepository.GetAsync(x => x.Id == vehicle.Id && x.DeleteDate == null);
                if (vehicleFromDb == null || !vehicleFromDb.Any()) continue;

                vehicle.InsertDate = vehicleFromDb.First().InsertDate;
                vehicle.PublicationEnvironment = vehicleFromDb.First().PublicationEnvironment;
                vehicle.PublicationStageId = vehicleFromDb.First().PublicationStageId;
                vehicle.PublicationStageDate = vehicleFromDb.First().PublicationStageDate;
                vehicle.PublicationStageSource = vehicleFromDb.First().PublicationStageSource;
                vehicle.SourceId = vehicleFromDb.First().SourceId;
                vehicle.SourceName = vehicleFromDb.First().SourceName;
            }

            changeRequestItemStagings.AddRange(entity.Vehicles.Select(vehicle => new ChangeRequestItemStaging()
            {
                ChangeType = ChangeType.Modify,
                EntityId = vehicle.Id.ToString(),
                CreatedDateTime = DateTime.UtcNow,
                Entity = typeof(Vehicle).Name,
                Payload = base.Serializer.Serialize(vehicle),
                EntityFullName = typeof(Vehicle).AssemblyQualifiedName
                // note: no need for existingPayload during replace.
                // todo: if needed to show in UI then only required.
            }));

            changeContent = string.Format("{0} / {1} / {2}"
            , baseVehicleFromDb.YearId
            , baseVehicleFromDb.Make.Name
            , baseVehicleFromDb.Model.Name);

            var changeRequestId = await base.SubmitReplaceChangeRequestAsync(entity, id, requestedBy, changeRequestItemStagings, comment, attachments, changeContent);

            baseVehicleFromDb.ChangeRequestId = changeRequestId;
            _baseVehicleRepositoryService.Update(baseVehicleFromDb);
            Repositories.SaveChanges();

            //NOTE: all vehicles linked to the basevehicle need to be updated with BaseVehicleChangeRequestId = CR
            var linkedVehicles = await vehicleRepository.GetAsync(x => x.BaseVehicleId == baseVehicleFromDb.Id && x.DeleteDate == null);

            foreach (var vehicle in linkedVehicles)
            {
                await _vehicleIndexingService.UpdateBaseVehicleChangeRequestIdAsync(vehicle.Id.ToString(), changeRequestId);
            }

            return changeRequestId;
        }

        public new async Task<BaseVehicleChangeRequestStagingModel> GetChangeRequestStaging<TId>(TId changeRequestId)
        {
            var result = await
                ChangeRequestBusinessService.GetChangeRequestStagingByChangeRequestIdAsync<BaseVehicle, TId>(changeRequestId);

            // business specific task.
            // fill value of "EntityCurrent"
            //if (!result.StagingItem.ChangeType.Equals(ChangeType.Add.ToString(), StringComparison.CurrentCultureIgnoreCase))
            //{
            //    result.EntityCurrent = await FindAsync(result.StagingItem.EntityId);
            //}

            List<Vehicle> vehicles = null;
            if (result.StagingItem.ChangeType == ChangeType.Replace.ToString())
            {
                var changeRequestIdLong = Convert.ToInt64(changeRequestId);
                var vehicleChangeRequestItems = await this.ChangeRequestBusinessService.GetChangeRequestItemStagingsAsync(item => item.ChangeRequestId == changeRequestIdLong);

                if (vehicleChangeRequestItems != null && vehicleChangeRequestItems.Any())
                {
                    var vehicleIds = vehicleChangeRequestItems.Select(item => Convert.ToInt32(item.EntityId)).ToList();
                    vehicles =
                        await
                            base.Repositories.GetRepositoryService<Vehicle>()
                                .GetAsync(item => vehicleIds.Any(id => id == item.Id) && item.DeleteDate == null);

                    //1. Extract the replacement base vehicle id from the first deserialized vehicleChangeRequestItems
                    var vehicle = Serializer.Deserialize<Vehicle>(vehicleChangeRequestItems[0].Payload);

                    //2. fill result.EntityStaging with the replacement base vehicle details
                    result.EntityStaging = await FindAsync(vehicle.BaseVehicleId);

                    // 3. fill currentEntity
                    result.EntityCurrent = await FindAsync(result.StagingItem.EntityId);

                    //NOTE: vehicles will have existing base vehicle id, replacement base vehicle id can be found only in deserialized vehicle
                    //foreach (var item in vehicleChangeRequestItems)
                    //{
                    //    vehicles.Add(Serializer.Deserialize<Vehicle>(item.Payload));
                    //}
                }
                else
                {
                    var vehicleChangeRequestStoreItems = await this.ChangeRequestBusinessService.GetChangeRequestItemAsync(item =>
                        item.ChangeRequestId == changeRequestIdLong);

                    if (vehicleChangeRequestStoreItems != null && vehicleChangeRequestStoreItems.Any())
                    {
                        var vehicleIds = vehicleChangeRequestStoreItems.Select(item => Convert.ToInt32(item.EntityId)).ToList();
                        vehicles =
                            await
                                base.Repositories.GetRepositoryService<Vehicle>()
                                    .GetAsync(item => vehicleIds.Any(id => id == item.Id) && item.DeleteDate == null);

                        //1. Extract the replacement base vehicle id from the first deserialized vehicleChangeRequestItems
                        var vehicle = Serializer.Deserialize<Vehicle>(vehicleChangeRequestStoreItems[0].Payload);

                        //2. fill result.EntityStaging with the replacement base vehicle details
                        result.EntityStaging = await FindAsync(vehicle.BaseVehicleId);

                        // 3. fill currentEntity
                        result.EntityCurrent = await FindAsync(result.StagingItem.EntityId);
                    }
                }
            }

            BaseVehicleChangeRequestStagingModel staging = new BaseVehicleChangeRequestStagingModel
            {
                EntityCurrent = result.EntityCurrent,
                EntityStaging = result.EntityStaging,
                //RequestorComments = result.RequestorComments,
                //ReviewerComments = result.ReviewerComments,
                Comments = result.Comments,
                StagingItem = result.StagingItem,
                ReplacementVehicles = vehicles,
                Attachments = result.Attachments
            };
            return staging;
        }

        public override AssociationCount AssociatedCount(string payload)
        {
            AssociationCount count = new AssociationCount();
            if (payload != null)
            {
                BaseVehicle deserializedEntry = Serializer.Deserialize<BaseVehicle>(payload);
                count.VehicleCount = deserializedEntry.VehicleCount;
            }
            return count;
        }

        public override async Task ClearChangeRequestId(long changeRequestId)
        {
            var approvedBaseVehicles = await _baseVehicleRepositoryService.GetAsync(x => x.ChangeRequestId == changeRequestId && x.DeleteDate == null);

            if (approvedBaseVehicles != null && approvedBaseVehicles.Any())
            {
                var baseVehicleFromDb = approvedBaseVehicles.First();   //can only be a single item
                baseVehicleFromDb.ChangeRequestId = null;
                _baseVehicleRepositoryService.Update(baseVehicleFromDb);
                Repositories.SaveChanges();
            }

            var vehicleRepositoryService = base.Repositories.GetRepositoryService<Vehicle>();
            List<Vehicle> approvedReplacementVehicles = await vehicleRepositoryService.GetAsync(m => m.ChangeRequestId == changeRequestId);

            if (approvedReplacementVehicles != null && approvedReplacementVehicles.Any())
            {
                foreach (var approvedReplacementVehicle in approvedReplacementVehicles)
                {
                    approvedReplacementVehicle.ChangeRequestId = null;
                    vehicleRepositoryService.Update(approvedReplacementVehicle);
                }
                Repositories.SaveChanges();
            }
        }

        public override async Task<List<BaseVehicle>> GetPendingAddChangeRequests(Expression<Func<BaseVehicle, bool>> whereCondition = null, int topCount = 0)
        {
            var existingChangeRequestItemStagings = await ChangeRequestBusinessService.GetChangeRequestItemStagingsAsync(
                x => x.Entity.Equals(typeof(BaseVehicle).Name, StringComparison.CurrentCultureIgnoreCase)
                && x.ChangeType == ChangeType.Add);

            List<BaseVehicle> pendingBaseVehicles = new List<BaseVehicle>();

            foreach (var existingChangeRequestItemStaging in existingChangeRequestItemStagings)
            {
                var pendingBaseVehicle = Serializer.Deserialize<BaseVehicle>(existingChangeRequestItemStaging.Payload);
                pendingBaseVehicle.ChangeRequestId = existingChangeRequestItemStaging.ChangeRequestId;
                pendingBaseVehicles.Add(pendingBaseVehicle);
            }

            if (whereCondition != null)
            {
                var predicate = whereCondition.Compile();

                pendingBaseVehicles = pendingBaseVehicles.Where(predicate).ToList();
            }

            return pendingBaseVehicles;
        }

        /// <summary>
        /// Find a single record which is not marked as deleted.
        /// </summary>
        /// <typeparam name="TKey">any</typeparam>
        /// <param name="id">the key property</param>
        /// <returns>Single entity object if condition matches, null if not found.</returns>
        private async Task<BaseVehicle> FindAsync<TKey>(TKey id)
        {
            var entityId = Convert.ToInt32(id);
            var entities = await _baseVehicleRepositoryService.GetAsync(m => m.Id == entityId && m.DeleteDate == null, 1);
            if (entities != null && entities.Any())
            {
                return entities.First();
            }
            return null;
        }
    }
}