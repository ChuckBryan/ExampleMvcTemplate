namespace ExampleMvcTemplate.ActionResults
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using Newtonsoft.Json.Serialization;

    public class StandardJsonResult : JsonResult
    {
        public StandardJsonResult()
        {
            ErrorMessages = new List<string>();
        }

        public IList<string> ErrorMessages { get; private set; }

        public void AddError(string errorMessage)
        {
            ErrorMessages.Add(errorMessage);
        }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            if (JsonRequestBehavior == JsonRequestBehavior.DenyGet &&
                string.Equals(context.HttpContext.Request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException(
                    "GET access is not allowed.  Change the JsonRequestBehavior if you need GET access.");
            }

            HttpResponseBase response = context.HttpContext.Response;
            response.ContentType = string.IsNullOrEmpty(ContentType) ? "application/json" : ContentType;

            if (ContentEncoding != null)
            {
                response.ContentEncoding = ContentEncoding;
            }

            SerializeData(response);
        }

        protected virtual void SerializeData(HttpResponseBase response)
        {
            if (ErrorMessages.Any())
            {
                object originalData = Data;
                Data = new
                {
                    Success = false,
                    OriginalData = originalData,
                    ErrorMessage = string.Join("\n", ErrorMessages),
                    ErrorMessages = ErrorMessages.ToArray()
                };

                response.StatusCode = 400;
            }

            var settings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolverX(),
                DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                Converters = new JsonConverter[]
                {
                    new StringEnumConverter()
                },
            };

            response.Write(JsonConvert.SerializeObject(Data, settings));
        }
    }

    public class StandardJsonResult<T> : StandardJsonResult
    {
        public new T Data
        {
            get { return (T)base.Data; }
            set { base.Data = value; }
        }
    }

    // NOTE: DataTables.NET camelcases nearly everything except for DT_RowId.
    public class CamelCasePropertyNamesContractResolverX : CamelCasePropertyNamesContractResolver
    {
        protected override string ResolvePropertyName(string propertyName)
        {
            // HACK: Expand this with Metadata?
            if (propertyName.Equals("DT_RowId"))
            {
                return propertyName;
            }

            return base.ResolvePropertyName(propertyName);
        }
    }
}