using System;
using System.Collections.Generic;
using System.Text;

namespace LoginUnicoGovBr.Core.Model
{
    public class APIResponse
    {
        public bool Result { get; set; }
        public dynamic Data { get; set; }
        public int Count { get; set; }
        public string Message { get; set; }
    }
}
