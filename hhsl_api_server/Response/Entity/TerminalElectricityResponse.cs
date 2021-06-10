using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using hhsl_api_server.Models;

namespace hhsl_api_server.Response.Entity
{
    public class TerminalElectricityResponse : TerminalElectricityEntity
    {
        public string Name { get; set; }

        public string No { get; set; }

        /// <summary>
        /// 终端类型
        /// </summary>
        public string TType { get; set; }
    }
}