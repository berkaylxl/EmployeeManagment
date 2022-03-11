using API.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.IO;
using System.Linq;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;
        public EmployeeController(IConfiguration configuration,IWebHostEnvironment environment)
        {
            _configuration = configuration;
            _env = environment;

        }

        [HttpGet]
        public JsonResult Get()
        {
            MongoClient mongoClient = new MongoClient(_configuration.GetConnectionString("EmployeeAppCon"));
            var dbList = mongoClient.GetDatabase("testdb").GetCollection<Employee>("Employee").AsQueryable();
            return new JsonResult(dbList);
        }

        [HttpPost]
        public JsonResult Post(Employee employee)
        {
            MongoClient mongoClient = new MongoClient(_configuration.GetConnectionString("EmployeeAppCon"));
            int LastEmployeeId = mongoClient.GetDatabase("testdb").GetCollection<Employee>("Employee").AsQueryable().Count();
            employee.EmployeeId = LastEmployeeId + 1;
            mongoClient.GetDatabase("testdb").GetCollection<Employee>("Employee").InsertOne(employee);
            return new JsonResult("Added Successfully");
        }
        [HttpPut]
        public JsonResult Put(Employee employee)
        {
            MongoClient mongoClient = new MongoClient(_configuration.GetConnectionString("EmployeeAppCon"));
            var filter = Builders<Employee>.Filter.Eq("EmployeeId", employee.EmployeeId);
            var update = Builders<Employee>.Update.Set("EmployeeName", employee.EmployeeName)
                                                   .Set("PhotoFileName", employee.PhotoFileName)
                                                   .Set("DateOfJoining", employee.DateOfJoining)
                                                   .Set("Department", employee.Department);                                        

            mongoClient.GetDatabase("testdb").GetCollection<Employee>("Employee").UpdateOne(filter, update);
            return new JsonResult("Updated successfully");


        }
        [HttpDelete]
        public JsonResult Delete(int id)
        {
            MongoClient mongoClient = new MongoClient(_configuration.GetConnectionString("EmployeeAppCon"));
            var filter = Builders<Employee>.Filter.Eq("EmployeeId", id);
            mongoClient.GetDatabase("testdb").GetCollection<Employee>("Employee").DeleteOne(filter);
            return new JsonResult("Deleted successfully");
        }


        [Route("saveFile")]
        public JsonResult SaveFile()
        {
            try
            {
                var httpRequest = Request.Form;
                var postedFile = httpRequest.Files[0];
                string fileName =postedFile.FileName;
                var physicalPath = _env.ContentRootPath + "/Photos/" + fileName;
                using (var stream=new FileStream(physicalPath,FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                }
                return new JsonResult(fileName);
            }
            catch (Exception)
            {
                return new JsonResult("anonymous.png");
               
            }
        }
    }
}
