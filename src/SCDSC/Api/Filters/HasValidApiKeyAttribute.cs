using System;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace SCDSC.Api.Filters
{
    public class HasValidApiKeyAttribute : AuthorizeAttribute
    {
        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            var manager = DataStudioConnectorManager.Create();
            var model = manager.GetConnectorItemModel();
            var querystring = actionContext.Request.GetQueryNameValuePairs();
            var receivedKey = querystring.SingleOrDefault(x => x.Key == ApiParameters.Key).Value;

            if (model.AllowEmptyApiKey && string.IsNullOrWhiteSpace(model.ApiKey))
            {
                return true;
            }

            return string.Equals(model.ApiKey, receivedKey, StringComparison.InvariantCulture);
        }
    }
}