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
    public class BrakeSystemBusinessService : VcdbBusinessService<BrakeSystem>, IBrakeSystemBusinessService
    {
        private readonly ISqlServerEfRepositoryService<BrakeSystem> _brakeSystemRepositoryService = null;
        public BrakeSystemBusinessService(IVcdbUnitOfWork vcdbRepositoryService,
            IVcdbChangeRequestBusinessService vcdbChangeRequestBusinessService,
            ITextSerializer serializer)
            : base(vcdbRepositoryService, vcdbChangeRequestBusinessService, serializer)
        {
            _brakeSystemRepositoryService = Repositories.GetRepositoryService<BrakeSystem>() as ISqlServerEfRepositoryService<BrakeSystem>;
        }

        public override async Task<List<BrakeSystem>> GetAllAsync(int topCount = 0)
        {
            return await _brakeSystemRepositoryService.GetAsync(m => m.DeleteDate == null, topCount);
        }

        public override async Task<BrakeSystem> GetAsync<TKey>(TKey id)
        {
            if (id.Equals(default(TKey)))
            {
                throw new ArgumentNullException(nameof(id));
            }

            var brakeSystem = await FindAsync(id);

            if (brakeSystem == null)
            {
                throw new NoRecordFound("Record Not Found");
            }

            brakeSystem.BrakeConfigCount = await _brakeSystemRepositoryService.GetCountAsync(brakeSystem, x => x.BrakeConfigs, y => y.DeleteDate == null);

            var brakeTypeId = Convert.ToInt32(id);
            brakeSystem.VehicleToBrakeConfigCount = _brakeSystemRepositoryService
                .GetAllQueryable()
                .Where(x => x.Id == brakeTypeId).Include("BrakeConfigs.VehicleToBrakeConfigs")
                .SelectMany(x => x.BrakeConfigs)
                .SelectMany(x => x.VehicleToBrakeConfigs).Count(y => y.DeleteDate == null);

            return brakeSystem;
        }

        public override async Task<long> SubmitAddChangeRequestAsync(BrakeSystem entity, string requestedBy, 
            List<ChangeRequestItemStaging> changeRequestItemStagings = null, CommentsStagingModel comment = null,
            List<AttachmentsModel> attachmentsStagingModels = null, string changeContent = null)
        {
            if (String.IsNullOrWhiteSpace(entity?.Name))
            {
                throw new ArgumentNullException(nameof(entity));
            }

            changeRequestItemStagings = changeRequestItemStagings ?? new List<ChangeRequestItemStaging>();

            await ValidateBrakeSystemHasNoChangeRequest(entity);
            
            changeRequestItemStagings.Add(new ChangeRequestItemStaging()
            {
                ChangeType = ChangeType.Add,
                EntityId = entity.Id.ToString(),
                Entity = typeof(BrakeSystem).Name,
                CreatedDateTime = DateTime.UtcNow,
                Payload = Serializer.Serialize(entity)
            });

            changeContent = string.Format("{0}"
                , entity.Name);

            // NOTE: change-request-comments-staging perfomed on base
            return await base.SubmitAddChangeRequestAsync(entity, requestedBy, changeRequestItemStagings, comment, attachmentsStagingModels, changeContent);
        }

        public override async Task<long> SubmitUpdateChangeRequestAsync<TId>(BrakeSystem entity, TId id, string requestedBy, 
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
                throw new ArgumentException("Invalid Brake System Id");
            }

            var brakeSystemFromDb = await FindAsync(id);
            if (brakeSystemFromDb == null)
            {
                throw new NoRecordFound("No Brake System exist");
            }

            changeRequestItemStagings = changeRequestItemStagings ?? new List<ChangeRequestItemStaging>();

            await ValidateBrakeSystemHasNoChangeRequest(entity);
            await ValidateBrakeSystemLookUpHasNoChangeRequest(entity);

            entity.InsertDate = brakeSystemFromDb.InsertDate;

            var existingEntity = new VehicleTypeGroup()
            {
                Id = brakeSystemFromDb.Id,
                Name = brakeSystemFromDb.Name,
                DeleteDate = brakeSystemFromDb.DeleteDate,
                InsertDate = brakeSystemFromDb.InsertDate,
                LastUpdateDate = brakeSystemFromDb.LastUpdateDate
            };

            changeRequestItemStagings.Add(new ChangeRequestItemStaging()
            {
                ChangeType = ChangeType.Modify,
                EntityId = entity.Id.ToString(),
                CreatedDateTime = DateTime.UtcNow,
                Entity = typeof(BrakeSystem).Name,
                Payload = Serializer.Serialize(entity),
                ExistingPayload = base.Serializer.Serialize(existingEntity)
            });

            changeContent = string.Format("{0} > {1}"
                , brakeSystemFromDb.Name
                , entity.Name);

            var changeRequestId = await base.SubmitUpdateChangeRequestAsync(entity, id, requestedBy, ChangeType.Modify, changeRequestItemStagings, comment, attachments, changeContent);

            brakeSystemFromDb.ChangeRequestId = changeRequestId;
            _brakeSystemRepositoryService.Update(brakeSystemFromDb);
            Repositories.SaveChanges();
            return changeRequestId;
        }

        public override async Task<long> SubmitDeleteChangeRequestAsync<TId>(BrakeSystem entity, TId id, string requestedBy, List<ChangeRequestItemStaging> changeRequestItemStagings = null, CommentsStagingModel comment = null, List<AttachmentsModel> attachmentsStaging = null, string changeContent = null)
        {
            if (id.Equals(default(TId)))
            {
                throw new ArgumentNullException(nameof(id));
            }
            var brakeSystemFromDb = await FindAsync(id);
            if (brakeSystemFromDb == null)
            {
                throw new NoRecordFound("No Brake System exist");
            }

            changeRequestItemStagings = changeRequestItemStagings ?? new List<ChangeRequestItemStaging>();

            await ValidateBrakeSystemLookUpHasNoChangeRequest(brakeSystemFromDb);

            changeRequestItemStagings.Add(new ChangeRequestItemStaging()
            {
                EntityId = brakeSystemFromDb.Id.ToString(),
                Entity = typeof(BrakeSystem).Name,
                ChangeType = ChangeType.Delete,
                CreatedDateTime = DateTime.UtcNow,
                Payload = Serializer.Serialize(new Make()
                {
                    Id = brakeSystemFromDb.Id,
                    Name = brakeSystemFromDb.Name
                })
            });

            changeContent = string.Format("{0}"
                , entity.Name);

            var changeRequestId = await base.SubmitDeleteChangeRequestAsync(brakeSystemFromDb, id, requestedBy, changeRequestItemStagings, comment, attachmentsStaging, changeContent);

            brakeSystemFromDb.ChangeRequestId = changeRequestId;
            _brakeSystemRepositoryService.Update(brakeSystemFromDb);
            Repositories.SaveChanges();

            return changeRequestId;
        }
        public override async Task<ChangeRequestStagingModel<BrakeSystem>> GetChangeRequestStaging<TId>(TId changeRequestId)
        {
            ChangeRequestStagingModel<BrakeSystem> staging =
              await ChangeRequestBusinessService.GetChangeRequestStagingByChangeRequestIdAsync<BrakeSystem, TId>(changeRequestId);

            //if (!staging.StagingItem.ChangeType.Equals(ChangeType.Add.ToString(), StringComparison.CurrentCultureIgnoreCase))
            //{
            //    BrakeSystem currentBrakeSystem = await FindAsync(Convert.ToInt32(staging.StagingItem.EntityId));
            //    staging.EntityCurrent = currentBrakeSystem;
            //}

            return staging;
        }

        private async Task ValidateBrakeSystemHasNoChangeRequest(BrakeSystem brakeSystem)
        {
            IList<BrakeSystem> brakeSystemsFromDb = await _brakeSystemRepositoryService.GetAsync(x => x.Name.ToLower().Equals(brakeSystem.Name.ToLower()) && x.DeleteDate == null);

            if (brakeSystemsFromDb != null && brakeSystemsFromDb.Any())
            {
                throw new RecordAlreadyExist("Brake System already exists");
            }

            var changerequestid =
                await
                    ChangeRequestBusinessService.ChangeRequestExist<BrakeSystem>(
                        x => x.Name.ToLower().Equals(brakeSystem.Name.ToLower()));
            if(changerequestid>0)
            {
                throw new ChangeRequestExistException("There is already an open CR ID "+ changerequestid + " for Brake System Name.");
            }
        }

        private async Task ValidateBrakeSystemLookUpHasNoChangeRequest(BrakeSystem brakeSystem)
        {
            var changerequestid =
                await
                    ChangeRequestBusinessService.ChangeRequestExist<BrakeConfig>(
                        x => x.BrakeSystemId.Equals(brakeSystem.Id));
           if(changerequestid>0)
            {
                throw new ChangeRequestExistException($"There is already an open CR ID "+ changerequestid + " for brake Config using this Brake System.");
            }
        }

        public override AssociationCount AssociatedCount(string payload)
        {
            AssociationCount count = new AssociationCount();
            if (payload != null)
            {
                BrakeSystem deserializedEntry = Serializer.Deserialize<BrakeSystem>(payload);
                count.BrakeConfigCount = deserializedEntry.BrakeConfigCount;
                count.VehicleToBrakeConfigCount = deserializedEntry.VehicleToBrakeConfigCount;
            }
            return count;
        }

        public override async Task ClearChangeRequestId(long changeRequestId)
        {
            var entities = await _brakeSystemRepositoryService.GetAsync(x => x.ChangeRequestId == changeRequestId && x.DeleteDate == null);

            if (entities != null && entities.Any())
            {
                var entity = entities.First();   //can only be a single item
                entity.ChangeRequestId = null;
                _brakeSystemRepositoryService.Update(entity);
                Repositories.SaveChanges();
            }
        }

        /// <summary>
        /// Find a single record which is not marked as deleted.
        /// </summary>
        /// <typeparam name="TKey">any</typeparam>
        /// <param name="id">the key property</param>
        /// <returns>Single entity object if condition matches, null if not found.</returns>
        private async Task<BrakeSystem> FindAsync<TKey>(TKey id)
        {
            var entityId = Convert.ToInt32(id);
            var entities = await _brakeSystemRepositoryService.GetAsync(m => m.Id == entityId && m.DeleteDate == null, 1);
            if (entities != null && entities.Any())
            {
                return entities.First();
            }
            return null;
        }
    }
}
