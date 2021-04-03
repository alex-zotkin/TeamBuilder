using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TeamBuilder.Models
{
    public class User
    {
        public int UserId { get; set; }
        public int VkId { get; set; }
        public string AccessToken { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Course { get; set; }
        public int Group { get; set; }
        public string Languages { get; set; }
        public string Description { get; set; }
        public string Photo50 { get; set; }
        public string PhotoMax { get; set; }


        //Подписки на проекты
        
        //Команды, где состоит (в разных проектах)
        public List<TeamUser> Teams { get; set; }
        
        //Проекты, которые администрирует
        public List<ProjectUser> AdminProjects { get; set; }

        //Проекты, которые жюрирует
        public List<ProjectJury> JuryProjects { get; set; }

        //Оценки от пользователя
        //public List<Mark> Marks { get; set; }

        public List<Comment> Comments { get; set; }


        //public bool UserIsAdmin(Project Project) => AdminProjects.Contains(Project.ProjectId) ? true : false;
    }

    
}
