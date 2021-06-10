using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;

namespace hhsl_api_server.Models
{
    public class MonitorNodeEntity
    {
        public int Id { get; set; }

        public string No { get; set; }

        public string Name { get; set; }

        public string Desc { get; set; }

        public int PId { get; set; }

        public int GId { get; set; }

        public int SId { get; set; }
    }
}