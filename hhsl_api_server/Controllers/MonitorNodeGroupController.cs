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
    public class MonitorNodeGroupController : ApiController
    {
        // add
        [HttpPost]
        public ApiResponse Add([FromBody] MonitorGroupEntity group)
        {
            ApiResponse response = new ApiResponse();
            MySqlOperator opr = new MySqlOperator();
            opr.Connect();
            var sql = $"INSERT INTO " +
                      $"monitor_node_group(`Name`, `Desc`) " +
                      $"VALUES('{group.Name}', '{group.Desc}')";
            opr.Execute(sql);
            opr.DisConnected();
            return response;

        }
        // edit
        [HttpPost]
        public ApiResponse Edit([FromBody] MonitorGroupEntity group)
        {
            ApiResponse response = new ApiResponse();
            MySqlOperator opr = new MySqlOperator();
            opr.Connect();
            var sql = $"UPDATE monitor_node_group " +
                      $"SET " +
                      $"`Name` = '{group.Name}', `Desc` = '{group.Desc}' " +
                      $"WHERE Id = {group.Id}";
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
            var sql = $"SELECT mng.*, COUNT(mn.GId) as Count, mp.`Name` as MPName " +
                      $"FROM monitor_node_group as mng " +
                      $"LEFT JOIN monitor_node as mn " +
                      $"on mn.GId = mng.Id " +
                      $"LEFT JOIN monitor_project as mp " +
                      $"on mp.Id = mn.PId " +
                      $"GROUP BY mng.Id " +
                      $"LIMIT {(pIndex - 1) * count}, {count}";
            // page count select
            var sqlPage = $"SELECT COUNT(1) " +
                          $"FROM monitor_node_group";
            var totalObj = opr.ExecuteScalar(sqlPage);
            var total = Convert.ToInt32(totalObj);

            if (total == 0)
            {
                opr.DisConnected();
                return response;
            }


            var reader = opr.Reader(sql);
            List<MonitorNodeGroupResponse> groups = new List<MonitorNodeGroupResponse>();
            while (reader.Read())
            {
                groups.Add(new MonitorNodeGroupResponse
                {
                    Id = reader.GetInt322("Id"),
                    Name = reader.GetString2("Name"),
                    Desc = reader.GetString2("Desc"),
                    Count = reader.GetInt322("Count"), 
                });
            }

            reader.Close();
            opr.DisConnected();

            response.Data = new PageResponseEntity { Index = pIndex, Total = total, Count = count, Data = groups };
            return response;
        }


        [HttpGet]
        public ApiResponse All()
        {
            ApiResponse response = new ApiResponse();
            MySqlOperator opr = new MySqlOperator();
            opr.Connect();
            var sql = $"SELECT * FROM monitor_node_group";
            var reader = opr.Reader(sql);
            List<MonitorGroupEntity> groups = new List<MonitorGroupEntity>();
            while (reader.Read())
            {
                groups.Add(new MonitorGroupEntity
                {
                    Id = reader.GetInt322("Id"),
                    Name = reader.GetString2("Name"),
                    Desc = reader.GetString2("Desc"),
                });
            }
            reader.Close();
            opr.DisConnected();
            response.Data = groups;
            return response;

        }


        // del
        [HttpGet]
        public ApiResponse Del(int id)
        {
            ApiResponse response = new ApiResponse();
            MySqlOperator opr = new MySqlOperator();
            opr.Connect();
            var sql = $"DELETE FROM monitor_node_group WHERE Id = {id}";
            opr.Execute(sql);
            opr.DisConnected();
            return response;

        }
    }
}
