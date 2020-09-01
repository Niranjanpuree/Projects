using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoCare.Product.Application.Infrastructure.ExceptionHelpers;
using AutoCare.Product.Application.Models.BusinessModels;
using AutoCare.Product.Application.RepositoryServices;
using AutoCare.Product.Infrastructure.ExceptionHandler;
using AutoCare.Product.Infrastructure.RepositoryService;
using AutoCare.Product.Infrastructure.Serializer;
using AutoCare.Product.Vcdb.Model;
using System.Linq.Expressions;
using AutoCare.Product.VcdbSearch.ApplicationService;
using AutoCare.Product.VcdbSearch.Indexing.ApplicationService;

namespace AutoCare.Product.Application.BusinessServices.Vcdb
{
    public class DriveTypeBusinessService : VcdbBusinessService<DriveType>, IDriveTypeBusinessService
    {
        private readonly ISqlServerEfRepositoryService<DriveType> _driveTypeRepositoryService;
        private readonly IVehicleToDriveTypeIndexingService _vehicleToDriveTypeIndexingService = null;
        private readonly IVehicleToDriveTypeSearchService _vehicleToDriveTypeSearchService = null;

        public DriveTypeBusinessService(IVcdbUnitOfWork vcdbRepositoryService,
            IVcdbChangeRequestBusinessService vcdbChangeRequestBusinessService,
            IVehicleToDriveTypeIndexingService vehicleToDriveTypeIndexingService,
            ITextSerializer serializer, IVehicleToDriveTypeSearchService vehicleToDriveTypeSearchService)
            : base(vcdbRepositoryService, vcdbChangeRequestBusinessService, serializer)
        {
            _driveTypeRepositoryService = Repositories.GetRepositoryService<DriveType>() as ISqlServerEfRepositoryService<DriveType>;
            _vehicleToDriveTypeIndexingService = vehicleToDriveTypeIndexingService;
            _vehicleToDriveTypeSearchService = vehicleToDriveTypeSearchService;
        }

        public override async Task<List<DriveType>> GetAllAsync(int topCount = 0)
        {
            return await _driveTypeRepositoryService.GetAsync(m => m.DeleteDate == null, topCount);
        }

        
        public override async Task<DriveType> GetAsync<TKey>(TKey id)
        {
            if (id.Equals(default(TKey)))
            {
                throw new ArgumentNullException(nameof(id));
            }

            var DriveType = await FindAsync(id);

            if (DriveType == null)
            {
                throw new NoRecordFound("Record Not Found");
            }

            DriveType.VehicleToDriveTypeCount =
                await _driveTypeRepositoryService.GetCountAsync(DriveType, x => x.VehicleToDriveTypes, y => y.DeleteDate == null);

            return DriveType;
        }

        public override async Task<long> SubmitAddChangeRequestAsync(DriveType entity, string requestedBy,
            List<ChangeRequestItemStaging> changeRequestItemStagings = null, CommentsStagingModel comment = null,
            List<AttachmentsModel> attachmentsStagingModels = null, string changeContent = null)
        {
            if (string.IsNullOrWhiteSpace(entity?.Name))
            {
                throw new ArgumentNullException(nameof(entity));
            }

            // validations
            await ValidateDriveTypeIsNotDuplicate(entity, ChangeType.Add);
            await ValidateDriveTypeHasNoChangeRequest(entity, ChangeType.Add);
            

            changeRequestItemStagings = changeRequestItemStagings ?? new List<ChangeRequestItemStaging>();
            changeRequestItemStagings.Add(new ChangeRequestItemStaging
            {
                EntityId = entity.Id.ToString(),
                Entity = typeof(DriveType).Name,
                ChangeType = ChangeType.Add,
                CreatedDateTime = DateTime.UtcNow,
                Payload = Serializer.Serialize(entity)
            });

            changeContent = string.Format("{0}"
                , entity.Name);

            return await base.SubmitAddChangeRequestAsync(entity, requestedBy, changeRequestItemStagings, comment, attachmentsStagingModels, changeContent);

        }

        public override async Task<long> SubmitUpdateChangeRequestAsync<TId>(DriveType entity, TId id,
            string requestedBy,
            ChangeType changeType = ChangeType.None,
            List<ChangeRequestItemStaging> changeRequestItemStagings = null, CommentsStagingModel comment = null, List<AttachmentsModel> attachments = null, string changeContent = null)
        {
            if (string.IsNullOrWhiteSpace(entity?.Name))
            {
                throw new ArgumentNullException(nameof(entity));
            }
            if (id.Equals(default(TId)))
            {
                throw new ArgumentNullException(nameof(id));
            }
            if (!entity.Id.Equals(id))
            {
                throw new ArgumentException("Invalid DriveType Id");
            }

            var driveTypeDb = await FindAsync(id);

            if (driveTypeDb == null)
            {
                throw new NoRecordFound("Record Not Found");
            }

            // validations
            await ValidateDriveTypeIsNotDuplicate(entity, ChangeType.Modify);
            await ValidateDriveTypeHasNoChangeRequest(entity, ChangeType.Modify);
            //await ValidateDriveTypeLookUpHasNoChangeRequest(entity);

            // fill up audit information
            entity.InsertDate = driveTypeDb.InsertDate;
            entity.LastUpdateDate = driveTypeDb.LastUpdateDate;
            entity.DeleteDate = driveTypeDb.DeleteDate;

            changeRequestItemStagings = changeRequestItemStagings ?? new List<ChangeRequestItemStaging>();

            // to eliminate circular reference during serialize
            var existingEntity = new DriveType()
            {
                Id = driveTypeDb.Id,
                Name = driveTypeDb.Name,
                ChangeRequestId = driveTypeDb.ChangeRequestId,
                VehicleToDriveTypeCount = driveTypeDb.VehicleToDriveTypeCount,
                DeleteDate = driveTypeDb.DeleteDate,
                InsertDate = driveTypeDb.InsertDate,
                LastUpdateDate = driveTypeDb.LastUpdateDate
            };

            changeRequestItemStagings.Add(new ChangeRequestItemStaging
            {
                EntityId = entity.Id.ToString(),
                Entity = typeof(DriveType).Name,
                ChangeType = ChangeType.Modify,
                CreatedDateTime = DateTime.UtcNow,
                Payload = Serializer.Serialize(entity),
                ExistingPayload = base.Serializer.Serialize(existingEntity) // add existing record as ExistingPayload
            });

            changeContent = string.Format("{0} > {1}"
                , driveTypeDb.Name
                , entity.Name);

            var changeRequestId = await
                    base.SubmitUpdateChangeRequestAsync(entity, id, requestedBy, ChangeType.Modify,
                        changeRequestItemStagings, comment, attachments, changeContent);

            driveTypeDb.ChangeRequestId = changeRequestId;
            _driveTypeRepositoryService.Update(driveTypeDb);
            Repositories.SaveChanges();
            IList<VehicleToDriveType> existingVehicleToDriveTypes =
                await
                    base.Repositories.GetRepositoryService<VehicleToDriveType>()
                        .GetAsync(item => item.DriveTypeId.Equals(driveTypeDb.Id) && item.DeleteDate == null);

            //NOTE: updating change request id in child dependent tables is not valid

            if (existingVehicleToDriveTypes == null || existingVehicleToDriveTypes.Count == 0)
            {
                var driveTypeSearchResult = await _vehicleToDriveTypeSearchService.SearchAsync(null, $"driveTypeId eq {driveTypeDb.Id}");
                var existingDriveTypeDocuments = driveTypeSearchResult.Documents;
                if (existingDriveTypeDocuments != null && existingDriveTypeDocuments.Any())
                {
                    foreach (var existingDriveTypeDocument in existingDriveTypeDocuments)
                    {
                        //existingVehicleDocument.VehicleId must be a GUID string
                        await _vehicleToDriveTypeIndexingService.UpdateDriveTypeChangeRequestIdAsync(existingDriveTypeDocument.VehicleToDriveTypeId, changeRequestId);
                    }
                }
            }
            else
            {
                //NOTE: driveTypeDb.VehicleToDriveTypes will be null because it is not lazy loaded
                foreach (var vehicleToDriveType in existingVehicleToDriveTypes)
                {
                    await _vehicleToDriveTypeIndexingService.
                         UpdateDriveTypeChangeRequestIdAsync(vehicleToDriveType.Id.ToString(), changeRequestId);
                }
            }
            
            return changeRequestId;
        }

        public override async Task<long> SubmitReplaceChangeRequestAsync<TId>(DriveType entity, TId id, string requestedBy, List<ChangeRequestItemStaging> changeRequestItemStagings = null, CommentsStagingModel comment = null, List<AttachmentsModel> attachments = null, string changeContent = null)
        {
            if (string.IsNullOrWhiteSpace(entity?.Name))
            {
                throw new ArgumentNullException(nameof(entity));
            }
            if (id.Equals(default(TId)))
            {
                throw new ArgumentNullException(nameof(id));
            }
            if (!entity.Id.Equals(id))
            {
                throw new ArgumentException("Invalid DriveType Id");
            }

            var driveTypeFromDb = await FindAsync(id);
            if (driveTypeFromDb == null)
            {
                throw new NoRecordFound("No DriveType exist");
            }

            changeRequestItemStagings = changeRequestItemStagings ?? new List<ChangeRequestItemStaging>();

            //await ValidateDriveTypeLookUpHasNoChangeRequest(entity);

            //Fill in the existing values to avoid being overwritten when final approve in change review screen.
            var vehicleToDriveTypeRepository = base.Repositories.GetRepositoryService<VehicleToDriveType>();
            foreach (var vehicleToDriveType in entity.VehicleToDriveTypes)
            {
                
                var vehicleToDriveTypeFromDb = await vehicleToDriveTypeRepository.GetAsync(x => x.Id == vehicleToDriveType.Id && x.DeleteDate == null);
                if (vehicleToDriveTypeFromDb == null || !vehicleToDriveTypeFromDb.Any()) continue;

                vehicleToDriveType.InsertDate = vehicleToDriveTypeFromDb.First().InsertDate;
            }

            changeRequestItemStagings.AddRange(entity.VehicleToDriveTypes.Select(item => new ChangeRequestItemStaging()
            {
                ChangeType = ChangeType.Modify,
                EntityId = item.Id.ToString(),
                CreatedDateTime = DateTime.UtcNow,
                Entity = typeof(VehicleToDriveType).Name,
                Payload = base.Serializer.Serialize(item),
                EntityFullName = typeof(VehicleToDriveType).AssemblyQualifiedName
            }));

            

            var changeRequestId = await base.SubmitReplaceChangeRequestAsync(entity, id, requestedBy, changeRequestItemStagings, comment, attachments, changeContent);

            driveTypeFromDb.ChangeRequestId = changeRequestId;
            _driveTypeRepositoryService.Update(driveTypeFromDb);
            Repositories.SaveChanges();

            //NOTE: all vehicletoDriveTypes linked to the DriveType need to be updated with DriveTypeChangeRequestId = CR
            var linkedVehicles = await vehicleToDriveTypeRepository.GetAsync(x => x.DriveType.Id == driveTypeFromDb.Id && x.DeleteDate == null);

            //NOTE: updating change request id in child dependent tables is not valid

            if (linkedVehicles == null || linkedVehicles.Count == 0)
            {
                var driveTypeSearchResult = await _vehicleToDriveTypeSearchService.SearchAsync(null, $"driveTypeId eq {driveTypeFromDb.Id}");
                var existingDriveTypeDocuments = driveTypeSearchResult.Documents;
                if (existingDriveTypeDocuments != null && existingDriveTypeDocuments.Any())
                {
                    foreach (var existingDriveTypeDocument in existingDriveTypeDocuments)
                    {
                        //existingVehicleDocument.VehicleId must be a GUID string
                        await _vehicleToDriveTypeIndexingService.UpdateDriveTypeChangeRequestIdAsync(existingDriveTypeDocument.VehicleToDriveTypeId, changeRequestId);
                    }
                }
            }
            else
            {
                //NOTE: driveTypeDb.VehicleToDriveTypes will be null because it is not lazy loaded
                foreach (var vehicleToDriveType in linkedVehicles)
                {
                    await _vehicleToDriveTypeIndexingService.
                         UpdateDriveTypeChangeRequestIdAsync(vehicleToDriveType.Id.ToString(), changeRequestId);
                }
            }

            return changeRequestId;
        }

        public override async Task<long> SubmitDeleteChangeRequestAsync<TId>(DriveType entity, TId id, string requestedBy,
            List<ChangeRequestItemStaging> changeRequestItemStagings = null, CommentsStagingModel comment = null, List<AttachmentsModel> attachmentsStaging = null, string changeContent = null)
        {
            if (id.Equals(default(TId)))
            {
                throw new ArgumentNullException(nameof(id));
            }

            var driveTypeFromDb = await FindAsync(id);
            if (driveTypeFromDb == null)
            {
                throw new NoRecordFound("No Drive Type exist");
            }

            DriveType driveTypeSubmit = new DriveType()
            {
                Id = driveTypeFromDb.Id,
                VehicleToDriveTypeCount = driveTypeFromDb.VehicleToDriveTypeCount
            };

            changeRequestItemStagings = new List<ChangeRequestItemStaging>();
            //await ValidateDriveTypeLookUpHasNoChangeRequest(driveTypeFromDb);

            changeRequestItemStagings.Add(new ChangeRequestItemStaging()
            {
                ChangeType = ChangeType.Delete,
                EntityId = driveTypeSubmit.Id.ToString(),
                CreatedDateTime = DateTime.UtcNow,
                Entity = typeof(DriveType).Name,
                Payload = base.Serializer.Serialize(driveTypeSubmit)
            });

            IList<VehicleToDriveType> existingVehicleToDriveTypes =
                await
                    base.Repositories.GetRepositoryService<VehicleToDriveType>()
                        .GetAsync(item => item.DriveTypeId.Equals(driveTypeSubmit.Id) && item.DeleteDate == null);

            if (existingVehicleToDriveTypes != null && existingVehicleToDriveTypes.Count > 0)
            {
                changeRequestItemStagings.AddRange(
                    existingVehicleToDriveTypes.Select(vehicleToDriveType => new ChangeRequestItemStaging()
                    {
                        ChangeType = ChangeType.Delete,
                        EntityId = vehicleToDriveType.Id.ToString(),
                        CreatedDateTime = DateTime.UtcNow,
                        Entity = typeof(VehicleToDriveType).Name,
                        Payload = base.Serializer.Serialize(new VehicleToDriveType()
                        {
                            Id = vehicleToDriveType.Id,
                            DriveTypeId = vehicleToDriveType.DriveTypeId,
                            VehicleId = vehicleToDriveType.VehicleId
                        })
                    }));
            }
            changeContent = string.Format("{0}"
                , driveTypeFromDb.Name);

            var changeRequestId = await base.SubmitDeleteChangeRequestAsync(driveTypeSubmit, id, requestedBy, changeRequestItemStagings, comment, attachmentsStaging, changeContent);

            driveTypeFromDb.ChangeRequestId = changeRequestId;
            _driveTypeRepositoryService.Update(driveTypeFromDb);
            Repositories.SaveChanges();

            //NOTE: updating change request id in child dependent tables is not valid

            if (existingVehicleToDriveTypes == null || existingVehicleToDriveTypes.Count == 0)
            {
                var driveTypeSearchResult = await _vehicleToDriveTypeSearchService.SearchAsync(null, $"driveTypeId eq {driveTypeFromDb.Id}");
                var existingDriveTypeDocuments = driveTypeSearchResult.Documents;
                if (existingDriveTypeDocuments != null && existingDriveTypeDocuments.Any())
                {
                    foreach (var existingDriveTypeDocument in existingDriveTypeDocuments)
                    {
                        //existingVehicleDocument.VehicleId must be a GUID string
                        await _vehicleToDriveTypeIndexingService.UpdateDriveTypeChangeRequestIdAsync(existingDriveTypeDocument.VehicleToDriveTypeId, changeRequestId);
                    }
                }
            }
            else
            {
                foreach (var vehicleToDriveType in existingVehicleToDriveTypes)
                {
                    await _vehicleToDriveTypeIndexingService.
                         UpdateDriveTypeChangeRequestIdAsync(vehicleToDriveType.Id.ToString(), changeRequestId);
                }
            }

            return changeRequestId;
        }

        public new async Task<DriveTypeChangeRequestStagingModel> GetChangeRequestStaging<TId>(TId changeRequestId)
        {
            var result = await
                ChangeRequestBusinessService.GetChangeRequestStagingByChangeRequestIdAsync<DriveType, TId>(changeRequestId);

            List<VehicleToDriveType> vehicleToDriveTypes = null;
            if (result.StagingItem.ChangeType == ChangeType.Replace.ToString())
            {
                result.EntityStaging = result.EntityCurrent;
                var changeRequestIdLong = Convert.ToInt64(changeRequestId);
                var vehicleToDriveTypeChangeRequestItems = await this.ChangeRequestBusinessService.GetChangeRequestItemStagingsAsync(item => item.ChangeRequestId == changeRequestIdLong);

                if (vehicleToDriveTypeChangeRequestItems != null && vehicleToDriveTypeChangeRequestItems.Any())
                {
                    var vehicleToDriveTypeIds = vehicleToDriveTypeChangeRequestItems.Select(item => Convert.ToInt32(item.EntityId)).ToList();
                    vehicleToDriveTypes = await base.Repositories.GetRepositoryService<VehicleToDriveType>()
                        .GetAsync(item => vehicleToDriveTypeIds.Any(id => id == item.Id) && item.DeleteDate == null);

                    //1. Extract the replacement DriveType to DriveTypeid from the first deserialized vehicleToDriveTypeChangeRequestItems
                    var vehicleToDriveType = Serializer.Deserialize<VehicleToDriveType>(vehicleToDriveTypeChangeRequestItems[0].Payload);

                    //2. fill result.EntityStaging with the replacement DriveType details
                    result.EntityStaging = await FindAsync(vehicleToDriveType.DriveTypeId);

                    // 3. fill currentEntity
                    result.EntityCurrent = await FindAsync(result.StagingItem.EntityId);
                }
                else
                {
                    var vehicleToDriveTypeChangeRequestStoreItems = await this.ChangeRequestBusinessService.GetChangeRequestItemAsync(item =>
                        item.ChangeRequestId == changeRequestIdLong);

                    if (vehicleToDriveTypeChangeRequestStoreItems != null && vehicleToDriveTypeChangeRequestStoreItems.Any())
                    {
                        var vehicleToDriveTypeIds = vehicleToDriveTypeChangeRequestStoreItems.Select(item => Convert.ToInt32(item.EntityId)).ToList();
                        vehicleToDriveTypes =
                            await
                                base.Repositories.GetRepositoryService<VehicleToDriveType>()
                                    .GetAsync(item => vehicleToDriveTypeIds.Any(id => id == item.Id) && item.DeleteDate == null);

                        //1. Extract the replacement base vehicle id from the first deserialized vehicleChangeRequestItems
                        var vehicleToDriveType = Serializer.Deserialize<VehicleToDriveType>(vehicleToDriveTypeChangeRequestStoreItems[0].Payload);

                        //2. fill result.EntityStaging with the replacement base vehicle details
                        result.EntityStaging = await FindAsync(vehicleToDriveType.DriveTypeId);

                        // 3. fill currentEntity
                        result.EntityCurrent = await FindAsync(result.StagingItem.EntityId);
                    }
                }
            }

            DriveTypeChangeRequestStagingModel staging = new DriveTypeChangeRequestStagingModel
            {
                EntityCurrent = result.EntityCurrent,
                EntityStaging = result.EntityStaging,
                //RequestorComments = result.RequestorComments,
                //ReviewerComments = result.ReviewerComments,
                Comments = result.Comments,
                StagingItem = result.StagingItem,
                ReplacementVehicleToDriveTypes = vehicleToDriveTypes,
                Attachments = result.Attachments
            };
            return staging;
        }

        //public override AssociationCount AssociatedCount(string payload)
        //{
        //    AssociationCount count = new AssociationCount();
        //    if (payload != null)
        //    {
        //        DriveType deserializedEntry = Serializer.Deserialize<DriveType>(payload);
        //        count.VehicleToDriveTypeCount = deserializedEntry.VehicleToDriveTypeCount;
        //    }
        //    return count;
        //}

        public override async Task ClearChangeRequestId(long changeRequestId)
        {
            var entities = await _driveTypeRepositoryService.GetAsync(x => x.ChangeRequestId == changeRequestId && x.DeleteDate == null);

            if (entities != null && entities.Any())
            {
                var entity = entities.First();   //can only be a single item
                entity.ChangeRequestId = null;
                _driveTypeRepositoryService.Update(entity);
                Repositories.SaveChanges();
            }

            var vehicleToDriveTypeRepositoryService = base.Repositories.GetRepositoryService<VehicleToDriveType>();
            List<VehicleToDriveType> approvedReplacementVehicleToDriveTypes = await vehicleToDriveTypeRepositoryService.GetAsync(m => m.ChangeRequestId == changeRequestId);

            if (approvedReplacementVehicleToDriveTypes != null && approvedReplacementVehicleToDriveTypes.Any())
            {
                foreach (var approvedReplacementVehicleToDriveType in approvedReplacementVehicleToDriveTypes)
                {
                    approvedReplacementVehicleToDriveType.ChangeRequestId = null;
                    vehicleToDriveTypeRepositoryService.Update(approvedReplacementVehicleToDriveType);
                }
                Repositories.SaveChanges();
            }
        }

        public override async Task<List<DriveType>> GetPendingAddChangeRequests(Expression<Func<DriveType, bool>> whereCondition = null, int topCount = 0)
        {
            var existingChangeRequestItemStagings = await ChangeRequestBusinessService.GetChangeRequestItemStagingsAsync(
                x => x.Entity.Equals(typeof(DriveType).Name, StringComparison.CurrentCultureIgnoreCase)
                && x.ChangeType == ChangeType.Add);

            List<DriveType> pendingDriveTypes = new List<DriveType>();

            foreach (var existingChangeRequestItemStaging in existingChangeRequestItemStagings)
            {
                var pendingDriveType = Serializer.Deserialize<DriveType>(existingChangeRequestItemStaging.Payload);
                pendingDriveType.ChangeRequestId = existingChangeRequestItemStaging.ChangeRequestId;
                pendingDriveTypes.Add(pendingDriveType);
            }

            if (whereCondition != null)
            {
                var predicate = whereCondition.Compile();

                pendingDriveTypes = pendingDriveTypes.Where(predicate).ToList();
            }

            return pendingDriveTypes;
        }
        private async Task ValidateConfigurationDoesNotMatchWithExistingDriveType(DriveType driveType)
        {
            // 1. Validate that there is no DriveType config with same configuration
            IList<DriveType> driveTypeFromDb =
                await
                    _driveTypeRepositoryService.GetAsync(
                        x => x.Name.Equals(driveType.Name)
                            
                             && x.DeleteDate == null);

            if (driveTypeFromDb != null && driveTypeFromDb.Count > 0)
            {
                throw new RecordAlreadyExist("DriveType with same configuration already exists.");
            }
        }
        
        private async Task<DriveType> FindAsync<TKey>(TKey id)
        {
            var entityId = Convert.ToInt32(id);
            var entities = await _driveTypeRepositoryService.GetAsync(m => m.Id == entityId && m.DeleteDate == null, 1);
            if (entities != null && entities.Any())
            {
                return entities.First();
            }
            return null;
        }
        private async Task ValidateDriveTypeIsNotDuplicate(DriveType driveType, ChangeType changeType)
        {
            // validate no duplicate DriveType Name.
            if (changeType == ChangeType.Add)
            {
                IList<DriveType> driveTypeFromDb =
                    await
                        _driveTypeRepositoryService.GetAsync(
                            x => x.Name.Trim().Equals(driveType.Name.Trim(), StringComparison.CurrentCultureIgnoreCase)
                                 && x.DeleteDate == null);
                if (driveTypeFromDb != null && driveTypeFromDb.Any())
                {
                    throw new RecordAlreadyExist($"There is aleady a DriveType Name: {driveType.Name}.");
                }
            }
            else if (changeType == ChangeType.Modify) // allow whitespace and case correction
            {
                var driveTypeFromDb = await FindAsync(driveType.Id);

                if (driveType.Name.Trim().Equals(driveTypeFromDb.Name.Trim(), StringComparison.CurrentCulture))
                {
                    throw new RecordAlreadyExist($"There is aleady a DriveType Name: {driveType.Name}.");
                }
                if (!driveType.Name.Trim().Equals(driveTypeFromDb.Name.Trim(), StringComparison.CurrentCultureIgnoreCase))
                {
                    IList<DriveType> driveTypesFromDb =
                        await
                            _driveTypeRepositoryService.GetAsync(
                                x => x.Name.Trim().Equals(driveType.Name.Trim(), StringComparison.CurrentCultureIgnoreCase)
                                     && x.DeleteDate == null);
                    if (driveTypesFromDb != null && driveTypesFromDb.Any())
                    {
                        throw new RecordAlreadyExist($"There is aleady a DriveType Name: {driveType.Name}.");
                    }
                }
            }
        }
        private async Task ValidateDriveTypeHasNoChangeRequest(DriveType driveType, ChangeType changeType)
        {
            // validate no CR for this DriveType Id.
            if (changeType != ChangeType.Add)
            {

                var changerequestid =
                    (await ChangeRequestBusinessService.ChangeRequestExist<DriveType>(item => item.Id == driveType.Id));

                if (changerequestid > 0)
                {
                    throw new ChangeRequestExistException($"There is already an open CR ID {changerequestid} with DriveType ID : {driveType.Id}.");
                }
            }

            // validate no CR for this DriveType Name.
            var changerequestid1 =
            await
                ChangeRequestBusinessService.ChangeRequestExist<DriveType>(
                    x => x.Name.Trim().Equals(driveType.Name.Trim(), StringComparison.CurrentCultureIgnoreCase));

            if (changerequestid1 > 0)
            {
                throw new ChangeRequestExistException($"There is already an open CR ID {changerequestid1} with same DriveType Name : {driveType.Name}.");
            }
        }
        private async Task ValidateDriveTypeLookUpHasNoChangeRequest(DriveType driveType)
        {
            // validate no CR for base vehicle using this DriveType.
            var changerequestid =
                await
                    ChangeRequestBusinessService.ChangeRequestExist<VehicleToDriveType>(x => x.DriveType.Id.Equals(driveType.Id));
            if (changerequestid > 0)
            {
                throw new ChangeRequestExistException($"There is already an open CR ID {changerequestid} for Vehicle To DriveType using this DriveType.");
            }
        }
       

    }
}