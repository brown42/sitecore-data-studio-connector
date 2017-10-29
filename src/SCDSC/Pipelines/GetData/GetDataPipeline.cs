using Sitecore.Pipelines;

namespace SCDSC.Pipelines.GetData
{
    public static class GetDataPipeline
    {
        public static void Run(GetDataPipelineArgs args)
        {
            CorePipeline.Run("getData", args, "dataStudio", true);
        }
    }
}