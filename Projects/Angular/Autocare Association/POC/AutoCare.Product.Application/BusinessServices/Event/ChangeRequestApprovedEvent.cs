using AutoCare.Product.Vcdb.Model;

namespace AutoCare.Product.Application.BusinessServices.Event
{
    public class ChangeRequestApprovedEvent : ChangeRequestReviewEvent
    {
        public ChangeRequestStatus ChangeRequestStatus { get; set; }
    }
}
