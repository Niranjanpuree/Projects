using System;
using System.Threading.Tasks;

namespace AutoCare.Product.Infrastructure.RepositoryService
{
    public interface IUnitOfWork
    {
        IRepositoryService<T> GetRepositoryService<T>() where T : class;
        T GetRepositoryService<T, TModel>() 
            where T : IRepositoryService<TModel> 
            where TModel : class;
        object GetRepositoryService(Type entityType);
        int SaveChanges();
        Task<int> SaveChangesAsync();
    }
}
