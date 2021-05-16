using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using TeamBuilder.Models;
using CsvHelper;
using System.Net;
using System.Text;

namespace TeamBuilder.Controllers
{
    public class MarksController : Controller
    {
        private DataBaseContext db;

        IWebHostEnvironment _appEnvironment;
        public MarksController(DataBaseContext context, IWebHostEnvironment appEnvironment)
        {
            db = context;
            _appEnvironment = appEnvironment;
        }

        public async Task<IActionResult> Index(int ProjectId)
        {
            ViewData["ProfileVisible"] = HttpContext.Request.Cookies.ContainsKey("UserData");
            int VkId = int.Parse(HttpContext.Request.Cookies["UserData"]);
            User User = await db.Users.Where(u => u.VkId == VkId).FirstAsync();
            Project Project = await db.Projects.Where(p => p.ProjectId == ProjectId).FirstOrDefaultAsync();
            if (Project == null)
                return RedirectToRoute("home");
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
            if (Jury.Count() == 0)
                return "NOJURY;" + Project.ProjectId;

            List<User> Users = await db.Users.ToListAsync();
            List<Mark> Marks = await db.Marks.Where(t => t.Team.ProjectId == ProjectId).ToListAsync();
            List<Team> Teams = await db.Teams.Where(p => p.ProjectId == ProjectId).ToListAsync();
            

            List<TeamsAndMarks> TeamsAndMarks = new List<TeamsAndMarks>();

            foreach (Team t in Teams)
            {
                int Summary = 0;
                List<List<Mark>> L = new List<List<Mark>>();
                List<int> StageSum = new List<int>();
                List<string> MarkNames = Marks.Where(team => team.Team == t).Where(u => u.User == null).Select(u => u.Name).ToList();
                foreach(string name in MarkNames)
                {
                    List<Mark> Stage = Marks.Where(u => u.User != null).Where(n => n.Name == name).Where(team => team.Team == t).ToList();
                    L.Add(Stage);

                    StageSum.Add(Stage.Select(m => m.Points).Sum() / Jury.Count());
                    Summary += Stage.Select(m => m.Points).Sum() / Jury.Count();

                }

                TeamsAndMarks TA = new TeamsAndMarks
                {
                    Team = t,
                    Marks = L,
                    Users = await db.TeamUsers.Where(team => team.Team == t).Select(u => u.User).ToListAsync(),
                    StageSum = StageSum,
                    Summary = Summary,
                };
                TeamsAndMarks.Add(TA);
            }

            int MarksCount = Marks.Where(u => u.User == null).Count();

            

            AllInfoAboutMarks data = new AllInfoAboutMarks {
                Project = Project,
                IsUserAdmin = IsUserAdmin,
                IsUserJury = IsUserJury,
                CurrentUser = User,
                Teams = TeamsAndMarks,
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



        [HttpPost]
        public async Task<IActionResult> Download(int ProjectId)
        {
            Project Project = await db.Projects.Where(p => p.ProjectId == ProjectId).FirstAsync();
            List<User> Users = await db.TeamUsers.Where(t => t.Team.ProjectId == ProjectId).Select(u => u.User).OrderBy(u => u.Course).ThenBy(u => u.Group).ThenBy(u => u.LastName).ThenBy(u => u.FirstName).ToListAsync();
            List<User> Jury = await db.ProjectJury.Where(p => p.ProjectId == ProjectId).Select(u => u.User).ToListAsync();
            List<Team> Teams = await db.Teams.Where(p => p.ProjectId == ProjectId).ToListAsync();
            List<Mark> Marks = await db.Marks.Where(t => t.Team.ProjectId == ProjectId).ToListAsync();

            List<TeamsAndMarks> TeamsAndMarks = new List<TeamsAndMarks>();
            

            foreach (Team t in Teams)
            {
                List<List<Mark>> L = new List<List<Mark>>();
                List<string> MarkNames = Marks.Where(team => team.Team == t).Where(u => u.User == null).Select(u => u.Name).ToList();
                foreach (string name in MarkNames)
                {
                    List<Mark> Stage = Marks.Where(u => u.User != null).Where(n => n.Name == name).Where(te => te.Team == t).ToList();
                    L.Add(Stage);

                }
                TeamsAndMarks TM = new TeamsAndMarks
                {
                    Team = t,
                    Marks = L,
                    Users = await db.TeamUsers.Where(team => team.Team == t).Select(u => u.User).ToListAsync()
                };
                TeamsAndMarks.Add(TM);
            }
            string path = _appEnvironment.WebRootPath + "/files/";
            string fileName = DateTimeOffset.Now.ToUnixTimeSeconds().ToString() + ".csv";
            string fullPath = path + fileName;

            FileInfo file = new FileInfo(fullPath);
            if (file.Exists)
            {
                file.Delete();
            }
            file.Create().Close();

            
            using (StreamWriter w = new StreamWriter(fullPath, false, Encoding.UTF8))
            {
                w.WriteLine($"{Project.Name}");
                w.WriteLine();
                w.WriteLine();

                w.WriteLine($"Участники проектной деятельности");
                w.WriteLine("Фамилия Имя;Курс;Группа;Vk;Название команды");
                foreach(User u  in Users)
                {
                    Team t = await db.TeamUsers.Where(us => us.User == u).Select(t => t.Team).FirstAsync();
                    w.WriteLine($"{u.LastName} {u.FirstName};{u.Course};{u.Group};https://vk.com/id{u.VkId};{t.Title}");
                }

                w.WriteLine();
                w.WriteLine();

                w.WriteLine($"Участники не вступившие в команды");
                w.WriteLine("Фамилия Имя;Курс;Группа;Vk;");
                List<User> UsersNotInTeam = db.Users.Where(c => c.Course != 3).ToArray().Except(Users.ToArray()).ToList();
                foreach (User u in UsersNotInTeam)
                {
                    w.WriteLine($"{u.LastName} {u.FirstName};{u.Course};{u.Group};https://vk.com/id{u.VkId}");
                }

                w.WriteLine();
                w.WriteLine();

                w.WriteLine($"Члены жюри");
                w.WriteLine("Фамилия Имя; Vk;");
                foreach (User u in Jury)
                    w.WriteLine($"{u.LastName} {u.FirstName}; https://vk.com/id{u.VkId}");

                w.WriteLine();
                w.WriteLine();


                w.WriteLine($"Команды");

                string str = "№;Название;Тип;Описание;Участники;";
                foreach (List<Mark> m in TeamsAndMarks[0].Marks)
                    str += m[0].Name + ";";
                w.WriteLine(str);
                for(int j = 0; j <= TeamsAndMarks.Count(); j++)
                {
                    TeamsAndMarks tm = TeamsAndMarks[j];
                    List<int> StageSum = new List<int>();

                    w.Write($"{j + 1};{tm.Team.Title};{tm.Team.Description};{tm.Team.Type};");
                    int i = 0;
                    while (i < tm.Users.Count || i < Jury.Count())
                    {
                        string s = i == 0 ? "" : ";;;";


                        if (i < tm.Users.Count)
                            s += $"{tm.Users[i].LastName} {tm.Users[i].FirstName} (https://vk.com/id{tm.Users[i].VkId}) - {tm.Users[i].Course} курс {tm.Users[i].Group} группа";
                        else
                            if (!(i == 0 && tm.Users.Count==0))
                            {
                                s += ";";
                            }
                        foreach (List<Mark> marks in tm.Marks)
                        {
                            s += ";";
                            if (i < marks.Count)
                            {
                                s += $"{marks[i].User.LastName} {marks[i].User.FirstName} - {marks[i].Points}";
                                StageSum.Add((int)Math.Floor((Double)marks.Select(m => m.Points).Sum() / Jury.Count()));
                            }
                        }
                        w.WriteLine(s);
                        
                        i++;
                    }

                    int Sumary = 0;
                    string stageStr = $";Сумарно;;;;";
                    for (i = 0; i < tm.Marks.Count(); i++)
                    {
                        stageStr += StageSum[i].ToString() + ";";
                        Sumary += StageSum[i];
                    }
                        ;
                    stageStr += Sumary.ToString();
                    w.WriteLine(stageStr);
                    w.WriteLine();
                }
            }
            

            WebClient webClient = new WebClient();
            webClient.DownloadFile(fullPath, fileName);
            //file.Delete();

            return Ok();
        }
    }
}
