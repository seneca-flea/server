using SenecaFleaServer.Models;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SenecaFleaServer.Controllers
{
    public class ItemController : ApiController
    {
        // TODO: Filter items by item title
        // TODO: Filter items by price range
        // TODO: Filter items by book information (title, author)
        // TODO: Add and update item with image and pickup details

        private ItemManager m;

        public ItemController()
        {
            m = new ItemManager();
        }
        
        public ItemController(DataContext repo)
        {
            m = new ItemManager(repo);
        }

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
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

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

        // PUT: api/Items/5
        public IHttpActionResult Put(int id, [FromBody]ItemEdit editedItem)
        {
            // Ensure that an "editedItem" is in the entity body
            if (editedItem == null)
            {
                return BadRequest("Must send an entity body with the request");
            }

            // Ensure that the id value in the URI matches the id value in the entity body
            if (id != editedItem.ItemId)
            {
                return BadRequest("Invalid data in the entity body");
            }

            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            var changedItem = m.ItemEdit(editedItem);

            if (changedItem == null)
            {
                // HTTP 400
                return BadRequest("Cannot edit the object");
            }
            else
            {
                // HTTP 200 with the changed item in the entity body
                return Ok<ItemBase>(changedItem);
            }
        }

        // DELETE: api/Item/5
        public void Delete(int id)
        {
            m.ItemDelete(id);
        }

        // GET: /api/Item/filter/title/{title}
        [HttpGet, Route("api/Item/filter/title/{title}")]
        public IHttpActionResult FilterByTitle(string title)
        {
            if (title == null) { return NotFound(); }

            var items = m.FilterByTitle(title);

            if (items == null) { return NotFound(); }

            return Ok(items);
        }

        // GET: /api/Item/filter/status/{status}
        [HttpGet, Route("api/Item/filter/status/{status}")]
        public IHttpActionResult FilterByStatus(string status)
        {
            if (status == null) { return NotFound(); }

            var items = m.FilterByStatus(status);

            if (items == null) { return NotFound(); }

            return Ok(items);
        }

        // GET: /api/Item/filter/coursename/{coursename}
        [HttpGet, Route("api/Item/filter/coursename/{coursename}")]
        public IHttpActionResult FilterByCourseName(string courseName)
        {
            if (courseName == null) { return NotFound(); }

            var items = m.FilterByCourseName(courseName);

            if (items == null) { return NotFound(); }

            return Ok(items);
        }

        // GET: /api/Item/filter/coursecode/{coursecode}
        [HttpGet, Route("api/Item/filter/coursecode/{coursecode}")]
        public IHttpActionResult FilterByCourseCode(string courseCode)
        {
            if (courseCode == null) { return NotFound(); }

            var items = m.FilterByCourseCode(courseCode);

            if (items == null) { return NotFound(); }

            return Ok(items);
        }
    }
}