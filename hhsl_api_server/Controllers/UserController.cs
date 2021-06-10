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
    public class UserController : ApiController
    {
        [HttpPost]
        public ApiResponse Login([FromBody] UserInfoEntity user)
        {
            ApiResponse response = new ApiResponse();
            MySqlOperator opr = new MySqlOperator();
            opr.Connect();

            // 判断用户是否存在
            var sql = $"SELECT Id FROM userinfo WHERE Account = '{user.Account}' AND Pwd = '{user.Pwd}'";
            var idObj = opr.ExecuteScalar(sql);
            var id = Convert.ToInt32(idObj);
            if (id == 0)
            {
                response.Code = ApiResponseCode.Error;
                response.Msg = "登录失败：用户名或密码错误！";
            }
            else
            {
                // 更新最后登录时间
                var sqlUpdateTime = $"UPDATE userinfo " +
                                    $"SET LLTime = '{TimeTool.Now()}' " +
                                    $"WHERE Id = {id}";
                opr.Execute(sqlUpdateTime);
            }
            opr.DisConnected();
            return response;
        }

        [HttpPost]
        public ApiResponse Add([FromBody] UserInfoEntity user)
        {
            ApiResponse response = new ApiResponse();
            MySqlOperator opr = new MySqlOperator();
            opr.Connect();
            // 判断账号是否存在

            var exitsSql = $"SELECT COUNT(1) FROM userinfo WHERE Account = '{user.Account}'";
            var countObj = opr.ExecuteScalar(exitsSql);
            var count = Convert.ToInt32(countObj);
            if (count != 0)
            {
                response.Code = ApiResponseCode.Error;
                response.Msg = "添加失败：该账号已存在！";
            }
            else
            {
                // 添加人员
                var sql = $"INSERT INTO " +
                          $"userinfo(Account, Pwd, Email, Phone, Type, Name, ) " +
                          $"VALUES(" +
                          $"'{user.Account}', '{user.Pwd}', '{user.Email}', " +
                          $"'{user.Phone}', {user.Type}, '{user.Name}'" +
                          $")";
                opr.Execute(sql);
            }
            opr.DisConnected();
            return response;
        }


        [HttpGet]
        public ApiResponse List(int pIndex, int count)
        {
            ApiResponse response = new ApiResponse();
            MySqlOperator opr = new MySqlOperator();
            opr.Connect();
            var sql = $"SELECT Id, Account, Name, Phone, Email, Type, LLTime " +
                      $"FROM userinfo " +
                      $"LIMIT {(pIndex - 1) * count}, {count}";

            // page count select
            var sqlPage = $"SELECT COUNT(1) " +
                          $"FROM userinfo";
            var totalObj = opr.ExecuteScalar(sqlPage);
            var total = Convert.ToInt32(totalObj);

            if (total == 0)
            {
                opr.DisConnected();
                return response;
            }

            var reader = opr.Reader(sql);
            List<object> users = new List<object>();
            while (reader.Read())
            {
                users.Add(new
                {
                    Account = reader.GetString2("Account"),
                    Name = reader.GetString2("Name"),
                    Phone = reader.GetString2("Phone"),
                    Email = reader.GetString2("Email"),
                    Type = reader.GetInt322("Type"),
                    LLTime = reader.GetString2("LLTime"),
                    Id = reader.GetInt322("Id"),
                });
            }
            reader.Close();
            opr.DisConnected();

            response.Data = new PageResponseEntity { Index = pIndex, Total = total, Count = count, Data = users};
            return response;

        }


        [HttpGet]
        public ApiResponse Del(int id)
        {
            ApiResponse response = new ApiResponse();
            MySqlOperator opr = new MySqlOperator();
            opr.Connect();
            var sql = $"DELETE FROM userinfo WHERE Id = {id}";
            opr.Execute(sql);
            opr.DisConnected();
            return response;

        }
    }
}
