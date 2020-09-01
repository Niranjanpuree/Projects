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
    public class BrakeABSBusinessService : VcdbBusinessService<BrakeABS>, IBrakeABSBusinessService
    {
        private readonly ISqlServerEfRepositoryService<BrakeABS> _brakeABSRepositoryService = null;

        public BrakeABSBusinessService(IVcdbUnitOfWork vcdbRepositoryService,
            IVcdbChangeRequestBusinessService vcdbChangeRequestBusinessService,
            ITextSerializer serializer)
            : base(vcdbRepositoryService, vcdbChangeRequestBusinessService, serializer)
        {
            _brakeABSRepositoryService = Repositories.GetRepositoryService<BrakeABS>() as ISqlServerEfRepositoryService<BrakeABS>;
        }

        public override async Task<List<BrakeABS>> GetAllAsync(int topCount = 0)
        {
            return await _brakeABSRepositoryService.GetAsync(x => x.DeleteDate == null, topCount);
        }

        public override async Task<BrakeABS> GetAsync<TKey>(TKey id)
        {
            if (id.Equals(default(TKey)))
            {
                throw new ArgumentNullException(nameof(id));
            }

            var brakeABS = await FindAsync(id);

            if (brakeABS == null)
            {
                throw new NoRecordFound("Record Not Found");
            }

            brakeABS.BrakeConfigCount = await _brakeABSRepositoryService.GetCountAsync(brakeABS, x => x.BrakeConfigs, y => y.DeleteDate == null);

            var brakeTypeId = Convert.ToInt32(id);
            brakeABS.VehicleToBrakeConfigCount = _brakeABSRepositoryService
                .GetAllQueryable()
                .Where(x => x.Id == brakeTypeId)
                .Include("BrakeConfigs.VehicleToBrakeConfigs")
                .SelectMany(x => x.BrakeConfigs)
                .SelectMany(x => x.VehicleToBrakeConfigs).Count(y => y.DeleteDate == null);

            return brakeABS;
        }

        public override async Task<long> SubmitAddChangeRequestAsync(BrakeABS entity, string requestedBy,
            List<ChangeRequestItemStaging> changeRequestItemStagings = null, CommentsStagingModel comment = null,
            List<AttachmentsModel> attachmentsStagingModels = null, string changeContent = null)
        {
            if (String.IsNullOrWhiteSpace(entity?.Name))
            {
                throw new ArgumentNullException(nameof(entity));
            }

            changeRequestItemStagings = changeRequestItemStagings ?? new List<ChangeRequestItemStaging>();
            await ValidateBrakeAbsHasNoChangeRequest(entity);

            changeRequestItemStagings.Add(new ChangeRequestItemStaging()
            {
                ChangeType = ChangeType.Add,
                EntityId = entity.Id.ToString(),
                CreatedDateTime = DateTime.UtcNow,
                Entity = typeof(BrakeABS).Name,
                Payload = base.Serializer.Serialize(entity)
            });

            changeContent = string.Format("{0}"
                , entity.Name);

            // NOTE: change-request-comments-staging perfomed on base

            return await base.SubmitAddChangeRequestAsync(entity, requestedBy, changeRequestItemStagings, comment,attachmentsStagingModels, changeContent);
        }

        public override async Task<long> SubmitUpdateChangeRequestAsync<TId>(BrakeABS entity, TId id, string requestedBy,
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
                throw new ArgumentException("Invalid Brake ABS Id");
            }

            var brakeABSFromDb = await FindAsync(id);
            if (brakeABSFromDb == null)
            {
                throw new NoRecordFound("No Brake ABS exist");
            }

            changeRequestItemStagings = changeRequestItemStagings ?? new List<ChangeRequestItemStaging>();
            await ValidateBrakeAbsHasNoChangeRequest(entity);
            await ValidateBrakeAbsLookUpHasNoChangeRequest(entity);

            entity.InsertDate = brakeABSFromDb.InsertDate;
            var existingEntity = new BrakeABS()
            {
                Id = brakeABSFromDb.Id,
                Name = brakeABSFromDb.Name,
                ChangeRequestId = brakeABSFromDb.ChangeRequestId,
               VehicleToBrakeConfigCount = brakeABSFromDb.VehicleToBrakeConfigCount,
                DeleteDate = brakeABSFromDb.DeleteDate,
                InsertDate = brakeABSFromDb.InsertDate,
                LastUpdateDate = brakeABSFromDb.LastUpdateDate
            };
            changeRequestItemStagings.Add(new ChangeRequestItemStaging()
            {
                ChangeType = ChangeType.Modify,
                EntityId = entity.Id.ToString(),
                CreatedDateTime = DateTime.UtcNow,
                Entity = typeof(BrakeABS).Name,
                Payload = Serializer.Serialize(entity),
                ExistingPayload = Serializer.Serialize(existingEntity)
            });

            changeContent = string.Format("{0} > {1}"
                , brakeABSFromDb.Name
                , entity.Name);

            var changeRequestId = await base.SubmitUpdateChangeRequestAsync(entity, id, requestedBy, ChangeType.Modify, changeRequestItemStagings, comment, attachments, changeContent);

            brakeABSFromDb.ChangeRequestId = changeRequestId;
            _brakeABSRepositoryService.Update(brakeABSFromDb);
            Repositories.SaveChanges();

            return changeRequestId;
        }

        public override async Task<long> SubmitDeleteChangeRequestAsync<TId>(BrakeABS entity, TId id, string requestedBy, List<ChangeRequestItemStaging> changeRequestItemStagings = null, CommentsStagingModel comment = null, List<AttachmentsModel> attachmentsStaging = null, string changeContent = null)
        {
            if (id.Equals(default(TId)))
            {
                throw new ArgumentNullException(nameof(id));
            }
            var breakAbsFromDb = await FindAsync(id);
            if (breakAbsFromDb == null)
            {
                throw new NoRecordFound("No Brake ABS exist");
            }

            changeRequestItemStagings = changeRequestItemStagings ?? new List<ChangeRequestItemStaging>();

            await ValidateBrakeAbsLookUpHasNoChangeRequest(breakAbsFromDb);

            changeRequestItemStagings.Add(new ChangeRequestItemStaging()
            {
                EntityId = id.ToString(),
                Entity = typeof(BrakeABS).Name,
                ChangeType = ChangeType.Delete,
                CreatedDateTime = DateTime.UtcNow,
                Payload = Serializer.Serialize(new BrakeABS
                {
                    Id = breakAbsFromDb.Id,
                    Name = breakAbsFromDb.Name
                })
            });

            changeContent = string.Format("{0}"
                , breakAbsFromDb.Name);

            var changeRequestId = await base.SubmitDeleteChangeRequestAsync(breakAbsFromDb, id, requestedBy, changeRequestItemStagings, comment, attachmentsStaging, changeContent);

            breakAbsFromDb.ChangeRequestId = changeRequestId;
            _brakeABSRepositoryService.Update(breakAbsFromDb);
            Repositories.SaveChanges();

            return changeRequestId;
        }

        public override async Task<ChangeRequestStagingModel<BrakeABS>> GetChangeRequestStaging<TId>(TId changeRequestId)
        {
            ChangeRequestStagingModel<BrakeABS> staging =
              await ChangeRequestBusinessService.GetChangeRequestStagingByChangeRequestIdAsync<BrakeABS, TId>(changeRequestId);
            //if (!staging.StagingItem.ChangeType.Equals(ChangeType.Add.ToString(), StringComparison.CurrentCultureIgnoreCase))
            //{
            //    BrakeABS currentBrakeABS = await FindAsync(staging.StagingItem.EntityId);
            //    staging.EntityCurrent = currentBrakeABS;
            //}

            return staging;
        }

        private async Task ValidateBrakeAbsHasNoChangeRequest(BrakeABS brakeAbs)
        {
            IList<BrakeABS> brakeAbssFromDb = await _brakeABSRepositoryService.GetAsync(x => x.Name.ToLower().Equals(brakeAbs.Name.ToLower()) && x.DeleteDate == null);

            if (brakeAbssFromDb != null && brakeAbssFromDb.Any())
            {
                throw new RecordAlreadyExist("Brake ABS already exists");
            }
            // vehicle
            var changeRequestId =
                await ChangeRequestBusinessService.ChangeRequestExist<BrakeABS>(
                        x => x.Name.ToLower().Equals(brakeAbs.Name.ToLower()));
            if (changeRequestId > 0)

            {
                throw new ChangeRequestExistException($"There is already an open CR ID {changeRequestId} for BrakeABS Name.");
            }
        }

        private async Task ValidateBrakeAbsLookUpHasNoChangeRequest(BrakeABS brakeAbs)
        {
            var changeRequestId =
               await ChangeRequestBusinessService.ChangeRequestExist<BrakeConfig>(x => x.BrakeABSId.Equals(brakeAbs.Id));
            if (changeRequestId > 0)

            {
                throw new ChangeRequestExistException($"There is already an open CR ID {changeRequestId} for brake Config using this Brake ABS.");
            }

        }

        public override AssociationCount AssociatedCount(string payload)
        {
            AssociationCount count = new AssociationCount();
            if (payload != null)
            {
                BrakeABS deserializedEntry = Serializer.Deserialize<BrakeABS>(payload);
                count.BrakeConfigCount = deserializedEntry.BrakeConfigCount;
                count.VehicleToBrakeConfigCount = deserializedEntry.VehicleToBrakeConfigCount;
            }
            return count;
        }

        public override async Task ClearChangeRequestId(long changeRequestId)
        {
            var entities = await _brakeABSRepositoryService.GetAsync(x => x.ChangeRequestId == changeRequestId && x.DeleteDate == null);

            if (entities != null && entities.Any())
            {
                var entity = entities.First();   //can only be a single item
                entity.ChangeRequestId = null;
                _brakeABSRepositoryService.Update(entity);
                Repositories.SaveChanges();
            }
        }

        /// <summary>
        /// Find a single record which is not marked as deleted.
        /// </summary>
        /// <typeparam name="TKey">any</typeparam>
        /// <param name="id">the key property</param>
        /// <returns>Single entity object if condition matches, null if not found.</returns>
        private async Task<BrakeABS> FindAsync<TKey>(TKey id)
        {
            var entityId = Convert.ToInt32(id);
            var entities = await _brakeABSRepositoryService.GetAsync(m => m.Id == entityId && m.DeleteDate == null, 1);
            if (entities != null && entities.Any())
            {
                return entities.First();
            }
            return null;
        }
    }
}
