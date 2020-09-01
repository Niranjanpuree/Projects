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
    public class FuelSystemDesignBusinessService : VcdbBusinessService<FuelSystemDesign>, IFuelSystemDesignBusinessService
    {
        private readonly ISqlServerEfRepositoryService<FuelSystemDesign> _fuelSystemDesignRepositoryService;
        private readonly IVcdbChangeRequestBusinessService _vcdbChangeRequestBusinessService;

        public FuelSystemDesignBusinessService(IVcdbUnitOfWork vcdbRepositoryService,
            IVcdbChangeRequestBusinessService vcdbChangeRequestBusinessService,
            ITextSerializer serializer)
            : base(vcdbRepositoryService, vcdbChangeRequestBusinessService, serializer)
        {
            _fuelSystemDesignRepositoryService = Repositories.GetRepositoryService<FuelSystemDesign>() as ISqlServerEfRepositoryService<FuelSystemDesign>;
            _vcdbChangeRequestBusinessService = vcdbChangeRequestBusinessService;
        }

        public override async Task<List<FuelSystemDesign>> GetAllAsync(int topCount = 0)
        {
            return await _fuelSystemDesignRepositoryService.GetAsync(m => m.DeleteDate == null, topCount);
        }

        public override async Task<FuelSystemDesign> GetAsync<TKey>(TKey id)
        {
            // validations
            if (id.Equals(default(TKey)))
            {
                throw new ArgumentNullException(nameof(id));
            }

            var fuelSystemDesign = await FindAsync(id);

            if (fuelSystemDesign == null)
            {
                throw new NoRecordFound("Record Not Found");
            }

            fuelSystemDesign.FuelDeliveryConfigCount = await _fuelSystemDesignRepositoryService.GetCountAsync(fuelSystemDesign, x => x.FuelDeliveryConfigs, y => y.DeleteDate == null);

            var fuelSystemDesignId = Convert.ToInt32(id);
            //fuelSystemDesign.FuelDeliveryConfigCount = _fuelSystemDesignRepositoryService
            //    .GetAllQueryable()
            //    .Where(x => x.Id == fuelSystemDesignId).Include("FuelDeliveryConfigs.")
            //    .SelectMany(x => x.FuelDeliveryConfigs)
            //    .SelectMany(x => x.).Count(y => y.DeleteDate == null);

            return fuelSystemDesign;
        }

        public override async Task<long> SubmitAddChangeRequestAsync(FuelSystemDesign entity, string requestedBy,
            List<ChangeRequestItemStaging> changeRequestItemStagings = null, CommentsStagingModel comment = null,
            List<AttachmentsModel> attachmentsStagingModels = null, string changeContent = null)
        {
            if (string.IsNullOrWhiteSpace(entity?.FuelSystemDesignName))
            {
                throw new ArgumentNullException(nameof(entity));
            }

            // validations
            await ValidateFuelSystemDesignIsNotDuplicate(entity, ChangeType.Add);
            await ValidateFuelSystemDesignHasNoChangeRequest(entity, ChangeType.Add);

            changeRequestItemStagings = changeRequestItemStagings ?? new List<ChangeRequestItemStaging>();
            changeRequestItemStagings.Add(new ChangeRequestItemStaging
            {
                EntityId = entity.Id.ToString(),
                Entity = typeof(FuelSystemDesign).Name,
                ChangeType = ChangeType.Add,
                CreatedDateTime = DateTime.UtcNow,
                Payload = Serializer.Serialize(entity)
            });

            changeContent = string.Format("{0}"
                , entity.FuelSystemDesignName);

            return await base.SubmitAddChangeRequestAsync(entity, requestedBy, changeRequestItemStagings, comment, attachmentsStagingModels, changeContent);
        }

        public override async Task<long> SubmitUpdateChangeRequestAsync<TId>(FuelSystemDesign entity, TId id, string requestedBy,
            ChangeType changeType = ChangeType.None,
            List<ChangeRequestItemStaging> changeRequestItemStagings = null, CommentsStagingModel comment = null, List<AttachmentsModel> attachments = null, string changeContent = null)
        {
            if (string.IsNullOrWhiteSpace(entity?.FuelSystemDesignName))
            {
                throw new ArgumentNullException(nameof(entity));
            }
            if (id.Equals(default(TId)))
            {
                throw new ArgumentNullException(nameof(id));
            }
            if (!entity.Id.Equals(id))
            {
                throw new ArgumentException("Invalid FuelSystemDesign Id");
            }

            var fuelSystemDesignFromDb = await FindAsync(id);

            if (fuelSystemDesignFromDb == null)
            {
                throw new NoRecordFound("Record Not Found");
            }

            // validations
            await ValidateFuelSystemDesignIsNotDuplicate(entity, ChangeType.Modify);
            await ValidateFuelSystemDesignHasNoChangeRequest(entity, ChangeType.Modify);
            await ValidateFuelSystemDesignLookUpHasNoChangeRequest(entity);

            // fill up audit information
            entity.InsertDate = fuelSystemDesignFromDb.InsertDate;
            entity.LastUpdateDate = fuelSystemDesignFromDb.LastUpdateDate;
            entity.DeleteDate = fuelSystemDesignFromDb.DeleteDate;

            changeRequestItemStagings = changeRequestItemStagings ?? new List<ChangeRequestItemStaging>();

            // to eliminate circular reference during serialize
            var existingEntity = new FuelSystemDesign()
            {
                Id = fuelSystemDesignFromDb.Id,
                FuelSystemDesignName = fuelSystemDesignFromDb.FuelSystemDesignName,
                ChangeRequestId = fuelSystemDesignFromDb.ChangeRequestId,
                FuelDeliveryConfigCount = fuelSystemDesignFromDb.FuelDeliveryConfigCount,
                FuelDeliveryConfigs = fuelSystemDesignFromDb.FuelDeliveryConfigs,
                DeleteDate = fuelSystemDesignFromDb.DeleteDate,
                InsertDate = fuelSystemDesignFromDb.InsertDate,
                LastUpdateDate = fuelSystemDesignFromDb.LastUpdateDate
            };

            changeRequestItemStagings.Add(new ChangeRequestItemStaging
            {
                EntityId = entity.Id.ToString(),
                Entity = typeof(FuelSystemDesign).Name,
                ChangeType = ChangeType.Modify,
                CreatedDateTime = DateTime.UtcNow,
                Payload = Serializer.Serialize(entity),
                ExistingPayload = base.Serializer.Serialize(existingEntity) // add existing record as ExistingPayload
            });

            changeContent = string.Format("{0} > {1}"
                , fuelSystemDesignFromDb.FuelSystemDesignName
                , entity.FuelSystemDesignName);

            var changeRequestId = await
                    base.SubmitUpdateChangeRequestAsync(entity, id, requestedBy, ChangeType.Modify,
                        changeRequestItemStagings, comment, attachments, changeContent);

            fuelSystemDesignFromDb.ChangeRequestId = changeRequestId;
            _fuelSystemDesignRepositoryService.Update(fuelSystemDesignFromDb);
            Repositories.SaveChanges();

            return changeRequestId;
        }

        public override async Task<long> SubmitDeleteChangeRequestAsync<TId>(FuelSystemDesign entity, TId id, string requestedBy,
            List<ChangeRequestItemStaging> changeRequestItemStagings = null, CommentsStagingModel comment = null, List<AttachmentsModel> attachmentsStaging = null, string changeContent = null)
        {
            if (id.Equals(default(TId)))
            {
                throw new ArgumentNullException(nameof(id));
            }

            var fuelSystemDesignFromDb = await FindAsync(id);

            if (fuelSystemDesignFromDb == null)
            {
                throw new NoRecordFound("Record Not Found");
            }

            // validations
            await ValidateFuelSystemDesignLookUpHasNoChangeRequest(entity);

            changeRequestItemStagings = changeRequestItemStagings ?? new List<ChangeRequestItemStaging>();
            changeRequestItemStagings.Add(new ChangeRequestItemStaging
            {
                EntityId = entity.Id.ToString(),
                Entity = typeof(FuelSystemDesign).Name,
                ChangeType = ChangeType.Delete,
                CreatedDateTime = DateTime.UtcNow,
                Payload = Serializer.Serialize(new FuelSystemDesign
                {
                    Id = fuelSystemDesignFromDb.Id,
                    FuelSystemDesignName = fuelSystemDesignFromDb.FuelSystemDesignName
                })
            });

            changeContent = string.Format("{0}"
                , entity.FuelSystemDesignName);

            var changeRequestId = await base.SubmitDeleteChangeRequestAsync(entity, id, requestedBy, changeRequestItemStagings, comment, attachmentsStaging, changeContent);

            fuelSystemDesignFromDb.ChangeRequestId = changeRequestId;
            _fuelSystemDesignRepositoryService.Update(fuelSystemDesignFromDb);
            Repositories.SaveChanges();

            return changeRequestId;
        }

        public override async Task<ChangeRequestStagingModel<FuelSystemDesign>> GetChangeRequestStaging<TId>(TId changeRequestId)
        {
            ChangeRequestStagingModel<FuelSystemDesign> staging = await
                this._vcdbChangeRequestBusinessService.GetChangeRequestStagingByChangeRequestIdAsync<FuelSystemDesign, TId>(changeRequestId);

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
                FuelSystemDesign deserializedEntry = Serializer.Deserialize<FuelSystemDesign>(payload);
                count.FuelDeliveryConfigCount = deserializedEntry.FuelDeliveryConfigCount;
            }
            return count;
        }

        public override async Task ClearChangeRequestId(long changeRequestId)
        {
            var entities = await _fuelSystemDesignRepositoryService.GetAsync(m => m.ChangeRequestId == changeRequestId && m.DeleteDate == null);

            if (entities != null && entities.Any())
            {
                var entity = entities.First();   //can only be a single item
                entity.ChangeRequestId = null;
                _fuelSystemDesignRepositoryService.Update(entity);
                Repositories.SaveChanges();
            }
        }

        private async Task ValidateFuelSystemDesignIsNotDuplicate(FuelSystemDesign fuelSystemDesign, ChangeType changeType)
        {
            // validate no duplicate FuelSystemDesign Name.
            if (changeType == ChangeType.Add)
            {
                IList<FuelSystemDesign> fuelSystemDesignsFromDb =
                    await
                        _fuelSystemDesignRepositoryService.GetAsync(
                            x => x.FuelSystemDesignName.Trim().Equals(fuelSystemDesign.FuelSystemDesignName.Trim(), StringComparison.CurrentCultureIgnoreCase)
                                 && x.DeleteDate == null);
                if (fuelSystemDesignsFromDb != null && fuelSystemDesignsFromDb.Any())
                {
                    throw new RecordAlreadyExist($"There is aleady a FuelSystemDesign Name: {fuelSystemDesign.FuelSystemDesignName}.");
                }
            }
            else if (changeType == ChangeType.Modify) // allow whitespace and case correction
            {
                var fuelSystemDesignFromDb = await FindAsync(fuelSystemDesign.Id);

                if (fuelSystemDesign.FuelSystemDesignName.Trim().Equals(fuelSystemDesignFromDb.FuelSystemDesignName.Trim(), StringComparison.CurrentCulture))
                {
                    throw new RecordAlreadyExist($"There is aleady a fuelSystemDesign Name: {fuelSystemDesign.FuelSystemDesignName}.");
                }
                if (!fuelSystemDesign.FuelSystemDesignName.Trim().Equals(fuelSystemDesignFromDb.FuelSystemDesignName.Trim(), StringComparison.CurrentCultureIgnoreCase))
                {
                    IList<FuelSystemDesign> fuelSystemDesignsFromDb =
                        await
                            _fuelSystemDesignRepositoryService.GetAsync(
                                x => x.FuelSystemDesignName.Trim().Equals(fuelSystemDesign.FuelSystemDesignName.Trim(), StringComparison.CurrentCultureIgnoreCase)
                                     && x.DeleteDate == null);
                    if (fuelSystemDesignsFromDb != null && fuelSystemDesignsFromDb.Any())
                    {
                        throw new RecordAlreadyExist($"There is aleady a fuelSystemDesign Name: {fuelSystemDesign.FuelSystemDesignName}.");
                    }
                }
            }
        }

        private async Task ValidateFuelSystemDesignHasNoChangeRequest(FuelSystemDesign fuelSystemDesign, ChangeType changeType)
        {
            // validate no CR for this FuelSystemDesign Id.
            if (changeType != ChangeType.Add)
            {

                var changerequestid =
                    (await ChangeRequestBusinessService.ChangeRequestExist<FuelSystemDesign>(item => item.Id == fuelSystemDesign.Id));

                if (changerequestid > 0)
                {
                    throw new ChangeRequestExistException($"There is already an open CR ID {changerequestid} with FuelSystemDesign ID : {fuelSystemDesign.Id}.");
                }
            }

            // validate no CR for this FuelSystemDesign Name.
            var changerequestid1 =
            await
                ChangeRequestBusinessService.ChangeRequestExist<FuelSystemDesign>(
                    x => x.FuelSystemDesignName.Trim().Equals(fuelSystemDesign.FuelSystemDesignName.Trim(), StringComparison.CurrentCultureIgnoreCase));

            if (changerequestid1 > 0)
            {
                throw new ChangeRequestExistException($"There is already an open CR ID {changerequestid1} with same FuelSystemDesign Name : {fuelSystemDesign.FuelSystemDesignName}.");
            }
        }

        private async Task ValidateFuelSystemDesignLookUpHasNoChangeRequest(FuelSystemDesign fuelSystemDesign)
        {
            // validate no CR for base vehicle using this FuelSystemDesign.
            var changerequestid =
                await
                    ChangeRequestBusinessService.ChangeRequestExist<FuelDeliveryConfig>(x => x.FuelSystemDesignId == fuelSystemDesign.Id);
            if (changerequestid > 0)
            {
                throw new ChangeRequestExistException($"There is already an open CR ID {changerequestid} for FuelDeliveryConfig using this FuelSystemDesign.");
            }
        }

        /// <summary>
        /// Find a single record which is not marked as deleted.
        /// </summary>
        /// <typeparam name="TKey">any</typeparam>
        /// <param name="id">the key property</param>
        /// <returns>Single entity object if condition matches, null if not found.</returns>
        private async Task<FuelSystemDesign> FindAsync<TKey>(TKey id)
        {
            var entityId = Convert.ToInt32(id);
            var entities = await _fuelSystemDesignRepositoryService.GetAsync(m => m.Id == entityId && m.DeleteDate == null, 1);
            if (entities != null && entities.Any())
            {
                return entities.First();
            }
            return null;
        }
    }
}