using Sitecore.Configuration;

namespace SCDSC
{
    public static class DataStudioConnectorSettings
    {
        public static string Database => Settings.GetSetting("SCDSC.Database", "master");
        public static string RootPath => Settings.GetSetting("SCDSC.RootPath", "/sitecore/system/marketing control panel/Data Studio Connector");
    }
}