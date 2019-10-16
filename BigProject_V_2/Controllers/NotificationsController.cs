using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace BigProject_V_2.Controllers
{
    public class NotificationsController : Controller
    {
        public IActionResult EmailConfirmed(string userId, string code)
        {
            if(string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(code))
            {
                return Redirect("/login"); 
            }
            return View(); 
        }
    }
}