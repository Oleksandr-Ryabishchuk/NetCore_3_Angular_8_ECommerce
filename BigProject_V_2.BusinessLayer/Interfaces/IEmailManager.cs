using BigProject_V_2.DataAccessLayer.Email;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BigProject_V_2.BusinessLayer.Interfaces
{
    public interface IEmailManager
    {
        Task<SendEmailResponse> SendEmailAsync(string userEmail, string emailSubject, string message);
    }
}
