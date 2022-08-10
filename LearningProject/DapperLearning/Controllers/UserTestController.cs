using Dapper;
using DapperLearning.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace DapperLearning.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserTestController : ControllerBase
    {
        private readonly IConfiguration configuration;
        public UserTestController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        [HttpGet]
        public async Task<ActionResult<List<TestUser>>> GetUsers()
        {
            using var connection = new SqlConnection(configuration.GetConnectionString("default"));
            var res = await connection.QueryAsync<TestUser>("select * from UsersTest");
            return Ok(res);
        }

        [HttpGet("GetBy")]
        public async Task<ActionResult<TestUser>> GetUserById(int ID)
        {
            using var connection = new SqlConnection(configuration.GetConnectionString("default"));
            var res = await connection.QueryAsync<TestUser>("select * from UsersTest where ID=@ID", new { ID = ID });
            return Ok(res);
        }

        [HttpPost]
        public async Task<ActionResult<List<TestUser>>> CreateUser(TestUser testUser)
        {
            using var connection = new SqlConnection(configuration.GetConnectionString("default"));
            var res = await connection.ExecuteAsync("insert into UsersTest ([Name],[Surname]) values (@Name,@Surname) ", testUser);
            return Ok(await AllUsers(connection));
        }


        [HttpPut]
        public async Task<ActionResult<List<TestUser>>> UpdateUser(TestUser testUser)
        {
            using var connection = new SqlConnection(configuration.GetConnectionString("default"));
            var res = await connection.ExecuteAsync("update UsersTest set  [Name]=@Name , [Surname]=@Surname where ID=@ID", new { Name = testUser.Name, Surname = testUser.Surname, ID = testUser.ID });
            return Ok(await AllUsers(connection));
        }
        [HttpDelete("ID")]
        public async Task<ActionResult<List<TestUser>>> DeleteUser(int ID)
        {
            using var connection = new SqlConnection(configuration.GetConnectionString("default"));
            var res = await connection.ExecuteAsync("delete UsersTest where ID=@ID", new {  ID = ID });
            return Ok(await AllUsers(connection));
        }
        private static async Task<IEnumerable<TestUser>> AllUsers(SqlConnection connection)
        {
             return await connection.QueryAsync<TestUser>("select * from UsersTest");
        }

    }
}
