using SCDSC.DataSets;
using SCDSC.Pipelines.GetSchema;
using SCDSC.Schemas;
using Sitecore.Data;
using Sitecore.ExperienceAnalytics.Api;
using Sitecore.ExperienceAnalytics.Api.Encoding;
using Sitecore.ExperienceAnalytics.Api.Query;
using Sitecore.ExperienceAnalytics.Api.Response;
using Sitecore.SecurityModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SCDSC.Pipelines.GetData
{
    public class GetDataFromDatabase
    {
        private readonly IReportingService _reportingService = ApiContainer.Repositories.GetReportingService();
        private readonly IEncoder<ReportResponse> _encoder = ApiContainer.GetReportResponseEncoder();
        private readonly ITextCodec _decoder = ApiContainer.GetKeyCodec();

        public void Process(GetDataPipelineArgs args)
        {
            var manager = DataStudioConnectorManager.Create();
            var model = manager.GetDataSetItemModel(args.Request.DataSetId);

            if (model?.Segments == null || model.Segments.Count == 0)
            {
                return;
            }

            var schema = GetSchema(args);
            var rows = model.Segments.SelectMany(segment => GetSegmentData(args, model, segment));
            var dataSet = new FilteredDataSet(schema, rows, args.Request.Fields);
            args.Result = dataSet;
        }

        private IDataStudioSchema GetSchema(GetDataPipelineArgs args)
        {
            var getSchemaArgs = new GetSchemaPipelineArgs
            {
                Request = args.Request
            };

            GetSchemaPipeline.Run(getSchemaArgs);

            return getSchemaArgs.Result;
        }

        private IEnumerable<object[]> GetSegmentData(GetDataPipelineArgs args, DataSetItemModel dataSet, ID segmentId)
        {
            using (new SecurityDisabler())
            {
                var query = new ReportQuery
                {

                    Segments = new[] { segmentId.ToString() },
                    Keys = new[] { _decoder.Decode("all") },
                    Fields = args.Request.Fields.ToArray(),
                    Parameters =
                    {
                        KeyTop = dataSet.MaxKeysPerSegment,
                        KeyOrderBy = new FieldSort
                        {
                            Direction = SortDirection.Desc,
                            Field = SortField.Visits
                        },
                        DateFrom = args.Request.StartDate ?? DateTime.Now.AddDays(-30),
                        DateTo = args.Request.EndDate ?? DateTime.Now,
                        TimeResolution = dataSet.TimeResolution
                    }
                };

                var response = _encoder.Encode(_reportingService.RunQuery(query));
                string TranslateKey(string fieldName, string value) => response.Data.Localization.Fields.
                    SingleOrDefault(f => f.Field == fieldName)?.Translations[value];

                var rows = response.Data.Content.Select(row => new object[]
                {
                    TranslateKey("segment", row.Segment),
                    row.DateLabel,
                    row.Date?.ToString("yyyyMMdd"),
                    TranslateKey("key", row.Key),
                    row.Visits,
                    row.Value,
                    row.Conversions,
                    row.TimeOnSite,
                    row.PageViews,
                    row.Bounces,
                    row.Count,
                    row.ValuePerVisit,
                    row.BounceRate,
                    row.AvgVisitDuration,
                    row.ConversionRate,
                    row.AvgVisitPageViews,
                    row.AvgVisitCount,
                    row.AvgPageCount,
                    row.AvgCountValue
                });

                return rows;
            }
        }
    }
}