﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
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
using AutoMapper;
#pragma warning disable CS0618

namespace SenecaFleaServer.Tests.Controllers
{
    [TestClass]
    public class MessageControllerTest
    {
        TestAppContext context;
        MessageController controller;

        [TestInitialize]
        public void SetUp()
        {
            AutoMapperConfig.RegisterMappings();
            Mapper.CreateMap<Message, MessageAdd>();
            context = new TestAppContext();
            controller = new MessageController(context);
        }

        [TestMethod]
        public void MessageGetById()
        {
            Message message = SetUpMessageData();
            SetUpController(controller, HttpMethod.Get);

            IHttpActionResult result = controller.Get(message.MessageId);

            var negResult = result as OkNegotiatedContentResult<MessageBase>;
            Assert.AreEqual(message.MessageId, negResult.Content.MessageId);
            Assert.AreEqual(message.ItemId, negResult.Content.ItemId);
            Assert.AreEqual(message.Text, negResult.Content.Text);
            //TODO: [Han] add test data to campare
            //Assert.AreEqual("2016-10-04T00:00:00", negResult.Content.time);
            //Assert.AreEqual(new User { }, negResult.Content.sender);
            //Assert.AreEqual(new User { }, negResult.Content.receiver);
        }

        //[TestMethod]
        public void MessageAdd()
        {
            var message = Mapper.Map<MessageAdd>(GetMessageData());
            SetUpController(controller, HttpMethod.Post);

            IHttpActionResult result = controller.Post(message);

            var negResult = result as CreatedNegotiatedContentResult<MessageBase>;
            Assert.AreEqual(1, negResult.Content.MessageId);
            Assert.AreEqual(message.ItemId, negResult.Content.ItemId);
            Assert.AreEqual(message.Text, negResult.Content.Text);
            //TODO: [Han] add test data to campare
            //Assert.AreEqual("2016-10-04T00:00:00", negResult.Content.time);
            //Assert.AreEqual(new User { }, negResult.Content.sender);
            //Assert.AreEqual(new User { }, negResult.Content.receiver);
        }

        //[TestMethod]
        public void MessageDelete()
        {
            int id = SetUpMessageData().MessageId;
            SetUpController(controller, HttpMethod.Delete);

            Message result = context.Messages.Find(id);
            Assert.IsNotNull(result);

            controller.Delete(id);

            result = context.Messages.Find(id);
            Assert.IsNull(result);
        }

        // ##################################################################
        // Retrieve sample data
        private Message GetMessageData()
        {
            context.Users.Add(new User() { UserId = 101 });
            context.Users.Add(new User() { UserId = 102 });
            context.Items.Add(new Item() { ItemId = 1 });

            var message = new Message {
                MessageId = 1,
                ItemId = 1,
                SenderId = 101,
                ReceiverId = 102,
                Time = DateTime.Now,
                Text = "Hello World"
            };

            return message;
        }

        // Add sample data to context
        private Message SetUpMessageData()
        {
            Message message = GetMessageData();
            context.Messages.Add(message);

            return message;
        }

        private static void SetUpController(ApiController controller, HttpMethod httpMethod)
        {
            var config = new HttpConfiguration();
            var request = new HttpRequestMessage(httpMethod, "http://localhost/api/message");
            var route = config.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{id}");
            var routeData = new HttpRouteData(route, 
                new HttpRouteValueDictionary { { "controller", "message" } });

            controller.ControllerContext = new HttpControllerContext(config, routeData, request);
            controller.Request = request;
            controller.Request.Properties[HttpPropertyKeys.HttpConfigurationKey] = config;
            controller.Request.Properties[HttpPropertyKeys.HttpRouteDataKey] = routeData;
            controller.Url = new UrlHelper(request);
        }
    }
}
