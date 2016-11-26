﻿using SenecaFleaServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace SenecaFleaServer.Controllers
{
    //[Authorize]
    [AllowAnonymous]
    public class UserController : ApiController
    {
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
        [Authorize(Roles = "SenecaFleaAdministrator")]
        [ResponseType(typeof(IEnumerable<UserBase>))]
        public IHttpActionResult Get()
        {
            return Ok(m.UserGetAll());
        }

        // GET: api/User/5
        /// <summary>
        /// Retrieve a user
        /// </summary>
        /// <param name="id">User Id</param>
        [Authorize(Roles = "User, SenecaFleaAdministrator")]
        [ResponseType(typeof(UserBase))]
        public IHttpActionResult Get(int? id)
        {
            var o = m.UserGetById(id.GetValueOrDefault());

            if (o == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(o);
            }            
        }

        [Authorize(Roles = "User, SenecaFleaAdministrator")]
        [Route("api/User/{id}/Location")]
        [ResponseType(typeof(UserWithAllInfo))]
        public IHttpActionResult GetUserWithLocation(int? id)
        {
            var o = m.UserUserWithLocationById(id.GetValueOrDefault());

            if (o == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(o);
            }
        }

        [Authorize(Roles = "User, SenecaFleaAdministrator")]
        [Route("api/User/{id}/AllInfo")]
        [ResponseType(typeof(UserWithAllInfo))]
        public IHttpActionResult GetUserWithAllInfo(int? id)
        {
            var o = m.UserGetWithAllInfoById(id.GetValueOrDefault());

            if (o == null)
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
        [AllowAnonymous]
        [ResponseType(typeof(UserBase))]
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

        //// PUT: api/User/5
        ///// <summary>
        ///// Edit a user's info
        ///// </summary>
        ///// <param name="id">User Id</param>
        ///// <param name="editedItem"></param>
        //[ResponseType(typeof(UserBase))]
        //[Authorize(Roles = "SenecaFleaAdministrator")]
        //public IHttpActionResult Put(int? id, [FromBody]UserEdit editedItem)
        //{
        //    // Ensure that an "editedItem" is in the entity body
        //    if (editedItem == null)
        //    {
        //        return BadRequest("Must send an entity body with the request");
        //    }

        //    // Ensure that the id value in the URI matches the id value in the entity body
        //    if (id.GetValueOrDefault() != editedItem.UserId)
        //    {
        //        return BadRequest("Invalid data in the entity body");
        //    }

        //    // Ensure that we can use the incoming data
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }
        //    else
        //    {
        //        // Attempt to update the item
        //        var changedItem = m.UserEdit(editedItem);

        //        // Notice the ApiController convenience methods
        //        if (changedItem == null)
        //        {
        //            // HTTP 400
        //            return BadRequest("Cannot edit the object");
        //        }
        //        else
        //        {
        //            // HTTP 200 with the changed item in the entity body
        //            return Ok(changedItem);
        //        }
        //    }
        //}


        #region For Users

        // PUT: api/User/5
        /// <summary>
        /// Edit a user's location
        /// </summary>
        /// <param name="id">User Id</param>
        /// <param name="editedItem"></param>
        [Route("api/User/{id}/SetLocation")]
        [ResponseType(typeof(UserBase))]
        [Authorize(Roles = "User")]
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

        // GET: api/User/5/Favorites
        /// <summary>
        /// Get favourites
        /// </summary>
        /// <param name="id">User Id</param>
        [HttpGet, Route("api/User/{id}/Favorites")]
        [ResponseType(typeof(IEnumerable<ItemBase>))]
        [Authorize(Roles = "User")]
        public IHttpActionResult GetFavorite(int? id)
        {
            var items = m.UserGetFavorite(id.GetValueOrDefault());

            return Ok(items);
        }

        // PUT: api/User/5/AddFavorite
        /// <summary>
        /// Add a favourite
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <param name="itemId">Item Id</param>
        [HttpPut, Route("api/User/{userId}/AddFavorite/{itemId}")]
        [Authorize(Roles = "User")]
        public IHttpActionResult AddFavorite(int? userId, int? itemId)
        {
            // Ensure that an "editedItem" is in the entity body
            if (userId == null || itemId == null)
            {
                return BadRequest("Missing parameters");
            }

            // Add favorite
            var added = m.UserAddFavorite(userId.Value, itemId.Value);

            if (!added)
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
        /// <param name="userId">User Id</param>
        /// <param name="itemId">Item Id</param>
        [HttpPut, Route("api/User/{userId}/RemoveFavorite/{itemId}")]
        [Authorize(Roles = "User")]
        public IHttpActionResult RemoveFavorite(int? userId, int? itemId)
        {
            // Ensure that an "editedItem" is in the entity body
            if (userId == null || itemId == null)
            {
                return BadRequest("Missing parameters");
            }

            // Remove favorite
            var removed = m.UserRemoveFavorite(userId.Value, itemId.Value);

            if (!removed)
            {
                return BadRequest("Cannot remove item to favorites");
            }
            else
            {
                return Ok();
            }
        }

        // GET: api/User/5/History
        /// <summary>
        /// Get user's purchase history
        /// </summary>
        /// <param name="id">User Id</param>
        [HttpGet, Route("api/User/{id}/History")]
        [ResponseType(typeof(IEnumerable<PurchaseHistoryBase>))]
        [Authorize(Roles = "User")]
        public IHttpActionResult GetHistory(int? id)
        {
            var items = m.UserGetHistory(id.GetValueOrDefault());

            return Ok(items);
        }

        // POST: api/User/5/History
        /// <summary>
        /// Add to user's purchase history
        /// </summary>
        /// <param name="id">User Id</param>
        /// <param name="obj"></param>
        [HttpPost, Route("api/User/{id}/History")]
        [Authorize(Roles = "User")]
        public IHttpActionResult AddHistory(int? id, [FromBody]PurchaseHistoryAdd obj)
        {
            if (obj == null)
            {
                return BadRequest("Must send an entity body with the request");
            }

            var added = m.UserAddHistory(id.GetValueOrDefault(), obj);

            if (!added)
            {
                return BadRequest("Cannot add to history");
            }
            else
            {
                return Ok();
            }
        }

        // DELETE: api/User/5
        /// <summary>
        /// Delete a user
        /// </summary>
        /// <param name="id">User Id</param>
        [Authorize(Roles = "User")]
        public void Delete(int id)
        {
            m.UserDelete(id);
        }

        #endregion For Users
    }
}
