using System.Collections.Generic;

namespace SCDSC.Schemas
{
    public class Semantics
    {
        public IDictionary<string, object> Properties { get; set; } = new Dictionary<string, object>();

        public Semantics SetConceptType(ConceptType type)
        {
            Properties["conceptType"] = type.ToString().ToUpperInvariant();
            return this;
        }

        public Semantics SetIsReaggregatable(bool value)
        {
            Properties["isReaggregatable"] = value;
            return this;
        }
    }
}