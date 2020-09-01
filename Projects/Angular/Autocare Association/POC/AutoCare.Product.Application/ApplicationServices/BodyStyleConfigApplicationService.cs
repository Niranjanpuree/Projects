using AutoCare.Product.Application.BusinessServices.Vcdb;
using AutoCare.Product.Application.Models.BusinessModels;
using AutoCare.Product.Vcdb.Model;
using System.Threading.Tasks;

namespace AutoCare.Product.Application.ApplicationServices
{
    public class BodyStyleConfigApplicationService: VcdbApplicationService<BodyStyleConfig>, IBodyStyleConfigApplicationService
    {
        private readonly IBodyStyleConfigBusinessService _bodyStyleConfigBusinessService = null;
        public BodyStyleConfigApplicationService(IBodyStyleConfigBusinessService bodyStyleConfigBusinessService)
            :base(bodyStyleConfigBusinessService)
        {
            _bodyStyleConfigBusinessService = bodyStyleConfigBusinessService;
        }

        public new async Task<BodyStyleConfigChangeRequestStagingModel> GetChangeRequestStaging<TId>(TId changeRequestId)
        {
            return await _bodyStyleConfigBusinessService.GetChangeRequestStaging(changeRequestId);
        }
    }
}

