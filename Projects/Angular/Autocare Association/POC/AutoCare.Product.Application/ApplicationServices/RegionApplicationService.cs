using AutoCare.Product.Application.BusinessServices.Vcdb;
using AutoCare.Product.Vcdb.Model;

namespace AutoCare.Product.Application.ApplicationServices
{
    public class RegionApplicationService : VcdbApplicationService<Region>, IRegionApplicationService
    {
        public RegionApplicationService(IRegionBusinessService regionBusinessService)
            :base(regionBusinessService)
        {
        }
    }
}
