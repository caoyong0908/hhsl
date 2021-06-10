using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace hhsl_api_server.Models
{
    public class WarningInfoEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public double UValue { get; set; }

        public double DValue { get; set; }

        public string StartTime { get; set; }

        public string EndTime { get; set; }

        public int MNId { get; set; }

        public int WLId { get; set; }
    }
}