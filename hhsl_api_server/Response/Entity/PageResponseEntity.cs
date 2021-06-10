using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace hhsl_api_server.Response.Entity
{
    public class PageResponseEntity
    {
        public int Index { get; set; }

        public int Count { get; set; }

        public int PageCount => Total % Count == 0 ? Total / Count : Total / Count + 1;

        public int Total { get; set; }

        public object Data { get; set; }
    }
}