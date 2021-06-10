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
    public class WarningLevelController : ApiController
    {
        // add
        [HttpPost]
        public ApiResponse Add([FromBody] WarningLevelEntity level)
        {
            ApiResponse response = new ApiResponse();
            MySqlOperator opr = new MySqlOperator();
            opr.Connect();
            var sql = $"INSERT INTO " +
                      $"warning_level(`Name`, PName, Phone, Email, Type) " +
                      $"VALUES('{level.Name}', '{level.PName}', " +
                      $"'{level.Phone}', '{level.Email}', {level.Type})";
            opr.Execute(sql);
            opr.DisConnected();
            return response;

        }
        // edit
        [HttpPost]
        public ApiResponse Edit([FromBody] WarningLevelEntity level)
        {
            ApiResponse response = new ApiResponse();
            MySqlOperator opr = new MySqlOperator();
            opr.Connect();
            var sql = $"UPDATE warning_level " +
                      $"SET " +
                      $"`Name` = '{level.Name}', PName = '{level.PName}', " +
                      $"Phone = '{level.Phone}', Email = '{level.Email}', Type = {level.Type} " +
                      $"WHERE Id = {level.Id}";
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
            var sql = $"DELETE FROM warning_level WHERE Id = {id}";
            opr.Execute(sql);
            opr.DisConnected();
            return response;

        }
        // list
        // list
        [HttpGet]
        public ApiResponse List(int pIndex, int count)
        {
            ApiResponse response = new ApiResponse();
            MySqlOperator opr = new MySqlOperator();
            opr.Connect();
            var sql = $"SELECT * FROM warning_level";

            var sqlPage = $"SELECT COUNT(1) FROM warning_level";
            var totalObj = opr.ExecuteScalar(sqlPage);
            var total = Convert.ToInt32(totalObj);

            if (total == 0)
            {
                opr.DisConnected();
                return response;
            }

            var reader = opr.Reader(sql);
            List<WarningLevelEntity> levels = new List<WarningLevelEntity>();
            while (reader.Read())
            {
                levels.Add(new WarningLevelEntity
                {
                    Id = reader.GetInt322("Id"),
                    Type = reader.GetInt322("Type"),
                    Email = reader.GetString2("Email"),
                    Phone = reader.GetString2("Phone"),
                    Name = reader.GetString2("Name"),
                    PName = reader.GetString2("PName"),
                });
                
            }
            opr.DisConnected();

            response.Data = new PageResponseEntity { Index = pIndex, Total = total, Count = count, Data = levels };
            return response;
        }
    }
}
