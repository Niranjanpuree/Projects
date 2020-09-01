using AutoCare.Product.Application.BusinessServices.Vcdb;
using AutoCare.Product.Vcdb.Model;

namespace AutoCare.Product.Application.ApplicationServices
{
    public class VehicleTypeApplicationService : VcdbApplicationService<VehicleType>, IVehicleTypeApplicationService
    {
        public VehicleTypeApplicationService(IVehicleTypeBusinessService vehicleTypeBusinessService)
            :base(vehicleTypeBusinessService)
        {
        }
    }
}
