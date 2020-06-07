using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Ploomes.API
{
    class Api
    {
        private static HttpClient client = new HttpClient();
        private string usrKey = "";

        public Api()
        {
            client.BaseAddress = new Uri("https://api2.ploomes.com/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("User-Key", usrKey);
        }

        private string DoTask(Task<HttpResponseMessage> task)
        {
            task.Wait();
            task.Result.EnsureSuccessStatusCode();

            var corpoDoRetorno = Task.Run(() => task.Result.Content.ReadAsStringAsync());
            corpoDoRetorno.Wait();

            return corpoDoRetorno.Result;
        }

        private string PostSync(string uri, dynamic body)
        {
            var retorno = Task.Run(() => client.PostAsync(uri, new StringContent(body.ToString())));
            return DoTask(retorno);
        }

        private string PatchSync(string uri, dynamic body)
        {
            var retorno = Task.Run(() => 
                client.SendAsync(new HttpRequestMessage(new HttpMethod("PATCH"), uri)
                    {
                        Content = new StringContent(body.ToString())
                    }
                )
            );
            return DoTask(retorno);
        }

        public string CreateClient(dynamic body)
        {
            return PostSync("/Contacts?$select=Id", body);
        }

        public string CreateDeal(dynamic body)
        {
            return PostSync("/Deals?$select=Id", body);
        }

        public string CreateTask(dynamic body)
        {
            return PostSync("/Tasks?$select=Id", body);
        }

        public string UpdateDeal(dynamic body, int id = 0)
        {
            return PatchSync($"/Deals({id})?$select=Id", body);
        }

        public string UpdateTask(dynamic body, int id = 0)
        {
            return PatchSync($"/Tasks({id})?$select=Id", body);
        }

        public string FinishTask(int id)
        {
            return PostSync($"/Tasks({id})/Finish?$select=Id", new JObject());
        }

        public string WinDeal(int id)
        {
            return PostSync($"Deals({id})/Win?$select=Id", new JObject());
        }

        public string InteractionRecord(dynamic body)
        {
            return PostSync("/InteractionRecords?$select=Id", body);
        }
    }
}
