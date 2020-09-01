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
    public class BodyTypeBusinessService : VcdbBusinessService<BodyType>, IBodyTypeBusinessService
    {
        private readonly ISqlServerEfRepositoryService<BodyType> _bodyTypeRepositoryService = null;

        public BodyTypeBusinessService(IVcdbUnitOfWork vcdbRepositoryService,
            IVcdbChangeRequestBusinessService vcdbChangeRequestBusinessService,
            ITextSerializer serializer)
            : base(vcdbRepositoryService, vcdbChangeRequestBusinessService, serializer)
        {
            _bodyTypeRepositoryService = Repositories.GetRepositoryService<BodyType>() as ISqlServerEfRepositoryService<BodyType>;
        }

        public override async Task<List<BodyType>> GetAllAsync(int topCount = 0)
        {
            return await _bodyTypeRepositoryService.GetAsync(x => x.DeleteDate == null, topCount);
        }

        public override async Task<BodyType> GetAsync<TKey>(TKey id)
        {
            if (id.Equals(default(TKey)))
            {
                throw new ArgumentNullException(nameof(id));
            }

            var bodyType = await FindAsync(id);

            if (bodyType == null)
            {
                throw new NoRecordFound("Record Not Found");
            }

            bodyType.BodyStyleConfigCount = await _bodyTypeRepositoryService.GetCountAsync(bodyType, x => x.BodyStyleConfigs,
                y => y.DeleteDate == null);

            var bodyTypeId = Convert.ToInt32(id);
            bodyType.VehicleToBodyStyleConfigCount = _bodyTypeRepositoryService
                .GetAllQueryable()
                .Where(x => x.Id == bodyTypeId)
                .Include("BodyStyleConfigs.VehicleToBodyStyleConfigs")
                .SelectMany(x => x.BodyStyleConfigs)
                .SelectMany(x => x.VehicleToBodyStyleConfigs).Count(y => y.DeleteDate == null);

            return bodyType;
        }

        public override async Task<long> SubmitAddChangeRequestAsync(BodyType entity, string requestedBy,
            List<ChangeRequestItemStaging> changeRequestItemStagings = null, CommentsStagingModel comment = null,
            List<AttachmentsModel> attachmentsStagingModels = null, string changeContent = null)
        {
            if (String.IsNullOrWhiteSpace(entity?.Name))
            {
                throw new ArgumentNullException(nameof(entity));
            }

            changeRequestItemStagings = changeRequestItemStagings ?? new List<ChangeRequestItemStaging>();

            await ValidateBodyTypeHasNoChangeRequest(entity);

            changeRequestItemStagings.Add(new ChangeRequestItemStaging()
            {
                ChangeType = ChangeType.Add,
                EntityId = entity.Id.ToString(),
                Entity = typeof(BodyType).Name,
                CreatedDateTime = DateTime.UtcNow,
                Payload = Serializer.Serialize(entity)
            });

            changeContent = string.Format("{0}"
                , entity.Name);

            // NOTE: change-request-comments-staging perfomed on base

            return await base.SubmitAddChangeRequestAsync(entity, requestedBy, changeRequestItemStagings, comment, attachmentsStagingModels, changeContent);
        }

        public override async Task<long> SubmitUpdateChangeRequestAsync<TId>(BodyType entity, TId id, string requestedBy,
            ChangeType changeType = ChangeType.None,
            List<ChangeRequestItemStaging> changeRequestItemStagings = null, CommentsStagingModel comment = null, List<AttachmentsModel> attachments = null, string changeContent = null)
        {
            if (String.IsNullOrWhiteSpace(entity?.Name))
            {
                throw new ArgumentNullException(nameof(entity));
            }

            if (id.Equals(default(TId)))
            {
                throw new ArgumentNullException(nameof(id));
            }

            if (!entity.Id.Equals(id))
            {
                throw new ArgumentException("Invalid Body Type Id");
            }

            var bodyTypeFromDb = await FindAsync(id);
            if (bodyTypeFromDb == null)
            {
                throw new NoRecordFound("No Body Type exist");
            }

            changeRequestItemStagings = changeRequestItemStagings ?? new List<ChangeRequestItemStaging>();

            await ValidateBodyTypeHasNoChangeRequest(entity);
            await ValidateBodyTypeLookUpHasNoChangeRequest(entity);

            entity.InsertDate = bodyTypeFromDb.InsertDate;

            // to eliminate circular reference during serialize
            var existingEntity = new BodyType()
            {
                Id = bodyTypeFromDb.Id,
                Name = bodyTypeFromDb.Name,
                ChangeRequestId = bodyTypeFromDb.ChangeRequestId,
                BodyStyleConfigCount = bodyTypeFromDb.BodyStyleConfigCount,
                VehicleToBodyStyleConfigCount = bodyTypeFromDb.VehicleToBodyStyleConfigCount,
                DeleteDate = bodyTypeFromDb.DeleteDate,
                InsertDate = bodyTypeFromDb.InsertDate,
                LastUpdateDate = bodyTypeFromDb.LastUpdateDate
            };

            changeRequestItemStagings.Add(new ChangeRequestItemStaging()
            {
                ChangeType = ChangeType.Modify,
                EntityId = entity.Id.ToString(),
                CreatedDateTime = DateTime.UtcNow,
                Entity = typeof(BodyType).Name,
                Payload = Serializer.Serialize(entity),
                ExistingPayload = base.Serializer.Serialize(existingEntity) // add existing record as ExistingPayload
            });

            changeContent = string.Format("{0} > {1}"
                , bodyTypeFromDb.Name
                , entity.Name);

            var changeRequestId = await base.SubmitUpdateChangeRequestAsync(entity, id, requestedBy, ChangeType.Modify, changeRequestItemStagings, comment, attachments, changeContent);

            bodyTypeFromDb.ChangeRequestId = changeRequestId;
            _bodyTypeRepositoryService.Update(bodyTypeFromDb);
            Repositories.SaveChanges();
            return changeRequestId;
        }

        public override async Task<long> SubmitDeleteChangeRequestAsync<TId>(BodyType entity, TId id, string requestedBy, List<ChangeRequestItemStaging> changeRequestItemStagings = null, CommentsStagingModel comment = null, List<AttachmentsModel> attachmentsStaging = null, string changeContent = null)
        {
            // validation
            if (id.Equals(default(TId)))
            {
                throw new ArgumentNullException(nameof(id));
            }
            var bodyTypeFromDb = await FindAsync(id);
            if (bodyTypeFromDb == null)
            {
                throw new NoRecordFound("No Body Type exist");
            }

            changeRequestItemStagings = changeRequestItemStagings ?? new List<ChangeRequestItemStaging>();

            await ValidateBodyTypeLookUpHasNoChangeRequest(bodyTypeFromDb);

            changeRequestItemStagings.Add(new ChangeRequestItemStaging()
            {
                EntityId = bodyTypeFromDb.Id.ToString(),
                Entity = typeof(BodyType).Name,
                ChangeType = ChangeType.Delete,
                CreatedDateTime = DateTime.UtcNow,
                Payload = Serializer.Serialize(new Make()
                {
                    Id = bodyTypeFromDb.Id,
                    Name = bodyTypeFromDb.Name
                })
            });

            changeContent = string.Format("{0}"
               , bodyTypeFromDb.Name);

            var changeRequestId = await base.SubmitDeleteChangeRequestAsync(bodyTypeFromDb, id, requestedBy, changeRequestItemStagings, comment, attachmentsStaging, changeContent);

            bodyTypeFromDb.ChangeRequestId = changeRequestId;
            _bodyTypeRepositoryService.Update(bodyTypeFromDb);
            Repositories.SaveChanges();

            return changeRequestId;
        }

        public override async Task<ChangeRequestStagingModel<BodyType>> GetChangeRequestStaging<TId>(TId changeRequestId)
        {
            ChangeRequestStagingModel<BodyType> staging =
                await ChangeRequestBusinessService.GetChangeRequestStagingByChangeRequestIdAsync<BodyType, TId>(changeRequestId);
            //if (!staging.StagingItem.ChangeType.Equals(ChangeType.Add.ToString(), StringComparison.CurrentCultureIgnoreCase))
            //{
            //    BrakeType currentBrakeType = await FindAsync(staging.StagingItem.EntityId);
            //    staging.EntityCurrent = currentBrakeType;
            //}

            return staging;
        }

        private async Task ValidateBodyTypeHasNoChangeRequest(BodyType bodyType)
        {
            IList<BodyType> bodyTypesFromDb = await _bodyTypeRepositoryService.GetAsync(x => x.Name.ToLower().Equals(bodyType.Name.ToLower()) && x.DeleteDate == null);

            if (bodyTypesFromDb != null && bodyTypesFromDb.Any())
            {
                throw new RecordAlreadyExist("Body Type already exists");
            }

            var changerequestid =
                await
                    ChangeRequestBusinessService.ChangeRequestExist<BodyType>(
                        x => x.Name.ToLower().Equals(bodyType.Name.ToLower()));
            if (changerequestid > 0)
            {
                throw new ChangeRequestExistException("There is already an open CR ID " + changerequestid + " with same Body Type Name.");
            }
        }

        private async Task ValidateBodyTypeLookUpHasNoChangeRequest(BodyType bodyType)
        {

            var changerequestid =
                await
                    ChangeRequestBusinessService.ChangeRequestExist<BodyStyleConfig>(
                        x => x.BodyTypeId.Equals(bodyType.Id));
            if (changerequestid > 0)
            {
                throw new ChangeRequestExistException($"There is already an open CR ID " + changerequestid + " for Body Style Config using this Body Type.");
            }
        }

        public override AssociationCount AssociatedCount(string payload)
        {
            AssociationCount count = new AssociationCount();
            if (payload != null)
            {
                BodyType deserializedEntry = Serializer.Deserialize<BodyType>(payload);
                count.VehicleToBodyStyleConfigCount = deserializedEntry.VehicleToBodyStyleConfigCount;
                count.BodyStyleConfigCount = deserializedEntry.BodyStyleConfigCount;
            }
            return count;
        }

        public override async Task ClearChangeRequestId(long changeRequestId)
        {
            var entities = await _bodyTypeRepositoryService.GetAsync(m => m.ChangeRequestId == changeRequestId);

            if (entities != null && entities.Any())
            {
                var entity = entities.First();   //can only be a single item
                entity.ChangeRequestId = null;
                _bodyTypeRepositoryService.Update(entity);
                Repositories.SaveChanges();
            }
        }

        /// <summary>
        /// Find a single record which is not marked as deleted.
        /// </summary>
        /// <typeparam name="TKey">any</typeparam>
        /// <param name="id">the key property</param>
        /// <returns>Single entity object if condition matches, null if not found.</returns>
        private async Task<BodyType> FindAsync<TKey>(TKey id)
        {
            var entityId = Convert.ToInt32(id);
            var entities = await _bodyTypeRepositoryService.GetAsync(m => m.Id == entityId && m.DeleteDate == null, 1);
            if (entities != null && entities.Any())
            {
                return entities.First();
            }
            return null;
        }
    }
}
