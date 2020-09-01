using System;
using System.Linq;
using System.Threading.Tasks;
using AutoCare.Product.Application.RepositoryServices;
using AutoCare.Product.Infrastructure.ExceptionHandler;
using AutoCare.Product.Infrastructure.RepositoryService;
using AutoCare.Product.Infrastructure.Serializer;
using AutoCare.Product.Vcdb.Model;
using System.Collections.Generic;
using System.Linq.Expressions;
using AutoCare.Product.Application.Models.BusinessModels;
using AutoCare.Product.Application.Infrastructure.ExceptionHelpers;
using AutoCare.Product.VcdbSearch.Indexing.ApplicationService;
using AutoCare.Product.VcdbSearch.ApplicationService;

namespace AutoCare.Product.Application.BusinessServices.Vcdb
{
    public class BrakeConfigBusinessService : VcdbBusinessService<BrakeConfig>, IBrakeConfigBusinessService
    {
        private readonly IRepositoryService<BrakeConfig> _brakeConfigRepositoryService = null;
        private readonly IVehicleToBrakeConfigIndexingService _vehicleToBrakeConfigIndexingService = null;
        private readonly IVehicleToBrakeConfigSearchService _vehicletoBrakeConfigSearchService;

        public BrakeConfigBusinessService(IVcdbUnitOfWork vcdbRepositoryService,
            IVcdbChangeRequestBusinessService vcdbChangeRequestBusinessService,
            ITextSerializer serializer, IVehicleToBrakeConfigIndexingService vehicleToBrakeConfigIndexingService = null,
            IVehicleToBrakeConfigSearchService vehicletoBrakeConfigSearchService = null)
            : base(vcdbRepositoryService, vcdbChangeRequestBusinessService, serializer)
        {
            _brakeConfigRepositoryService = Repositories.GetRepositoryService<BrakeConfig>();
            _vehicleToBrakeConfigIndexingService = vehicleToBrakeConfigIndexingService;
            _vehicletoBrakeConfigSearchService = vehicletoBrakeConfigSearchService;
        }

        public override async Task<List<BrakeConfig>> GetAllAsync(int topCount = 0)
        {
            return await _brakeConfigRepositoryService.GetAsync(x => x.DeleteDate == null, topCount);
        }

        public override async Task<BrakeConfig> GetAsync<TKey>(TKey id)
        {
            if (id.Equals(default(TKey)))
            {
                throw new ArgumentNullException(nameof(id));
            }

            var brakeConfig = await FindAsync(id);

            if (brakeConfig == null)
            {
                throw new NoRecordFound("Record Not Found");
            }

            brakeConfig.VehicleToBrakeConfigCount =
                await _brakeConfigRepositoryService.GetCountAsync(brakeConfig, x => x.VehicleToBrakeConfigs, y => y.DeleteDate == null);

            return brakeConfig;
        }

        public override async Task<long> SubmitAddChangeRequestAsync(BrakeConfig entity, string requestedBy,
            List<ChangeRequestItemStaging> changeRequestItemStagings = null, CommentsStagingModel comment = null,
            List<AttachmentsModel> attachmentsStagingModels = null, string changeContent = null)
        {
            if (entity.BrakeSystemId.Equals(default(int))
                || entity.BrakeABSId.Equals(default(int))
                || entity.FrontBrakeTypeId.Equals(default(int))
                || entity.RearBrakeTypeId.Equals(default(int)))
            {
                throw new ArgumentException(nameof(entity));
            }

            changeRequestItemStagings = new List<ChangeRequestItemStaging>();
            // Validation check for insert of new brake config

            await ValidateConfigurationDoesNotMatchWithExistingBrakeConfig(entity);
            await ValidateNoChangeRequestExistsWithSameConfiguration(entity);
            await ValidateBrakeConfigLookUpHasNoChangeRequest(entity);


            changeRequestItemStagings.Add(new ChangeRequestItemStaging()
            {
                ChangeType = ChangeType.Add,
                EntityId = entity.Id.ToString(),
                CreatedDateTime = DateTime.UtcNow,
                Entity = typeof(BrakeConfig).Name,
                Payload = base.Serializer.Serialize(entity)
            });

            var brakeTypeRepositoryService = Repositories.GetRepositoryService<BrakeType>() as IVcdbSqlServerEfRepositoryService<BrakeType>;
            var brakeAbsRepositoryService = Repositories.GetRepositoryService<BrakeABS>() as IVcdbSqlServerEfRepositoryService<BrakeABS>;
            var brakeSystemRepositoryService = Repositories.GetRepositoryService<BrakeSystem>() as IVcdbSqlServerEfRepositoryService<BrakeSystem>;

            BrakeType frontBrakeType = null;
            BrakeType rearBrakeType = null;
            BrakeABS brakeAbs = null;
            BrakeSystem brakeSystem = null;

            if (brakeTypeRepositoryService != null && brakeAbsRepositoryService != null && brakeSystemRepositoryService != null)
            {
                var frontBrakeTypes = await brakeTypeRepositoryService.GetAsync(m => m.Id == entity.FrontBrakeTypeId && m.DeleteDate == null, 1);
                if (frontBrakeTypes != null && frontBrakeTypes.Any())
                {
                    frontBrakeType = frontBrakeTypes.First();
                }
                var rearBrakeTypes = await brakeTypeRepositoryService.GetAsync(m => m.Id == entity.RearBrakeTypeId && m.DeleteDate == null, 1);
                if (rearBrakeTypes != null && rearBrakeTypes.Any())
                {
                    rearBrakeType = rearBrakeTypes.First();
                }
                var brakeAbses = await brakeAbsRepositoryService.GetAsync(m => m.Id == entity.BrakeABSId && m.DeleteDate == null, 1);
                if (brakeAbses != null && brakeAbses.Any())
                {
                    brakeAbs = brakeAbses.First();
                }
                var brakeSystems = await brakeSystemRepositoryService.GetAsync(m => m.Id == entity.BrakeSystemId && m.DeleteDate == null, 1);
                if (brakeSystems != null && brakeSystems.Any())
                {
                    brakeSystem = brakeSystems.First();
                }

                changeContent = string.Format("{0} / {1} / {2} / {3}", frontBrakeType.Name, rearBrakeType.Name, brakeAbs.Name, brakeSystem.Name);
            }

            // NOTE: change-request-comments-staging perfomed on base

            return await base.SubmitAddChangeRequestAsync(entity, requestedBy, changeRequestItemStagings, comment, attachmentsStagingModels, changeContent);
        }

        public override async Task<long> SubmitUpdateChangeRequestAsync<TId>(BrakeConfig entity, TId id,
            string requestedBy,
            ChangeType changeType = ChangeType.None,
            List<ChangeRequestItemStaging> changeRequestItemStagings = null, CommentsStagingModel comment = null,
            List<AttachmentsModel> attachments = null, string changeContent = null)
        {
            if (entity.BrakeSystemId.Equals(default(int))
                || entity.BrakeABSId.Equals(default(int))
                || entity.FrontBrakeTypeId.Equals(default(int))
                || entity.RearBrakeTypeId.Equals(default(int)))
            {
                throw new ArgumentException(nameof(entity));
            }

            if (id.Equals(default(TId)))
            {
                throw new ArgumentNullException(nameof(id));
            }

            if (!entity.Id.Equals(id))
            {
                throw new ArgumentException("Invalid Brake Config Id");
            }

            var brakeConfigFromDb = await FindAsync(id);
            if (brakeConfigFromDb == null)
            {
                throw new NoRecordFound("No Brake Config exist");
            }

            var existingEntity = new BrakeConfig()
            {
                Id = brakeConfigFromDb.Id,
                BrakeABSId = brakeConfigFromDb.BrakeABSId,
                BrakeSystemId = brakeConfigFromDb.BrakeSystemId,
                FrontBrakeTypeId = brakeConfigFromDb.FrontBrakeTypeId,
                RearBrakeTypeId = brakeConfigFromDb.RearBrakeTypeId,
                ChangeRequestId = brakeConfigFromDb.ChangeRequestId,
                DeleteDate = brakeConfigFromDb.DeleteDate,
                InsertDate = brakeConfigFromDb.InsertDate,
                LastUpdateDate = brakeConfigFromDb.LastUpdateDate,
                VehicleToBrakeConfigCount = brakeConfigFromDb.VehicleToBrakeConfigCount
            };

            changeRequestItemStagings = new List<ChangeRequestItemStaging>();

            // Validation check for update of existing brake config
            await ValidateConfigurationDoesNotMatchWithExistingBrakeConfig(entity);
            await ValidateNoChangeRequestExistsWithSameConfiguration(entity);
            await ValidateBrakeConfigLookUpHasNoChangeRequest(entity);
            await ValidateBrakeConfigDependentHasNoChangeRequest(entity);

            entity.InsertDate = brakeConfigFromDb.InsertDate;

            changeRequestItemStagings.Add(new ChangeRequestItemStaging()
            {
                ChangeType = ChangeType.Modify,
                EntityId = entity.Id.ToString(),
                CreatedDateTime = DateTime.UtcNow,
                Entity = typeof(BrakeConfig).Name,
                Payload = base.Serializer.Serialize(entity),
                ExistingPayload = base.Serializer.Serialize(existingEntity)
            });

            changeContent = string.Format("{0} / {1} / {2} / {3}"
                , brakeConfigFromDb.FrontBrakeType.Name
                , brakeConfigFromDb.RearBrakeType.Name
                , brakeConfigFromDb.BrakeABS.Name
                , brakeConfigFromDb.BrakeSystem.Name);

            // NOTE: change-request-comments-staging perfomed on base
            var changeRequestId = await base.SubmitUpdateChangeRequestAsync(entity, id, requestedBy, ChangeType.Modify, changeRequestItemStagings, comment, attachments, changeContent);

            brakeConfigFromDb.ChangeRequestId = changeRequestId;
            _brakeConfigRepositoryService.Update(brakeConfigFromDb);
            Repositories.SaveChanges();

            //NOTE: updating change request id in child dependent tables is not valid

            IList<VehicleToBrakeConfig> existingVehicleToBrakeConfigs =
                await
                    base.Repositories.GetRepositoryService<VehicleToBrakeConfig>()
                        .GetAsync(item => item.BrakeConfigId.Equals(brakeConfigFromDb.Id) && item.DeleteDate == null);

            if (existingVehicleToBrakeConfigs == null || existingVehicleToBrakeConfigs.Count == 0)
            {
                var vehicleToBrakeConfigSearchResult = await _vehicletoBrakeConfigSearchService.SearchAsync(null, $"brakeConfigId eq {brakeConfigFromDb.Id}");
                var existingVehicleToBrakeConfigDocuments = vehicleToBrakeConfigSearchResult.Documents;
                if (existingVehicleToBrakeConfigDocuments != null && existingVehicleToBrakeConfigDocuments.Any())
                {
                    foreach (var existingVehicleToBrakeConfigDocument in existingVehicleToBrakeConfigDocuments)
                    {
                        //existingVehicleDocument.VehicleId must be a GUID string
                        await _vehicleToBrakeConfigIndexingService.UpdateBrakeConfigChangeRequestIdAsync(existingVehicleToBrakeConfigDocument.VehicleToBrakeConfigId, changeRequestId);
                    }
                }
            }
            else
            {
                //NOTE: brakeConfigFromDb.VehicleToBrakeConfigs will be null because it is not lazy loaded
                foreach (var vehicleToBrakeConfig in existingVehicleToBrakeConfigs)
                {
                    await _vehicleToBrakeConfigIndexingService.UpdateBrakeConfigChangeRequestIdAsync(vehicleToBrakeConfig.Id.ToString(), changeRequestId);
                }
            }

            return changeRequestId;
        }

        public override async Task<long> SubmitReplaceChangeRequestAsync<TId>(BrakeConfig entity, TId id, string requestedBy, List<ChangeRequestItemStaging> changeRequestItemStagings = null, CommentsStagingModel comment = null, List<AttachmentsModel> attachments = null, string changeContent = null)
        {
            if (entity.BrakeSystemId.Equals(default(int))
                || entity.BrakeABSId.Equals(default(int))
                || entity.FrontBrakeTypeId.Equals(default(int))
                || entity.RearBrakeTypeId.Equals(default(int)))
            {
                throw new ArgumentException(nameof(entity));
            }

            if (id.Equals(default(TId)))
            {
                throw new ArgumentNullException(nameof(id));
            }

            if (!entity.Id.Equals(id))
            {
                throw new ArgumentException("Invalid Brake Config Id");
            }

            var brakeConfigFromDb = await FindAsync(id);
            if (brakeConfigFromDb == null)
            {
                throw new NoRecordFound("No Brake Config exist");
            }

            changeRequestItemStagings = changeRequestItemStagings ?? new List<ChangeRequestItemStaging>();

            await ValidateBrakeConfigLookUpHasNoChangeRequest(entity);
            await ValidateBrakeConfigDependentHasNoChangeRequest(entity);
            //await ValidateBrakeConfigHasNoReplacementChangeRequest(entity);       //not required

            //Fill in the existing values to avoid being overwritten when final approve in change review screen.
            var vehicleToBrakeConfigRepository = base.Repositories.GetRepositoryService<VehicleToBrakeConfig>();
            foreach (var vehicleToBrakeConfig in entity.VehicleToBrakeConfigs)
            {
                //var vehicleToBrakeConfigFromDb = await vehicleToBrakeConfigRepository.FindAsync(vehicleToBrakeConfig.Id);
                var vehicleToBrakeConfigFromDb = await vehicleToBrakeConfigRepository.GetAsync(x => x.Id == vehicleToBrakeConfig.Id && x.DeleteDate == null);
                if (vehicleToBrakeConfigFromDb == null || !vehicleToBrakeConfigFromDb.Any()) continue;

                vehicleToBrakeConfig.InsertDate = vehicleToBrakeConfigFromDb.First().InsertDate;
            }

            changeRequestItemStagings.AddRange(entity.VehicleToBrakeConfigs.Select(item => new ChangeRequestItemStaging()
            {
                ChangeType = ChangeType.Modify,
                EntityId = item.Id.ToString(),
                CreatedDateTime = DateTime.UtcNow,
                Entity = typeof(VehicleToBrakeConfig).Name,
                Payload = base.Serializer.Serialize(item),
                EntityFullName = typeof(VehicleToBrakeConfig).AssemblyQualifiedName
            }));

            changeContent = string.Format("{0} / {1} / {2} / {3}"
                , brakeConfigFromDb.FrontBrakeType.Name
                , brakeConfigFromDb.RearBrakeType.Name
                , brakeConfigFromDb.BrakeABS.Name
                , brakeConfigFromDb.BrakeSystem.Name);

            var changeRequestId = await base.SubmitReplaceChangeRequestAsync(entity, id, requestedBy, changeRequestItemStagings, comment, attachments, changeContent);

            brakeConfigFromDb.ChangeRequestId = changeRequestId;
            _brakeConfigRepositoryService.Update(brakeConfigFromDb);
            Repositories.SaveChanges();

            //NOTE: all vehicletobrakeconfigs linked to the brakeconfig need to be updated with BrakeConfigChangeRequestId = CR
            var linkedVehicleToBrakeConfigs = await vehicleToBrakeConfigRepository.GetAsync(x => x.BrakeConfigId == brakeConfigFromDb.Id && x.DeleteDate == null);

            foreach (var vehicleToBrakeConfig in linkedVehicleToBrakeConfigs)
            {
                await _vehicleToBrakeConfigIndexingService.UpdateBrakeConfigChangeRequestIdAsync(vehicleToBrakeConfig.Id.ToString(), changeRequestId);
            }

            return changeRequestId;
        }

        public override async Task<long> SubmitDeleteChangeRequestAsync<TId>(BrakeConfig entity, TId id, string requestedBy,
            List<ChangeRequestItemStaging> changeRequestItemStagings = null, CommentsStagingModel comment = null,
            List<AttachmentsModel> attachmentsStaging = null, string changeContent = null)
        {
            if (id.Equals(default(TId)))
            {
                throw new ArgumentNullException(nameof(id));
            }

            var brakeConfigFromDb = await FindAsync(id);
            if (brakeConfigFromDb == null)
            {
                throw new NoRecordFound("No Brake Config exist");
            }

            BrakeConfig brakeConfigSubmit = new BrakeConfig()
            {
                Id = brakeConfigFromDb.Id,
                FrontBrakeTypeId = brakeConfigFromDb.FrontBrakeTypeId,
                RearBrakeTypeId = brakeConfigFromDb.RearBrakeTypeId,
                BrakeABSId = brakeConfigFromDb.BrakeABSId,
                BrakeSystemId = brakeConfigFromDb.BrakeSystemId,
                VehicleToBrakeConfigCount = entity.VehicleToBrakeConfigCount
            };

            changeRequestItemStagings = new List<ChangeRequestItemStaging>();

            await ValidateNoChangeRequestExistsWithSameConfiguration(brakeConfigFromDb);
            await ValidateBrakeConfigDependentHasNoChangeRequest(brakeConfigFromDb);

            changeRequestItemStagings.Add(new ChangeRequestItemStaging()
            {
                ChangeType = ChangeType.Delete,
                EntityId = brakeConfigSubmit.Id.ToString(),
                CreatedDateTime = DateTime.UtcNow,
                Entity = typeof(BrakeConfig).Name,
                Payload = base.Serializer.Serialize(brakeConfigSubmit)
            });

            IList<VehicleToBrakeConfig> existingVehicleToBrakeConfigs =
                await
                    base.Repositories.GetRepositoryService<VehicleToBrakeConfig>()
                        .GetAsync(item => item.BrakeConfigId.Equals(brakeConfigSubmit.Id) && item.DeleteDate == null);

            if (existingVehicleToBrakeConfigs != null && existingVehicleToBrakeConfigs.Count > 0)
            {
                changeRequestItemStagings.AddRange(
                    existingVehicleToBrakeConfigs.Select(vehicleToBrakeConfig => new ChangeRequestItemStaging()
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

            changeContent = string.Format("{0} / {1} / {2} / {3}"
                , brakeConfigFromDb.FrontBrakeType.Name
                , brakeConfigFromDb.RearBrakeType.Name
                , brakeConfigFromDb.BrakeABS.Name
                , brakeConfigFromDb.BrakeSystem.Name);

            var changeRequestId = await base.SubmitDeleteChangeRequestAsync(brakeConfigSubmit, id, requestedBy, changeRequestItemStagings, comment, attachmentsStaging, changeContent);

            brakeConfigFromDb.ChangeRequestId = changeRequestId;
            _brakeConfigRepositoryService.Update(brakeConfigFromDb);
            Repositories.SaveChanges();

            //NOTE: updating change request id in child dependent tables is not valid

            if (existingVehicleToBrakeConfigs == null || existingVehicleToBrakeConfigs.Count == 0)
            {
                var vehicleToBrakeConfigSearchResult = await _vehicletoBrakeConfigSearchService.SearchAsync(null, $"brakeConfigId eq {brakeConfigFromDb.Id}");
                var existingVehicleToBrakeConfigDocuments = vehicleToBrakeConfigSearchResult.Documents;
                if (existingVehicleToBrakeConfigDocuments != null && existingVehicleToBrakeConfigDocuments.Any())
                {
                    foreach (var existingVehicleToBrakeConfigDocument in existingVehicleToBrakeConfigDocuments)
                    {
                        //existingVehicleDocument.VehicleId must be a GUID string
                        await _vehicleToBrakeConfigIndexingService.UpdateBrakeConfigChangeRequestIdAsync(existingVehicleToBrakeConfigDocument.VehicleToBrakeConfigId, changeRequestId);
                    }
                }
            }
            else
            {
                //NOTE: brakeConfigFromDb.VehicleToBrakeConfigs will be null because it is not lazy loaded
                foreach (var vehicleToBrakeConfig in existingVehicleToBrakeConfigs)
                {
                    await _vehicleToBrakeConfigIndexingService.UpdateBrakeConfigChangeRequestIdAsync(vehicleToBrakeConfig.Id.ToString(), changeRequestId);
                }
            }

            return changeRequestId;
        }

        public new async Task<BrakeConfigChangeRequestStagingModel> GetChangeRequestStaging<TId>(TId changeRequestId)
        {
            var result = await
                ChangeRequestBusinessService.GetChangeRequestStagingByChangeRequestIdAsync<BrakeConfig, TId>(changeRequestId);

            // business specific task.
            // fill value of "EntityCurrent"
            //if (!result.StagingItem.ChangeType.Equals(ChangeType.Add.ToString(), StringComparison.CurrentCultureIgnoreCase))
            //{
            //    result.EntityCurrent = await FindAsync(result.StagingItem.EntityId);
            //}

            List<VehicleToBrakeConfig> vehicleToBrakeConfigs = null;
            if (result.StagingItem.ChangeType == ChangeType.Replace.ToString())
            {
                result.EntityStaging = result.EntityCurrent;
                var changeRequestIdLong = Convert.ToInt64(changeRequestId);
                var vehicleToBrakeConfigChangeRequestItems = await this.ChangeRequestBusinessService.GetChangeRequestItemStagingsAsync(item => item.ChangeRequestId == changeRequestIdLong);

                if (vehicleToBrakeConfigChangeRequestItems != null && vehicleToBrakeConfigChangeRequestItems.Any())
                {
                    var vehicleToBrakeConfigIds = vehicleToBrakeConfigChangeRequestItems.Select(item => Convert.ToInt32(item.EntityId)).ToList();
                    vehicleToBrakeConfigs = await base.Repositories.GetRepositoryService<VehicleToBrakeConfig>()
                        .GetAsync(item => vehicleToBrakeConfigIds.Any(id => id == item.Id) && item.DeleteDate == null);

                    //1. Extract the replacement brake config to brake config id from the first deserialized vehicleToBrakeConfigChangeRequestItems
                    var vehicleToBrakeConfig = Serializer.Deserialize<VehicleToBrakeConfig>(vehicleToBrakeConfigChangeRequestItems[0].Payload);

                    //2. fill result.EntityStaging with the replacement brake config details
                    result.EntityStaging = await FindAsync(vehicleToBrakeConfig.BrakeConfigId);

                    // 3. fill currentEntity
                    result.EntityCurrent = await FindAsync(result.StagingItem.EntityId);
                }
                else
                {
                    var vehicleToBrakeConfigChangeRequestStoreItems = await this.ChangeRequestBusinessService.GetChangeRequestItemAsync(item =>
                        item.ChangeRequestId == changeRequestIdLong);

                    if (vehicleToBrakeConfigChangeRequestStoreItems != null && vehicleToBrakeConfigChangeRequestStoreItems.Any())
                    {
                        var vehicleToBrakeConfigIds = vehicleToBrakeConfigChangeRequestStoreItems.Select(item => Convert.ToInt32(item.EntityId)).ToList();
                        vehicleToBrakeConfigs =
                            await
                                base.Repositories.GetRepositoryService<VehicleToBrakeConfig>()
                                    .GetAsync(item => vehicleToBrakeConfigIds.Any(id => id == item.Id) && item.DeleteDate == null);

                        //1. Extract the replacement base vehicle id from the first deserialized vehicleChangeRequestItems
                        var vehicleToBrakeConfig = Serializer.Deserialize<VehicleToBrakeConfig>(vehicleToBrakeConfigChangeRequestStoreItems[0].Payload);

                        //2. fill result.EntityStaging with the replacement base vehicle details
                        result.EntityStaging = await FindAsync(vehicleToBrakeConfig.BrakeConfigId);

                        // 3. fill currentEntity
                        result.EntityCurrent = await FindAsync(result.StagingItem.EntityId);
                    }
                }
            }

            BrakeConfigChangeRequestStagingModel staging = new BrakeConfigChangeRequestStagingModel
            {
                EntityCurrent = result.EntityCurrent,
                EntityStaging = result.EntityStaging,
                //RequestorComments = result.RequestorComments,
                //ReviewerComments = result.ReviewerComments,
                Comments = result.Comments,
                StagingItem = result.StagingItem,
                ReplacementVehicleToBrakeConfigs = vehicleToBrakeConfigs,
                Attachments = result.Attachments
            };
            return staging;
        }

        private async Task ValidateConfigurationDoesNotMatchWithExistingBrakeConfig(BrakeConfig brakeConfig)
        {
            // 1. Validate that there is no brake config with same configuration
            IList<BrakeConfig> brakeConfigFromDb =
                await
                    _brakeConfigRepositoryService.GetAsync(
                        x => x.FrontBrakeTypeId.Equals(brakeConfig.FrontBrakeTypeId)
                             && x.RearBrakeTypeId.Equals(brakeConfig.RearBrakeTypeId)
                             && x.BrakeSystemId.Equals(brakeConfig.BrakeSystemId)
                             && x.BrakeABSId.Equals(brakeConfig.BrakeABSId)
                             && x.DeleteDate == null);

            if (brakeConfigFromDb != null && brakeConfigFromDb.Count > 0)
            {
                throw new RecordAlreadyExist("Brake Config with same configuration already exists.");
            }
        }

        private async Task ValidateNoChangeRequestExistsWithSameConfiguration(BrakeConfig brakeConfig)
        {
            // 2. Validate that there is no open CR with same configuration
            var ChangeRequestID = await ChangeRequestBusinessService.ChangeRequestExist<BrakeConfig>(x =>
                x.FrontBrakeTypeId.Equals(brakeConfig.FrontBrakeTypeId)
                && x.RearBrakeTypeId.Equals(brakeConfig.RearBrakeTypeId)
                && x.BrakeSystemId.Equals(brakeConfig.BrakeSystemId)
                && x.BrakeABSId.Equals(brakeConfig.BrakeABSId));
            if (ChangeRequestID > 0)
            {
                throw new ChangeRequestExistException("There is already an open CR ID " + ChangeRequestID + " with same brake configuration.");
            }
        }

        private async Task ValidateBrakeConfigLookUpHasNoChangeRequest(BrakeConfig brakeConfig)
        {
            // 3. Validate that there are no open CR for any of the items that make of configuration. Front Brake, Rear Brake, Brake System, Brake ABS
            {
                // Front Brake
                var changerequestid =
                    await
                        ChangeRequestBusinessService.ChangeRequestExist(typeof(BrakeType).Name,
                            brakeConfig.FrontBrakeTypeId);
                if (changerequestid > 0)
                {
                    throw new ChangeRequestExistException(
                        $"There is already an open CR ID {changerequestid} for Front Brake ID : {brakeConfig.FrontBrakeTypeId}.");
                }

                // Rear Brake
                var changerequestID = await ChangeRequestBusinessService.ChangeRequestExist(typeof(BrakeType).Name, brakeConfig.RearBrakeTypeId);
                ;
                if (changerequestID > 0)
                {
                    throw new ChangeRequestExistException(
                        $"There is already an open CR ID {changerequestID} for Rear Brake ID : {brakeConfig.RearBrakeTypeId}.");
                }

                // Brake System
                var Changerequestid =
                    await
                        ChangeRequestBusinessService.ChangeRequestExist(typeof(BrakeSystem).Name,
                            brakeConfig.BrakeSystemId);

                if (Changerequestid > 0)
                {
                    throw new ChangeRequestExistException(
                        $"There is already an open CR ID {Changerequestid} for Brake System ID : {brakeConfig.BrakeSystemId}.");
                }

                // Brake ABS
                var CRID =
                    await
                        ChangeRequestBusinessService.ChangeRequestExist(typeof(BrakeABS).Name, brakeConfig.BrakeABSId);
                if (CRID > 0)
                {
                    throw new ChangeRequestExistException(
                        $"There is already an open CR ID {CRID} for Brake ABS ID : {brakeConfig.BrakeABSId}.");
                }
            }
        }

        private async Task ValidateBrakeConfigDependentHasNoChangeRequest(BrakeConfig brakeConfig)
        {
            var changerequestid = await ChangeRequestBusinessService.ChangeRequestExist<VehicleToBrakeConfig>(x =>
                x.BrakeConfigId.Equals(brakeConfig.Id));
            if (changerequestid > 0)
            {
                throw new ChangeRequestExistException("There is already an open CR ID " + changerequestid + " for Vehicle to Brake Configuration using this brake Config.");
            }
        }

        public override AssociationCount AssociatedCount(string payload)
        {
            AssociationCount count = new AssociationCount();
            if (payload != null)
            {
                BrakeConfig deserializedEntry = Serializer.Deserialize<BrakeConfig>(payload);
                count.VehicleToBrakeConfigCount = deserializedEntry.VehicleToBrakeConfigCount;
            }
            return count;
        }

        public override async Task ClearChangeRequestId(long changeRequestId)
        {
            var entities = await _brakeConfigRepositoryService.GetAsync(x => x.ChangeRequestId == changeRequestId && x.DeleteDate == null);

            if (entities != null && entities.Any())
            {
                var entity = entities.First();   //can only be a single item
                entity.ChangeRequestId = null;
                _brakeConfigRepositoryService.Update(entity);
                Repositories.SaveChanges();
            }

            var vehicleToBrakeConfigRepositoryService = base.Repositories.GetRepositoryService<VehicleToBrakeConfig>();
            List<VehicleToBrakeConfig> approvedReplacementVehicleToBrakeConfigs = await vehicleToBrakeConfigRepositoryService.GetAsync(m => m.ChangeRequestId == changeRequestId);

            if (approvedReplacementVehicleToBrakeConfigs != null && approvedReplacementVehicleToBrakeConfigs.Any())
            {
                foreach (var approvedReplacementVehicleToBrakeConfig in approvedReplacementVehicleToBrakeConfigs)
                {
                    approvedReplacementVehicleToBrakeConfig.ChangeRequestId = null;
                    vehicleToBrakeConfigRepositoryService.Update(approvedReplacementVehicleToBrakeConfig);
                }
                Repositories.SaveChanges();
            }
        }

        public override async Task<List<BrakeConfig>> GetPendingAddChangeRequests(Expression<Func<BrakeConfig, bool>> whereCondition = null, int topCount = 0)
        {
            var existingChangeRequestItemStagings = await ChangeRequestBusinessService.GetChangeRequestItemStagingsAsync(
                x => x.Entity.Equals(typeof(BrakeConfig).Name, StringComparison.CurrentCultureIgnoreCase)
                && x.ChangeType == ChangeType.Add);

            List<BrakeConfig> pendingBrakeConfigs = new List<BrakeConfig>();

            foreach (var existingChangeRequestItemStaging in existingChangeRequestItemStagings)
            {
                var pendingBrakeConfig = Serializer.Deserialize<BrakeConfig>(existingChangeRequestItemStaging.Payload);
                pendingBrakeConfig.ChangeRequestId = existingChangeRequestItemStaging.ChangeRequestId;
                pendingBrakeConfigs.Add(pendingBrakeConfig);
            }

            if (whereCondition != null)
            {
                var predicate = whereCondition.Compile();

                pendingBrakeConfigs = pendingBrakeConfigs.Where(predicate).ToList();
            }

            return pendingBrakeConfigs;
        }

        /// <summary>
        /// Find a single record which is not marked as deleted.
        /// </summary>
        /// <typeparam name="TKey">any</typeparam>
        /// <param name="id">the key property</param>
        /// <returns>Single entity object if condition matches, null if not found.</returns>
        private async Task<BrakeConfig> FindAsync<TKey>(TKey id)
        {
            var entityId = Convert.ToInt32(id);
            var entities = await _brakeConfigRepositoryService.GetAsync(m => m.Id == entityId && m.DeleteDate == null, 1);
            if (entities != null && entities.Any())
            {
                return entities.First();
            }
            return null;
        }
    }
}
