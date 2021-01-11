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
        private const string _authenticationURI = "https://sso.staging.acesso.gov.br/authorize";
        private const string _tokenURI = "https://sso.staging.acesso.gov.br/token";
        private const string _logoutURI = "https://sso.staging.acesso.gov.br/logout";

        private LoginGovBrAuthenticationParam _request { get; set; }        

        public LoginUnicoGovBrRequest(string client_id, string client_secret, string redirect_uri, string logout_redirect_uri) : this(client_id, client_secret, redirect_uri, logout_redirect_uri, new List<Scopes>()) { }
        public LoginUnicoGovBrRequest(string client_id, string client_secret, string redirect_uri, string logout_redirect_uri, IEnumerable<Scopes> scopes)
        {
            this._request = new LoginGovBrAuthenticationParam(client_id, client_secret, redirect_uri, logout_redirect_uri);

            // A instanciação de LoginGovBrAuthenticationParam de autenticação atribui por padrão os escopos OpenID, Email, Phone e Profile
            // Como os escopos são identificados já na autenticação do usuário, sua atribuição poderá ser realizada apenas na instanciação da classe
            if (scopes.Count() > 0) this._request.Scope = scopes;
        }

        /// <summary>
        /// Retorna a URI para requisição da autenticação do usuário.
        /// </summary>
        /// <returns>URI de autenticação</returns>
        public string GetAuthenticationURI()
        {
            return string.Format("{0}?{1}", _authenticationURI, this._request.ToString());
        }

        /// <summary>
        /// Retorna a URL para requisição do logout no login único do governo.
        /// </summary>
        /// <returns></returns>
        public string GetLogoutURI()
        {
            return string.Format("{0}?post_logout_redirect_uri={1}", _logoutURI, this._request.LogoutRedirectURI);
        }

        /// <summary>
        /// Realiza a requisição do token de acesso e demais informações do usuário autenticado através do Login Único GOV.BR
        /// </summary>
        /// <param name="responseCode">Resposta da requisição de autenticação realizada através da página de login e senha do Governo Federal.</param>
        /// <returns>Resultado da operação</returns>        
        public async Task<APIResponse> RequestToken(LoginGovBrAuthenticationResponse responseCode)
        {            
            // Define a URI de requisição ao token
            string tokenURI = string.Format("{0}?grant_type={1}&code={2}&redirect_uri={3}", _tokenURI, "authorization_code", responseCode.Code, this._request.RedirectURI);

            // Define os dados a serem enviados via POST
            Dictionary<string, string> postData = new Dictionary<string, string>();

            // Instancia o helper de requisição
            RequestHelper request = new RequestHelper();

            // Realiza a chamada com a autenticação requisitada
            APIResponse response = await request.DoPostWithAuthentication(tokenURI, this._request.ClientID, this._request.ClientSecret, postData);

            // Realiza as conversões caso a requisição tenha sido realizada com sucesso
            if (response.Result)
            {
                // Converte a resposta para um objeto de usuário
                response.Data = JsonConvert.DeserializeObject<LoginGovBrUserData>(response.Data);
            }

            return response;
        }
    }
}
