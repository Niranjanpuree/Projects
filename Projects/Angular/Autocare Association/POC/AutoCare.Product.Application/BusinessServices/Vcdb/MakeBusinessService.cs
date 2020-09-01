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
    public class MakeBusinessService : VcdbBusinessService<Make>, IMakeBusinessService
    {
        private readonly ISqlServerEfRepositoryService<Make> _makeRepositoryService;
        private readonly IVcdbChangeRequestBusinessService _vcdbChangeRequestBusinessService;

        public MakeBusinessService(IVcdbUnitOfWork vcdbRepositoryService,
            IVcdbChangeRequestBusinessService vcdbChangeRequestBusinessService,
            ITextSerializer serializer)
            : base(vcdbRepositoryService, vcdbChangeRequestBusinessService, serializer)
        {
            _makeRepositoryService = Repositories.GetRepositoryService<Make>() as ISqlServerEfRepositoryService<Make>;
            _vcdbChangeRequestBusinessService = vcdbChangeRequestBusinessService;
        }

        public override async Task<List<Make>> GetAllAsync(int topCount = 0)
        {
            return await _makeRepositoryService.GetAsync(m => m.DeleteDate == null, topCount);
        }

        public override async Task<Make> GetAsync<TKey>(TKey id)
        {
            // validations
            if (id.Equals(default(TKey)))
            {
                throw new ArgumentNullException(nameof(id));
            }

            var make = await FindAsync(id);

            if (make == null)
            {
                throw new NoRecordFound("Record Not Found");
            }

            make.BaseVehicleCount = await _makeRepositoryService.GetCountAsync(make, x => x.BaseVehicles, y => y.DeleteDate == null);

            var makeId = Convert.ToInt32(id);
            make.VehicleCount = _makeRepositoryService
                .GetAllQueryable()
                .Where(x => x.Id == makeId).Include("BaseVehicles.Vehicles")
                .SelectMany(x => x.BaseVehicles)
                .SelectMany(x => x.Vehicles).Count(y => y.DeleteDate == null);

            return make;
        }

        public override async Task<long> SubmitAddChangeRequestAsync(Make entity, string requestedBy,
            List<ChangeRequestItemStaging> changeRequestItemStagings = null, CommentsStagingModel comment = null,
            List<AttachmentsModel> attachmentsStagingModels = null, string changeContent = null)
        {
            if (string.IsNullOrWhiteSpace(entity?.Name))
            {
                throw new ArgumentNullException(nameof(entity));
            }

            // validations
            await ValidateMakeIsNotDuplicate(entity, ChangeType.Add);
            await ValidateMakeHasNoChangeRequest(entity, ChangeType.Add);

            changeRequestItemStagings = changeRequestItemStagings ?? new List<ChangeRequestItemStaging>();
            changeRequestItemStagings.Add(new ChangeRequestItemStaging
            {
                EntityId = entity.Id.ToString(),
                Entity = typeof(Make).Name,
                ChangeType = ChangeType.Add,
                CreatedDateTime = DateTime.UtcNow,
                Payload = Serializer.Serialize(entity)
            });

            changeContent = string.Format("{0}"
                , entity.Name);

            return await base.SubmitAddChangeRequestAsync(entity, requestedBy, changeRequestItemStagings, comment, attachmentsStagingModels, changeContent);
        }

        public override async Task<long> SubmitUpdateChangeRequestAsync<TId>(Make entity, TId id, string requestedBy,
            ChangeType changeType = ChangeType.None,
            List<ChangeRequestItemStaging> changeRequestItemStagings = null, CommentsStagingModel comment = null, List<AttachmentsModel> attachments = null, string changeContent = null)
        {
            if (string.IsNullOrWhiteSpace(entity?.Name))
            {
                throw new ArgumentNullException(nameof(entity));
            }
            if (id.Equals(default(TId)))
            {
                throw new ArgumentNullException(nameof(id));
            }
            if (!entity.Id.Equals(id))
            {
                throw new ArgumentException("Invalid Make Id");
            }

            var makeFromDb = await FindAsync(id);

            if (makeFromDb == null)
            {
                throw new NoRecordFound("Record Not Found");
            }

            // validations
            await ValidateMakeIsNotDuplicate(entity, ChangeType.Modify);
            await ValidateMakeHasNoChangeRequest(entity, ChangeType.Modify);
            await ValidateMakeLookUpHasNoChangeRequest(entity);

            // fill up audit information
            entity.InsertDate = makeFromDb.InsertDate;
            entity.LastUpdateDate = makeFromDb.LastUpdateDate;
            entity.DeleteDate = makeFromDb.DeleteDate;

            changeRequestItemStagings = changeRequestItemStagings ?? new List<ChangeRequestItemStaging>();

            // to eliminate circular reference during serialize
            var existingEntity = new Make()
            {
                Id = makeFromDb.Id,
                Name = makeFromDb.Name,
                ChangeRequestId = makeFromDb.ChangeRequestId,
                VehicleCount = makeFromDb.VehicleCount,
                BaseVehicleCount = makeFromDb.BaseVehicleCount,
                DeleteDate = makeFromDb.DeleteDate,
                InsertDate = makeFromDb.InsertDate,
                LastUpdateDate = makeFromDb.LastUpdateDate
            };

            changeRequestItemStagings.Add(new ChangeRequestItemStaging
            {
                EntityId = entity.Id.ToString(),
                Entity = typeof(Make).Name,
                ChangeType = ChangeType.Modify,
                CreatedDateTime = DateTime.UtcNow,
                Payload = Serializer.Serialize(entity),
                ExistingPayload = base.Serializer.Serialize(existingEntity) // add existing record as ExistingPayload
            });

            changeContent = string.Format("{0} > {1}"
                , makeFromDb.Name
                , entity.Name);

            var changeRequestId = await
                    base.SubmitUpdateChangeRequestAsync(entity, id, requestedBy, ChangeType.Modify,
                        changeRequestItemStagings, comment, attachments, changeContent);

            makeFromDb.ChangeRequestId = changeRequestId;
            _makeRepositoryService.Update(makeFromDb);
            Repositories.SaveChanges();

            return changeRequestId;
        }

        public override async Task<long> SubmitDeleteChangeRequestAsync<TId>(Make entity, TId id, string requestedBy,
            List<ChangeRequestItemStaging> changeRequestItemStagings = null, CommentsStagingModel comment = null, List<AttachmentsModel> attachmentsStaging = null, string changeContent = null)
        {
            if (id.Equals(default(TId)))
            {
                throw new ArgumentNullException(nameof(id));
            }

            var makeFromDb = await FindAsync(id);

            if (makeFromDb == null)
            {
                throw new NoRecordFound("Record Not Found");
            }

            // validations
            await ValidateMakeLookUpHasNoChangeRequest(entity);

            changeRequestItemStagings = changeRequestItemStagings ?? new List<ChangeRequestItemStaging>();
            changeRequestItemStagings.Add(new ChangeRequestItemStaging
            {
                EntityId = entity.Id.ToString(),
                Entity = typeof(Make).Name,
                ChangeType = ChangeType.Delete,
                CreatedDateTime = DateTime.UtcNow,
                Payload = Serializer.Serialize(new Make
                {
                    Id = makeFromDb.Id,
                    Name = makeFromDb.Name
                })
            });

            changeContent = string.Format("{0}"
                , entity.Name);

            var changeRequestId = await base.SubmitDeleteChangeRequestAsync(entity, id, requestedBy, changeRequestItemStagings, comment, attachmentsStaging, changeContent);

            makeFromDb.ChangeRequestId = changeRequestId;
            _makeRepositoryService.Update(makeFromDb);
            Repositories.SaveChanges();

            return changeRequestId;
        }

        public override async Task<ChangeRequestStagingModel<Make>> GetChangeRequestStaging<TId>(TId changeRequestId)
        {
            ChangeRequestStagingModel<Make> staging = await
                this._vcdbChangeRequestBusinessService.GetChangeRequestStagingByChangeRequestIdAsync<Make, TId>(changeRequestId);

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
                Make deserializedEntry = Serializer.Deserialize<Make>(payload);
                count.BaseVehicleCount = deserializedEntry.BaseVehicleCount;
                count.VehicleCount = deserializedEntry.VehicleCount;
            }
            return count;
        }

        public override async Task ClearChangeRequestId(long changeRequestId)
        {
            var entities = await _makeRepositoryService.GetAsync(m => m.ChangeRequestId == changeRequestId && m.DeleteDate == null);

            if (entities != null && entities.Any())
            {
                var entity = entities.First();   //can only be a single item
                entity.ChangeRequestId = null;
                _makeRepositoryService.Update(entity);
                Repositories.SaveChanges();
            }
        }

        private async Task ValidateMakeIsNotDuplicate(Make make, ChangeType changeType)
        {
            // validate no duplicate Make Name.
            if (changeType == ChangeType.Add)
            {
                IList<Make> makesFromDb =
                    await
                        _makeRepositoryService.GetAsync(
                            x => x.Name.Trim().Equals(make.Name.Trim(), StringComparison.CurrentCultureIgnoreCase)
                                 && x.DeleteDate == null);
                if (makesFromDb != null && makesFromDb.Any())
                {
                    throw new RecordAlreadyExist($"There is aleady a Make Name: {make.Name}.");
                }
            }
            else if (changeType == ChangeType.Modify) // allow whitespace and case correction
            {
                var makeFromDb = await FindAsync(make.Id);

                if (make.Name.Trim().Equals(makeFromDb.Name.Trim(), StringComparison.CurrentCulture))
                {
                    throw new RecordAlreadyExist($"There is aleady a Make Name: {make.Name}.");
                }
                if (!make.Name.Trim().Equals(makeFromDb.Name.Trim(), StringComparison.CurrentCultureIgnoreCase))
                {
                    IList<Make> makesFromDb =
                        await
                            _makeRepositoryService.GetAsync(
                                x => x.Name.Trim().Equals(make.Name.Trim(), StringComparison.CurrentCultureIgnoreCase)
                                     && x.DeleteDate == null);
                    if (makesFromDb != null && makesFromDb.Any())
                    {
                        throw new RecordAlreadyExist($"There is aleady a Make Name: {make.Name}.");
                    }
                }
            }
        }

        private async Task ValidateMakeHasNoChangeRequest(Make make, ChangeType changeType)
        {
            // validate no CR for this Make Id.
            if (changeType != ChangeType.Add)
            {

                var changerequestid =
                    (await ChangeRequestBusinessService.ChangeRequestExist<Make>(item => item.Id == make.Id));

                if(changerequestid>0)
                {
                    throw new ChangeRequestExistException($"There is already an open CR ID {changerequestid} with Make ID : {make.Id}.");
                }
            }

            // validate no CR for this Make Name.
            var changerequestid1=
            await
                ChangeRequestBusinessService.ChangeRequestExist<Make>(
                    x => x.Name.Trim().Equals(make.Name.Trim(), StringComparison.CurrentCultureIgnoreCase));

            if(changerequestid1>0){
                throw new ChangeRequestExistException($"There is already an open CR ID {changerequestid1} with same Make Name : {make.Name}.");
            }
        }

        private async Task ValidateMakeLookUpHasNoChangeRequest(Make make)
        {
            // validate no CR for base vehicle using this make.
            var changerequestid=
                await
                    ChangeRequestBusinessService.ChangeRequestExist<BaseVehicle>(x => x.MakeId == make.Id);
            if(changerequestid > 0)
            {
                throw new ChangeRequestExistException($"There is already an open CR ID {changerequestid} for base Vehicle using this Make.");
            }
        }

        /// <summary>
        /// Find a single record which is not marked as deleted.
        /// </summary>
        /// <typeparam name="TKey">any</typeparam>
        /// <param name="id">the key property</param>
        /// <returns>Single entity object if condition matches, null if not found.</returns>
        private async Task<Make> FindAsync<TKey>(TKey id)
        {
            var entityId = Convert.ToInt32(id);
            var entities = await _makeRepositoryService.GetAsync(m => m.Id == entityId && m.DeleteDate == null, 1);
            if (entities != null && entities.Any())
            {
                return entities.First();
            }
            return null;
        }
    }
}