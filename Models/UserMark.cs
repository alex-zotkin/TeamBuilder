using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TeamBuilder.Models
{
    public class UserMark
    {
        public int Id { get; set; }
        public User User { get; set; }
        public int Mark { get; set; }
    }
}
