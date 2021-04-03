using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TeamBuilder.Models
{
    public class Application
    {
        public int ApplicationId { get; set; }
        public Team Team { get; set; }
        public User User { get; set; }
        public bool Checked { get; set; }
        public bool Successed { get; set; }
        public DateTime Date { get; set; }
    }
}
