using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TeamBuilder.Models
{
    public class Mark
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int MaxPoints { get; set; }
        public Team Team { get; set; }

        public User User { get; set; }

        public int Points { get; set; }
    }
}
