namespace AutoCare.Product.Web.ViewModelMappers
{
    public interface IViewModelMapper<in TSource, out TDest>
        where TSource: class
        where TDest: class, new()
    {
        TDest Map(TSource source);
    }
}
