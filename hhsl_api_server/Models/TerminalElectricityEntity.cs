using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace hhsl_api_server.Models
{
    public class TerminalElectricityEntity
    {
        public int Id { get; set; }

        public int Tid { get; set; }

        public string LLTime { get; set; }

        public string Type { get; set; }

        public double Electricity { get; set; }
    }
}