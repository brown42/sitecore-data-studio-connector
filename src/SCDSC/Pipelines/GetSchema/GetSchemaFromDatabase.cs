using SCDSC.DataSets;
using SCDSC.Schemas;
using Sitecore.Data;
using Sitecore.Diagnostics;
using Sitecore.Reflection;

namespace SCDSC.Pipelines.GetSchema
{
    public class GetSchemaFromDatabase
    {
        public void Process(GetSchemaPipelineArgs args)
        {
            var database = Database.GetDatabase(DataStudioConnectorSettings.Database);
            var dataSetItem = database.GetItem(args.Request.DataSetId);

            if (dataSetItem == null)
            {
                Log.Warn($"[DataStudio] Unable to retrieve data set '{args.Request.DataSetId}' from database '{DataStudioConnectorSettings.Database}'", this);
                return;
            }

            args.Result = ReflectionUtil.CreateObject(dataSetItem["Schema Type"]) as DataStudioSchema;

            if (args.Result == null)
            {
                Log.Error($"[DataStudio] {nameof(GetSchemaFromDatabase)} was unable to cast the schema type '{dataSetItem["Schema Type"]}' to '{typeof(DataStudioSchema)}'", this);
            }
        }
    }
}