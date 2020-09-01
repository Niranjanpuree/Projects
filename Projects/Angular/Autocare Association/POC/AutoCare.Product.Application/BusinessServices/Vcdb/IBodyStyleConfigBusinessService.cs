using AutoCare.Product.Application.Models.BusinessModels;
using AutoCare.Product.Vcdb.Model;
using System.Threading.Tasks;

namespace AutoCare.Product.Application.BusinessServices.Vcdb
{
    public interface IBodyStyleConfigBusinessService: IVcdbBusinessService<BodyStyleConfig>
    {
        new Task<BodyStyleConfigChangeRequestStagingModel> GetChangeRequestStaging<TId>(TId changeRequestId);
    }
}
