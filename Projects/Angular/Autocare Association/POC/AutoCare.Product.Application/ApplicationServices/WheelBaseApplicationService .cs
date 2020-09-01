using AutoCare.Product.Application.BusinessServices.Vcdb;
using AutoCare.Product.Application.Models.BusinessModels;
using AutoCare.Product.Vcdb.Model;
using System.Threading.Tasks;

namespace AutoCare.Product.Application.ApplicationServices
{
    public class WheelBaseApplicationService : VcdbApplicationService<WheelBase>, IWheelBaseApplicationService
    {
        private readonly WheelBaseBusinessService _wheelBaseBusinessService = null;
        public WheelBaseApplicationService(WheelBaseBusinessService wheelBaseBusinessService)
            : base(wheelBaseBusinessService)
        {
            _wheelBaseBusinessService = wheelBaseBusinessService;
        }
        public new async Task<WheelBaseChangeRequestStagingModel> GetChangeRequestStaging<TId>(TId changeRequestId)
        {
            return await _wheelBaseBusinessService.GetChangeRequestStaging(changeRequestId);
        }
    }
}
