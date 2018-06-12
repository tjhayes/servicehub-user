using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Logging;
using ServiceHub.User.Context.Repositories;

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

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            return await Task.Run(() => Ok(_userStorage.GetById(id)));
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
