using System.Collections.Generic;

namespace SCDSC.Schemas
{
    public interface IDataStudioSchema
    {
        IList<SchemaField> Schema { get; }
    }
}