using AutoCare.Product.Application.BusinessServices.Vcdb;
using AutoCare.Product.Vcdb.Model;

namespace AutoCare.Product.Application.ApplicationServices
{
    public class PublicationStageApplicationService : VcdbApplicationService<PublicationStage>, IPublicationStageApplicationService
    {
        public PublicationStageApplicationService(IVcdbBusinessService<PublicationStage> publicationStageApplicationServices)
            : base(publicationStageApplicationServices)
        {
        }
    }
}
