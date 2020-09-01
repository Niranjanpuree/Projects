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
    public class BodyStyleConfigBusinessService : VcdbBusinessService<BodyStyleConfig>, IBodyStyleConfigBusinessService
    {
        private readonly IRepositoryService<BodyStyleConfig> _bodyStyleConfigRepositoryService = null;
        private readonly IVehicleToBodyStyleConfigIndexingService _vehicleToBodyStyleConfigIndexingService;
        private readonly IVehicleToBodyStyleConfigSearchService _vehicleToBodyStyleConfigSearchService;

        public BodyStyleConfigBusinessService(IVcdbUnitOfWork vcdbRepositoryService,
            IVcdbChangeRequestBusinessService vcdbChangeRequestBusinessService,
            ITextSerializer serializer,
            IVehicleToBodyStyleConfigIndexingService vehicleToBodyStyleConfigIndexingService = null,
            IVehicleToBodyStyleConfigSearchService vehicleToBodyStyleConfigSearchService = null)
            : base(vcdbRepositoryService, vcdbChangeRequestBusinessService, serializer)
        {
            _bodyStyleConfigRepositoryService = Repositories.GetRepositoryService<BodyStyleConfig>();
            _vehicleToBodyStyleConfigIndexingService = vehicleToBodyStyleConfigIndexingService;
            _vehicleToBodyStyleConfigSearchService = vehicleToBodyStyleConfigSearchService;
        }

        public override async Task<List<BodyStyleConfig>> GetAllAsync(int topCount = 0)
        {
            return await _bodyStyleConfigRepositoryService.GetAsync(x => x.DeleteDate == null, topCount);
        }

        public override async Task<BodyStyleConfig> GetAsync<TKey>(TKey id)
        {
            if (id.Equals(default(TKey)))
            {
                throw new ArgumentNullException(nameof(id));
            }

            var bodyStyleConfig = await FindAsync(id);

            if (bodyStyleConfig == null)
            {
                throw new NoRecordFound("Record Not Found");
            }

            bodyStyleConfig.VehicleToBodyStyleConfigCount =
                await _bodyStyleConfigRepositoryService.GetCountAsync(bodyStyleConfig, x => x.VehicleToBodyStyleConfigs, y => y.DeleteDate == null);

            return bodyStyleConfig;
        }

        public override async Task<long> SubmitAddChangeRequestAsync(BodyStyleConfig entity, string requestedBy,
            List<ChangeRequestItemStaging> changeRequestItemStagings = null, CommentsStagingModel comment = null,
            List<AttachmentsModel> attachmentsStagingModels = null, string changeContent = null)
        {
            if (entity.BodyNumDoorsId.Equals(default(int))
                || entity.BodyTypeId.Equals(default(int)))
            {
                throw new ArgumentException(nameof(entity));
            }

            changeRequestItemStagings = new List<ChangeRequestItemStaging>();
            // Validation check for insert of new body style config

            await ValidateConfigurationDoesNotMatchWithExistingBodyStyleConfig(entity);
            await ValidateNoChangeRequestExistsWithSameConfiguration(entity);
            await ValidateBodyStyleConfigLookUpHasNoChangeRequest(entity);

            changeRequestItemStagings.Add(new ChangeRequestItemStaging()
            {
                ChangeType = ChangeType.Add,
                EntityId = entity.Id.ToString(),
                CreatedDateTime = DateTime.UtcNow,
                Entity = typeof(BodyStyleConfig).Name,
                Payload = base.Serializer.Serialize(entity)
            });

            var bodyTypeRepositoryService = Repositories.GetRepositoryService<BodyType>() as IVcdbSqlServerEfRepositoryService<BodyType>;
            var bodyNumDoorsRepositoryService = Repositories.GetRepositoryService<BodyNumDoors>() as IVcdbSqlServerEfRepositoryService<BodyNumDoors>;

            BodyType bodyType = null;
            BodyNumDoors bodyNumDoors = null;

            if (bodyTypeRepositoryService != null && bodyNumDoorsRepositoryService != null)
            {
                var bodyTypes = await bodyTypeRepositoryService.GetAsync(m => m.Id == entity.BodyTypeId && m.DeleteDate == null, 1);
                if (bodyTypes != null && bodyTypes.Any())
                {
                    bodyType = bodyTypes.First();
                }
                var bodyNumDoorses = await bodyNumDoorsRepositoryService.GetAsync(m => m.Id == entity.BodyNumDoorsId && m.DeleteDate == null, 1);
                if (bodyNumDoorses != null && bodyNumDoorses.Any())
                {
                    bodyNumDoors = bodyNumDoorses.First();
                }

                changeContent = string.Format("{0} / {1}", bodyType.Name, bodyNumDoors.NumDoors);
            }

            // NOTE: change-request-comments-staging perfomed on base

            return await base.SubmitAddChangeRequestAsync(entity, requestedBy, changeRequestItemStagings, comment, attachmentsStagingModels, changeContent);
        }

        public override async Task<long> SubmitUpdateChangeRequestAsync<TId>(BodyStyleConfig entity, TId id,
            string requestedBy,
            ChangeType changeType = ChangeType.None,
            List<ChangeRequestItemStaging> changeRequestItemStagings = null, CommentsStagingModel comment = null, List<AttachmentsModel> attachments = null, string changeContent = null)
        {
            if (entity.BodyTypeId.Equals(default(int))
                || entity.BodyNumDoorsId.Equals(default(int)))
            {
                throw new ArgumentException(nameof(entity));
            }

            if (id.Equals(default(TId)))
            {
                throw new ArgumentNullException(nameof(id));
            }

            if (!entity.Id.Equals(id))
            {
                throw new ArgumentException("Invalid Body Style Config Id");
            }

            var bodyStyleConfigFromDb = await FindAsync(id);
            if (bodyStyleConfigFromDb == null)
            {
                throw new NoRecordFound("No Body Style Config exist");
            }

            var existingEntity = new BodyStyleConfig()
            {
                Id = bodyStyleConfigFromDb.Id,
                BodyNumDoorsId = bodyStyleConfigFromDb.BodyNumDoorsId,
                BodyTypeId = bodyStyleConfigFromDb.BodyTypeId,
                ChangeRequestId = bodyStyleConfigFromDb.ChangeRequestId,
                DeleteDate = bodyStyleConfigFromDb.DeleteDate,
                InsertDate = bodyStyleConfigFromDb.InsertDate,
                LastUpdateDate = bodyStyleConfigFromDb.LastUpdateDate,
                VehicleToBodyStyleConfigCount = bodyStyleConfigFromDb.VehicleToBodyStyleConfigCount
            };

            changeRequestItemStagings = new List<ChangeRequestItemStaging>();

            // Validation check for update of existing body style config
            await ValidateConfigurationDoesNotMatchWithExistingBodyStyleConfig(entity);
            await ValidateNoChangeRequestExistsWithSameConfiguration(entity);
            await ValidateBodyStyleConfigLookUpHasNoChangeRequest(entity);
            await ValidateBodyStyleConfigDependentHasNoChangeRequest(entity);

            entity.InsertDate = bodyStyleConfigFromDb.InsertDate;

            changeRequestItemStagings.Add(new ChangeRequestItemStaging()
            {
                ChangeType = ChangeType.Modify,
                EntityId = entity.Id.ToString(),
                CreatedDateTime = DateTime.UtcNow,
                Entity = typeof(BodyStyleConfig).Name,
                Payload = base.Serializer.Serialize(entity),
                ExistingPayload = base.Serializer.Serialize(existingEntity)
            });

            changeContent = string.Format("{0} / {1}"
                , bodyStyleConfigFromDb.BodyType.Name
                , bodyStyleConfigFromDb.BodyNumDoors.NumDoors);

            // NOTE: change-request-comments-staging perfomed on base

            var changeRequestId = await base.SubmitUpdateChangeRequestAsync(entity, id, requestedBy, ChangeType.Modify, changeRequestItemStagings, comment, attachments, changeContent);

            bodyStyleConfigFromDb.ChangeRequestId = changeRequestId;
            _bodyStyleConfigRepositoryService.Update(bodyStyleConfigFromDb);
            Repositories.SaveChanges();

            //NOTE: updating change request id in child dependent tables is not valid

            IList<VehicleToBodyStyleConfig> existingVehicleToBodyStyleConfigs =
                await
                    base.Repositories.GetRepositoryService<VehicleToBodyStyleConfig>()
                        .GetAsync(item => item.BodyStyleConfigId.Equals(bodyStyleConfigFromDb.Id) && item.DeleteDate == null);

            if (existingVehicleToBodyStyleConfigs == null || existingVehicleToBodyStyleConfigs.Count == 0)
            {
                var vehicleToBodyStyleConfigSearchResult = await _vehicleToBodyStyleConfigSearchService.SearchAsync(null, $"bodyStyleConfigId eq {bodyStyleConfigFromDb.Id}");
                var existingVehicleToBodyStyleConfigDocuments = vehicleToBodyStyleConfigSearchResult.Documents;
                if (existingVehicleToBodyStyleConfigDocuments != null && existingVehicleToBodyStyleConfigDocuments.Any())
                {
                    foreach (var existingVehicleToBodyStyleConfigDocument in existingVehicleToBodyStyleConfigDocuments)
                    {
                        //existingVehicleToBodyStyleConfigDocument.VehicleToBodyStyleConfigId must be a GUID string
                        await _vehicleToBodyStyleConfigIndexingService.UpdateBodyStyleConfigChangeRequestIdAsync(existingVehicleToBodyStyleConfigDocument.VehicleToBodyStyleConfigId, changeRequestId);
                    }
                }
            }
            else
            {
                //NOTE: bodyStyleConfigFromDb.VehicleToBodyStyleConfigs will be null because it is not lazy loaded
                foreach (var vehicleToBodyStyleConfig in existingVehicleToBodyStyleConfigs)
                {
                    await _vehicleToBodyStyleConfigIndexingService.UpdateBodyStyleConfigChangeRequestIdAsync(vehicleToBodyStyleConfig.Id.ToString(), changeRequestId);
                }
            }

            return changeRequestId;
        }

        public override async Task<long> SubmitReplaceChangeRequestAsync<TId>(BodyStyleConfig entity, TId id, string requestedBy, List<ChangeRequestItemStaging> changeRequestItemStagings = null, CommentsStagingModel comment = null, List<AttachmentsModel> attachments = null, string changeContent = null)
        {
            if (entity.BodyTypeId.Equals(default(int))
                || entity.BodyNumDoorsId.Equals(default(int)))
            {
                throw new ArgumentException(nameof(entity));
            }

            if (id.Equals(default(TId)))
            {
                throw new ArgumentNullException(nameof(id));
            }

            if (!entity.Id.Equals(id))
            {
                throw new ArgumentException("Invalid Body Style Config Id");
            }

            var bodyStyleConfigFromDb = await FindAsync(id);
            if (bodyStyleConfigFromDb == null)
            {
                throw new NoRecordFound("No Body Style Config exist");
            }

            changeRequestItemStagings = changeRequestItemStagings ?? new List<ChangeRequestItemStaging>();

            await ValidateBodyStyleConfigLookUpHasNoChangeRequest(entity);
            await ValidateBodyStyleConfigDependentHasNoChangeRequest(entity);

            //Fill in the existing values to avoid being overwritten when final approve in change review screen.
            var vehicleToBodyStyleConfigRepository = base.Repositories.GetRepositoryService<VehicleToBodyStyleConfig>();
            foreach (var vehicleToBodyStyleConfig in entity.VehicleToBodyStyleConfigs)
            {
                //var vehicleToBodyStyleConfigFromDb = await vehicleToBodyStyleConfigRepository.FindAsync(vehicleToBodyStyleConfig.Id);
                var vehicleToBodyStyleConfigFromDb = await vehicleToBodyStyleConfigRepository.GetAsync(x => x.Id == vehicleToBodyStyleConfig.Id && x.DeleteDate == null);
                if (vehicleToBodyStyleConfigFromDb == null || !vehicleToBodyStyleConfigFromDb.Any()) continue;

                vehicleToBodyStyleConfig.InsertDate = vehicleToBodyStyleConfigFromDb.First().InsertDate;
            }

            changeRequestItemStagings.AddRange(entity.VehicleToBodyStyleConfigs.Select(item => new ChangeRequestItemStaging()
            {
                ChangeType = ChangeType.Modify,
                EntityId = item.Id.ToString(),
                CreatedDateTime = DateTime.UtcNow,
                Entity = typeof(VehicleToBodyStyleConfig).Name,
                Payload = base.Serializer.Serialize(item),
                EntityFullName = typeof(VehicleToBodyStyleConfig).AssemblyQualifiedName
            }));

            changeContent = string.Format("{0} / {1}"
               , bodyStyleConfigFromDb.BodyType.Name
               , bodyStyleConfigFromDb.BodyNumDoors.NumDoors);

            var changeRequestId = await base.SubmitReplaceChangeRequestAsync(entity, id, requestedBy, changeRequestItemStagings, comment, attachments, changeContent);

            bodyStyleConfigFromDb.ChangeRequestId = changeRequestId;
            _bodyStyleConfigRepositoryService.Update(bodyStyleConfigFromDb);
            Repositories.SaveChanges();

            //NOTE: all vehicletobodystyleconfigs linked to the bodystyleconfig need to be updated with BodyStyleConfigChangeRequestId = CR
            var linkedVehicleToBodyStyleConfigs = await vehicleToBodyStyleConfigRepository.GetAsync(x => x.BodyStyleConfigId == bodyStyleConfigFromDb.Id && x.DeleteDate == null);

            foreach (var vehicleToBodyStyleConfig in linkedVehicleToBodyStyleConfigs)
            {
                await _vehicleToBodyStyleConfigIndexingService.UpdateBodyStyleConfigChangeRequestIdAsync(vehicleToBodyStyleConfig.Id.ToString(), changeRequestId);
            }

            return changeRequestId;
        }

        public override async Task<long> SubmitDeleteChangeRequestAsync<TId>(BodyStyleConfig entity, TId id, string requestedBy,
            List<ChangeRequestItemStaging> changeRequestItemStagings = null, CommentsStagingModel comment = null, List<AttachmentsModel> attachmentsStaging = null, string changeContent = null)
        {
            if (id.Equals(default(TId)))
            {
                throw new ArgumentNullException(nameof(id));
            }

            var bodyStyleConfigFromDb = await FindAsync(id);
            if (bodyStyleConfigFromDb == null)
            {
                throw new NoRecordFound("No Body Style Config exist");
            }

            BodyStyleConfig bodyStyleConfigSubmit = new BodyStyleConfig()
            {
                Id = bodyStyleConfigFromDb.Id,
                BodyTypeId = bodyStyleConfigFromDb.BodyTypeId,
                BodyNumDoorsId = bodyStyleConfigFromDb.BodyNumDoorsId,
                VehicleToBodyStyleConfigCount = entity.VehicleToBodyStyleConfigCount
            };

            changeRequestItemStagings = new List<ChangeRequestItemStaging>();

            await ValidateNoChangeRequestExistsWithSameConfiguration(bodyStyleConfigFromDb);
            await ValidateBodyStyleConfigDependentHasNoChangeRequest(bodyStyleConfigFromDb);

            changeRequestItemStagings.Add(new ChangeRequestItemStaging()
            {
                ChangeType = ChangeType.Delete,
                EntityId = bodyStyleConfigSubmit.Id.ToString(),
                CreatedDateTime = DateTime.UtcNow,
                Entity = typeof(BodyStyleConfig).Name,
                Payload = base.Serializer.Serialize(bodyStyleConfigSubmit)
            });

            IList<VehicleToBodyStyleConfig> existingVehicleToBodyStyleConfigs =
                await
                    base.Repositories.GetRepositoryService<VehicleToBodyStyleConfig>()
                        .GetAsync(item => item.BodyStyleConfigId.Equals(bodyStyleConfigSubmit.Id) && item.DeleteDate == null);

            if (existingVehicleToBodyStyleConfigs != null && existingVehicleToBodyStyleConfigs.Count > 0)
            {
                changeRequestItemStagings.AddRange(
                    existingVehicleToBodyStyleConfigs.Select(vehicleToBodyStyleConfig => new ChangeRequestItemStaging()
                    {
                        ChangeType = ChangeType.Delete,
                        EntityId = vehicleToBodyStyleConfig.Id.ToString(),
                        CreatedDateTime = DateTime.UtcNow,
                        Entity = typeof(VehicleToBodyStyleConfig).Name,
                        Payload = base.Serializer.Serialize(new VehicleToBodyStyleConfig()
                        {
                            Id = vehicleToBodyStyleConfig.Id,
                            BodyStyleConfigId = vehicleToBodyStyleConfig.BodyStyleConfigId,
                            VehicleId = vehicleToBodyStyleConfig.VehicleId
                        })
                    }));
            }

            changeContent = string.Format("{0} / {1}"
               , bodyStyleConfigFromDb.BodyType.Name
               , bodyStyleConfigFromDb.BodyNumDoors.NumDoors);

            var changeRequestId = await base.SubmitDeleteChangeRequestAsync(bodyStyleConfigSubmit, id, requestedBy, changeRequestItemStagings, comment, attachmentsStaging, changeContent);

            bodyStyleConfigFromDb.ChangeRequestId = changeRequestId;
            _bodyStyleConfigRepositoryService.Update(bodyStyleConfigFromDb);
            Repositories.SaveChanges();

            //NOTE: updating change request id in child dependent tables is not valid
            if (existingVehicleToBodyStyleConfigs == null || existingVehicleToBodyStyleConfigs.Count == 0)
            {
                var vehicleToBodyStyleConfigSearchResult = await _vehicleToBodyStyleConfigSearchService.SearchAsync(null, $"bodyStyleConfigId eq {bodyStyleConfigFromDb.Id}");
                var existingVehicleToBodyStyleConfigDocuments = vehicleToBodyStyleConfigSearchResult.Documents;
                if (existingVehicleToBodyStyleConfigDocuments != null && existingVehicleToBodyStyleConfigDocuments.Any())
                {
                    foreach (var existingVehicleToBodyStyleConfigDocument in existingVehicleToBodyStyleConfigDocuments)
                    {
                        //existingVehicleToBodyStyleConfigDocument.VehicleToBodyStyleConfigId must be a GUID string
                        await _vehicleToBodyStyleConfigIndexingService.UpdateBodyStyleConfigChangeRequestIdAsync(existingVehicleToBodyStyleConfigDocument.VehicleToBodyStyleConfigId, changeRequestId);
                    }
                }
            }
            else
            {
                //NOTE: bodyStyleConfigFromDb.VehicleToBodyStyleConfigs will be null because it is not lazy loaded
                foreach (var vehicleToBodyStyleConfig in existingVehicleToBodyStyleConfigs)
                {
                    await _vehicleToBodyStyleConfigIndexingService.UpdateBodyStyleConfigChangeRequestIdAsync(vehicleToBodyStyleConfig.Id.ToString(), changeRequestId);
                }
            }

            return changeRequestId;
        }

        public new async Task<BodyStyleConfigChangeRequestStagingModel> GetChangeRequestStaging<TId>(TId changeRequestId)
        {
            var result = await
                ChangeRequestBusinessService.GetChangeRequestStagingByChangeRequestIdAsync<BodyStyleConfig, TId>(changeRequestId);

            List<VehicleToBodyStyleConfig> vehicleToBodyStyleConfigs = null;
            if (result.StagingItem.ChangeType == ChangeType.Replace.ToString())
            {
                result.EntityStaging = result.EntityCurrent;
                var changeRequestIdLong = Convert.ToInt64(changeRequestId);
                var vehicleToBodyStyleConfigChangeRequestItems = await this.ChangeRequestBusinessService.GetChangeRequestItemStagingsAsync(item => item.ChangeRequestId == changeRequestIdLong);

                if (vehicleToBodyStyleConfigChangeRequestItems != null && vehicleToBodyStyleConfigChangeRequestItems.Any())
                {
                    var vehicleToBodyStyleConfigIds = vehicleToBodyStyleConfigChangeRequestItems.Select(item => Convert.ToInt32(item.EntityId)).ToList();
                    vehicleToBodyStyleConfigs = await base.Repositories.GetRepositoryService<VehicleToBodyStyleConfig>()
                        .GetAsync(item => vehicleToBodyStyleConfigIds.Any(id => id == item.Id) && item.DeleteDate == null);

                    //1. Extract the replacement body style config to body style config id from the first deserialized vehicleToBodyStyleConfigChangeRequestItems
                    var vehicleToBodyStyleConfig = Serializer.Deserialize<VehicleToBodyStyleConfig>(vehicleToBodyStyleConfigChangeRequestItems[0].Payload);

                    //2. fill result.EntityStaging with the replacement Body Style config details
                    result.EntityStaging = await FindAsync(vehicleToBodyStyleConfig.BodyStyleConfigId);

                    // 3. fill currentEntity
                    result.EntityCurrent = await FindAsync(result.StagingItem.EntityId);
                }
                else
                {
                    var vehicleToBodyStyleConfigChangeRequestStoreItems = await this.ChangeRequestBusinessService.GetChangeRequestItemAsync(item =>
                        item.ChangeRequestId == changeRequestIdLong);

                    if (vehicleToBodyStyleConfigChangeRequestStoreItems != null && vehicleToBodyStyleConfigChangeRequestStoreItems.Any())
                    {
                        var vehicleToBodyStyleConfigIds = vehicleToBodyStyleConfigChangeRequestStoreItems.Select(item => Convert.ToInt32(item.EntityId)).ToList();
                        vehicleToBodyStyleConfigs =
                            await
                                base.Repositories.GetRepositoryService<VehicleToBodyStyleConfig>()
                                    .GetAsync(item => vehicleToBodyStyleConfigIds.Any(id => id == item.Id) && item.DeleteDate == null);

                        //1. Extract the replacement base vehicle id from the first deserialized vehicleChangeRequestItems
                        var vehicleToBodyStyleConfig = Serializer.Deserialize<VehicleToBodyStyleConfig>(vehicleToBodyStyleConfigChangeRequestStoreItems[0].Payload);

                        //2. fill result.EntityStaging with the replacement base vehicle details
                        result.EntityStaging = await FindAsync(vehicleToBodyStyleConfig.BodyStyleConfigId);

                        // 3. fill currentEntity
                        result.EntityCurrent = await FindAsync(result.StagingItem.EntityId);
                    }
                }
            }

            BodyStyleConfigChangeRequestStagingModel staging = new BodyStyleConfigChangeRequestStagingModel
            {
                EntityCurrent = result.EntityCurrent,
                EntityStaging = result.EntityStaging,
                // RequestorComments = result.RequestorComments,
                //ReviewerComments = result.ReviewerComments,
                Comments = result.Comments,
                StagingItem = result.StagingItem,
                ReplacementVehicleToBodyStyleConfigs = vehicleToBodyStyleConfigs,
                Attachments = result.Attachments
            };
            return staging;
        }

        public override AssociationCount AssociatedCount(string payload)
        {
            AssociationCount count = new AssociationCount();
            if (payload != null)
            {
                BodyStyleConfig deserializedEntry = Serializer.Deserialize<BodyStyleConfig>(payload);
                count.VehicleToBodyStyleConfigCount = deserializedEntry.VehicleToBodyStyleConfigCount;
            }
            return count;
        }

        public override async Task ClearChangeRequestId(long changeRequestId)
        {
            var entities = await _bodyStyleConfigRepositoryService.GetAsync(x => x.ChangeRequestId == changeRequestId && x.DeleteDate == null);

            if (entities != null && entities.Any())
            {
                var entity = entities.First();   //can only be a single item
                entity.ChangeRequestId = null;
                _bodyStyleConfigRepositoryService.Update(entity);
                Repositories.SaveChanges();
            }

            var vehicleToBodyStyleConfigRepositoryService = base.Repositories.GetRepositoryService<VehicleToBodyStyleConfig>();
            List<VehicleToBodyStyleConfig> approvedReplacementVehicleToBodyStyleConfigs = await vehicleToBodyStyleConfigRepositoryService.GetAsync(m => m.ChangeRequestId == changeRequestId);

            if (approvedReplacementVehicleToBodyStyleConfigs != null && approvedReplacementVehicleToBodyStyleConfigs.Any())
            {
                foreach (var approvedReplacementVehicleToBodyStyleConfig in approvedReplacementVehicleToBodyStyleConfigs)
                {
                    approvedReplacementVehicleToBodyStyleConfig.ChangeRequestId = null;
                    vehicleToBodyStyleConfigRepositoryService.Update(approvedReplacementVehicleToBodyStyleConfig);
                }
                Repositories.SaveChanges();
            }
        }

        public override async Task<List<BodyStyleConfig>> GetPendingAddChangeRequests(Expression<Func<BodyStyleConfig, bool>> whereCondition = null, int topCount = 0)
        {
            var existingChangeRequestItemStagings = await ChangeRequestBusinessService.GetChangeRequestItemStagingsAsync(
                x => x.Entity.Equals(typeof(BodyStyleConfig).Name, StringComparison.CurrentCultureIgnoreCase)
                && x.ChangeType == ChangeType.Add);

            List<BodyStyleConfig> pendingBodyStyleConfigs = new List<BodyStyleConfig>();

            foreach (var existingChangeRequestItemStaging in existingChangeRequestItemStagings)
            {
                var pendingBodyStyleConfig = Serializer.Deserialize<BodyStyleConfig>(existingChangeRequestItemStaging.Payload);
                pendingBodyStyleConfig.ChangeRequestId = existingChangeRequestItemStaging.ChangeRequestId;
                pendingBodyStyleConfigs.Add(pendingBodyStyleConfig);
            }

            if (whereCondition != null)
            {
                var predicate = whereCondition.Compile();

                pendingBodyStyleConfigs = pendingBodyStyleConfigs.Where(predicate).ToList();
            }

            return pendingBodyStyleConfigs;
        }

        private async Task ValidateConfigurationDoesNotMatchWithExistingBodyStyleConfig(BodyStyleConfig bodyStyleConfig)
        {
            // 1. Validate that there is no body style config with same configuration
            IList<BodyStyleConfig> bodyStyleConfigFromDb =
                await
                    _bodyStyleConfigRepositoryService.GetAsync(
                        x => x.BodyTypeId.Equals(bodyStyleConfig.BodyTypeId)
                             && x.BodyNumDoorsId.Equals(bodyStyleConfig.BodyNumDoorsId)
                             && x.DeleteDate == null);

            if (bodyStyleConfigFromDb != null && bodyStyleConfigFromDb.Count > 0)
            {
                throw new RecordAlreadyExist("Body Style Config with same configuration already exists.");
            }
        }

        private async Task ValidateNoChangeRequestExistsWithSameConfiguration(BodyStyleConfig bodyStyleConfig)
        {
            // 2. Validate that there is no open CR with same configuration
            var changeRequestId = await ChangeRequestBusinessService.ChangeRequestExist<BodyStyleConfig>(x =>
                x.BodyTypeId.Equals(bodyStyleConfig.BodyTypeId)
                && x.BodyNumDoorsId.Equals(bodyStyleConfig.BodyNumDoorsId));
            if (changeRequestId > 0)
            {
                throw new ChangeRequestExistException("There is already an open CR ID " + changeRequestId + " with same body style configuration.");
            }
        }

        private async Task ValidateBodyStyleConfigLookUpHasNoChangeRequest(BodyStyleConfig bodyStyleConfig)
        {
            // 3. Validate that there are no open CR for any of the items that make of configuration. Body Type and Body Number Door
            {
                // Body Number Door
                var changerequestid =
                    await
                        ChangeRequestBusinessService.ChangeRequestExist(typeof(BodyNumDoors).Name,
                            bodyStyleConfig.BodyNumDoorsId);
                if (changerequestid > 0)
                {
                    throw new ChangeRequestExistException(
                        $"There is already an open CR ID {changerequestid} for Body Number Doors Length ID : {bodyStyleConfig.BodyNumDoorsId}.");
                }

                // Body Type
                var changerequestID = await ChangeRequestBusinessService.ChangeRequestExist(typeof(BodyType).Name, bodyStyleConfig.BodyTypeId);
                ;
                if (changerequestID > 0)
                {
                    throw new ChangeRequestExistException(
                        $"There is already an open CR ID {changerequestID} for Body Type ID : {bodyStyleConfig.BodyTypeId}.");
                }
            }
        }

        private async Task ValidateBodyStyleConfigDependentHasNoChangeRequest(BodyStyleConfig bodyStyleConfig)
        {
            var changerequestid = await ChangeRequestBusinessService.ChangeRequestExist<VehicleToBodyStyleConfig>(x =>
                x.BodyStyleConfigId.Equals(bodyStyleConfig.Id));
            if (changerequestid > 0)
            {
                throw new ChangeRequestExistException("There is already an open CR ID " + changerequestid + " for Vehicle to Body Style Configuration using this Body Style Config.");
            }
        }

        private async Task<BodyStyleConfig> FindAsync<TKey>(TKey id)
        {
            var entityId = Convert.ToInt32(id);
            var entities = await _bodyStyleConfigRepositoryService.GetAsync(m => m.Id == entityId && m.DeleteDate == null, 1);
            if (entities != null && entities.Any())
            {
                return entities.First();
            }
            return null;
        }
    }
}
