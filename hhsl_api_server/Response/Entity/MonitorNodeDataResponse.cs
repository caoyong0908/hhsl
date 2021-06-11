using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using hhsl_api_server.Models;
using Newtonsoft.Json.Serialization;

namespace hhsl_api_server.Response.Entity
{
    public class MonitorNodeDataResponse : MonitorDataEntity
    {
        public string Name { get; set; }

        public string PName { get; set; }
    }
}