using System.Data.Entity;
using AutoCare.Product.Infrastructure.RepositoryService;
using AutoCare.Product.Infrastructure.Serializer;

namespace AutoCare.Product.Application.RepositoryServices
{
    public class PadbSqlServerEfRepositoryService<T> : SqlServerEfRepositoryService<T>, IPadbSqlServerEfRepositoryService<T>
        where T: class
    {
        public PadbSqlServerEfRepositoryService(DbContext context, ITextSerializer serializer) : base(context, serializer)
        {
        }
    }
}
