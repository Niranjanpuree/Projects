using AutoCare.Product.Vcdb.Model;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoCare.Product.Application.Models.BusinessModels;

namespace AutoCare.Product.Application.BusinessServices
{
    public interface IBusinessService<T>
        where T : class
    {
        Task<List<T>> GetAllAsync(int topCount = 0);
        Task<T> GetAsync<TKey>(TKey id);
        Task<List<T>> GetAsync(Expression<Func<T, bool>> whereCondition, int topCount = 0);
        Task<bool> AddAsync(T entity);
        Task<bool> AddAllAsync(List<T> entities);
        Task<bool> UpdateAsync<TId>(T entity);
        Task<bool> RemoveAsync<TKey>(TKey id);
        Task<int> GetCountAsync<TElement>(T entity, Expression<Func<T, ICollection<TElement>>> collectionProperty)
            where TElement : class;

        Task<ChangeRequestStagingModel<T>> GetChangeRequestStaging<TId>(TId changeRequestId);
        Task<List<T>> GetPendingAddChangeRequests(Expression<Func<T, bool>> whereCondition = null, int topCount = 0);
        Task<bool> SubmitChangeRequestReviewAsync<TId>(TId changeRequestId, ChangeRequestReviewModel review);

        Task<List<ChangeEntityFacet>> GetAllChangeTypes();

        Task<long> ChangeRequestExist<TId>(T entity, TId id);

        Task<bool> SubmitLikeAsync(long id, string likedBy, string likedStatus);
        
    }
}
 