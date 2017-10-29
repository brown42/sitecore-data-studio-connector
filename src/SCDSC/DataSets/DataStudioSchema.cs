using SCDSC.Schemas;
using System.Collections.Generic;

namespace SCDSC.DataSets
{
    public class DataStudioSchema : IDataStudioSchema
    {
        public IList<SchemaField> Schema { get; set; } = new List<SchemaField>();

        public SchemaField AddDimension(string name, DataType dataType = DataType.String)
        {
            var field = new SchemaField
            {
                Name = name,
                Label = name,
                DataType = dataType,
                Semantics = new Semantics().SetConceptType(ConceptType.Dimension)
            };

            Schema.Add(field);

            return field;
        }

        public SchemaField AddMetric(string name, DataType dataType = DataType.Number, bool isReaggregatable = true)
        {
            var field = new SchemaField
            {
                Name = name,
                Label = name,
                DataType = dataType,
                Semantics = new Semantics().SetConceptType(ConceptType.Metric).SetIsReaggregatable(isReaggregatable)
            };

            Schema.Add(field);

            return field;
        }
    }
}