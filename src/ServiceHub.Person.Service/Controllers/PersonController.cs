using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using CM = ServiceHub.Person.Context.Models;
using System.Collections.Generic;
using System;

namespace ServiceHub.Person.Service.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class PersonController : Controller
    {
        private CM.PersonRepository _Repo;

        private void _updateDatabase()
        {
            var now = DateTime.Now;
            var TimeStamp = _Repo.GetById("5af0953c153254170c367ef1").Result; //TODO: place _id in json after creating the document in CosmosDB
            var then = TimeStamp.LastModified;
            var diff = now.Subtract(then);
            var period = TimeSpan.FromHours(12.0);
            bool doCheck = (TimeSpan.Compare(diff, period) > 0);
            if (doCheck)
            {
                Console.WriteLine("database needs updating");
                // call context object to update database
            }
            else
            {
                Console.WriteLine("proceed, no update needed.");
                return;
            }
        }

        public PersonController(CM.PersonRepository repo)
        { 
            _Repo = repo;
        }

        [HttpGet]
        public async Task<IEnumerable<CM.Person>> Get()
        {
            return await _Repo.GetAll();
        }

        [HttpGet("email/{email}")]
        public async Task<IActionResult> GetByEmail(string email)
        {
            var result2 = await _Repo.GetAll();
            var result = result2.FirstOrDefault(p => p.Email == email);
            if (result is null)
            {
                return NotFound();
            }
            return Ok(result);
        }
    }
}
