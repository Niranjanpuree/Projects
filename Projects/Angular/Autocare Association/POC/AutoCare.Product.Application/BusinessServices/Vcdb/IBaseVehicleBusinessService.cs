using AutoCare.Product.Application.Models.BusinessModels;
using AutoCare.Product.Vcdb.Model;
using System.Threading.Tasks;

namespace AutoCare.Product.Application.BusinessServices.Vcdb
{
    public interface IBaseVehicleBusinessService : IVcdbBusinessService<BaseVehicle>
    {
        new Task<BaseVehicleChangeRequestStagingModel> GetChangeRequestStaging<TId>(TId changeRequestId);
    }
}