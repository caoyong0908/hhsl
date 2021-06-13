using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using hhsl_api_server.Models;

namespace hhsl_api_server.Response.Entity
{
    public class WarningRecordResponse : WarningRecordEntity
    {
        public string WIName { get; set; }

        public string WLName { get; set; }

        public string MNName { get; set; }

        public string MPName { get; set; }

        public string Time { get; set; }

        public double Data { get; set; }

        public double UValue { get; set; }

        public double DValue { get; set; }

    }
}