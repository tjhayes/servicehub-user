using System.Collections.Generic;
using System.Threading.Tasks;

namespace Housing.Foundation.Library.Interfaces
{
    /// <summary>
    /// This is an interface for AModelController which performs CRUD operations.
    /// </summary>
    /// <typeparam name="TModel">This can be Person, Apartment, or Batch</typeparam>
    public interface IRepository<TModel> where TModel : IModel
    {
        /// <summary>
        /// This function will return all the records from TModel
        /// Uses GET
        /// </summary>
        /// <returns>Returns lists of documents</returns>
        Task<IEnumerable<TModel>> GetAll();

        /// <summary>
        /// This function will return a document based on id(ModelId or ObjectId in MongoDB)
        /// Uses GET
        /// </summary>
        /// <param name="id"> ModelId</param>
        /// <returns>A document from MongoDB</returns>
        Task<TModel> GetById(string id);

        /// <summary>
        /// This function will post a document to the database
        /// Uses POST
        /// </summary>
        /// <param name="model">This can be any of Person, Batch or Apartment</param>
        Task Create(TModel model);

        /// <summary>
        /// This function will update the database document based on ModelId
        /// Uses PUT
        /// </summary>
        /// <param name="id">ModelId</param>
        /// <param name="model">This can be any of Person, Batch or Apartment</param>
        /// <returns>True if id was found, false otherwise</returns>
        Task<bool> UpdateById(string id, TModel model);

        /// <summary>
        /// This function will delete document based on ModelId
        /// Uses DELETE
        /// </summary>
        /// <param name="id">ModelId</param>
        /// <returns>True if id was found, false otherwise</returns>
        Task<bool> DeleteById(string id);
    }
}
