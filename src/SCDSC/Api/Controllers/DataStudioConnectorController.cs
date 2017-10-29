using Newtonsoft.Json.Linq;
using SCDSC.Api.Filters;
using SCDSC.Api.ModelBinders;
using SCDSC.Models;
using SCDSC.Pipelines.GetData;
using SCDSC.Pipelines.GetSchema;
using Sitecore.Diagnostics;
using System.Web.Http;
using System.Web.Http.ModelBinding;

namespace SCDSC.Api.Controllers
{
    [HasValidApiKey]
    [IsEnabled]
    [CamelCaseControllerConfiguration]
    [RoutePrefix("-/api/datastudio")]
    public class DataStudioConnectorController : ApiController
    {
        [HttpGet]
        [Route("schema")]
        public IHttpActionResult GetSchema([ModelBinder(typeof(DataStudioConnectorRequestModelBinder))] DataStudioConnectorRequest request)
        {
            Log.Debug($"[DataStudioConnector] GetSchema() Request \n{JObject.FromObject(request)}\n", this);

            var args = new GetSchemaPipelineArgs
            {
                Request = request
            };

            GetSchemaPipeline.Run(args);

            Log.Debug($"[DataStudioConnector] GetSchema() Response \n{JObject.FromObject(args.Result)}\n", this);

            return Ok(args.Result);
        }

        [HttpGet]
        [Route("data")]
        public IHttpActionResult GetData([ModelBinder(typeof(DataStudioConnectorRequestModelBinder))] DataStudioConnectorRequest request)
        {
            Log.Debug($"[DataStudioConnector] GetData() Request \n\n{JObject.FromObject(request)}", this);

            var args = new GetDataPipelineArgs
            {
                Request = request
            };

            GetDataPipeline.Run(args);

            Log.Debug($"[DataStudioConnector] GetData() Response \n{JObject.FromObject(args.Result)}\n", this);

            return Ok(args.Result);
        }
    }
}
