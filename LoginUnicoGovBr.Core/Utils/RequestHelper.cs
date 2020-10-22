using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace LoginUnicoGovBr.Core.Utils
{
    public class RequestHelper
    {
        private HttpClient _httpClient { get; set; }        

        public RequestHelper()
        {
            _httpClient = new HttpClient();
        }

        public async Task<HttpResponseMessage> DoPostWithAuthentication(string uri, string bearerToken, Dictionary<string,string> postData)
        {
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);

            return await DoPostAsync(uri, postData);
        }

        public async Task<HttpResponseMessage> DoPostWithAuthentication(string uri, string client_id, string client_secret, Dictionary<string,string> postData)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes($"{client_id}:{client_secret}");
            string token = Convert.ToBase64String(plainTextBytes);

            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", token);

            return await DoPostAsync(uri, postData);
        }

        private async Task<HttpResponseMessage> DoPostAsync(string uri, Dictionary<string, string> postData)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            
            try
            {
                string payload = JsonConvert.SerializeObject(postData);
                HttpContent content = new StringContent(payload, Encoding.UTF8, "application/json");

                response = await _httpClient.PostAsync(uri, content);                
            }
            catch (Exception e)
            {
                response.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                response.ReasonPhrase = string.Format("Falha na Requisição: {0}", e);
            }

            return response;
        }
    }
}
