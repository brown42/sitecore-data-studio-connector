using SCDSC.Models;
using Sitecore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;

namespace SCDSC.Api.ModelBinders
{
    public class DataStudioConnectorRequestModelBinder : IModelBinder
    {
        public bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
        {
            var connectorRequest = new DataStudioConnectorRequest
            {
                DataSetId = bindingContext.ValueProvider.GetValue(ApiParameters.DataSetId).AttemptedValue,
                StartDate = GetDateTimeValue(actionContext, bindingContext, ApiParameters.StartDate),
                EndDate = GetDateTimeValue(actionContext, bindingContext, ApiParameters.EndDate),
                Fields = GetCommaDelimitedValue(actionContext, bindingContext, ApiParameters.Fields, new List<string>())
            };

            bindingContext.Model = connectorRequest;

            return true;
        }

        protected virtual List<string> GetCommaDelimitedValue(HttpActionContext actionContext, ModelBindingContext bindingContext, string key, List<string> defaultValue = null)
        {
            var value = bindingContext.ValueProvider.GetValue(key)?.AttemptedValue;

            if (string.IsNullOrWhiteSpace(value))
            {
                return defaultValue;
            }

            var fields = StringUtil.Split(value, ',', true);
            return fields.ToList();
        }

        protected virtual DateTime? GetDateTimeValue(HttpActionContext actionContext, ModelBindingContext bindingContext, string key, DateTime? defaultValue = null)
        {
            var value = bindingContext.ValueProvider.GetValue(key)?.AttemptedValue;
            return DateTime.TryParse(value, out var result) ?  result : defaultValue;
        }
    }
}