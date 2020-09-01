using AutoCare.Product.Application.BusinessServices.Vcdb;
using AutoCare.Product.Vcdb.Model;

namespace AutoCare.Product.Application.ApplicationServices
{
    public class VehicleToMfrBodyCodeApplicationService : VcdbApplicationService<VehicleToMfrBodyCode>, IVehicleToMfrBodyCodeApplicationService
    {
        public VehicleToMfrBodyCodeApplicationService(IVehicleToMfrBodyCodeBusinessService vehicleToMfrBodyCodeBusinessService)
            :base(vehicleToMfrBodyCodeBusinessService)
        {
        }
    }
}
