using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ServiceHub.User.Context.Repositories;
using ServiceHub.User.Context.Utilities;

namespace ServiceHub.User.Service.Controllers
{
    [Route("api/[controller]")]
    public class TempUserController : Controller
    {
        private readonly UserStorage _userStorage;

        public TempUserController(IUserRepository userRepository)
        {
            _userStorage = new UserStorage(userRepository);
        }

        /// <summary>
        /// Get all users.
        /// </summary>
        /// <returns>OkObjectResult with an IEnumerable of all users,
        /// or a 500 StatusCodeResult if an error occurs.</returns>
        [HttpGet]
        [ProducesResponseType(500)]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Library.Models.User>))]
        public async Task<IActionResult> Get()
        {
            try
            {
                var contextUsers = _userStorage.Get();
                var libraryUsers = UserModelMapper.List_ContextToLibrary(contextUsers);
                if(libraryUsers == null) { return new StatusCodeResult(500); }
                return await Task.Run(() => Ok(libraryUsers));
            }
            catch
            {
                return new StatusCodeResult(500);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            return await Task.Run(() => Ok(_userStorage.GetById(id)));
        }

        /// <summary>
        /// Gets all users of a certain type.
        /// </summary>
        /// <param name="type"> A string representing the type of user to filter by. </param>
        /// <returns>If the type is not an accepted type, returns a 400 StatusCodeResult. If
        /// the server fails to return a complete list of valid users, returns a 500
        /// StatusCodeResut. Otherwise returns a list of validated Users. </returns>
        [HttpPost]
        [Route("type")]
        [ProducesResponseType(500)]
        [ProducesResponseType(400)]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Library.Models.User>))]
        public async Task<IActionResult> GetByType([FromBody] string type)
        {
            if(type == null) { return BadRequest("Invalid type."); }
            bool isValidType = false;
            foreach (var validType in Library.Models.User.ValidUppercaseTypes)
            {
                if (type.ToUpper() == validType) { isValidType = true; }
            }
            if (!isValidType) { return BadRequest("Invalid type."); }

            try
            {
                var users = await Task.Run(() => _userStorage.Get());
                var contextUsers = new List<Context.Models.User>();
                foreach (var contextUser in users)
                {
                    if(contextUser.Type.ToUpper() == type.ToUpper())
                    {
                        contextUsers.Add(contextUser);
                    }
                }
                var libraryUsers = UserModelMapper.List_ContextToLibrary(contextUsers);
                return Ok(libraryUsers);
            }
            catch
            {
                return new StatusCodeResult(500);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]object value)
        {
            return await Task.Run(() => Ok());
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody]object value)
        {
            return await Task.Run(() => Ok());
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return await Task.Run(() => Ok());
        }
    }
}
