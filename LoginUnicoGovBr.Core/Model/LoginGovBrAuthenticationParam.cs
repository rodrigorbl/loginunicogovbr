using LoginUnicoGovBr.Core.Utils;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Encodings.Web;

namespace LoginUnicoGovBr.Core.Model
{
    public class LoginGovBrAuthenticationParam
    {
        public string ResponseType { get; set; }
        public string ClientID { get; set; }
        public string ClientSecret { get; set; }
        public IEnumerable<Scopes> Scope { get; set; }
        public string RedirectURI { get; set; }
        public string LogoutRedirectURI { get; set; }
        public string Nonce { get; private set; }
        public string State { get; private set; }

        public LoginGovBrAuthenticationParam( string clienteID, string clientSecret, string redirect_uri, string logout_redirect_uri )
        {
            ResponseType = "code";
            ClientID = clienteID;
            ClientSecret = clientSecret;
            Scope = new List<Scopes>() { Scopes.OpenID, Scopes.Email, Scopes.Phone, Scopes.Profile };
            RedirectURI = System.Web.HttpUtility.UrlEncode(redirect_uri);
            LogoutRedirectURI = System.Web.HttpUtility.UrlEncode(logout_redirect_uri);
            Nonce = RandomHelper.RandomString(16);
            State = RandomHelper.RandomString(16);
        }

        public override string ToString()
        {
            return string.Format("response_type={0}&client_id={1}&scope={2}&redirect_uri={3}&nonce={4}&state={5}",
                this.ResponseType,
                this.ClientID,
                String.Join("+", Scope).ToLower(),
                this.RedirectURI,
                this.Nonce,
                this.State);                
        }
    }
}
