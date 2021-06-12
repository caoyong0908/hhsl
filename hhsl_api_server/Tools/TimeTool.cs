using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace hhsl_api_server.Tools
{
    public static class TimeTool
    {
        public static string Now()
        {
            string reuslt = "";
            reuslt = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
            return reuslt;
        }

        public static string Get(DateTime dt)
        {
            string reuslt = "";
            reuslt = dt.ToString("yyyy-MM-dd hh:mm:ss");
            return reuslt;
        }
    }
}