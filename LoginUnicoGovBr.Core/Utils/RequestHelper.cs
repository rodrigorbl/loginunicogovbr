using LoginUnicoGovBr.Core.Model;
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

        private void SetAuthenticationHeader(string authType, string authSecret)
        {
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(authType, authSecret);
        }

        public async Task<APIResponse> DoGetWithAuthentication(string uri, string bearerToken)
        {
            this.SetAuthenticationHeader("Bearer", bearerToken);
            return await DoGetAsync(uri);
        }

        public async Task<APIResponse> DoPostWithAuthentication(string uri, string bearerToken, Dictionary<string,string> postData)
        {
            this.SetAuthenticationHeader("Bearer", bearerToken);
            return await DoPostAsync(uri, postData);
        }

        public async Task<APIResponse> DoPostWithAuthentication(string uri, string client_id, string client_secret, Dictionary<string,string> postData)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes($"{client_id}:{client_secret}");
            string token = Convert.ToBase64String(plainTextBytes);

            this.SetAuthenticationHeader("Basic", token);
            return await DoPostAsync(uri, postData);
        }

        public async Task<APIResponse> DoGetAsync(string uri)
        {
            HttpResponseMessage response = new HttpResponseMessage();

            try
            {
                response = await _httpClient.GetAsync(uri);
            }
            catch (Exception e)
            {
                response.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                response.ReasonPhrase = string.Format("Falha na Requisição: {0}", e.Message);
            }

            return PrepareResponse(response);
        }

        public async Task<APIResponse> DoPostAsync(string uri, Dictionary<string, string> postData)
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
                response.ReasonPhrase = string.Format("Falha na Requisição: {0}", e.Message);
            }

            return PrepareResponse(response);
        }

        /// <summary>
        /// Formata o resultado da chamada HTTP para a instância da classe padrão de retorno de dados da API
        /// </summary>
        /// <param name="response">Resposta da execução da chamada de API</param>
        /// <returns></returns>
        private APIResponse PrepareResponse(HttpResponseMessage response)
        {
            APIResponse apiResponse = new APIResponse();

            // Se a resposta retornou corretamente, prossegue com a conversão dos dados
            if (response.IsSuccessStatusCode)
            {
                // Realiza conversões com base no content-type
                if (response.Content.Headers.ContentType.MediaType.Contains("image"))
                {
                    apiResponse.Data = response.Content.ReadAsByteArrayAsync().Result;
                }
                else
                {
                    // Recupera o conteúdo da chamada
                    apiResponse.Data = response.Content.ReadAsStringAsync().Result;
                }

                // Atribui os dados à instância da resposta
                apiResponse.Result = true;
                apiResponse.Count = 1;
                //apiResponse.Data = conteudo;
            }
            else
            {
                apiResponse.Result = false;
                apiResponse.Count = 0;
                apiResponse.Message = response.ReasonPhrase;
            }

            return apiResponse;
        }
    }
}
