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
    public class WarningRecordController : ApiController
    {
        // add 
        [HttpPost]
        public ApiResponse Add([FromBody] WarningRecordEntity record)
        {
            ApiResponse response = new ApiResponse();
            MySqlOperator opr = new MySqlOperator();
            opr.Connect();
            var sql = $"INSERT INTO " +
                      $"warning_record(WIId, MNDId, Status) " +
                      $"VALUES( {record.WIId}, {record.MNDId}, {0})";
            opr.Execute(sql);
            opr.DisConnected();
            return response;
        }

        // list & search
        [HttpGet]
        public ApiResponse List(int pIndex, int count, string sTime = "", string eTime = "", int pId = 0, int gId = 0,
            int nId = 0)
        {
            ApiResponse response = new ApiResponse();
            MySqlOperator opr = new MySqlOperator();
            opr.Connect();

            if (string.IsNullOrEmpty(sTime))
            {
                sTime = TimeTool.Get(DateTime.MinValue);
            }

            if (string.IsNullOrEmpty(eTime))
            {
                eTime = TimeTool.Now();
            }


            var selectRule = $"WHERE mnd.Time BETWEEN '{sTime}' AND '{eTime}' ";
            if (pId != 0)
            {
                selectRule += $"AND mn.PId = {pId}";
            }

            if (gId != 0)
            {
                selectRule += $"AND mn.GId = {gId}";

            }

            if (nId != 0)
            {
                selectRule += $"AND mn.Id = {nId}";

            }

            var sql = $"SELECT wr.*, wi.`Name` AS WIName, wl.`Name` AS WLName, " +
                      $"mnd.`Data`, mnd.Time, mn.`Name` AS MNName, " +
                      $"wi.UValue, wi.DValue, mp.`Name` AS MPName " +
                      $"FROM warning_record AS wr " +
                      $"LEFT JOIN warning_info AS wi ON wi.Id = wr.WIId " +
                      $"LEFT JOIN warning_level AS wl ON wl.Id = wi.WLId " +
                      $"LEFT JOIN monitor_node_data AS mnd ON mnd.Id = wr.MNDId " +
                      $"LEFT JOIN monitor_node AS mn ON mn.Id = mnd.Mid " +
                      $"LEFT JOIN monitor_project AS mp ON mn.PId = mp.Id " +
                      $"{selectRule} " +
                      $"LIMIT {(pIndex - 1) * count}, {count}";

            var sqlPage = $"SELECT COUNT(1) " +
                          $"FROM warning_record AS wr " +
                          $"LEFT JOIN warning_info AS wi ON wi.Id = wr.WIId " +
                          $"LEFT JOIN warning_level AS wl ON wl.Id = wi.WLId " +
                          $"LEFT JOIN monitor_node_data AS mnd ON mnd.Id = wr.MNDId " +
                          $"LEFT JOIN monitor_node AS mn ON mn.Id = mnd.Mid " +
                          $"LEFT JOIN monitor_project AS mp ON mn.PId = mp.Id " +
                          $"{selectRule} ";
            var totalObj = opr.ExecuteScalar(sqlPage);
            var total = Convert.ToInt32(totalObj);

            if (total == 0)
            {
                opr.DisConnected();
                return response;
            }



            var reader = opr.Reader(sql);
            List<WarningRecordResponse> datas = new List<WarningRecordResponse>();
            while (reader.Read())
            {
                datas.Add(new WarningRecordResponse
                {
                    Data = reader.GetDouble2("Data"),
                    UValue = reader.GetDouble2("UValue"),
                    DValue = reader.GetDouble2("DValue"),
                    Id = reader.GetInt322("Id"),
                    Time = reader.GetString2("Time"),
                    WLName = reader.GetString2("WLName"),
                    WIName = reader.GetString2("WIName"),
                    MPName = reader.GetString2("MPName"),
                    MNName = reader.GetString2("MNName"), 
                    MNDId = reader.GetInt322("MNDId"),
                    WIId = reader.GetInt322("WIId"),
                    Status = reader.GetInt322("Status"), 
                });
            }
            reader.Close();
            opr.DisConnected();

            response.Data = new PageResponseEntity { Index = pIndex, Total = total, Count = count, Data = datas };
            return response;
        }


        // handle

    }
}
