using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TeamBuilder.Models;

namespace TeamBuilder.Views.Login
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            ViewData["ProfileVisible"] = HttpContext.Request.Cookies.ContainsKey("UserData");
            if (HttpContext.Request.Cookies.ContainsKey("UserData"))
            {
                return Redirect("/");
            }
            else
                return View();
                
        }


        public IActionResult Logout()
        {
            HttpContext.Response.Cookies.Delete("UserData");
            return RedirectToRoute("home");
        }
    }
}
