using AutoCare.Product.Vcdb.Model;
using System.Threading.Tasks;
using AutoCare.Product.Application.BusinessServices.Event;

namespace AutoCare.Product.Application.BusinessServices
{
    public interface IChangeRequestReviewEventHandler
    {
        bool Handle(ChangeRequestApprovedEvent changeRequestApprovedEvent);
        bool Handle(ChangeRequestRejectedEvent changeRequestRejectedEvent);
        bool Handle(ChangeRequestPrelimApprovedEvent changeRequestPrelimApprovedEvent);
        AssociationCount AssociatedCount(string payload);
        Task ClearChangeRequestId(long changeRequestId);
    }
}
