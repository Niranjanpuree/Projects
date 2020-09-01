using System.Collections.Generic;
using System.Threading.Tasks;
using AutoCare.Product.Vcdb.Model;
using System;
using System.Linq.Expressions;

namespace AutoCare.Product.Application.BusinessServices
{
    public interface IChangeRequestItemBusinessService<T>: IChangeRequestStagingBase<T>
        where T:class
    {
        Task<T> AddAsync<TEntity, TId>(TEntity entity, TId id, string requestedBy, ChangeType changetype);
        Task<List<T>> GetAllChangeRequestItemsAsync(int topCount = 0);
        Task<List<T>> GetChangeRequestItemsAsync(
            Expression<Func<T, bool>> whereCondition, int topCount = 0);
        Task<long> ChangeRequestItemExistAsync<TEntity, TId>(TEntity entity, TId id);
        Task<long> ChangeRequestItemExistAsync<TId>(string entityName, TId id);
        Task<long> ChangeRequestItemExistAsync<TEntity>(Expression<Func<TEntity, bool>> whereCondition);
    }
}
