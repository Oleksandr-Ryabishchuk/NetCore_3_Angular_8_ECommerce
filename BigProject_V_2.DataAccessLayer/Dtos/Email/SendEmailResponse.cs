using System;
using System.Collections.Generic;
using System.Text;

namespace BigProject_V_2.DataAccessLayer.Email
{
    public class SendEmailResponse
    {
        public bool Successful => ErrorMsg == null;
        public string ErrorMsg { get; set; }
    }
}
