namespace AutoCare.Product.Application.BusinessServices.Padb
{
    // todo: note: use concrete classes of staging, item & comments for pcdb
    public interface IPadbChangeRequestBusinessService<T, TItem, TComment, TAttachment> : IChangeRequestBusinessService<T, TItem, TComment, TAttachment>
        where T:class
        where TItem: class
        where TComment: class
        where TAttachment : class
    {
    }
}
