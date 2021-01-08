using System;
using System.Collections.Generic;
using System.Text;

namespace LoginUnicoGovBr.Core.Model
{
    public class LoginGovBrEmpesa
    {
        public string CNPJ { get; set; }
        public string RazaoSocial { get; set; }
        public DateTime DataCriacao { get; set; }
    }
}
