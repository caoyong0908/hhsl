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
    public class LogController : ApiController
    {
        [HttpGet]
        public ApiResponse List(int pIndex, int count)
        {
            ApiResponse response = new ApiResponse();
            MySqlOperator opr = new MySqlOperator();
            opr.Connect();
            var sql = $"SELECT Title, Content, Form, Time " +
                      $"FROM system_log " +
                      $"LIMIT {(pIndex - 1) * count}, {count}";
            var reader = opr.Reader(sql);
            List<SystemLogEntity> logs = new List<SystemLogEntity>();
            while (reader.Read())
            {
                logs.Add(new SystemLogEntity
                {
                    Title = reader.GetString2("Title"),
                    Content = reader.GetString2("Content"),
                    Form = reader.GetString2("Form"),
                    Time = reader.GetString2("Time"),
                });
            }
            reader.Close();
            opr.DisConnected();

            response.Data = logs;
            return response;
        }


        [HttpGet]
        public ApiResponse Select(string startTime, string endTime, int pIndex, int count)
        {
            ApiResponse response = new ApiResponse();
            MySqlOperator opr = new MySqlOperator();
            opr.Connect();
            var sql = $"SELECT Title, Content, Form, Time " +
                      $"FROM system_log " +
                      $"WHERE Time BETWEEN '{startTime}' AND '{endTime}' " +
                      $"LIMIT {(pIndex - 1) * count}, {count}";
            var reader = opr.Reader(sql);
            List<SystemLogEntity> logs = new List<SystemLogEntity>();
            while (reader.Read())
            {
                logs.Add(new SystemLogEntity
                {
                    Title = reader.GetString2("Title"),
                    Content = reader.GetString2("Content"),
                    Form = reader.GetString2("Form"),
                    Time = reader.GetString2("Time"),
                });
            }
            reader.Close();
            opr.DisConnected();
            response.Data = logs;
            return response;
        }



        [HttpPost]
        public ApiResponse Add([FromBody] SystemLogEntity log)
        {
            ApiResponse response = new ApiResponse();
            MySqlOperator opr = new MySqlOperator();
            opr.Connect();
            log.Time = TimeTool.Now();
            var sql = $"INSERT INTO " +
                      $"system_log(Title, Content, Form, Time) " +
                      $"VALUES('{log.Title}', '{log.Content}', '{log.Form}', '{log.Time}')";
            opr.Execute(sql);
            opr.DisConnected();
            return response;
        }


    }
}
