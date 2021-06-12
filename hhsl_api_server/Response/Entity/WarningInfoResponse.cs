using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using hhsl_api_server.Models;

namespace hhsl_api_server.Response.Entity
{
    public class WarningInfoResponse : WarningInfoEntity
    {
        public string WLName { get; set; }

        public string PName { get; set; }

        public string MNName { get; set; }
    }
}