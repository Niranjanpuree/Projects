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
    public class BedLengthBusinessService : VcdbBusinessService<BedLength>, IBedLengthBusinessService
    {
        private readonly ISqlServerEfRepositoryService<BedLength> _bedLengthRepositoryService = null;
        public BedLengthBusinessService(IVcdbUnitOfWork vcdbRepositoryService,
            IVcdbChangeRequestBusinessService vcdbChangeRequestBusinessService,
            ITextSerializer serializer)
            : base(vcdbRepositoryService, vcdbChangeRequestBusinessService, serializer)
        {
            _bedLengthRepositoryService = Repositories.GetRepositoryService<BedLength>() as ISqlServerEfRepositoryService<BedLength>;
        }

        public override async Task<BedLength> GetAsync<TKey>(TKey id)
        {
            if (id.Equals(default(TKey)))
            {
                throw new ArgumentNullException(nameof(id));
            }

            var bedlength = await FindAsync(id);

            if (bedlength == null)
            {
                throw new NoRecordFound("Record Not Found");
            }
            var bedlengthid = Convert.ToInt32(id);
            bedlength.BedConfigCount = await _bedLengthRepositoryService.GetCountAsync(bedlength, x => x.BedConfigs,
             y => y.DeleteDate == null);
            bedlength.VehicleToBedConfigCount = _bedLengthRepositoryService
                .GetAllQueryable()
                .Where(x => x.Id == bedlengthid)
                .Include("BedConfigs.VehicleToBedConfigs")
                .SelectMany(x => x.BedConfigs)
                .SelectMany(x => x.VehicleToBedConfigs).Count(y => y.DeleteDate == null);

            return bedlength;

        }

        private async Task<BedLength> FindAsync<TKey>(TKey id)
        {
            var entityId = Convert.ToInt32(id);
            var entities = await _bedLengthRepositoryService.GetAsync(m => m.Id == entityId && m.DeleteDate == null, 1);
            if (entities != null && entities.Any())
            {
                return entities.First();
            }
            return null;
        }

        public override async Task<List<BedLength>> GetAllAsync(int topCount = 0)
        {
            return await _bedLengthRepositoryService.GetAsync(m => m.DeleteDate == null, topCount);
        }

        public override async Task<long> SubmitAddChangeRequestAsync(BedLength entity, string requestedBy,
           List<ChangeRequestItemStaging> changeRequestItemStagings = null, CommentsStagingModel comment = null,
           List<AttachmentsModel> attachmentsStagingModels = null, string changeContent = null)
        {
            if (String.IsNullOrWhiteSpace(entity?.Length))
            {
                throw new ArgumentNullException(nameof(entity));
            }
            if (String.IsNullOrWhiteSpace(entity?.BedLengthMetric))
            {
                throw new ArgumentNullException(nameof(entity));
            }

            changeRequestItemStagings = changeRequestItemStagings ?? new List<ChangeRequestItemStaging>();

            await ValidateBedLengthIsNotDuplicate(entity, ChangeType.Add);
            await ValidateBedLengthHasNoChangeRequest(entity);

            changeRequestItemStagings.Add(new ChangeRequestItemStaging()
            {
                ChangeType = ChangeType.Add,
                EntityId = entity.Id.ToString(),
                Entity = typeof(BedLength).Name,
                CreatedDateTime = DateTime.UtcNow,
                Payload = Serializer.Serialize(entity)
            });

            changeContent = string.Format("{0} / {1}"
                , entity.Length, entity.BedLengthMetric);

            // NOTE: change-request-comments-staging perfomed on base

            return await base.SubmitAddChangeRequestAsync(entity, requestedBy, changeRequestItemStagings, comment, attachmentsStagingModels, changeContent);
        }

        public override async Task<long> SubmitUpdateChangeRequestAsync<TId>(BedLength entity, TId id, string requestedBy,
           ChangeType changeType = ChangeType.None,
           List<ChangeRequestItemStaging> changeRequestItemStagings = null, CommentsStagingModel comment = null, List<AttachmentsModel> attachments = null, string changeContent = null)
        {
            if (String.IsNullOrWhiteSpace(entity?.Length))
            {
                throw new ArgumentNullException(nameof(entity));
            }
            if (String.IsNullOrWhiteSpace(entity?.BedLengthMetric))
            {
                throw new ArgumentNullException(nameof(entity));
            }

            if (id.Equals(default(TId)))
            {
                throw new ArgumentNullException(nameof(id));
            }

            if (!entity.Id.Equals(id))
            {
                throw new ArgumentException("Invalid Bed Length Id");
            }

            var bedLengthFromDb = await FindAsync(id);
            if (bedLengthFromDb == null)
            {
                throw new NoRecordFound("No Bed Length exist");
            }

            changeRequestItemStagings = changeRequestItemStagings ?? new List<ChangeRequestItemStaging>();
            await ValidateBedLengthIsNotDuplicate(entity, ChangeType.Modify);
            await ValidateBedLengthHasNoChangeRequest(entity);
            await ValidateBedLengthLookUpHasNoChangeRequest(entity);

            entity.InsertDate = bedLengthFromDb.InsertDate;
            var existingEntity = new BedLength()
            {
                Id = bedLengthFromDb.Id,
                Length = bedLengthFromDb.Length,
                BedLengthMetric = bedLengthFromDb.BedLengthMetric,
                ChangeRequestId = bedLengthFromDb.ChangeRequestId,
                VehicleToBedConfigCount = bedLengthFromDb.VehicleToBedConfigCount,
                DeleteDate = bedLengthFromDb.DeleteDate,
                InsertDate = bedLengthFromDb.InsertDate,
                LastUpdateDate = bedLengthFromDb.LastUpdateDate
            };
            changeRequestItemStagings.Add(new ChangeRequestItemStaging()
            {
                ChangeType = ChangeType.Modify,
                EntityId = entity.Id.ToString(),
                CreatedDateTime = DateTime.UtcNow,
                Entity = typeof(BedLength).Name,
                Payload = Serializer.Serialize(entity),
                ExistingPayload = Serializer.Serialize(existingEntity)
            });

            changeContent = string.Format("{0} / {1} > {2} / {3}"
                , bedLengthFromDb.Length, bedLengthFromDb.BedLengthMetric
                , entity.Length, entity.BedLengthMetric);

            var changeRequestId = await base.SubmitUpdateChangeRequestAsync(entity, id, requestedBy, ChangeType.Modify, changeRequestItemStagings, comment, attachments, changeContent);

            bedLengthFromDb.ChangeRequestId = changeRequestId;
            _bedLengthRepositoryService.Update(bedLengthFromDb);
            Repositories.SaveChanges();

            return changeRequestId;
        }

        public override async Task<long> SubmitDeleteChangeRequestAsync<TId>(BedLength entity, TId id, string requestedBy, List<ChangeRequestItemStaging> changeRequestItemStagings = null, CommentsStagingModel comment = null, List<AttachmentsModel> attachmentsStaging = null, string changeContent = null)
        {
            // validation
            if (id.Equals(default(TId)))
            {
                throw new ArgumentNullException(nameof(id));
            }
            var bedLengthFromDb = await FindAsync(id);
            if (bedLengthFromDb == null)
            {
                throw new NoRecordFound("No Bed Length exist");
            }

            changeRequestItemStagings = changeRequestItemStagings ?? new List<ChangeRequestItemStaging>();

            await ValidateBedLengthLookUpHasNoChangeRequest(bedLengthFromDb);

            changeRequestItemStagings.Add(new ChangeRequestItemStaging()
            {
                EntityId = bedLengthFromDb.Id.ToString(),
                Entity = typeof(BedLength).Name,
                ChangeType = ChangeType.Delete,
                CreatedDateTime = DateTime.UtcNow,
                Payload = Serializer.Serialize(new BedLength()
                {
                    Id = bedLengthFromDb.Id,
                    Length = bedLengthFromDb.Length,
                    BedLengthMetric = bedLengthFromDb.BedLengthMetric
                })
            });

            changeContent = string.Format("{0} / {1}"
                , bedLengthFromDb.Length, bedLengthFromDb.BedLengthMetric);

            var changeRequestId = await base.SubmitDeleteChangeRequestAsync(bedLengthFromDb, id, requestedBy, changeRequestItemStagings, comment, attachmentsStaging, changeContent);

            bedLengthFromDb.ChangeRequestId = changeRequestId;
            _bedLengthRepositoryService.Update(bedLengthFromDb);
            Repositories.SaveChanges();

            return changeRequestId;
        }

        public override AssociationCount AssociatedCount(string payload)
        {
            AssociationCount count = new AssociationCount();
            if (payload != null)
            {
                BedLength deserializedEntry = Serializer.Deserialize<BedLength>(payload);
                count.BedConfigCount = deserializedEntry.BedConfigCount;
                count.VehicleToBedConfigCount = deserializedEntry.VehicleToBedConfigCount;
            }
            return count;
        }

        private async Task ValidateBedLengthLookUpHasNoChangeRequest(BedLength bedLength)
        {

            var changerequestid =
                await
                    ChangeRequestBusinessService.ChangeRequestExist<BedConfig>(
                        x => x.BedLengthId.Equals(bedLength.Id));
            if (changerequestid > 0)
            {
                throw new ChangeRequestExistException($"There is already an open CR ID " + changerequestid + " for bed Config using this Bed Length.");
            }
        }

        private async Task ValidateBedLengthHasNoChangeRequest(BedLength bedLength)
        {
            IList<BedLength> bedLengthsFromDb = await _bedLengthRepositoryService.GetAsync(x => x.Length.Equals(bedLength.Length) && x.BedLengthMetric.Equals(bedLength.BedLengthMetric) && x.DeleteDate == null);

            if (bedLengthsFromDb != null && bedLengthsFromDb.Any())
            {
                throw new RecordAlreadyExist("Bed Length already exists");
            }

            var changerequestid =
                await
                    ChangeRequestBusinessService.ChangeRequestExist<BedLength>(
                        x => x.Length.Equals(bedLength.Length) && x.BedLengthMetric.Equals(bedLength.BedLengthMetric));
            if (changerequestid > 0)
            {
                throw new ChangeRequestExistException("There is already an open CR ID " + changerequestid + " with same Bed Length and Bed Length Metric.");
            }
        }

        public override async Task<ChangeRequestStagingModel<BedLength>> GetChangeRequestStaging<TId>(TId changeRequestId)
        {
            ChangeRequestStagingModel<BedLength> staging =
              await ChangeRequestBusinessService.GetChangeRequestStagingByChangeRequestIdAsync<BedLength, TId>(changeRequestId);
            return staging;
        }

        public override async Task ClearChangeRequestId(long changeRequestId)
        {
            var entities = await _bedLengthRepositoryService.GetAsync(x => x.ChangeRequestId == changeRequestId && x.DeleteDate == null);

            if (entities != null && entities.Any())
            {
                var entity = entities.First();   //can only be a single item
                entity.ChangeRequestId = null;
                _bedLengthRepositoryService.Update(entity);
                Repositories.SaveChanges();
            }
        }

        private async Task ValidateBedLengthIsNotDuplicate(BedLength bedLength, ChangeType changeType)
        {
            // validate no duplicate Make Name.
            if (changeType == ChangeType.Add)
            {
                IList<BedLength> bedLengthFromDb =
                    await
                        _bedLengthRepositoryService.GetAsync(
                            x => x.Length.Trim().Equals(bedLength.Length.Trim(), StringComparison.CurrentCultureIgnoreCase) 
                                 && x.BedLengthMetric.Trim().Equals(bedLength.BedLengthMetric.Trim(), StringComparison.CurrentCultureIgnoreCase)
                                 && x.DeleteDate == null);
                if (bedLengthFromDb != null && bedLengthFromDb.Any())
                {
                    throw new RecordAlreadyExist($"There is aleady a Bed with same Bed Length : {bedLength.Length} and BedLength Metric {bedLength.BedLengthMetric}.");
                }
            }
            else if (changeType == ChangeType.Modify) // allow whitespace and case correction
            {
                var bedlengthFromDb = await FindAsync(bedLength.Id);

                if (bedLength.Length.Trim().Equals(bedlengthFromDb.Length.Trim(), StringComparison.CurrentCulture)
                    && bedLength.BedLengthMetric.Trim().Equals(bedlengthFromDb.BedLengthMetric.Trim(), StringComparison.CurrentCulture))
                {
                    throw new RecordAlreadyExist($"There is aleady a Bed with same Bed Length : {bedLength.Length} and Bed Length Metric {bedLength.BedLengthMetric}.");
                }
                if (!bedLength.Length.Trim().Equals(bedlengthFromDb.Length.Trim(), StringComparison.CurrentCultureIgnoreCase))
                {
                    IList<BedLength> bedLengthsFromDb =
                        await
                            _bedLengthRepositoryService.GetAsync(
                                x => x.Length.Trim().Equals(bedLength.Length.Trim(), StringComparison.CurrentCultureIgnoreCase)
                                     && x.BedLengthMetric.Trim().Equals(bedLength.BedLengthMetric.Trim(), StringComparison.CurrentCultureIgnoreCase)
                                     && x.DeleteDate == null);
                    if (bedLengthsFromDb != null && bedLengthsFromDb.Any())
                    {
                        throw new RecordAlreadyExist($"There is aleady a Bed with same Bed Length : {bedLength.Length} and Bed Length Metric {bedLength.BedLengthMetric}.");
                    }
                }
            }
        }
    }
}
