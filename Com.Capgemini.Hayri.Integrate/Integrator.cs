using Com.Capgemini.Hayri.Model;
using Com.Capgemini.Hayri.Model.Request;
using Com.Capgemini.Hayri.Model.Response;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Com.Capgemini.Hayri.Integrate
{
    public class Integrator
    {
        public GetTransactionsResponse GetTransactionsFromRemoteAPI(GetTransactionsRequest request)
        {
            return GetTransactionRESTAPI(Constants.GET_TRANSACTIONS_URL, request).GetAwaiter().GetResult();
        }
        public AddTransactionResponse AddTransactionQuee(AddTransactionRequest request)
        {
            return AddTransactionRESTAPI(Constants.ADD_TRANSACTION_URL, request).GetAwaiter().GetResult();
        }
        private async Task<GetTransactionsResponse> GetTransactionRESTAPI(string url, GetTransactionsRequest request)
        {
            HttpClient client = new HttpClient();
            var requestBody = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.GetAsync(url + "/" + request.CustomerID);
            response.EnsureSuccessStatusCode();
            var responseBody = response.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<GetTransactionsResponse>(responseBody);
        }

        private async Task<AddTransactionResponse> AddTransactionRESTAPI(string url,AddTransactionRequest request)
        {
            HttpClient client = new HttpClient();
            var requestBody = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(url, requestBody);
            response.EnsureSuccessStatusCode();
            var responseBody = response.Content.ReadAsStringAsync().Result;
            return  JsonConvert.DeserializeObject<AddTransactionResponse>(responseBody);
        }
    }
}
