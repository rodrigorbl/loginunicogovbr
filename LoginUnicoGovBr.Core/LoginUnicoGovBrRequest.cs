using LoginUnicoGovBr.Core.Model;
using LoginUnicoGovBr.Core.Utils;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace LoginUnicoGovBr.Core
{
    /// <summary>
    /// Classe responsável por prover os métodos de autenticação com Login Único do Governo Federal e requisição do token com os dados do usuário autenticado
    /// </summary>
    public class LoginUnicoGovBrRequest
    {
        //private const string _authenticationURI = "https://sso.staging.acesso.gov.br/authorize";
        //private const string _tokenURI = "https://sso.staging.acesso.gov.br/token";

        private const string _authenticationURI = "http://127.0.0.1/mockloginunico/authorize";
        private const string _tokenURI = "http://127.0.0.1/mockloginunico/token";

        private LoginGovBrAuthenticationParam _request { get; set; }        

        public LoginUnicoGovBrRequest(string client_id, string client_secret, string redirect_uri) : this(client_id, client_secret, redirect_uri, new List<Scopes>()) { }
        public LoginUnicoGovBrRequest(string client_id, string client_secret, string redirect_uri, IEnumerable<Scopes> scopes)
        {
            this._request = new LoginGovBrAuthenticationParam(client_id, client_secret, redirect_uri);

            // A instanciação de LoginGovBrAuthenticationParam de autenticação atribui por padrão todos os escopos existentes
            if (scopes.Count() > 0) this._request.Scope = scopes;
        }

        /// <summary>
        /// Retorna a URI para requisição da autenticação do usuário.
        /// </summary>
        /// <returns>URI de autenticação</returns>
        public string GetAuthenticationURI()
        {
            return _authenticationURI + "?" + this._request.ToString();
        }

        /// <summary>
        /// Realiza a requisição do token de acesso e demais informações do usuário autenticado através do Login Único GOV.BR
        /// </summary>
        /// <param name="responseCode">Resposta da requisição de autenticação realizada através da página de login e senha do Governo Federal.</param>
        /// <returns>Resultado da operação</returns>        
        public async Task<APIResponse> RequestToken(LoginGovBrAuthenticationResponse responseCode)
        {            
            // Define a URI de requisição ao token
            string tokenURI = string.Format("{0}?grant_type={1}&code={2}&redirect_uri={3}", _tokenURI, "autorization_code", responseCode.Code, this._request.RedirectURI);

            // Define os dados a serem enviados via POST
            Dictionary<string, string> postData = new Dictionary<string, string>();

            // Instancia o helper de requisição
            RequestHelper request = new RequestHelper();

            // Realiza a chamada com a autenticação requisitada
            HttpResponseMessage response = await request.DoPostWithAuthentication(tokenURI, this._request.ClientID, this._request.ClientSecret, postData);

            // Cria a instanca do APIResponse
            APIResponse apiData = new APIResponse();

            // Se a resposta retornou corretamente, prossegue com a conversão dos dados
            if (response.IsSuccessStatusCode)
            {
                // Recupera o conteúdo da chamada
                string conteudo = response.Content.ReadAsStringAsync().Result;

                // Atribui os dados à instância da resposta
                apiData.Result = true;
                apiData.Count = 1;
                apiData.Data = JsonConvert.DeserializeObject<LoginGovBrUserData>(conteudo);
            }
            else
            {
                apiData.Result = false;
                apiData.Count = 0;
                apiData.Message = response.ReasonPhrase;
            }

            return apiData;
        }
    }
}