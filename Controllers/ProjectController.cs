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
using Newtonsoft.Json;

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
            Project.Teams = new List<Team>();

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
                    Team.Type = "Пустой тип команды";
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
                /*Mark UMark = new Mark();
                UMark.MaxPoints = 10;
                UMark.Name = "Оценки студентов";
                //UMark.UserMark = new List<UserMark>();
                UMark.Team = t;
                Marks.Add(UMark);*/
                //db.Marks.Add(UMark);
                //Оценки админов
                foreach (var m in MarkNames)
                {
                    Mark Mark = new Mark();
                    Mark.Name = m.Key;
                    Mark.MaxPoints = m.Value;
                    Mark.Team = t;
                    Mark.User = null;
                    Mark.Points = 0;

                    //Mark.UserMark = new List<UserMark>();
                    Marks.Add(Mark);
                    //db.Marks.Add(Mark);
                }
                //t.Marks = Marks;
            }

            db.Projects.Add(Project);
            await db.SaveChangesAsync();


            return Json(Project.ProjectId);

        }


        [HttpPost]
        public async Task<string> AllInfoAboutProject(int id)
        {
           
            int VkId = int.Parse(HttpContext.Request.Cookies["UserData"]);
            User User = await db.Users.Where(u => u.VkId == VkId).FirstAsync();
            IEnumerable<Team> Teams = await db.Teams.Where(t => t.ProjectId == id).ToListAsync();
            

            bool IsUserAdmin = await db.ProjectUsers.Where(u => u.UserId == User.UserId).Select(p => p.ProjectId).ContainsAsync(id);
            bool IsUserJury = await db.ProjectJury.Where(u => u.UserId == User.UserId).Select(p => p.ProjectId).ContainsAsync(id);

            var IsUserInTeamTemp = await db.TeamUsers.Where(t => t.Team.ProjectId == id).Where(u =>u.User == User).Select(t => t.Team).FirstOrDefaultAsync();
            int IsUserInTeam = IsUserInTeamTemp != null ? IsUserInTeamTemp.TeamId : -1;

            IEnumerable <User> ProjectAdmins = await db.ProjectUsers.Where(p => p.ProjectId == id).Select(u => u.User).ToListAsync();
            IEnumerable<User> ProjectJury = await db.ProjectJury.Where(p => p.ProjectId == id).Select(u => u.User).ToListAsync();
            IEnumerable<User> Users = await db.TeamUsers.Where(t => t.Team.ProjectId == id)
                                            .Select(u => u.User).ToListAsync();
            Users = Users.OrderBy(u => u.LastName).ThenBy(u=>u.FirstName);
            Project Project = await db.Projects.Where(p => p.ProjectId == id).FirstAsync();

            IEnumerable<User> AllAdmins = db.Users.Where(u => (u.Course == 3)).ToArray().Except(ProjectAdmins.ToArray()).ToList();
            IEnumerable<User> AllJury = db.Users.Where(u => (u.Course == 3)).ToArray().Except(ProjectJury.ToArray()).ToList();

            IEnumerable<New> News = await db.News.Where(n => n.ProjectId == id).ToListAsync();

            
            Dictionary<int, List<string>> TeamUsers = new Dictionary<int, List<string>>();
            foreach(Team T in Teams)
            {
                List<string> ls = new List<string>();
                    foreach (User U in db.TeamUsers.Where(t=>t.Team == T).Select(u=>u.User).ToList())
                    {
                        string temp = "";
                        temp += U.FirstName;
                        temp += "@" + U.LastName;
                        temp += "@" + U.VkId;
                        temp += "@" + U.Photo50;
                        ls.Add(temp);
                    }
                TeamUsers[T.TeamId] = ls;
            }


            List<int> UserApplicationTeamsId = await db.Applications.Where(t => Teams.Contains(t.Team)).Where(u => u.User == User).Select(t => t.Team.TeamId).ToListAsync();
            List<Application> ApplicationsForUser = await db.Applications.Where(t => Teams.Contains(t.Team)).Where(u => u.User == User).ToListAsync();

            AllInfoAboutProject data = new AllInfoAboutProject{ CurrentUser = User,
                                                                IsUserAdmin = IsUserAdmin,
                                                                IsUserJury = IsUserJury,
                                                                IsUserInTeam = IsUserInTeam,
                                                                ProjectAdmins = ProjectAdmins,
                                                                ProjectJury = ProjectJury,
                                                                AllAdmins = AllAdmins,
                                                                AllJury = AllJury,
                                                                Users = Users,
                                                                Project = Project,
                                                                News = News,
                                                                Teams = Teams,
                                                                TeamUsers = TeamUsers,
                                                                UserApplicationTeamsId = UserApplicationTeamsId,
                                                                ApplicationsForUser = ApplicationsForUser,
            };  

            //JsonSerializer.Serialize(data) 
            return JsonConvert.SerializeObject(data);
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
        public async Task<IActionResult> AddInProjectJury(int id, string VkId)
        {
            Project Project = await db.Projects.Where(p => p.ProjectId == id).FirstAsync();
            User User = await db.Users.Where(u => u.VkId == int.Parse(VkId)).FirstAsync();
            ProjectJury PJ = new ProjectJury();
            PJ.Project = Project;
            PJ.ProjectId = Project.ProjectId;
            PJ.User = User;
            PJ.UserId = User.UserId;
            db.ProjectJury.Add(PJ);
            

            List<Team> Teams = await db.Teams.Where(p => p.ProjectId == id).ToListAsync();
            List<Mark> Marks = await db.Marks.Where(t => t.Team.ProjectId == id).Where(u => u.User == null).ToListAsync();


            foreach (Mark m in Marks)
            {
                if (m.Name != "Оценки студентов")
                {
                    Mark NewMark = new Mark {
                        MaxPoints = m.MaxPoints,
                        Name = m.Name,
                        Team = m.Team,
                        Points = m.Points,
                        User = User,  
                    };
                    await db.Marks.AddAsync(NewMark);
                }
            }

            db.SaveChanges();

            return Ok();
        }


        [HttpPost]
        public async Task<IActionResult> DeleteFromProjectJury(int id, string VkId)
        {
            Project Project = await db.Projects.Where(p => p.ProjectId == id).FirstAsync();
            User User = await db.Users.Where(u => u.VkId == int.Parse(VkId)).FirstAsync();

            ProjectJury PJ = await db.ProjectJury.Where(p => p.ProjectId == id).Where(u => u.User == User).FirstAsync();
            db.ProjectJury.Remove(PJ);
            List<Mark> Marks = await db.Marks.Where(t => t.Team.ProjectId == id).Where(u => u.User == User).ToListAsync();
            foreach (Mark m in Marks)
            {
                db.Marks.Remove(m);
            }
            

            db.SaveChanges();

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

        [HttpPost]
        public async Task<IActionResult> JoinTeam(string VkId, int TeamId)
        {
            User User = await db.Users.Where(u => u.VkId == int.Parse(VkId)).FirstAsync();
            int countUsersInTeam = db.TeamUsers.Where(t => t.TeamId == TeamId).Select(u => u.User).Count();
            Team Team = await db.Teams.Where(t => t.TeamId == TeamId).FirstAsync();
            TeamUser TU = new TeamUser
            {
                Team = Team,
                TeamId = TeamId,
                User = User, 
                UserId = User.UserId
            };

            if(countUsersInTeam == 0)
            {
                if (User.Course == 1 && Team.Count1 < Team.MaxCount1)
                {
                    Team.TeamLead = User;
                    Team.Count1 += 1;
                    await db.TeamUsers.AddAsync(TU);
                    
                }
                else if (User.Course == 2 && Team.Count2 < Team.MaxCount2)
                {
                    Team.TeamLead = User;
                    Team.Count2 += 1;
                    await db.TeamUsers.AddAsync(TU);
                }
                List<Application> Applications = await db.Applications.Where(u => u.User == User).ToListAsync();
                db.Applications.RemoveRange(Applications);
            }
            else
            {
                Application Application = new Application {
                    Team = Team,
                    User = User,
                    Checked = false,
                    Successed = false,
                    Date = DateTime.Now,
                };
                await db.Applications.AddAsync(Application);
            }

            await db.SaveChangesAsync();
            return Ok();
        }


        [HttpPost]
        public async Task<IActionResult> ExitTeam(string VkId, int TeamId)
        {
            User User = await db.Users.Where(u => u.VkId == int.Parse(VkId)).FirstAsync();
            Team Team = await db.Teams.Where(t => t.TeamId == TeamId).FirstAsync();
            TeamUser TU = await db.TeamUsers.Where(u => u.User == User).FirstAsync();
            db.TeamUsers.Remove(TU);

            List<Application> UserApplication = await db.Applications.Where(t => t.Team.ProjectId == Team.ProjectId).Where(u => u.User == User).ToListAsync();
            db.Applications.RemoveRange(UserApplication);
            await db.SaveChangesAsync();


            int countUsersInTeam = db.TeamUsers.Where(t => t.TeamId == TeamId).Select(u => u.User).Count();
            if (countUsersInTeam == 0)
            {
                Team.TeamLead = null;
            }

            Team.Count1 = db.TeamUsers.Where(t => t.Team == Team).Where(u=>u.User.Course == 1).Select(u => u.User).Count();
            Team.Count2 = db.TeamUsers.Where(t => t.Team == Team).Where(u => u.User.Course == 2 || u.User.Course == 3).Select(u => u.User).Count();
            await db.SaveChangesAsync();

            return Ok();
        }
    }
}
