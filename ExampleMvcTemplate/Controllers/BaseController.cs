namespace ExampleMvcTemplate.Controllers
{
    using System.Linq;
    using System.Security.Policy;
    using System.Web.Mvc;
    using ActionResults;
    using Newtonsoft.Json;

    public abstract class BaseController : Controller
    {
        protected StandardJsonResult JsonValidationError()
        {
            var result = new StandardJsonResult();

            foreach (var validationError in ModelState.Values.SelectMany(v => v.Errors))
            {
                result.AddError(validationError.ErrorMessage);
            }
            return result;
        }

        protected StandardJsonResult JsonError(string errorMessage)
        {
            var result = new StandardJsonResult();

            result.AddError(errorMessage);

            return result;
        }

        protected StandardJsonResult<T> JsonSuccess<T>(T data,
            JsonRequestBehavior behavior = JsonRequestBehavior.AllowGet)
        {
            return new StandardJsonResult<T> { Data = data, JsonRequestBehavior = behavior };
        }

        /// <summary>
        /// Supports the Ajax Based Validation. If not in an Ajax call, this
        /// method will perform a normal Redirect to Action. Otherwise, 
        /// it will build up a Content Result with a Redirect Parameter
        /// </summary>
        /// <param name="actionName">The action to redirect to</param>
        /// <returns></returns>
        public new ActionResult RedirectToAction(string actionName)
        {
            if (IsAjaxRequest())
                return RedirectToActionJson(actionName);
            return base.RedirectToAction(actionName);
        }
        /// <summary>
        /// Supports the Ajax Based Validation. If not in an Ajax call, this
        /// method will perform a normal Redirect to Action. Otherwise, 
        /// it will build up a Content Result with a Redirect Parameter
        /// </summary>
        /// <param name="actionName">The Action to redirect to</param>
        /// <param name="controllerName">The Controller to redirect to</param>
        /// <returns></returns>
        public new ActionResult RedirectToAction(string actionName, string controllerName)
        {
            if (IsAjaxRequest())
                return RedirectToActionJson(actionName, controllerName);

            return base.RedirectToAction(actionName, controllerName);
        }

        /// <summary>
        /// Supports the Ajax Based Validation. If not in an Ajax call, this
        /// method will perform a normal Redirect to Action. Otherwise, 
        /// it will build up a Content Result with a Redirect Parameter 
        /// </summary>
        /// <param name="actionName">The action to redirect to</param>
        /// <param name="controllerName">The Controller to redirect to</param>
        /// <param name="values">Route Parameter collection.</param>
        /// <returns></returns>
        public new ActionResult RedirectToAction(string actionName, string controllerName, object values)
        {
            if (IsAjaxRequest())
                return RedirectToActionJson(actionName, controllerName, values);

            return base.RedirectToAction(actionName, controllerName, values);
        }

        protected bool IsAjaxRequest()
        {
            var isAjax = Request.Headers["X-Requested-With"] == "XMLHttpRequest";
            return isAjax;
        }

        private ContentResult RedirectToActionJson(string actionName)
        {
            return JsonNet(new
                {
                    redirect = Url.Action(actionName)
                }
            );
        }

        private ContentResult RedirectToActionJson(string actionName, string actionController)
        {
            return JsonNet(new
                {
                    redirect = Url.Action(actionName, actionController)
                }
            );
        }

        private ContentResult RedirectToActionJson(string action, string controller, object values)
        {
            return JsonNet(new
                {
                    redirect = Url.Action(action, controller, values)
                }
            );
        }

        protected ContentResult JsonNet(object model)
        {
            var serialized = JsonConvert.SerializeObject(model, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });

            return new ContentResult
            {
                Content = serialized,
                ContentType = "application/json"
            };
        }
    }
}