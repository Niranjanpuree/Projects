using AutoCare.Product.Application.BusinessServices.Vcdb;
using AutoCare.Product.Vcdb.Model;

namespace AutoCare.Product.Application.ApplicationServices
{
    public class VehicleToWheelBaseApplicationService: VcdbApplicationService<VehicleToWheelBase>, IVehicleToWheelBaseApplicationService
    {
        public VehicleToWheelBaseApplicationService(IVehicleToWheelBaseBusinessService vehicleToWheelBaseBusinessService)
            :base(vehicleToWheelBaseBusinessService)
        {
        }
    }
}
