using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TeamBuilder.Models;

namespace TeamBuilder.Controllers
{
    public class ProfileController : Controller
    {
        private readonly DataBaseContext db;
        public ProfileController(DataBaseContext context)
        {
            db = context;
        }
        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            ViewData["ProfileVisible"] = HttpContext.Request.Cookies.ContainsKey("UserData");
            int VkId = int.Parse(HttpContext.Request.Cookies["UserData"]);
            User User = await db.Users.Where(u => u.VkId == VkId).FirstAsync();
            ViewData["IsProfileNotFull"] = User.Course != 0 && User.Group != 0 && User.Description != "" ? false : true;  
            ViewData["FirstName"] = User.FirstName;
            ViewData["LastName"] = User.LastName;
            ViewData["Photo50"] = User.Photo50;
            ViewData["PhotoMax"] = User.PhotoMax.Replace("n9-26", "pp");

            ViewData["Course"] = User.Course;
            ViewData["Group"] = User.Group;
            ViewData["Description"] = User.Description;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int course, int group, string description)
        {
            int VkId = int.Parse(HttpContext.Request.Cookies["UserData"]);
            User User = await db.Users.Where(u => u.VkId == VkId).FirstAsync();
            User.Course = course;
            User.Group = group;
            User.Description = description;
            await db.SaveChangesAsync();
            /*return Content($"{course}, {group}, {description}");*/
            return RedirectToRoute("home");
            
        }
    }
}
