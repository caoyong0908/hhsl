using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using hhsl_api_server.Models;
using hhsl_api_server.Response;
using hhsl_api_server.Sql;
using hhsl_api_server.Tools;

namespace hhsl_api_server.Controllers
{
    public class MonitorNodeDataController : ApiController
    {
        // add(mid, data)
        [HttpPost]
        public ApiResponse Add([FromBody] MonitorDataEntity data)
        {
            ApiResponse response = new ApiResponse();
            MySqlOperator opr = new MySqlOperator();
            opr.Connect();
            var sql = $"INSERT INTO " +
                      $"monitor_node_data(Mid, Time, Data) " +
                      $"VALUES({data.MId}, '{TimeTool.Now()}', {data.Data})";
            opr.Execute(sql);
            opr.DisConnected();
            return response;

        }
        [HttpGet]
        public ApiResponse List(int pId, int gId, int nId, string sTime, string eTime)
        {
            ApiResponse response = new ApiResponse();
            MySqlOperator opr = new MySqlOperator();
            opr.Connect();
            // group parameter







            opr.DisConnected();
            return response;
        }
        // seach // project group node

        // 


    }
}
