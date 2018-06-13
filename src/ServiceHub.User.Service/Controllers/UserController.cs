using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Logging;
using ServiceHub.User.Context.Repositories;
using ServiceHub.User.Context.Utilities;

namespace ServiceHub.User.Service.Controllers
{
    [Route("api/[controller]")]
    public class UserController : BaseController
    {
        private readonly UserStorage _userStorage;

        public UserController(ILoggerFactory loggerFactory /*, IQueueClient queueClientSingleton*/)
          : base(loggerFactory /*, queueClientSingleton*/)
        {
            _userStorage = new UserStorage(new UserRepository());
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
                return await Task.Run(() => Ok(_userStorage.GetById(id)));
            }
            catch(Exception e)
            {
                return NotFound(e);
            }
        }

        /// <summary>
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
                if(upperGender == x)
                {
                    validGender = true;
                }
            }

            if (!validGender)
            {
                return BadRequest();
            }
            else
            {
                if (gender.ToUpper() == "M")
                {
                    var users = _userStorage.Get();
                    var GUsers = new List<ServiceHub.User.Library.Models.User>();

                    foreach (var x in users)
                    {
                        if (x.Gender[0].ToString().ToUpper() == "M")
                        {
                            GUsers.Add(x);
                        }
                    }
                    return await Task.Run(() => Ok(GUsers));
                }
                else if (gender.ToUpper() == "F")
                {
                    var users = _userStorage.Get();
                    var GUsers = new List<ServiceHub.User.Library.Models.User>();

                    foreach (var x in users)
                    {
                        if (x.Gender[0].ToString().ToUpper() == "M")
                        {
                            GUsers.Add(x);
                        }
                    }
                    return await Task.Run(() => Ok(GUsers));
                }
                else
                {
                    return BadRequest();
                }
            }
        }

        [HttpGet("{type}")]
        public async Task<IActionResult> GetByType(string type)
        {
            string[] types = ServiceHub.User.Library.Models.User.ValidUppercaseTypes;
            string upperType = type.ToUpper();
            bool validType = false;

            foreach (var x in types)
            {
                if (upperType == x)
                {
                    validType = true;
                }
            }

            if(validType)
            {
                var users = _userStorage.Get();
                var TUsers = new List<ServiceHub.User.Library.Models.User>();

                foreach (var x in users)
                {
                    if (x.Type.ToUpper() == upperType)
                    {
                        TUsers.Add(x);
                    }
                }
                return await Task.Run(() => Ok(TUsers));
            }
            else
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Updates the user of the id of the new model.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPut()]
        public async Task<IActionResult> Put(ServiceHub.User.Library.Models.User value)
        {
            try
            { 
                _userStorage.Update(value);
                return await Task.Run(() => Ok());
            }
            catch(Exception e)
            {
                return BadRequest();
            }
        }


        //[HttpDelete("{id}")]
        //public async Task<IActionResult> Delete(int id)
        //{
        //    return await Task.Run(() => Ok());
        //}

        protected override void UseReceiver()
        {
            var messageHandlerOptions = new MessageHandlerOptions(ReceiverExceptionHandler)
            {
                AutoComplete = false
            };

            //queueClient.RegisterMessageHandler(ReceiverMessageProcessAsync, messageHandlerOptions);
        }

        protected override void UseSender(Message message)
        {
            Task.Run(() =>
              SenderMessageProcessAsync(message)
            );
        }
    }
}
