using Microsoft.VisualStudio.TestTools.UnitTesting;
using SenecaFleaServer.Controllers;
using SenecaFleaServer.Models;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.Http.Hosting;
using System.Web.Http.Routing;
using System.Web.Http.Controllers;

namespace SenecaFleaServer.Tests.Controllers
{
    [TestClass]
    public class ItemControllerTest
    {
        [TestMethod]
        public void ItemGetById()
        {
            // Arrange
            ItemAdd itemData = GetItemData();
            var context = new TestAppContext();
            SetUpItemData(context);
            ItemController controller = new ItemController(context);
            SetupController(controller, HttpMethod.Get);

            // Act
            IHttpActionResult result = controller.Get(5);

            // Assert
            var negResult = result as OkNegotiatedContentResult<ItemBase>;
            Assert.AreEqual(5, negResult.Content.ItemId);
            Assert.AreEqual(itemData.Title, negResult.Content.Title);
        }

        [TestMethod]
        public void ItemAdd()
        {
            // Arrange
            ItemController controller = new ItemController(new TestAppContext());
            SetupController(controller, HttpMethod.Post);
            ItemAdd itemData = GetItemData();
            
            // Act
            IHttpActionResult result = controller.Post(itemData);

            // Assert
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

        public void SetUpItemData(TestAppContext context)
        {
            ItemAdd itemData = GetItemData();
            context.Items.Add(new Item {
                ItemId = 5,
                Title = itemData.Title,
                Price = itemData.Price,
                Description = itemData.Description
            });
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
