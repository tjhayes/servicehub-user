using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServiceHub.User.Context.Repositories
{
    /// <summary>
    /// Repository interface for User model with CRUD functionality.
    /// </summary>
    public interface IUserRepository
    {
        Task Insert(Context.Models.User user);
        Task<List<Context.Models.User>> Get();
        Task<Context.Models.User> GetById(Guid id);
        Task Update(Context.Models.User user);
        Task Delete(Guid id);
        Task DeleteAll();
    }
}
