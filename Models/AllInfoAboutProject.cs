using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TeamBuilder.Models
{
    public class AllInfoAboutProject
    {
        //Вся информация о пользователях и администраторах в проекте
        public User CurrentUser { get; set; }
        public bool IsUserAdmin { get; set; }
        public bool IsUserJury { get; set; }
        public int? IsUserInTeam { get; set; }
        public IEnumerable<User> ProjectAdmins { get; set; }
        public IEnumerable<User> ProjectJury { get; set; }
        public IEnumerable<User> AllAdmins { get; set; }
        public IEnumerable<User> AllJury { get; set; }
        public IEnumerable<User> Users { get; set; }
        public Project Project { get; set; }
        public IEnumerable<New> News { get; set; }
        public IEnumerable<Team> Teams { get; set; }
        public Dictionary<int, List<string>> TeamUsers { get; set; }
        public IEnumerable<int> UserApplicationTeamsId { get; set; }
        public IEnumerable<Application> ApplicationsForUser { get; set; }
        //public Dictionary<Team, List<User>> UserTeam { get; set; }
    }
}
