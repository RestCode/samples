using System;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;
using WebApi.Proxies.One.Clients;
using WebApi.Proxies.One.Models;

namespace DemoForOldXmlConfig
{
    /// <summary>
    /// this project use old xml config file for test forward compatbility
    /// before run this , build server project in this solution and run the exe file
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {

            var baseAddr = "http://localhost:8888";
            Console.WriteLine("hello------from origin http client");

            var client = new HttpClient { BaseAddress = new Uri(baseAddr) };

            var res = client.GetAsync("api/test/GetFromSimpleArg?id=world").Result.Content.ReadAsStringAsync().Result;
            Console.WriteLine(res);
            Console.WriteLine("\n\n");


            var proxy = new ProxyClient();
            Console.WriteLine("hello------from web api proxy");

            var v1 = proxy.GetFromSimpleArg("just say this is my name");
            Console.WriteLine("1---" + JsonConvert.SerializeObject(v1));

            v1 = proxy.GetFromSimpleArgAsync("just say this is my name").Result;
            Console.WriteLine("1-async---" + JsonConvert.SerializeObject(v1));

            var complexModel = new ComplexModel() { Age = 18, Name = "super star" };
            var v2 = proxy.GetFromComplexArg(complexModel);
            Console.WriteLine("2---" + JsonConvert.SerializeObject(v2));

            var v3 = proxy.GetFromMixedArg(2016, complexModel);
            Console.WriteLine("3---" + JsonConvert.SerializeObject(v3));


            var nest = new NestedModel()
            {
                Id = 999999,
                ComplexModel = complexModel
            };
            var v4 = proxy.PostFromMixedArg("this is my str", nest, complexModel);
            Console.WriteLine("4---" + JsonConvert.SerializeObject(v4));

            var v5 = proxy.PostFromMixedArg2("this is my str", complexModel, nest);
            Console.WriteLine("5---" + JsonConvert.SerializeObject(v5));


            Console.WriteLine("---------------done!");
            Console.ReadLine();

        }
    }


    public class ProxyClient : TestClient
    {
        protected override List<KeyValuePair<string, object>> GenerateGetFromComplexArgKeyValueList(ComplexModel dataArg)
        {
            return new List<KeyValuePair<string, object>>()
                {
                    new KeyValuePair<string, object>("Name", dataArg.Name),
                    new KeyValuePair<string, object>("Age", dataArg.Age),
                };
        }

        protected override List<KeyValuePair<string, object>> GenerateGetFromMixedArgKeyValueList(int id, ComplexModel dataArg)
        {
            return new List<KeyValuePair<string, object>>()
                {
                    new KeyValuePair<string, object>("Name", dataArg.Name),
                    new KeyValuePair<string, object>("Age", dataArg.Age),
                };
        }

        protected override List<KeyValuePair<string, object>> GeneratePostFromMixedArg2KeyValueList(string simpleStr, ComplexModel uriComplexArg, NestedModel bodyNestedArg)
        {
            return new List<KeyValuePair<string, object>>()
                {
                    new KeyValuePair<string, object>("Name", uriComplexArg.Name),
                    new KeyValuePair<string, object>("Age", uriComplexArg.Age),
                };
        }
    }
}