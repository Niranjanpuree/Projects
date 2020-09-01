using AutoCare.Product.Application.Models.BusinessModels;
using AutoCare.Product.Vcdb.Model;
using System.Threading.Tasks;

namespace AutoCare.Product.Application.ApplicationServices
{
    public interface IBodyStyleConfigApplicationService: IVcdbApplicationService<BodyStyleConfig>
    {
        new Task<BodyStyleConfigChangeRequestStagingModel> GetChangeRequestStaging<TId>(TId changeRequestId);
    }
}
