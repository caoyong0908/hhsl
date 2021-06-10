using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using hhsl_api_server.Response;
using hhsl_api_server.Sql;

namespace hhsl_api_server.Controllers
{
    public class SensorController : ApiController
    {
        // add

        // edit

        // del
        [HttpGet]
        public ApiResponse Del(int id)
        {
            ApiResponse response = new ApiResponse();
            MySqlOperator opr = new MySqlOperator();
            opr.Connect();
            var sql = $"DELETE FROM sensor_info WHERE Id = {id}";
            opr.Execute(sql);
            opr.DisConnected();
            return response;

        }

        // list

        [HttpGet]
        public ApiResponse List(int pIndex, int count)
        {
            ApiResponse response = new ApiResponse();
            MySqlOperator opr = new MySqlOperator();
            opr.Connect();


            opr.DisConnected();
            return response;

        }
    }
}
