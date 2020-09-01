using AutoCare.Product.Application.BusinessServices.Vcdb;
using AutoCare.Product.Application.Models.BusinessModels;
using AutoCare.Product.Vcdb.Model;
using System.Threading.Tasks;

namespace AutoCare.Product.Application.ApplicationServices
{
    public class BrakeConfigApplicationService : VcdbApplicationService<BrakeConfig>, IBrakeConfigApplicationService
    {
        private readonly IBrakeConfigBusinessService _brakeConfigBusinessService = null;
        public BrakeConfigApplicationService(IBrakeConfigBusinessService brakeConfigBusinessService)
            :base(brakeConfigBusinessService)
        {
            _brakeConfigBusinessService = brakeConfigBusinessService;
        }

        public new async Task<BrakeConfigChangeRequestStagingModel> GetChangeRequestStaging<TId>(TId changeRequestId)
        {
            return await _brakeConfigBusinessService.GetChangeRequestStaging(changeRequestId);
        }
    }
}
