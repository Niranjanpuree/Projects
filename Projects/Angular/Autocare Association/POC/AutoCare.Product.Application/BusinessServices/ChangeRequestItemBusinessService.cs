using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AutoCare.Product.Infrastructure.RepositoryService;
using AutoCare.Product.Infrastructure.Serializer;
using AutoCare.Product.Vcdb.Model;
using AutoMapper;
using System.Linq.Expressions;

namespace AutoCare.Product.Application.BusinessServices
{
    public abstract class ChangeRequestItemBusinessService<T> : ChangeRequestStagingBase<T>, IChangeRequestItemBusinessService<T> 
        where T : class
    {
        private readonly ITextSerializer _serializer;
        private readonly IRepositoryService<T> _changeRequestItemRepositoryService;

        protected ChangeRequestItemBusinessService(IUnitOfWork repositories, IMapper autoMapper, ITextSerializer serializer) 
            : base(repositories, serializer)
        {
            _serializer = serializer;
            _changeRequestItemRepositoryService = repositories.GetRepositoryService<T>();
        }

        public virtual async Task<List<T>> GetAllChangeRequestItemsAsync(int topCount)
        {
            return await _changeRequestItemRepositoryService.GetAllAsync(topCount);
        }

        public virtual async Task<List<T>> GetChangeRequestItemsAsync(Expression<Func<T, bool>> whereCondition, int topCount = 0)
        {
            return await _changeRequestItemRepositoryService.GetAsync(whereCondition, topCount);
        }

        public abstract Task<long> ChangeRequestItemExistAsync<TEntity, TId>(TEntity entity, TId id);

        public abstract Task<long> ChangeRequestItemExistAsync<TId>(string entityName, TId id);

        public abstract Task<long> ChangeRequestItemExistAsync<TEntity>(Expression<Func<TEntity, bool>> whereCondition);

        protected bool IsMatch<TEntity>(TEntity plEntity, TEntity entity)
        {
            var plEntityProperties = plEntity.GetType().GetProperties();
            var entityProperties = entity.GetType().GetProperties();

            foreach (var entityPropertyInfo in entityProperties)
            {
                // if KeyAttribute ignore compare. because the payload can be either from insert, update. which will have diff in keyid.
                if (!entityPropertyInfo.GetCustomAttributes(typeof(KeyAttribute), true).Any())
                {
                    string entitPropertyName = entityPropertyInfo.Name;

                    if (entityPropertyInfo.PropertyType.IsValueType
                        || entityPropertyInfo.PropertyType.IsPrimitive
                        || entityPropertyInfo.PropertyType == typeof(string))
                    {
                        var crEntityPropertyInfo =
                            plEntityProperties.FirstOrDefault(
                                propertyInfo => propertyInfo.Name.Equals(entitPropertyName));

                        if (crEntityPropertyInfo != null)
                        {
                            var crEntityValue = crEntityPropertyInfo.GetValue(plEntity, null);
                            var entityValue = entityPropertyInfo.GetValue(entity, null);
                            if (crEntityValue != null && entityValue != null && !crEntityValue.Equals(entityValue))
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
        }

        public abstract Task<T> AddAsync<TEntity, TId>(TEntity entity, TId id, string requestedBy,
            ChangeType changetype);
    }
}
