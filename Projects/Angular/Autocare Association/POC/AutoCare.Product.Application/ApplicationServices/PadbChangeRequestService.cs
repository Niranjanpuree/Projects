using AutoCare.Product.Application.BusinessServices.Padb;

namespace AutoCare.Product.Application.ApplicationServices
{
    // todo: note: use concrete classes of staging, item & comments for pcdb
    // todo: note: remove abstract property 
    public abstract class PadbChangeRequestService<T, TItem, TComment, TAttachment> : ChangeRequestService<T, TItem, TComment, TAttachment>, IPadbChangeRequestService<T, TItem, TComment, TAttachment>
        where T : class
        where TItem : class
        where TComment : class
        where TAttachment : class
    {
        public PadbChangeRequestService(IPadbChangeRequestBusinessService<T, TItem, TComment, TAttachment> changeRequestBusinessService)
            : base(changeRequestBusinessService)
        {
        }
    }
}
