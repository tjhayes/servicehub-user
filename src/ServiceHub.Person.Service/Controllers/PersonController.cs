using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using LM = ServiceHub.Person.Library.Models;
using CM = ServiceHub.Person.Context.Models;
using System.Collections.Generic;

namespace ServiceHub.Person.Service.Controllers
{

    /// <summary>
    /// This Controller will provide data from our DB for the person service.
    /// </summary>
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class PersonController : Controller
    {
        private CM.PersonRepository _Repo;

        public PersonController(CM.PersonRepository repo)
        { 
            _Repo = repo;
        }

        /// <summary>
        /// This is a GET method that returns all the documents from MongoDB.
        /// </summary>
        /// <returns>A list of Documents</returns>
        [HttpGet]
        public async Task<IEnumerable<LM.Person>> Get()
        {
            return await _Repo.GetAll();
        }


        /// <summary>
        /// This will perform GET operation based on Email field.
        /// </summary>
        /// <param name="email">Email id as string</param>
        /// <returns>A document from Database, if unsuccessfull, returns 404</returns>
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

        [HttpGet("name/{name}")]
        public async Task<IActionResult> GetByName(string name)
        {
            var result2 = await _Repo.GetAll();
            var result = result2.FirstOrDefault(p => p.Name == name);
            if (result is null)
            {
                return NotFound();
            }
            return Ok(result);
        }
    }
}
