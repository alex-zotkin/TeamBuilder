using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TeamBuilder.Models;

namespace TeamBuilder.Controllers
{
    public class ProjectController : Controller
    {

        private DataBaseContext db;

        public ProjectController(DataBaseContext context)
        {
            db = context;
        }

        public async Task<IActionResult> Index(int id)
        {
            ViewData["ProfileVisible"] = HttpContext.Request.Cookies.ContainsKey("UserData");
            int VkId = int.Parse(HttpContext.Request.Cookies["UserData"]);
            User User = await db.Users.Where(u => u.VkId == VkId).FirstAsync();
            ViewData["FirstName"] = User.FirstName;
            ViewData["LastName"] = User.LastName;
            ViewData["Photo50"] = User.Photo50;

            Project Project = await db.Projects.Where(p => p.ProjectId == id).FirstAsync();
            ViewData["Title"] = Project.Name;
            /*List<>
            List<Team> AllTeams = await db.Teams.ToListAsync();
            List<Project> AdminProjects;
            if (User.Course == 3)
            {
                AdminProjects = await db.ProjectUsers.Where(u=>u.UserId == User.UserId).Select(p=>p.Project).ToListAsync();
            }
            ViewData["Title"] = Project.Name;*/

            //ViewData["Count_Teams"] = Project.Teams.Count();

            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Add(string teams, string marks)
        {
            //Admin
            int VkId = int.Parse(HttpContext.Request.Cookies["UserData"]);
            User Admin = await db.Users.Where(u => u.VkId == VkId).FirstAsync();


            Project Project = new Project();
            Project.Name = $"Проектная деятельность {DateTime.Now.Year}";
            Project.Date = DateTime.Today;
            /* Project.Teams = new List<Team>();*/

            Project.Admins = new List<ProjectUser>();
            ProjectUser PU = new ProjectUser();
            PU.Project = Project;
            PU.ProjectId = Project.ProjectId;
            PU.User = Admin;
            PU.UserId = Admin.UserId;
            Project.Admins.Add(PU);

            Dictionary<string, int> MarkNames = new Dictionary<string, int>();

            teams = teams.Replace("\\", "").Replace("\"", "").Replace(":", " ");
            marks = marks.Replace("\\", "").Replace("\"", "").Replace(":", " ");

            //Парсинг команды
            int index = 1;
            foreach (Match m in Regex.Matches(teams, @"(?<={).+?(?=})"))
            {
                int count_teams = int.Parse(Regex.Match(m.ToString(), @"(?<=count_teams )\d+").ToString());
                int course1 = int.Parse(Regex.Match(m.ToString(), @"(?<=course1 )\d+").ToString());
                int course2 = int.Parse(Regex.Match(m.ToString(), @"(?<=course2 )\d+").ToString());

                for (int i = 0; i < count_teams; i++)
                {
                    Team Team = new Team();
                    Team.Title = $"Команда #{index}";
                    Team.Comments = new List<Comment>();
                    Team.Description = "";
                    Team.MaxCount1 = course1;
                    Team.MaxCount2 = course2;
                    Team.Img = "https://volgograd.osamarket.ru/upload/noimg.png";
                    Team.Languages = "";
                    Team.Project = Project;
                    Team.Users = new List<TeamUser>();
                    db.Teams.Add(Team);
                    //await db.SaveChangesAsync();
                    Project.Teams.Add(Team);

                    index++;
                }
            }


            //Парсинг критериев
            foreach (Match m in Regex.Matches(marks, @"(?<={).+?(?=})"))
            {
                string krit_name = Regex.Match(m.ToString(), @"(?<=krit_name ).+(?=,)").ToString();
                int points = int.Parse(Regex.Match(m.ToString(), @"(?<=points )\d+").ToString());
                MarkNames.Add(krit_name, points);
            }

            foreach (Team t in Project.Teams)
            {
                List<Mark> Marks = new List<Mark>();
                //Оценки пользователей
                Mark UMark = new Mark();
                UMark.MaxPoints = 10;
                UMark.Name = "Оценки студентов";
                UMark.UserMark = new List<UserMark>();
                UMark.Team = t;
                Marks.Add(UMark);
                //db.Marks.Add(UMark);
                //Оценки админов
                foreach (var m in MarkNames)
                {
                    Mark Mark = new Mark();
                    Mark.Name = m.Key;
                    Mark.MaxPoints = m.Value;
                    Mark.Team = t;
                    Mark.UserMark = new List<UserMark>();
                    Marks.Add(Mark);
                    //db.Marks.Add(Mark);
                }
                t.Marks = Marks;
            }

            db.Projects.Add(Project);
            await db.SaveChangesAsync();


            return Json(Project.ProjectId);

        }


        [HttpPost]
        public async Task<IActionResult> AllInfoAboutProject(int id)
        {
            int VkId = int.Parse(HttpContext.Request.Cookies["UserData"]);
            User User = await db.Users.Where(u => u.VkId == VkId).FirstAsync();

            bool IsUserAdmin = await db.ProjectUsers.Where(u => u.UserId == User.UserId).Select(p => p.ProjectId).ContainsAsync(id);
            IEnumerable<User> ProjectAdmins = await db.ProjectUsers.Where(p => p.ProjectId == id).Select(u => u.User).ToListAsync();
            IEnumerable<User> UsersInTeams = await db.TeamUsers.Where(t => t.Team.Project.ProjectId == id)
                                            .Select(u => u.User).ToListAsync();
            UsersInTeams = UsersInTeams.OrderBy(u => u.LastName).ThenBy(u=>u.FirstName);
            Project Project = await db.Projects.Where(p => p.ProjectId == id).FirstAsync();
            //IEnumerable<User> AllAdmins = await db.ProjectUsers.Where(p => p.ProjectId != id).Where(p=>p.User.).Select(u => u.User).Distinct().ToListAsync();
            IEnumerable<User> AllAdmins = db.Users.Where(u => (u.Course == 3)).ToArray().Except(ProjectAdmins.ToArray()).ToList();

            AllInfoAboutProject data = new AllInfoAboutProject{ IsUserAdmin = IsUserAdmin,
                                                                ProjectAdmins = ProjectAdmins,
                                                                AllAdmins = AllAdmins,
                                                                UsersInTeams = UsersInTeams,
                                                                Project = Project
            };

            return Json(JsonConvert.SerializeObject(data));
        }
        

        [HttpPost]
        public async Task<IActionResult> ChangeProjectName(int id, string newName)
        {
            Project Project = await db.Projects.Where(p => p.ProjectId == id).FirstAsync();
            Project.Name = newName;
            await db.SaveChangesAsync();
            return Ok();
        }
    }
}
