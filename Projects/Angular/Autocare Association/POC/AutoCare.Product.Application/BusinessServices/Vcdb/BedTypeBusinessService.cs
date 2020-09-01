
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
    public class BedTypeBusinessService : VcdbBusinessService<BedType>, IBedTypeBusinessService
    {
        private readonly ISqlServerEfRepositoryService<BedType> _bedTypeRepositoryService = null;

        public BedTypeBusinessService(IVcdbUnitOfWork vcdbRepositoryService,
            IVcdbChangeRequestBusinessService vcdbChangeRequestBusinessService,
            ITextSerializer serializer)
            : base(vcdbRepositoryService, vcdbChangeRequestBusinessService, serializer)
        {
            _bedTypeRepositoryService = Repositories.GetRepositoryService<BedType>() as ISqlServerEfRepositoryService<BedType>;
        }

        public override async Task<List<BedType>> GetAllAsync(int topCount = 0)
        {
            return await _bedTypeRepositoryService.GetAsync(x => x.DeleteDate == null, topCount);
        }

        public override async Task<BedType> GetAsync<TKey>(TKey id)
        {
            if (id.Equals(default(TKey)))
            {
                throw new ArgumentNullException(nameof(id));
            }

            var bedType = await FindAsync(id);

            if (bedType == null)
            {
                throw new NoRecordFound("Record Not Found");
            }

            bedType.BedConfigCount = await _bedTypeRepositoryService.GetCountAsync(bedType, x => x.BedConfigs,
                y => y.DeleteDate == null);

            var bedTypeId = Convert.ToInt32(id);
            bedType.VehicleToBedConfigCount = _bedTypeRepositoryService
                .GetAllQueryable()
                .Where(x => x.Id == bedTypeId)
                .Include("BedConfigs.VehicleToBedConfigs")
                .SelectMany(x => x.BedConfigs)
                .SelectMany(x => x.VehicleToBedConfigs).Count(y => y.DeleteDate == null);

            return bedType;
        }

        public override async Task<long> SubmitAddChangeRequestAsync(BedType entity, string requestedBy,
            List<ChangeRequestItemStaging> changeRequestItemStagings = null, CommentsStagingModel comment = null,
            List<AttachmentsModel> attachmentsStagingModels = null, string changeContent = null)
        {
            if (String.IsNullOrWhiteSpace(entity?.Name))
            {
                throw new ArgumentNullException(nameof(entity));
            }

            changeRequestItemStagings = changeRequestItemStagings ?? new List<ChangeRequestItemStaging>();

            await ValidateBedTypeHasNoChangeRequest(entity);

            changeRequestItemStagings.Add(new ChangeRequestItemStaging()
            {
                ChangeType = ChangeType.Add,
                EntityId = entity.Id.ToString(),
                Entity = typeof(BedType).Name,
                CreatedDateTime = DateTime.UtcNow,
                Payload = Serializer.Serialize(entity)
            });

            changeContent = string.Format("{0}"
                , entity.Name);

            // NOTE: change-request-comments-staging perfomed on base

            return await base.SubmitAddChangeRequestAsync(entity, requestedBy, changeRequestItemStagings, comment, attachmentsStagingModels, changeContent);
        }

        public override async Task<long> SubmitUpdateChangeRequestAsync<TId>(BedType entity, TId id, string requestedBy,
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
                throw new ArgumentException("Invalid Bed Type Id");
            }

            var bedTypeFromDb = await FindAsync(id);
            if (bedTypeFromDb == null)
            {
                throw new NoRecordFound("No Bed Type exist");
            }

            changeRequestItemStagings = changeRequestItemStagings ?? new List<ChangeRequestItemStaging>();

            await ValidateBedTypeHasNoChangeRequest(entity);
            await ValidateBedTypeLookUpHasNoChangeRequest(entity);

            entity.InsertDate = bedTypeFromDb.InsertDate;

            // to eliminate circular reference during serialize
            var existingEntity = new BedType()
            {
                Id = bedTypeFromDb.Id,
                Name = bedTypeFromDb.Name,
                ChangeRequestId = bedTypeFromDb.ChangeRequestId,
                BedConfigCount = bedTypeFromDb.BedConfigCount,
                VehicleToBedConfigCount = bedTypeFromDb.VehicleToBedConfigCount,
                DeleteDate = bedTypeFromDb.DeleteDate,
                InsertDate = bedTypeFromDb.InsertDate,
                LastUpdateDate = bedTypeFromDb.LastUpdateDate
            };

            changeRequestItemStagings.Add(new ChangeRequestItemStaging()
            {
                ChangeType = ChangeType.Modify,
                EntityId = entity.Id.ToString(),
                CreatedDateTime = DateTime.UtcNow,
                Entity = typeof(BedType).Name,
                Payload = Serializer.Serialize(entity),
                ExistingPayload = base.Serializer.Serialize(existingEntity) // add existing record as ExistingPayload
            });

            changeContent = string.Format("{0} > {1}"
                , bedTypeFromDb.Name
                , entity.Name);

            var changeRequestId = await base.SubmitUpdateChangeRequestAsync(entity, id, requestedBy, ChangeType.Modify, changeRequestItemStagings, comment, attachments, changeContent);

            bedTypeFromDb.ChangeRequestId = changeRequestId;
            _bedTypeRepositoryService.Update(bedTypeFromDb);
            Repositories.SaveChanges();

            return changeRequestId;
        }

        public override async Task<long> SubmitDeleteChangeRequestAsync<TId>(BedType entity, TId id, string requestedBy, List<ChangeRequestItemStaging> changeRequestItemStagings = null, CommentsStagingModel comment = null, List<AttachmentsModel> attachmentsStaging = null, string changeContent = null)
        {
            // validation
            if (id.Equals(default(TId)))
            {
                throw new ArgumentNullException(nameof(id));
            }
            var bedTypeFromDb = await FindAsync(id);
            if (bedTypeFromDb == null)
            {
                throw new NoRecordFound("No Bed Type exist");
            }

            changeRequestItemStagings = changeRequestItemStagings ?? new List<ChangeRequestItemStaging>();

            await ValidateBedTypeLookUpHasNoChangeRequest(bedTypeFromDb);

            changeRequestItemStagings.Add(new ChangeRequestItemStaging()
            {
                EntityId = bedTypeFromDb.Id.ToString(),
                Entity = typeof(BedType).Name,
                ChangeType = ChangeType.Delete,
                CreatedDateTime = DateTime.UtcNow,
                Payload = Serializer.Serialize(new Make()
                {
                    Id = bedTypeFromDb.Id,
                    Name = bedTypeFromDb.Name
                })
            });

            changeContent = string.Format("{0}"
                , bedTypeFromDb.Name);

            var changeRequestId = await base.SubmitDeleteChangeRequestAsync(bedTypeFromDb, id, requestedBy, changeRequestItemStagings, comment, attachmentsStaging, changeContent);

            bedTypeFromDb.ChangeRequestId = changeRequestId;
            _bedTypeRepositoryService.Update(bedTypeFromDb);
            Repositories.SaveChanges();

            return changeRequestId;
        }

        public override async Task<ChangeRequestStagingModel<BedType>> GetChangeRequestStaging<TId>(TId changeRequestId)
        {
            ChangeRequestStagingModel<BedType> staging =
                await ChangeRequestBusinessService.GetChangeRequestStagingByChangeRequestIdAsync<BedType, TId>(changeRequestId);
            //if (!staging.StagingItem.ChangeType.Equals(ChangeType.Add.ToString(), StringComparison.CurrentCultureIgnoreCase))
            //{
            //    BrakeType currentBrakeType = await FindAsync(staging.StagingItem.EntityId);
            //    staging.EntityCurrent = currentBrakeType;
            //}

            return staging;
        }

        private async Task ValidateBedTypeHasNoChangeRequest(BedType bedType)
        {
            IList<BedType> bedTypesFromDb = await _bedTypeRepositoryService.GetAsync(x => x.Name.ToLower().Equals(bedType.Name.ToLower()) && x.DeleteDate == null);

            if (bedTypesFromDb != null && bedTypesFromDb.Any())
            {
                throw new RecordAlreadyExist("Bed Type already exists");
            }

            var changerequestid =
                await
                    ChangeRequestBusinessService.ChangeRequestExist<BedType>(
                        x => x.Name.ToLower().Equals(bedType.Name.ToLower()));
            if (changerequestid > 0)
            {
                throw new ChangeRequestExistException("There is already an open CR ID " + changerequestid + "  with same Bed Type Name.");
            }
        }

        private async Task ValidateBedTypeLookUpHasNoChangeRequest(BedType bedType)
        {

            var changerequestid =
                await
                    ChangeRequestBusinessService.ChangeRequestExist<BedConfig>(
                        x => x.BedTypeId.Equals(bedType.Id));
            if (changerequestid > 0)
            {
                throw new ChangeRequestExistException($"There is already an open CR ID " + changerequestid + " for bed Config using this Bed Type.");
            }
        }

        public override AssociationCount AssociatedCount(string payload)
        {
            AssociationCount count = new AssociationCount();
            if (payload != null)
            {
                BedType deserializedEntry = Serializer.Deserialize<BedType>(payload);
                count.VehicleToBedConfigCount = deserializedEntry.VehicleToBedConfigCount;
            }
            return count;
        }

        public override async Task ClearChangeRequestId(long changeRequestId)
        {
            var entities = await _bedTypeRepositoryService.GetAsync(m => m.ChangeRequestId == changeRequestId);

            if (entities != null && entities.Any())
            {
                var entity = entities.First();   //can only be a single item
                entity.ChangeRequestId = null;
                _bedTypeRepositoryService.Update(entity);
                Repositories.SaveChanges();
            }
        }

        /// <summary>
        /// Find a single record which is not marked as deleted.
        /// </summary>
        /// <typeparam name="TKey">any</typeparam>
        /// <param name="id">the key property</param>
        /// <returns>Single entity object if condition matches, null if not found.</returns>
        private async Task<BedType> FindAsync<TKey>(TKey id)
        {
            var entityId = Convert.ToInt32(id);
            var entities = await _bedTypeRepositoryService.GetAsync(m => m.Id == entityId && m.DeleteDate == null, 1);
            if (entities != null && entities.Any())
            {
                return entities.First();
            }
            return null;
        }
    }
}
