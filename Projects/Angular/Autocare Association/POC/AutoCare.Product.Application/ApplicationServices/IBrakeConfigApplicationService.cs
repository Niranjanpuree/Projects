using AutoCare.Product.Application.Models.BusinessModels;
using AutoCare.Product.Vcdb.Model;
using System.Threading.Tasks;

namespace AutoCare.Product.Application.ApplicationServices
{
    public interface IBrakeConfigApplicationService: IVcdbApplicationService<BrakeConfig>
    {
        new Task<BrakeConfigChangeRequestStagingModel> GetChangeRequestStaging<TId>(TId changeRequestId);
    }
}
