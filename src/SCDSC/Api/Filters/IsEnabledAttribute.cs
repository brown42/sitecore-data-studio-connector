using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace SCDSC.Api.Filters
{
    public class IsEnabledAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            HandleActionExecuting(actionContext);
            base.OnActionExecuting(actionContext);
        }

        public override Task OnActionExecutingAsync(HttpActionContext actionContext, CancellationToken cancellationToken)
        {
            HandleActionExecuting(actionContext);
            return base.OnActionExecutingAsync(actionContext, cancellationToken);
        }

        protected virtual void HandleActionExecuting(HttpActionContext actionContext)
        {
            var manager = DataStudioConnectorManager.Create();
            var model = manager.GetConnectorItemModel();

            if (!model.IsEnabled)
            {
                actionContext.Response = new HttpResponseMessage(HttpStatusCode.Forbidden);
            }
        }
    }
}