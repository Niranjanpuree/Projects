
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
    public class EngineDesignationBusinessService : VcdbBusinessService<EngineDesignation>, IEngineDesignationBusinessService
    {
        private readonly ISqlServerEfRepositoryService<EngineDesignation> _engineDesignationRepositoryService = null;

        public EngineDesignationBusinessService(IVcdbUnitOfWork vcdbRepositoryService,
            IVcdbChangeRequestBusinessService vcdbChangeRequestBusinessService,
            ITextSerializer serializer)
            : base(vcdbRepositoryService, vcdbChangeRequestBusinessService, serializer)
        {
            _engineDesignationRepositoryService = Repositories.GetRepositoryService<EngineDesignation>() as ISqlServerEfRepositoryService<EngineDesignation>;
        }

        public override async Task<List<EngineDesignation>> GetAllAsync(int topCount = 0)
        {
            return await _engineDesignationRepositoryService.GetAsync(x => x.DeleteDate == null, topCount);
        }

        public override async Task<EngineDesignation> GetAsync<TKey>(TKey id)
        {
            if (id.Equals(default(TKey)))
            {
                throw new ArgumentNullException(nameof(id));
            }

            var engineDesignation = await FindAsync(id);

            if (engineDesignation == null)
            {
                throw new NoRecordFound("Record Not Found");
            }

            engineDesignation.EngineConfigCount = await _engineDesignationRepositoryService.GetCountAsync(engineDesignation, x => x.EngineConfigs,
                y => y.DeleteDate == null);

            var engineDesignationId = Convert.ToInt32(id);
            engineDesignation.VehicleToEngineConfigCount = _engineDesignationRepositoryService
                .GetAllQueryable()
                .Where(x => x.Id == engineDesignationId)
                .Include("EngineConfigs.VehicleToEngineConfigs")
                .SelectMany(x => x.EngineConfigs)
                .SelectMany(x => x.VehicleToEngineConfigs).Count(y => y.DeleteDate == null);

            return engineDesignation;
        }

        public override async Task<long> SubmitAddChangeRequestAsync(EngineDesignation entity, string requestedBy,
            List<ChangeRequestItemStaging> changeRequestItemStagings = null, CommentsStagingModel comment = null,
            List<AttachmentsModel> attachmentsStagingModels = null, string changeContent = null)
        {
            if (String.IsNullOrWhiteSpace(entity?.EngineDesignationName))
            {
                throw new ArgumentNullException(nameof(entity));
            }

            changeRequestItemStagings = changeRequestItemStagings ?? new List<ChangeRequestItemStaging>();

            await ValidateEngineDesignationHasNoChangeRequest(entity);

            changeRequestItemStagings.Add(new ChangeRequestItemStaging()
            {
                ChangeType = ChangeType.Add,
                EntityId = entity.Id.ToString(),
                Entity = typeof(EngineDesignation).Name,
                CreatedDateTime = DateTime.UtcNow,
                Payload = Serializer.Serialize(entity)
            });

            changeContent = string.Format("{0}"
                , entity.EngineDesignationName);

            // NOTE: change-request-comments-staging perfomed on base

            return await base.SubmitAddChangeRequestAsync(entity, requestedBy, changeRequestItemStagings, comment, attachmentsStagingModels, changeContent);
        }

        public override async Task<long> SubmitUpdateChangeRequestAsync<TId>(EngineDesignation entity, TId id, string requestedBy,
            ChangeType changeType = ChangeType.None,
            List<ChangeRequestItemStaging> changeRequestItemStagings = null, CommentsStagingModel comment = null, List<AttachmentsModel> attachments = null, string changeContent = null)
        {
            if (String.IsNullOrWhiteSpace(entity?.EngineDesignationName))
            {
                throw new ArgumentNullException(nameof(entity));
            }

            if (id.Equals(default(TId)))
            {
                throw new ArgumentNullException(nameof(id));
            }

            if (!entity.Id.Equals(id))
            {
                throw new ArgumentException("Invalid Engine Designation Id");
            }

            var engineDesignationFromDb = await FindAsync(id);
            if (engineDesignationFromDb == null)
            {
                throw new NoRecordFound("No Engine Designation exist");
            }

            changeRequestItemStagings = changeRequestItemStagings ?? new List<ChangeRequestItemStaging>();

            await ValidateEngineDesignationHasNoChangeRequest(entity);
            await ValidateEngineDesignationLookUpHasNoChangeRequest(entity);

            entity.InsertDate = engineDesignationFromDb.InsertDate;

            // to eliminate circular reference during serialize
            var existingEntity = new EngineDesignation()
            {
                Id = engineDesignationFromDb.Id,
                EngineDesignationName = engineDesignationFromDb.EngineDesignationName,
                ChangeRequestId = engineDesignationFromDb.ChangeRequestId,
                EngineConfigCount = engineDesignationFromDb.EngineConfigCount,
                //v = engineDesignationFromDb.v,
                DeleteDate = engineDesignationFromDb.DeleteDate,
                InsertDate = engineDesignationFromDb.InsertDate,
                LastUpdateDate = engineDesignationFromDb.LastUpdateDate
            };

            changeRequestItemStagings.Add(new ChangeRequestItemStaging()
            {
                ChangeType = ChangeType.Modify,
                EntityId = entity.Id.ToString(),
                CreatedDateTime = DateTime.UtcNow,
                Entity = typeof(EngineDesignation).Name,
                Payload = Serializer.Serialize(entity),
                ExistingPayload = base.Serializer.Serialize(existingEntity) // add existing record as ExistingPayload
            });

            changeContent = string.Format("{0} > {1}"
                , engineDesignationFromDb.EngineDesignationName
                , entity.EngineDesignationName);

            var changeRequestId = await base.SubmitUpdateChangeRequestAsync(entity, id, requestedBy, ChangeType.Modify, changeRequestItemStagings, comment, attachments, changeContent);

            engineDesignationFromDb.ChangeRequestId = changeRequestId;
            _engineDesignationRepositoryService.Update(engineDesignationFromDb);
            Repositories.SaveChanges();

            return changeRequestId;
        }

        public override async Task<long> SubmitDeleteChangeRequestAsync<TId>(EngineDesignation entity, TId id, string requestedBy, List<ChangeRequestItemStaging> changeRequestItemStagings = null, CommentsStagingModel comment = null, List<AttachmentsModel> attachmentsStaging = null, string changeContent = null)
        {
            // validation
            if (id.Equals(default(TId)))
            {
                throw new ArgumentNullException(nameof(id));
            }
            var engineDesignationFromDb = await FindAsync(id);
            if (engineDesignationFromDb == null)
            {
                throw new NoRecordFound("No  Engine Designation exist");
            }

            changeRequestItemStagings = changeRequestItemStagings ?? new List<ChangeRequestItemStaging>();

            await ValidateEngineDesignationLookUpHasNoChangeRequest(engineDesignationFromDb);

            changeRequestItemStagings.Add(new ChangeRequestItemStaging()
            {
                EntityId = engineDesignationFromDb.Id.ToString(),
                Entity = typeof(EngineDesignation).Name,
                ChangeType = ChangeType.Delete,
                CreatedDateTime = DateTime.UtcNow,
                Payload = Serializer.Serialize(new EngineDesignation()
                {
                    Id = engineDesignationFromDb.Id,
                    EngineDesignationName = engineDesignationFromDb.EngineDesignationName
                })
            });

            changeContent = string.Format("{0}"
                , engineDesignationFromDb.EngineDesignationName);

            var changeRequestId = await base.SubmitDeleteChangeRequestAsync(engineDesignationFromDb, id, requestedBy, changeRequestItemStagings, comment, attachmentsStaging, changeContent);

            engineDesignationFromDb.ChangeRequestId = changeRequestId;
            _engineDesignationRepositoryService.Update(engineDesignationFromDb);
            Repositories.SaveChanges();

            return changeRequestId;
        }

        public override async Task<ChangeRequestStagingModel<EngineDesignation>> GetChangeRequestStaging<TId>(TId changeRequestId)
        {
            ChangeRequestStagingModel<EngineDesignation> staging =
                await ChangeRequestBusinessService.GetChangeRequestStagingByChangeRequestIdAsync<EngineDesignation, TId>(changeRequestId);
            //if (!staging.StagingItem.ChangeType.Equals(ChangeType.Add.ToString(), StringComparison.CurrentCultureIgnoreCase))
            //{
            //    BrakeType currentBrakeType = await FindAsync(staging.StagingItem.EntityId);
            //    staging.EntityCurrent = currentBrakeType;
            //}

            return staging;
        }

        private async Task ValidateEngineDesignationHasNoChangeRequest(EngineDesignation engineDesignation)
        {
            IList<EngineDesignation> engineDesignationsFromDb = await _engineDesignationRepositoryService.GetAsync(x => x.EngineDesignationName.ToLower().Equals(engineDesignation.EngineDesignationName.ToLower()) && x.DeleteDate == null);

            if (engineDesignationsFromDb != null && engineDesignationsFromDb.Any())
            {
                throw new RecordAlreadyExist(" Engine Designation already exists");
            }

            var changerequestid =
                await
                    ChangeRequestBusinessService.ChangeRequestExist<EngineDesignation>(
                        x => x.EngineDesignationName.ToLower().Equals(engineDesignation.EngineDesignationName.ToLower()));
            if (changerequestid > 0)
            {
                throw new ChangeRequestExistException("There is already an open CR ID " + changerequestid + "  with same  Engine Designation Name.");
            }
        }

        private async Task ValidateEngineDesignationLookUpHasNoChangeRequest(EngineDesignation engineDesignation)
        {

            var changerequestid =
                await
                    ChangeRequestBusinessService.ChangeRequestExist<EngineConfig>(
                        x => x.EngineDesignationId.Equals(engineDesignation.Id));
            if (changerequestid > 0)
            {
                throw new ChangeRequestExistException($"There is already an open CR ID " + changerequestid + " for Engine Config using this Engine Designation.");
            }
        }

        //public override AssociationCount AssociatedCount(string payload)
        //{
        //    AssociationCount count = new AssociationCount();
        //    if (payload != null)
        //    {
        //        EngineDesignation deserializedEntry = Serializer.Deserialize<EngineDesignation>(payload);
        //        count.VehicleToBedConfigCount = deserializedEntry.v;
        //    }
        //    return count;
        //}

        public override async Task ClearChangeRequestId(long changeRequestId)
        {
            var entities = await _engineDesignationRepositoryService.GetAsync(m => m.ChangeRequestId == changeRequestId);

            if (entities != null && entities.Any())
            {
                var entity = entities.First();   //can only be a single item
                entity.ChangeRequestId = null;
                _engineDesignationRepositoryService.Update(entity);
                Repositories.SaveChanges();
            }
        }

        /// <summary>
        /// Find a single record which is not marked as deleted.
        /// </summary>
        /// <typeparam name="TKey">any</typeparam>
        /// <param name="id">the key property</param>
        /// <returns>Single entity object if condition matches, null if not found.</returns>
        private async Task<EngineDesignation> FindAsync<TKey>(TKey id)
        {
            var entityId = Convert.ToInt32(id);
            var entities = await _engineDesignationRepositoryService.GetAsync(m => m.Id == entityId && m.DeleteDate == null, 1);
            if (entities != null && entities.Any())
            {
                return entities.First();
            }
            return null;
        }
    }
}
