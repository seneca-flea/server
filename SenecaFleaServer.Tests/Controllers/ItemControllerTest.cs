using Microsoft.VisualStudio.TestTools.UnitTesting;
using SenecaFleaServer.Controllers;
using SenecaFleaServer.Models;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.Http.Hosting;
using System.Web.Http.Routing;
using System.Web.Http.Controllers;
using AutoMapper;

namespace SenecaFleaServer.Tests.Controllers
{
    [TestClass]
    public class ItemControllerTest
    {
        [TestMethod]
        public void ItemGetById()
        {
            ItemAdd itemData = GetItemData();
            var context = new TestAppContext();
            context.Items.Add(new Item {
                ItemId = 5,
                Title = "C++ Programming",
                Price = (decimal)12.99,
                Description = "Programming in C++"
            });
            ItemController controller = new ItemController(context);
            SetupController(controller, HttpMethod.Get);

            IHttpActionResult result = controller.Get(5);

            var negResult = result as OkNegotiatedContentResult<ItemBase>;
            Assert.AreEqual(5, negResult.Content.ItemId);
            Assert.AreEqual(itemData.Title, negResult.Content.Title);
        }

        [TestMethod]
        public void ItemAdd()
        {
            ItemController controller = new ItemController(new TestAppContext());
            SetupController(controller, HttpMethod.Post);
            ItemAdd itemData = GetItemData();

            IHttpActionResult result = controller.Post(itemData);

            var negResult = result as CreatedNegotiatedContentResult<ItemBase>;
            Assert.AreEqual(1, negResult.Content.ItemId);
            Assert.AreEqual(itemData.Title, negResult.Content.Title);
        }

        public ItemAdd GetItemData()
        {
            var itemData = new ItemAdd();

            itemData = new ItemAdd {
                Title = "C++ Programming",
                Price = (decimal)12.99,
                Description = "Programming in C++"
            };

            return itemData;
        }

        private static void SetupController(ApiController controller, HttpMethod httpMethod)
        {
            AutoMapperConfig.RegisterMappings();
            var config = new HttpConfiguration();
            var request = new HttpRequestMessage(HttpMethod.Post, "http://localhost/api/item");
            var route = config.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{id}");
            var routeData = new HttpRouteData(route, new HttpRouteValueDictionary { { "controller", "item" } });

            controller.ControllerContext = new HttpControllerContext(config, routeData, request);
            controller.Request = request;
            controller.Request.Properties[HttpPropertyKeys.HttpConfigurationKey] = config;
            controller.Request.Properties[HttpPropertyKeys.HttpRouteDataKey] = routeData;
            controller.Url = new UrlHelper(request);
        }
    }
}
