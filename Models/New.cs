using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TeamBuilder.Models
{
    public class New
    {
        public int NewId { get; set; }
        public int ProjectId { get; set; }
        //public Project Project { get; set; }
        public string Text { get; set; }
        public User Author { get; set; }
        public DateTime Date { get; set; }
    }
}
