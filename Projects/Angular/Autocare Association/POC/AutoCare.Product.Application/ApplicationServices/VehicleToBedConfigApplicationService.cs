using AutoCare.Product.Application.BusinessServices.Vcdb;
using AutoCare.Product.Vcdb.Model;

namespace AutoCare.Product.Application.ApplicationServices
{
    public class VehicleToBedConfigApplicationService : VcdbApplicationService<VehicleToBedConfig>, IVehicleToBedConfigApplicationService
    {
        public VehicleToBedConfigApplicationService(IVehicleToBedConfigBusinessService vehicleToBedConfigBusinessService)
            :base(vehicleToBedConfigBusinessService)
        {
        }
    }
}
