using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace hhsl_api_server.Models
{
    public class TerminalSettingEntity
    {
        public int Id { get; set; }

        public  int TId { get; set; }

        public int CollectInterval { get; set; }

        public int CollectModel { get; set; }

        public string LLTime { get; set; }

        public int SendInterval { get; set; }

        public int CollectTimes { get; set; }

        public int SendStatus { get; set; }

    }
}