using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace LoginUnicoGovBr.Core.Model
{
    public class AccessToken
    {
        [Description("CPF do usuário autenticado")]
        public string Sub { get; set; }
        [Description("Client ID da aplicação onde o usuário se autenticou")]
        public string Aud { get; set; }
        [Description("Escopos autorizados pelo provedor de autenticação")]
        public string[] Scope { get; set; }
        [Description("Listagem dos fatores de autenticação do usuário. Pode ser “passwd” se o mesmo logou fornecendo a senha, ou “x509” se o mesmo utilizou certificado digital ou certificado em nuvem.")]
        public string[] Amr { get; set; }
        [Description("URL do provedor de autenticação que emitiu o token")]
        public string Iss { get; set; }
        [Description("Data/hora de expiração do token")]
        public DateTime Exp { get; set; }
        [Description("Data/hora em que o token foi emitido")]
        public DateTime Iat { get; set; }
        [Description("Identificador único do token, reconhecido internamente pelo provedor de autenticação")]
        public string Jti { get; set; }
        [Description("CNPJ vinculado ao usuário autenticado. Atributo será preenchido quando autenticação ocorrer por certificado digital de pessoal jurídica")]
        public string CNPJ { get; set; }
    }
}
