using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeamBuilder.Models;

namespace TeamBuilder.ViewModels
{
    public class HomeViewModel
    {

        public User User { get; set; }
        public List<TeamUser> AllUsers {get;set;}
        public IEnumerable<Project> ProjectsForAdmin { get; set; }
        public IEnumerable<Team> UserTeam { get; set; }

        public IEnumerable<Project> AllProjects { get; set; }
        public IEnumerable<Team> AllTeams { get; set; }

        public int CountTeamInProject(Project p)
        {
            return AllTeams.Where(t => t.Project.ProjectId == p.ProjectId).Count();
        }

        public (int, int) Max12UsersInProject(Project p)
        {
            int max1 = 0;
            int max2 = 0;
            foreach(Team t in AllTeams.Where(t => t.Project.ProjectId == p.ProjectId))
            {
                max1 += t.MaxCount1;
                max2 += t.MaxCount2;
            }

            return (max1, max2);
        }

        public (int, int) CountUsersInTeamsInProject(Project p)
        {
            int c1 = 0;
            int c2 = 0;
            foreach (Team t in AllTeams.Where(t => t.Project.ProjectId == p.ProjectId))
            {
                c1 += AllUsers.Where(team => team.TeamId == t.TeamId).Where(u => u.User.Course == 1).Count();
                c2 += AllUsers.Where(team => team.TeamId == t.TeamId).Where(u => u.User.Course == 2).Count();
            }

            return (c1, c2);
        }

        public int FullMaxCountInProject(Project p)
        {
            return Max12UsersInProject(p).Item1 + Max12UsersInProject(p).Item2;
        }

        public int FullCountUsersInProject(Project p)
        {
            return CountUsersInTeamsInProject(p).Item1 + CountUsersInTeamsInProject(p).Item2;
        }
    }

    
}
