using ServiceHub.Person.Library.Abstracts;
using ServiceHub.Person.Library.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using CM = ServiceHub.Person.Context.Models;

namespace ServiceHub.Person.Service.Controllers
{
    /// <summary>
    /// This Controller will extend AModelController abstract class, used for Person service.
    /// </summary>
    [Route("api/[controller]")]
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

        [HttpGet("name/{name}")]
        public async Task<IActionResult> GetByName(string name)
        {
            var result2 = await Repo.GetAll();

            var result = result2.FirstOrDefault(p => p.Name == name);

            if (result is null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpGet("batchName/{batchName}")]
        public async Task<IActionResult> GetByBatchName(string batchName)
        {
            var result2 = await Repo.GetAll();

            var result = result2.Where(p => p.BatchName == batchName);

            if (result is null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpGet("phone/{phone}")]
        public async Task<IActionResult> GetByPhone(string phone)
        {
            var result2 = await Repo.GetAll();

            var result = result2.FirstOrDefault(p => p.Phone == phone);

            if (result is null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpGet("hasCar/{hasCar}")]
        public async Task<IActionResult> GetByHasCar(bool hasCar)
        {
            var result2 = await Repo.GetAll();

            var result = result2.Where(p => p.HasCar == hasCar);

            if (result is null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpGet("isMale/{isMale}")]
        public async Task<IActionResult> GetByIsMale(bool isMale)
        {
            var result2 = await Repo.GetAll();

            var result = result2.Where(p => p.IsMale == isMale);

            if (result is null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpGet("address/{address}")]
        public async Task<IActionResult> GetByAddress(string address)
        {
            var result2 = await Repo.GetAll();

            var result = result2.Where(p => p.Address == address);

            if (result is null)
            {
                return NotFound();
            }

            return Ok(result);
        }
    }
}
