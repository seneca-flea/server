using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SenecaFleaServer.Models;
using System.Web.Http.Description;

namespace SenecaFleaServer.Controllers
{
    public class MessageController : ApiController
    {
        private MessageManager m;

        public MessageController()
        {
            m = new MessageManager();
        }

        public MessageController(DataContext repo)
        {
            m = new MessageManager(repo);
        }

        // GET: api/Message
        /// <summary>
        /// Retrieve all messages
        /// </summary>
        [ResponseType(typeof(IEnumerable<MessageBase>))]
        public IHttpActionResult Get()
        {
            return Ok(m.MessageGetAll());
        }

        // GET: api/Message/5
        /// <summary>
        /// Retrieve a message
        /// </summary>
        /// <param name="id">Message Id</param>
        [ResponseType(typeof(MessageBase))]
        public IHttpActionResult Get(int? id)
        {
            if (!id.HasValue) { return NotFound(); }

            var obj = m.MessageGetById(id.Value);

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
        /// <summary>
        /// Add a message
        /// </summary>
        /// <param name="newItem"></param>
        [ResponseType(typeof(MessageBase))]
        public IHttpActionResult Post([FromBody]MessageAdd newItem)
        {
            if(newItem == null) { return BadRequest("Must send an entity body with the objec"); }

            if(!ModelState.IsValid) { return BadRequest(ModelState); }

            var addedItem = m.MessageAdd(newItem);

            if(addedItem == null) { return BadRequest("Cannot add the object"); }

            var uri = Url.Link("DefaultApi", new { id = addedItem.MessageId});

            return Created(uri, addedItem);
        }

        // DELETE: api/Message/5
        /// <summary>
        /// Delete a message
        /// </summary>
        /// <param name="id">Message Id</param>
        public void Delete(int id)
        {
            m.MessageDelete(id);
        }
    }
}
