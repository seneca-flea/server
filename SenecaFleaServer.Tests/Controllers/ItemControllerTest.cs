using Microsoft.VisualStudio.TestTools.UnitTesting;
using SenecaFleaServer.Controllers;
using SenecaFleaServer.Models;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.Http.Hosting;
using System.Web.Http.Routing;
using System.Web.Http.Controllers;
using System.Collections.Generic;
using AutoMapper;
using System.Linq;
#pragma warning disable CS0618

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
            Mapper.CreateMap<Item, ItemAdd>();
            context = new TestAppContext();
            controller = new ItemController(context);
        }

        [TestMethod]
        public void ItemGetById()
        {
            // Arrange
            Item itemData = GetItemData();
            SetUpItemData(context);
            SetupController(controller, HttpMethod.Get);

            // Act
            IHttpActionResult result = controller.Get(itemData.ItemId);

            // Assert
            var negResult = result as OkNegotiatedContentResult<ItemBase>;
            Assert.AreEqual(itemData.ItemId, negResult.Content.ItemId);
            Assert.AreEqual(itemData.Title, negResult.Content.Title);
        }

        [TestMethod]
        public void ItemAdd()
        {
            // Arrange
            var itemData = Mapper.Map<ItemAdd>(GetItemData());
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

            var itemData = new ItemEdit {
                ItemId = 1,
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
            int id = GetItemData().ItemId;

            Item result = context.Items.Find(id);
            Assert.IsNotNull(result);

            // Act
            controller.Delete(id);

            // Assert
            result = context.Items.Find(id);
            Assert.IsNull(result);
        }

        [TestMethod]
        public void ItemFilterByCategory()
        {
            // Arrange
            SetupItemDataArray(context);
            SetupController(controller, HttpMethod.Get);
            var item = Mapper.Map<ItemBase>(context.Items.Find(5));

            // Act
            IHttpActionResult result = controller.FilterByCourseName("Programming with C++");

            // Assert
            var negResult = result as OkNegotiatedContentResult<IEnumerable<ItemBase>>;
            Assert.AreEqual(item.ItemId, negResult.Content.FirstOrDefault().ItemId);
        }

        [TestMethod]
        public void ItemFilterByCourseName()
        {
            // Arrange
            SetupItemDataArray(context);
            SetupController(controller, HttpMethod.Get);
            var item = Mapper.Map<ItemBase>(context.Items.Find(5));

            // Act
            IHttpActionResult result = controller.FilterByCategory("Selling");

            // Assert
            var negResult = result as OkNegotiatedContentResult<IEnumerable<ItemBase>>;
            Assert.AreEqual(item.ItemId, negResult.Content.FirstOrDefault().ItemId);
        }

        [TestMethod]
        public void ItemFilterByCourseCode()
        {
            // Arrange
            SetupItemDataArray(context);
            SetupController(controller, HttpMethod.Get);
            var item = Mapper.Map<ItemBase>(context.Items.Find(5));

            // Act
            IHttpActionResult result = controller.FilterByCourseCode("OOP");

            // Assert
            var negResult = result as OkNegotiatedContentResult<IEnumerable<ItemBase>>;
            Assert.AreEqual(item.ItemId, negResult.Content.FirstOrDefault().ItemId);
        }


        // ##################################################################
        // Retrieve sample data
        public Item GetItemData()
        {
            var itemData = new Item {
                ItemId = 5,
                Title = "The C++ Programming Language (4th Edition)",
                Price = (decimal)39.99,
                Description = "Programming in C++",
                Status = "Selling"
            };

            return itemData;
        }

        public void SetupItemDataArray(TestAppContext context)
        {
            var coursedata = new Course {
                CourseId = 2,
                Name = "Programming with C++",
                Code = "OOP"
            };

            context.Courses.Add(coursedata);

            var itemData = GetItemData();
            itemData.Courses.Add(coursedata);
            context.Items.Add(itemData);
        }

        // Add sample data to context
        public void SetUpItemData(TestAppContext context)
        {
            Item itemData = GetItemData();
            context.Items.Add(itemData);
        }

        private static void SetupController(ApiController controller, HttpMethod httpMethod)
        {
            AutoMapperConfig.RegisterMappings();
            var config = new HttpConfiguration();
            var request = new HttpRequestMessage(HttpMethod.Post, "http://localhost/api/item");
            var route = config.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{id}");
            var routeData = new HttpRouteData(route, 
                new HttpRouteValueDictionary { { "controller", "item" } });

            controller.ControllerContext = new HttpControllerContext(config, routeData, request);
            controller.Request = request;
            controller.Request.Properties[HttpPropertyKeys.HttpConfigurationKey] = config;
            controller.Request.Properties[HttpPropertyKeys.HttpRouteDataKey] = routeData;
            controller.Url = new UrlHelper(request);
        }
    }
}
