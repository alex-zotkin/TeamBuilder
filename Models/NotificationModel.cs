using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TeamBuilder.Models
{
    public class NotificationModel
    {
        public IEnumerable<Application> ApplicationsForTeamlead { get; set; }
        public IEnumerable<Application> ApplicationsForUser { get; set; }
    }
}
