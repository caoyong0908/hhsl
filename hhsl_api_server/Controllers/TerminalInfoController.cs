using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using hhsl_api_server.Models;
using hhsl_api_server.Response;
using hhsl_api_server.Sql;

namespace hhsl_api_server.Controllers
{
    public class TerminalInfoController : ApiController
    {
        // add
        [HttpPost]
        public ApiResponse Add([FromBody] TerminalInfoEntity info)
        {
            ApiResponse response = new ApiResponse();
            MySqlOperator opr = new MySqlOperator();
            opr.Connect();
            var sql = $"INSERT INTO terminal_info" +
                      $"(No, Name, Type, NetId, CommType, ProtocolType, DataTemplate, Location, Vendor, Model, `Desc`) " +
                      $"VALUES('{info.No}', '{info.Name}', '{info.Type}', {info.NetId}, " +
                      $"{info.CommType}, {info.ProtocolType}, '{info.DataTemplate}', " +
                      $"'{info.Location}', '{info.Vendor}', '{info.Model}', '{info.Desc}')";
            opr.Execute(sql);
            opr.DisConnected();
            return response;

        }
        // list(分页)
        [HttpGet]
        public ApiResponse List(int pIndex, int count)
        {
            ApiResponse response = new ApiResponse();
            MySqlOperator opr = new MySqlOperator();
            opr.Connect();
            var sql = $"SELECT * " +
                      $"FROM terminal_info " +
                      $"LIMIT {(pIndex - 1) * count}, {count}";
            var reader = opr.Reader(sql);
            List<TerminalInfoEntity> infos = new List<TerminalInfoEntity>();
            while (reader.Read())
            {
                infos.Add(new TerminalInfoEntity
                {
                    CommType = reader.GetInt322("CommType"),
                    Id = reader.GetInt322("Id"),
                    NetId = reader.GetInt322("NetId"),
                    ProtocolType = reader.GetInt322("ProtocolType"),
                    Location = reader.GetString2("Location"),
                    No = reader.GetString2("No"),
                    DataTemplate = reader.GetString2("DataTemplate"),
                    Desc = reader.GetString2("Desc"),
                    Model = reader.GetString2("Model"),
                    Name = reader.GetString2("Name"),
                    Vendor = reader.GetString2("Vendor"),
                    Type = reader.GetString2("Type")
                });
            }
            reader.Close();
            opr.DisConnected();
            response.Data = infos;
            return response;
        }

        // edit
        [HttpPost]
        public ApiResponse Edit([FromBody] TerminalInfoEntity info)
        {
            ApiResponse response = new ApiResponse();
            MySqlOperator opr = new MySqlOperator();
            opr.Connect();
            var sql = $"UPDATE terminal_info " +
                      $"SET No = '{info.No}', Name = '{info.Name}', Type = '{info.Type}', " +
                      $"NetId = {info.NetId}, CommType = {info.CommType}, " +
                      $"ProtocolType = {info.ProtocolType}, DataTemplate = '{info.DataTemplate}', " +
                      $"Location = '{info.Location}', Vendor = '{info.Vendor}', Model = '{info.Model}', " +
                      $"Desc = '{info.Desc}' " +
                      $"WHERE Id = {info.Id}";
            opr.Execute(sql);
            opr.DisConnected();
            return response;

        }
        // 

        // del
        [HttpGet]
        public ApiResponse Del(int id)
        {
            ApiResponse response = new ApiResponse();
            MySqlOperator opr = new MySqlOperator();
            opr.Connect();
            var sql = $"DELETE FROM terminal_info WHERE Id = {id}";
            opr.Execute(sql);
            opr.DisConnected();
            return response;

        }
    }
}
