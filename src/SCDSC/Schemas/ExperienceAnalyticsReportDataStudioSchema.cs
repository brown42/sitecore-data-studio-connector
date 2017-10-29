using SCDSC.DataSets;

namespace SCDSC.Schemas
{
    public class ExperienceAnalyticsReportDataStudioSchema : DataStudioSchema
    {
        public ExperienceAnalyticsReportDataStudioSchema()
        {
            AddDimension("Segment");
            AddDimension("DateLabel");
            AddDimension("Date");
            AddDimension("Key");
            AddMetric("Visits");
            AddMetric("Value");
            AddMetric("Conversions");
            AddMetric("TimeOnSite");
            AddMetric("PageViews");
            AddMetric("Bounces");
            AddMetric("Count");
            AddMetric("ValuePerVisit");
            AddMetric("BounceRate");
            AddMetric("AvgVisitDuration");
            AddMetric("ConversionRate");
            AddMetric("AvgVisitPageViews");
            AddMetric("AvgVisitCount");
            AddMetric("AvgPageCount");
            AddMetric("AvgCountValue");
        }
    }
}