using AutoCare.Product.Application.Models.BusinessModels;
using AutoCare.Product.Vcdb.Model;
using System.Threading.Tasks;

namespace AutoCare.Product.Application.ApplicationServices
{
    public interface IBaseVehicleApplicationService: IVcdbApplicationService<BaseVehicle>
    {
        new Task<BaseVehicleChangeRequestStagingModel> GetChangeRequestStaging<TId>(TId changeRequestId);
    }
}
