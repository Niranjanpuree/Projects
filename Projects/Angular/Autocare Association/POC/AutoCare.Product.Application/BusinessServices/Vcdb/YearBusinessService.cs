using AutoCare.Product.Application.RepositoryServices;
using AutoCare.Product.Infrastructure.ExceptionHandler;
using AutoCare.Product.Infrastructure.RepositoryService;
using AutoCare.Product.Infrastructure.Serializer;
using AutoCare.Product.Vcdb.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoCare.Product.Application.Models.BusinessModels;

namespace AutoCare.Product.Application.BusinessServices.Vcdb
{
    public class YearBusinessService : VcdbBusinessService<Year>, IYearBusinessService
    {
        private readonly ISqlServerEfRepositoryService<Year> _yearRepositoryService;

        public YearBusinessService(IVcdbUnitOfWork vcdbUnitOfWork,
            IVcdbChangeRequestBusinessService vcdbChangeRequstBusinessService,
            ITextSerializer serializer)
            : base(vcdbUnitOfWork, vcdbChangeRequstBusinessService, serializer)
        {
            _yearRepositoryService = Repositories.GetRepositoryService<Year>() as ISqlServerEfRepositoryService<Year>;
        }

        public override async Task<List<Year>> GetAllAsync(int topCount = 0)
        {
            return await _yearRepositoryService.GetAsync(m => m.DeleteDate == null, topCount);
        }

        public override async Task<Year> GetAsync<TKey>(TKey id)
        {
            if (id.Equals(default(TKey)))
            {
                throw new ArgumentNullException(nameof(id));
            }

            var year = await FindAsync(id);

            if (year == null)
            {
                throw new NoRecordFound("Record Not Found");
            }

            year.BaseVehicleCount = await _yearRepositoryService.GetCountAsync(year, x => x.BaseVehicles, y => y.DeleteDate == null);

            return year;
        }

        public override AssociationCount AssociatedCount(string payload)
        {
            AssociationCount count = new AssociationCount();
            if (payload != null)
            {
                Year deserializedEntry = Serializer.Deserialize<Year>(payload);
                count.BaseVehicleCount = deserializedEntry.BaseVehicleCount;
            }
            return count;
        }

        public override async Task<long> SubmitAddChangeRequestAsync(Year entity, string requestedBy, 
            List<ChangeRequestItemStaging> changeRequestItemStagings = null, CommentsStagingModel comment = null,
            List<AttachmentsModel> attachmentsStagingModels = null, string changeContent = null)
        {
            if (entity?.Id == default(int))
            {
                throw new ArgumentNullException("Year is required");
            }

            var yearFromDb = await FindAsync(entity.Id);
            if (yearFromDb != null)
            {
                throw new RecordAlreadyExist("Year already exists");
            }
            changeRequestItemStagings = changeRequestItemStagings ?? new List<ChangeRequestItemStaging>();
            changeRequestItemStagings.Add(new ChangeRequestItemStaging()
            {
                EntityId = "0",
                Entity = typeof(Year).Name,
                ChangeType = ChangeType.Add,
                CreatedDateTime = DateTime.UtcNow,
                Payload = Serializer.Serialize(entity)
            });

            // NOTE: change-request-comments-staging perfomed on base

            return await base.SubmitAddChangeRequestAsync(entity, requestedBy, changeRequestItemStagings, comment, attachmentsStagingModels);
        }

        public override async Task<long> SubmitUpdateChangeRequestAsync<TId>(Year entity, TId id, string requestedBy,
            ChangeType changeType = ChangeType.None,
            List<ChangeRequestItemStaging> changeRequestItemStagings = null, CommentsStagingModel comment = null, List<AttachmentsModel> attachments = null, string changeContent = null)
        {
            if (entity?.Id == default(int))
            {
                throw new ArgumentNullException("Year is required");
            }
            if (id.Equals(default(TId)))
            {
                throw new ArgumentNullException(nameof(id));
            }

            if (!entity.Id.ToString().Equals(id))
            {
                throw new ArgumentException("Invalid Year Id");
            }

            var yearFromDb = await FindAsync(id);
            if (yearFromDb == null)
            {
                throw new NoRecordFound("Year doesn't exists");
            }

            // NOTE: change-request-comments-staging perfomed on base
            
            return await base.SubmitUpdateChangeRequestAsync(entity, id, requestedBy, ChangeType.None, null, comment);
        }

        public override async Task<ChangeRequestStagingModel<Year>> GetChangeRequestStaging<TId>(TId changeRequestId)
        {
            ChangeRequestStagingModel<Year> staging = await
                ChangeRequestBusinessService.GetChangeRequestStagingByChangeRequestIdAsync<Year, TId>(changeRequestId);

            // business specific task.
            // fill value of "EntityCurrent"
            if (!staging.StagingItem.ChangeType.Equals(ChangeType.Add.ToString(), StringComparison.CurrentCultureIgnoreCase))
            {
                Year currentYear = await FindAsync(staging.StagingItem.EntityId);
                staging.EntityCurrent = currentYear;
            }

            return staging;
        }

        public override async Task<long> SubmitDeleteChangeRequestAsync<TId>(Year entity, TId id, string requestedBy, List<ChangeRequestItemStaging> changeRequestItemStagings = null,CommentsStagingModel comment=null, List<AttachmentsModel> attachmentsStaging = null, string changeContent = null)
        {
            if (id.Equals(default(TId)))
            {
                throw new ArgumentNullException(nameof(id));
            }
            var yearFromDb = await FindAsync(id);
            if (yearFromDb == null)
            {
                throw new NoRecordFound("No Year exist");
            }

            changeRequestItemStagings = changeRequestItemStagings ?? new List<ChangeRequestItemStaging>();

          

            changeRequestItemStagings.Add(new ChangeRequestItemStaging()
            {
                EntityId = yearFromDb.Id.ToString(),
                Entity = typeof(Year).Name,
                ChangeType = ChangeType.Delete,
                CreatedDateTime = DateTime.UtcNow,
                Payload = Serializer.Serialize(new Year()
                {
                    Id = yearFromDb.Id
                   
                })
            });

            var changeRequestId = await base.SubmitDeleteChangeRequestAsync(yearFromDb, id, requestedBy, changeRequestItemStagings, comment, attachmentsStaging);
            yearFromDb.ChangeRequestId = changeRequestId;
            _yearRepositoryService.Update(yearFromDb);
            Repositories.SaveChanges();
            return changeRequestId;
        }

        /// <summary>
        /// Find a single record which is not marked as deleted.
        /// </summary>
        /// <typeparam name="TKey">any</typeparam>
        /// <param name="id">the key property</param>
        /// <returns>Single entity object if condition matches, null if not found.</returns>
        private async Task<Year> FindAsync<TKey>(TKey id)
        {
            var entityId = Convert.ToInt32(id);
            var entities = await _yearRepositoryService.GetAsync(m => m.Id == entityId && m.DeleteDate == null, 1);
            if (entities != null && entities.Any())
            {
                return entities.First();
            }
            return null;
        }
    }
}
