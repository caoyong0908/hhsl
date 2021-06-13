using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace hhsl_api_server.Models
{
    public class WarningRecordEntity
    {
        public int Id { get; set; }

        public  int WIId { get; set; }

        public int MNDId { get; set; }

        public int Status { get; set; }

    }
}