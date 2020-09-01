using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoCare.Product.Application.Infrastructure.ExceptionHelpers;
using AutoCare.Product.Application.RepositoryServices;
using AutoCare.Product.Infrastructure.Serializer;
using AutoCare.Product.Vcdb.Model;
using AutoMapper;

namespace AutoCare.Product.Application.BusinessServices.Vcdb
{
    public class VcdbChangeRequestItemBusinessService : ChangeRequestItemBusinessService<ChangeRequestItemStaging>, IVcdbChangeRequestItemBusinessService
    {
        private readonly ITextSerializer _serializer;
        private readonly IVcdbSqlServerEfRepositoryService<ChangeRequestItemStaging> _changeRequestItemStagingRepositoryService;

        public VcdbChangeRequestItemBusinessService(IVcdbUnitOfWork repositories, IMapper autoMapper,
            ITextSerializer serializer) : base(repositories, autoMapper, serializer)
        {
            _serializer = serializer;
            _changeRequestItemStagingRepositoryService = repositories.GetRepositoryService<ChangeRequestItemStaging>()
                as IVcdbSqlServerEfRepositoryService<ChangeRequestItemStaging>;
        }

        public override async Task<List<ChangeRequestItemStaging>> GetChangeRequestItemsAsync(
            Expression<Func<ChangeRequestItemStaging, bool>> whereCondition, int topCount = 0)
        {
            return await this._changeRequestItemStagingRepositoryService.GetAsync(whereCondition, topCount);
        }

        public override async Task<long> ChangeRequestItemExistAsync<TEntity, TId>(TEntity entity, TId id)
        {
            var pendingChangeRequestExist = await this._changeRequestItemStagingRepositoryService.GetAsync(
               x => x.Entity.Equals(typeof(TEntity).Name, StringComparison.CurrentCultureIgnoreCase) &&
                    x.EntityId.Equals(id.ToString(), StringComparison.InvariantCultureIgnoreCase));

            if (pendingChangeRequestExist != null && pendingChangeRequestExist.Any())
            {
                return pendingChangeRequestExist.First().ChangeRequestId;
            }
            return 0;
        }


        public override async Task<long> ChangeRequestItemExistAsync<TId>(string entityName, TId id)
        {
            var pendingChangeRequestExist = await this._changeRequestItemStagingRepositoryService.GetAsync(
                x => x.Entity.Equals(entityName, StringComparison.CurrentCultureIgnoreCase) &&
                     x.EntityId.Equals(id.ToString(), StringComparison.InvariantCultureIgnoreCase));

            if (pendingChangeRequestExist != null && pendingChangeRequestExist.Any())
            {
                return pendingChangeRequestExist.First().ChangeRequestId;
            }
            return 0;
        }

        public override async Task<long> ChangeRequestItemExistAsync<TEntity>(Expression<Func<TEntity, bool>> whereCondition)
        {
            var existingChangeRequestItemStagings = await this._changeRequestItemStagingRepositoryService.GetAsync(
                x => x.Entity.Equals(typeof(TEntity).Name, StringComparison.CurrentCultureIgnoreCase));

            var predicate = whereCondition.Compile();
            var matchingChangeRequestItemStagings = existingChangeRequestItemStagings.Where(changeRequest =>
                new List<TEntity> { _serializer.Deserialize<TEntity>(changeRequest.Payload) }.Any(predicate)).ToList();

            if (matchingChangeRequestItemStagings != null && matchingChangeRequestItemStagings.Any())
            {
                return matchingChangeRequestItemStagings.First().ChangeRequestId;
            }
            return 0;
        }

        public override async Task<ChangeRequestItemStaging> AddAsync<TEntity, TId>(TEntity entity, TId id, string requestedBy, ChangeType changetype)
        {
            if (changetype != ChangeType.Add)
            {
                var pendingChangeRequestExist = await ChangeRequestItemExistAsync(entity, id);
                if (pendingChangeRequestExist > 0)
                {
                    throw new ChangeRequestExistException(
                        $"Change request already exist for table : {typeof(TEntity).Name} with CR ID " + pendingChangeRequestExist + "");
                }
            }

            //prevent duplicate request with same details
            var existingChangeRequestItems = await this._changeRequestItemStagingRepositoryService.GetAsync(item =>
                item.Entity.Equals(typeof(TEntity).Name, StringComparison.CurrentCultureIgnoreCase));

            foreach (var changeRequest in existingChangeRequestItems)
            {
                TEntity plEntity = _serializer.Deserialize<TEntity>(changeRequest.Payload);

                if (base.IsMatch(plEntity, entity))
                {
                    throw new ChangeRequestExistException(
                        $"Matching change request found on {changeRequest.CreatedDateTime}.");
                }
            }

            return new ChangeRequestItemStaging
            {
                ChangeType = changetype,
                CreatedDateTime = DateTime.UtcNow,
                Entity = typeof(TEntity).Name,
                EntityId = id.ToString(),
                Payload = _serializer.Serialize(entity)
            };
        }
    }
}
