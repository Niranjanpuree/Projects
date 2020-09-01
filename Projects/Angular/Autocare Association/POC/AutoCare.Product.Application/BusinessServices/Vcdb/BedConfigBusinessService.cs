using AutoCare.Product.Application.Infrastructure.ExceptionHelpers;
using AutoCare.Product.Application.Models.BusinessModels;
using AutoCare.Product.Application.RepositoryServices;
using AutoCare.Product.Infrastructure.ExceptionHandler;
using AutoCare.Product.Infrastructure.RepositoryService;
using AutoCare.Product.Infrastructure.Serializer;
using AutoCare.Product.Vcdb.Model;
using AutoCare.Product.VcdbSearch.ApplicationService;
using AutoCare.Product.VcdbSearch.Indexing.ApplicationService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AutoCare.Product.Application.BusinessServices.Vcdb
{
    public class BedConfigBusinessService : VcdbBusinessService<BedConfig>, IBedConfigBusinessService
    {
        private readonly IRepositoryService<BedConfig> _bedConfigRepositoryService = null;
        private readonly IVehicleToBedConfigIndexingService _vehicleToBedConfigIndexingService = null;
        private readonly IVehicleToBedConfigSearchService _vehicletoBedConfigSearchService;

        public BedConfigBusinessService(IVcdbUnitOfWork vcdbRepositoryService,
            IVcdbChangeRequestBusinessService vcdbChangeRequestBusinessService,
            ITextSerializer serializer, IVehicleToBedConfigIndexingService vehicleToBedConfigIndexingService = null,
            IVehicleToBedConfigSearchService vehicletoBedConfigSearchService = null)
            : base(vcdbRepositoryService, vcdbChangeRequestBusinessService, serializer)
        {
            _bedConfigRepositoryService = Repositories.GetRepositoryService<BedConfig>();
            _vehicleToBedConfigIndexingService = vehicleToBedConfigIndexingService;
            _vehicletoBedConfigSearchService = vehicletoBedConfigSearchService;
        }

        public override async Task<List<BedConfig>> GetAllAsync(int topCount = 0)
        {
            return await _bedConfigRepositoryService.GetAsync(x => x.DeleteDate == null, topCount);
        }

        public override async Task<BedConfig> GetAsync<TKey>(TKey id)
        {
            if (id.Equals(default(TKey)))
            {
                throw new ArgumentNullException(nameof(id));
            }

            var bedConfig = await FindAsync(id);

            if (bedConfig == null)
            {
                throw new NoRecordFound("Record Not Found");
            }

            bedConfig.VehicleToBedConfigCount =
                await _bedConfigRepositoryService.GetCountAsync(bedConfig, x => x.VehicleToBedConfigs, y => y.DeleteDate == null);

            return bedConfig;
        }

        public override async Task<long> SubmitAddChangeRequestAsync(BedConfig entity, string requestedBy,
            List<ChangeRequestItemStaging> changeRequestItemStagings = null, CommentsStagingModel comment = null,
            List<AttachmentsModel> attachmentsStagingModels = null, string changeContent = null)
        {
            if (entity.BedLengthId.Equals(default(int))
                || entity.BedTypeId.Equals(default(int)))
            {
                throw new ArgumentException(nameof(entity));
            }

            changeRequestItemStagings = new List<ChangeRequestItemStaging>();
            // Validation check for insert of new bed config

            await ValidateConfigurationDoesNotMatchWithExistingBedConfig(entity);
            await ValidateNoChangeRequestExistsWithSameConfiguration(entity);
            await ValidateBedConfigLookUpHasNoChangeRequest(entity);


            changeRequestItemStagings.Add(new ChangeRequestItemStaging()
            {
                ChangeType = ChangeType.Add,
                EntityId = entity.Id.ToString(),
                CreatedDateTime = DateTime.UtcNow,
                Entity = typeof(BedConfig).Name,
                Payload = base.Serializer.Serialize(entity)
            });

            var bedTypeRepositoryService = Repositories.GetRepositoryService<BedType>() as IVcdbSqlServerEfRepositoryService<BedType>;
            var bedLengthRepositoryService = Repositories.GetRepositoryService<BedLength>() as IVcdbSqlServerEfRepositoryService<BedLength>;

            BedType bedType = null;
            BedLength bedLength = null;

            if (bedTypeRepositoryService != null && bedLengthRepositoryService != null)
            {
                var bedTypes = await bedTypeRepositoryService.GetAsync(m => m.Id == entity.BedTypeId && m.DeleteDate == null, 1);
                if (bedTypes != null && bedTypes.Any())
                {
                    bedType = bedTypes.First();
                }
                var bedLengths = await bedLengthRepositoryService.GetAsync(m => m.Id == entity.BedLengthId && m.DeleteDate == null, 1);
                if (bedLengths != null && bedLengths.Any())
                {
                    bedLength = bedLengths.First();
                }

                changeContent = string.Format("{0} / {1} / {2}", bedType.Name, bedLength.Length, bedLength.BedLengthMetric);
            }

            // NOTE: change-request-comments-staging perfomed on base

            return await base.SubmitAddChangeRequestAsync(entity, requestedBy, changeRequestItemStagings, comment, attachmentsStagingModels, changeContent);
        }

        public override async Task<long> SubmitUpdateChangeRequestAsync<TId>(BedConfig entity, TId id,
            string requestedBy,
            ChangeType changeType = ChangeType.None,
            List<ChangeRequestItemStaging> changeRequestItemStagings = null, CommentsStagingModel comment = null, List<AttachmentsModel> attachments = null, string changeContent = null)
        {
            if (entity.BedTypeId.Equals(default(int))
                || entity.BedLengthId.Equals(default(int)))
            {
                throw new ArgumentException(nameof(entity));
            }

            if (id.Equals(default(TId)))
            {
                throw new ArgumentNullException(nameof(id));
            }

            if (!entity.Id.Equals(id))
            {
                throw new ArgumentException("Invalid Bed Config Id");
            }

            var bedConfigFromDb = await FindAsync(id);
            if (bedConfigFromDb == null)
            {
                throw new NoRecordFound("No Bed Config exist");
            }

            var existingEntity = new BedConfig()
            {
                Id = bedConfigFromDb.Id,
                BedLengthId = bedConfigFromDb.BedLengthId,
                BedTypeId = bedConfigFromDb.BedTypeId,
                ChangeRequestId = bedConfigFromDb.ChangeRequestId,
                DeleteDate = bedConfigFromDb.DeleteDate,
                InsertDate = bedConfigFromDb.InsertDate,
                LastUpdateDate = bedConfigFromDb.LastUpdateDate,
                VehicleToBedConfigCount = bedConfigFromDb.VehicleToBedConfigCount
            };

            changeRequestItemStagings = new List<ChangeRequestItemStaging>();

            // Validation check for update of existing bed config
            await ValidateConfigurationDoesNotMatchWithExistingBedConfig(entity);
            await ValidateNoChangeRequestExistsWithSameConfiguration(entity);
            await ValidateBedConfigLookUpHasNoChangeRequest(entity);
            await ValidateBedConfigDependentHasNoChangeRequest(entity);

            entity.InsertDate = bedConfigFromDb.InsertDate;

            changeRequestItemStagings.Add(new ChangeRequestItemStaging()
            {
                ChangeType = ChangeType.Modify,
                EntityId = entity.Id.ToString(),
                CreatedDateTime = DateTime.UtcNow,
                Entity = typeof(BedConfig).Name,
                Payload = base.Serializer.Serialize(entity),
                ExistingPayload = base.Serializer.Serialize(existingEntity)
            });

            changeContent = string.Format("{0} / {1} / {2}"
                , bedConfigFromDb.BedType.Name
                , bedConfigFromDb.BedLength.Length
                , bedConfigFromDb.BedLength.BedLengthMetric);

            // NOTE: change-request-comments-staging perfomed on base

            var changeRequestId = await base.SubmitUpdateChangeRequestAsync(entity, id, requestedBy, ChangeType.Modify, changeRequestItemStagings, comment, attachments, changeContent);

            bedConfigFromDb.ChangeRequestId = changeRequestId;
            _bedConfigRepositoryService.Update(bedConfigFromDb);
            Repositories.SaveChanges();

            //NOTE: updating change request id in child dependent tables is not valid

            IList<VehicleToBedConfig> existingVehicleToBedConfigs =
                await
                    base.Repositories.GetRepositoryService<VehicleToBedConfig>()
                        .GetAsync(item => item.BedConfigId.Equals(bedConfigFromDb.Id) && item.DeleteDate == null);

            if (existingVehicleToBedConfigs == null || existingVehicleToBedConfigs.Count == 0)
            {
                var vehicleToBedConfigSearchResult = await _vehicletoBedConfigSearchService.SearchAsync(null, $"bedConfigId eq {bedConfigFromDb.Id}");
                var existingVehicleToBedConfigDocuments = vehicleToBedConfigSearchResult.Documents;
                if (existingVehicleToBedConfigDocuments != null && existingVehicleToBedConfigDocuments.Any())
                {
                    foreach (var existingVehicleToBedConfigDocument in existingVehicleToBedConfigDocuments)
                    {
                        //existingVehicleToBedConfigDocument.VehicleToBedConfigId must be a GUID string
                        await _vehicleToBedConfigIndexingService.UpdateBedConfigChangeRequestIdAsync(existingVehicleToBedConfigDocument.VehicleToBedConfigId, changeRequestId);
                    }
                }
            }
            else
            {
                //NOTE: bedConfigFromDb.VehicleToBedConfigs will be null because it is not lazy loaded
                foreach (var vehicleToBedConfig in existingVehicleToBedConfigs)
                {
                    await _vehicleToBedConfigIndexingService.UpdateBedConfigChangeRequestIdAsync(vehicleToBedConfig.Id.ToString(), changeRequestId);
                }
            }

            return changeRequestId;
        }

        public override async Task<long> SubmitReplaceChangeRequestAsync<TId>(BedConfig entity, TId id, string requestedBy, List<ChangeRequestItemStaging> changeRequestItemStagings = null, CommentsStagingModel comment = null, List<AttachmentsModel> attachments = null, string changeContent = null)
        {
            if (entity.BedTypeId.Equals(default(int))
                || entity.BedLengthId.Equals(default(int)))
            {
                throw new ArgumentException(nameof(entity));
            }

            if (id.Equals(default(TId)))
            {
                throw new ArgumentNullException(nameof(id));
            }

            if (!entity.Id.Equals(id))
            {
                throw new ArgumentException("Invalid Bed Config Id");
            }

            var bedConfigFromDb = await FindAsync(id);
            if (bedConfigFromDb == null)
            {
                throw new NoRecordFound("No Bed Config exist");
            }

            changeRequestItemStagings = changeRequestItemStagings ?? new List<ChangeRequestItemStaging>();

            await ValidateBedConfigLookUpHasNoChangeRequest(entity);
            await ValidateBedConfigDependentHasNoChangeRequest(entity);

            //Fill in the existing values to avoid being overwritten when final approve in change review screen.
            var vehicleToBedConfigRepository = base.Repositories.GetRepositoryService<VehicleToBedConfig>();
            foreach (var vehicleToBedConfig in entity.VehicleToBedConfigs)
            {
                //var vehicleToBedConfigFromDb = await vehicleToBedConfigRepository.FindAsync(vehicleToBedConfig.Id);
                var vehicleToBedConfigFromDb = await vehicleToBedConfigRepository.GetAsync(x => x.Id == vehicleToBedConfig.Id && x.DeleteDate == null);
                if (vehicleToBedConfigFromDb == null || !vehicleToBedConfigFromDb.Any()) continue;

                vehicleToBedConfig.InsertDate = vehicleToBedConfigFromDb.First().InsertDate;
            }

            changeRequestItemStagings.AddRange(entity.VehicleToBedConfigs.Select(item => new ChangeRequestItemStaging()
            {
                ChangeType = ChangeType.Modify,
                EntityId = item.Id.ToString(),
                CreatedDateTime = DateTime.UtcNow,
                Entity = typeof(VehicleToBedConfig).Name,
                Payload = base.Serializer.Serialize(item),
                EntityFullName = typeof(VehicleToBedConfig).AssemblyQualifiedName
            }));

            changeContent = string.Format("{0} / {1} / {2}"
                , bedConfigFromDb.BedType.Name
                , bedConfigFromDb.BedLength.Length
                , bedConfigFromDb.BedLength.BedLengthMetric);

            var changeRequestId = await base.SubmitReplaceChangeRequestAsync(entity, id, requestedBy, changeRequestItemStagings, comment, attachments, changeContent);

            bedConfigFromDb.ChangeRequestId = changeRequestId;
            _bedConfigRepositoryService.Update(bedConfigFromDb);
            Repositories.SaveChanges();

            //NOTE: all vehicletobedconfigs linked to the bedconfig need to be updated with BedConfigChangeRequestId = CR
            var linkedVehicleToBedConfigs = await vehicleToBedConfigRepository.GetAsync(x => x.BedConfigId == bedConfigFromDb.Id && x.DeleteDate == null);

            foreach (var vehicleToBedConfig in linkedVehicleToBedConfigs)
            {
                await _vehicleToBedConfigIndexingService.UpdateBedConfigChangeRequestIdAsync(vehicleToBedConfig.Id.ToString(), changeRequestId);
            }

            return changeRequestId;
        }

        public override async Task<long> SubmitDeleteChangeRequestAsync<TId>(BedConfig entity, TId id, string requestedBy,
            List<ChangeRequestItemStaging> changeRequestItemStagings = null, CommentsStagingModel comment = null, List<AttachmentsModel> attachmentsStaging = null, string changeContent = null)
        {
            if (id.Equals(default(TId)))
            {
                throw new ArgumentNullException(nameof(id));
            }

            var bedConfigFromDb = await FindAsync(id);
            if (bedConfigFromDb == null)
            {
                throw new NoRecordFound("No Bed Config exist");
            }

            BedConfig bedConfigSubmit = new BedConfig()
            {
                Id = bedConfigFromDb.Id,
                BedTypeId = bedConfigFromDb.BedTypeId,
                BedLengthId = bedConfigFromDb.BedLengthId,
                VehicleToBedConfigCount = entity.VehicleToBedConfigCount
            };

            changeRequestItemStagings = new List<ChangeRequestItemStaging>();

            await ValidateNoChangeRequestExistsWithSameConfiguration(bedConfigFromDb);
            await ValidateBedConfigDependentHasNoChangeRequest(bedConfigFromDb);

            changeRequestItemStagings.Add(new ChangeRequestItemStaging()
            {
                ChangeType = ChangeType.Delete,
                EntityId = bedConfigSubmit.Id.ToString(),
                CreatedDateTime = DateTime.UtcNow,
                Entity = typeof(BedConfig).Name,
                Payload = base.Serializer.Serialize(bedConfigSubmit)
            });

            IList<VehicleToBedConfig> existingVehicleToBedConfigs =
                await
                    base.Repositories.GetRepositoryService<VehicleToBedConfig>()
                        .GetAsync(item => item.BedConfigId.Equals(bedConfigSubmit.Id) && item.DeleteDate == null);

            if (existingVehicleToBedConfigs != null && existingVehicleToBedConfigs.Count > 0)
            {
                changeRequestItemStagings.AddRange(
                    existingVehicleToBedConfigs.Select(vehicleToBedConfig => new ChangeRequestItemStaging()
                    {
                        ChangeType = ChangeType.Delete,
                        EntityId = vehicleToBedConfig.Id.ToString(),
                        CreatedDateTime = DateTime.UtcNow,
                        Entity = typeof(VehicleToBedConfig).Name,
                        Payload = base.Serializer.Serialize(new VehicleToBedConfig()
                        {
                            Id = vehicleToBedConfig.Id,
                            BedConfigId = vehicleToBedConfig.BedConfigId,
                            VehicleId = vehicleToBedConfig.VehicleId
                        })
                    }));
            }

            changeContent = string.Format("{0} / {1} / {2}"
                , bedConfigFromDb.BedType.Name
                , bedConfigFromDb.BedLength.Length
                , bedConfigFromDb.BedLength.BedLengthMetric);

            var changeRequestId = await base.SubmitDeleteChangeRequestAsync(bedConfigSubmit, id, requestedBy, changeRequestItemStagings, comment, attachmentsStaging, changeContent);

            bedConfigFromDb.ChangeRequestId = changeRequestId;
            _bedConfigRepositoryService.Update(bedConfigFromDb);
            Repositories.SaveChanges();

            //NOTE: updating change request id in child dependent tables is not valid
            if (existingVehicleToBedConfigs == null || existingVehicleToBedConfigs.Count == 0)
            {
                var vehicleToBedConfigSearchResult = await _vehicletoBedConfigSearchService.SearchAsync(null, $"bedConfigId eq {bedConfigFromDb.Id}");
                var existingVehicleToBedConfigDocuments = vehicleToBedConfigSearchResult.Documents;
                if (existingVehicleToBedConfigDocuments != null && existingVehicleToBedConfigDocuments.Any())
                {
                    foreach (var existingVehicleToBedConfigDocument in existingVehicleToBedConfigDocuments)
                    {
                        //existingVehicleToBedConfigDocument.VehicleToBedConfigId must be a GUID string
                        await _vehicleToBedConfigIndexingService.UpdateBedConfigChangeRequestIdAsync(existingVehicleToBedConfigDocument.VehicleToBedConfigId, changeRequestId);
                    }
                }
            }
            else
            {
                //NOTE: bedConfigFromDb.VehicleToBedConfigs will be null because it is not lazy loaded
                foreach (var vehicleToBedConfig in existingVehicleToBedConfigs)
                {
                    await _vehicleToBedConfigIndexingService.UpdateBedConfigChangeRequestIdAsync(vehicleToBedConfig.Id.ToString(), changeRequestId);
                }
            }

            return changeRequestId;
        }

        public new async Task<BedConfigChangeRequestStagingModel> GetChangeRequestStaging<TId>(TId changeRequestId)
        {
            var result = await
                ChangeRequestBusinessService.GetChangeRequestStagingByChangeRequestIdAsync<BedConfig, TId>(changeRequestId);

            List<VehicleToBedConfig> vehicleToBedConfigs = null;
            if (result.StagingItem.ChangeType == ChangeType.Replace.ToString())
            {
                result.EntityStaging = result.EntityCurrent;
                var changeRequestIdLong = Convert.ToInt64(changeRequestId);
                var vehicleToBedConfigChangeRequestItems = await this.ChangeRequestBusinessService.GetChangeRequestItemStagingsAsync(item => item.ChangeRequestId == changeRequestIdLong);

                if (vehicleToBedConfigChangeRequestItems != null && vehicleToBedConfigChangeRequestItems.Any())
                {
                    var vehicleToBedConfigIds = vehicleToBedConfigChangeRequestItems.Select(item => Convert.ToInt32(item.EntityId)).ToList();
                    vehicleToBedConfigs = await base.Repositories.GetRepositoryService<VehicleToBedConfig>()
                        .GetAsync(item => vehicleToBedConfigIds.Any(id => id == item.Id) && item.DeleteDate == null);

                    //1. Extract the replacement bed config to bed config id from the first deserialized vehicleToBedConfigChangeRequestItems
                    var vehicleToBedConfig = Serializer.Deserialize<VehicleToBedConfig>(vehicleToBedConfigChangeRequestItems[0].Payload);

                    //2. fill result.EntityStaging with the replacement Bed config details
                    result.EntityStaging = await FindAsync(vehicleToBedConfig.BedConfigId);

                    // 3. fill currentEntity
                    result.EntityCurrent = await FindAsync(result.StagingItem.EntityId);
                }
                else
                {
                    var vehicleToBedConfigChangeRequestStoreItems = await this.ChangeRequestBusinessService.GetChangeRequestItemAsync(item =>
                        item.ChangeRequestId == changeRequestIdLong);

                    if (vehicleToBedConfigChangeRequestStoreItems != null && vehicleToBedConfigChangeRequestStoreItems.Any())
                    {
                        var vehicleToBedConfigIds = vehicleToBedConfigChangeRequestStoreItems.Select(item => Convert.ToInt32(item.EntityId)).ToList();
                        vehicleToBedConfigs =
                            await
                                base.Repositories.GetRepositoryService<VehicleToBedConfig>()
                                    .GetAsync(item => vehicleToBedConfigIds.Any(id => id == item.Id) && item.DeleteDate == null);

                        //1. Extract the replacement base vehicle id from the first deserialized vehicleChangeRequestItems
                        var vehicleToBedConfig = Serializer.Deserialize<VehicleToBedConfig>(vehicleToBedConfigChangeRequestStoreItems[0].Payload);

                        //2. fill result.EntityStaging with the replacement base vehicle details
                        result.EntityStaging = await FindAsync(vehicleToBedConfig.BedConfigId);

                        // 3. fill currentEntity
                        result.EntityCurrent = await FindAsync(result.StagingItem.EntityId);
                    }
                }
            }

            BedConfigChangeRequestStagingModel staging = new BedConfigChangeRequestStagingModel
            {
                EntityCurrent = result.EntityCurrent,
                EntityStaging = result.EntityStaging,
                //RequestorComments = result.RequestorComments,
                //ReviewerComments = result.ReviewerComments,
                Comments = result.Comments,
                StagingItem = result.StagingItem,
                ReplacementVehicleToBedConfigs = vehicleToBedConfigs,
                Attachments = result.Attachments
            };
            return staging;
        }

        public override AssociationCount AssociatedCount(string payload)
        {
            AssociationCount count = new AssociationCount();
            if (payload != null)
            {
                BedConfig deserializedEntry = Serializer.Deserialize<BedConfig>(payload);
                count.VehicleToBedConfigCount = deserializedEntry.VehicleToBedConfigCount;
            }
            return count;
        }

        public override async Task ClearChangeRequestId(long changeRequestId)
        {
            var entities = await _bedConfigRepositoryService.GetAsync(x => x.ChangeRequestId == changeRequestId && x.DeleteDate == null);

            if (entities != null && entities.Any())
            {
                var entity = entities.First();   //can only be a single item
                entity.ChangeRequestId = null;
                _bedConfigRepositoryService.Update(entity);
                Repositories.SaveChanges();
            }

            var vehicleToBedConfigRepositoryService = base.Repositories.GetRepositoryService<VehicleToBedConfig>();
            List<VehicleToBedConfig> approvedReplacementVehicleToBedConfigs = await vehicleToBedConfigRepositoryService.GetAsync(m => m.ChangeRequestId == changeRequestId);

            if (approvedReplacementVehicleToBedConfigs != null && approvedReplacementVehicleToBedConfigs.Any())
            {
                foreach (var approvedReplacementVehicleToBedConfig in approvedReplacementVehicleToBedConfigs)
                {
                    approvedReplacementVehicleToBedConfig.ChangeRequestId = null;
                    vehicleToBedConfigRepositoryService.Update(approvedReplacementVehicleToBedConfig);
                }
                Repositories.SaveChanges();
            }
        }

        public override async Task<List<BedConfig>> GetPendingAddChangeRequests(Expression<Func<BedConfig, bool>> whereCondition = null, int topCount = 0)
        {
            var existingChangeRequestItemStagings = await ChangeRequestBusinessService.GetChangeRequestItemStagingsAsync(
                x => x.Entity.Equals(typeof(BedConfig).Name, StringComparison.CurrentCultureIgnoreCase)
                && x.ChangeType == ChangeType.Add);

            List<BedConfig> pendingBedConfigs = new List<BedConfig>();

            foreach (var existingChangeRequestItemStaging in existingChangeRequestItemStagings)
            {
                var pendingBedConfig = Serializer.Deserialize<BedConfig>(existingChangeRequestItemStaging.Payload);
                pendingBedConfig.ChangeRequestId = existingChangeRequestItemStaging.ChangeRequestId;
                pendingBedConfigs.Add(pendingBedConfig);
            }

            if (whereCondition != null)
            {
                var predicate = whereCondition.Compile();

                pendingBedConfigs = pendingBedConfigs.Where(predicate).ToList();
            }

            return pendingBedConfigs;
        }

        private async Task ValidateConfigurationDoesNotMatchWithExistingBedConfig(BedConfig bedConfig)
        {
            // 1. Validate that there is no bed config with same configuration
            IList<BedConfig> bedConfigFromDb =
                await
                    _bedConfigRepositoryService.GetAsync(
                        x => x.BedTypeId.Equals(bedConfig.BedTypeId)
                             && x.BedLengthId.Equals(bedConfig.BedLengthId)
                             && x.DeleteDate == null);

            if (bedConfigFromDb != null && bedConfigFromDb.Count > 0)
            {
                throw new RecordAlreadyExist("Bed Config with same configuration already exists.");
            }
        }

        private async Task ValidateNoChangeRequestExistsWithSameConfiguration(BedConfig bedConfig)
        {
            // 2. Validate that there is no open CR with same configuration
            var ChangeRequestID = await ChangeRequestBusinessService.ChangeRequestExist<BedConfig>(x =>
                x.BedTypeId.Equals(bedConfig.BedTypeId)
                && x.BedLengthId.Equals(bedConfig.BedLengthId));
            if (ChangeRequestID > 0)
            {
                throw new ChangeRequestExistException("There is already an open CR ID " + ChangeRequestID + " with same bed configuration.");
            }
        }

        private async Task ValidateBedConfigLookUpHasNoChangeRequest(BedConfig bedConfig)
        {
            // 3. Validate that there are no open CR for any of the items that make of configuration. Bed Type and Bed Length
            {
                // Bed length
                var changerequestid =
                    await
                        ChangeRequestBusinessService.ChangeRequestExist(typeof(BedLength).Name,
                            bedConfig.BedLengthId);
                if (changerequestid > 0)
                {
                    throw new ChangeRequestExistException(
                        $"There is already an open CR ID {changerequestid} for Bed Length ID : {bedConfig.BedLengthId}.");
                }

                // Bed Type
                var changerequestID = await ChangeRequestBusinessService.ChangeRequestExist(typeof(BedType).Name, bedConfig.BedTypeId);
                ;
                if (changerequestID > 0)
                {
                    throw new ChangeRequestExistException(
                        $"There is already an open CR ID {changerequestID} for Bed Type ID : {bedConfig.BedTypeId}.");
                }
            }
        }

        private async Task ValidateBedConfigDependentHasNoChangeRequest(BedConfig bedConfig)
        {
            var changerequestid = await ChangeRequestBusinessService.ChangeRequestExist<VehicleToBedConfig>(x =>
                x.BedConfigId.Equals(bedConfig.Id));
            if (changerequestid > 0)
            {
                throw new ChangeRequestExistException("There is already an open CR ID " + changerequestid + " for Vehicle to Bed Configuration using this bed Config.");
            }
        }

        private async Task<BedConfig> FindAsync<TKey>(TKey id)
        {
            var entityId = Convert.ToInt32(id);
            var entities = await _bedConfigRepositoryService.GetAsync(m => m.Id == entityId && m.DeleteDate == null, 1);
            if (entities != null && entities.Any())
            {
                return entities.First();
            }
            return null;
        }
    }
}
