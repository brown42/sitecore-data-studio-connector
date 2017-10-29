using Newtonsoft.Json.Serialization;
using System;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web.Http.Controllers;

namespace SCDSC.Api
{
    public class CamelCaseControllerConfigurationAttribute : Attribute, IControllerConfiguration
    {
        public void Initialize(HttpControllerSettings controllerSettings, HttpControllerDescriptor controllerDescriptor)
        {
            var formatter = controllerSettings.Formatters.Single(x => x.GetType() == typeof(JsonMediaTypeFormatter));
            controllerSettings.Formatters.Remove(formatter);

            formatter = new JsonMediaTypeFormatter
            {
                SerializerSettings = { ContractResolver = new CamelCasePropertyNamesContractResolver() }
            };

            controllerSettings.Formatters.Insert(0, formatter);
        }
    }
}