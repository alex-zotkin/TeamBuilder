using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TeamBuilder.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public Team Team { get; set; }
        public User User { get; set; }
        public string Text { get; set; }

    }
}
