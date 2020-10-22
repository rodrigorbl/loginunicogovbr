using System;
using Xunit;
using LoginUnicoGovBr;
using LoginUnicoGovBr.Core;
using LoginUnicoGovBr.Core.Model;
using System.Threading.Tasks;

namespace XUnitLoginUnicoGovBrTest
{
    public class LoginUnicoGovBrTest
    {
        [Fact]
        public void GerarLinkRequisicaoAutenticacaoCorreto()
        {
            LoginUnicoGovBrRequest request = new LoginUnicoGovBrRequest("ec4318d6-f797-4d65-b4f7-39a33bf4d544", "123abc", "https://localhost:44323/Home/Autorizar");
            string requestLink = request.GetAuthenticationURI();

            // Contains foi utilizado para ignorar a parte que traz NONCE (randomico) e STATE (também randomico)
            Assert.Contains("http://127.0.0.1/mockloginunico/authorize?response_type=code&client_id=ec4318d6-f797-4d65-b4f7-39a33bf4d544&scope=openid+email+phone+profile&redirect_uri=https%3a%2f%2flocalhost%3a44323%2fHome%2fAutorizar", requestLink);
        }

        [Fact]
        public async Task RequisitarTokenUsuarioAposAutenticacaoSiteGovBr()
        {
            LoginUnicoGovBrRequest request = new LoginUnicoGovBrRequest("ec4318d6-f797-4d65-b4f7-39a33bf4d544", "123abc", "https://localhost:44323/Home/Autorizar");

            // Adiciona o Response Code
            LoginGovBrAuthenticationResponse authResponseCode = new LoginGovBrAuthenticationResponse() { Code = "XVUnpEteWP", State = "123" };

            // Faz a requisição do Token
            APIResponse response = await request.RequestToken(authResponseCode);

            // Verifica se a resposta é verdadeira e possui uma instância do usuário
            Assert.True(response.Result);
            Assert.IsType<LoginGovBrUserData>(response.Data);
        }
    }
}
