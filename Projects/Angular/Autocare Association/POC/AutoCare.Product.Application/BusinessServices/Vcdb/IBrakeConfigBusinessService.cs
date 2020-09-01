using AutoCare.Product.Application.Models.BusinessModels;
using AutoCare.Product.Vcdb.Model;
using System.Threading.Tasks;

namespace AutoCare.Product.Application.BusinessServices.Vcdb
{
    public interface IBrakeConfigBusinessService : IVcdbBusinessService<BrakeConfig>
    {
        new Task<BrakeConfigChangeRequestStagingModel> GetChangeRequestStaging<TId>(TId changeRequestId);
    }
}