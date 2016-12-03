using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SenecaFleaServer.Models;

namespace SenecaFleaServer.Controllers
{
    //[Authorize(Roles = "SenecaFleaAdministrator")]
    public class AdminItemController : Controller
    {
        private AdminManager m = new AdminManager();


        // GET: AdminItem
        /// <summary>
        /// View Item List for administration
        /// </summary>
        /// <returns></returns>
        //[Authorize(Roles = "SenecaFleaAdministrator")]
        public ActionResult Index()
        {
            // Fetch the collection
            IEnumerable<ItemBase> cc = m.ItemGet();
            // Pass the collection to the view
            return View(cc);
        }

        // GET: AdminItem/Details/5
        /// <summary>
        /// View an item for administration
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Details(int? id)
        {
            // Attempt to get the matching object
            var o = m.ItemGetByIdWithMedia(id.GetValueOrDefault());

            if (o == null)
            {
                return HttpNotFound();
            }
            else
            {
                // Pass the object to the view
                return View(o);
            }            
        }


        // GET: AdminItem/Delete/5
        /// <summary>
        /// Request to delete an item for administration
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Delete(int? id)
        {
            var itemToDelete = m.ItemGetByIdWithMedia(id.GetValueOrDefault());

            if (itemToDelete == null)
            {                
                // Simply redirect
                return RedirectToAction("index");
            }
            else
            {
                return View(itemToDelete);
            }
        }

        // POST: AdminItem/Delete/5
        /// <summary>
        /// Delete an item for administration
        /// </summary>
        /// <param name="id"></param>
        /// <param name="collection"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete(int? id, FormCollection collection)
        {
            m.ItemDelete(id.GetValueOrDefault());

            return RedirectToAction("index");
        }

        #region Unsupport functions

        //// GET: AdminItem/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: AdminItem/Create
        //[HttpPost]
        //public ActionResult Create(FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add insert logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //// GET: AdminItem/Edit/5
        //public ActionResult Edit(int id)
        //{
        //    return View();
        //}

        //// POST: AdminItem/Edit/5
        //[HttpPost]
        //public ActionResult Edit(int id, FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add update logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        #endregion Unsupport functions
    }
}
