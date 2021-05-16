using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeamBuilder.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace TeamBuilder.Controllers
{
    public class TeamController : Controller
    {
        private DataBaseContext db;
        IWebHostEnvironment _appEnvironment;
        public TeamController(DataBaseContext context, IWebHostEnvironment appEnvironment)
        {
            db = context;
            _appEnvironment = appEnvironment;
        }

        public async Task<IActionResult> Index(int ProjectId, int TeamId)
        {
            ViewData["ProfileVisible"] = HttpContext.Request.Cookies.ContainsKey("UserData");
            int VkId = int.Parse(HttpContext.Request.Cookies["UserData"]);
            User User = await db.Users.Where(u => u.VkId == VkId).FirstAsync();
            ViewData["FirstName"] = User.FirstName;
            ViewData["LastName"] = User.LastName;
            ViewData["Photo50"] = User.Photo50;

            Team Team = await db.Teams.Where(t => t.TeamId == TeamId).FirstOrDefaultAsync();
            if (Team == null)
                return Redirect($"/project/{ProjectId}");
            ViewData["Title"] = Team.Title;

            return View();
        }

        [HttpPost]
        public async Task<string> AllInfoAboutTeam(int TeamId)
        {
            int VkId = int.Parse(HttpContext.Request.Cookies["UserData"]);
            User User = await db.Users.Where(u => u.VkId == VkId).FirstAsync();
            
            List<User> Users = await db.TeamUsers.Where(t => t.TeamId == TeamId).Select(u => u.User).ToListAsync();

            bool IsUserInTeam = db.TeamUsers.Where(t => t.Team.TeamId == TeamId).Select(u=>u.User).ToList().Contains(User);
            Team Team = await db.Teams.Where(t => t.TeamId == TeamId).FirstAsync();

            List<User> Jury = await db.ProjectJury.Where(p => p.ProjectId == Team.ProjectId).Select(u => u.User).ToListAsync();
            bool IsUserTeamLead = false;
            if (Team.TeamLead != null)
            {
               IsUserTeamLead = Team.TeamLead.UserId == User.UserId ? true : false;
            }
            else
            {
                IsUserTeamLead = false;
            }

            bool IsUserAdmin = db.ProjectUsers.Where(p => p.ProjectId == Team.ProjectId).Select(u => u.User).ToList().Contains(User);

            List<Mark> Marks = new List<Mark>();
            List<Mark> MarkNames = await db.Marks.Where(u => u.User == null).Where(t => t.Team == Team).ToListAsync();
            int Summary = 0;
            int SumMaxPoints = 0 ;
            foreach(Mark Mark in MarkNames)
            {
                int stageSum = db.Marks.Where(n => n.Name == Mark.Name).Where(u => u.User != null).Where(t => t.Team == Team).Select(p => p.Points).Sum() / Jury.Count();
                SumMaxPoints += await db.Marks.Where(n => n.Name == Mark.Name).Where(u => u.User != null).Where(t => t.Team == Team).Select(p => p.MaxPoints).FirstAsync();
                Summary += stageSum;
                Mark temp = new Mark {
                    MaxPoints = Mark.MaxPoints,
                    Name = Mark.Name,
                    Points = stageSum,
                    Team = Mark.Team
                };
                Marks.Add(temp);
            }

            Application App = await db.Applications.Where(t => t.Team == Team).Where(u => u.User == User).FirstOrDefaultAsync();
            string Application = "";
            if (App != null)
            {
                if (App.Checked == false)
                    Application = "ожидание";
                else if(App.Checked == true && App.Successed == false)
                    Application = "отказ";
            }

            List<Chat> Chat = await db.Chats.Where(t => t.Team.TeamId == TeamId).OrderBy(d => d.Date).ToListAsync();

            List<Link> Links = await db.Links.Where(t => t.Team.TeamId == TeamId).ToListAsync();

            List<FileModel> Files = await db.Files.Where(f => f.Team.TeamId == TeamId).OrderBy(f=>f.Date).ToListAsync();

            bool isUserInProject = db.TeamUsers.Where(t => t.Team.ProjectId == Team.ProjectId).Select(u => u.User).ToList().Contains(User);

            AllInfoAboutTeam data = new AllInfoAboutTeam { Team = Team,
                                                           Users = Users,
                                                           isUserInTeam = IsUserInTeam,
                                                           isUserAdmin = IsUserAdmin,
                                                           isUserTeamLead = IsUserTeamLead,
                                                           Marks = Marks,
                                                           Summary = Summary,
                                                           SumMaxPoints = SumMaxPoints,
                                                           CurrentUser = User,
                                                           Chat = Chat,
                                                           Links = Links,
                                                           Files = Files,
                                                           Application = Application,
                                                           isUserInProject = isUserInProject};
            return JsonConvert.SerializeObject(data);
        }


        [HttpPost]
        public async Task<IActionResult> ChangeInfoTeam(int TeamId, string inputName, string data)
        {
            Team Team = await db.Teams.Where(t=>t.TeamId == TeamId).FirstAsync();

            switch (inputName)
            {
                case "inputTitle":
                    Team.Title = data;
                    break;
                case "inputProjectType":
                    Team.Type = data;
                    break;
                case "inputDescription":
                    Team.Description= data;
                    break;
                case "inputCount1":
                    Team.MaxCount1 = int.Parse(data);
                    break;
                case "inputCount2":
                    Team.MaxCount2 = int.Parse(data);
                    break;
            }

            await db.SaveChangesAsync();

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> ChangeTeamImg(int TeamId, string ImgSrc)
        {
            Team Team = await db.Teams.Where(t => t.TeamId == TeamId).FirstAsync();
            Team.Img = ImgSrc;
            await db.SaveChangesAsync();
            return Ok();
        }



        [HttpPost]
        public async Task<IActionResult> SetTeamLead(int UserId, int TeamId)
        {
            Team Team = await db.Teams.Where(t => t.TeamId == TeamId).FirstAsync();
            User User = await db.Users.Where(u => u.UserId == UserId).FirstAsync();
            Team.TeamLead = User;
            await db.SaveChangesAsync();
            return Ok();
        }


        [HttpPost]
        public async Task<IActionResult> SendMes(int TeamId, int UserId, string Text)
        {
            Team Team = await db.Teams.Where(t => t.TeamId == TeamId).FirstAsync();
            User User = await db.Users.Where(u => u.UserId == UserId).FirstAsync();
            Chat Chat = new Chat {
                Team = Team,
                User = User,
                Text = Text,
                Date = DateTime.Now
            };
            await db.Chats.AddAsync(Chat);
            await db.SaveChangesAsync();
            return Ok();
        }



        [HttpPost]
        public async Task<IActionResult> DelFile(int Id)
        {
            FileModel F = await db.Files.Where(fi => fi.Id == Id).FirstAsync();
            System.IO.File.Delete(_appEnvironment.WebRootPath + "/files/" + F.Name);
            db.Files.Remove(F);
            await db.SaveChangesAsync();
            return Ok();
        }


        [HttpPost]
        public async Task<IActionResult> DelMes(int ChatId)
        {
            Chat Chat = await db.Chats.Where(c => c.ChatId == ChatId).FirstAsync();
            db.Chats.Remove(Chat);
            await db.SaveChangesAsync();
            return Ok();
        }



        [HttpPost]
        public async Task<IActionResult> SaveLinks([FromForm] Link[] Links, int TeamId)
        {
            List<Link> OldLinks = await db.Links.Where(t => t.Team.TeamId == TeamId).ToListAsync();
            foreach(Link l in OldLinks)
            {
                db.Links.Remove(l);
            }

            await db.SaveChangesAsync();
            
            foreach(Link l in Links)
            {
                if (l.Name != null && l.Value != null)
                {
                    Link NewLink = new Link
                    {
                        Name = l.Name,
                        Team = await db.Teams.Where(t => t.TeamId == TeamId).FirstAsync(),
                        Value = l.Value.ToLower().Replace("https://", "").Replace("http://", "").Replace("www.", "")
                    };

                    await db.Links.AddAsync(NewLink);
                    await db.SaveChangesAsync();
                }
            }
            

            return Ok();
        }



        [HttpPost]
        public async Task<IActionResult> LoadFiles(IFormFile uploadedFile, string TeamId)
        {
            Team Team = await db.Teams.Where(t => t.TeamId == int.Parse(TeamId)).FirstAsync();
            if (uploadedFile != null)
            {
                var date = DateTimeOffset.Now.ToUnixTimeSeconds();
                // путь к папке Files
                string path = _appEnvironment.WebRootPath + "/files/" + date + "-" + uploadedFile.FileName;
                // сохраняем файл в папку Files в каталоге wwwroot
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await uploadedFile.CopyToAsync(fileStream);
                }
                FileModel file = new FileModel { Name = date + "-" + uploadedFile.FileName,
                                                 Path = path,
                                                 Team = Team,
                                                 Date = DateTime.Now};
                await db.Files.AddAsync(file);
            }
            await db.SaveChangesAsync();



            return Redirect("/project/" + Team.ProjectId + "/team/" + Team.TeamId);
        }
    }

   
}
