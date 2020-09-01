using AutoCare.Product.Application.Models.BusinessModels;
using AutoCare.Product.Vcdb.Model;
using System.Threading.Tasks;

namespace AutoCare.Product.Application.BusinessServices.Vcdb
{
    public interface IBedConfigBusinessService: IVcdbBusinessService<BedConfig>
    {
        new Task<BedConfigChangeRequestStagingModel> GetChangeRequestStaging<TId>(TId changeRequestId);
    }
}
