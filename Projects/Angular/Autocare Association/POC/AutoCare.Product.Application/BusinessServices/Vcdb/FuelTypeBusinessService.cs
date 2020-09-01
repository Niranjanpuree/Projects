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
    public class FuelTypeBusinessService : VcdbBusinessService<FuelType>, IFuelTypeBusinessService
    {
        private readonly ISqlServerEfRepositoryService<FuelType> _fuelTypeRepositoryService;
        private readonly IVcdbChangeRequestBusinessService _vcdbChangeRequestBusinessService;

        public FuelTypeBusinessService(IVcdbUnitOfWork vcdbRepositoryService,
            IVcdbChangeRequestBusinessService vcdbChangeRequestBusinessService,
            ITextSerializer serializer)
            : base(vcdbRepositoryService, vcdbChangeRequestBusinessService, serializer)
        {
            _fuelTypeRepositoryService = Repositories.GetRepositoryService<FuelType>() as ISqlServerEfRepositoryService<FuelType>;
            _vcdbChangeRequestBusinessService = vcdbChangeRequestBusinessService;
        }

        public override async Task<List<FuelType>> GetAllAsync(int topCount = 0)
        {
            return await _fuelTypeRepositoryService.GetAsync(m => m.DeleteDate == null, topCount);
        }

        public override async Task<FuelType> GetAsync<TKey>(TKey id)
        {
            // validations
            if (id.Equals(default(TKey)))
            {
                throw new ArgumentNullException(nameof(id));
            }

            var fuelType = await FindAsync(id);

            if (fuelType == null)
            {
                throw new NoRecordFound("Record Not Found");
            }

           

            var fuelTypeId = Convert.ToInt32(id);
            fuelType.EngineConfigCount = await _fuelTypeRepositoryService.GetCountAsync(fuelType, x => x.EngineConfigs,
                y => y.DeleteDate == null);

            var engineDesignationId = Convert.ToInt32(id);
            fuelType.VehicleToEngineConfigCount = _fuelTypeRepositoryService
                .GetAllQueryable()
                .Where(x => x.Id == engineDesignationId)
                .Include("EngineConfigs.VehicleToEngineConfigs")
                .SelectMany(x => x.EngineConfigs)
                .SelectMany(x => x.VehicleToEngineConfigs).Count(y => y.DeleteDate == null);
          

            return fuelType;
        }

        public override async Task<long> SubmitAddChangeRequestAsync(FuelType entity, string requestedBy,
            List<ChangeRequestItemStaging> changeRequestItemStagings = null, CommentsStagingModel comment = null,
            List<AttachmentsModel> attachmentsStagingModels = null, string changeContent = null)
        {
            if (string.IsNullOrWhiteSpace(entity?.FuelTypeName))
            {
                throw new ArgumentNullException(nameof(entity));
            }

            // validations
            await ValidateFuelTypeIsNotDuplicate(entity, ChangeType.Add);
            await ValidateFuelTypeHasNoChangeRequest(entity, ChangeType.Add);

            changeRequestItemStagings = changeRequestItemStagings ?? new List<ChangeRequestItemStaging>();
            changeRequestItemStagings.Add(new ChangeRequestItemStaging
            {
                EntityId = entity.Id.ToString(),
                Entity = typeof(FuelType).Name,
                ChangeType = ChangeType.Add,
                CreatedDateTime = DateTime.UtcNow,
                Payload = Serializer.Serialize(entity)
            });

            changeContent = string.Format("{0}"
                , entity.FuelTypeName);

            return await base.SubmitAddChangeRequestAsync(entity, requestedBy, changeRequestItemStagings, comment, attachmentsStagingModels, changeContent);
        }

        public override async Task<long> SubmitUpdateChangeRequestAsync<TId>(FuelType entity, TId id, string requestedBy,
            ChangeType changeType = ChangeType.None,
            List<ChangeRequestItemStaging> changeRequestItemStagings = null, CommentsStagingModel comment = null, List<AttachmentsModel> attachments = null, string changeContent = null)
        {
            if (string.IsNullOrWhiteSpace(entity?.FuelTypeName))
            {
                throw new ArgumentNullException(nameof(entity));
            }
            if (id.Equals(default(TId)))
            {
                throw new ArgumentNullException(nameof(id));
            }
            if (!entity.Id.Equals(id))
            {
                throw new ArgumentException("Invalid FuelType Id");
            }

            var fuelTypeFromDb = await FindAsync(id);

            if (fuelTypeFromDb == null)
            {
                throw new NoRecordFound("Record Not Found");
            }

            // validations
            await ValidateFuelTypeIsNotDuplicate(entity, ChangeType.Modify);
            await ValidateFuelTypeHasNoChangeRequest(entity, ChangeType.Modify);
           

            // fill up audit information
            entity.InsertDate = fuelTypeFromDb.InsertDate;
            entity.LastUpdateDate = fuelTypeFromDb.LastUpdateDate;
            entity.DeleteDate = fuelTypeFromDb.DeleteDate;

            changeRequestItemStagings = changeRequestItemStagings ?? new List<ChangeRequestItemStaging>();

            // to eliminate circular reference during serialize
            var existingEntity = new FuelType()
            {
                Id = fuelTypeFromDb.Id,
                FuelTypeName = fuelTypeFromDb.FuelTypeName,
                ChangeRequestId = fuelTypeFromDb.ChangeRequestId,
                DeleteDate = fuelTypeFromDb.DeleteDate,
                InsertDate = fuelTypeFromDb.InsertDate,
                LastUpdateDate = fuelTypeFromDb.LastUpdateDate
            };

            changeRequestItemStagings.Add(new ChangeRequestItemStaging
            {
                EntityId = entity.Id.ToString(),
                Entity = typeof(FuelType).Name,
                ChangeType = ChangeType.Modify,
                CreatedDateTime = DateTime.UtcNow,
                Payload = Serializer.Serialize(entity),
                ExistingPayload = base.Serializer.Serialize(existingEntity) // add existing record as ExistingPayload
            });

            changeContent = string.Format("{0} > {1}"
                , fuelTypeFromDb.FuelTypeName
                , entity.FuelTypeName);

            var changeRequestId = await
                    base.SubmitUpdateChangeRequestAsync(entity, id, requestedBy, ChangeType.Modify,
                        changeRequestItemStagings, comment, attachments, changeContent);

            fuelTypeFromDb.ChangeRequestId = changeRequestId;
            _fuelTypeRepositoryService.Update(fuelTypeFromDb);
            Repositories.SaveChanges();

            return changeRequestId;
        }

        public override async Task<long> SubmitDeleteChangeRequestAsync<TId>(FuelType entity, TId id, string requestedBy,
            List<ChangeRequestItemStaging> changeRequestItemStagings = null, CommentsStagingModel comment = null, List<AttachmentsModel> attachmentsStaging = null, string changeContent = null)
        {
            if (id.Equals(default(TId)))
            {
                throw new ArgumentNullException(nameof(id));
            }

            var fuelTypeFromDb = await FindAsync(id);

            if (fuelTypeFromDb == null)
            {
                throw new NoRecordFound("Record Not Found");
            }

           

            changeRequestItemStagings = changeRequestItemStagings ?? new List<ChangeRequestItemStaging>();
            changeRequestItemStagings.Add(new ChangeRequestItemStaging
            {
                EntityId = entity.Id.ToString(),
                Entity = typeof(FuelType).Name,
                ChangeType = ChangeType.Delete,
                CreatedDateTime = DateTime.UtcNow,
                Payload = Serializer.Serialize(new FuelType
                {
                    Id = fuelTypeFromDb.Id,
                    FuelTypeName = fuelTypeFromDb.FuelTypeName
                })
            });

            changeContent = string.Format("{0}"
                , entity.FuelTypeName);

            var changeRequestId = await base.SubmitDeleteChangeRequestAsync(entity, id, requestedBy, changeRequestItemStagings, comment, attachmentsStaging, changeContent);

            fuelTypeFromDb.ChangeRequestId = changeRequestId;
            _fuelTypeRepositoryService.Update(fuelTypeFromDb);
            Repositories.SaveChanges();

            return changeRequestId;
        }

        public override async Task<ChangeRequestStagingModel<FuelType>> GetChangeRequestStaging<TId>(TId changeRequestId)
        {
            ChangeRequestStagingModel<FuelType> staging = await
                this._vcdbChangeRequestBusinessService.GetChangeRequestStagingByChangeRequestIdAsync<FuelType, TId>(changeRequestId);

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
                FuelType deserializedEntry = Serializer.Deserialize<FuelType>(payload);
               
            }
            return count;
        }

        public override async Task ClearChangeRequestId(long changeRequestId)
        {
            var entities = await _fuelTypeRepositoryService.GetAsync(m => m.ChangeRequestId == changeRequestId && m.DeleteDate == null);

            if (entities != null && entities.Any())
            {
                var entity = entities.First();   //can only be a single item
                entity.ChangeRequestId = null;
                _fuelTypeRepositoryService.Update(entity);
                Repositories.SaveChanges();
            }
        }

        private async Task ValidateFuelTypeIsNotDuplicate(FuelType fuelType, ChangeType changeType)
        {
            // validate no duplicate FuelType Name.
            if (changeType == ChangeType.Add)
            {
                IList<FuelType> fuelTypesFromDb =
                    await
                        _fuelTypeRepositoryService.GetAsync(
                            x => x.FuelTypeName.Trim().Equals(fuelType.FuelTypeName.Trim(), StringComparison.CurrentCultureIgnoreCase)
                                 && x.DeleteDate == null);
                if (fuelTypesFromDb != null && fuelTypesFromDb.Any())
                {
                    throw new RecordAlreadyExist($"There is aleady a FuelType Name: {fuelType.FuelTypeName}.");
                }
            }
            else if (changeType == ChangeType.Modify) // allow whitespace and case correction
            {
                var fuelTypeFromDb = await FindAsync(fuelType.Id);

                if (fuelType.FuelTypeName.Trim().Equals(fuelTypeFromDb.FuelTypeName.Trim(), StringComparison.CurrentCulture))
                {
                    throw new RecordAlreadyExist($"There is aleady a fuelType Name: {fuelType.FuelTypeName}.");
                }
                if (!fuelType.FuelTypeName.Trim().Equals(fuelTypeFromDb.FuelTypeName.Trim(), StringComparison.CurrentCultureIgnoreCase))
                {
                    IList<FuelType> fuelTypesFromDb =
                        await
                            _fuelTypeRepositoryService.GetAsync(
                                x => x.FuelTypeName.Trim().Equals(fuelType.FuelTypeName.Trim(), StringComparison.CurrentCultureIgnoreCase)
                                     && x.DeleteDate == null);
                    if (fuelTypesFromDb != null && fuelTypesFromDb.Any())
                    {
                        throw new RecordAlreadyExist($"There is aleady a fuelType Name: {fuelType.FuelTypeName}.");
                    }
                }
            }
        }

        private async Task ValidateFuelTypeHasNoChangeRequest(FuelType fuelType, ChangeType changeType)
        {
            // validate no CR for this FuelType Id.
            if (changeType != ChangeType.Add)
            {

                var changerequestid =
                    (await ChangeRequestBusinessService.ChangeRequestExist<FuelType>(item => item.Id == fuelType.Id));

                if (changerequestid > 0)
                {
                    throw new ChangeRequestExistException($"There is already an open CR ID {changerequestid} with FuelType ID : {fuelType.Id}.");
                }
            }

            // validate no CR for this FuelType Name.
            var changerequestid1 =
            await
                ChangeRequestBusinessService.ChangeRequestExist<FuelType>(
                    x => x.FuelTypeName.Trim().Equals(fuelType.FuelTypeName.Trim(), StringComparison.CurrentCultureIgnoreCase));

            if (changerequestid1 > 0)
            {
                throw new ChangeRequestExistException($"There is already an open CR ID {changerequestid1} with same FuelType Name : {fuelType.FuelTypeName}.");
            }
        }

       

        /// <summary>
        /// Find a single record which is not marked as deleted.
        /// </summary>
        /// <typeparam name="TKey">any</typeparam>
        /// <param name="id">the key property</param>
        /// <returns>Single entity object if condition matches, null if not found.</returns>
        private async Task<FuelType> FindAsync<TKey>(TKey id)
        {
            var entityId = Convert.ToInt32(id);
            var entities = await _fuelTypeRepositoryService.GetAsync(m => m.Id == entityId && m.DeleteDate == null, 1);
            if (entities != null && entities.Any())
            {
                return entities.First();
            }
            return null;
        }
    }
}