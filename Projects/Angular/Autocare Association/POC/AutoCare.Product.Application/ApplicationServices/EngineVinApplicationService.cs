using AutoCare.Product.Application.BusinessServices.Vcdb;
using AutoCare.Product.Vcdb.Model;

namespace AutoCare.Product.Application.ApplicationServices
{
    public class EngineVinApplicationService : VcdbApplicationService<EngineVin>, IEngineVinApplicationService
    {
        public EngineVinApplicationService(IEngineVinBusinessService engineVinBusinessService)
            : base(engineVinBusinessService)
        {
        }
    }
}
