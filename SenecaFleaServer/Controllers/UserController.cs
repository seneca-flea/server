﻿using SenecaFleaServer.Models;
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
        public IHttpActionResult Get()
        {
            return Ok(m.UserGetAll());
        }

        // GET: api/User/5
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
        public IHttpActionResult Post([FromBody]UserAdd newItem)
        {
            // Ensure that the URI is clean (and does not have an id parameter)
            if (Request.GetRouteData().Values["id"] != null) { return BadRequest("Invalid request URI"); }

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
            var uri = Url.Link("DefaultApi", new { id=addedItem.Id});

            return Created(uri, addedItem);
        }

        // PUT: api/User/5
        public IHttpActionResult Put(int? id, [FromBody]UserEdit editedItem)
        {
            // Ensure that an "editedItem" is in the entity body
            if (editedItem == null)
            {
                return BadRequest("Must send an entity body with the request");
            }

            // Ensure that the id value in the URI matches the id value in the entity body
            if (id.GetValueOrDefault() != editedItem.Id)
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
        [Route("api/User/{id}/SetLocation")]
        public IHttpActionResult Put(int? id, [FromBody]UserEditLocation editedItem)
        {
            // Ensure that an "editedItem" is in the entity body
            if (editedItem == null)
            {
                return BadRequest("Must send an entity body with the request");
            }

            // Ensure that the id value in the URI matches the id value in the entity body
            if (id.GetValueOrDefault() != editedItem.Id)
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

        // DELETE: api/User/5
        public void Delete(int id)
        {
            // In a controller 'Delete' method, a void return type will
            // automatically generate a HTTP 204 "No content" response
            m.UserDelete(id);
        }
    }
}
