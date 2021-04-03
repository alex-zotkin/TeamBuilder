using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TeamBuilder.Models
{
    public class AllInfoAboutMarks
    {
        public Project Project { get; set; }
        public bool IsUserAdmin { get; set; }
        public bool IsUserJury { get; set; }
        public User CurrentUser { get; set; }
        public List<TeamsAndMarks> Teams { get; set; }
        public List<int> StageSum { get; set; }
        public int Summary { get; set; }
    }
}
