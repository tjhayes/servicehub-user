using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
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

        [HttpPost]
        [Route("seed")]
        public async Task<IActionResult> Post()
        {
            try
            {
                const string connectionString = @"mongodb://db";
                IMongoCollection<User.Context.Models.User> mc =
                    new MongoClient(connectionString)
                        .GetDatabase("userdb")
                        .GetCollection<User.Context.Models.User>("users");

                UserStorage context = new UserStorage(new UserRepository(mc));
                //string jsonStr = System.IO.File.ReadAllText("../MockUsers.json");
                string jsonStr = DbSeeder.GetUsers();
                List<User.Context.Models.User> users = 
                    Deserialize<List<User.Context.Models.User>>(jsonStr);

                foreach (var user in users)
                {
                    await Task.Run(() => context.Insert(user));
                }
            }
            catch(Exception e)
            {
                return await Task.Run(() => BadRequest(e));
            }

            return await Task.Run(() => Ok());
        }

        // Deserialize JSON string and return object.
        private T Deserialize<T>(string jsonStr)
        {
            T obj = default(T);
            MemoryStream ms = new MemoryStream();
            try
            {
                DataContractJsonSerializer ser =
                    new DataContractJsonSerializer(typeof(T));
                StreamWriter writer = new StreamWriter(ms);
                writer.Write(jsonStr);
                writer.Flush();
                ms.Position = 0;
                obj = (T)ser.ReadObject(ms);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                ms.Close();
            }
            return obj;
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
                if(libraryUsers == null) { return await Task.Run(() => new StatusCodeResult(500)); }
                return await Task.Run(() => Ok(libraryUsers));
            }
            catch
            {
                return await Task.Run(() => new StatusCodeResult(500));
                
            }
        }

        /// <summary>
        /// Finds the user based on the id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>the user with matching Id, or a 404 error</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(Library.Models.User))]
        public async Task<IActionResult> Get(Guid id)
        {
            try
            {
                var libraryUser = UserModelMapper.ContextToLibrary(_userStorage.GetById(id));
                if (libraryUser == null)
                {
                    return await Task.Run(() => NotFound());
                }
                else
                {
                    return await Task.Run(() => Ok(libraryUser));
                }
            }
            catch (Exception e)
            {
                return await Task.Run(() => NotFound(e));
            }
        }

        /// <summary>
        /// Finds the users by Gender.
        /// </summary>
        /// <param name="gender"></param>
        /// <returns>all users with the specified gender, or a 400 error
        /// if the gender isn't valid, or a 500 error if a database error
        /// occured.</returns>
        [HttpGet("gender/{gender}")]
        [ProducesResponseType(500)]
        [ProducesResponseType(400)]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Library.Models.User>))]
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
                return await Task.Run(() => BadRequest($"Invalid gender: {gender}."));
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
                        if(libraryUser == null) { return await Task.Run(() => new StatusCodeResult(500)); }
                        GUsers.Add(libraryUser);
                    }
                }
                return await Task.Run(() => Ok(GUsers));
            }
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
            if(type == null) { return await Task.Run(() =>  BadRequest("Invalid type.")); }
            bool isValidType = false;
            foreach (var validType in Library.Models.User.ValidUppercaseTypes)
            {
                if (type.ToUpper() == validType) { isValidType = true; }
            }
            if (!isValidType) { return await Task.Run(() => BadRequest("Invalid type.")); }

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
                return await Task.Run(() => Ok(libraryUsers));
            }
            catch
            {
                return await Task.Run(() => new StatusCodeResult(500));
            }
        }

        /// <summary>
        /// Updates the user's address and/or location
        /// </summary>
        /// <param name="user">the user to update</param>
        /// <returns>200 Ok if the update is successful, 400 Bad Request
        /// if the user id, location or address are invalid, or 500
        /// Internal Server Error if a database error occurs.</returns>
        [HttpPut]
        [ProducesResponseType(500)]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        public async Task<IActionResult> Put([FromBody]ServiceHub.User.Library.Models.User user)
        {
            try
            {
                if (user == null)
                {
                    return await Task.Run(() => BadRequest("Invalid user: object was null"));
                }
                else
                {
                    var id = user.UserId;
                    if(user.UserId == Guid.Empty) { return await Task.Run(() => BadRequest("Invalid User Id")); }
                    var contextUser = _userStorage.GetById(user.UserId);
                    if(contextUser == null) { return await Task.Run(() => BadRequest("User not found")); }
                    var libraryUser = UserModelMapper.ContextToLibrary(contextUser);
                    if(libraryUser == null) { return await Task.Run(() => new StatusCodeResult(500)); }

                    if(user.Location != null) { libraryUser.Location = user.Location; }
                    libraryUser.Address = user.Address;
                    contextUser = UserModelMapper.LibraryToContext(libraryUser);
                    if(contextUser == null) { return await Task.Run(() => BadRequest("Invalid update of location or address.")); }
                    _userStorage.Update(contextUser);
                    return await Task.Run(() => Ok());
                }
            }
            catch
            {
                return await Task.Run(() => new StatusCodeResult(500));
            }
       }

        /// <summary>
        /// Creates a new user.
        /// </summary>
        /// <param name="user"> A User model to be provided from an external source
        /// via JSON.  If the model is a valid model, it will be cast to a db-ready model
        /// and stored in the database. </param>
        /// <returns> If the user is accepted, it will return a 202, Accepted code.
        /// Otherwise, it will return a 400, client-error code. </returns>
        [HttpPost]
        [ProducesResponseType(400)]
        [ProducesResponseType(202)]
        public async Task<IActionResult> Post([FromBody] User.Library.Models.User user)
        {
            if(user == null) { return await Task.Run(() => BadRequest("Invalid user: User is null")); }
            user.UserId = Guid.NewGuid();
            var contextUser = UserModelMapper.LibraryToContext(user);
            if(contextUser == null) { return await Task.Run(() => BadRequest("Invalid user: Validation failed")); }
            _userStorage.Insert(contextUser);
            return await Task.Run(() => Accepted());
        }
    }
}
