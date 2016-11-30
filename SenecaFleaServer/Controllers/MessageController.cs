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
    [Authorize]
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
        /// Retrieve all messages for administration
        /// </summary>
        //[Authorize(Roles = "User")]
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
        //[Authorize(Roles = "User")]
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
        /// <param name="newItem">MessageAdd</param>
        //[Authorize(Roles = "User")]
        [ResponseType(typeof(MessageBase))]
        public IHttpActionResult Post([FromBody]MessageAdd newItem)
        {
            if(newItem == null) { return BadRequest("Must send an entity body with the object"); }

            if(!ModelState.IsValid) { return BadRequest(ModelState); }

            var addedItem = m.MessageAdd(newItem);

            if(addedItem == null) { return BadRequest("Cannot add the object"); }

            var uri = Url.Link("DefaultApi", new { id = addedItem.MessageId});

            return Created(uri, addedItem);
        }

        // DELETE: api/Message/Delete/2/Receiver/3
        /// <summary>
        /// Delete messages that a user sends and receives by its identifier
        /// </summary>
        /// <param name="SenderId"></param>
        /// <param name="ReceiverId"></param>
        //[Authorize(Roles = "User")]
        [HttpDelete, Route("api/Message/Delete/Sender/{SenderId}/Receiver/{ReceiverId}")]
        public void DeleteByUser(int SenderId, int ReceiverId)
        {
            m.MessageDelete(SenderId, ReceiverId);
        }

        // DELETE: api/Message/5
        /// <summary>
        /// Delete a message
        /// </summary>
        /// <param name="id">Message Id</param>
        //[Authorize(Roles = "User")]
        public void Delete(int id)
        {
            m.MessageDeleteById(id);
        }

        // Get messages by identifiers
        /// <summary>
        /// Get all messages that a user sends and receives by its identifiers
        /// </summary>
        /// <param name="userId">A user identifier</param>
        /// <returns>A collection of MessageBase</returns>
        //[Authorize(Roles = "User")]
        [HttpGet, Route("api/Message/filter/User/{userId}")]
        [ResponseType(typeof(IEnumerable<MessageBase>))]
        public IHttpActionResult FilterByUserId(int userId)
        {
            //TODO: How would I retrieve the current user id?

            if (userId < 0) { return BadRequest("Must send a userId"); }

            var msgs = m.MessageFilterByUserId(userId);

            if(msgs == null) { return NotFound(); }

            return Ok(msgs);            
        }

        // Get messages by an identifier, filted by a receiver
        /// <summary>
        /// Get messages by an identifier, filted by a receiver
        /// </summary>
        /// <param name="filterObj">MessageFilterByUserIdWithReceiver</param>
        /// <returns>A collection of MessageBase</returns>
        //[Authorize(Roles = "User")]
        [HttpGet, Route("api/Message/filter/UserWithReceiver")]
        [ResponseType(typeof(IEnumerable<MessageBase>))]
        public IHttpActionResult FilterByUserIdWithReceiver([FromBody]MessageFilterByUserIdWithReceiver filterObj)
        {
            if (filterObj == null ) { return BadRequest("Must send an entity body"); }

            var msgs = m.MessageFilterByUserIdWithReceiver(filterObj);

            if (msgs == null) { return NotFound(); }

            return Ok(msgs);
        }

        // Get messages by an identifier, filtered by datetime
        /// <summary>
        /// Get messages by an identifier, filtered by datetime
        /// </summary>
        /// <param name="filterObj">MessageFilterByUserIdWithTime</param>
        /// <returns>A collection of MessageBase</returns>
        //[Authorize(Roles = "User")]
        [HttpGet, Route("api/Message/filter/UserWithDate")]
        [ResponseType(typeof(IEnumerable<MessageBase>))]
        public IHttpActionResult FilterByUserIdWithDate([FromBody]MessageFilterByUserIdWithTime filterObj)
        {
            if (filterObj == null) { return BadRequest("Must send an entity body"); }

            var msgs = m.MessageFilterByUserIdWithDate(filterObj);

            if (msgs == null) { return NotFound(); }

            return Ok(msgs);
        }

        // Get messages by an identifier, filtered by item
        /// <summary>
        /// Get messages by an identifier, filtered by item
        /// </summary>
        /// <param name="filterObj">MessageFilterByUserIdWithItem</param>
        /// <returns>A collection of MessageBase</returns>
        //[Authorize(Roles = "User")]
        [HttpGet, Route("api/Message/filter/UserWithItem")]
        [ResponseType(typeof(IEnumerable<MessageBase>))]
        public IHttpActionResult FilterByUserIdWithItem([FromBody] MessageFilterByUserIdWithItem filterObj)
        {
            if (filterObj == null) { return BadRequest("Must send an entity body"); }

            var msgs = m.MessageFilterByUserIdWithItem(filterObj);

            if (msgs == null) { return NotFound(); }

            return Ok(msgs);
        }
    }
}
