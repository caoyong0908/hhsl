using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace hhsl_api_server.Models
{
    public class SensorInfoEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; } 
        public string Spec { get; set; }    
        public string Vendor { get; set; } 
        public string Formula { get; set; } 
        public string Precision { get; set; }
        
        public string Classify { get; set; }    
        public string Tag { get; set; }

        public int TId { get; set; }
        public string Desc { get; set; }
    }
}