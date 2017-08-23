namespace ExampleMvcTemplate.Models.Helpers
{
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Mvc.Html;

    public static class HtmlHelperExtensions
    {
        public static IHtmlString BootstrapLabel(
            this HtmlHelper helper,
            string propertyName, int length = 2)
        {
            return helper.Label(propertyName, new
            {
                @class = $"col-md-{length} control-label"
            });
        }
    }
}