using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.Net.Http.Headers;

namespace RESAS_Test
{
    class Program
    {
        static async Task Main()
        {
            var api = Environment.GetEnvironmentVariable("RESAS_API", EnvironmentVariableTarget.User);
            var baseUrl = "https://opendata.resas-portal.go.jp/";


            DotNetEnv.Env.Load(".env");
            var api2 = DotNetEnv.Env.GetString("RESAS_API");

            var prefecturesApi = "api/v1/prefectures";
            var industriesApi = "api/v1/industries/broad";
            var url = baseUrl + industriesApi;

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("X-API-KEY", api);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var res1 = await client.GetAsync(url);
                var res2 = await res1.Content.ReadAsStringAsync();

                Console.WriteLine(res2);
            }
        }
    }
}
