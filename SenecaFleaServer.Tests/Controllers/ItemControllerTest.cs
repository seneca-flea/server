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
using System.Net.Http.Headers;
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
            AutoMapperConfig.RegisterMappings();
            Mapper.CreateMap<Item, ItemAdd>();
            context = new TestAppContext();
            controller = new ItemController(context);
        }

        [TestMethod]
        public void ItemGetById()
        {
            // Arrange
            Item item = SetUpItemData();
            SetupController(controller, HttpMethod.Get);

            // Act
            IHttpActionResult result = controller.Get(item.ItemId);

            // Assert
            var negResult = result as OkNegotiatedContentResult<ItemBase>;
            Assert.AreEqual(item.ItemId, negResult.Content.ItemId);
            Assert.AreEqual(item.Title, negResult.Content.Title);
        }

        [TestMethod]
        public void ItemAdd()
        {
            // Arrange
            var item = Mapper.Map<ItemAdd>(GetItemData());
            item.BookTitle = "Book Title";
            item.BookYear = 2016;
            SetupController(controller, HttpMethod.Post);

            // Act
            IHttpActionResult result = controller.Post(item);

            // Assert
            var negResult = result as CreatedNegotiatedContentResult<ItemBase>;
            Assert.AreEqual(context.Items.Count(), 1);
            Assert.AreEqual(1, negResult.Content.ItemId);
            Assert.AreEqual(item.Title, negResult.Content.Title);
        }

        // TODO: Figure out how to unit test editing data
        // This is currently broken
        //[TestMethod]
        public void ItemEdit()
        {
            // Arrange
            SetUpItemData();
            SetupController(controller, HttpMethod.Put);

            var itemData = new ItemEdit {
                ItemId = 5,
                Title = "JavaScript: The Good Parts",
                Price = (decimal)39.99,
                Description = "Programming in Javscript"
            };

            // Act
            //IHttpActionResult result = controller.Put(itemData.ItemId, itemData);

            // Assert
            //var negResult = result as OkNegotiatedContentResult<ItemBase>;
            //Assert.AreEqual(itemData.Title, negResult.Content.Title);
        }

        [TestMethod]
        public void ItemDelete()
        {
            // Arrange
            int id = SetUpItemData().ItemId;
            SetupController(controller, HttpMethod.Delete);

            Item result = context.Items.Find(id);
            Assert.IsNotNull(result);

            // Act
            controller.Delete(id);

            // Assert
            result = context.Items.Find(id);
            Assert.IsNull(result);
        }

        [TestMethod]
        public void ItemFilterByUser()
        {
            // Arrange
            Item item = SetUpItemData();
            SetupController(controller, HttpMethod.Get);

            // Act
            IHttpActionResult result = controller.FilterByUser(item.SellerId);

            // Assert
            var negResult = result as OkNegotiatedContentResult<IEnumerable<ItemBase>>;
            Assert.AreEqual(item.ItemId, negResult.Content.FirstOrDefault().ItemId);
        }

        [TestMethod]
        public void ItemFilterByTitle()
        {
            // Arrange
            Item item = SetUpItemData();
            SetupController(controller, HttpMethod.Get);

            // Act
            IHttpActionResult result = controller.FilterByTitle("C++");

            // Assert
            var negResult = result as OkNegotiatedContentResult<IEnumerable<ItemBase>>;
            Assert.AreEqual(item.ItemId, negResult.Content.FirstOrDefault().ItemId);
        }

        [TestMethod]
        public void ItemFilterByStatus()
        {
            // Arrange
            Item item = SetUpItemData();
            SetupController(controller, HttpMethod.Get);

            // Act
            IHttpActionResult result = controller.FilterByStatus("Available");

            // Assert
            var negResult = result as OkNegotiatedContentResult<IEnumerable<ItemBase>>;
            Assert.AreEqual(item.ItemId, negResult.Content.FirstOrDefault().ItemId);
        }

        [TestMethod]
        public void ItemFilterByCourseName()
        {
            // Arrange
            Item item = SetUpItemData();
            SetupController(controller, HttpMethod.Get);

            // Act
            IHttpActionResult result = controller.FilterByCourseName("Programming with C++");

            // Assert
            var negResult = result as OkNegotiatedContentResult<IEnumerable<ItemBase>>;
            Assert.AreEqual(item.ItemId, negResult.Content.FirstOrDefault().ItemId);
        }

        [TestMethod]
        public void ItemFilterByCourseCode()
        {
            // Arrange
            Item item = SetUpItemData();
            SetupController(controller, HttpMethod.Get);

            // Act
            IHttpActionResult result = controller.FilterByCourseCode("OOP");

            // Assert
            var negResult = result as OkNegotiatedContentResult<IEnumerable<ItemBase>>;
            Assert.AreEqual(item.ItemId, negResult.Content.FirstOrDefault().ItemId);
        }

        [TestMethod]
        public void ItemFilterByPriceRange()
        {
            // Arrange
            Item item = SetUpItemData();
            SetupController(controller, HttpMethod.Get);

            // Act
            IHttpActionResult result = controller.FilterByPriceRange(30, 60);

            // Assert
            var negResult = result as OkNegotiatedContentResult<IEnumerable<ItemBase>>;
            Assert.AreEqual(item.ItemId, negResult.Content.FirstOrDefault().ItemId);
        }

        [TestMethod]
        public void ItemAddPhoto()
        {
            // Arrange
            Item item = SetUpItemData();
            var bytes = new byte[] { 0x20 };

            SetupController(controller, HttpMethod.Post);
            controller.Request.Content = new ByteArrayContent(bytes);
            controller.Request.Content.Headers.Add("Content-Type", "image/jpg");

            // Act
            controller.AddImage(item.ItemId, bytes);

            // Assert
            Assert.AreEqual(item.Images.Count, 2);
        }

        [TestMethod]
        public void ItemGetPhoto()
        {
            // Arrange
            Item item = SetUpItemData();
            
            SetupController(controller, HttpMethod.Get);
            controller.Request.Content = new ByteArrayContent(new byte[] { });
            controller.Request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("image/*"));

            // Act
            IHttpActionResult result = controller.Get(item.ItemId);

            // Assert
            var negResult = result as OkNegotiatedContentResult<IEnumerable<Image>>;
            Assert.AreEqual(negResult.Content.Count(), 1);
            Assert.AreEqual(negResult.Content.First().Photo, item.Images.First().Photo);
        }

        // ##################################################################
        // Retrieve sample data
        public Item GetItemData()
        {
            var user = new User { UserId = 1 };

            var course = new Course
            {
                CourseId = 2,
                Name = "Programming with C++",
                Code = "OOP"
            };

            var image = new Image
            {
                ImageId = 1,
                ContentType = "image/jpg",
                Photo = new byte[] { 0x20 }
            };

            var item = new Item
            {
                ItemId = 5,
                Title = "The C++ Programming Language (4th Edition)",
                Price = (decimal)39.99,
                Description = "Programming in C++",
                Status = "Available",
                Type = "Book",
                SellerId = user.UserId
            };

            context.Users.Add(user);
            context.Courses.Add(course);
            item.Courses.Add(course);
            item.Images.Add(image);

            return item;
        }

        // Add sample data to context
        public Item SetUpItemData()
        {
            Item item = GetItemData();
            context.Items.Add(item);

            return item;
        }

        private static void SetupController(ApiController controller, HttpMethod httpMethod)
        {
            var config = new HttpConfiguration();
            var request = new HttpRequestMessage(httpMethod, "http://localhost/api/item");
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
