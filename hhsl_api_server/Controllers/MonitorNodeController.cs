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
    public class MonitorNodeController : ApiController
    {
        // add 
        [HttpPost]
        public ApiResponse Add([FromBody] MonitorNodeEntity node)
        {
            ApiResponse response = new ApiResponse();
            MySqlOperator opr = new MySqlOperator();
            opr.Connect();
            var sql = $"INSERT INTO " +
                      $"monitor_node(`No`, `Name`, `Desc`, PId, GId, SId) " +
                      $"VALUES('{node.No}', '{node.Name}', '{node.Desc}', {node.PId}, {node.GId}, {node.SId})";
            opr.Execute(sql);
            opr.DisConnected();
            return response;

        }
        // edit

        [HttpPost]
        public ApiResponse Edit([FromBody] MonitorNodeEntity node)
        {
            ApiResponse response = new ApiResponse();
            MySqlOperator opr = new MySqlOperator();
            opr.Connect();
            var sql = $"UPDATE monitor_node " +
                      $"SET `No` = '{node.No}', `Name` = '{node.Name}', " +
                      $"`Desc` = '{node.Desc}', PId = {node.PId}, " +
                      $"GId = {node.GId}, SId = {node.SId} " +
                      $"WHERE Id = {node.Id}";
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
            var sql = $"SELECT mn.*, mp.`Name` AS PName, mng.`Name` AS GName, " +
                      $"ti.`Name` as TName, si.`Name` AS SName " +
                      $"FROM monitor_node AS mn " +
                      $"LEFT JOIN monitor_project AS mp " +
                      $"ON mn.PId = mp.Id " +
                      $"LEFT JOIN monitor_node_group AS mng " +
                      $"ON mng.Id = mn.GId " +
                      $"LEFT JOIN sensor_info AS si " +
                      $"ON mn.SId = si.Id " +
                      $"LEFT JOIN terminal_info  AS ti " +
                      $"ON si.TId = ti.Id " +
                      $"LIMIT {(pIndex - 1) * count}, {count}";

            var sqlPage = $"SELECT COUNT(1) " +
                          $"FROM monitor_node";
            var totalObj = opr.ExecuteScalar(sqlPage);
            var total = Convert.ToInt32(totalObj);

            if (total == 0)
            {
                opr.DisConnected();
                return response;
            }

            var reader = opr.Reader(sql);
            List<MonitorNodeResponse> nodes = new List<MonitorNodeResponse>();
            while (reader.Read())
            {
                nodes.Add(new MonitorNodeResponse
                {
                    Desc = reader.GetString2("Desc"),
                    GName = reader.GetString2("GName"),
                    PName = reader.GetString2("PName"),
                    SName = reader.GetString2("SName"),
                    Name = reader.GetString2("Name"),
                    TName = reader.GetString2("TName"),
                    No = reader.GetString2("No"),
                    Id = reader.GetInt322("Id"),
                    PId = reader.GetInt322("PId"),
                    GId = reader.GetInt322("GId"),
                    SId = reader.GetInt322("SId"), 
                });
            }
            reader.Close();
            opr.DisConnected();
            response.Data = new PageResponseEntity { Index = pIndex, Total = total, Count = count, Data = nodes };
            return response;

        }
        // del
        [HttpGet]
        public ApiResponse Del(int id)
        {
            ApiResponse response = new ApiResponse();
            MySqlOperator opr = new MySqlOperator();
            opr.Connect();
            var sql = $"DELETE FROM monitor_node WHERE Id = {id}";
            opr.Execute(sql);
            opr.DisConnected();
            return response;

        }

    }
}
