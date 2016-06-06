using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;

namespace EnjoySample.Restaurant.Command.WebApi
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);

            ContainerBuilder builder = new ContainerBuilder();

            var config = GlobalConfiguration.Configuration;

            AutofacConfig.Configure(builder);

            var container = builder.Build();

            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }
    }
}
