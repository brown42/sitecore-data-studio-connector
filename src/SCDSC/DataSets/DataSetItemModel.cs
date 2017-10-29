using Sitecore.Data;
using Sitecore.ExperienceAnalytics.Api.Query;
using System.Collections.Generic;

namespace SCDSC.DataSets
{
    public class DataSetItemModel
    {
        public int MaxKeysPerSegment { get; set; } = 5;
        public List<ID> Segments { get; } = new List<ID>();
        public TimeResolution TimeResolution { get; set; }
    }
}