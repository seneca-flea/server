using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AutoMapper;
using SenecaFleaServer.Models;
using SenecaFleaServer.Controllers;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.Http.Hosting;
using System.Web.Http.Routing;
using System.Web.Http.Controllers;
using System.Collections.Generic;
#pragma warning disable CS0618

namespace SenecaFleaServer.Tests.Controllers
{
    [TestClass]
    public class UserControllerTest
    {
        TestAppContext context;
        UserController controller;

        [TestInitialize]
        public void SetUp()
        {
            Mapper.CreateMap<User, UserAdd>();
            context = new TestAppContext();
            controller = new UserController(context);
        }

        [TestMethod]
        public void UserAdd()
        {
            // Arrange
            var itemData = Mapper.Map<UserAdd>(GetUserData());
            SetupController(controller, HttpMethod.Post);

            // Act
            IHttpActionResult result = controller.Post(itemData);

            // Assert
            var negResult = result as CreatedNegotiatedContentResult<UserBase>;
            Assert.AreEqual(1, negResult.Content.UserId);
            Assert.AreEqual(itemData.Email, negResult.Content.Email);
        }

        [TestMethod]
        public void UserGetById()
        {
            // Arrange
            User userData = GetUserData();
            SetupUserData(context);
            SetupController(controller, HttpMethod.Get);

            // Act
            IHttpActionResult result = controller.Get(userData.UserId);

            // Assert
            var negResult = result as OkNegotiatedContentResult<UserBase>;
            Assert.AreEqual(userData.UserId, negResult.Content.UserId);
            
        }

        //[TestMethod]
        public void UserEdit()
        {
            // Arrange
            SetupUserData(context);
            SetupController(controller, HttpMethod.Put);

            var itemData = new UserEdit
            {
                UserId = 1,
                PhoneNumber = "123-456-9876"                
            };

            // Act
            //IHttpActionResult result = controller.Put(itemData.ItemId, itemData);

            // Assert
            //var negResult = result as OkNegotiatedContentResult<ItemBase>;
        }

        //[TestMethod]
        public void UserEditLocation()
        {
            // Arrange
            SetupUserData(context);
            SetupController(controller, HttpMethod.Put);

            GoogleMap map = new GoogleMap();

            var userEditLocationData = new UserEditLocation
            {
                UserId = 1,
                PreferableLocation = new Location{
                    PostalCode = "M2V2C9",
                    Street = "70 Pond Road",
                    City ="Toronto",
                    Province ="ON"//, map
                }
            };
        }

        [TestMethod]
        public void UserDelete()
        {
            // Arrange
            SetupUserData(context);
            SetupController(controller, HttpMethod.Delete);
            int id = GetUserData().UserId;

            User result = context.Users.Find(id);
            Assert.IsNotNull(result);

            // Act
            controller.Delete(id);

            // Assert
            result = context.Users.Find(id);
            Assert.IsNull(result);
        }

        // ##################################################################
        // Retrieve sample data
        private User GetUserData()
        {
            var itemData = new User
            {
                UserId = 1,
                FirstName = "Eunju",
                LastName = "Han",
                Email = "ejhan4@myseneca.ca",
                PhoneNumber = "123-456-7890",
                PreferableLocation = new Location()

                ////these properties are not allowed to add when adding new user
                //FavoriteItems,
                //IsLogged,
                //Messages,
                //PurchaseHistories
            };

            return itemData;
        }

        // Add sample data to context
        private void SetupUserData(TestAppContext context)
        {
            User user = GetUserData();
            context.Users.Add(user);
        }

        private static void SetupController(ApiController controller, HttpMethod htttpMethod)
        {
            AutoMapperConfig.RegisterMappings();
            var config = new HttpConfiguration();
            var request = new HttpRequestMessage(HttpMethod.Post, "http://localhost/api/message");
            var route = config.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{id}");
            var routeData = new HttpRouteData(route,
                new HttpRouteValueDictionary { { "controller", "user" } });

            controller.ControllerContext = new HttpControllerContext(config, routeData, request);
            controller.Request = request;
            controller.Request.Properties[HttpPropertyKeys.HttpConfigurationKey] = config;
            controller.Request.Properties[HttpPropertyKeys.HttpRouteDataKey] = routeData;
            controller.Url = new UrlHelper(request);
        }
    }
}
