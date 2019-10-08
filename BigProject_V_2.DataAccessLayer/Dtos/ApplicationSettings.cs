using System;
using System.Collections.Generic;
using System.Text;

namespace BigProject_V_2.DataAccessLayer.Dtos
{
    public class ApplicationSettings
    {
        // Props for JWT Tokens Signature
        public string Site { get; set; }
        public string Audience { get; set; }
        public string ExpireTime { get; set; }
        public string Secret { get; set; }
    }
}
