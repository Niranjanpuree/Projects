using AutoCare.Product.Application.BusinessServices.Vcdb;
using AutoCare.Product.Vcdb.Model;

namespace AutoCare.Product.Application.ApplicationServices
{
    public class VcdbChangeRequestService : ChangeRequestService<ChangeRequestStaging, ChangeRequestItemStaging, CommentsStaging, AttachmentsStaging>, IVcdbChangeRequestService
    {
        public VcdbChangeRequestService(IVcdbChangeRequestBusinessService changeRequestBusinessService) 
            : base(changeRequestBusinessService)
        {
        }
    }
}
