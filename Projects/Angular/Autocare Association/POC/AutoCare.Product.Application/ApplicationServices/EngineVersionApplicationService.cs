using AutoCare.Product.Application.BusinessServices.Vcdb;
using AutoCare.Product.Vcdb.Model;

namespace AutoCare.Product.Application.ApplicationServices
{
    public class EngineVersionApplicationService : VcdbApplicationService<EngineVersion>, IEngineVersionApplicationService
    {
        public EngineVersionApplicationService(IEngineVersionBusinessService engineVersionBusinessService)
            : base(engineVersionBusinessService)
        {
        }
    }
}
