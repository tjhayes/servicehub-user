using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Logging;
using ServiceHub.User.Context.Repositories;
using System.Collections.Generic;

namespace ServiceHub.User.Service.Controllers
{
    [Route("api/[controller]")]
    public class UserController : BaseController
    {
        private readonly UserStorage _userStorage;

        public UserController(ILoggerFactory loggerFactory, IQueueClient queueClientSingleton)
          : base(loggerFactory, queueClientSingleton)
        {
            _userStorage = new UserStorage(new UserRepository());
        }

        /// <summary>
        /// Get all users.
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Get()
        {
            try
            {
                var users = _userStorage.Get();
                return await Task.Run(() => Ok(users));
            }
            catch(Exception e)
            {
                return NotFound();
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
        [HttpGet]
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

        /// <summary>
        /// Updates the user of the id of the new model.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
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

            queueClient.RegisterMessageHandler(ReceiverMessageProcessAsync, messageHandlerOptions);
        }

        protected override void UseSender(Message message)
        {
            Task.Run(() =>
              SenderMessageProcessAsync(message)
            );
        }
    }
}
