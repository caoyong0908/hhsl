using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace hhsl_api_server.Models
{
    public class MonitorMethodEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Desc { get; set; }

        public int PId { get; set; }
    }
}