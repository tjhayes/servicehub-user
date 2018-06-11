using System;
using System.Linq;

namespace ServiceHub.User.Context.Repositories
{
    /// <summary>
    /// Repository interface for User model with CRUD functionality.
    /// </summary>
    public interface IUserRepository
    {
        void Insert(Library.Models.User user);
        IQueryable<Library.Models.User> Get();
        Library.Models.User GetById(Guid id);
        void Update(Library.Models.User user);
        void Delete(Guid id);
        void DeleteAll();
    }
}
