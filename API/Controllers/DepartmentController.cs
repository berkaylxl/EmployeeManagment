using API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System.Linq;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public DepartmentController(IConfiguration configuration)
        {
            _configuration = configuration;

        }

        [HttpGet]
        public JsonResult Get()
        {
            MongoClient mongoClient = new MongoClient(_configuration.GetConnectionString("EmployeeAppCon"));
            var dbList = mongoClient.GetDatabase("testdb").GetCollection<Department>("Department").AsQueryable();
            return new JsonResult(dbList);
        }

        [HttpPost]
        public JsonResult Post(Department dep)
        {
            MongoClient mongoClient = new MongoClient(_configuration.GetConnectionString("EmployeeAppCon"));
            int LastDepartmenId = mongoClient.GetDatabase("testdb").GetCollection<Department>("Department").AsQueryable().Count();
            dep.DepartmentId = LastDepartmenId + 1;
            mongoClient.GetDatabase("testdb").GetCollection<Department>("Department").InsertOne(dep);
            return new JsonResult("Added Successfully");
        }
        [HttpPut]
        public JsonResult Put (Department dep)
        {
            MongoClient mongoClient = new MongoClient(_configuration.GetConnectionString("EmployeeAppCon"));
            var filter = Builders<Department>.Filter.Eq("DepartmentId", dep.DepartmentId);
            var update = Builders<Department>.Update.Set("DepartmentName", dep.DepartmentName);
            mongoClient.GetDatabase("testdb").GetCollection<Department>("Department").UpdateOne(filter,update);
            return new JsonResult("Updated successfully");


        }
        [HttpDelete]
        public JsonResult Delete(int id)
        {
            MongoClient mongoClient = new MongoClient(_configuration.GetConnectionString("EmployeeAppCon"));
            var filter = Builders<Department>.Filter.Eq("DepartmentId",id);
            mongoClient.GetDatabase("testdb").GetCollection<Department>("Department").DeleteOne(filter);
            return new JsonResult("Deleted successfully");
        }
    }
}
