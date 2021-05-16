using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TeamBuilder.Models
{
    public class AllInfoAboutTeam
    {
        public Team Team { get; set; }
        public List<User> Users { get; set; }
        public List<Mark> Marks {get; set;}
        public int Summary { get; set; }
        public int SumMaxPoints { get; set; }
        public bool isUserInTeam { get; set; }
        public bool isUserTeamLead { get; set; }
        public bool isUserAdmin { get; set; }
        public User CurrentUser { get; set; }
        public List<Chat> Chat { get; set; }
        public List<Link> Links { get; set; }

        public List<FileModel> Files { get; set; }

        public string Application { get; set; }

        public bool isUserInProject { get; set; }
        
    }
}
