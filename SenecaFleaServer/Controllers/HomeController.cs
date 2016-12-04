using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using SenecaFleaServer.Models;

namespace SenecaFleaServer.Controllers
{
    public class HomeController : Controller
    {
        private AdminManager m;

        public HomeController()
        {
            m = new AdminManager();
        }

        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }

        //public ActionResult Login()
        //{
        //    return View();
        //}

        //// POST: Test/Create
        //[HttpPost]
        //public async Task<ActionResult> Login(LoginItems collection)
        //{
        //    try
        //    {
        //        // TODO: Add insert logic here
        //        bool re = await m.GetAccessToken(collection);
        //        if (re)
        //        {
        //            return RedirectToAction("Index");
        //        }
        //        else
        //        {
        //            return View();
        //        }
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}
    }
}
