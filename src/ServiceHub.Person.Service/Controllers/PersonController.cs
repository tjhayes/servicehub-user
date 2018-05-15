﻿using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using CM = ServiceHub.Person.Context.Models;
using System.Collections.Generic;
using System;
using Microsoft.Extensions.Logging;
using ServiceHub.Person.Context.Interfaces;

namespace ServiceHub.Person.Service.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class PersonController : Controller
    {
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
            else
            {
                return;
            }
        }
        private IRepository<CM.Person> _Repo;
        public PersonController(IRepository<CM.Person> repo)
        { 
            _Repo = repo;
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
    }
}
