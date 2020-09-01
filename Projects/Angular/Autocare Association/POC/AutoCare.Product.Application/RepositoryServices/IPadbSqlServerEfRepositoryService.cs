using AutoCare.Product.Infrastructure.RepositoryService;

namespace AutoCare.Product.Application.RepositoryServices
{
    public interface IPadbSqlServerEfRepositoryService<T> : IRepositoryService<T>
        where T: class
    {
    }
}
