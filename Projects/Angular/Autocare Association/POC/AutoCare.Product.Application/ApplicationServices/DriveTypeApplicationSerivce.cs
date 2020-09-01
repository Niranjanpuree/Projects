using AutoCare.Product.Application.BusinessServices.Vcdb;
using AutoCare.Product.Application.Models.BusinessModels;
using AutoCare.Product.Vcdb.Model;
using System.Threading.Tasks;

namespace AutoCare.Product.Application.ApplicationServices
{
    public class DriveTypeApplicationService : VcdbApplicationService<DriveType>, IDriveTypeApplicationService
    {
        private readonly IDriveTypeBusinessService _driveTypeBusinessService = null;
        public DriveTypeApplicationService(IDriveTypeBusinessService driveTypeBusinessService)
            :base(driveTypeBusinessService)
        {
            _driveTypeBusinessService = driveTypeBusinessService;
        }
        public new async Task<DriveTypeChangeRequestStagingModel> GetChangeRequestStaging<TId>(TId changeRequestId)
        {
            return await _driveTypeBusinessService.GetChangeRequestStaging(changeRequestId);
        }
    }
}
