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
    public class FuelDeliverySubTypeBusinessService : VcdbBusinessService<FuelDeliverySubType>, IFuelDeliverySubTypeBusinessService
    {
        private readonly ISqlServerEfRepositoryService<FuelDeliverySubType> _fuelDeliverySubTypeRepositoryService;
        private readonly IVcdbChangeRequestBusinessService _vcdbChangeRequestBusinessService;

        public FuelDeliverySubTypeBusinessService(IVcdbUnitOfWork vcdbRepositoryService,
            IVcdbChangeRequestBusinessService vcdbChangeRequestBusinessService,
            ITextSerializer serializer)
            : base(vcdbRepositoryService, vcdbChangeRequestBusinessService, serializer)
        {
            _fuelDeliverySubTypeRepositoryService = Repositories.GetRepositoryService<FuelDeliverySubType>() as ISqlServerEfRepositoryService<FuelDeliverySubType>;
            _vcdbChangeRequestBusinessService = vcdbChangeRequestBusinessService;
        }

        public override async Task<List<FuelDeliverySubType>> GetAllAsync(int topCount = 0)
        {
            return await _fuelDeliverySubTypeRepositoryService.GetAsync(m => m.DeleteDate == null, topCount);
        }

        public override async Task<FuelDeliverySubType> GetAsync<TKey>(TKey id)
        {
            // validations
            if (id.Equals(default(TKey)))
            {
                throw new ArgumentNullException(nameof(id));
            }

            var fuelDeliverySubType = await FindAsync(id);

            if (fuelDeliverySubType == null)
            {
                throw new NoRecordFound("Record Not Found");
            }

            fuelDeliverySubType.FuelDeliveryConfigCount = await _fuelDeliverySubTypeRepositoryService.GetCountAsync(fuelDeliverySubType, x => x.FuelDeliveryConfigs, y => y.DeleteDate == null);

            var fuelDeliverySubTypeId = Convert.ToInt32(id);
            //fuelDeliverySubType.FuelDeliveryConfigCount = _fuelDeliverySubTypeRepositoryService
            //    .GetAllQueryable()
            //    .Where(x => x.Id == fuelDeliverySubTypeId).Include("FuelDeliveryConfigs.")
            //    .SelectMany(x => x.FuelDeliveryConfigs)
            //    .SelectMany(x => x.).Count(y => y.DeleteDate == null);

            return fuelDeliverySubType;
        }

        public override async Task<long> SubmitAddChangeRequestAsync(FuelDeliverySubType entity, string requestedBy,
            List<ChangeRequestItemStaging> changeRequestItemStagings = null, CommentsStagingModel comment = null,
            List<AttachmentsModel> attachmentsStagingModels = null, string changeContent = null)
        {
            if (string.IsNullOrWhiteSpace(entity?.FuelDeliverySubTypeName))
            {
                throw new ArgumentNullException(nameof(entity));
            }

            // validations
            await ValidateFuelDeliverySubTypeIsNotDuplicate(entity, ChangeType.Add);
            await ValidateFuelDeliverySubTypeHasNoChangeRequest(entity, ChangeType.Add);

            changeRequestItemStagings = changeRequestItemStagings ?? new List<ChangeRequestItemStaging>();
            changeRequestItemStagings.Add(new ChangeRequestItemStaging
            {
                EntityId = entity.Id.ToString(),
                Entity = typeof(FuelDeliverySubType).Name,
                ChangeType = ChangeType.Add,
                CreatedDateTime = DateTime.UtcNow,
                Payload = Serializer.Serialize(entity)
            });

            changeContent = string.Format("{0}"
                , entity.FuelDeliverySubTypeName);

            return await base.SubmitAddChangeRequestAsync(entity, requestedBy, changeRequestItemStagings, comment, attachmentsStagingModels, changeContent);
        }

        public override async Task<long> SubmitUpdateChangeRequestAsync<TId>(FuelDeliverySubType entity, TId id, string requestedBy,
            ChangeType changeType = ChangeType.None,
            List<ChangeRequestItemStaging> changeRequestItemStagings = null, CommentsStagingModel comment = null, List<AttachmentsModel> attachments = null, string changeContent = null)
        {
            if (string.IsNullOrWhiteSpace(entity?.FuelDeliverySubTypeName))
            {
                throw new ArgumentNullException(nameof(entity));
            }
            if (id.Equals(default(TId)))
            {
                throw new ArgumentNullException(nameof(id));
            }
            if (!entity.Id.Equals(id))
            {
                throw new ArgumentException("Invalid FuelDeliverySubType Id");
            }

            var fuelDeliverySubTypeFromDb = await FindAsync(id);

            if (fuelDeliverySubTypeFromDb == null)
            {
                throw new NoRecordFound("Record Not Found");
            }

            // validations
            await ValidateFuelDeliverySubTypeIsNotDuplicate(entity, ChangeType.Modify);
            await ValidateFuelDeliverySubTypeHasNoChangeRequest(entity, ChangeType.Modify);
            await ValidateFuelDeliverySubTypeLookUpHasNoChangeRequest(entity);

            // fill up audit information
            entity.InsertDate = fuelDeliverySubTypeFromDb.InsertDate;
            entity.LastUpdateDate = fuelDeliverySubTypeFromDb.LastUpdateDate;
            entity.DeleteDate = fuelDeliverySubTypeFromDb.DeleteDate;

            changeRequestItemStagings = changeRequestItemStagings ?? new List<ChangeRequestItemStaging>();

            // to eliminate circular reference during serialize
            var existingEntity = new FuelDeliverySubType()
            {
                Id = fuelDeliverySubTypeFromDb.Id,
                FuelDeliverySubTypeName = fuelDeliverySubTypeFromDb.FuelDeliverySubTypeName,
                ChangeRequestId = fuelDeliverySubTypeFromDb.ChangeRequestId,
                FuelDeliveryConfigCount= fuelDeliverySubTypeFromDb.FuelDeliveryConfigCount,
                FuelDeliveryConfigs=fuelDeliverySubTypeFromDb.FuelDeliveryConfigs,
                DeleteDate = fuelDeliverySubTypeFromDb.DeleteDate,
                InsertDate = fuelDeliverySubTypeFromDb.InsertDate,
                LastUpdateDate = fuelDeliverySubTypeFromDb.LastUpdateDate
            };

            changeRequestItemStagings.Add(new ChangeRequestItemStaging
            {
                EntityId = entity.Id.ToString(),
                Entity = typeof(FuelDeliverySubType).Name,
                ChangeType = ChangeType.Modify,
                CreatedDateTime = DateTime.UtcNow,
                Payload = Serializer.Serialize(entity),
                ExistingPayload = base.Serializer.Serialize(existingEntity) // add existing record as ExistingPayload
            });

            changeContent = string.Format("{0} > {1}"
                , fuelDeliverySubTypeFromDb.FuelDeliverySubTypeName
                , entity.FuelDeliverySubTypeName);

            var changeRequestId = await
                    base.SubmitUpdateChangeRequestAsync(entity, id, requestedBy, ChangeType.Modify,
                        changeRequestItemStagings, comment, attachments, changeContent);

            fuelDeliverySubTypeFromDb.ChangeRequestId = changeRequestId;
            _fuelDeliverySubTypeRepositoryService.Update(fuelDeliverySubTypeFromDb);
            Repositories.SaveChanges();

            return changeRequestId;
        }

        public override async Task<long> SubmitDeleteChangeRequestAsync<TId>(FuelDeliverySubType entity, TId id, string requestedBy,
            List<ChangeRequestItemStaging> changeRequestItemStagings = null, CommentsStagingModel comment = null, List<AttachmentsModel> attachmentsStaging = null, string changeContent = null)
        {
            if (id.Equals(default(TId)))
            {
                throw new ArgumentNullException(nameof(id));
            }

            var fuelDeliverySubTypeFromDb = await FindAsync(id);

            if (fuelDeliverySubTypeFromDb == null)
            {
                throw new NoRecordFound("Record Not Found");
            }

            // validations
            await ValidateFuelDeliverySubTypeLookUpHasNoChangeRequest(entity);

            changeRequestItemStagings = changeRequestItemStagings ?? new List<ChangeRequestItemStaging>();
            changeRequestItemStagings.Add(new ChangeRequestItemStaging
            {
                EntityId = entity.Id.ToString(),
                Entity = typeof(FuelDeliverySubType).Name,
                ChangeType = ChangeType.Delete,
                CreatedDateTime = DateTime.UtcNow,
                Payload = Serializer.Serialize(new FuelDeliverySubType
                {
                    Id = fuelDeliverySubTypeFromDb.Id,
                    FuelDeliverySubTypeName = fuelDeliverySubTypeFromDb.FuelDeliverySubTypeName
                })
            });

            changeContent = string.Format("{0}"
                , entity.FuelDeliverySubTypeName);

            var changeRequestId = await base.SubmitDeleteChangeRequestAsync(entity, id, requestedBy, changeRequestItemStagings, comment, attachmentsStaging, changeContent);

            fuelDeliverySubTypeFromDb.ChangeRequestId = changeRequestId;
            _fuelDeliverySubTypeRepositoryService.Update(fuelDeliverySubTypeFromDb);
            Repositories.SaveChanges();

            return changeRequestId;
        }

        public override async Task<ChangeRequestStagingModel<FuelDeliverySubType>> GetChangeRequestStaging<TId>(TId changeRequestId)
        {
            ChangeRequestStagingModel<FuelDeliverySubType> staging = await
                this._vcdbChangeRequestBusinessService.GetChangeRequestStagingByChangeRequestIdAsync<FuelDeliverySubType, TId>(changeRequestId);

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
                FuelDeliverySubType deserializedEntry = Serializer.Deserialize<FuelDeliverySubType>(payload);
                count.FuelDeliveryConfigCount = deserializedEntry.FuelDeliveryConfigCount;
            }
            return count;
        }

        public override async Task ClearChangeRequestId(long changeRequestId)
        {
            var entities = await _fuelDeliverySubTypeRepositoryService.GetAsync(m => m.ChangeRequestId == changeRequestId && m.DeleteDate == null);

            if (entities != null && entities.Any())
            {
                var entity = entities.First();   //can only be a single item
                entity.ChangeRequestId = null;
                _fuelDeliverySubTypeRepositoryService.Update(entity);
                Repositories.SaveChanges();
            }
        }

        private async Task ValidateFuelDeliverySubTypeIsNotDuplicate(FuelDeliverySubType fuelDeliverySubType, ChangeType changeType)
        {
            // validate no duplicate FuelDeliverySubType Name.
            if (changeType == ChangeType.Add)
            {
                IList<FuelDeliverySubType> fuelDeliverySubTypesFromDb =
                    await
                        _fuelDeliverySubTypeRepositoryService.GetAsync(
                            x => x.FuelDeliverySubTypeName.Trim().Equals(fuelDeliverySubType.FuelDeliverySubTypeName.Trim(), StringComparison.CurrentCultureIgnoreCase)
                                 && x.DeleteDate == null);
                if (fuelDeliverySubTypesFromDb != null && fuelDeliverySubTypesFromDb.Any())
                {
                    throw new RecordAlreadyExist($"There is aleady a FuelDeliverySubType Name: {fuelDeliverySubType.FuelDeliverySubTypeName}.");
                }
            }
            else if (changeType == ChangeType.Modify) // allow whitespace and case correction
            {
                var fuelDeliverySubTypeFromDb = await FindAsync(fuelDeliverySubType.Id);

                if (fuelDeliverySubType.FuelDeliverySubTypeName.Trim().Equals(fuelDeliverySubTypeFromDb.FuelDeliverySubTypeName.Trim(), StringComparison.CurrentCulture))
                {
                    throw new RecordAlreadyExist($"There is aleady a fuelDeliverySubType Name: {fuelDeliverySubType.FuelDeliverySubTypeName}.");
                }
                if (!fuelDeliverySubType.FuelDeliverySubTypeName.Trim().Equals(fuelDeliverySubTypeFromDb.FuelDeliverySubTypeName.Trim(), StringComparison.CurrentCultureIgnoreCase))
                {
                    IList<FuelDeliverySubType> fuelDeliverySubTypesFromDb =
                        await
                            _fuelDeliverySubTypeRepositoryService.GetAsync(
                                x => x.FuelDeliverySubTypeName.Trim().Equals(fuelDeliverySubType.FuelDeliverySubTypeName.Trim(), StringComparison.CurrentCultureIgnoreCase)
                                     && x.DeleteDate == null);
                    if (fuelDeliverySubTypesFromDb != null && fuelDeliverySubTypesFromDb.Any())
                    {
                        throw new RecordAlreadyExist($"There is aleady a fuelDeliverySubType Name: {fuelDeliverySubType.FuelDeliverySubTypeName}.");
                    }
                }
            }
        }

        private async Task ValidateFuelDeliverySubTypeHasNoChangeRequest(FuelDeliverySubType fuelDeliverySubType, ChangeType changeType)
        {
            // validate no CR for this FuelDeliverySubType Id.
            if (changeType != ChangeType.Add)
            {

                var changerequestid =
                    (await ChangeRequestBusinessService.ChangeRequestExist<FuelDeliverySubType>(item => item.Id == fuelDeliverySubType.Id));

                if(changerequestid>0)
                {
                    throw new ChangeRequestExistException($"There is already an open CR ID {changerequestid} with FuelDeliverySubType ID : {fuelDeliverySubType.Id}.");
                }
            }

            // validate no CR for this FuelDeliverySubType Name.
            var changerequestid1 =
            await
                ChangeRequestBusinessService.ChangeRequestExist<FuelDeliverySubType>(
                    x => x.FuelDeliverySubTypeName.Trim().Equals(fuelDeliverySubType.FuelDeliverySubTypeName.Trim(), StringComparison.CurrentCultureIgnoreCase));

            if(changerequestid1>0){
                throw new ChangeRequestExistException($"There is already an open CR ID {changerequestid1} with same FuelDeliverySubType Name : {fuelDeliverySubType.FuelDeliverySubTypeName}.");
            }
        }

        private async Task ValidateFuelDeliverySubTypeLookUpHasNoChangeRequest(FuelDeliverySubType fuelDeliverySubType)
        {
            // validate no CR for base vehicle using this FuelDeliverySubType.
            var changerequestid =
                await
                    ChangeRequestBusinessService.ChangeRequestExist < FuelDeliveryConfig>(x => x.FuelDeliverySubTypeId == fuelDeliverySubType.Id);
            if(changerequestid > 0)
            {
                throw new ChangeRequestExistException($"There is already an open CR ID {changerequestid} for FuelDeliveryConfig using this FuelDeliverySubType.");
            }
        }

        /// <summary>
        /// Find a single record which is not marked as deleted.
        /// </summary>
        /// <typeparam name="TKey">any</typeparam>
        /// <param name="id">the key property</param>
        /// <returns>Single entity object if condition matches, null if not found.</returns>
        private async Task<FuelDeliverySubType> FindAsync<TKey>(TKey id)
        {
            var entityId = Convert.ToInt32(id);
            var entities = await _fuelDeliverySubTypeRepositoryService.GetAsync(m => m.Id == entityId && m.DeleteDate == null, 1);
            if (entities != null && entities.Any())
            {
                return entities.First();
            }
            return null;
        }
    }
}