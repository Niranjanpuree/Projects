using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AutoCare.Product.Application.Infrastructure.ExceptionHelpers;
using AutoCare.Product.Application.Models.BusinessModels;
using AutoCare.Product.Application.RepositoryServices;
using AutoCare.Product.Infrastructure.ExceptionHandler;
using AutoCare.Product.Infrastructure.RepositoryService;
using AutoCare.Product.Infrastructure.Serializer;
using AutoCare.Product.Vcdb.Model;

namespace AutoCare.Product.Application.BusinessServices.Vcdb
{
    public class FuelDeliveryTypeBusinessService : VcdbBusinessService<FuelDeliveryType>, IFuelDeliveryTypeBusinessService
    {
        private readonly ISqlServerEfRepositoryService<FuelDeliveryType> _fuelDeliveryTypeRepositoryService;
        private readonly IVcdbChangeRequestBusinessService _vcdbChangeRequestBusinessService;

        public FuelDeliveryTypeBusinessService(IVcdbUnitOfWork vcdbRepositoryService,
            IVcdbChangeRequestBusinessService vcdbChangeRequestBusinessService,
            ITextSerializer serializer)
            : base(vcdbRepositoryService, vcdbChangeRequestBusinessService, serializer)
        {
            _fuelDeliveryTypeRepositoryService = Repositories.GetRepositoryService<FuelDeliveryType>() as ISqlServerEfRepositoryService<FuelDeliveryType>;
            _vcdbChangeRequestBusinessService = vcdbChangeRequestBusinessService;
        }

        public override async Task<List<FuelDeliveryType>> GetAllAsync(int topCount = 0)
        {
            return await _fuelDeliveryTypeRepositoryService.GetAsync(m => m.DeleteDate == null, topCount);
        }

        public override async Task<FuelDeliveryType> GetAsync<TKey>(TKey id)
        {
            // validations
            if (id.Equals(default(TKey)))
            {
                throw new ArgumentNullException(nameof(id));
            }

            var fuelDeliveryType = await FindAsync(id);

            if (fuelDeliveryType == null)
            {
                throw new NoRecordFound("Record Not Found");
            }

            fuelDeliveryType.FuelDeliveryConfigCount = await _fuelDeliveryTypeRepositoryService.GetCountAsync(fuelDeliveryType, x => x.FuelDeliveryConfigs, y => y.DeleteDate == null);

            var fuelDeliveryTypeId = Convert.ToInt32(id);
            //fuelDeliveryType.FuelDeliveryConfigCount = _fuelDeliveryTypeRepositoryService
            //    .GetAllQueryable()
            //    .Where(x => x.Id == fuelDeliveryTypeId).Include("FuelDeliveryConfigs.")
            //    .SelectMany(x => x.FuelDeliveryConfigs)
            //    .SelectMany(x => x.).Count(y => y.DeleteDate == null);

            return fuelDeliveryType;
        }

        public override async Task<long> SubmitAddChangeRequestAsync(FuelDeliveryType entity, string requestedBy,
            List<ChangeRequestItemStaging> changeRequestItemStagings = null, CommentsStagingModel comment = null,
            List<AttachmentsModel> attachmentsStagingModels = null, string changeContent = null)
        {
            if (string.IsNullOrWhiteSpace(entity?.FuelDeliveryTypeName))
            {
                throw new ArgumentNullException(nameof(entity));
            }

            // validations
            await ValidateFuelDeliveryTypeIsNotDuplicate(entity, ChangeType.Add);
            await ValidateFuelDeliveryTypeHasNoChangeRequest(entity, ChangeType.Add);

            changeRequestItemStagings = changeRequestItemStagings ?? new List<ChangeRequestItemStaging>();
            changeRequestItemStagings.Add(new ChangeRequestItemStaging
            {
                EntityId = entity.Id.ToString(),
                Entity = typeof(FuelDeliveryType).Name,
                ChangeType = ChangeType.Add,
                CreatedDateTime = DateTime.UtcNow,
                Payload = Serializer.Serialize(entity)
            });

            changeContent = string.Format("{0}"
                , entity.FuelDeliveryTypeName);

            return await base.SubmitAddChangeRequestAsync(entity, requestedBy, changeRequestItemStagings, comment, attachmentsStagingModels, changeContent);
        }

        public override async Task<long> SubmitUpdateChangeRequestAsync<TId>(FuelDeliveryType entity, TId id, string requestedBy,
            ChangeType changeType = ChangeType.None,
            List<ChangeRequestItemStaging> changeRequestItemStagings = null, CommentsStagingModel comment = null, List<AttachmentsModel> attachments = null, string changeContent = null)
        {
            if (string.IsNullOrWhiteSpace(entity?.FuelDeliveryTypeName))
            {
                throw new ArgumentNullException(nameof(entity));
            }
            if (id.Equals(default(TId)))
            {
                throw new ArgumentNullException(nameof(id));
            }
            if (!entity.Id.Equals(id))
            {
                throw new ArgumentException("Invalid FuelDeliveryType Id");
            }

            var fuelDeliveryTypeFromDb = await FindAsync(id);

            if (fuelDeliveryTypeFromDb == null)
            {
                throw new NoRecordFound("Record Not Found");
            }

            // validations
            await ValidateFuelDeliveryTypeIsNotDuplicate(entity, ChangeType.Modify);
            await ValidateFuelDeliveryTypeHasNoChangeRequest(entity, ChangeType.Modify);
            await ValidateFuelDeliveryTypeLookUpHasNoChangeRequest(entity);

            // fill up audit information
            entity.InsertDate = fuelDeliveryTypeFromDb.InsertDate;
            entity.LastUpdateDate = fuelDeliveryTypeFromDb.LastUpdateDate;
            entity.DeleteDate = fuelDeliveryTypeFromDb.DeleteDate;

            changeRequestItemStagings = changeRequestItemStagings ?? new List<ChangeRequestItemStaging>();

            // to eliminate circular reference during serialize
            var existingEntity = new FuelDeliveryType()
            {
                Id = fuelDeliveryTypeFromDb.Id,
                FuelDeliveryTypeName = fuelDeliveryTypeFromDb.FuelDeliveryTypeName,
                ChangeRequestId = fuelDeliveryTypeFromDb.ChangeRequestId,
                FuelDeliveryConfigCount = fuelDeliveryTypeFromDb.FuelDeliveryConfigCount,
                FuelDeliveryConfigs = fuelDeliveryTypeFromDb.FuelDeliveryConfigs,
                DeleteDate = fuelDeliveryTypeFromDb.DeleteDate,
                InsertDate = fuelDeliveryTypeFromDb.InsertDate,
                LastUpdateDate = fuelDeliveryTypeFromDb.LastUpdateDate
            };

            changeRequestItemStagings.Add(new ChangeRequestItemStaging
            {
                EntityId = entity.Id.ToString(),
                Entity = typeof(FuelDeliveryType).Name,
                ChangeType = ChangeType.Modify,
                CreatedDateTime = DateTime.UtcNow,
                Payload = Serializer.Serialize(entity),
                ExistingPayload = base.Serializer.Serialize(existingEntity) // add existing record as ExistingPayload
            });

            changeContent = string.Format("{0} > {1}"
                , fuelDeliveryTypeFromDb.FuelDeliveryTypeName
                , entity.FuelDeliveryTypeName);

            var changeRequestId = await
                    base.SubmitUpdateChangeRequestAsync(entity, id, requestedBy, ChangeType.Modify,
                        changeRequestItemStagings, comment, attachments, changeContent);

            fuelDeliveryTypeFromDb.ChangeRequestId = changeRequestId;
            _fuelDeliveryTypeRepositoryService.Update(fuelDeliveryTypeFromDb);
            Repositories.SaveChanges();

            return changeRequestId;
        }

        public override async Task<long> SubmitDeleteChangeRequestAsync<TId>(FuelDeliveryType entity, TId id, string requestedBy,
            List<ChangeRequestItemStaging> changeRequestItemStagings = null, CommentsStagingModel comment = null, List<AttachmentsModel> attachmentsStaging = null, string changeContent = null)
        {
            if (id.Equals(default(TId)))
            {
                throw new ArgumentNullException(nameof(id));
            }

            var fuelDeliveryTypeFromDb = await FindAsync(id);

            if (fuelDeliveryTypeFromDb == null)
            {
                throw new NoRecordFound("Record Not Found");
            }

            // validations
            await ValidateFuelDeliveryTypeLookUpHasNoChangeRequest(entity);

            changeRequestItemStagings = changeRequestItemStagings ?? new List<ChangeRequestItemStaging>();
            changeRequestItemStagings.Add(new ChangeRequestItemStaging
            {
                EntityId = entity.Id.ToString(),
                Entity = typeof(FuelDeliveryType).Name,
                ChangeType = ChangeType.Delete,
                CreatedDateTime = DateTime.UtcNow,
                Payload = Serializer.Serialize(new FuelDeliveryType
                {
                    Id = fuelDeliveryTypeFromDb.Id,
                    FuelDeliveryTypeName = fuelDeliveryTypeFromDb.FuelDeliveryTypeName
                })
            });

            changeContent = string.Format("{0}"
                , entity.FuelDeliveryTypeName);

            var changeRequestId = await base.SubmitDeleteChangeRequestAsync(entity, id, requestedBy, changeRequestItemStagings, comment, attachmentsStaging, changeContent);

            fuelDeliveryTypeFromDb.ChangeRequestId = changeRequestId;
            _fuelDeliveryTypeRepositoryService.Update(fuelDeliveryTypeFromDb);
            Repositories.SaveChanges();

            return changeRequestId;
        }

        public override async Task<ChangeRequestStagingModel<FuelDeliveryType>> GetChangeRequestStaging<TId>(TId changeRequestId)
        {
            ChangeRequestStagingModel<FuelDeliveryType> staging = await
                this._vcdbChangeRequestBusinessService.GetChangeRequestStagingByChangeRequestIdAsync<FuelDeliveryType, TId>(changeRequestId);

            // business specific task.
            // fill value of "EntityCurrent"
            //if (!staging.StagingItem.ChangeType.Equals(ChangeType.Add.ToString(), StringComparison.CurrentCultureIgnoreCase))
            //{
            //    Make currentMake = await FindAsync(staging.StagingItem.EntityId);
            //    staging.EntityCurrent =  await FindAsync(staging.StagingItem.EntityId);
            //}

            return staging;
        }

        public override AssociationCount AssociatedCount(string payload)
        {
            AssociationCount count = new AssociationCount();
            if (payload != null)
            {
                FuelDeliveryType deserializedEntry = Serializer.Deserialize<FuelDeliveryType>(payload);
                count.FuelDeliveryConfigCount = deserializedEntry.FuelDeliveryConfigCount;
            }
            return count;
        }

        public override async Task ClearChangeRequestId(long changeRequestId)
        {
            var entities = await _fuelDeliveryTypeRepositoryService.GetAsync(m => m.ChangeRequestId == changeRequestId && m.DeleteDate == null);

            if (entities != null && entities.Any())
            {
                var entity = entities.First();   //can only be a single item
                entity.ChangeRequestId = null;
                _fuelDeliveryTypeRepositoryService.Update(entity);
                Repositories.SaveChanges();
            }
        }

        private async Task ValidateFuelDeliveryTypeIsNotDuplicate(FuelDeliveryType fuelDeliveryType, ChangeType changeType)
        {
            // validate no duplicate FuelDeliveryType Name.
            if (changeType == ChangeType.Add)
            {
                IList<FuelDeliveryType> fuelDeliveryTypesFromDb =
                    await
                        _fuelDeliveryTypeRepositoryService.GetAsync(
                            x => x.FuelDeliveryTypeName.Trim().Equals(fuelDeliveryType.FuelDeliveryTypeName.Trim(), StringComparison.CurrentCultureIgnoreCase)
                                 && x.DeleteDate == null);
                if (fuelDeliveryTypesFromDb != null && fuelDeliveryTypesFromDb.Any())
                {
                    throw new RecordAlreadyExist($"There is aleady a FuelDeliveryType Name: {fuelDeliveryType.FuelDeliveryTypeName}.");
                }
            }
            else if (changeType == ChangeType.Modify) // allow whitespace and case correction
            {
                var fuelDeliveryTypeFromDb = await FindAsync(fuelDeliveryType.Id);

                if (fuelDeliveryType.FuelDeliveryTypeName.Trim().Equals(fuelDeliveryTypeFromDb.FuelDeliveryTypeName.Trim(), StringComparison.CurrentCulture))
                {
                    throw new RecordAlreadyExist($"There is aleady a fuelDeliveryType Name: {fuelDeliveryType.FuelDeliveryTypeName}.");
                }
                if (!fuelDeliveryType.FuelDeliveryTypeName.Trim().Equals(fuelDeliveryTypeFromDb.FuelDeliveryTypeName.Trim(), StringComparison.CurrentCultureIgnoreCase))
                {
                    IList<FuelDeliveryType> fuelDeliveryTypesFromDb =
                        await
                            _fuelDeliveryTypeRepositoryService.GetAsync(
                                x => x.FuelDeliveryTypeName.Trim().Equals(fuelDeliveryType.FuelDeliveryTypeName.Trim(), StringComparison.CurrentCultureIgnoreCase)
                                     && x.DeleteDate == null);
                    if (fuelDeliveryTypesFromDb != null && fuelDeliveryTypesFromDb.Any())
                    {
                        throw new RecordAlreadyExist($"There is aleady a fuelDeliveryType Name: {fuelDeliveryType.FuelDeliveryTypeName}.");
                    }
                }
            }
        }

        private async Task ValidateFuelDeliveryTypeHasNoChangeRequest(FuelDeliveryType fuelDeliveryType, ChangeType changeType)
        {
            // validate no CR for this FuelDeliveryType Id.
            if (changeType != ChangeType.Add)
            {

                var changerequestid =
                    (await ChangeRequestBusinessService.ChangeRequestExist<FuelDeliveryType>(item => item.Id == fuelDeliveryType.Id));

                if (changerequestid > 0)
                {
                    throw new ChangeRequestExistException($"There is already an open CR ID {changerequestid} with FuelDeliveryType ID : {fuelDeliveryType.Id}.");
                }
            }

            // validate no CR for this FuelDeliveryType Name.
            var changerequestid1 =
            await
                ChangeRequestBusinessService.ChangeRequestExist<FuelDeliveryType>(
                    x => x.FuelDeliveryTypeName.Trim().Equals(fuelDeliveryType.FuelDeliveryTypeName.Trim(), StringComparison.CurrentCultureIgnoreCase));

            if (changerequestid1 > 0)
            {
                throw new ChangeRequestExistException($"There is already an open CR ID {changerequestid1} with same FuelDeliveryType Name : {fuelDeliveryType.FuelDeliveryTypeName}.");
            }
        }

        private async Task ValidateFuelDeliveryTypeLookUpHasNoChangeRequest(FuelDeliveryType fuelDeliveryType)
        {
            // validate no CR for base vehicle using this FuelDeliveryType.
            var changerequestid =
                await
                    ChangeRequestBusinessService.ChangeRequestExist<FuelDeliveryConfig>(x => x.FuelDeliveryTypeId == fuelDeliveryType.Id);
            if (changerequestid > 0)
            {
                throw new ChangeRequestExistException($"There is already an open CR ID {changerequestid} for FuelDeliveryConfig using this FuelDeliveryType.");
            }
        }

        /// <summary>
        /// Find a single record which is not marked as deleted.
        /// </summary>
        /// <typeparam name="TKey">any</typeparam>
        /// <param name="id">the key property</param>
        /// <returns>Single entity object if condition matches, null if not found.</returns>
        private async Task<FuelDeliveryType> FindAsync<TKey>(TKey id)
        {
            var entityId = Convert.ToInt32(id);
            var entities = await _fuelDeliveryTypeRepositoryService.GetAsync(m => m.Id == entityId && m.DeleteDate == null, 1);
            if (entities != null && entities.Any())
            {
                return entities.First();
            }
            return null;
        }
    }
}