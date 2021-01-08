using LoginUnicoGovBr.Core.Model;
using LoginUnicoGovBr.Core.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginUnicoGovBr.Core
{
    public class LoginUnicoGovBrServicos
    {
        private string CPF;
        private string AccessToken;

        public LoginUnicoGovBrServicos(string cpf, string access_token)
        {
            this.CPF = cpf;
            this.AccessToken = access_token;
        }

        public async Task<APIResponse> GetEmpresas()
        {
            // Instancia a variável de retorno
            APIResponse response = new APIResponse();

            // Verifica se o scope necessário foi adicionado à requisição de autenticação
            //if (this.UserData.AccessData.Scope.ToList().Contains(Scopes.GOVBR_Empresa.ToString().ToLower()))
            if (1 == 1)
            {
                // Define a URI da requisição da imagem
                string serviceURI = string.Format("https://api.staging.acesso.gov.br/empresas/v2/empresas?filtrar-por-participante={0}", this.CPF);

                // Instancia o helper de requisição
                RequestHelper request = new RequestHelper();

                // Realiza a chamada com a autenticação requisitada
                response = await request.DoGetWithAuthentication(serviceURI, this.AccessToken);

                // Converte a resposta para o objeto correto
                response.Data = JsonConvert.DeserializeObject<LoginGovBrEmpesa>(response.Data);
            }
            else
            {
                response = new APIResponse()
                {
                    Count = 0,
                    Message = "Para recuperar os dados das empresas vinculadas ao CPF, é necessário incluir o scope 'govbr_empresa' à requisição de autenticação.",
                    Result = false
                };
            }

            return response;
        }

        public async Task<APIResponse> GetDetalhesEmpresa(string cnpj)
        {
            // Define a URI da requisição da imagem
            string serviceURI = string.Format("https://api.staging.acesso.gov.br/empresas/v2/empresas/{0}/participantes/{1}", cnpj, this.CPF);

            // Instancia o helper de requisição
            RequestHelper request = new RequestHelper();

            // Realiza a chamada com a autenticação requisitada
            APIResponse response = await request.DoGetWithAuthentication(serviceURI, this.AccessToken);

            // Converte a resposta para o objeto correto
            response.Data = JsonConvert.DeserializeObject<LoginGovBrEmpesa>(response.Data);

            return response;
        }

        public async Task<APIResponse> GetPicture()
        {
            // Define a URI da requisição da imagem
            string pictureURI = "https://sso.staging.acesso.gov.br/userinfo/picture";

            // Instancia o helper de requisição
            RequestHelper request = new RequestHelper();

            // Realiza a chamada com a autenticação requisitada
            APIResponse response = await request.DoGetWithAuthentication(pictureURI, this.AccessToken);            

            // Converte o retorno para o formato de imagem
            string picture = string.Format("data: {0};base64,{1}", "image/jpeg", Convert.ToBase64String(response.Data));
            response.Data = picture;

            return response;
        }

        public async Task<APIResponse> GetAllConfiabilidades()
        {
            // Recupera cada uma das confiabilidades existentes
            APIResponse niveis = await this.GetConfiabilidade(TipoConfiabilidade.Niveis);
            APIResponse categorias = await this.GetConfiabilidade(TipoConfiabilidade.Categorias);
            APIResponse selos = await this.GetConfiabilidade(TipoConfiabilidade.Selos);

            // Atribui a resposta a um único objeto APIResponse
            List<LoginGovBrConfiabilidade> confiabilidades = new List<LoginGovBrConfiabilidade>();

            confiabilidades.Add(niveis.Data);
            confiabilidades.Add(categorias.Data);
            confiabilidades.Add(selos.Data);

            APIResponse response = new APIResponse()
            {
                Count = 3,
                Data = confiabilidades,
                Result = true,
                Message = (niveis.Result && categorias.Result && selos.Result ? "" : "Não foi possível recuperar uma ou mais confiabilidades. Verifique quais não foram retornadas e tente novamente ou faça a chamada individualmente.")
            };

            return response;
        }
        
        private async Task<APIResponse> GetConfiabilidade(TipoConfiabilidade tipoConfiabilidade)
        {
            string confiabilidadeURI = "";
            
            // Determina a URI do serviço da confiabilidade selecionada
            switch (tipoConfiabilidade)
            {
                case TipoConfiabilidade.Niveis: confiabilidadeURI = "https://api.staging.acesso.gov.br/confiabilidades/v3/contas/{0}/niveis?response-type=ids"; break;
                case TipoConfiabilidade.Categorias: confiabilidadeURI = "https://api.staging.acesso.gov.br/confiabilidades/v3/contas/{0}/categorias?response-type=ids"; break;
                case TipoConfiabilidade.Selos: confiabilidadeURI = "https://api.staging.acesso.gov.br/confiabilidades/v3/contas/{0}/confiabilidades?response-type=ids"; break;
            }

            // Instancia a variável de retorno
            APIResponse response = new APIResponse();

            // Verifica se o scope necessário foi adicionado à requisição de autenticação
            //if (this.UserData.AccessData.Scope.ToList().Contains(Scopes.GOVBR_Confiabilidades.ToString().ToLower()))
            if (1 == 1)
            {
                // Define a URI da requisição da imagem
                confiabilidadeURI = string.Format(confiabilidadeURI, this.CPF);

                // Instancia o helper de requisição
                RequestHelper request = new RequestHelper();

                // Realiza a chamada com a autenticação requisitada
                response = await request.DoGetWithAuthentication(confiabilidadeURI, this.AccessToken);

                // Realiza as conversões caso a requisição tenha sido realizada com sucesso
                if (response.Result)
                {
                    // Converte a resposta para um objeto correto
                    LoginGovBrConfiabilidade confiabilidade = JsonConvert.DeserializeObject<LoginGovBrConfiabilidade>(response.Data);
                    confiabilidade.TipoConfiabilidade = tipoConfiabilidade;
                    response.Data = confiabilidade;
                }
            }
            else
            {
                response = new APIResponse()
                {
                    Count = 0,
                    Message = "Para recuperar os dados das confiabilidades vinculadas ao CPF, é necessário incluir o scope 'govbr_confiabilidades' à requisição de autenticação.",
                    Result = false
                };
            }

            return response;
        }        
    }
}
