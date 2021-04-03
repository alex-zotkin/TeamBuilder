using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeamBuilder.Models;

namespace TeamBuilder.ViewModels
{
    public class ProjectViewModel
    {
        public List<TeamUser> TeamUsers { get; set; }
        public List<Team> Teams { get; set; }
    }
}
