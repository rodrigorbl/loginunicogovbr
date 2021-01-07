using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LoginUnicoGovBr.Core.Model
{
    [Serializable]
    public class LoginGovBrUserData
    {
        public string Access_Token { get; set; }
        public string ID_Token { get; set; }
        public string Token_Type { get; set; }
        public int Expires_In { get; set; }


        public AccessToken AccessData
        {
            get
            {
                string payload = this.Access_Token.Split(".")[1];

                var base64EncodedBytes = this.ConvertFromBase64String(payload);
                string token = System.Text.Encoding.UTF8.GetString(base64EncodedBytes);

                return JsonConvert.DeserializeObject<AccessToken>(token);
            }
        }
        
        
        public IDToken TokenData
        {
            get
            {
                string payload = this.ID_Token.Split(".")[1];

                var base64EncodedBytes = this.ConvertFromBase64String(payload);
                string token = System.Text.Encoding.UTF8.GetString(base64EncodedBytes);

                return JsonConvert.DeserializeObject<IDToken>(token);
            }
        }

        // Exclusivamente para correção do bug de conversão que existe no .NET Core
        private byte[] ConvertFromBase64String(string input)
        {
            if (String.IsNullOrWhiteSpace(input)) return null;
            try
            {
                string working = input.Replace('-', '+').Replace('_', '/'); ;
                while (working.Length % 4 != 0)
                {
                    working += '=';
                }
                return Convert.FromBase64String(working);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
