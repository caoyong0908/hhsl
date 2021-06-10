using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace hhsl_api_server.Models
{
    public class TerminalInfoEntity
    {
        public int Id { get; set; }

        public string No { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public int NetId { get; set; }

        public int CommType { get; set; }

        public int ProtocolType { get; set; }

        public string DataTemplate { get; set; }

        public string Location { get; set; }

        public string Vendor { get; set; }

        public string Model { get; set; }

        public string Desc { get; set; }
    }
}