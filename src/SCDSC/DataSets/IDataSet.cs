using SCDSC.Schemas;
using System.Collections.Generic;

namespace SCDSC.DataSets
{
    public interface IDataSet
    {
        IEnumerable<SchemaField> Schema { get; }
        IEnumerable<DataSetRow> Rows { get; }
        bool CachedData { get; }
    }
}