using AutoCare.Product.Application.BusinessServices.Vcdb;
using AutoCare.Product.Vcdb.Model;

namespace AutoCare.Product.Application.ApplicationServices
{
    public class VehicleTypeGroupApplicationService : VcdbApplicationService<VehicleTypeGroup>, IVehicleTypeGroupApplicationService
    {
        public VehicleTypeGroupApplicationService(IVehicleTypeGroupBusinessService vehicleTypeGroupBusinessService)
            :base(vehicleTypeGroupBusinessService)
        {
        }
    }
}
