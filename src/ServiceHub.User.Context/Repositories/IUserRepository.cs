using System;
using System.Linq;

namespace ServiceHub.User.Context.Repositories
{
    /// <summary>
    /// Repository interface for User model with CRUD functionality.
    /// </summary>
    public interface IUserRepository
    {
        void Insert(Context.Models.User user);
        IQueryable<Context.Models.User> Get();
        Context.Models.User GetById(Guid id);
        void Update(Context.Models.User user);
        void Delete(Guid id);
        void DeleteAll();
    }
}
