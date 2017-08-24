using System.Web;
using System.Web.Mvc;

namespace ExampleMvcTemplate
{
    using Infrastructure.ActionFilters;

    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new ValidatorActionFilter());
        }
    }
}
