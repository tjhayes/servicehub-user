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

        /// <summary>
        /// Finds the user based on the id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            try
            {
                var libraryUser = UserModelMapper.ContextToLibrary(_userStorage.GetById(id));
                if (libraryUser == null)
                {
                    return NotFound();
                }
                else
                {
                    return await Task.Run(() => Ok(libraryUser));
                }
            }
            catch (Exception e)
            {
                return NotFound(e);
            }
        }

        /// Finds the users by Gender.
        /// </summary>
        /// <param name="gender"></param>
        /// <returns></returns>
        [HttpGet("{gender}")]
        public async Task<IActionResult> GetByGender(string gender)
        {
            string[] genders = ServiceHub.User.Library.Models.User.ValidUppercaseGenders;
            string upperGender = gender.ToUpper();
            bool validGender = false;

            foreach (var x in genders)
            {
                if (upperGender == x)
                {
                    validGender = true;
                }
            }

            if (!validGender)
            {
                return BadRequest($"Invalid gender: {gender}.");
            }
            else
            {
                var users = _userStorage.Get();
                var GUsers = new List<ServiceHub.User.Library.Models.User>();

                foreach (var x in users)
                {
                    if(x.Gender.ToUpper() == upperGender)
                    {
                        var libraryUser = UserModelMapper.ContextToLibrary(x);
                        if(libraryUser == null) { return new StatusCodeResult(500); }
                        GUsers.Add(libraryUser);
                    }
                }
                return await Task.Run(() => Ok(GUsers));
            }
        }

     
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


        /// Updates the user of the id of the new model.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPut()]
        public async Task<IActionResult> Put(ServiceHub.User.Library.Models.User user)
        {
            try
            {
                if (user == null)
                {
                    return BadRequest("Invalid user: object was null");
                }
                else
                {
                    var id = user.UserId;
                    if(user.UserId == Guid.Empty) { return BadRequest("Invalid User Id"); }
                    var contextUser = _userStorage.GetById(user.UserId);
                    if(contextUser == null) { return BadRequest("User not found"); }
                    var libraryUser = UserModelMapper.ContextToLibrary(contextUser);
                    if(libraryUser == null) { return new StatusCodeResult(500); }

                    if(user.Location != null) { libraryUser.Location = user.Location; }
                    if(user.Address != null) { libraryUser.Address = user.Address; }

                    contextUser = UserModelMapper.LibraryToContext(libraryUser);
                    if(contextUser == null) { return BadRequest("Invalid update of location or address."); }
                    _userStorage.Update(contextUser);
                    return await Task.Run(() => Ok());
                }
            }
            catch
            {
                return new StatusCodeResult(500);
            }
       }

        /// Creates a new user.
        /// </summary>
        /// <param name="type"> A User model to be provided from an external source
        /// via JSON.  If the model is a valid model, it will be cast to a db-ready model
        /// and stored in the database. </param>
        /// <returns> If the user is accepted, it will return a 201, Accepted code.
        /// Otherwise, it will return a 400, client-error code. </returns>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] User.Library.Models.User user)
        {
            if(user == null) { return BadRequest("Invalid user: User is null"); }
            var contextUser = UserModelMapper.LibraryToContext(user);
            if(contextUser == null) { return BadRequest("Invalid user: Validation failed"); }
            _userStorage.Insert(contextUser);
            return await Task.Run(() => Accepted());
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return await Task.Run(() => Ok());
        }
    }
}
