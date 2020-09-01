using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AutoCare.Product.Application.RepositoryServices
{
    public interface IRepositoryService<T>
        where T: class
    {
        Task<List<T>> GetAllAsync(int topCount = 0);
        Task<List<T>> GetAsync(Expression<Func<T, bool>> whereCondition);
        Task<T> FindAsync<TId>(TId id);
        void Add(T entity);
        void AddAll(List<T> entities);
        void Update(T entity);
        Task<IEnumerable<T>> RemoveAsync(Expression<Func<T, bool>> whereCondition);
        Task<T> RemoveAsync<TKey>(TKey id);
        //Task<ChangeRequestStaging> BuildChangeRequestStagingAsync(T entity, string requestedBy); 
        //Task<ChangeRequestStaging> BuildChangeRequestStaging(T entity, string requestedBy);
    }
}
