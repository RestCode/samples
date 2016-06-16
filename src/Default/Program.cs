using System;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Owin.Hosting;
using Owin;
using WebApiProxy.Server;

namespace Default
{
    class Program
    {
        static void Main(string[] args)
        {

            var baseAddr = "http://localhost:8888";
            using (var server = WebApp.Start<Startup>(baseAddr))
            {
                Console.WriteLine("hello");

                var client = new HttpClient {BaseAddress = new Uri(baseAddr)};

                var res = client.GetAsync("api/test/GetFromSimpleArg?id=world").Result.Content.ReadAsStringAsync().Result;
                Console.WriteLine(res);


                Console.WriteLine("---------------done!");
                Console.ReadLine();
            }

        }
    }


    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();
            config.RegisterProxyRoutes();
            config.MapHttpAttributeRoutes();
            app.UseWebApi(config);
        }
    }
}