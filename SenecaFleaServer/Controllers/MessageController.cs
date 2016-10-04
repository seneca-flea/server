using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SenecaFleaServer.Models;

namespace SenecaFleaServer.Controllers
{
    public class MessageController : ApiController
    {
        private MessageManager m = new MessageManager();

        // GET: api/Message
        public IHttpActionResult Get()
        {
            return Ok(m.MessageGetAll());
        }

        // GET: api/Message/5
        public IHttpActionResult Get(int id)
        {
            var obj = m.MessageGetById(id);
            if (obj == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(obj);
            }
        }

        // POST: api/Message
        public IHttpActionResult Post([FromBody]MessageAdd newItem)
        {
            if(Request.GetRouteData().Values["id"] != null ) { return BadRequest("Invalid request URI"); }

            if(newItem == null) { return BadRequest("Must send an entity body with the objec"); }

            if(!ModelState.IsValid) { return BadRequest(ModelState); }

            var addedItem = m.MessageAdd(newItem);

            if(addedItem == null) { return BadRequest("Cannot add the object"); }

            var uri = Url.Link("DefaultApi", new { id = addedItem.id});
            
            return Created(uri, addedItem);
        }

        //// PUT: api/Message/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        // DELETE: api/Message/5
        public void Delete(int id)
        {
            m.MessageDelete(id);
        }
    }
}
