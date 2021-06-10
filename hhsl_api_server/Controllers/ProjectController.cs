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
    public class ProjectController : ApiController
    {
        [HttpGet]
        public ApiResponse List()
        {
            ApiResponse response = new ApiResponse();
            MySqlOperator opr = new MySqlOperator();
            opr.Connect();
            var sql = $"SELECT * FROM project_info";
            var reader = opr.Reader(sql);
            List<ProjectInfoEntity> projectList = new List<ProjectInfoEntity>();
            while (reader.Read())
            {
                projectList.Add(new ProjectInfoEntity
                {
                    Address = reader.GetString2("Address"),
                    Desc = reader.GetString2("Desc"),
                    Id = reader.GetInt322("Id"),
                    LatLong = reader.GetString2("LatLong"),
                    Location = reader.GetString2("Location"),
                    Pic = reader.GetString2("Pic"),
                    Type = reader.GetString2("Type"),
                    Name = reader.GetString2("Name"),

                });
            }
            opr.DisConnected();
            response.Data = projectList;
            return response;
        }

        [HttpPost]
        public ApiResponse Add([FromBody] ProjectInfoEntity project)
        {
            ApiResponse response = new ApiResponse();
            MySqlOperator opr = new MySqlOperator();
            opr.Connect();
            var sql =
                $"INSERT INTO " +
                $"project_info(Name, `Desc`, Address, LatLong, Location, Pic, Type) " +
                $"VALUES ('{project.Name}', '{project.Desc}', '{project.Address}', " +
                $"'{project.LatLong}', '{project.Location}', '{project.Pic}', '{project.Type}')";
            opr.Execute(sql);
            opr.DisConnected();
            return response;
        }

        [HttpPost]
        public ApiResponse Edit([FromBody] ProjectInfoEntity project)
        {
            ApiResponse response = new ApiResponse();
            MySqlOperator opr = new MySqlOperator();
            opr.Connect();
            var sql =
                $"UPDATE project_info SET " +
                $"Name = '{project.Name}', `Desc` = '{project.Desc}', Address = '{project.Address}', " +
                $"LatLong = '{project.LatLong}', Location = '{project.Location}', " +
                $"Pic = '{project.Pic}', Type = '{project.Type}' " +
                $"WHERE Id = {project.Id}";
            opr.Execute(sql);
            opr.DisConnected();
            return response;
        }

        [HttpGet]
        public ApiResponse Del(int id)
        {
            ApiResponse response = new ApiResponse();
            MySqlOperator opr = new MySqlOperator();
            opr.Connect();
            var sql = $"DELETE FROM project_info WHERE Id = {id}";
            opr.Execute(sql);
            opr.DisConnected();
            return response;
        }

        [HttpGet]
        public ApiResponse Get(int id)
        {
            ApiResponse response = new ApiResponse();
            MySqlOperator opr = new MySqlOperator();
            opr.Connect();
            var sql = $"SELECT * FROM project_info WHERE Id = {id}";
            var reader = opr.Reader(sql);
            ProjectInfoEntity project = null;
            if(reader.Read())
            {
                project = new ProjectInfoEntity
                {
                    Address = reader.GetString2("Address"),
                    Desc = reader.GetString2("Desc"),
                    Id = reader.GetInt322("Id"),
                    LatLong = reader.GetString2("LatLong"),
                    Location = reader.GetString2("Location"),
                    Pic = reader.GetString2("Pic"),
                    Type = reader.GetString2("Type"),
                    Name = reader.GetString2("Name"),

                };
            }
            opr.DisConnected();
            response.Data = project;
            return response;
        }
    }
}
