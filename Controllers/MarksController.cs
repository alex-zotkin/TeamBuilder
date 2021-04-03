using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeamBuilder.Models;

namespace TeamBuilder.Controllers
{
    public class MarksController : Controller
    {
        private DataBaseContext db;

        public MarksController(DataBaseContext context)
        {
            db = context;
        }

        public async Task<IActionResult> Index(int ProjectId)
        {
            ViewData["ProfileVisible"] = HttpContext.Request.Cookies.ContainsKey("UserData");
            int VkId = int.Parse(HttpContext.Request.Cookies["UserData"]);
            User User = await db.Users.Where(u => u.VkId == VkId).FirstAsync();
            Project Project = await db.Projects.Where(p => p.ProjectId == ProjectId).FirstAsync();
            ViewData["FirstName"] = User.FirstName;
            ViewData["LastName"] = User.LastName;
            ViewData["Photo50"] = User.Photo50;

            ViewData["Title"] = "Оценки проектной деятельности " + Project.Name;

            return View();
        }


        [HttpPost]
        public async Task<string> AllInfoAboutMarks(int ProjectId)
        {
            Project Project = await db.Projects.Where(p => p.ProjectId == ProjectId).FirstAsync();

            ViewData["ProfileVisible"] = HttpContext.Request.Cookies.ContainsKey("UserData");
            int VkId = int.Parse(HttpContext.Request.Cookies["UserData"]);
            User User = await db.Users.Where(u => u.VkId == VkId).FirstAsync();

            bool IsUserAdmin = await db.ProjectUsers.Where(p => p.ProjectId == ProjectId).AnyAsync(u => u.User == User);
            bool IsUserJury = await db.ProjectJury.Where(p => p.ProjectId == ProjectId).AnyAsync(u => u.User == User);

            List<User> Jury = await db.ProjectJury.Where(p => p.ProjectId == ProjectId).Select(u => u.User).ToListAsync();

            List<User> Users = await db.Users.ToListAsync();
            List<Mark> Marks = await db.Marks.Where(t => t.Team.ProjectId == ProjectId).ToListAsync();
            List<Team> Teams = await db.Teams.Where(p => p.ProjectId == ProjectId).ToListAsync();
            

            List<TeamsAndMarks> TeamsAndMarks = new List<TeamsAndMarks>();

            List<List<Mark>> L = new List<List<Mark>>();
            List<int> StageSum = new List<int>();
            int Summary = 0;
            foreach (Team t in Teams)
            {
                List<string> MarkNames = Marks.Where(team => team.Team == t).Where(u => u.User == null).Select(u => u.Name).ToList();
                foreach(string name in MarkNames)
                {
                    List<Mark> Stage = Marks.Where(u => u.User != null).Where(n => n.Name == name).ToList();
                    L.Add(Stage);

                    StageSum.Add(Stage.Select(m => m.Points).Sum() / Jury.Count());
                    Summary += Stage.Select(m => m.Points).Sum() / Jury.Count();

                }
                TeamsAndMarks.Add(new TeamsAndMarks
                {
                    Team = t,
                    Marks = L,
                    Users = await db.TeamUsers.Where(team => team.Team == t).Select(u=>u.User).ToListAsync()
                }) ; ;
            }

            int MarksCount = Marks.Where(u => u.User == null).Count();

            

            AllInfoAboutMarks data = new AllInfoAboutMarks {
                Project = Project,
                IsUserAdmin = IsUserAdmin,
                IsUserJury = IsUserJury,
                CurrentUser = User,
                Teams = TeamsAndMarks,
                StageSum = StageSum,
                Summary = Summary,
            };
            return JsonConvert.SerializeObject(data);
        }


        [HttpPost]
        public async Task<IActionResult> SetMark(int UserId, int Teamid, string Stage, int Mark)
        {
            Mark M = await db.Marks.Where(u => u.User.UserId == UserId).Where(t => t.Team.TeamId == Teamid).Where(s => s.Name == Stage).FirstAsync();
            if(Mark > M.MaxPoints)
            {
                M.Points = M.MaxPoints;
            } else if(Mark < 0)
            {
                M.Points = 0;
            }
            else
            {
                M.Points = Mark;
            }
            await db.SaveChangesAsync();
            return Ok();
        }


        [HttpPost]
        public async Task<IActionResult> OpenMarks(int ProjectId, string Set)
        {
            Project Project = await db.Projects.Where(p => p.ProjectId == ProjectId).FirstAsync();
            Project.IsMarksOpen = Set == "open";
            await db.SaveChangesAsync();

            return Ok();
        }
    }
}
