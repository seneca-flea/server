using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SenecaFleaServer.Models;
using AutoMapper;

namespace SenecaFleaServer.Controllers
{
    public class MessageManager
    {
        private DataContext ds;

        // Constructors
        public MessageManager()
        {
            ds = new DataContext();
        }

        public MessageManager(DataContext context)
        {
            ds = context;
        }

        // Get all messages
        public IEnumerable<MessageBase> MessageGetAll()
        {
            var c = ds.Messages.OrderBy(m => m.MessageId).Take(100);
            return Mapper.Map<IEnumerable<MessageBase>>(c);            
        }

        // Get message by identifiers
        public MessageBase MessageGetById(int id)
        {
            var o = ds.Messages.SingleOrDefault(m => m.MessageId == id);
            return (o == null) ? null : Mapper.Map<MessageBase>(o);
        }

        // Add message
        public MessageBase MessageAdd(MessageAdd newItem)
        {
            // Set id
            int? newId = ds.Messages.Select(m => (int?)m.MessageId).Max() + 1;
            if (newId == null) { newId = 1; }

            var addedItem = Mapper.Map<Message>(newItem);
            addedItem.MessageId = (int)newId;
            ds.Messages.Add(addedItem);
            ds.SaveChanges();

            return (addedItem == null) ? null : Mapper.Map<MessageBase>(addedItem);
        }

        // Delete message
        public void MessageDelete(int id)
        {
            var storedItem = ds.Messages.Find(id);

            if (storedItem != null)
            {
                try
                {
                    ds.Messages.Remove(storedItem);
                    ds.SaveChanges();
                }
                catch (Exception) {
                    //log and handle it
                }
            }
        }
    }
}