using AutoCare.Product.Vcdb.Model;

namespace AutoCare.Product.Application.ApplicationServices
{
    public interface IVcdbChangeRequestService : IChangeRequestService<ChangeRequestStaging, ChangeRequestItemStaging, CommentsStaging, AttachmentsStaging>
    {
    }
}
