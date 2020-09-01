using System;
using System.Threading.Tasks;

namespace AutoCare.Product.Application.RepositoryServices
{
    public interface IUnitOfWork
    {
        IRepositoryService<T> GetRepositoryService<T>() where T : class;
        object GetRepositoryService(Type entityType);
        int SaveChanges();
        Task<int> SaveChangesAsync();
    }
}
