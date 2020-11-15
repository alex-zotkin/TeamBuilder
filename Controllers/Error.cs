using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace TeamBuilder.Controllers
{
    public class Error : Controller
    {
        public IActionResult Index(string? code = "404")
        {
            ViewData["StatusCode"] = code;
            return View();
        }
    }
}
