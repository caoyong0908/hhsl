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
    public class SensorController : ApiController
    {
        // add
        [HttpPost]
        public ApiResponse Add([FromBody]SensorInfoEntity sensor)
        {
            ApiResponse response = new ApiResponse();
            MySqlOperator opr = new MySqlOperator();
            opr.Connect();
            var sql = $"INSERT INTO " +
                      $"sensor_info(Id, `Name`, Type, Spec, " +
                      $"Vendor, Formula, `Precision`, " +
                      $"Classify, Tag, TId, `Desc`) " +
                      $"VALUES( {sensor.Id}, '{sensor.Name}', '{sensor.Type}', " +
                      $"'{sensor.Spec}', '{sensor.Vendor}', '{sensor.Formula}', " +
                      $"'{sensor.Precision}', '{sensor.Classify}', '{sensor.Tag}', " +
                      $"{sensor.TId}, '{sensor.Desc}')";

            opr.Execute(sql);
            opr.DisConnected();
            return response;
        }
        // edit
        [HttpPost]
        public ApiResponse Edit([FromBody] SensorInfoEntity sensor)
        {
            ApiResponse response = new ApiResponse();
            MySqlOperator opr = new MySqlOperator();
            opr.Connect();
            var sql = $"UPDATE sensor_info " +
                      $"SET " +
                      $"`Name` = '{sensor.Name}', Type = '{sensor.Type}', " +
                      $"Spec = '{sensor.Spec}', Vendor = '{sensor.Vendor}', " +
                      $"Formula = '{sensor.Formula}', `Precision` = '{sensor.Precision}', " +
                      $"Classify = '{sensor.Classify}', Tag = '{sensor.Tag}', " +
                      $"TId = {sensor.TId}, `Desc` = '{sensor.Desc}' " +
                      $"WHERE Id = {sensor.Id}";

            opr.Execute(sql);
            opr.DisConnected();
            return response;
        }
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

            var sql = $"SELECT si.*, ti.`Name` as TName " +
                      $"FROM sensor_info as si " +
                      $"LEFT JOIN terminal_info as ti " +
                      $"ON si.TId = ti.Id " +
                      $"LIMIT {(pIndex - 1) * count}, {count}";

            var sqlPage = $"SELECT COUNT(1) FROM sensor_info";
            var totalObj = opr.ExecuteScalar(sqlPage);
            var total = Convert.ToInt32(totalObj);

            if (total == 0)
            {
                opr.DisConnected();
                return response;
            }

            var reader = opr.Reader(sql);
            List<SensorInfoResponse> sensors = new List<SensorInfoResponse>();
            while (reader.Read())
            {
                sensors.Add(new SensorInfoResponse
                {
                    Classify = reader.GetString2("Classify"),
                    Desc = reader.GetString2("Desc"),
                    TName = reader.GetString2("TName"),
                    Formula = reader.GetString2("Formula"),
                    Name = reader.GetString2("Name"),
                    Precision = reader.GetString2("Precision"),
                    Spec = reader.GetString2("Spec"),
                    Tag = reader.GetString2("Tag"),
                    Type = reader.GetString2("Type"),
                    Vendor = reader.GetString2("Vendor"), 
                    Id = reader.GetInt322("Id"),
                    TId = reader.GetInt322("TId"), 
                });
            }
            opr.DisConnected();

            response.Data = new PageResponseEntity { Index = pIndex, Total = total, Count = count, Data = sensors};
            return response;
        }
    }
}
