using AutoCare.Product.Application.BusinessServices.Vcdb;
using AutoCare.Product.Application.Models.BusinessModels;
using AutoCare.Product.Vcdb.Model;
using System.Threading.Tasks;

namespace AutoCare.Product.Application.ApplicationServices
{
    public class BaseVehicleApplicationService : VcdbApplicationService<BaseVehicle>, IBaseVehicleApplicationService
    {
        private readonly IBaseVehicleBusinessService _baseVehicleBusinessService = null;
        public BaseVehicleApplicationService(IBaseVehicleBusinessService baseVehicleBusinessService)
            :base(baseVehicleBusinessService)
        {
            _baseVehicleBusinessService = baseVehicleBusinessService;
        }

        public new async Task<BaseVehicleChangeRequestStagingModel> GetChangeRequestStaging<TId>(TId changeRequestId) {
            return await _baseVehicleBusinessService.GetChangeRequestStaging(changeRequestId);
        }

    }
}
