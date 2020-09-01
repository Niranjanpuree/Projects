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
    public class BrakeTypeBusinessService : VcdbBusinessService<BrakeType>, IBrakeTypeBusinessService
    {
        private readonly ISqlServerEfRepositoryService<BrakeType> _brakeTypeRepositoryService = null;

        public BrakeTypeBusinessService(IVcdbUnitOfWork vcdbRepositoryService,
            IVcdbChangeRequestBusinessService vcdbChangeRequestBusinessService,
            ITextSerializer serializer)
            : base(vcdbRepositoryService, vcdbChangeRequestBusinessService, serializer)
        {
            _brakeTypeRepositoryService = Repositories.GetRepositoryService<BrakeType>() as ISqlServerEfRepositoryService<BrakeType>;
        }

        public override async Task<List<BrakeType>> GetAllAsync(int topCount = 0)
        {
            return await _brakeTypeRepositoryService.GetAsync(x => x.DeleteDate == null, topCount);
        }

        public override async Task<BrakeType> GetAsync<TKey>(TKey id)
        {
            if (id.Equals(default(TKey)))
            {
                throw new ArgumentNullException(nameof(id));
            }

            var brakeType = await FindAsync(id);

            if (brakeType == null)
            {
                throw new NoRecordFound("Record Not Found");
            }

            brakeType.FrontBrakeConfigCount = await _brakeTypeRepositoryService.GetCountAsync(brakeType, x => x.BrakeConfigs_FrontBrakeTypeId,
                y => y.DeleteDate == null);
            brakeType.RearBrakeConfigCount = await _brakeTypeRepositoryService.GetCountAsync(brakeType, x => x.BrakeConfigs_RearBrakeTypeId,
                y => y.DeleteDate == null);

            var brakeTypeId = Convert.ToInt32(id);
            brakeType.VehicleToBrakeConfigCount = _brakeTypeRepositoryService
                .GetAllQueryable()
                .Where(x => x.Id == brakeTypeId)
                .Include("BrakeConfigs_FrontBrakeTypeId.VehicleToBrakeConfigs")
                .SelectMany(x=>x.BrakeConfigs_FrontBrakeTypeId)
                .SelectMany(x=> x.VehicleToBrakeConfigs).Count(y => y.DeleteDate == null);

            brakeType.VehicleToBrakeConfigCount += _brakeTypeRepositoryService
                .GetAllQueryable()
                .Where(x => x.Id == brakeTypeId)
                .Include("BrakeConfigs_RearBrakeTypeId.VehicleToBrakeConfigs")
                .SelectMany(x => x.BrakeConfigs_RearBrakeTypeId)
                .SelectMany(x => x.VehicleToBrakeConfigs).Count(y => y.DeleteDate == null);
            
            return brakeType;
        }

        public override async Task<long> SubmitAddChangeRequestAsync(BrakeType entity, string requestedBy, 
            List<ChangeRequestItemStaging> changeRequestItemStagings = null, CommentsStagingModel comment = null,
            List<AttachmentsModel> attachmentsStagingModels = null, string changeContent = null)
        {
            if (String.IsNullOrWhiteSpace(entity?.Name))
            {
                throw new ArgumentNullException(nameof(entity));
            }

            changeRequestItemStagings = changeRequestItemStagings ?? new List<ChangeRequestItemStaging>();

            await ValidateBrakeTypeHasNoChangeRequest(entity);

            changeRequestItemStagings.Add(new ChangeRequestItemStaging()
            {
                ChangeType = ChangeType.Add,
                EntityId = entity.Id.ToString(),
                Entity = typeof(BrakeType).Name,
                CreatedDateTime = DateTime.UtcNow,
                Payload = Serializer.Serialize(entity)
            });

            changeContent = string.Format("{0}"
                , entity.Name);

            // NOTE: change-request-comments-staging perfomed on base

            return await base.SubmitAddChangeRequestAsync(entity, requestedBy, changeRequestItemStagings, comment, attachmentsStagingModels, changeContent);
        }

        public override async Task<long> SubmitUpdateChangeRequestAsync<TId>(BrakeType entity, TId id, string requestedBy, 
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
                throw new ArgumentException("Invalid Brake Type Id");
            }

            var brakeTypeFromDb = await FindAsync(id);
            if (brakeTypeFromDb == null)
            {
                throw new NoRecordFound("No Brake Type exist");
            }
            
            changeRequestItemStagings = changeRequestItemStagings ?? new List<ChangeRequestItemStaging>();

            await ValidateBrakeTypeHasNoChangeRequest(entity);
            await ValidateBrakeTypeLookUpHasNoChangeRequest(entity);

            entity.InsertDate = brakeTypeFromDb.InsertDate;

            // to eliminate circular reference during serialize
            var existingEntity = new BrakeType()
            {
                Id = brakeTypeFromDb.Id,
                Name = brakeTypeFromDb.Name,
                ChangeRequestId = brakeTypeFromDb.ChangeRequestId,
                FrontBrakeConfigCount = brakeTypeFromDb.FrontBrakeConfigCount,
                RearBrakeConfigCount = brakeTypeFromDb.RearBrakeConfigCount,
                VehicleToBrakeConfigCount = brakeTypeFromDb.VehicleToBrakeConfigCount,
                DeleteDate = brakeTypeFromDb.DeleteDate,
                InsertDate = brakeTypeFromDb.InsertDate,
                LastUpdateDate = brakeTypeFromDb.LastUpdateDate
            };

            changeRequestItemStagings.Add(new ChangeRequestItemStaging()
            {
                ChangeType = ChangeType.Modify,
                EntityId = entity.Id.ToString(),
                CreatedDateTime = DateTime.UtcNow,
                Entity = typeof(BrakeType).Name,
                Payload = Serializer.Serialize(entity),
                ExistingPayload = base.Serializer.Serialize(existingEntity) // add existing record as ExistingPayload
            });

            changeContent = string.Format("{0} > {1}"
                , brakeTypeFromDb.Name
                , entity.Name);

            var changeRequestId = await base.SubmitUpdateChangeRequestAsync(entity, id, requestedBy, ChangeType.Modify, changeRequestItemStagings, comment, attachments, changeContent);

            brakeTypeFromDb.ChangeRequestId = changeRequestId;
            _brakeTypeRepositoryService.Update(brakeTypeFromDb);
            Repositories.SaveChanges();
            return changeRequestId;
        }

        public override async Task<long> SubmitDeleteChangeRequestAsync<TId>(BrakeType entity, TId id, string requestedBy, List<ChangeRequestItemStaging> changeRequestItemStagings = null, CommentsStagingModel comment = null, List<AttachmentsModel> attachmentsStaging = null, string changeContent = null)
        {
            // validation
            if (id.Equals(default(TId)))
            {
                throw new ArgumentNullException(nameof(id));
            }
            var brakeTypeFromDb = await FindAsync(id);
            if (brakeTypeFromDb == null)
            {
                throw new NoRecordFound("No Brake Type exist");
            }

            changeRequestItemStagings = changeRequestItemStagings ?? new List<ChangeRequestItemStaging>();
            
            await ValidateBrakeTypeLookUpHasNoChangeRequest(brakeTypeFromDb);

            changeRequestItemStagings.Add(new ChangeRequestItemStaging()
            {
                EntityId = brakeTypeFromDb.Id.ToString(),
                Entity = typeof(BrakeType).Name,
                ChangeType = ChangeType.Delete,
                CreatedDateTime = DateTime.UtcNow,
                Payload = Serializer.Serialize(new Make()
                {
                    Id = brakeTypeFromDb.Id,
                    Name = brakeTypeFromDb.Name
                })
            });

            changeContent = string.Format("{0}"
                , entity.Name);

            var changeRequestId = await base.SubmitDeleteChangeRequestAsync(brakeTypeFromDb, id, requestedBy, changeRequestItemStagings, comment, attachmentsStaging, changeContent);

            brakeTypeFromDb.ChangeRequestId = changeRequestId;
            _brakeTypeRepositoryService.Update(brakeTypeFromDb);
            Repositories.SaveChanges();

            return changeRequestId;
        }

        public override  async Task<ChangeRequestStagingModel<BrakeType>> GetChangeRequestStaging<TId>(TId changeRequestId)
        {
            ChangeRequestStagingModel<BrakeType> staging =
                await ChangeRequestBusinessService.GetChangeRequestStagingByChangeRequestIdAsync<BrakeType, TId>(changeRequestId);
            //if (!staging.StagingItem.ChangeType.Equals(ChangeType.Add.ToString(), StringComparison.CurrentCultureIgnoreCase))
            //{
            //    BrakeType currentBrakeType = await FindAsync(staging.StagingItem.EntityId);
            //    staging.EntityCurrent = currentBrakeType;
            //}

            return staging;
        }

        private async Task ValidateBrakeTypeHasNoChangeRequest(BrakeType brakeType)
        {
            IList<BrakeType> brakeTypesFromDb = await _brakeTypeRepositoryService.GetAsync(x => x.Name.ToLower().Equals(brakeType.Name.ToLower()) && x.DeleteDate == null);

            if (brakeTypesFromDb != null && brakeTypesFromDb.Any())
            {
                throw new RecordAlreadyExist("Brake Type already exists");
            }

            var changerequestid =
                await
                    ChangeRequestBusinessService.ChangeRequestExist<BrakeType>(
                        x => x.Name.ToLower().Equals(brakeType.Name.ToLower()));
            if (changerequestid > 0)
            {
                throw new ChangeRequestExistException("There is already an open CR ID "+ changerequestid + " with same Brake Type Name.");
            }
        }

        private async Task ValidateBrakeTypeLookUpHasNoChangeRequest(BrakeType brakeType)
        {

            var changerequestid =
                await
                    ChangeRequestBusinessService.ChangeRequestExist<BrakeConfig>(
                        x => (x.FrontBrakeTypeId.Equals(brakeType.Id) || x.RearBrakeTypeId.Equals(brakeType.Id)));
            if(changerequestid>0)
            {
                throw new ChangeRequestExistException($"There is already an open CR ID "+ changerequestid + " for brake Config using this Brake Type.");
            }
        }

        public override AssociationCount AssociatedCount(string payload)
        {
            AssociationCount count = new AssociationCount();
            if (payload != null)
            {
                BrakeType deserializedEntry = Serializer.Deserialize<BrakeType>(payload);
                count.VehicleToBrakeConfigCount = deserializedEntry.VehicleToBrakeConfigCount;
                count.FrontBrakeConfigCount = deserializedEntry.FrontBrakeConfigCount;
                count.RearBrakeConfigCount = deserializedEntry.RearBrakeConfigCount;
            }
            return count;
        }

        public override async Task ClearChangeRequestId(long changeRequestId)
        {
            var entities = await _brakeTypeRepositoryService.GetAsync(m => m.ChangeRequestId == changeRequestId);

            if (entities != null && entities.Any())
            {
                var entity = entities.First();   //can only be a single item
                entity.ChangeRequestId = null;
                _brakeTypeRepositoryService.Update(entity);
                Repositories.SaveChanges();
            }
        }

        /// <summary>
        /// Find a single record which is not marked as deleted.
        /// </summary>
        /// <typeparam name="TKey">any</typeparam>
        /// <param name="id">the key property</param>
        /// <returns>Single entity object if condition matches, null if not found.</returns>
        private async Task<BrakeType> FindAsync<TKey>(TKey id)
        {
            var entityId = Convert.ToInt32(id);
            var entities = await _brakeTypeRepositoryService.GetAsync(m => m.Id == entityId && m.DeleteDate == null, 1);
            if (entities != null && entities.Any())
            {
                return entities.First();
            }
            return null;
        }
    }
}
