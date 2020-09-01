using System;
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
    public class VehicleTypeGroupBusinessService : VcdbBusinessService<VehicleTypeGroup>, IVehicleTypeGroupBusinessService
    {
        private readonly ISqlServerEfRepositoryService<VehicleTypeGroup> _vehicleTypeGroupRepositoryService = null;

        public VehicleTypeGroupBusinessService(IVcdbUnitOfWork vcdbRepositoryService,
            IVcdbChangeRequestBusinessService vcdbChangeRequestBusinessService,
            ITextSerializer serializer)
            : base(vcdbRepositoryService, vcdbChangeRequestBusinessService, serializer)
        {
            _vehicleTypeGroupRepositoryService = Repositories.GetRepositoryService<VehicleTypeGroup>() as ISqlServerEfRepositoryService<VehicleTypeGroup>;
        }

        public override async Task<List<VehicleTypeGroup>> GetAllAsync(int topCount = 0)
        {
            return await _vehicleTypeGroupRepositoryService.GetAsync(m => m.DeleteDate == null, topCount);
        }

        public override async Task<VehicleTypeGroup> GetAsync<TKey>(TKey id)
        {
            if (id.Equals(default(TKey)))
            {
                throw new ArgumentNullException(nameof(id));
            }

            var vehicleTypeGroup = await FindAsync(id);

            if (vehicleTypeGroup == null)
            {
                throw new NoRecordFound("Record Not Found");
            }

            vehicleTypeGroup.VehicleTypeCount = await _vehicleTypeGroupRepositoryService.GetCountAsync(vehicleTypeGroup, x => x.VehicleTypes, y => y.DeleteDate == null);

            return vehicleTypeGroup;
        }

        public override async Task<long> SubmitAddChangeRequestAsync(VehicleTypeGroup entity, string requestedBy, 
            List<ChangeRequestItemStaging> changeRequestItemStagings = null, CommentsStagingModel comment = null,
            List<AttachmentsModel> attachmentsStagingModels = null, string changeContent = null)
        {
            if (String.IsNullOrWhiteSpace(entity?.Name))
            {
                throw new ArgumentNullException(nameof(entity));
            }

            changeRequestItemStagings = changeRequestItemStagings ?? new List<ChangeRequestItemStaging>();

            await ValidateVehicleTypeGroupHasNoChangeRequest(entity);

            changeRequestItemStagings.Add(new ChangeRequestItemStaging()
            {
                EntityId = entity.Id.ToString(),
                Entity = typeof(VehicleTypeGroup).Name,
                ChangeType = ChangeType.Add,
                CreatedDateTime = DateTime.UtcNow,
                Payload = Serializer.Serialize(entity)
            });

            changeContent = string.Format("{0}"
                , entity.Name);

            // NOTE: change-request-comments-staging perfomed on base

            return await base.SubmitAddChangeRequestAsync(entity, requestedBy, changeRequestItemStagings, comment, attachmentsStagingModels, changeContent);
        }

        public override async Task<long> SubmitUpdateChangeRequestAsync<TId>(VehicleTypeGroup entity, TId id, string requestedBy,
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
                throw new ArgumentException("Invalid Vehicle Type Group Id");
            }

            var vehicleTypeGroupFromDb = await FindAsync(id);
            if (vehicleTypeGroupFromDb == null)
            {
                throw new NoRecordFound("No Vehicle Type Group exist");
            }

            changeRequestItemStagings = changeRequestItemStagings ?? new List<ChangeRequestItemStaging>();

            await ValidateVehicleTypeGroupHasNoChangeRequest(entity);
            await ValidateVehicleTypeGroupLookUpHasNoChangeRequest(entity);

            entity.InsertDate = vehicleTypeGroupFromDb.InsertDate;

            var existingEntity = new VehicleTypeGroup()
            {
                Id = vehicleTypeGroupFromDb.Id,
                Name = vehicleTypeGroupFromDb.Name,
                DeleteDate = vehicleTypeGroupFromDb.DeleteDate,
                InsertDate = vehicleTypeGroupFromDb.InsertDate,
                LastUpdateDate = vehicleTypeGroupFromDb.LastUpdateDate
            };

            changeRequestItemStagings.Add(new ChangeRequestItemStaging()
            {
                EntityId = entity.Id.ToString(),
                Entity = typeof(VehicleTypeGroup).Name,
                ChangeType = ChangeType.Modify,
                CreatedDateTime = DateTime.UtcNow,
                Payload = Serializer.Serialize(entity),
                ExistingPayload = base.Serializer.Serialize(existingEntity)
            });

            changeContent = string.Format("{0} > {1}"
                , vehicleTypeGroupFromDb.Name
                , entity.Name);

            var changeRequestId = await base.SubmitUpdateChangeRequestAsync(entity, id, requestedBy, ChangeType.Modify, changeRequestItemStagings, comment, attachments, changeContent);

            vehicleTypeGroupFromDb.ChangeRequestId = changeRequestId;
            _vehicleTypeGroupRepositoryService.Update(vehicleTypeGroupFromDb);
            Repositories.SaveChanges();
            return changeRequestId;
        }

        public override async Task<long> SubmitDeleteChangeRequestAsync<TId>(VehicleTypeGroup entity, TId id, string requestedBy, List<ChangeRequestItemStaging> changeRequestItemStagings = null,CommentsStagingModel comment=null, List<AttachmentsModel> attachmentsStaging = null, string changeContent = null)
        {
            if (id.Equals(default(TId)))
            {
                throw new ArgumentNullException(nameof(id));
            }

            var vehicleTypeGroupsFromDb = await FindAsync(id);
            if (vehicleTypeGroupsFromDb == null)
            {
                throw new NoRecordFound("No Sub-Model exist");
            }

            changeRequestItemStagings = changeRequestItemStagings ?? new List<ChangeRequestItemStaging>();

            await ValidateVehicleTypeGroupLookUpHasNoChangeRequest(vehicleTypeGroupsFromDb);

            changeRequestItemStagings.Add(new ChangeRequestItemStaging()
            {
                EntityId = vehicleTypeGroupsFromDb.Id.ToString(),
                ChangeType = ChangeType.Delete,
                CreatedDateTime = DateTime.UtcNow,
                Entity = typeof(VehicleTypeGroup).Name,
                Payload = base.Serializer.Serialize(new VehicleTypeGroup()
                {
                    Id = vehicleTypeGroupsFromDb.Id,
                    Name = vehicleTypeGroupsFromDb.Name
                })

            });

            changeContent = string.Format("{0}"
                , entity.Name);

            var changeRequestId = await base.SubmitDeleteChangeRequestAsync(vehicleTypeGroupsFromDb, id, requestedBy, changeRequestItemStagings, comment, attachmentsStaging, changeContent);

            vehicleTypeGroupsFromDb.ChangeRequestId = changeRequestId;
            _vehicleTypeGroupRepositoryService.Update(vehicleTypeGroupsFromDb);
            Repositories.SaveChanges();

            return changeRequestId;
        }

        public override async Task<ChangeRequestStagingModel<VehicleTypeGroup>> GetChangeRequestStaging<TId>(TId changeRequestId)
        {
            ChangeRequestStagingModel<VehicleTypeGroup> staging = await
                ChangeRequestBusinessService.GetChangeRequestStagingByChangeRequestIdAsync<VehicleTypeGroup, TId>(changeRequestId);

            // business specific task.
            // fill value of "EntityCurrent"
            //if (!staging.StagingItem.ChangeType.Equals(ChangeType.Add.ToString(), StringComparison.CurrentCultureIgnoreCase))
            //{
            //    VehicleTypeGroup currentVehicleTypeGroup = await FindAsync(staging.StagingItem.EntityId);
            //    staging.EntityCurrent = currentVehicleTypeGroup;
            //}

            return staging;
        }

        private async Task ValidateVehicleTypeGroupHasNoChangeRequest(VehicleTypeGroup vehicleTypeGroup)
        {
            IList<VehicleTypeGroup> vehicleTypeGroupsFromDb = await _vehicleTypeGroupRepositoryService.GetAsync(x => x.Name.ToLower().Equals(vehicleTypeGroup.Name.ToLower()) && x.DeleteDate == null);

            if (vehicleTypeGroupsFromDb != null && vehicleTypeGroupsFromDb.Any())
            {
                throw new RecordAlreadyExist("Vehicle Type Group already exists");
            }

            var changerequestid =
                await
                    ChangeRequestBusinessService.ChangeRequestExist<VehicleTypeGroup>(
                        x => x.Name.ToLower().Equals(vehicleTypeGroup.Name.ToLower()));
            if(changerequestid>0)
            {
                throw new ChangeRequestExistException($"There is already an open CR ID with {changerequestid} same Vehicle Type Group Name.");
            }
        }

        private async Task ValidateVehicleTypeGroupLookUpHasNoChangeRequest(VehicleTypeGroup vehicleTypeGroup)
        {
            var changerequestid =
                await
                    ChangeRequestBusinessService.ChangeRequestExist<VehicleType>(
                        x => x.VehicleTypeGroupId.Equals(vehicleTypeGroup.Id));
           
            if (changerequestid > 0) { 
                throw new ChangeRequestExistException($"There is already an open CR ID {changerequestid} for Vehicle Type using this Vehicle Type Group.");
            }
        }

        public override AssociationCount AssociatedCount(string payload)
        {
            AssociationCount count = new AssociationCount();
            if (payload != null)
            {
                VehicleTypeGroup  deserializedEntry = Serializer.Deserialize<VehicleTypeGroup>(payload);
                count.VehicleTypeCount = deserializedEntry.VehicleTypeCount;
            }
            return count;
        }

        public override async Task ClearChangeRequestId(long changeRequestId)
        {
            var entities = await _vehicleTypeGroupRepositoryService.GetAsync(m => m.ChangeRequestId == changeRequestId && m.DeleteDate == null);

            if (entities != null && entities.Any())
            {
                var entity = entities.First();   //can only be a single item
                entity.ChangeRequestId = null;
                _vehicleTypeGroupRepositoryService.Update(entity);
                Repositories.SaveChanges();
            }
        }

        /// <summary>
        /// Find a single record which is not marked as deleted.
        /// </summary>
        /// <typeparam name="TKey">any</typeparam>
        /// <param name="id">the key property</param>
        /// <returns>Single entity object if condition matches, null if not found.</returns>
        private async Task<VehicleTypeGroup> FindAsync<TKey>(TKey id)
        {
            var entityId = Convert.ToInt32(id);
            var entities = await _vehicleTypeGroupRepositoryService.GetAsync(m => m.Id == entityId && m.DeleteDate == null, 1);
            if (entities != null && entities.Any())
            {
                return entities.First();
            }
            return null;
        }
    }
}
