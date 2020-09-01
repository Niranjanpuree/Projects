using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AutoCare.Product.Infrastructure.RepositoryService
{
    public interface IRepositoryService<T>
        where T: class
    {
        Task<List<T>> GetAllAsync(int topCount = 0);
        Task<List<T>> GetAsync(Expression<Func<T, bool>> whereCondition, int topCount = 0, params string[] includeProperties);
        Task<T> FindAsync<TId>(TId id);
        void Add(T entity);
        void AddAll(List<T> entities);
        void Update(T entity);
        Task<IEnumerable<T>> RemoveAsync(Expression<Func<T, bool>> whereCondition);
        Task<T> RemoveAsync<TKey>(TKey id);
        Task<int> GetCountAsync<TElement>(T entity, Expression<Func<T, ICollection<TElement>>> collectionProperty, 
            Expression<Func<TElement, bool>> whereExpression = null)
            where TElement : class;
        T RemoveAsync(T entity);
        //Task<ChangeRequestStaging> BuildChangeRequestStagingAsync(T entity, string requestedBy); 
        //Task<ChangeRequestStaging> BuildChangeRequestStaging(T entity, string requestedBy);
        int GetCountAsync(Expression<Func<T, bool>> whereExpression);
    }
}
