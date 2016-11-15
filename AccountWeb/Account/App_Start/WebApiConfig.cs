using Account.Filters;
using Account.Infrustrues;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Account
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API 配置和服务

            // Web API 路由
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Formatters.Remove(config.Formatters.XmlFormatter);
            //config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            config.Filters.Add(new CustomExceptionFilterAttribute());

            //处理DateTime类型序列化后含有T的问题
            config.Formatters.JsonFormatter.SerializerSettings.Converters.Insert(0, new JsonDateTimeConverter());

            //允许跨域
            var cors = new EnableCorsAttribute("http://localhost:1500,http://localhost:2581,http://localhost:3722", "*", "*");
            config.EnableCors(cors);
        }
    }
}
