using System.Data.Entity;
using AutoCare.Product.Infrastructure.RepositoryService;
using AutoCare.Product.Infrastructure.Serializer;

namespace AutoCare.Product.Application.RepositoryServices
{
    public class VcdbSqlServerEfRepositoryService<T> : SqlServerEfRepositoryService<T>, IVcdbSqlServerEfRepositoryService<T>
        where T: class
    {
        public VcdbSqlServerEfRepositoryService(DbContext context, ITextSerializer serializer) : base(context, serializer)
        {
        }
    }
}
