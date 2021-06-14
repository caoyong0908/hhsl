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
    /// 终端设置
    /// </summary>
    public class TerminalSettingController : ApiController
    {

        // list
        [HttpGet]
        public ApiResponse List(int pIndex, int count)
        {
            ApiResponse response = new ApiResponse();
            MySqlOperator opr = new MySqlOperator();
            opr.Connect();
            var sql = $"SELECT ts.*, ti.Name, ti.No, ti.Type as TType, ti.Id AS TIId " +
                      $"FROM terminal_setting as ts " +
                      $"RIGHT JOIN terminal_info as ti ON ti.Id = ts.TId " +
                      $"LIMIT {(pIndex - 1) * count}, {count}";

            var sqlPage = $"SELECT COUNT(1) " +
                          $"FROM terminal_setting as ts " +
                          $"RIGHT JOIN terminal_info as ti ON ti.Id = ts.TId ";
            var totalObj = opr.ExecuteScalar(sqlPage);
            var total = Convert.ToInt32(totalObj);

            if (total == 0)
            {
                opr.DisConnected();
                return response;
            }
            var reader = opr.Reader(sql);
            List<TerminalSettingResponse> setting = new List<TerminalSettingResponse>();
            while (reader.Read())
            {
                setting.Add(new TerminalSettingResponse
                {
                    Id = reader.GetInt322("Id"),
                    TId = reader.GetInt322("TId"),
                    CollectInterval = reader.GetInt322("CollectInterval"),
                    CollectModel = reader.GetInt322("CollectModel"),
                    CollectTimes = reader.GetInt322("CollectTimes"),
                    SendInterval = reader.GetInt322("SendInterval"), 
                    SendStatus = reader.GetInt322("SendStatus"), 
                    LLTime = reader.GetString2("LLTime"),
                    Name = reader.GetString2("Name"),
                    No = reader.GetString2("No"),
                    TType = reader.GetString2("TType"), 
                    TIId = reader.GetInt322("TIId")
                });
            }
            reader.Close();
            opr.DisConnected();

            response.Data = new PageResponseEntity { Index = pIndex, Total = total, Count = count, Data = setting };
            return response;

        }

        [HttpGet]
        public ApiResponse Search(int pIndex, int count, string name = "", string type = "")
        {
            ApiResponse response = new ApiResponse();
            MySqlOperator opr = new MySqlOperator();
            opr.Connect();
            var sql = $"SELECT ts.*, ti.Name, ti.No, ti.Type as TType, ti.Id AS TIId " +
                      $"FROM terminal_setting as ts " +
                      $"RIGHT JOIN terminal_info as ti ON ti.Id = ts.TId " +
                      $"WHERE ti.`Name` LIKE '%{name}%' " +
                      $"AND ti.Type LIKE '%{type}%' " +
                      $"LIMIT {(pIndex - 1) * count}, {count}";

            var sqlPage = $"SELECT COUNT(1) " +
                          $"FROM terminal_setting as ts " +
                          $"RIGHT JOIN terminal_info as ti ON ti.Id = ts.TId ";
            var totalObj = opr.ExecuteScalar(sqlPage);
            var total = Convert.ToInt32(totalObj);

            if (total == 0)
            {
                opr.DisConnected();
                return response;
            }
            var reader = opr.Reader(sql);
            List<TerminalSettingResponse> setting = new List<TerminalSettingResponse>();
            while (reader.Read())
            {
                setting.Add(new TerminalSettingResponse
                {
                    Id = reader.GetInt322("Id"),
                    TId = reader.GetInt322("TId"),
                    CollectInterval = reader.GetInt322("CollectInterval"),
                    CollectModel = reader.GetInt322("CollectModel"),
                    CollectTimes = reader.GetInt322("CollectTimes"),
                    SendInterval = reader.GetInt322("SendInterval"),
                    SendStatus = reader.GetInt322("SendStatus"),
                    LLTime = reader.GetString2("LLTime"),
                    Name = reader.GetString2("Name"),
                    No = reader.GetString2("No"),
                    TType = reader.GetString2("TType"),
                    TIId = reader.GetInt322("TIId")
                });
            }
            reader.Close();
            opr.DisConnected();

            response.Data = new PageResponseEntity { Index = pIndex, Total = total, Count = count, Data = setting };
            return response;
        }


        // add
        [HttpPost]
        public ApiResponse Add([FromBody] TerminalSettingEntity setting)
        {
            ApiResponse response = new ApiResponse();
            MySqlOperator opr = new MySqlOperator();
            opr.Connect();
            var sql = $"INSERT INTO " +
                      $"terminal_setting(TId, CollectInterval, CollectModel, SendInterval, CollectTimes) " +
                      $"VALUES( {setting.TId}, {setting.CollectInterval}, " +
                      $"{setting.CollectModel}, {setting.SendInterval}, " +
                      $"{setting.CollectTimes})";
            opr.Execute(sql);
            opr.DisConnected();
            return response;
        }

        // edit
        [HttpPost]
        public ApiResponse Edit([FromBody] TerminalSettingEntity setting)
        {
            ApiResponse response = new ApiResponse();
            MySqlOperator opr = new MySqlOperator();
            opr.Connect();
            var sql = $"UPDATE terminal_setting " +
                      $"SET " +
                      $"CollectInterval = {setting.CollectInterval}, CollectModel = {setting.CollectModel}, " +
                      $"SendInterval = {setting.SendInterval}, " +
                      $"CollectTimes = {setting.CollectTimes} " +
                      $"WHERE Id = {setting.Id}";
            opr.Execute(sql);
            opr.DisConnected();
            return response;
        }
    }
}
