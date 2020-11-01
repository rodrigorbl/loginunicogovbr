using System;
using System.Collections.Generic;
using System.Text;

namespace LoginUnicoGovBr.Core.Model
{
    [Serializable]
    public class LoginGovBrUserData
    {
        public AccessToken Access_Token { get; set; }
        public IDToken ID_Token { get; set; }
        public string Token_Type { get; set; }
        public int Expires_In { get; set; }
    }
}
