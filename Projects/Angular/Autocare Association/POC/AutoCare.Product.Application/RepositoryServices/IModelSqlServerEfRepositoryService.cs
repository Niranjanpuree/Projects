using System.Threading.Tasks;
using AutoCare.Product.Vcdb.Model;

namespace AutoCare.Product.Application.RepositoryServices
{
    public interface IModelSqlServerEfRepositoryService : IVcdbSqlServerEfRepositoryService<Model>
    {
        Task<int> GetDependentVehiclesCount(int modelId);
    }
}
