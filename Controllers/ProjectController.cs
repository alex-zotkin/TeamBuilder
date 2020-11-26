using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
//using Newtonsoft.Json;
//using Newtonsoft.Json.Linq;
using TeamBuilder.Models;
using System.Text.Json;

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
                    Team.Description = "Пустая команда";
                    Team.MaxCount1 = course1;
                    Team.MaxCount2 = course2;
                    Team.Img = "https://volgograd.osamarket.ru/upload/noimg.png";
                    Team.Languages = "";
                    Team.ProjectId = Project.ProjectId;
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
            IEnumerable<User> UsersInTeams = await db.TeamUsers.Where(t => t.Team.ProjectId == id)
                                            .Select(u => u.User).ToListAsync();
            UsersInTeams = UsersInTeams.OrderBy(u => u.LastName).ThenBy(u=>u.FirstName);
            Project Project = await db.Projects.Where(p => p.ProjectId == id).FirstAsync();

            IEnumerable<User> AllAdmins = db.Users.Where(u => (u.Course == 3)).ToArray().Except(ProjectAdmins.ToArray()).ToList();

            IEnumerable<New> News = await db.News.Where(n => n.ProjectId == id).ToListAsync();

            IEnumerable<Team> Teams = await db.Teams.Where(t => t.ProjectId == id).ToListAsync();

            AllInfoAboutProject data = new AllInfoAboutProject{ CurrentUser = User,
                                                                IsUserAdmin = IsUserAdmin,
                                                                ProjectAdmins = ProjectAdmins,
                                                                AllAdmins = AllAdmins,
                                                                UsersInTeams = UsersInTeams,
                                                                Project = Project,
                                                                News = News,
                                                                Teams = Teams
            };

            //JsonConvert.SerializeObject(data)
            return Json(JsonSerializer.Serialize(data));
        }
        

        [HttpPost]
        public async Task<IActionResult> ChangeProjectName(int id, string newName)
        {
            Project Project = await db.Projects.Where(p => p.ProjectId == id).FirstAsync();
            Project.Name = newName;
            await db.SaveChangesAsync();
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> AddInProjectAdmin(int id, string VkId)
        {
            Project Project = await db.Projects.Where(p => p.ProjectId == id).FirstAsync();
            User User = await db.Users.Where(u => u.VkId == int.Parse(VkId)).FirstAsync();
            ProjectUser PU = new ProjectUser();
            PU.Project = Project;
            PU.ProjectId = Project.ProjectId;
            PU.User = User;
            PU.UserId = User.UserId;


            db.ProjectUsers.Add(PU);
            await db.SaveChangesAsync();

            return Ok();
        }


        [HttpPost]
        public async Task<IActionResult> DeleteFromProjectAdmin(int id, string VkId)
        {
            Project Project = await db.Projects.Where(p => p.ProjectId == id).FirstAsync();
            User User = await db.Users.Where(u => u.VkId == int.Parse(VkId)).FirstAsync();

            ProjectUser PU = await db.ProjectUsers.Where(p => p.ProjectId == id).Where(u => u.User == User).FirstAsync();

            db.ProjectUsers.Remove(PU);
            await db.SaveChangesAsync();

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> AddInProjectNews(int id, string VkId, string text)
        {
            User User = await db.Users.Where(u => u.VkId == int.Parse(VkId)).FirstAsync();
            Project Project = await db.Projects.Where(p => p.ProjectId == id).FirstAsync();
            New New = new New { ProjectId = id,
                                Author = User,
                                Date = DateTime.Now,
                                Text = text};
            await db.News.AddAsync(New);
            await db.SaveChangesAsync();

            return Ok();
        }


        [HttpPost]
        public async Task<IActionResult> DeleteInProjectNews(int newId)
        {
            New New = await db.News.Where(n => n.NewId == newId).FirstAsync();
            db.News.Remove(New);
            await db.SaveChangesAsync();

            return Ok();
        }
    }
}
