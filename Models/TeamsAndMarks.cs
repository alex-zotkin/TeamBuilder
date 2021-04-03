using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TeamBuilder.Models
{
    public class TeamsAndMarks
    {
        public Team Team { get; set; }
        public List<List<Mark>> Marks { get; set; }
        public List<User> Users { get; set; }
    }
}
