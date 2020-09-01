namespace AutoCare.Product.Application.BusinessServices
{
    public interface IBusinessServiceRuntimeResolver
    {
        object Resolve(string typeName);
    }
}