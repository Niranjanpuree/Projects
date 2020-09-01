
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AutoCare.Product.Application.RepositoryServices;
using AutoCare.Product.Infrastructure.ExceptionHandler;
using AutoCare.Product.Infrastructure.RepositoryService;
using AutoCare.Product.Infrastructure.Serializer;
using AutoCare.Product.Vcdb.Model;
using System.Collections.Generic;
using AutoCare.Product.Application.Models.BusinessModels;
using AutoCare.Product.Application.Infrastructure.ExceptionHelpers;

namespace AutoCare.Product.Application.BusinessServices.Vcdb
{
    public class EngineVersionBusinessService : VcdbBusinessService<EngineVersion>, IEngineVersionBusinessService
    {
        private readonly ISqlServerEfRepositoryService<EngineVersion> _engineVersionRepositoryService = null;

        public EngineVersionBusinessService(IVcdbUnitOfWork vcdbRepositoryService,
            IVcdbChangeRequestBusinessService vcdbChangeRequestBusinessService,
            ITextSerializer serializer)
            : base(vcdbRepositoryService, vcdbChangeRequestBusinessService, serializer)
        {
            _engineVersionRepositoryService = Repositories.GetRepositoryService<EngineVersion>() as ISqlServerEfRepositoryService<EngineVersion>;
        }

        public override async Task<List<EngineVersion>> GetAllAsync(int topCount = 0)
        {
            return await _engineVersionRepositoryService.GetAsync(x => x.DeleteDate == null, topCount);
        }

        public override async Task<EngineVersion> GetAsync<TKey>(TKey id)
        {
            if (id.Equals(default(TKey)))
            {
                throw new ArgumentNullException(nameof(id));
            }

            var engineVersion = await FindAsync(id);

            if (engineVersion == null)
            {
                throw new NoRecordFound("Record Not Found");
            }

            engineVersion.EngineConfigCount = await _engineVersionRepositoryService.GetCountAsync(engineVersion, x => x.EngineConfigs,
                y => y.DeleteDate == null);

            var engineVersionId = Convert.ToInt32(id);
            engineVersion.VehicleToEngineConfigCount = _engineVersionRepositoryService
                .GetAllQueryable()
                .Where(x => x.Id == engineVersionId)
                .Include("EngineConfigs.VehicleToEngineConfigs")
                .SelectMany(x => x.EngineConfigs)
                .SelectMany(x => x.VehicleToEngineConfigs).Count(y => y.DeleteDate == null);

            return engineVersion;
        }

        public override async Task<long> SubmitAddChangeRequestAsync(EngineVersion entity, string requestedBy,
            List<ChangeRequestItemStaging> changeRequestItemStagings = null, CommentsStagingModel comment = null,
            List<AttachmentsModel> attachmentsStagingModels = null, string changeContent = null)
        {
            if (String.IsNullOrWhiteSpace(entity?.EngineVersionName))
            {
                throw new ArgumentNullException(nameof(entity));
            }

            changeRequestItemStagings = changeRequestItemStagings ?? new List<ChangeRequestItemStaging>();

            await ValidateEngineVersionHasNoChangeRequest(entity);

            changeRequestItemStagings.Add(new ChangeRequestItemStaging()
            {
                ChangeType = ChangeType.Add,
                EntityId = entity.Id.ToString(),
                Entity = typeof(EngineVersion).Name,
                CreatedDateTime = DateTime.UtcNow,
                Payload = Serializer.Serialize(entity)
            });

            changeContent = string.Format("{0}"
                , entity.EngineVersionName);

            // NOTE: change-request-comments-staging perfomed on base

            return await base.SubmitAddChangeRequestAsync(entity, requestedBy, changeRequestItemStagings, comment, attachmentsStagingModels, changeContent);
        }

        public override async Task<long> SubmitUpdateChangeRequestAsync<TId>(EngineVersion entity, TId id, string requestedBy,
            ChangeType changeType = ChangeType.None,
            List<ChangeRequestItemStaging> changeRequestItemStagings = null, CommentsStagingModel comment = null, List<AttachmentsModel> attachments = null, string changeContent = null)
        {
            if (String.IsNullOrWhiteSpace(entity?.EngineVersionName))
            {
                throw new ArgumentNullException(nameof(entity));
            }

            if (id.Equals(default(TId)))
            {
                throw new ArgumentNullException(nameof(id));
            }

            if (!entity.Id.Equals(id))
            {
                throw new ArgumentException("Invalid Engine Version Id");
            }

            var engineVersionFromDb = await FindAsync(id);
            if (engineVersionFromDb == null)
            {
                throw new NoRecordFound("No Engine Version exist");
            }

            changeRequestItemStagings = changeRequestItemStagings ?? new List<ChangeRequestItemStaging>();

            await ValidateEngineVersionHasNoChangeRequest(entity);
            await ValidateEngineVersionLookUpHasNoChangeRequest(entity);

            entity.InsertDate = engineVersionFromDb.InsertDate;

            // to eliminate circular reference during serialize
            var existingEntity = new EngineVersion()
            {
                Id = engineVersionFromDb.Id,
                EngineVersionName = engineVersionFromDb.EngineVersionName,
                ChangeRequestId = engineVersionFromDb.ChangeRequestId,
                EngineConfigCount = engineVersionFromDb.EngineConfigCount,
                //v = engineVersionFromDb.v,
                DeleteDate = engineVersionFromDb.DeleteDate,
                InsertDate = engineVersionFromDb.InsertDate,
                LastUpdateDate = engineVersionFromDb.LastUpdateDate
            };

            changeRequestItemStagings.Add(new ChangeRequestItemStaging()
            {
                ChangeType = ChangeType.Modify,
                EntityId = entity.Id.ToString(),
                CreatedDateTime = DateTime.UtcNow,
                Entity = typeof(EngineVersion).Name,
                Payload = Serializer.Serialize(entity),
                ExistingPayload = base.Serializer.Serialize(existingEntity) // add existing record as ExistingPayload
            });

            changeContent = string.Format("{0} > {1}"
                , engineVersionFromDb.EngineVersionName
                , entity.EngineVersionName);

            var changeRequestId = await base.SubmitUpdateChangeRequestAsync(entity, id, requestedBy, ChangeType.Modify, changeRequestItemStagings, comment, attachments, changeContent);

            engineVersionFromDb.ChangeRequestId = changeRequestId;
            _engineVersionRepositoryService.Update(engineVersionFromDb);
            Repositories.SaveChanges();

            return changeRequestId;
        }

        public override async Task<long> SubmitDeleteChangeRequestAsync<TId>(EngineVersion entity, TId id, string requestedBy, List<ChangeRequestItemStaging> changeRequestItemStagings = null, CommentsStagingModel comment = null, List<AttachmentsModel> attachmentsStaging = null, string changeContent = null)
        {
            // validation
            if (id.Equals(default(TId)))
            {
                throw new ArgumentNullException(nameof(id));
            }
            var engineVersionFromDb = await FindAsync(id);
            if (engineVersionFromDb == null)
            {
                throw new NoRecordFound("No  Engine Version exist");
            }

            changeRequestItemStagings = changeRequestItemStagings ?? new List<ChangeRequestItemStaging>();

            await ValidateEngineVersionLookUpHasNoChangeRequest(engineVersionFromDb);

            changeRequestItemStagings.Add(new ChangeRequestItemStaging()
            {
                EntityId = engineVersionFromDb.Id.ToString(),
                Entity = typeof(EngineVersion).Name,
                ChangeType = ChangeType.Delete,
                CreatedDateTime = DateTime.UtcNow,
                Payload = Serializer.Serialize(new EngineVersion()
                {
                    Id = engineVersionFromDb.Id,
                    EngineVersionName = engineVersionFromDb.EngineVersionName
                })
            });

            changeContent = string.Format("{0}"
                , engineVersionFromDb.EngineVersionName);

            var changeRequestId = await base.SubmitDeleteChangeRequestAsync(engineVersionFromDb, id, requestedBy, changeRequestItemStagings, comment, attachmentsStaging, changeContent);

            engineVersionFromDb.ChangeRequestId = changeRequestId;
            _engineVersionRepositoryService.Update(engineVersionFromDb);
            Repositories.SaveChanges();

            return changeRequestId;
        }

        public override async Task<ChangeRequestStagingModel<EngineVersion>> GetChangeRequestStaging<TId>(TId changeRequestId)
        {
            ChangeRequestStagingModel<EngineVersion> staging =
                await ChangeRequestBusinessService.GetChangeRequestStagingByChangeRequestIdAsync<EngineVersion, TId>(changeRequestId);
            //if (!staging.StagingItem.ChangeType.Equals(ChangeType.Add.ToString(), StringComparison.CurrentCultureIgnoreCase))
            //{
            //    BrakeType currentBrakeType = await FindAsync(staging.StagingItem.EntityId);
            //    staging.EntityCurrent = currentBrakeType;
            //}

            return staging;
        }

        private async Task ValidateEngineVersionHasNoChangeRequest(EngineVersion engineVersion)
        {
            IList<EngineVersion> engineVersionsFromDb = await _engineVersionRepositoryService.GetAsync(x => x.EngineVersionName.ToLower().Equals(engineVersion.EngineVersionName.ToLower()) && x.DeleteDate == null);

            if (engineVersionsFromDb != null && engineVersionsFromDb.Any())
            {
                throw new RecordAlreadyExist(" Engine Version already exists");
            }

            var changerequestid =
                await
                    ChangeRequestBusinessService.ChangeRequestExist<EngineVersion>(
                        x => x.EngineVersionName.ToLower().Equals(engineVersion.EngineVersionName.ToLower()));
            if (changerequestid > 0)
            {
                throw new ChangeRequestExistException("There is already an open CR ID " + changerequestid + "  with same  Engine Version Name.");
            }
        }

        private async Task ValidateEngineVersionLookUpHasNoChangeRequest(EngineVersion engineVersion)
        {

            var changerequestid =
                await
                    ChangeRequestBusinessService.ChangeRequestExist<EngineConfig>(
                        x => x.EngineVersionId.Equals(engineVersion.Id));
            if (changerequestid > 0)
            {
                throw new ChangeRequestExistException($"There is already an open CR ID " + changerequestid + " for Engine Config using this Engine Version.");
            }
        }

        //public override AssociationCount AssociatedCount(string payload)
        //{
        //    AssociationCount count = new AssociationCount();
        //    if (payload != null)
        //    {
        //        EngineVersion deserializedEntry = Serializer.Deserialize<EngineVersion>(payload);
        //        count.VehicleToBedConfigCount = deserializedEntry.v;
        //    }
        //    return count;
        //}

        public override async Task ClearChangeRequestId(long changeRequestId)
        {
            var entities = await _engineVersionRepositoryService.GetAsync(m => m.ChangeRequestId == changeRequestId);

            if (entities != null && entities.Any())
            {
                var entity = entities.First();   //can only be a single item
                entity.ChangeRequestId = null;
                _engineVersionRepositoryService.Update(entity);
                Repositories.SaveChanges();
            }
        }

        /// <summary>
        /// Find a single record which is not marked as deleted.
        /// </summary>
        /// <typeparam name="TKey">any</typeparam>
        /// <param name="id">the key property</param>
        /// <returns>Single entity object if condition matches, null if not found.</returns>
        private async Task<EngineVersion> FindAsync<TKey>(TKey id)
        {
            var entityId = Convert.ToInt32(id);
            var entities = await _engineVersionRepositoryService.GetAsync(m => m.Id == entityId && m.DeleteDate == null, 1);
            if (entities != null && entities.Any())
            {
                return entities.First();
            }
            return null;
        }
    }
}
