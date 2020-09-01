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
    public class RegionBusinessService : VcdbBusinessService<Region>, IRegionBusinessService
    {
        private readonly ISqlServerEfRepositoryService<Region> _regionRepositoryService = null;

        public RegionBusinessService(IVcdbUnitOfWork vcdbRepositoryService,
            IVcdbChangeRequestBusinessService vcdbChangeRequestBusinessService,
            ITextSerializer serializer)
            : base(vcdbRepositoryService, vcdbChangeRequestBusinessService, serializer)
        {
            _regionRepositoryService = Repositories.GetRepositoryService<Region>() as ISqlServerEfRepositoryService<Region>;
        }

        public override async Task<List<Region>> GetAllAsync(int topCount = 0)
        {
            return await _regionRepositoryService.GetAsync(m => m.DeleteDate == null, topCount);
        }

        public override async Task<Region> GetAsync<TKey>(TKey id)
        {
            if (id.Equals(default(TKey)))
            {
                throw new ArgumentNullException(nameof(id));
            }

            var region = await FindAsync(id);

            if (region == null)
            {
                throw new NoRecordFound("Record Not Found");
            }

            region.VehicleCount = await _regionRepositoryService.GetCountAsync(region, x => x.Vehicles, y => y.DeleteDate == null);

            return region;
        }

        public override async Task<long> SubmitAddChangeRequestAsync(Region entity, string requestedBy, 
            List<ChangeRequestItemStaging> changeRequestItemStagings = null, CommentsStagingModel comment = null,
            List<AttachmentsModel> attachmentsStagingModels = null, string changeContent = null)
        {
            if (String.IsNullOrWhiteSpace(entity?.Name))
            {
                throw new ArgumentNullException(nameof(entity));
            }
            changeRequestItemStagings = changeRequestItemStagings ?? new List<ChangeRequestItemStaging>();

            await ValidateRegionHasNoChangeRequest(entity);

            changeRequestItemStagings.Add(new ChangeRequestItemStaging()
            {
                EntityId = entity.Id.ToString(),
                Entity =  typeof(Region).Name,
                ChangeType = ChangeType.Add,
                CreatedDateTime = DateTime.UtcNow,
                Payload = Serializer.Serialize(entity)
            });

            changeContent = string.Format("{0}"
                , entity.Name);

            // NOTE: change-request-comments-staging perfomed on base

            return await base.SubmitAddChangeRequestAsync(entity, requestedBy, changeRequestItemStagings, comment, attachmentsStagingModels, changeContent);
        }

        public override async Task<long> SubmitUpdateChangeRequestAsync<TId>(Region entity, TId id, string requestedBy, 
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
                throw new ArgumentException("Invalid Region Id");
            }

            var regionFromDb = await FindAsync(id);
            if (regionFromDb == null)
            {
                throw new NoRecordFound("No Region exist");
            }

            changeRequestItemStagings = changeRequestItemStagings ?? new List<ChangeRequestItemStaging>();

            await ValidateRegionHasNoChangeRequest(entity);
            await ValidateRegionLookUpHasNoChangeRequest(entity);

            entity.InsertDate = regionFromDb.InsertDate;
            entity.ParentId = regionFromDb.ParentId;
            entity.RegionAbbr_2 = regionFromDb.RegionAbbr_2;

            var existingEntity = new Region()
            {
                Id = regionFromDb.Id,
                Name= regionFromDb.Name,
                ChangeRequestId = regionFromDb.ChangeRequestId,
                RegionAbbr = regionFromDb.RegionAbbr,
                VehicleCount = regionFromDb.VehicleCount,
                DeleteDate = regionFromDb.DeleteDate,
                InsertDate = regionFromDb.InsertDate,
                LastUpdateDate = regionFromDb.LastUpdateDate
            };

            changeRequestItemStagings.Add(new ChangeRequestItemStaging()
            {
                EntityId = entity.Id.ToString(),
                Entity = typeof(Region).Name,
                ChangeType = ChangeType.Modify,
                CreatedDateTime = DateTime.UtcNow,
                Payload = Serializer.Serialize(entity),
                ExistingPayload = Serializer.Serialize(existingEntity)
            });

            changeContent = string.Format("{0} > {1}"
                , regionFromDb.Name
                , entity.Name);

            var changeRequestId = await base.SubmitUpdateChangeRequestAsync(entity, id, requestedBy, ChangeType.Modify, changeRequestItemStagings, comment, attachments, changeContent);

            regionFromDb.ChangeRequestId = changeRequestId;
            _regionRepositoryService.Update(regionFromDb);
            Repositories.SaveChanges();

            return changeRequestId;
        }

        public override async Task<long> SubmitDeleteChangeRequestAsync<TId>(Region entity, TId id, string requestedBy, List<ChangeRequestItemStaging> changeRequestItemStagings = null, CommentsStagingModel comment = null, List<AttachmentsModel> attachmentsStaging = null, string changeContent = null)
        {
            if (id.Equals(default(TId)))
            {
                throw new ArgumentNullException(nameof(id));
            }
            var regionFromDb = await FindAsync(id);
            if (regionFromDb == null)
            {
                throw new NoRecordFound("No Region exist");
            }

            changeRequestItemStagings = changeRequestItemStagings ?? new List<ChangeRequestItemStaging>();

            await ValidateRegionLookUpHasNoChangeRequest(regionFromDb);

            changeRequestItemStagings.Add(new ChangeRequestItemStaging()
            {
                EntityId = regionFromDb.Id.ToString(),
                Entity = typeof(Region).Name,
                ChangeType = ChangeType.Delete,
                CreatedDateTime = DateTime.UtcNow,
                Payload = Serializer.Serialize(new Region()
                {
                    Id = regionFromDb.Id,
                    Name = regionFromDb.Name
                })
            });

            changeContent = string.Format("{0}"
                , entity.Name);

            var changeRequestId = await base.SubmitDeleteChangeRequestAsync(regionFromDb, id, requestedBy, changeRequestItemStagings, comment, attachmentsStaging, changeContent);

            regionFromDb.ChangeRequestId = changeRequestId;
            _regionRepositoryService.Update(regionFromDb);
            Repositories.SaveChanges();

            return changeRequestId;
        }

        private async Task ValidateRegionHasNoChangeRequest(Region region)
        {
            IList<Region> regionsFromDb = await _regionRepositoryService.GetAsync(x => x.Name.ToLower().Equals(region.Name.ToLower()) && x.DeleteDate == null);
            if (regionsFromDb != null && regionsFromDb.Any())
            {
                    throw new RecordAlreadyExist("Region already exists");
            }
            var changerequestid =
                await
                    ChangeRequestBusinessService.ChangeRequestExist<Region>(
                        x =>
                            x.Name.ToLower().Equals(region.Name.ToLower()));// only region name is checked
            if(changerequestid>0)
            {
                throw new ChangeRequestExistException("There is already an open CR with ID "+ changerequestid + " for the same Region Name.");
            }
            
        }

        private async Task ValidateRegionLookUpHasNoChangeRequest(Region region)
        {
            var changerequestid =
                await
                    ChangeRequestBusinessService.ChangeRequestExist<Vehicle>(x => x.RegionId.Equals(region.Id));
            if(changerequestid>0)
            {
                throw new ChangeRequestExistException($"There is already an open CR ID {changerequestid} for Vehicle using this Region name.");
            }
        }

        public override async Task<ChangeRequestStagingModel<Region>> GetChangeRequestStaging<TId>(TId changeRequestId)
        {
            ChangeRequestStagingModel<Region> staging = await
                ChangeRequestBusinessService.GetChangeRequestStagingByChangeRequestIdAsync<Region, TId>(changeRequestId);

            // business specific task.
            // fill value of "EntityCurrent"
            //if (!staging.StagingItem.ChangeType.Equals(ChangeType.Add.ToString(), StringComparison.CurrentCultureIgnoreCase))
            //{
            //    Region currentModel = await FindAsync(staging.StagingItem.EntityId);
            //    staging.EntityCurrent = currentModel;
            //}
           return staging;
        }

        public override AssociationCount AssociatedCount(string payload)
        {
            AssociationCount count = new AssociationCount();
            if (payload != null)
            {
                Region deserializedEntry = Serializer.Deserialize<Region>(payload);
                count.VehicleCount = deserializedEntry.VehicleCount;
            }
            return count;
        }

        public override async Task ClearChangeRequestId(long changeRequestId)
        {
            var entities = await _regionRepositoryService.GetAsync(m => m.ChangeRequestId == changeRequestId && m.DeleteDate == null);

            if (entities != null && entities.Any())
            {
                var entity = entities.First();   //can only be a single item
                entity.ChangeRequestId = null;
                _regionRepositoryService.Update(entity);
                Repositories.SaveChanges();
            }
        }

        /// <summary>
        /// Find a single record which is not marked as deleted.
        /// </summary>
        /// <typeparam name="TKey">any</typeparam>
        /// <param name="id">the key property</param>
        /// <returns>Single entity object if condition matches, null if not found.</returns>
        private async Task<Region> FindAsync<TKey>(TKey id)
        {
            var entityId = Convert.ToInt32(id);
            var entities = await _regionRepositoryService.GetAsync(m => m.Id == entityId && m.DeleteDate == null, 1);
            if (entities != null && entities.Any())
            {
                return entities.First();
            }
            return null;
        }
    }
}
