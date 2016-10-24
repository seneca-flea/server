using SenecaFleaServer.Models;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace SenecaFleaServer.Controllers
{
    public class ItemController : ApiController
    {
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
        /// <summary>
        /// Retrieve an item
        /// </summary>
        /// <param name="id">Item ID</param>
        [ResponseType(typeof(ItemBase))]
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
        /// <summary>
        /// Add an item
        /// </summary>
        /// <param name="newItem"></param>
        [ResponseType(typeof(ItemBase))]
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
                return Created(uri, addedItem);
            }
        }

        // PUT: api/Items/5
        /// <summary>
        /// Edit an item
        /// </summary>
        /// <param name="id">Item Id</param>
        /// <param name="editedItem"></param>
        [ResponseType(typeof(ItemBase))]
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
                return Ok(changedItem);
            }
        }

        // DELETE: api/Item/5
        /// <summary>
        /// Delete an item 
        /// </summary>
        /// <param name="id">Item Id</param>
        public void Delete(int id)
        {
            m.ItemDelete(id);
        }

        // GET: /api/Item/filter/title/{title}
        /// <summary>
        /// Filter items by title
        /// </summary>
        /// <param name="title">Title</param>
        [HttpGet, Route("api/Item/filter/title/{title}")]
        [ResponseType(typeof(IEnumerable<ItemBase>))]
        public IHttpActionResult FilterByTitle(string title)
        {
            if (title == null) { return NotFound(); }

            var items = m.FilterByTitle(title);

            if (items == null) { return NotFound(); }

            return Ok(items);
        }

        // GET: /api/Item/filter/status/{status}
        /// <summary>
        /// Filter items by status
        /// </summary>
        /// <param name="status">Status</param>
        [HttpGet, Route("api/Item/filter/status/{status}")]
        [ResponseType(typeof(IEnumerable<ItemBase>))]
        public IHttpActionResult FilterByStatus(string status)
        {
            if (status == null) { return NotFound(); }

            var items = m.FilterByStatus(status);

            if (items == null) { return NotFound(); }

            return Ok(items);
        }

        // GET: /api/Item/filter/coursename/{coursename}
        /// <summary>
        /// Filter items by course name
        /// </summary>
        /// <param name="courseName">Course Name</param>
        /// <returns></returns>
        [HttpGet, Route("api/Item/filter/coursename/{coursename}")]
        [ResponseType(typeof(IEnumerable<ItemBase>))]
        public IHttpActionResult FilterByCourseName(string courseName)
        {
            if (courseName == null) { return NotFound(); }

            var items = m.FilterByCourseName(courseName);

            if (items == null) { return NotFound(); }

            return Ok(items);
        }

        // GET: /api/Item/filter/coursecode/{coursecode}
        /// <summary>
        /// Filter items by course code
        /// </summary>
        /// <param name="courseCode">Course Code</param>
        /// <returns></returns>
        [HttpGet, Route("api/Item/filter/coursecode/{coursecode}")]
        [ResponseType(typeof(IEnumerable<ItemBase>))]
        public IHttpActionResult FilterByCourseCode(string courseCode)
        {
            if (courseCode == null) { return NotFound(); }

            var items = m.FilterByCourseCode(courseCode);

            if (items == null) { return NotFound(); }

            return Ok(items);
        }
    }
}