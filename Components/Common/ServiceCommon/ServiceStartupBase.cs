﻿using System.Web.Http;
using Microsoft.Owin;
using Owin;
using ServiceCommon;

[assembly: OwinStartup(typeof(ServiceStartupBase))]

namespace ServiceCommon
{
    public abstract class ServiceStartupBase
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            var config = new HttpConfiguration();
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            config.Routes.MapHttpRoute(
                name: "ManagementApi",
                routeTemplate: "api/{controller}/{action}"
            );

            appBuilder.UseWebApi(config);
        }
    }
}
