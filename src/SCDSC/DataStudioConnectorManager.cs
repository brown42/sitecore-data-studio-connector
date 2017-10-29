using SCDSC.DataSets;
using SCDSC.Models;
using Sitecore;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Diagnostics;
using Sitecore.ExperienceAnalytics.Api.Query;

namespace SCDSC
{
    public class DataStudioConnectorManager
    {
        private const string DailyTimeResolution = "daily";
        private const string WeeklyTimeResolution = "weekly";
        private const string MonthlyTimeResolution = "monthly";
        private const int DefaultMaxKeysPerSegment = 5;
        private readonly Database _database;

        public static DataStudioConnectorManager Create()
        {
            var database = Database.GetDatabase(DataStudioConnectorSettings.Database);
            return new DataStudioConnectorManager(database);
        }

        public DataStudioConnectorManager(Database database)
        {
            _database = database;
        }

        public DataStudioConnectorItemModel GetConnectorItemModel()
        {
            var item = _database.GetItem(DataStudioConnectorSettings.RootPath);
            var model = new DataStudioConnectorItemModel();

            if (item != null)
            {
                model.IsEnabled = ((CheckboxField)item.Fields["Enabled"]).Checked;
                model.ApiKey = item["API Key"];
                model.AllowEmptyApiKey = ((CheckboxField)item.Fields["Allow Empty API Key"]).Checked;
            }
            else
            {
                Log.Warn($"[DataStudioConnector] Could not load configuration item from path '{DataStudioConnectorSettings.RootPath}'", this);
            }

            return model;
        }

        public DataSetItemModel GetDataSetItemModel(string id)
        {
            var item = _database.GetItem(id);
            var model = new DataSetItemModel();

            if (item != null)
            {
                model.MaxKeysPerSegment = MainUtil.GetInt(item["Max Keys Per Segment"], DefaultMaxKeysPerSegment);
                model.TimeResolution = ReadTimeResolution(StringUtil.GetString(item["Time Resolution"], WeeklyTimeResolution));

                var segmentsField = (MultilistField)item.Fields["Segments"];

                if (segmentsField != null)
                {
                    model.Segments.AddRange(segmentsField.TargetIDs);
                }
                else
                {
                    Log.Warn($"[DataStudio] Could not load segments field from item '{item.Paths.Path}'", this);
                }
            }
            else
            {
                Log.Warn($"[DataStudio] Could not load data set item '{id}'", this);
            }

            return model;
        }

        private TimeResolution ReadTimeResolution(string value)
        {
            switch (value)
            {
                case DailyTimeResolution:
                    return TimeResolution.Daily;

                case WeeklyTimeResolution:
                    return TimeResolution.Weekly;

                case MonthlyTimeResolution:
                    return TimeResolution.Monthly;

                default:
                    return TimeResolution.Weekly;
            }
        }
    }
}