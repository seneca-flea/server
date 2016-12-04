using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SenecaFleaServer.Models;

namespace SenecaFleaServer.Controllers
{
    [Authorize(Roles = "SenecaFleaAdministrator")]
    public class AdminItemController : ApiController
    {
        private AdminManager m;

        public AdminItemController()
        {
            m = new AdminManager();
        }

        // GET: api/AdminItem
        [Authorize(Roles = "SenecaFleaAdministrator")]
        public IHttpActionResult Get()
        {
            IEnumerable<ItemBase> cc = m.ItemGet();
            return Ok(cc);
        }

        // GET: api/AdminItem/5
        [Authorize(Roles = "SenecaFleaAdministrator")]
        public IHttpActionResult Get(int? id)
        {
            var o = m.ItemGetByIdWithMedia(id.GetValueOrDefault());

            if (o == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(o);
            }
        }

        // DELETE: api/AdminItem/5
        [Authorize(Roles = "SenecaFleaAdministrator")]
        public IHttpActionResult Delete(int id)
        {
            m.ItemDelete(id);

            return Ok("Deleted");
        }

        //// POST: api/AdminItem
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT: api/AdminItem/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

    }
}
