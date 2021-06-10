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
    public class MonitorMethodController : ApiController
    {
        // add
        [HttpPost]
        public ApiResponse Add([FromBody] MonitorMethodEntity method)
        {
            ApiResponse response = new ApiResponse();
            MySqlOperator opr = new MySqlOperator();
            opr.Connect();
            var sql = $"INSERT INTO " +
                      $"monitor_method(Name, `Desc`, PId) " +
                      $"VALUES ('{method.Name}', '{method.Desc}', {method.PId})";
            opr.Execute(sql);
            opr.DisConnected();
            return response;
        }

        // list
        [HttpGet]
        public ApiResponse List(int pid,int pIndex, int count)
        {
            ApiResponse response = new ApiResponse();
            MySqlOperator opr = new MySqlOperator();
            opr.Connect();
            var sql = $"SELECT Id, Name, `Desc` " +
                      $"FROM monitor_method " +
                      $"WHERE PId = {pid} " +
                      $"LIMIT {(pIndex - 1) * count}, {count}";

            var sqlPage = $"SELECT COUNT(1) " +
                          $"FROM monitor_method " +
                          $"WHERE PId = {pid} ";
            var totalObj = opr.ExecuteScalar(sqlPage);
            var total = Convert.ToInt32(totalObj);

            if (total == 0)
            {
                opr.DisConnected();
                return response;
            }

            var reader = opr.Reader(sql);
            List<MonitorMethodEntity> projects = new List<MonitorMethodEntity>();
            while (reader.Read())
            {
                projects.Add(new MonitorMethodEntity
                {
                    Id = reader.GetInt322("Id"),
                    Name = reader.GetString2("Name"),
                    Desc = reader.GetString2("Desc"),
                    PId = pid,
                });
            }
            reader.Close();
            opr.DisConnected();

            response.Data = new PageResponseEntity { Index = pIndex, Total = total, Data = projects };

            return response;
        }

        // del
        [HttpGet]
        public ApiResponse Del(int id)
        {
            ApiResponse response = new ApiResponse();
            MySqlOperator opr = new MySqlOperator();
            opr.Connect();
            var sql = $"DELETE FROM monitor_method WHERE Id = {id}";
            opr.Execute(sql);
            opr.DisConnected();
            return response;

        }

        // edit

        [HttpPost]
        public ApiResponse Edit([FromBody] MonitorMethodEntity method)
        {
            ApiResponse response = new ApiResponse();
            MySqlOperator opr = new MySqlOperator();
            opr.Connect();
            var sql = $"UPDATE monitor_method " +
                      $"SET Name = '{method.Name}', `Desc` = '{method.Desc}' " +
                      $"WHERE Id = {method.Id}";
            opr.Execute(sql);
            opr.DisConnected();
            return response;
        }
    }
}
