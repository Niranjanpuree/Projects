using AutoCare.Product.Application.BusinessServices.Vcdb;
using AutoCare.Product.Vcdb.Model;

namespace AutoCare.Product.Application.ApplicationServices
{
    public class VehicleToBodyStyleConfigApplicationService : VcdbApplicationService<VehicleToBodyStyleConfig>, IVehicleToBodyStyleConfigApplicationService
    {
        public VehicleToBodyStyleConfigApplicationService(IVehicleToBodyStyleConfigBusinessService vehicleToBodyStyleConfigBusinessService)
            : base(vehicleToBodyStyleConfigBusinessService)
        {
        }
    }
}
