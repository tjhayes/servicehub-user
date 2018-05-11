using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using CM = ServiceHub.Person.Context.Models;
using System.Collections.Generic;
using ServiceHub.Person.Context.Interfaces;

namespace ServiceHub.Person.Service.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class PersonController : Controller
    {
//        private CM.PersonRepository _Repo;
        private IRepository<CM.Person> _Repo;
        public PersonController(IRepository<CM.Person> repo)
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
            var result = result2.FirstOrDefault(p => p.EMail == email);
            if (result is null)
            {
                return NotFound();
            }
            return Ok(result);
        }
    }
}
