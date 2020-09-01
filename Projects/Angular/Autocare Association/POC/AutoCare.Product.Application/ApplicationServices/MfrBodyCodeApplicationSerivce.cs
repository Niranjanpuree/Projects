using AutoCare.Product.Application.BusinessServices.Vcdb;
using AutoCare.Product.Application.Models.BusinessModels;
using AutoCare.Product.Vcdb.Model;
using System.Threading.Tasks;

namespace AutoCare.Product.Application.ApplicationServices
{
    public class MfrBodyCodeApplicationService : VcdbApplicationService<MfrBodyCode>, IMfrBodyCodeApplicationService
    {
        private readonly IMfrBodyCodeBusinessService _mfrBodyCodeBusinessService = null;
        public MfrBodyCodeApplicationService(IMfrBodyCodeBusinessService mfrBodyCodeBusinessService)
            :base(mfrBodyCodeBusinessService)
        {
            _mfrBodyCodeBusinessService = mfrBodyCodeBusinessService;
        }
        public new async Task<MfrBodyCodeChangeRequestStagingModel> GetChangeRequestStaging<TId>(TId changeRequestId)
        {
            return await _mfrBodyCodeBusinessService.GetChangeRequestStaging(changeRequestId);
        }
    }
}
