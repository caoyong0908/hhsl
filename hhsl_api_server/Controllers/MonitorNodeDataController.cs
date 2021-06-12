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
using MySql.Data.MySqlClient;

namespace hhsl_api_server.Controllers
{
    public class MonitorNodeDataController : ApiController
    {
        // add(mid, data)
        [HttpPost]
        public ApiResponse Add([FromBody] MonitorDataEntity data)
        {
            ApiResponse response = new ApiResponse();
            MySqlOperator opr = new MySqlOperator();
            opr.Connect();
            var sql = $"INSERT INTO " +
                      $"monitor_node_data(Mid, Time, Data) " +
                      $"VALUES({data.MId}, '{TimeTool.Now()}', {data.Data})";
            opr.Execute(sql);
            opr.DisConnected();
            return response;

        }
        /// <summary>
        /// 实时数据
        /// </summary>
        /// <param name="pId">监测项目id</param>
        /// <param name="gId">测点分组id</param>
        /// <param name="nId">测点id</param>
        /// <param name="count">数据数量</param>
        /// <returns></returns>
        [HttpGet]
        public ApiResponse News( int pId = 0, int gId = 0, int nId = 0, int count = 10)
        {
            ApiResponse response = new ApiResponse();
            MySqlOperator opr = new MySqlOperator();
            opr.Connect();
            // group parameter
            var selectRule = "";
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


            var sql = $"SELECT mnd.*, mn.`Name`, mp.`Name` AS PName " +
                      $"FROM monitor_node_data AS mnd " +
                      $"INNER JOIN monitor_node AS mn ON mn.Id = mnd.Mid " +
                      $"LEFT JOIN monitor_project AS mp ON mp.Id = mn.PId " +
                      $"WHERE {count} > (SELECT COUNT(1) FROM monitor_node_data WHERE Mid = mnd.Mid and Time > mnd.Time) " +
                      $"{selectRule} ORDER BY mnd.Time Desc";
            MySqlDataReader reader = opr.Reader(sql);
            List<MonitorNodeDataResponse> datas = new List<MonitorNodeDataResponse>();
            while (reader.Read())
            {
                datas.Add(new MonitorNodeDataResponse
                {
                    Data = reader.GetDouble2("Data"),
                    Id = reader.GetInt322("Id"),
                    MId = reader.GetInt322("MId"), 
                    Name = reader.GetString2("Name"),
                    PName = reader.GetString2("PName"),
                    Time = reader.GetString2("Time"),
                });
            }
            reader.Close();
            opr.DisConnected();
            response.Data = datas;
            return response;
        }

        [HttpGet]
        public ApiResponse Search(int pIndex, int count, string sTime, string eTime, int pId = 0, int gId = 0, int nId = 0)
        {
            ApiResponse response = new ApiResponse();
            MySqlOperator opr = new MySqlOperator();
            opr.Connect();
            // group parameter

            // 判断时间

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

            var sql = $"SELECT mnd.*, mn.`Name`, mp.`Name` AS PName, mng.`Name` AS GName " +
                      $"FROM monitor_node_data AS mnd " +
                      $"INNER JOIN monitor_node AS mn ON mn.Id = mnd.Mid " +
                      $"LEFT JOIN monitor_project AS mp ON mp.Id = mn.PId " +
                      $"LEFT JOIN monitor_node_group AS mng ON mn.GId = mng.Id " +
                      $"{selectRule} " +
                      $"ORDER BY mnd.Time Desc " +
                      $"LIMIT {(pIndex - 1) * count}, {count}";


            var sqlPage = $"SELECT COUNT(1) " +
                          $"FROM monitor_node_data AS mnd " +
                          $"INNER JOIN monitor_node AS mn ON mn.Id = mnd.Mid " +
                          $"LEFT JOIN monitor_project AS mp ON mp.Id = mn.PId " +
                          $"LEFT JOIN monitor_node_group AS mng ON mn.GId = mng.Id " +
                          $"{selectRule}";
            var totalObj = opr.ExecuteScalar(sqlPage);
            var total = Convert.ToInt32(totalObj);

            if (total == 0)
            {
                opr.DisConnected();
                return response;
            }

            var reader = opr.Reader(sql);
            List<MonitorNodeDataResponse> datas = new List<MonitorNodeDataResponse>();
            while (reader.Read())
            {
                datas.Add(new MonitorNodeDataResponse
                {
                    Data = reader.GetDouble2("Data"),
                    Id = reader.GetInt322("Id"),
                    MId = reader.GetInt322("MId"),
                    Name = reader.GetString2("Name"),
                    PName = reader.GetString2("PName"),
                    Time = reader.GetString2("Time"),
                    GName = reader.GetString2("GName"),
                });
            }
            reader.Close();
            opr.DisConnected();

            response.Data = new PageResponseEntity { Index = pIndex, Total = total, Count = count, Data = datas };
            return response;
        }
    }
}
