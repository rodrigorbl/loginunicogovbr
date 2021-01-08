using System;
using System.Collections.Generic;
using System.Text;

namespace LoginUnicoGovBr.Core.Model
{
    public class LoginGovBrConfiabilidade
    {
        public int ID { get; set; }
        public DateTime DataAtualizacao { get; set; }
        public TipoConfiabilidade TipoConfiabilidade { get; set; }
    }
}
