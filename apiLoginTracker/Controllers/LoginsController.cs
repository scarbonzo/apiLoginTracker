using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1apiLoginTracker.Models;
using MongoDB.Driver;
using apiLoginTracker;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1apiLoginTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginsController : ControllerBase
    {
        // GET: api/<LoginsController>
        [HttpGet]
        public IEnumerable<Login> Get(int skip = 0, int take = 10)
        {
            try
            {
                var dbClient = new MongoClient(configuration.dbconnection);
                var database = dbClient.GetDatabase(configuration.dbname);
                var collection = database.GetCollection<Login>(configuration.dbcollection);

                var result = collection.AsQueryable()
                    .OrderByDescending(l => l.Timestamp)
                    .Skip(skip)
                    .Take(take)
                    .ToList();

                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        // GET api/<LoginsController>/5
        [HttpGet("{id}")]
        public Login Get(Guid id)
        {
            try
            {
                var dbClient = new MongoClient(configuration.dbconnection);
                var database = dbClient.GetDatabase(configuration.dbname);
                var collection = database.GetCollection<Login>(configuration.dbcollection);

                var result = collection.AsQueryable()
                    .FirstOrDefault(l => l.Id == id);

                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }
               
    }
}
