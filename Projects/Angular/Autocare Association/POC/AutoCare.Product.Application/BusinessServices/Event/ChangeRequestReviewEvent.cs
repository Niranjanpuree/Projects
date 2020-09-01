using AutoCare.Product.Infrastructure.Bus.EventBus;
using AutoCare.Product.Vcdb.Model;

namespace AutoCare.Product.Application.BusinessServices.Event
{
    public abstract class ChangeRequestReviewEvent: IEvent
    {
        public string Entity { get; set; }
        public string Payload { get; set; }
        public ChangeType ChangeType { get; set; }
        public string EntityId { get; set; }
        public long ChangeRequestId { get; set; }
    }
}
