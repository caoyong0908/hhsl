using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace hhsl_api_server.Models
{
    public class UserInfoEntity
    {
        public int Id { get; set; }

        public string Account { get; set; }

        public string Pwd { get; set; }

        public int Type { get; set; }

        public string Name { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public string LLTime { get; set; }
    }
}