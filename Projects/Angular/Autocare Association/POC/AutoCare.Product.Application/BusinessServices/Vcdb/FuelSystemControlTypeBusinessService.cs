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
    public class FuelSystemControlTypeBusinessService : VcdbBusinessService<FuelSystemControlType>, IFuelSystemControlTypeBusinessService
    {
        private readonly ISqlServerEfRepositoryService<FuelSystemControlType> _fuelSystemControlTypeRepositoryService;
        private readonly IVcdbChangeRequestBusinessService _vcdbChangeRequestBusinessService;

        public FuelSystemControlTypeBusinessService(IVcdbUnitOfWork vcdbRepositoryService,
            IVcdbChangeRequestBusinessService vcdbChangeRequestBusinessService,
            ITextSerializer serializer)
            : base(vcdbRepositoryService, vcdbChangeRequestBusinessService, serializer)
        {
            _fuelSystemControlTypeRepositoryService = Repositories.GetRepositoryService<FuelSystemControlType>() as ISqlServerEfRepositoryService<FuelSystemControlType>;
            _vcdbChangeRequestBusinessService = vcdbChangeRequestBusinessService;
        }

        public override async Task<List<FuelSystemControlType>> GetAllAsync(int topCount = 0)
        {
            return await _fuelSystemControlTypeRepositoryService.GetAsync(m => m.DeleteDate == null, topCount);
        }

        public override async Task<FuelSystemControlType> GetAsync<TKey>(TKey id)
        {
            // validations
            if (id.Equals(default(TKey)))
            {
                throw new ArgumentNullException(nameof(id));
            }

            var fuelSystemControlType = await FindAsync(id);

            if (fuelSystemControlType == null)
            {
                throw new NoRecordFound("Record Not Found");
            }

            fuelSystemControlType.FuelDeliveryConfigCount = await _fuelSystemControlTypeRepositoryService.GetCountAsync(fuelSystemControlType, x => x.FuelDeliveryConfigs, y => y.DeleteDate == null);

            var fuelSystemControlTypeId = Convert.ToInt32(id);
            //fuelSystemControlType.FuelDeliveryConfigCount = _fuelSystemControlTypeRepositoryService
            //    .GetAllQueryable()
            //    .Where(x => x.Id == fuelSystemControlTypeId).Include("FuelDeliveryConfigs.")
            //    .SelectMany(x => x.FuelDeliveryConfigs)
            //    .SelectMany(x => x.).Count(y => y.DeleteDate == null);

            return fuelSystemControlType;
        }

        public override async Task<long> SubmitAddChangeRequestAsync(FuelSystemControlType entity, string requestedBy,
            List<ChangeRequestItemStaging> changeRequestItemStagings = null, CommentsStagingModel comment = null,
            List<AttachmentsModel> attachmentsStagingModels = null, string changeContent = null)
        {
            if (string.IsNullOrWhiteSpace(entity?.FuelSystemControlTypeName))
            {
                throw new ArgumentNullException(nameof(entity));
            }

            // validations
            await ValidateFuelSystemControlTypeIsNotDuplicate(entity, ChangeType.Add);
            await ValidateFuelSystemControlTypeHasNoChangeRequest(entity, ChangeType.Add);

            changeRequestItemStagings = changeRequestItemStagings ?? new List<ChangeRequestItemStaging>();
            changeRequestItemStagings.Add(new ChangeRequestItemStaging
            {
                EntityId = entity.Id.ToString(),
                Entity = typeof(FuelSystemControlType).Name,
                ChangeType = ChangeType.Add,
                CreatedDateTime = DateTime.UtcNow,
                Payload = Serializer.Serialize(entity)
            });

            changeContent = string.Format("{0}"
                , entity.FuelSystemControlTypeName);

            return await base.SubmitAddChangeRequestAsync(entity, requestedBy, changeRequestItemStagings, comment, attachmentsStagingModels, changeContent);
        }

        public override async Task<long> SubmitUpdateChangeRequestAsync<TId>(FuelSystemControlType entity, TId id, string requestedBy,
            ChangeType changeType = ChangeType.None,
            List<ChangeRequestItemStaging> changeRequestItemStagings = null, CommentsStagingModel comment = null, List<AttachmentsModel> attachments = null, string changeContent = null)
        {
            if (string.IsNullOrWhiteSpace(entity?.FuelSystemControlTypeName))
            {
                throw new ArgumentNullException(nameof(entity));
            }
            if (id.Equals(default(TId)))
            {
                throw new ArgumentNullException(nameof(id));
            }
            if (!entity.Id.Equals(id))
            {
                throw new ArgumentException("Invalid FuelSystemControlType Id");
            }

            var fuelSystemControlTypeFromDb = await FindAsync(id);

            if (fuelSystemControlTypeFromDb == null)
            {
                throw new NoRecordFound("Record Not Found");
            }

            // validations
            await ValidateFuelSystemControlTypeIsNotDuplicate(entity, ChangeType.Modify);
            await ValidateFuelSystemControlTypeHasNoChangeRequest(entity, ChangeType.Modify);
            await ValidateFuelSystemControlTypeLookUpHasNoChangeRequest(entity);

            // fill up audit information
            entity.InsertDate = fuelSystemControlTypeFromDb.InsertDate;
            entity.LastUpdateDate = fuelSystemControlTypeFromDb.LastUpdateDate;
            entity.DeleteDate = fuelSystemControlTypeFromDb.DeleteDate;

            changeRequestItemStagings = changeRequestItemStagings ?? new List<ChangeRequestItemStaging>();

            // to eliminate circular reference during serialize
            var existingEntity = new FuelSystemControlType()
            {
                Id = fuelSystemControlTypeFromDb.Id,
                FuelSystemControlTypeName = fuelSystemControlTypeFromDb.FuelSystemControlTypeName,
                ChangeRequestId = fuelSystemControlTypeFromDb.ChangeRequestId,
                FuelDeliveryConfigCount = fuelSystemControlTypeFromDb.FuelDeliveryConfigCount,
                FuelDeliveryConfigs = fuelSystemControlTypeFromDb.FuelDeliveryConfigs,
                DeleteDate = fuelSystemControlTypeFromDb.DeleteDate,
                InsertDate = fuelSystemControlTypeFromDb.InsertDate,
                LastUpdateDate = fuelSystemControlTypeFromDb.LastUpdateDate
            };

            changeRequestItemStagings.Add(new ChangeRequestItemStaging
            {
                EntityId = entity.Id.ToString(),
                Entity = typeof(FuelSystemControlType).Name,
                ChangeType = ChangeType.Modify,
                CreatedDateTime = DateTime.UtcNow,
                Payload = Serializer.Serialize(entity),
                ExistingPayload = base.Serializer.Serialize(existingEntity) // add existing record as ExistingPayload
            });

            changeContent = string.Format("{0} > {1}"
                , fuelSystemControlTypeFromDb.FuelSystemControlTypeName
                , entity.FuelSystemControlTypeName);

            var changeRequestId = await
                    base.SubmitUpdateChangeRequestAsync(entity, id, requestedBy, ChangeType.Modify,
                        changeRequestItemStagings, comment, attachments, changeContent);

            fuelSystemControlTypeFromDb.ChangeRequestId = changeRequestId;
            _fuelSystemControlTypeRepositoryService.Update(fuelSystemControlTypeFromDb);
            Repositories.SaveChanges();

            return changeRequestId;
        }

        public override async Task<long> SubmitDeleteChangeRequestAsync<TId>(FuelSystemControlType entity, TId id, string requestedBy,
            List<ChangeRequestItemStaging> changeRequestItemStagings = null, CommentsStagingModel comment = null, List<AttachmentsModel> attachmentsStaging = null, string changeContent = null)
        {
            if (id.Equals(default(TId)))
            {
                throw new ArgumentNullException(nameof(id));
            }

            var fuelSystemControlTypeFromDb = await FindAsync(id);

            if (fuelSystemControlTypeFromDb == null)
            {
                throw new NoRecordFound("Record Not Found");
            }

            // validations
            await ValidateFuelSystemControlTypeLookUpHasNoChangeRequest(entity);

            changeRequestItemStagings = changeRequestItemStagings ?? new List<ChangeRequestItemStaging>();
            changeRequestItemStagings.Add(new ChangeRequestItemStaging
            {
                EntityId = entity.Id.ToString(),
                Entity = typeof(FuelSystemControlType).Name,
                ChangeType = ChangeType.Delete,
                CreatedDateTime = DateTime.UtcNow,
                Payload = Serializer.Serialize(new FuelSystemControlType
                {
                    Id = fuelSystemControlTypeFromDb.Id,
                    FuelSystemControlTypeName = fuelSystemControlTypeFromDb.FuelSystemControlTypeName
                })
            });

            changeContent = string.Format("{0}"
                , entity.FuelSystemControlTypeName);

            var changeRequestId = await base.SubmitDeleteChangeRequestAsync(entity, id, requestedBy, changeRequestItemStagings, comment, attachmentsStaging, changeContent);

            fuelSystemControlTypeFromDb.ChangeRequestId = changeRequestId;
            _fuelSystemControlTypeRepositoryService.Update(fuelSystemControlTypeFromDb);
            Repositories.SaveChanges();

            return changeRequestId;
        }

        public override async Task<ChangeRequestStagingModel<FuelSystemControlType>> GetChangeRequestStaging<TId>(TId changeRequestId)
        {
            ChangeRequestStagingModel<FuelSystemControlType> staging = await
                this._vcdbChangeRequestBusinessService.GetChangeRequestStagingByChangeRequestIdAsync<FuelSystemControlType, TId>(changeRequestId);

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
                FuelSystemControlType deserializedEntry = Serializer.Deserialize<FuelSystemControlType>(payload);
                count.FuelDeliveryConfigCount = deserializedEntry.FuelDeliveryConfigCount;
            }
            return count;
        }

        public override async Task ClearChangeRequestId(long changeRequestId)
        {
            var entities = await _fuelSystemControlTypeRepositoryService.GetAsync(m => m.ChangeRequestId == changeRequestId && m.DeleteDate == null);

            if (entities != null && entities.Any())
            {
                var entity = entities.First();   //can only be a single item
                entity.ChangeRequestId = null;
                _fuelSystemControlTypeRepositoryService.Update(entity);
                Repositories.SaveChanges();
            }
        }

        private async Task ValidateFuelSystemControlTypeIsNotDuplicate(FuelSystemControlType fuelSystemControlType, ChangeType changeType)
        {
            // validate no duplicate FuelSystemControlType Name.
            if (changeType == ChangeType.Add)
            {
                IList<FuelSystemControlType> fuelSystemControlTypesFromDb =
                    await
                        _fuelSystemControlTypeRepositoryService.GetAsync(
                            x => x.FuelSystemControlTypeName.Trim().Equals(fuelSystemControlType.FuelSystemControlTypeName.Trim(), StringComparison.CurrentCultureIgnoreCase)
                                 && x.DeleteDate == null);
                if (fuelSystemControlTypesFromDb != null && fuelSystemControlTypesFromDb.Any())
                {
                    throw new RecordAlreadyExist($"There is aleady a FuelSystemControlType Name: {fuelSystemControlType.FuelSystemControlTypeName}.");
                }
            }
            else if (changeType == ChangeType.Modify) // allow whitespace and case correction
            {
                var fuelSystemControlTypeFromDb = await FindAsync(fuelSystemControlType.Id);

                if (fuelSystemControlType.FuelSystemControlTypeName.Trim().Equals(fuelSystemControlTypeFromDb.FuelSystemControlTypeName.Trim(), StringComparison.CurrentCulture))
                {
                    throw new RecordAlreadyExist($"There is aleady a fuelSystemControlType Name: {fuelSystemControlType.FuelSystemControlTypeName}.");
                }
                if (!fuelSystemControlType.FuelSystemControlTypeName.Trim().Equals(fuelSystemControlTypeFromDb.FuelSystemControlTypeName.Trim(), StringComparison.CurrentCultureIgnoreCase))
                {
                    IList<FuelSystemControlType> fuelSystemControlTypesFromDb =
                        await
                            _fuelSystemControlTypeRepositoryService.GetAsync(
                                x => x.FuelSystemControlTypeName.Trim().Equals(fuelSystemControlType.FuelSystemControlTypeName.Trim(), StringComparison.CurrentCultureIgnoreCase)
                                     && x.DeleteDate == null);
                    if (fuelSystemControlTypesFromDb != null && fuelSystemControlTypesFromDb.Any())
                    {
                        throw new RecordAlreadyExist($"There is aleady a fuelSystemControlType Name: {fuelSystemControlType.FuelSystemControlTypeName}.");
                    }
                }
            }
        }

        private async Task ValidateFuelSystemControlTypeHasNoChangeRequest(FuelSystemControlType fuelSystemControlType, ChangeType changeType)
        {
            // validate no CR for this FuelSystemControlType Id.
            if (changeType != ChangeType.Add)
            {

                var changerequestid =
                    (await ChangeRequestBusinessService.ChangeRequestExist<FuelSystemControlType>(item => item.Id == fuelSystemControlType.Id));

                if (changerequestid > 0)
                {
                    throw new ChangeRequestExistException($"There is already an open CR ID {changerequestid} with FuelSystemControlType ID : {fuelSystemControlType.Id}.");
                }
            }

            // validate no CR for this FuelSystemControlType Name.
            var changerequestid1 =
            await
                ChangeRequestBusinessService.ChangeRequestExist<FuelSystemControlType>(
                    x => x.FuelSystemControlTypeName.Trim().Equals(fuelSystemControlType.FuelSystemControlTypeName.Trim(), StringComparison.CurrentCultureIgnoreCase));

            if (changerequestid1 > 0)
            {
                throw new ChangeRequestExistException($"There is already an open CR ID {changerequestid1} with same FuelSystemControlType Name : {fuelSystemControlType.FuelSystemControlTypeName}.");
            }
        }

        private async Task ValidateFuelSystemControlTypeLookUpHasNoChangeRequest(FuelSystemControlType fuelSystemControlType)
        {
            // validate no CR for base vehicle using this FuelSystemControlType.
            var changerequestid =
                await
                    ChangeRequestBusinessService.ChangeRequestExist<FuelDeliveryConfig>(x => x.FuelSystemControlTypeId == fuelSystemControlType.Id);
            if (changerequestid > 0)
            {
                throw new ChangeRequestExistException($"There is already an open CR ID {changerequestid} for FuelDeliveryConfig using this FuelSystemControlType.");
            }
        }

        /// <summary>
        /// Find a single record which is not marked as deleted.
        /// </summary>
        /// <typeparam name="TKey">any</typeparam>
        /// <param name="id">the key property</param>
        /// <returns>Single entity object if condition matches, null if not found.</returns>
        private async Task<FuelSystemControlType> FindAsync<TKey>(TKey id)
        {
            var entityId = Convert.ToInt32(id);
            var entities = await _fuelSystemControlTypeRepositoryService.GetAsync(m => m.Id == entityId && m.DeleteDate == null, 1);
            if (entities != null && entities.Any())
            {
                return entities.First();
            }
            return null;
        }
    }
}