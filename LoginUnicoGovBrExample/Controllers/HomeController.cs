using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using LoginUnicoGovBrExample.Models;
using LoginUnicoGovBr.Core;
using LoginUnicoGovBr.Core.Model;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace LoginUnicoGovBrExample.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private LoginUnicoGovBrRequest LoginUnicoRequest { get; set; }

        public HomeController(ILogger<HomeController> logger, IOptions<LoginUnicoGovBrConfig> config)
        {
            _logger = logger;

            // Instanciação da classe de apoio à autenticação com Login Único do Governo Federal. Necessário ter sido cadastrado na plataforma Gov.Br para obter o client_id e client_secret.
            this.LoginUnicoRequest = new LoginUnicoGovBrRequest(config.Value.ClientID, config.Value.ClientSecret, config.Value.RedirectURI);
        }

        /// <summary>
        /// Página de exemplo da solicitação de autenticação através do Login Único do Governo Federal
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            // Processar a página de login da aplicação e adionar o botão extra da autenticação via Gov.Br
            return View(this.LoginUnicoRequest);
        }

        /// <summary>
        /// Página de destino do "redirect_uri", responsável pelo processo de obtenção do token com os dados do usuário e processamento da autorização na aplicação
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        [HttpGet]        
        public async Task<IActionResult> Autorizar(LoginGovBrAuthenticationResponse response)
        {
            LoginGovBrUserData userData = new LoginGovBrUserData();
            try
            {
                // Realiza a chamada da API
                APIResponse apiResponse = await this.LoginUnicoRequest.RequestToken(response);

                // De posse do código de acesso (code), faz a chamada para recuperação do TOKEN de Autorização    
                if (apiResponse.Result)
                {
                    // Armazena os dados recuperados pela requisição
                    userData = (LoginGovBrUserData)apiResponse.Data;

                    // Proceder com a criação da sessão e autorização dentro da aplicação
                }
            }
            catch (Exception e)
            {

            }

            return View(userData);
        }        

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
