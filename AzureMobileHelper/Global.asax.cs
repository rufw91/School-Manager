using System.Web.Http;
using System.Web.Routing;

namespace AzureMobileHelper
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            WebApiConfig.Register();
        }
    }
}