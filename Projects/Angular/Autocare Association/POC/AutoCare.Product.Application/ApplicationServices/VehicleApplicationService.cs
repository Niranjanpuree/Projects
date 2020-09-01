using AutoCare.Product.Application.BusinessServices.Vcdb;
using AutoCare.Product.Vcdb.Model;

namespace AutoCare.Product.Application.ApplicationServices
{
    public class VehicleApplicationService : VcdbApplicationService<Vehicle>, IVehicleApplicationService
    {
        public VehicleApplicationService(IVehicleBusinessService vehicleApplicationService)
            : base(vehicleApplicationService)
        {

        }
    }
}
