using Newtonsoft.Json;
using SCDSC.Serialization.Json;

namespace SCDSC.Schemas
{
    public class SchemaField
    {
        public string Name { get; set; }
        public string Label { get; set; }
        [JsonConverter(typeof(DataTypeConverter))]
        public DataType DataType { get; set; }
        public Semantics Semantics { get; set; } = new Semantics();
    }
}