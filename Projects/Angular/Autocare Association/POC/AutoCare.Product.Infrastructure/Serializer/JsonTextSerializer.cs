using System.IO;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace AutoCare.Product.Infrastructure.Serializer
{
    public class JsonTextSerializer : ITextSerializer
    {
        private readonly JsonSerializer _serializer = null;

        public JsonTextSerializer()
        {
            _serializer = new JsonSerializer();
        }

        public string Serialize(object objectToSerialize)
        {
            if (objectToSerialize == null)
            {
                return null;
            }

            var stringWriter = new StringWriter();
            var jsonWriter = new JsonTextWriter(stringWriter);
#if DEBUG
            jsonWriter.Formatting = Formatting.Indented;
#endif
            _serializer.Serialize(jsonWriter, objectToSerialize);
            stringWriter.Flush();
            return stringWriter.ToString();
        }

        public T Deserialize<T>(string payload)

        {
            var jsonReader = new JsonTextReader(new StringReader(payload));
            try
            {
                return _serializer.Deserialize<T>(jsonReader);
            }
            catch (JsonSerializationException ex)
            {
                throw new SerializationException(ex.Message, ex);
            }
        }
    }
}
