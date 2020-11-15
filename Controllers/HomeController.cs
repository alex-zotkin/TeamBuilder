using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TeamBuilder.Models;
using TeamBuilder.ViewModels;

namespace TeamBuilder.Controllers
{
    public class HomeController : Controller
    {
        private readonly DataBaseContext db;

        public HomeController(DataBaseContext context)
        {
            db = context;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["ProfileVisible"] = HttpContext.Request.Cookies.ContainsKey("UserData");
            int VkId = int.Parse(HttpContext.Request.Cookies["UserData"]);
            User User = await db.Users.Where(u => u.VkId == VkId).FirstAsync();
            List<TeamUser> AllUsers = await db.TeamUsers.ToListAsync();

            List<Project> ProjectsForAdmin = await db.ProjectUsers.Where(p=> p.User.VkId == VkId).Select(p=>p.Project).OrderByDescending(p=>p.Date).ToListAsync();
            List<Team> UserTeam = await db.TeamUsers.Where(u => u.User.VkId == VkId).Select(t=>t.Team).ToListAsync();

            List<Project> AllProjects = await db.Projects.OrderByDescending(p => p.Date).ToListAsync();
            List<Team> AllTeams = await db.Teams.ToListAsync();
            HomeViewModel data = new HomeViewModel { User = User,
                                                     AllUsers= AllUsers,
                                                     ProjectsForAdmin = ProjectsForAdmin, 
                                                     UserTeam = UserTeam,
                                                     AllProjects = AllProjects,
                                                     AllTeams = AllTeams};

            ViewData["FirstName"] = User.FirstName;
            ViewData["LastName"] = User.LastName;
            ViewData["Photo50"] = User.Photo50;

            return View(data);                
        }


        
    }
}
