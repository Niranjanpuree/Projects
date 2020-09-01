using AutoCare.Product.Application.BusinessServices.Vcdb;
using AutoCare.Product.Application.Models.BusinessModels;
using AutoCare.Product.Vcdb.Model;
using System.Threading.Tasks;

namespace AutoCare.Product.Application.ApplicationServices
{
    public class BedConfigApplicationService: VcdbApplicationService<BedConfig>, IBedConfigApplicationService
    {
        private readonly IBedConfigBusinessService _bedConfigBusinessService = null;
        public BedConfigApplicationService(IBedConfigBusinessService bedConfigBusinessService)
            :base(bedConfigBusinessService)
        {
            _bedConfigBusinessService = bedConfigBusinessService;
        }

        public new async Task<BedConfigChangeRequestStagingModel> GetChangeRequestStaging<TId>(TId changeRequestId)
        {
            return await _bedConfigBusinessService.GetChangeRequestStaging(changeRequestId);
        }
    }
}

