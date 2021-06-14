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
        public ActionResult Get(string machine, string username, string dc, bool desc, bool locks, bool unlocks, bool logons, bool logoffs, string gateway, DateTime? start, DateTime? end, int skip = 0, int take = 25)
        {
            try
            {
                var dbClient = new MongoClient(configuration.dbconnection);
                var database = dbClient.GetDatabase(configuration.dbname);
                var collection = database.GetCollection<Login>(configuration.dbcollection);

                var data = collection.AsQueryable();
                
                if(start == null)
                {
                    start = DateTime.Now.Date;
                }

                if (end == null)
                {
                    end = DateTime.Now.AddHours(23).AddMinutes(59).AddSeconds(59);
                }

                var result = data.Where(l => l.Timestamp > start)
                    .Where(l => l.Timestamp < end);

                if (desc)
                {
                    result = result.OrderByDescending(l => l.Timestamp);
                }
                
                if(!locks)
                {
                    result = result.Where(l => l.LoginType.ToLower() != "l");
                }

                if (!unlocks)
                {
                    result = result.Where(l => l.LoginType.ToLower() != "u");
                }

                if (!logons)
                {
                    result = result.Where(l => l.LoginType.ToLower() != "+");
                }

                if (!logoffs)
                {
                    result = result.Where(l => l.LoginType.ToLower() != "-");
                }

                if (machine != null)
                {
                    result = result.Where(l => l.Machine.ToLower().Contains(machine.ToLower()));
                }

                if (username != null)
                {
                    result = result.Where(l => l.Username.ToLower().Contains(username.ToLower()));
                }

                if (dc != null)
                {
                    result = result.Where(l => l.DomainController.ToLower().Contains(dc.ToLower()));
                }

                if (gateway != null)
                {
                    result = result.Where(l => l.Gateway.ToLower().Contains(gateway.ToLower()));
                }

                return Ok(result
                    .Skip(skip)
                    .Take(take)
                    .ToList());
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest(e);
            }
        }

        // GET api/<LoginsController>/5
        [HttpGet("{id}")]
        public ActionResult Get(Guid id)
        {
            try
            {
                var dbClient = new MongoClient(configuration.dbconnection);
                var database = dbClient.GetDatabase(configuration.dbname);
                var collection = database.GetCollection<Login>(configuration.dbcollection);

                var result = collection.AsQueryable()
                    .FirstOrDefault(l => l.Id == id);

                return Ok(result);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest(e);
            }
        }
               
    }
}
