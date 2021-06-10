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
    /// 终端电量
    /// </summary>
    public class TerminalElectricityController : ApiController
    {
        // add
        [HttpPost]
        public ApiResponse Add([FromBody] TerminalElectricityEntity electricity)
        {
            ApiResponse response = new ApiResponse();
            MySqlOperator opr = new MySqlOperator();
            opr.Connect();
            var sql = $"INSERT INTO termina" +
                      $"l_electricity(TId, LLTime, Electricity, Type) " +
                      $"VALUES( {electricity.Tid}, '{electricity.LLTime}', {electricity.Electricity}, '{electricity.Type}')";
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
            var sql = $"SELECT te.*, ti.Name, ti.No, ti.Type as TType " +
                      $"FROM terminal_electricity as te " +
                      $"RIGHT JOIN terminal_info as ti " +
                      $"ON ti.Id = te.TId " +
                      $"LIMIT {(pIndex - 1) * count}, {count}";
            var reader = opr.Reader(sql);
            List<TerminalElectricityResponse> infos = new List<TerminalElectricityResponse>();
            while (reader.Read())
            {
                 infos.Add(new TerminalElectricityResponse
                 {
                     Id = reader.GetInt322("Id"),
                     Tid = reader.GetInt322("Tid"),
                     Electricity = reader.GetDouble2("Electricity"),
                     LLTime = reader.GetString2("LLTime"),
                     Name = reader.GetString2("Name"),
                     No = reader.GetString2("No"),
                     TType = reader.GetString2("TType"),
                     Type = reader.GetString2("Type"),
                 });   
            }
            reader.Close();
            opr.DisConnected();
            response.Data = infos;
            return response;

        }
    }
}
