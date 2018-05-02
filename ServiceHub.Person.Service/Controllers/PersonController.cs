using ServiceHub.Person.Library.Abstracts;
using ServiceHub.Person.Library.Interfaces;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System.Linq;
using System.Threading.Tasks;
using CM = ServiceHub.Person.Context.Models;

namespace ServiceHub.Person.Service.Controllers
{
    /// <summary>
    /// This Controller will extend AModelController abstract class, used for Person service.
    /// </summary>
    public class PersonController : AModelController<CM.Person>
    {
        public PersonController(IRepository<CM.Person> repo) : base(repo) { }

        /// <summary>
        /// This will perform GET operation based on Email field.
        /// </summary>
        /// <param name="email">Email id as string</param>
        /// <returns>A document from Database, if unsuccessfull, returns 404</returns>
        [HttpGet("email/{email}")]
        public async Task<IActionResult> GetByEmail(string email)
        {
            var result2 = await Repo.GetAll();

            var result = result2.FirstOrDefault(p => p.Email == email);

            if (result is null)
            {
                return NotFound();
            }

            return Ok(result);
        }
    }
}
