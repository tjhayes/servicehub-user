using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using CM = ServiceHub.Person.Context.Models;
using System.Collections.Generic;
using System;
using ServiceHub.Person.Context.Interfaces;
using System.Net.Http;
using ServiceHub.Person.Library.Models;

namespace ServiceHub.Person.Service.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class PersonController : Controller
    {
        private readonly IRepository<CM.Person> _Repo;
        private readonly Settings _Settings;
        private void _updateDatabase()
        {
            var now = DateTime.UtcNow;//best to use UTC
            var then = _Repo.LastGlobalUpdateTime();
            var diff = now.Subtract(then);
            var period = TimeSpan.FromHours(12.0);
            bool doCheck = (TimeSpan.Compare(diff, period) > 0);
            if (doCheck)
            {
                _Repo.UpdateRepository();
            }
        }
        public PersonController(IRepository<CM.Person> repo, Settings settings)
        {
            _Repo = repo;
            _Settings = settings;
            _updateDatabase();
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

        [HttpGet("id/{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var result = await _Repo.GetById(id);

            if(result is null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var person = await _Repo.GetById(id);
            string urlAddition = "/" + person.ModelId;
            var result = await new HttpClient().DeleteAsync(_Settings.BaseURL + urlAddition);

            if (result.IsSuccessStatusCode)
            {
                await _Repo.DeleteById(id);
                return Ok();
            }
            return Ok(result);
        }
    }
}
