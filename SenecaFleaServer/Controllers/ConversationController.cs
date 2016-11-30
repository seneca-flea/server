using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using SenecaFleaServer.Models;

namespace SenecaFleaServer.Controllers
{
    [Authorize]
    public class ConversationController : ApiController
    {
        private MessageManager m;

        public ConversationController()
        {
            m = new MessageManager();
        }

        // GET: api/Conversation
        /// <summary>
        /// Retrieve all conversation by a current user
        /// </summary>
        /// <returns></returns>
        [ResponseType(typeof(IEnumerable<ConversationBase>))]
        public IHttpActionResult Get()
        {
            return Ok(m.ConversationGetAllByCurrentUser());
        }

        // GET: api/Conversation/5
        /// <summary>
        /// Retrieve a conversation by a receiverId
        /// </summary>
        /// <param name="id">Receiver Id</param>
        /// <returns></returns>
        public IHttpActionResult Get(int? id)
        {
            if (!id.HasValue) { return NotFound(); }

            var obj = m.ConversationGetByReceiver(id.Value);

            if (obj == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(obj);
            }
        }

        //// POST: api/Conversation
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT: api/Conversation/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        // DELETE: api/Conversation/5
        /// <summary>
        /// Delete a conversation including its messages by receiverId or senderId
        /// </summary>
        /// <param name="id">ReceiverId or SenderId</param>
        public void Delete(int id)
        {
            m.ConversationDeleteByReceiver(id);
        }
    }
}
