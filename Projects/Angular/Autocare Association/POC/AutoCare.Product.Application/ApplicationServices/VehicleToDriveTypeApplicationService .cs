using AutoCare.Product.Application.BusinessServices.Vcdb;
using AutoCare.Product.Vcdb.Model;

namespace AutoCare.Product.Application.ApplicationServices
{
    public class VehicleToDriveTypeApplicationService : VcdbApplicationService<VehicleToDriveType>, IVehicleToDriveTypeApplicationService
    {
        public VehicleToDriveTypeApplicationService(IVehicleToDriveTypeBusinessService vehicleToDriveTypeBusinessService)
            :base(vehicleToDriveTypeBusinessService)
        {
        }
    }
}
