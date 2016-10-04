using Microsoft.VisualStudio.TestTools.UnitTesting;
using SenecaFleaServer.Controllers;
using SenecaFleaServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Hosting;
using System.Web.Http.Results;
using System.Web.Http.Routing;

namespace SenecaFleaServer.Tests.Controllers
{
    [TestClass]
    class MessageControllerTest
    {
        TestAppContext context;
        MessageController controller;


        public void SetUp()
        {
            context = new TestAppContext();
            controller = new MessageController();
        }



        public void MessageGetById()
        {
            //MessageAdd message = GetMessageData();
            SetUpMessageData(context);
            SetUpController(controller, HttpMethod.Get);

            IHttpActionResult result = controller.Get(1);

            var negResult = result as OkNegotiatedContentResult<MessageBase>;
            Assert.AreEqual(1, negResult.Content.id);
            Assert.AreEqual(1, negResult.Content.ItemId);
            //TODO: [Han] add test data to campare
            //Assert.AreEqual("", negResult.Content.text);
            //Assert.AreEqual("2016-10-04T00:00:00", negResult.Content.time);
            //Assert.AreEqual(new User { }, negResult.Content.sender);
            //Assert.AreEqual(new User { }, negResult.Content.receiver);
        }


        [TestMethod]
        public void MessageAdd()
        {
            MessageAdd message = GetMessageData();
            SetUpController(controller, HttpMethod.Post);


            IHttpActionResult re = controller.Post(message);

            var negResult = re as OkNegotiatedContentResult<MessageBase>;
            Assert.AreEqual(2, negResult.Content.id);
            Assert.AreEqual(2, negResult.Content.ItemId);
            //TODO: [Han] add test data to campare
            //Assert.AreEqual("", negResult.Content.text);
            //Assert.AreEqual("2016-10-04T00:00:00", negResult.Content.time);
            //Assert.AreEqual(new User { }, negResult.Content.sender);
            //Assert.AreEqual(new User { }, negResult.Content.receiver);
        }


        [TestMethod]
        public void MessageDelete()
        {
            SetUpMessageData(context);
            SetUpController(controller, HttpMethod.Delete);

            Message result = context.Messages.Find(1);
            Assert.IsNotNull(result);

            controller.Delete(1);

            result = context.Messages.Find(1);
            Assert.IsNull(result);
        }

        private MessageAdd GetMessageData()
        {
            var itemData = new MessageAdd {
                sender = new User(),
                receiver = new User(),
                ItemId = 1,
                time = DateTime.Now
            };

            return itemData;
        }

        private void SetUpMessageData(TestAppContext context)
        {
            MessageAdd messageData = GetMessageData();
            context.Messages.Add(new Message {
                ItemId = messageData.ItemId,
                sender = messageData.sender,
                receiver = messageData.receiver,
                time = messageData.time
            });
        }


        private static void SetUpController(ApiController controller, HttpMethod htttpMethod )
        {
            AutoMapperConfig.RegisterMappings();
            var config = new HttpConfiguration();
            var request = new HttpRequestMessage(HttpMethod.Post, "http://localhost/api/message");
            var route = config.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{id}");
            var routeData = new HttpRouteData(route, new HttpRouteValueDictionary { { "controller", "message" } });


            controller.ControllerContext = new HttpControllerContext(config, routeData, request);
            controller.Request = request;
            controller.Request.Properties[HttpPropertyKeys.HttpConfigurationKey] = config;
            controller.Request.Properties[HttpPropertyKeys.HttpRouteDataKey] = routeData;
            controller.Url = new UrlHelper(request);
        }
    }
}
