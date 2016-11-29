using AutoMapper;
using SenecaFleaServer.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;

namespace SenecaFleaServer.Controllers
{
    [Authorize]
    public class ItemController : ApiController
    {
        // TODO: Filter items by book information (title, author)
        // TODO: Add and update item with pickup details

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
        [Authorize(Roles = "User, SenecaFleaAdministrator")]
        [ResponseType(typeof(ItemBase))]
        public IHttpActionResult Get(int? id)
        {
            if (!id.HasValue) { return NotFound(); }

            // Attempt to get item
            var obj = m.ItemGetByIdWithMedia(id.Value);

            if (obj == null) { return NotFound(); }

            var imageHeader = Request.Headers.Accept
                    .SingleOrDefault(a => a.MediaType.ToLower().StartsWith("image/"));

            if (imageHeader == null)
            {
                // Return item info
                return Ok(Mapper.Map<ItemBase>(obj));
            }
            else
            {
                if (obj.Images.Count() > 0)
                {
                    // Return images
                    return Ok(obj.Images);
                }
                else
                {
                    return NotFound();
                }
            }
        }

        // POST: api/Item
        /// <summary>
        /// Add an item
        /// </summary>
        /// <param name="newItem"></param>
        [Authorize(Roles = "User")]
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

        // PUT: api/Item/5
        /// <summary>
        /// Edit an item
        /// </summary>
        /// <param name="id">Item Id</param>
        /// <param name="editedItem"></param>
        [Authorize(Roles = "User")]
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

        // POST: api/Item/5/AddImage
        /// <summary>
        /// Add an image to an item
        /// </summary>
        /// <param name="id">Item Id</param>
        /// <param name="photo">Image data</param>
        [Authorize(Roles = "User")]
        [Route("api/Item/{id}/addimage")]
        [HttpPost]
        public IHttpActionResult AddImage(int id, [FromBody]byte[] photo)
        {
            var contentType = Request.Content.Headers.ContentType.MediaType;

            // Attempt to save
            if (m.ItemAddPhoto(id, contentType, photo))
            {
                return StatusCode(HttpStatusCode.NoContent);
            }
            else
            {
                return BadRequest("Unable to set the photo");
            }
        }

        // DELETE: api/Item/5
        /// <summary>
        /// Delete an item 
        /// </summary>
        /// <param name="id">Item Id</param>
        [Authorize(Roles = "User, SenecaFleaAdministrator")]
        public void Delete(int id)
        {
            m.ItemDelete(id);
        }

        // GET: /api/Item/filter/user
        /// <summary>
        /// Filter items by user
        /// </summary>
        /// <param name="userid">User Id</param>
        [Authorize(Roles = "User")]
        [HttpGet, Route("api/Item/filter/")]
        [ResponseType(typeof(IEnumerable<ItemBase>))]
        public IHttpActionResult FilterByUser(int? userid)
        {
            if (userid == null) { return BadRequest("Empty request"); }

            var items = m.FilterByUser(userid.Value);

            if (items == null) { return NotFound(); }

            return Ok(items);
        }

        // GET: /api/Item/filter/title
        /// <summary>
        /// Filter items by title
        /// </summary>
        /// <param name="title">Title</param>
        [Authorize(Roles = "User")]
        [HttpGet, Route("api/Item/filter")]
        [ResponseType(typeof(IEnumerable<ItemBase>))]
        public IHttpActionResult FilterByTitle(string title)
        {
            if (title == null) { return BadRequest("Empty request"); }

            var items = m.FilterByTitle(title);

            if (items == null) { return NotFound(); }

            return Ok(items);
        }

        // GET: /api/Item/filter/status
        /// <summary>
        /// Filter items by status
        /// </summary>
        /// <param name="status">Status</param>
        [Authorize(Roles = "User")]
        [HttpGet, Route("api/Item/filter/")]
        [ResponseType(typeof(IEnumerable<ItemBase>))]
        public IHttpActionResult FilterByStatus(string status)
        {
            if (status == null) { return BadRequest("Empty request"); }

            var items = m.FilterByStatus(status);

            if (items == null) { return NotFound(); }

            return Ok(items);
        }

        // GET: /api/Item/filter/coursename
        /// <summary>
        /// Filter items by course name
        /// </summary>
        /// <param name="coursename">Course Name</param>
        [Authorize(Roles = "User")]
        [HttpGet, Route("api/Item/filter")]
        [ResponseType(typeof(IEnumerable<ItemBase>))]
        public IHttpActionResult FilterByCourseName(string coursename)
        {
            if (coursename == null) { return BadRequest("Empty request"); }

            var items = m.FilterByCourseName(coursename);

            if (items == null) { return NotFound(); }

            return Ok(items);
        }

        // GET: /api/Item/filter/coursecode
        /// <summary>
        /// Filter items by course code
        /// </summary>
        /// <param name="coursecode">Course Code</param>
        [Authorize(Roles = "User")]
        [HttpGet, Route("api/Item/filter/")]
        [ResponseType(typeof(IEnumerable<ItemBase>))]
        public IHttpActionResult FilterByCourseCode(string coursecode)
        {
            if (coursecode == null) { return BadRequest("Empty request"); }

            var items = m.FilterByCourseCode(coursecode);

            if (items == null) { return NotFound(); }

            return Ok(items);
        }

        // GET: /api/Item/filter/price
        /// <summary>
        /// Filter items by price range
        /// </summary>
        /// <param name="min">Minimum price</param>
        /// <param name="max">Maximum price</param>
        [Authorize(Roles = "User")]
        [HttpGet, Route("api/Item/filter/price/")]
        [ResponseType(typeof(IEnumerable<ItemBase>))]
        public IHttpActionResult FilterByPriceRange(int? min, int? max)
        {
            if (min == null || max == null) { return BadRequest("Missing parameters"); }
            if (min.Value > max.Value) { return BadRequest("Invalid parameters"); }

            var items = m.FilterByPriceRange(min.Value, max.Value);

            if (items == null) { return NotFound(); }

            return Ok(items);
        }
    }
}