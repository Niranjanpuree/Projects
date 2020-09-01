using AutoCare.Product.Application.BusinessServices.Vcdb;
using AutoCare.Product.Vcdb.Model;

namespace AutoCare.Product.Application.ApplicationServices
{
    public class VehicleToBrakeConfigApplicationService : VcdbApplicationService<VehicleToBrakeConfig>, IVehicleToBrakeConfigApplicationService
    {
        public VehicleToBrakeConfigApplicationService(IVehicleToBrakeConfigBusinessService vehicleToBrakeConfigBusinessService)
            :base(vehicleToBrakeConfigBusinessService)
        {
        }
    }
}
