using System;
using System.Collections.Generic;

namespace SCDSC.Models
{
    public class DataStudioConnectorRequest
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string DataSetId { get; set; }
        public List<string> Fields { get; set; }

        public override string ToString()
        {
            return $"{nameof(StartDate)}: {StartDate}, {nameof(EndDate)}: {EndDate}, {nameof(DataSetId)}: {DataSetId}, {nameof(Fields)}: {Fields}";
        }
    }
}