namespace AutoCare.Product.Application.ApplicationServices
{
    // todo: note: use concrete classes of staging, item & comments for pcdb
    public interface IPadbChangeRequestService<T, TItem, TComment, TAttachment> : IChangeRequestService<T, TItem, TComment, TAttachment>
        where T : class
        where TItem : class
        where TComment : class
        where TAttachment : class
    {
    }
}
