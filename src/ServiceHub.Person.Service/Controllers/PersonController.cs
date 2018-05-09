using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using LM = ServiceHub.Person.Library.Models;
using CM = ServiceHub.Person.Context.Models;
using System.Collections.Generic;

namespace ServiceHub.Person.Service.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class PersonController : Controller
    {
        private CM.PersonRepository _Repo;

        public PersonController(CM.PersonRepository repo)
        { 
            _Repo = repo;
        }

        [HttpGet]
        public async Task<IEnumerable<LM.Person>> Get()
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
