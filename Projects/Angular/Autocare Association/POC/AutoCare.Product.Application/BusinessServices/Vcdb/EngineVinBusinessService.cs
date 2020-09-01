
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
    public class EngineVinBusinessService : VcdbBusinessService<EngineVin>, IEngineVinBusinessService
    {
        private readonly ISqlServerEfRepositoryService<EngineVin> _engineVinRepositoryService = null;

        public EngineVinBusinessService(IVcdbUnitOfWork vcdbRepositoryService,
            IVcdbChangeRequestBusinessService vcdbChangeRequestBusinessService,
            ITextSerializer serializer)
            : base(vcdbRepositoryService, vcdbChangeRequestBusinessService, serializer)
        {
            _engineVinRepositoryService = Repositories.GetRepositoryService<EngineVin>() as ISqlServerEfRepositoryService<EngineVin>;
        }

        public override async Task<List<EngineVin>> GetAllAsync(int topCount = 0)
        {
            return await _engineVinRepositoryService.GetAsync(x => x.DeleteDate == null, topCount);
        }

        public override async Task<EngineVin> GetAsync<TKey>(TKey id)
        {
            if (id.Equals(default(TKey)))
            {
                throw new ArgumentNullException(nameof(id));
            }

            var engineVin = await FindAsync(id);

            if (engineVin == null)
            {
                throw new NoRecordFound("Record Not Found");
            }

            engineVin.EngineConfigCount = await _engineVinRepositoryService.GetCountAsync(engineVin, x => x.EngineConfigs,
                y => y.DeleteDate == null);

            var engineVinId = Convert.ToInt32(id);
            engineVin.VehicleToEngineConfigCount = _engineVinRepositoryService
                .GetAllQueryable()
                .Where(x => x.Id == engineVinId)
                .Include("EngineConfigs.VehicleToEngineConfigs")
                .SelectMany(x => x.EngineConfigs)
                .SelectMany(x => x.VehicleToEngineConfigs).Count(y => y.DeleteDate == null);

            return engineVin;
        }

        public override async Task<long> SubmitAddChangeRequestAsync(EngineVin entity, string requestedBy,
            List<ChangeRequestItemStaging> changeRequestItemStagings = null, CommentsStagingModel comment = null,
            List<AttachmentsModel> attachmentsStagingModels = null, string changeContent = null)
        {
            if (String.IsNullOrWhiteSpace(entity?.EngineVinName))
            {
                throw new ArgumentNullException(nameof(entity));
            }

            changeRequestItemStagings = changeRequestItemStagings ?? new List<ChangeRequestItemStaging>();

            await ValidateEngineVinHasNoChangeRequest(entity);

            changeRequestItemStagings.Add(new ChangeRequestItemStaging()
            {
                ChangeType = ChangeType.Add,
                EntityId = entity.Id.ToString(),
                Entity = typeof(EngineVin).Name,
                CreatedDateTime = DateTime.UtcNow,
                Payload = Serializer.Serialize(entity)
            });

            changeContent = string.Format("{0}"
                , entity.EngineVinName);

            // NOTE: change-request-comments-staging perfomed on base

            return await base.SubmitAddChangeRequestAsync(entity, requestedBy, changeRequestItemStagings, comment, attachmentsStagingModels, changeContent);
        }

        public override async Task<long> SubmitUpdateChangeRequestAsync<TId>(EngineVin entity, TId id, string requestedBy,
            ChangeType changeType = ChangeType.None,
            List<ChangeRequestItemStaging> changeRequestItemStagings = null, CommentsStagingModel comment = null, List<AttachmentsModel> attachments = null, string changeContent = null)
        {
            if (String.IsNullOrWhiteSpace(entity?.EngineVinName))
            {
                throw new ArgumentNullException(nameof(entity));
            }

            if (id.Equals(default(TId)))
            {
                throw new ArgumentNullException(nameof(id));
            }

            if (!entity.Id.Equals(id))
            {
                throw new ArgumentException("Invalid Engine Vin Id");
            }

            var engineVinFromDb = await FindAsync(id);
            if (engineVinFromDb == null)
            {
                throw new NoRecordFound("No Engine Vin exist");
            }

            changeRequestItemStagings = changeRequestItemStagings ?? new List<ChangeRequestItemStaging>();

            await ValidateEngineVinHasNoChangeRequest(entity);
            await ValidateEngineVinLookUpHasNoChangeRequest(entity);

            entity.InsertDate = engineVinFromDb.InsertDate;

            // to eliminate circular reference during serialize
            var existingEntity = new EngineVin()
            {
                Id = engineVinFromDb.Id,
                EngineVinName = engineVinFromDb.EngineVinName,
                ChangeRequestId = engineVinFromDb.ChangeRequestId,
                EngineConfigCount = engineVinFromDb.EngineConfigCount,
                //v = engineVinFromDb.v,
                DeleteDate = engineVinFromDb.DeleteDate,
                InsertDate = engineVinFromDb.InsertDate,
                LastUpdateDate = engineVinFromDb.LastUpdateDate
            };

            changeRequestItemStagings.Add(new ChangeRequestItemStaging()
            {
                ChangeType = ChangeType.Modify,
                EntityId = entity.Id.ToString(),
                CreatedDateTime = DateTime.UtcNow,
                Entity = typeof(EngineVin).Name,
                Payload = Serializer.Serialize(entity),
                ExistingPayload = base.Serializer.Serialize(existingEntity) // add existing record as ExistingPayload
            });

            changeContent = string.Format("{0} > {1}"
                , engineVinFromDb.EngineVinName
                , entity.EngineVinName);

            var changeRequestId = await base.SubmitUpdateChangeRequestAsync(entity, id, requestedBy, ChangeType.Modify, changeRequestItemStagings, comment, attachments, changeContent);

            engineVinFromDb.ChangeRequestId = changeRequestId;
            _engineVinRepositoryService.Update(engineVinFromDb);
            Repositories.SaveChanges();

            return changeRequestId;
        }

        public override async Task<long> SubmitDeleteChangeRequestAsync<TId>(EngineVin entity, TId id, string requestedBy, List<ChangeRequestItemStaging> changeRequestItemStagings = null, CommentsStagingModel comment = null, List<AttachmentsModel> attachmentsStaging = null, string changeContent = null)
        {
            // validation
            if (id.Equals(default(TId)))
            {
                throw new ArgumentNullException(nameof(id));
            }
            var engineVinFromDb = await FindAsync(id);
            if (engineVinFromDb == null)
            {
                throw new NoRecordFound("No  Engine Vin exist");
            }

            changeRequestItemStagings = changeRequestItemStagings ?? new List<ChangeRequestItemStaging>();

            await ValidateEngineVinLookUpHasNoChangeRequest(engineVinFromDb);

            changeRequestItemStagings.Add(new ChangeRequestItemStaging()
            {
                EntityId = engineVinFromDb.Id.ToString(),
                Entity = typeof(EngineVin).Name,
                ChangeType = ChangeType.Delete,
                CreatedDateTime = DateTime.UtcNow,
                Payload = Serializer.Serialize(new EngineVin()
                {
                    Id = engineVinFromDb.Id,
                    EngineVinName = engineVinFromDb.EngineVinName
                })
            });

            changeContent = string.Format("{0}"
                , engineVinFromDb.EngineVinName);

            var changeRequestId = await base.SubmitDeleteChangeRequestAsync(engineVinFromDb, id, requestedBy, changeRequestItemStagings, comment, attachmentsStaging, changeContent);

            engineVinFromDb.ChangeRequestId = changeRequestId;
            _engineVinRepositoryService.Update(engineVinFromDb);
            Repositories.SaveChanges();

            return changeRequestId;
        }

        public override async Task<ChangeRequestStagingModel<EngineVin>> GetChangeRequestStaging<TId>(TId changeRequestId)
        {
            ChangeRequestStagingModel<EngineVin> staging =
                await ChangeRequestBusinessService.GetChangeRequestStagingByChangeRequestIdAsync<EngineVin, TId>(changeRequestId);
            //if (!staging.StagingItem.ChangeType.Equals(ChangeType.Add.ToString(), StringComparison.CurrentCultureIgnoreCase))
            //{
            //    BrakeType currentBrakeType = await FindAsync(staging.StagingItem.EntityId);
            //    staging.EntityCurrent = currentBrakeType;
            //}

            return staging;
        }

        private async Task ValidateEngineVinHasNoChangeRequest(EngineVin engineVin)
        {
            IList<EngineVin> engineVinsFromDb = await _engineVinRepositoryService.GetAsync(x => x.EngineVinName.ToLower().Equals(engineVin.EngineVinName.ToLower()) && x.DeleteDate == null);

            if (engineVinsFromDb != null && engineVinsFromDb.Any())
            {
                throw new RecordAlreadyExist(" Engine Vin already exists");
            }

            var changerequestid =
                await
                    ChangeRequestBusinessService.ChangeRequestExist<EngineVin>(
                        x => x.EngineVinName.ToLower().Equals(engineVin.EngineVinName.ToLower()));
            if (changerequestid > 0)
            {
                throw new ChangeRequestExistException("There is already an open CR ID " + changerequestid + "  with same  Engine Vin Name.");
            }
        }

        private async Task ValidateEngineVinLookUpHasNoChangeRequest(EngineVin engineVin)
        {

            var changerequestid =
                await
                    ChangeRequestBusinessService.ChangeRequestExist<EngineConfig>(
                        x => x.EngineVinId.Equals(engineVin.Id));
            if (changerequestid > 0)
            {
                throw new ChangeRequestExistException($"There is already an open CR ID " + changerequestid + " for Engine Config using this Engine Vin.");
            }
        }

        //public override AssociationCount AssociatedCount(string payload)
        //{
        //    AssociationCount count = new AssociationCount();
        //    if (payload != null)
        //    {
        //        EngineVin deserializedEntry = Serializer.Deserialize<EngineVin>(payload);
        //        count.VehicleToBedConfigCount = deserializedEntry.v;
        //    }
        //    return count;
        //}

        public override async Task ClearChangeRequestId(long changeRequestId)
        {
            var entities = await _engineVinRepositoryService.GetAsync(m => m.ChangeRequestId == changeRequestId);

            if (entities != null && entities.Any())
            {
                var entity = entities.First();   //can only be a single item
                entity.ChangeRequestId = null;
                _engineVinRepositoryService.Update(entity);
                Repositories.SaveChanges();
            }
        }

        /// <summary>
        /// Find a single record which is not marked as deleted.
        /// </summary>
        /// <typeparam name="TKey">any</typeparam>
        /// <param name="id">the key property</param>
        /// <returns>Single entity object if condition matches, null if not found.</returns>
        private async Task<EngineVin> FindAsync<TKey>(TKey id)
        {
            var entityId = Convert.ToInt32(id);
            var entities = await _engineVinRepositoryService.GetAsync(m => m.Id == entityId && m.DeleteDate == null, 1);
            if (entities != null && entities.Any())
            {
                return entities.First();
            }
            return null;
        }
    }
}
