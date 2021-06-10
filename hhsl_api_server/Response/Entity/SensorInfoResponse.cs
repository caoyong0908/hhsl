using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using hhsl_api_server.Models;

namespace hhsl_api_server.Response.Entity
{
    public class SensorInfoResponse : SensorInfoEntity
    {
        /// <summary>
        /// zhongduan name
        /// </summary>
        public  string TName { get; set; }
    }
}