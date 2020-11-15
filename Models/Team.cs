using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TeamBuilder.Models
{
    public class Team
    {
        public int TeamId { get; set; }
        public Project Project { get;set; }
        public string Title { get; set; }
        public string Img { get; set; }
        public string Description { get; set; }
        public List<TeamUser> Users { get; set; }
        public string Languages { get; set; } //?
        public int MaxCount1 { get; set; }
        public int MaxCount2 { get; set; }

        public List<Mark> Marks { get; set; }
        public List<Comment> Comments { get; set; }
        //public Dictionary<User, string> LearningList { get; set; }
        //ToDo


    }
}
