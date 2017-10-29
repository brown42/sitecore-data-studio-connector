using SCDSC.DataSets;
using SCDSC.Models;
using Sitecore.Pipelines;

namespace SCDSC.Pipelines.GetData
{
    public class GetDataPipelineArgs : ResultPipelineArgs<IDataSet>
    {
        public DataStudioConnectorRequest Request { get; set; }
    }
}