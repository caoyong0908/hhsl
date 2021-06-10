using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using hhsl_api_server.Models;
using hhsl_api_server.Response;
using hhsl_api_server.Response.Entity;
using hhsl_api_server.Sql;

namespace hhsl_api_server.Controllers
{

    /// <summary>
    /// 监测项目
    /// </summary>
    public class MonitorProjectController : ApiController
    {
        [HttpGet]
        public ApiResponse List(int pIndex, int count)
        {
            ApiResponse response = new ApiResponse();
            MySqlOperator opr = new MySqlOperator();
            opr.Connect();
            var sql = $"SELECT Id, Name, `Desc` " +
                      $"FROM monitor_project " +
                      $"LIMIT {(pIndex - 1) * count}, {count}";

            var sqlPage = $"SELECT COUNT(1) " +
                          $"FROM monitor_project ";
            var totalObj = opr.ExecuteScalar(sqlPage);
            var total = Convert.ToInt32(totalObj);

            if (total == 0)
            {
                opr.DisConnected();
                return response;
            }

            var reader = opr.Reader(sql);
            List<MonitorProjectEntity> projects = new List<MonitorProjectEntity>();
            while (reader.Read())
            {
                projects.Add(new MonitorProjectEntity
                {
                    Id = reader.GetInt322("Id"),
                    Name = reader.GetString2("Name"),
                    Desc = reader.GetString2("Desc"),
                });
            }
            reader.Close();
            opr.DisConnected();

            response.Data = new PageResponseEntity { Index = pIndex, Total = total, Data = projects };
            return response;
        }

        [HttpPost]
        public ApiResponse Add([FromBody] MonitorProjectEntity project)
        {
            ApiResponse response = new ApiResponse();
            MySqlOperator opr = new MySqlOperator();
            opr.Connect();
            var sql = $"INSERT INTO " +
                      $"monitor_project(Name, `Desc`) " +
                      $"VALUES ('{project.Name}', '{project.Desc}')";
            opr.Execute(sql);
            opr.DisConnected();
            return response;
        }

        [HttpPost]
        public ApiResponse Edit([FromBody] MonitorProjectEntity project)
        {
            ApiResponse response = new ApiResponse();
            MySqlOperator opr = new MySqlOperator();
            opr.Connect();
            var sql = $"UPDATE monitor_project " +
                      $"SET Name = '{project.Name}', `Desc` = '{project.Desc}' " +
                      $"WHERE Id = {project.Id}";
            opr.Execute(sql);
            opr.DisConnected();
            return response;
        }



        [HttpGet]
        public ApiResponse Del(int id)
        {
            ApiResponse response = new ApiResponse();
            MySqlOperator opr = new MySqlOperator();
            opr.Connect();
            var sql = $"DELETE FROM monitor_project WHERE Id = {id}";
            opr.Execute(sql);
            opr.DisConnected();
            return response;

        }

    }
}
