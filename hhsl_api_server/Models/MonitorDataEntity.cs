using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace hhsl_api_server.Models
{
    public class MonitorDataEntity
    {
        public int Id { get; set; }

        public int MId { get; set; }

        public string Time { get; set; }

        public double Data { get; set; }


    }
}