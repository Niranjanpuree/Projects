namespace AutoCare.Product.Infrastructure.Serializer
{
    public interface ITextSerializer
    {
        string Serialize(object objectToSerialize);
        T Deserialize<T>(string payload);
    }
}