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
    public class BodyNumDoorsBusinessService  : VcdbBusinessService<BodyNumDoors>, IBodyNumDoorsBusinessService
    {
        private readonly ISqlServerEfRepositoryService<BodyNumDoors> _bodyNumDoorsRepositoryService = null;

        public BodyNumDoorsBusinessService(IVcdbUnitOfWork vcdbRepositoryService,
            IVcdbChangeRequestBusinessService vcdbChangeRequestBusinessService,
            ITextSerializer serializer)
            : base(vcdbRepositoryService, vcdbChangeRequestBusinessService, serializer)
        {
            _bodyNumDoorsRepositoryService = Repositories.GetRepositoryService<BodyNumDoors>() as ISqlServerEfRepositoryService<BodyNumDoors>;
        }

        public override async Task<List<BodyNumDoors>> GetAllAsync(int topCount = 0)
        {
            return await _bodyNumDoorsRepositoryService.GetAsync(x => x.DeleteDate == null, topCount);
        }

        public override async Task<BodyNumDoors> GetAsync<TKey>(TKey id)
        {
            if (id.Equals(default(TKey)))
            {
                throw new ArgumentNullException(nameof(id));
            }

            var bodyNumDoors = await FindAsync(id);

            if (bodyNumDoors == null)
            {
                throw new NoRecordFound("Record Not Found");
            }

            bodyNumDoors.BodyStyleConfigCount = await _bodyNumDoorsRepositoryService.GetCountAsync(bodyNumDoors, x => x.BodyStyleConfigs, y => y.DeleteDate == null);

            var brakeTypeId = Convert.ToInt32(id);
            bodyNumDoors.VehicleToBodyStyleConfigCount = _bodyNumDoorsRepositoryService
                .GetAllQueryable()
                .Where(x => x.Id == brakeTypeId)
                .Include("BodyStyleConfigs.VehicleToBodyConfigs")
                .SelectMany(x => x.BodyStyleConfigs)
                .SelectMany(x => x.VehicleToBodyStyleConfigs).Count(y => y.DeleteDate == null);

            return bodyNumDoors;
        }

        public override async Task<long> SubmitAddChangeRequestAsync(BodyNumDoors entity, string requestedBy,
            List<ChangeRequestItemStaging> changeRequestItemStagings = null, CommentsStagingModel comment = null,
            List<AttachmentsModel> attachmentsStagingModels = null, string changeContent = null)
        {
            if (String.IsNullOrWhiteSpace(entity?.NumDoors))
            {
                throw new ArgumentNullException(nameof(entity));
            }

            changeRequestItemStagings = changeRequestItemStagings ?? new List<ChangeRequestItemStaging>();
            await ValidateBodyNumDoorsIsNotDuplicate(entity, ChangeType.Add);
            await ValidateBodyNumDoorsHasNoChangeRequest(entity);

            changeRequestItemStagings.Add(new ChangeRequestItemStaging()
            {
                ChangeType = ChangeType.Add,
                EntityId = entity.Id.ToString(),
                CreatedDateTime = DateTime.UtcNow,
                Entity = typeof(BodyNumDoors).Name,
                Payload = base.Serializer.Serialize(entity)
            });

            changeContent = string.Format("{0}"
                , entity.NumDoors);

            // NOTE: change-request-comments-staging perfomed on base

            return await base.SubmitAddChangeRequestAsync(entity, requestedBy, changeRequestItemStagings, comment, attachmentsStagingModels, changeContent);
        }

        public override async Task<long> SubmitUpdateChangeRequestAsync<TId>(BodyNumDoors entity, TId id, string requestedBy,
            ChangeType changeType = ChangeType.None,
            List<ChangeRequestItemStaging> changeRequestItemStagings = null, CommentsStagingModel comment = null, List<AttachmentsModel> attachments = null, string changeContent = null)
        {
            if (String.IsNullOrWhiteSpace(entity?.NumDoors))
            {
                throw new ArgumentNullException(nameof(entity));
            }

            if (id.Equals(default(TId)))
            {
                throw new ArgumentNullException(nameof(id));
            }

            if (!entity.Id.Equals(id))
            {
                throw new ArgumentException("Invalid BodyNumDoors Id");
            }

            var bodyNumDoorsFromDb = await FindAsync(id);
            if (bodyNumDoorsFromDb == null)
            {
                throw new NoRecordFound("No Body Num Doors exist");
            }

            changeRequestItemStagings = changeRequestItemStagings ?? new List<ChangeRequestItemStaging>();
            await ValidateBodyNumDoorsIsNotDuplicate(entity,ChangeType.Modify);
            await ValidateBodyNumDoorsHasNoChangeRequest(entity);
            await ValidateBodyNumDoorsLookUpHasNoChangeRequest(entity);

            entity.InsertDate = bodyNumDoorsFromDb.InsertDate;
            var existingEntity = new BodyNumDoors()
            {
                Id = bodyNumDoorsFromDb.Id,
                NumDoors = bodyNumDoorsFromDb.NumDoors,
                ChangeRequestId = bodyNumDoorsFromDb.ChangeRequestId,
                VehicleToBodyStyleConfigCount = bodyNumDoorsFromDb.VehicleToBodyStyleConfigCount,
                DeleteDate = bodyNumDoorsFromDb.DeleteDate,
                InsertDate = bodyNumDoorsFromDb.InsertDate,
                LastUpdateDate = bodyNumDoorsFromDb.LastUpdateDate
            };
            changeRequestItemStagings.Add(new ChangeRequestItemStaging()
            {
                ChangeType = ChangeType.Modify,
                EntityId = entity.Id.ToString(),
                CreatedDateTime = DateTime.UtcNow,
                Entity = typeof(BodyNumDoors).Name,
                Payload = Serializer.Serialize(entity),
                ExistingPayload = Serializer.Serialize(existingEntity)
            });

            changeContent = string.Format("{0} > {1}"
                , bodyNumDoorsFromDb.NumDoors
                , entity.NumDoors);

            var changeRequestId = await base.SubmitUpdateChangeRequestAsync(entity, id, requestedBy, ChangeType.Modify, changeRequestItemStagings, comment, attachments, changeContent);

            bodyNumDoorsFromDb.ChangeRequestId = changeRequestId;
            _bodyNumDoorsRepositoryService.Update(bodyNumDoorsFromDb);
            Repositories.SaveChanges();

            return changeRequestId;
        }

        public override async Task<long> SubmitDeleteChangeRequestAsync<TId>(BodyNumDoors entity, TId id, string requestedBy, List<ChangeRequestItemStaging> changeRequestItemStagings = null, CommentsStagingModel comment = null, List<AttachmentsModel> attachmentsStaging = null, string changeContent = null)
        {
            if (id.Equals(default(TId)))
            {
                throw new ArgumentNullException(nameof(id));
            }
            var bodyNumDoorsFromDb = await FindAsync(id);
            if (bodyNumDoorsFromDb == null)
            {
                throw new NoRecordFound("No BodyNumDoors exist");
            }

            changeRequestItemStagings = changeRequestItemStagings ?? new List<ChangeRequestItemStaging>();

            await ValidateBodyNumDoorsLookUpHasNoChangeRequest(bodyNumDoorsFromDb);

            changeRequestItemStagings.Add(new ChangeRequestItemStaging()
            {
                EntityId = id.ToString(),
                Entity = typeof(BodyNumDoors).Name,
                ChangeType = ChangeType.Delete,
                CreatedDateTime = DateTime.UtcNow,
                Payload = Serializer.Serialize(new BodyNumDoors()
                {
                    Id = bodyNumDoorsFromDb.Id,
                    NumDoors = bodyNumDoorsFromDb.NumDoors
                })
            });

            changeContent = string.Format("{0}"
                , bodyNumDoorsFromDb.NumDoors);

            var changeRequestId = await base.SubmitDeleteChangeRequestAsync(bodyNumDoorsFromDb, id, requestedBy, changeRequestItemStagings, comment, attachmentsStaging, changeContent);

            bodyNumDoorsFromDb.ChangeRequestId = changeRequestId;
            _bodyNumDoorsRepositoryService.Update(bodyNumDoorsFromDb);
            Repositories.SaveChanges();

            return changeRequestId;
        }

        public override async Task<ChangeRequestStagingModel<BodyNumDoors>> GetChangeRequestStaging<TId>(TId changeRequestId)
        {
            ChangeRequestStagingModel<BodyNumDoors> staging =
              await ChangeRequestBusinessService.GetChangeRequestStagingByChangeRequestIdAsync<BodyNumDoors, TId>(changeRequestId);
            
            return staging;
        }

        private async Task ValidateBodyNumDoorsHasNoChangeRequest(BodyNumDoors bodyNumDoors)
        {
            IList<BodyNumDoors> bodyNumDoorsFromDb = await _bodyNumDoorsRepositoryService.GetAsync(x => x.NumDoors.ToLower().Equals(bodyNumDoors.NumDoors.ToLower()) && x.DeleteDate == null);

            if (bodyNumDoorsFromDb != null && bodyNumDoorsFromDb.Any())
            {
                throw new RecordAlreadyExist("Body NumDoors already exists");
            }
            // vehicle
            var changeRequestId =
                await ChangeRequestBusinessService.ChangeRequestExist<BodyNumDoors>(
                        x => x.NumDoors.ToLower().Equals(bodyNumDoors.NumDoors.ToLower()));
            if (changeRequestId > 0)

            {
                throw new ChangeRequestExistException($"There is already an open CR ID {changeRequestId} for the same BodyNumDoors.");
            }
         }

        private async Task ValidateBodyNumDoorsLookUpHasNoChangeRequest(BodyNumDoors bodyNumDoors)
        {
            var changeRequestId =
               await ChangeRequestBusinessService.ChangeRequestExist<BodyStyleConfig>(x => x.BodyNumDoorsId.Equals(bodyNumDoors.Id));
            if (changeRequestId > 0)

            {
                throw new ChangeRequestExistException($"There is already an open CR ID {changeRequestId} for Body Config using this BodyNumDoors.");
            }
}

        public override AssociationCount AssociatedCount(string payload)
        {
            AssociationCount count = new AssociationCount();
            if (payload != null)
            {
                BodyNumDoors deserializedEntry = Serializer.Deserialize<BodyNumDoors>(payload);
                count.BodyStyleConfigCount = deserializedEntry.BodyStyleConfigCount;
                count.VehicleToBodyStyleConfigCount = deserializedEntry.VehicleToBodyStyleConfigCount;
            }
            return count;
        }

        public override async Task ClearChangeRequestId(long changeRequestId)
        {
            var entities = await _bodyNumDoorsRepositoryService.GetAsync(x => x.ChangeRequestId == changeRequestId && x.DeleteDate == null);

            if (entities != null && entities.Any())
            {
                var entity = entities.First();   //can only be a single item
                entity.ChangeRequestId = null;
                _bodyNumDoorsRepositoryService.Update(entity);
                Repositories.SaveChanges();
            }
        }

        /// <summary>
        /// Find a single record which is not marked as deleted.
        /// </summary>
        /// <typeparam name="TKey">any</typeparam>
        /// <param name="id">the key property</param>
        /// <returns>Single entity object if condition matches, null if not found.</returns>
        private async Task<BodyNumDoors> FindAsync<TKey>(TKey id)
        {
            var entityId = Convert.ToInt32(id);
            var entities = await _bodyNumDoorsRepositoryService.GetAsync(m => m.Id == entityId && m.DeleteDate == null, 1);
            if (entities != null && entities.Any())
            {
                return entities.First();
            }
            return null;
        }

        private async Task ValidateBodyNumDoorsIsNotDuplicate(BodyNumDoors bodyNumDoors, ChangeType changeType)
        {
            // validate no duplicate Make Name.
            if (changeType == ChangeType.Add)
            {
                IList<BodyNumDoors> bodyNumDoorsFromDb =
                    await
                        _bodyNumDoorsRepositoryService.GetAsync(
                            x => x.NumDoors.Trim().Equals(bodyNumDoors.NumDoors.Trim(), StringComparison.CurrentCultureIgnoreCase)
                                 && x.DeleteDate == null);
                if (bodyNumDoorsFromDb != null && bodyNumDoorsFromDb.Any())
                {
                    throw new RecordAlreadyExist($"There is aleady a Body NumDoors {bodyNumDoors.NumDoors}.");
                }
            }
            else if (changeType == ChangeType.Modify) // allow whitespace and case correction
            {
                var bodyNumDoorsFromDb = await FindAsync(bodyNumDoors.Id);

                if (bodyNumDoors.NumDoors.Trim().Equals(bodyNumDoorsFromDb.NumDoors.Trim(), StringComparison.CurrentCulture))
                {
                    throw new RecordAlreadyExist($"There is aleady a Body NumDoors {bodyNumDoors.NumDoors}.");
                }
                if (!bodyNumDoors.NumDoors.Trim().Equals(bodyNumDoorsFromDb.NumDoors.Trim(), StringComparison.CurrentCultureIgnoreCase))
                {
                    IList<BodyNumDoors> makesFromDb =
                        await
                            _bodyNumDoorsRepositoryService.GetAsync(
                                x => x.NumDoors.Trim().Equals(bodyNumDoors.NumDoors.Trim(), StringComparison.CurrentCultureIgnoreCase)
                                     && x.DeleteDate == null);
                    if (makesFromDb != null && makesFromDb.Any())
                    {
                        throw new RecordAlreadyExist($"There is aleady a Body NumDoors {bodyNumDoors.NumDoors}.");
                    }
                }
            }
        }
    }
}
