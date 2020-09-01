using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AutoCare.Product.Infrastructure.Serializer;
using AutoCare.Product.Vcdb.Model;

namespace AutoCare.Product.Application.RepositoryServices
{
    public class ModelSqlServerEfRepositoryService : VcdbSqlServerEfRepositoryService<Model>, IModelSqlServerEfRepositoryService
    {
        public ModelSqlServerEfRepositoryService(DbContext context, ITextSerializer serializer) : base(context, serializer)
        {
        }

        public async Task<int> GetDependentVehiclesCount(int modelId)
        {
            return await base.GetAllQueryable()
                .Where(x => x.Id == modelId)
                .Include("BaseVehicles.Vehicles")
                .SelectMany(x => x.BaseVehicles)
                .SelectMany(x => x.Vehicles)
                .CountAsync();
        }
    }
}
