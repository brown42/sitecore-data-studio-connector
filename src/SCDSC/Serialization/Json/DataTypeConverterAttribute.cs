using Newtonsoft.Json;
using SCDSC.Schemas;
using System;

namespace SCDSC.Serialization.Json
{
    public class DataTypeConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var dataType = value as DataType?;

            if (!dataType.HasValue)
            {
                writer.WriteNull();
                return;
            }

            writer.WriteValue(dataType.Value.ToString().ToUpperInvariant());
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            // not used at the moment...
            throw new NotImplementedException();
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(DataType) == objectType;
        }
    }
}