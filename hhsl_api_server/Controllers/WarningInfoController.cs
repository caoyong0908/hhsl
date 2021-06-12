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
using hhsl_api_server.Tools;

namespace hhsl_api_server.Controllers
{
    public class WarningInfoController : ApiController
    {
        // add
        [HttpPost]
        public ApiResponse Add([FromBody] WarningInfoEntity info)
        {
            ApiResponse response = new ApiResponse();
            MySqlOperator opr = new MySqlOperator();
            opr.Connect();
            var sql = $"INSERT INTO " +
                      $"warning_info(`Name`, UValue, DValue, StartTime, EndTime, MNId, WLId) " +
                      $"VALUES('{info.Name}', {info.UValue}, {info.DValue}, " +
                      $"'{info.StartTime}', '{info.EndTime}', {info.MNId}, {info.WLId})";
            opr.Execute(sql);
            opr.DisConnected();
            return response;

        }

        // edit
        [HttpPost]
        public ApiResponse Edit([FromBody] WarningInfoEntity info)
        {
            ApiResponse response = new ApiResponse();
            MySqlOperator opr = new MySqlOperator();
            opr.Connect();
            var sql = $"UPDATE warning_info " +
                      $"SET `Name` = '{info.Name}', UValue = {info.UValue}, DValue = {info.DValue}, " +
                      $"StartTime = '{info.StartTime}', EndTime = '{info.EndTime}', " +
                      $"MNId = {info.MNId}, WLId = {info.WLId} " +
                      $"WHERE Id = {info.Id}";
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
            var sql = $"DELETE FROM warning_info WHERE Id = {id}";
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
            var sql = $"SELECT wi.*, wl.`Name` AS WLName, mn.`Name` AS MNName, mp.`Name` AS PName " +
                      $"FROM warning_info AS wi " +
                      $"INNER JOIN warning_level AS wl on wi.WLId = wl.Id " +
                      $"LEFT JOIN monitor_node AS mn ON mn.Id = wi.MNId " +
                      $"LEFT JOIN monitor_project AS mp ON mp.Id = mn.PId " +
                      $"LIMIT {(pIndex - 1) * count}, {count}";

            var sqlPage = $"SELECT COUNT(1) FROM warning_info";
            var totalObj = opr.ExecuteScalar(sqlPage);
            var total = Convert.ToInt32(totalObj);

            if (total == 0)
            {
                opr.DisConnected();
                return response;
            }

            var reader = opr.Reader(sql);
            List<WarningInfoResponse> infos = new List<WarningInfoResponse>();
            while (reader.Read())
            {
                infos.Add(new WarningInfoResponse
                {
                    EndTime = reader.GetString2("EndTime"),
                    StartTime = reader.GetString2("StartTime"),
                    MNName = reader.GetString2("MNName"),
                    PName = reader.GetString2("PName"),
                    Name = reader.GetString2("Name"),
                    WLName = reader.GetString2("WLName"),
                    Id = reader.GetInt322("Id"),
                    WLId = reader.GetInt322("WLId"),
                    MNId = reader.GetInt322("MNId"),
                    DValue = reader.GetDouble2("DValue"),
                    UValue = reader.GetDouble2("UValue"),
                });
            }

            reader.Close();
            opr.DisConnected();

            response.Data = new PageResponseEntity {Index = pIndex, Total = total, Count = count, Data = infos};
            return response;
        }


        // 开始时间 结束时间 检测项目 测点 
        [HttpGet]
        public ApiResponse Search(int pIndex, int count, string sTime, string eTime, int pId = 0, int mnId = 0)
        {
            ApiResponse response = new ApiResponse();
            MySqlOperator opr = new MySqlOperator();
            opr.Connect();

            // 判断时间

            if (string.IsNullOrEmpty(sTime))
            {
                sTime = TimeTool.Get(DateTime.MinValue);
            }

            if (string.IsNullOrEmpty(eTime))
            {
                eTime = TimeTool.Now();
            }
            var selectRule = $"WHERE wi.StartTime BETWEEN '{sTime}' AND '{eTime}' ";

            if (pId != 0)
            {
                selectRule += $"AND mn.PId = {pId}";
            }

            if (mnId != 0)
            {
                selectRule += $"AND wi.MNId = {mnId}";

            }

            var sql = $"SELECT wi.*, wl.`Name` AS WLName, mn.`Name` AS MNName, mp.`Name` AS PName " +
                      $"FROM warning_info AS wi " +
                      $"INNER JOIN warning_level AS wl on wi.WLId = wl.Id " +
                      $"LEFT JOIN monitor_node AS mn ON mn.Id = wi.MNId " +
                      $"LEFT JOIN monitor_project AS mp ON mp.Id = mn.PId " +
                      $"{selectRule} " +
                      $"LIMIT {(pIndex - 1) * count}, {count}";

            var sqlPage = $"SELECT COUNT(1) " +
                          $"FROM warning_info AS wi " +
                          $"INNER JOIN warning_level AS wl on wi.WLId = wl.Id " +
                          $"LEFT JOIN monitor_node AS mn ON mn.Id = wi.MNId " +
                          $"LEFT JOIN monitor_project AS mp ON mp.Id = mn.PId " +
                          $"{selectRule} ";
            var totalObj = opr.ExecuteScalar(sqlPage);
            var total = Convert.ToInt32(totalObj);

            if (total == 0)
            {
                opr.DisConnected();
                return response;
            }

            var reader = opr.Reader(sql);
            List<WarningInfoResponse> infos = new List<WarningInfoResponse>();
            while (reader.Read())
            {
                infos.Add(new WarningInfoResponse
                {
                    EndTime = reader.GetString2("EndTime"),
                    StartTime = reader.GetString2("StartTime"),
                    MNName = reader.GetString2("MNName"),
                    PName = reader.GetString2("PName"),
                    Name = reader.GetString2("Name"),
                    WLName = reader.GetString2("WLName"),
                    Id = reader.GetInt322("Id"),
                    WLId = reader.GetInt322("WLId"),
                    MNId = reader.GetInt322("MNId"),
                    DValue = reader.GetDouble2("DValue"),
                    UValue = reader.GetDouble2("UValue"),
                });
            }

            reader.Close();
            opr.DisConnected();

            response.Data = new PageResponseEntity {Index = pIndex, Total = total, Count = count, Data = infos};
            return response;
        }
    }
}
