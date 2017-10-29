using Sitecore.Pipelines;

namespace SCDSC.Pipelines.GetSchema
{
    public static class GetSchemaPipeline
    {
        public static void Run(GetSchemaPipelineArgs args)
        {
            CorePipeline.Run("getSchema", args, "dataStudio", true);
        }
    }
}