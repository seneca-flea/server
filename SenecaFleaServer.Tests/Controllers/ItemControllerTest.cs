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
        TestAppContext context;
        ItemController controller;

        [TestInitialize]
        public void SetUp()
        {
            context = new TestAppContext();
            controller = new ItemController(context);
        }

        [TestMethod]
        public void ItemGetById()
        {
            // Arrange
            ItemAdd itemData = GetItemData();
            SetUpItemData(context);
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
            ItemAdd itemData = GetItemData();
            SetupController(controller, HttpMethod.Post);

            // Act
            IHttpActionResult result = controller.Post(itemData);

            // Assert
            var negResult = result as CreatedNegotiatedContentResult<ItemBase>;
            Assert.AreEqual(1, negResult.Content.ItemId);
            Assert.AreEqual(itemData.Title, negResult.Content.Title);
        }

        //[TestMethod]
        public void ItemEdit()
        {
            // Arrange
            SetUpItemData(context);
            SetupController(controller, HttpMethod.Put);

            var itemData = new ItemEdit
            {
                ItemId = 5,
                Title = "JavaScript: The Good Parts",
                Price = (decimal)39.99,
                Description = "Programming in Javscript"
            };

            // Act
            //IHttpActionResult result = controller.Put(itemData.ItemId, itemData);

            // Assert
            //var negResult = result as OkNegotiatedContentResult<ItemBase>;
        }

        [TestMethod]
        public void ItemDelete()
        {
            // Arrange
            SetUpItemData(context);
            SetupController(controller, HttpMethod.Delete);

            Item result = context.Items.Find(5);
            Assert.IsNotNull(result);

            // Act
            controller.Delete(5);

            // Assert
            result = context.Items.Find(5);
            Assert.IsNull(result);
        }

        // Retrieve sample data
        public ItemAdd GetItemData()
        {
            var itemData = new ItemAdd
            {
                Title = "The C++ Programming Language (4th Edition)",
                Price = (decimal)39.99,
                Description = "Programming in C++"
            };

            return itemData;
        }

        // Add sample data to context
        public void SetUpItemData(TestAppContext context)
        {
            ItemAdd itemData = GetItemData();
            context.Items.Add(new Item
            {
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
