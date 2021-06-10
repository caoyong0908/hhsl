using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace hhsl_api_server.Models
{
    public class ProjectInfoEntity
    {
        public int Id { get; set; } 
        public string Name { get; set; }
        public string Type { get; set; }  
        public string  Address { get; set; } 
        public string LatLong { get; set; }
        public string Location { get; set; } 
        public string Desc { get; set; }    
        public string Pic { get; set; }
    }
}