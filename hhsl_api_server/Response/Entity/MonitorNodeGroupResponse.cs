using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using hhsl_api_server.Models;

namespace hhsl_api_server.Response.Entity
{
    public class MonitorNodeGroupResponse : MonitorGroupEntity
    {
        /// <summary>
        /// monitor count
        /// </summary>
        public int Count { get; set; }
    }
}