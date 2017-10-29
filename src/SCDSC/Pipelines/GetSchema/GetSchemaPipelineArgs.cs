using SCDSC.Models;
using SCDSC.Schemas;
using Sitecore.Pipelines;

namespace SCDSC.Pipelines.GetSchema
{
    public class GetSchemaPipelineArgs : ResultPipelineArgs<IDataStudioSchema>
    {
        public DataStudioConnectorRequest Request { get; set; }
    }
}