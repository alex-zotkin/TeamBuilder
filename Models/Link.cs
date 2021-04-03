using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TeamBuilder.Models
{
    public class Link
    {
        public int Id { get; set; }
        public Team Team { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
