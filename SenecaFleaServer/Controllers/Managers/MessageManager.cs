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
        

        // Get messages by identifiers
        public IEnumerable<MessageBase> FilterByUserId(int userId)
        {
            //TODO: How would I retrieve the current user id?

            // Find the user if exists
            var user = ds.Users.SingleOrDefault(u => u.UserId == userId);
            if (user == null) { return null; }

            // Get messages by userid
            var msgs = ds.Messages.Where(u => u.SenderId == userId);

            return Mapper.Map<IEnumerable<MessageBase>>(msgs);
        }

        // Get messages by identifiers, filted by one receivedId
        public IEnumerable<MessageBase> FilterByUserIdWithReceiverId(MessageFilterByUserIdWithReceiverId filterObj)
        {
            var userMsgs = FilterByUserId(filterObj.UserId);
                        
            var msgs = userMsgs.Where(u => u.ReceiverId == filterObj.ReceiverId);

            return Mapper.Map<IEnumerable<MessageBase>>(msgs);
        }

        // Get messages by identifiers, filtered by datetime
        public IEnumerable<MessageBase> FilterByUserIdWithDate(MessageFilterByUserIdWithTime filterObj)
        {
            var userMsgs = FilterByUserId(filterObj.UserId);
            
            var msgs = userMsgs.Where(u => u.Time == filterObj.Time);

            return Mapper.Map<IEnumerable<MessageBase>>(msgs);
        }

        // Get messages by identifiers, filtered by item
        public IEnumerable<MessageBase> FilterByUserIdWithItem(MessageFilterByUserIdWithItem filterObj)
        {
            var userMsgs = FilterByUserId(filterObj.UserId);            

            var msgs = userMsgs.Where(u => u.ItemId == filterObj.ItemId);

            return Mapper.Map<IEnumerable<MessageBase>>(msgs);
        }

    }
}