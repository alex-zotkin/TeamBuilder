using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TeamBuilder.Models
{
    public class Api
    {
         public string access_token { get; set; }
         public int expires_in { get; set; }
         public int user_id { get; set; }

    }
}
