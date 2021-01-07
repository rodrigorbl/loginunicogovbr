# Class Library Login Único Gov.BR
Biblioteca de Classes .NET Core para facilitar a integração como Login Único do Governo Federal (Gov.Br)

Implementado de acordo com o [Roteiro de Integração do Login Único](https://manual-roteiro-integracao-login-unico.servicos.gov.br/pt/stable/ "Roteiro de Integração do Login Único").

## Principais classes
*  **Core.LoginUnicoGovBrRequest** - Responsável por prover os métodos de autenticação com Login Único do Governo Federal e requisição do token com os dados do usuário autenticado;

* **Core.Model.LoginGovBrAuthenticationResponse** - Modelo que descreve os dados retornados pela primeira parte da autenticação através do Login Único;

* **Core.Model.LoginGovBrUserData** - Modelo que descreve os dados do usuário pessoa física ou jurídica autenticado através do Login Único.

## Workflow básico

1. Instancie a classe **LoginUnicoGovBrRequest** com o Client Id e Client Secret fornecidos e o Redirect URL e Logout URL informados durante o processo de cadastramento da sua aplicação junto aos órgãos competentes.

2. O método **_GetAuthenticationURI()_** fornece a URL completa para acessar a página de autenticação do Login Único e os parâmetros necessários para identificar sua aplicação.

3.  Na página definida como **Redirect URL**, instancie a classe **LoginGovBrAuthenticationResponse** com os dados de Code e State retornados na query string pela autenticação do Login Único.

4. A instância dessa classe deve ser utilizada como parâmetro para o método **_RequestToken_** que retornará como resposta a instância da classe **APIResponse** que encapsula o retorno de dados de quaiquer APIs. Os dados retornados pela API estarão armazenados no atributo dinâmico **Data**.

5. Esta Class Library implementa uma classe dedicada a armazenar específicamente o resultado da autenticação para pessoa física ou jurídica. A classe está no namespace Core.Model.LoginGovBrUserData.

6. Utilize o método **_GetLogoutURI()_** para obter a URL de logout no sistema do Login Único.

## ToDo
Apesar de existirem métodos para o acesso aos serviços de Confiabilidade e Categorias, estes ainda não foram completamente implementados ou testados.
