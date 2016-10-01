using SenecaFleaServer.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SenecaFleaServer.Controllers
{
    public class ItemController : ApiController
    {
        Manager m;
        public ItemController() { m = new Manager(); }
        public ItemController(DataContext repo) { m = new Manager(repo); }

        // GET: api/Item/5
        public IHttpActionResult Get(int? id)
        {
            if (!id.HasValue) { return NotFound(); }

            var obj = m.ItemGetById(id.Value);

            if (obj == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(obj);
            }
        }

        // POST: api/Item
        public IHttpActionResult Post([FromBody]ItemAdd newItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Attempt to add item
            var addedItem = m.ItemAdd(newItem);

            if (addedItem == null)
            {
                return BadRequest("Cannot add the object");
            }
            else
            {
                // HTTP 201 with the new object in the entity body
                var uri = Url.Link("DefaultApi", new { id = addedItem.ItemId });
                return Created<ItemBase>(uri, addedItem);
            }
        }
    }
}
