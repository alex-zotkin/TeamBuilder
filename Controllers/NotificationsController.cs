using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
//using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using TeamBuilder.Models;

namespace TeamBuilder.Controllers
{
    public class NotificationsController : Controller
    {
        private DataBaseContext db;

        public NotificationsController(DataBaseContext context)
        {
            db = context;
        }

        [HttpPost]
        public async Task<string> AllNotifications()
        {
            int VkId = int.Parse(HttpContext.Request.Cookies["UserData"]);
            User User = await db.Users.Where(u => u.VkId == VkId).FirstAsync();
            List<User> AllUsers = await db.Users.ToListAsync();
            List<Team> UserTeams = await db.Applications.Where(a => a.User == User).Select(t => t.Team).ToListAsync();
            List<Team> TeamleadTeams = await db.Teams.Where(t => t.TeamLead == User).ToListAsync();
            

            List<Application> ApplicationsForTeamlead = await db.Applications.Where(a => TeamleadTeams.Contains(a.Team)).Where(a => a.Checked == false).ToListAsync();
            List<Application> ApplicationsForUser = await db.Applications.Where(a => !TeamleadTeams.Contains(a.Team)).ToListAsync();

            NotificationModel data = new NotificationModel
            {
                ApplicationsForTeamlead = ApplicationsForTeamlead,
                ApplicationsForUser = ApplicationsForUser,
            };

            return JsonSerializer.Serialize(data);
        }


        [HttpPost]
        public async Task<IActionResult> DeleteApplication(int TeamId, int UserId)
        {
            Application Application = await db.Applications.Where(t => t.Team.TeamId == TeamId).Where(u => u.User.UserId == UserId).FirstAsync();
            db.Applications.Remove(Application);
            await db.SaveChangesAsync();
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> CheckApplication(int TeamId, int UserId, bool Successed)
        {
            Application Application = await db.Applications.Where(t => t.Team.TeamId == TeamId).Where(u => u.User.UserId == UserId).FirstAsync();
            Team Team = await db.Teams.Where(t => t.TeamId == TeamId).FirstAsync();
            User User = await db.Users.Where(u => u.UserId == UserId).FirstAsync();
            Application.Checked = true;
            Application.Successed = Successed;
            

            if (Successed)
            {
                TeamUser TU = new TeamUser
                {
                    Team = Team,
                    User = User,
                };

                if (User.Course == 1 && Team.Count1 < Team.MaxCount1)
                {
                    Team.Count1 += 1;
                    await db.TeamUsers.AddAsync(TU);
                } 
                else if (User.Course == 2 && Team.Count1 < Team.MaxCount2)
                {
                    Team.Count2 += 1;
                    await db.TeamUsers.AddAsync(TU);
                }
            }
            await db.SaveChangesAsync();

            return Ok();
        }
    }
}
