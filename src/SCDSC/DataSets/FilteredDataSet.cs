using SCDSC.Schemas;
using System.Collections.Generic;
using System.Linq;

namespace SCDSC.DataSets
{
    /// <summary>
    /// This data set returns a schema, and rows that only include the fields
    /// requested by data studio. Returning anything else will cause data studio
    /// to throw an error. Note that the default implementation that fetches data
    /// from sitecore analytics will only fetch the requested fields but does
    /// output a values collection of all fields.
    /// </summary>
    public class FilteredDataSet : IDataSet
    {
        private readonly IEnumerable<IList<object>> _rowData;
        private readonly List<bool> _fieldFlags;

        public FilteredDataSet(IDataStudioSchema schema, IEnumerable<IList<object>> rowData, IEnumerable<string> fieldsToKeep)
        {
            _rowData = rowData;
            _fieldFlags = new List<bool>();

            var fields = new List<SchemaField>();

            // remove any fields not explicitly requested
            // keeping track of the indexes that we removed
            for (var i = 0; i < schema.Schema.Count; i++)
            {
                var field = schema.Schema[i];

                if (fieldsToKeep.Contains(field.Name))
                {
                    fields.Add(field);
                    _fieldFlags.Add(true);
                }
                else
                {
                    _fieldFlags.Add(false);
                }
            }

            Schema = fields;
        }

        public bool CachedData => false;

        public IEnumerable<SchemaField> Schema { get; }

        public IEnumerable<DataSetRow> Rows
        {
            get
            {
                return _rowData.Select(data =>
                {
                    var row = new DataSetRow();

                    for (var i = 0; i < data.Count; i++)
                    {
                        if (_fieldFlags[i])
                        {
                            row.Values.Add(data[i]);
                        }
                    }

                    return row;
                });
            }
        }
    }
}