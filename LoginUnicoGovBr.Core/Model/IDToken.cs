using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace LoginUnicoGovBr.Core.Model
{
    public class IDToken
    {
        [Description("CPF do usuário autenticado")]
        public string Sub { get; set; }
        [Description("Listagem dos fatores de autenticação do usuário. Pode ser “passwd” se o mesmo logou fornecendo a senha, ou “x509” se o mesmo utilizou certificado digital ou certificado em nuvem")]
        public string[] Amr { get; set; }
        [Description("URL de acesso à foto do usuário cadastrada no Gov.br. A mesma é protegida e pode ser acessada passando o access token recebido")]
        public string Picture { get; set; }
        [Description("Nome cadastrado no Gov.br do usuário autenticado")]
        public string Name { get; set; }
        [Description("Confirma se o telefone foi validado no cadastro do Gov.br. Poderá ter o valor true ou false")]
        public bool PhoneNumberVerified { get; set; }
        [Description("Número de telefone cadastrado no Gov.br do usuário autenticado. Caso o atributo phone_number_verified do ID_TOKEN tiver o valor false, o atributo phone_number não virá no ID_TOKEN")]
        public string PhoneNumber { get; set; }
        [Description("Confirma se o email foi validado no cadastro do Gov.br. Poderá ter o valor true ou false")]
        public bool EmailVerified { get; set; }
        [Description("Endereço de e-mail cadastrado no Gov.br do usuário autenticado. Caso o atributo email_verified do ID_TOKEN tiver o valor false, o atributo email não virá no ID_TOKEN")]
        public string Email { get; set; }
        [Description("CNPJ vinculado ao usuário autenticado. Atributo será preenchido quando autenticação ocorrer por certificado digital de pessoal jurídica")]
        public string CNPJ { get; set; }
    }
}
