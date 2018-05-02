using Person.Library.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Person.Library.Abstracts
{
    /// <summary>
    /// This is a common controller used by PersonController, ApartmentController, or BatchController
    /// </summary>
    /// <typeparam name="TModel">Can be any of Person, Apartment, or Batch</typeparam>
    [Produces("application/json")]
    [Route("api")]
    public class AModelController<TModel> : Controller where TModel : IModel
    {
        protected IRepository<TModel> Repo { get; set; }

        public AModelController(IRepository<TModel> repo)
        {
            Repo = repo;
        }

        /// <summary>
        /// This is a GET method that returns all the documents from MongoDB.
        /// </summary>
        /// <returns>A list of Documents</returns>
        [HttpGet]
        public async Task<IEnumerable<TModel>> Get()
        {
            return await Repo.GetAll();
        }

        /// <summary>
        /// This will return Statuscodes.
        /// OK = 200 and NotFound =404
        /// </summary>
        /// <param name="id"></param>
        /// <returns>OK = 200 and NotFound =404</returns>
        [HttpGet("{id}", Name = "Get")]
        public async Task<IActionResult> Get(string id)
        {
            try
            {
                return  Ok( await Repo.GetById(id));
            }
            catch (ArgumentException)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// This will perform POST operation.
        /// </summary>
        /// <param name="model"></param>
        /// <returns>BadRequest = 400, and 201 if model was created. </returns>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]TModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                if (model == null)
                {
                    return BadRequest();
                }

                model.ModelId = null;

                await Repo.Create(model);

                if (model.ModelId == null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }
               
                return CreatedAtRoute("Get", new { id = model.ModelId }, model);
            }
        }

        /// <summary>
        /// This will perform PUT operation.
        /// </summary>
        /// <param name="id">ModelId</param>
        /// <param name="model">Model</param>
        /// <returns>BadRequest = 400, if model is not valid, and NotFound = 404 if it cannot update, and 204 if successful.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody]TModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                model.ModelId = id;

                bool success;
                try
                {
                    success = await Repo.UpdateById(id, model);
                }
                catch (ArgumentException)
                {
                    success = false;
                }
                //return 404 if cannot update, return 204 if successfull
                if (!success)
                {
                    return NotFound();
                }
                else
                {
                    return NoContent();
                }
            }
        }

        /// <summary>
        /// This will perform DELETE operation.
        /// </summary>
        /// <param name="id">ModelId</param>
        /// <returns>404 if unsuccessful, or 204 if deleted successfully.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            bool success;
            try
            {
                success = await Repo.DeleteById(id);
            }
            catch (ArgumentException)
            {
                success = false;
            }
            if (!success)
            {
                return NotFound();//404
            }
            else
            {
                return NoContent();//204
            }
        }
    }
}
