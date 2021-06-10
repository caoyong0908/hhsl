using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using hhsl_api_server.Models;

namespace hhsl_api_server.Response.Entity
{
    public class MonitorNodeResponse : MonitorNodeEntity
    {
        public string GName { get; set; }

        public string SName { get; set; }

        public string PName { get; set; }

        public string TName { get; set; }
    }
}