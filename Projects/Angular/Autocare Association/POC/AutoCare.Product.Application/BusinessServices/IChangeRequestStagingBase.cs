using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AutoCare.Product.Application.BusinessServices
{
    public interface IChangeRequestStagingBase<T>
        where T: class
    {
        Task<List<T>> GetAllAsync(int topCount = 0);
        Task<T> GetAsync<TKey>(TKey id);
        Task<List<T>> GetAsync(Expression<Func<T, bool>> whereCondition, int topCount = 0);
        void Add(T entity);
        void Update<TId>(T entity);
        Task<T> RemoveAsync<TKey>(TKey id);
        Task<int> SaveChangesAsync();
    }
}
