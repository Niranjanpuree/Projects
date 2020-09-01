using AutoCare.Product.Infrastructure.RepositoryService;

namespace AutoCare.Product.Application.RepositoryServices
{
    public interface IVcdbSqlServerEfRepositoryService<T> : IRepositoryService<T>
        where T: class
    {
    }
}
