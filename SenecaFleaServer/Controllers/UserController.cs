using SenecaFleaServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SenecaFleaServer.Controllers
{
    public class UserController : ApiController
    {
        // TODO: View user's bought or sold items

        private UserManager m;

        public UserController()
        {
            m = new UserManager();
        }

        public UserController(DataContext repo)
        {
            m = new UserManager(repo);
        }

        // GET: api/User
        /// <summary>
        /// Retrieve all users
        /// </summary>
        /// <returns></returns>
        public IHttpActionResult Get()
        {
            return Ok(m.UserGetAll());
        }

        // GET: api/User/5
        /// <summary>
        /// Retrieve a user
        /// </summary>
        /// <param name="id">User Id</param>
        /// <returns></returns>
        public IHttpActionResult Get(int? id)
        {
            var o = m.UserGetById(id.GetValueOrDefault());

            if(o == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(o);
            }            
        }

        // POST: api/User
        /// <summary>
        /// Add a user
        /// </summary>
        /// <param name="newItem"></param>
        /// <returns></returns>
        public IHttpActionResult Post([FromBody]UserAdd newItem)
        {
            // Ensure that the URI is clean (and does not have an id parameter)
            //if (Request.GetRouteData().Values["id"] != null) { return BadRequest("Invalid request URI"); }

            // Ensure that a "newItem" is in the entity body
            if (newItem == null) { return BadRequest("Must send an entity body with the request"); }

            // Ensure that we can use the incoming data
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            // Attempt to add the new object
            var addedItem = m.UserAdd(newItem);

            // Continue?
            if (addedItem == null) { return BadRequest("Cannot add the object"); }

            // HTTP 201 with the new object in the entity body
            // Notice how to create the URI for the Location header
            var uri = Url.Link("DefaultApi", new { id=addedItem.UserId});

            return Created(uri, addedItem);
        }

        // PUT: api/User/5
        /// <summary>
        /// Edit a user's info
        /// </summary>
        /// <param name="id">User Id</param>
        /// <param name="editedItem"></param>
        /// <returns></returns>
        public IHttpActionResult Put(int? id, [FromBody]UserEdit editedItem)
        {
            // Ensure that an "editedItem" is in the entity body
            if (editedItem == null)
            {
                return BadRequest("Must send an entity body with the request");
            }

            // Ensure that the id value in the URI matches the id value in the entity body
            if (id.GetValueOrDefault() != editedItem.UserId)
            {
                return BadRequest("Invalid data in the entity body");
            }

            // Ensure that we can use the incoming data
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                // Attempt to update the item
                var changedItem = m.UserEdit(editedItem);

                // Notice the ApiController convenience methods
                if (changedItem == null)
                {
                    // HTTP 400
                    return BadRequest("Cannot edit the object");
                }
                else
                {
                    // HTTP 200 with the changed item in the entity body
                    return Ok(changedItem);
                }
            }
        }

        // PUT: api/User/5
        /// <summary>
        /// Edit a user's location
        /// </summary>
        /// <param name="id">User Id</param>
        /// <param name="editedItem"></param>
        /// <returns></returns>
        [Route("api/User/{id}/SetLocation")]
        public IHttpActionResult Put(int? id, [FromBody]UserEditLocation editedItem)
        {
            // Ensure that an "editedItem" is in the entity body
            if (editedItem == null)
            {
                return BadRequest("Must send an entity body with the request");
            }

            // Ensure that the id value in the URI matches the id value in the entity body
            if (id.GetValueOrDefault() != editedItem.UserId)
            {
                return BadRequest("Invalid data in the entity body");
            }

            // Ensure that we can use the incoming data
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                // Attempt to update the item
                var changedItem = m.UserEditLocation(editedItem);

                // Notice the ApiController convenience methods
                if (changedItem == null)
                {
                    // HTTP 400
                    return BadRequest("Cannot edit the object");
                }
                else
                {
                    // HTTP 200 with the changed item in the entity body
                    return Ok(changedItem);
                }
            }
        }

        // PUT: api/User/5/AddFavorite
        /// <summary>
        /// Add a favourite
        /// </summary>
        /// <param name="id">User Id</param>
        /// <param name="favorite"></param>
        /// <returns></returns>
        [HttpPut, Route("api/User/{id}/AddFavorite")]
        public IHttpActionResult AddFavorite(int? id, [FromBody]UserFavorite favorite)
        {
            // Ensure that an "editedItem" is in the entity body
            if (favorite == null)
            {
                return BadRequest("Must send an entity body with the request");
            }

            // Ensure that the id value in the URI matches the id value in the entity body
            if (id.GetValueOrDefault() != favorite.UserId)
            {
                return BadRequest("Invalid data in the entity body");
            }

            // Ensure that we can use the incoming data
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            // Attempt to add favorite
            var addedFavorite = m.UserAddFavorite(favorite);

            if (!addedFavorite)
            {
                return BadRequest("Cannot add item to favorites");
            }
            else
            {
                return Ok();
            }
        }

        // PUT: api/User/5/RemoveFavorite
        /// <summary>
        /// Remove a favourite
        /// </summary>
        /// <param name="id">User Id</param>
        /// <param name="favorite"></param>
        /// <returns></returns>
        [HttpPut, Route("api/User/{id}/RemoveFavorite")]
        public IHttpActionResult RemoveFavorite(int? id, [FromBody]UserFavorite favorite)
        {
            // Ensure that an "editedItem" is in the entity body
            if (favorite == null)
            {
                return BadRequest("Must send an entity body with the request");
            }

            // Ensure that the id value in the URI matches the id value in the entity body
            if (id.GetValueOrDefault() != favorite.UserId)
            {
                return BadRequest("Invalid data in the entity body");
            }

            // Ensure that we can use the incoming data
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            // Remove favorite
            var removedFavorite = m.UserRemoveFavorite(favorite);

            return Ok();
        }

        // DELETE: api/User/5
        /// <summary>
        /// Delete a user
        /// </summary>
        /// <param name="id">User Id</param>
        public void Delete(int id)
        {
            m.UserDelete(id);
        }
    }
}
