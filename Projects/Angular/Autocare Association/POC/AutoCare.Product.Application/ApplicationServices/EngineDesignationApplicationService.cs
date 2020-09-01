using AutoCare.Product.Application.BusinessServices.Vcdb;
using AutoCare.Product.Vcdb.Model;

namespace AutoCare.Product.Application.ApplicationServices
{
    public class EngineDesignationApplicationService : VcdbApplicationService<EngineDesignation>, IEngineDesignationApplicationService
    {
        public EngineDesignationApplicationService(IEngineDesignationBusinessService engineDesignationBusinessService)
            : base(engineDesignationBusinessService)
        {
        }
    }
}
