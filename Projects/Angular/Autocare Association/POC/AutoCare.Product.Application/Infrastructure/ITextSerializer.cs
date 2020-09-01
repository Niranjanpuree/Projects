namespace AutoCare.Product.Application.Infrastructure
{
    public interface ITextSerializer
    {
        string Serialize(object objectToSerialize);
        object Deserialize(string payload);
    }
}