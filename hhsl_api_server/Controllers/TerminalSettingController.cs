﻿using System;
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
            var sql = $"SELECT ts.*, ti.Name, ti.No, ti.Type as TType " +
                      $"FROM terminal_setting as ts " +
                      $"RIGHT JOIN terminal_info as ti ON ti.Id = ts.TId " +
                      $"LIMIT {(pIndex - 1) * count}, {count}";
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
                    LLTime = reader.GetString("LLTime"),
                    Name = reader.GetString("Name"),
                    No = reader.GetString("No"),
                    TType = reader.GetString("TType"), 
                });
            }
            reader.Close();
            opr.DisConnected();
            response.Data = setting;
            return response;

        }
        // add
        [HttpGet]
        public ApiResponse Add([FromBody] TerminalSettingEntity setting)
        {
            ApiResponse response = new ApiResponse();
            MySqlOperator opr = new MySqlOperator();
            opr.Connect();
            var sql = $"INSERT INTO " +
                      $"terminal_setting(TId, CollectInterval, CollectModel, LLTime, SendInterval, CollectTimes, SendStatus) " +
                      $"VALUES( {setting.TId}, {setting.CollectInterval}, " +
                      $"{setting.CollectModel}, '{setting.LLTime}', {setting.SendInterval}, " +
                      $"{setting.CollectTimes}, {setting.SendStatus})";
            opr.Execute(sql);
            opr.DisConnected();
            return response;
        }

        // edit
        [HttpGet]
        public ApiResponse Edit([FromBody] TerminalSettingEntity setting)
        {
            ApiResponse response = new ApiResponse();
            MySqlOperator opr = new MySqlOperator();
            opr.Connect();
            var sql = $"UPDATE terminal_setting " +
                      $"SET " +
                      $"CollectInterval = {setting.CollectInterval}, CollectModel = {setting.CollectModel}, " +
                      $"LLTime = '{setting.LLTime}', SendInterval = {setting.SendInterval}, " +
                      $"CollectTimes = {setting.CollectTimes}, " +
                      $"SendStatus = {setting.SendStatus} " +
                      $"WHERE Id = {setting.Id}";
            opr.Execute(sql);
            opr.DisConnected();
            return response;
        }
    }
}