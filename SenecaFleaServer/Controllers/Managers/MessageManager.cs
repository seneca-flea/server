using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SenecaFleaServer.Models;
using AutoMapper;
using System.Security.Claims;
using System.Web.Http;

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


        private User GetCurrentUser()
        {
            var u = HttpContext.Current.User as ClaimsPrincipal;
            if (!HttpContext.Current.User.Identity.IsAuthenticated)
                throw new HttpResponseException(System.Net.HttpStatusCode.Unauthorized);
            // Fetch the object
            var currentUser = ds.Users.SingleOrDefault(i => i.Email == u.Identity.Name);
            return currentUser;
        }

        #region Conversation for administration

        // Get all conversations
        public IEnumerable<ConversationBase> ConversationGetAll()
        {
            var cs = ds.Conversations.OrderBy(c => c.ConversationId);//.Take(100);
            return Mapper.Map<IEnumerable<ConversationBase>>(cs);
        }

        // Get all conversations by userId
        public IEnumerable<ConversationBase> ConversationGetAllByUserId(int userId)
        {
            var cc = ds.Conversations.Where(c => c.user1 == userId | c.user2 == userId);
            return Mapper.Map<IEnumerable<ConversationBase>>(cc);
        }

        // Get a conversation by an conversation identifier
        public ConversationBase ConversationGetById(int id)
        {
            var o = ds.Conversations.SingleOrDefault(c => c.ConversationId == id);
            return (o == null) ? null : Mapper.Map<ConversationBase>(o);
        }
        
        #endregion  Conversation for administration

        // Get a conversation by a receiver
        public ConversationBase ConversationGetByReceiver(int receiverId)
        {
            var o = ds.Conversations.SingleOrDefault(c => c.user2 == receiverId);
            return (o == null) ? null : Mapper.Map<ConversationBase>(o);
        }

        // Get all conversations by userId
        public IEnumerable<ConversationBase> ConversationGetAllByCurrentUser()
        {
            User currentUser = GetCurrentUser();
            if (currentUser == null)
            {
                //throw new Exception();
                return null;
            }

            var cc = ds.Conversations.Where(c => c.user1 == currentUser.UserId | c.user2 == currentUser.UserId);
            return Mapper.Map<IEnumerable<ConversationBase>>(cc);
        }


        // Get a conversation with its messages by a receiver
        public ConversationWithMessage ConversationFilterByReceiverWithMessages(int receiverId)
        {
            var co = ds.Conversations.Include("Messages").SingleOrDefault(c => c.user2 == receiverId);            

            return (co == null) ? null : Mapper.Map<ConversationWithMessage>(co);
        }       

        // Get messages by identifiers, filtered by datetime
        public IEnumerable<ConversationBase> ConversationFilterByDate
            (ConversationFilterByReceiverWithDate filterObj)
        {
            var u = ds.Users.Find(filterObj.UserId);
            if (u == null) return null;           

            var conversations = ds.Conversations.Where(c => c.Time == filterObj.Time);

            return Mapper.Map<IEnumerable<ConversationBase>>(conversations);
        }       

        // Delete a conversation including its messages by ReceiverId or SenderId
        public void ConversationDeleteByReceiver(int receiverId)
        {
            // Check if the user exists
            var receiver = ds.Users.Find(receiverId);
            // Continue?
            if (receiver == null)
            {
                //throw new Exception();
                return;
            }

            var u = HttpContext.Current.User as ClaimsPrincipal;
            if (!HttpContext.Current.User.Identity.IsAuthenticated)
                throw new HttpResponseException(System.Net.HttpStatusCode.Unauthorized);
            // Fetch the object
            var currentUser = ds.Users.SingleOrDefault(i => i.Email == u.Identity.Name);
            if (currentUser == null)
            {
                //throw new Exception();
                return;
            }

            var conversation = ds.Conversations
                .Where(m => m.user1 == currentUser.UserId && m.user2 == receiverId);
            var msgs = ds.Messages
                .Where(m => m.SenderId == currentUser.UserId && m.ReceiverId == receiverId);

            var d1= Mapper.Map<Conversation>(conversation);
            var d2 = Mapper.Map<ICollection<Message>>(msgs);

            if (conversation == null || msgs == null)
            {
                //throw new Exception();
                //log and handle it
                return;
            }
            
            try
            {
                ds.Conversations.Remove(Mapper.Map<Conversation>(conversation));
                ds.Messages.RemoveRange(Mapper.Map<ICollection<Message>>(msgs));
                ds.SaveChanges();
            }
            catch (Exception)
            {
                //throw new Exception();
                return;
            
            }
        }
       
        // Delete a conversation by conversationid
        public void ConversationDelete(int id)
        {
            var conversation = ds.Conversations.Find(id);

            if (conversation != null)
            {
                try
                {
                    ds.Conversations.Remove(conversation);
                    ds.SaveChanges();
                }
                catch (Exception)
                {
                    //log and handle it
                    return;
                }
            }
        }



        #region Message

        // Get all messages
        public IEnumerable<MessageBase> MessageGetAll()
        {
            var c = ds.Messages.OrderBy(m => m.MessageId);//.Take(100);
            return Mapper.Map<IEnumerable<MessageBase>>(c);
        }

        // Get a message by an identifier
        public MessageBase MessageGetById(int id)
        {
            var o = ds.Messages.SingleOrDefault(m => m.MessageId == id);
            return (o == null) ? null : Mapper.Map<MessageBase>(o);
        }
       
        // Add a message
        public MessageBase MessageAdd(MessageAdd newItem)
        {
            // Check for matching users
            var senderId = ds.Users.SingleOrDefault(u => u.UserId == newItem.SenderId);
            var receiverId = ds.Users.SingleOrDefault(u => u.UserId == newItem.ReceiverId);

            // Continue?
            if (senderId == null || receiverId == null)
            {
                return null;
            }

            // Check for matching item if the item field is not empty
            if (newItem.ItemId != null)
            {
                var itemId = ds.Items.SingleOrDefault(i => i.ItemId == newItem.ItemId);
                if (itemId == null)
                {
                    return null;
                }
            }
            
            // Set id
            int? newId = ds.Messages.Select(m => (int?)m.MessageId).Max() + 1;
            if (newId == null) { newId = 1; }

            var addedItem = Mapper.Map<Message>(newItem);
            addedItem.MessageId = (int)newId;

            User currentUser = GetCurrentUser();
            var conversation = ds.Conversations.SingleOrDefault(c => c.user1 == currentUser.UserId || c.user2 == currentUser.UserId);            
            if(conversation == null)    //new conversation
            {
                Conversation con = new Conversation();
                con.user1 = newItem.SenderId;
                con.user2 = newItem.ReceiverId;
                con.Time = DateTime.Now;
                con.Messages.Add(addedItem);
                
                ds.Conversations.Add(con);
            }
            else //existing conversation
            {
                addedItem.Conversation = conversation;
                ds.Messages.Add(addedItem);
            }

            ds.SaveChanges();

            return (addedItem == null) ? null : Mapper.Map<MessageBase>(addedItem);
        }        

        // Delete messages
        public void MessageDelete(int SenderId, int ReceiverId)
        {
            // Check if the users exist
            var sender = ds.Users.Find(SenderId);
            var receiver = ds.Users.Find(ReceiverId);
            // Continue?
            if (sender == null | receiver == null)
            {
                //throw new Exception();
                return;
            }

            // Attention: when deleting messages, only messages that a user sends are deleted?
            var msgs = ds.Messages.Where(m => m.SenderId == SenderId
                    && m.ReceiverId == ReceiverId);

            if (msgs == null) return;
            
            try
            {
                ds.Messages.RemoveRange(msgs);
                ds.SaveChanges();
            }
            catch (Exception)
            {
                return;
            }            
        }

        // Delete a message
        public void MessageDeleteById(int id)
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

        #endregion Message

        // Get messages that a user sends or receives by its identifiers
        public IEnumerable<MessageBase> MessageFilterByUserId(int userId)
        {
            // Find the user if exists
            var user = ds.Users.SingleOrDefault(u => u.UserId == userId);
            if (user == null) { return null; }

            // Get messages that a user sends or receives by its identifier
            var msgs = ds.Messages.Where(u => u.SenderId == userId | u.ReceiverId == userId);

            return Mapper.Map<IEnumerable<MessageBase>>(msgs);
        }

        // Get messages by identifiers, filted by a receiver
        public IEnumerable<MessageBase> MessageFilterByUserIdWithReceiver(MessageFilterByUserIdWithReceiver filterObj)
        {
            var userMsgs = MessageFilterByUserId(filterObj.UserId);
                        
            var msgs = userMsgs.Where(u => u.ReceiverId == filterObj.ReceiverId);

            return Mapper.Map<IEnumerable<MessageBase>>(msgs);
        }

        // Get messages by identifiers, filtered by datetime
        public IEnumerable<MessageBase> MessageFilterByUserIdWithDate(MessageFilterByUserIdWithTime filterObj)
        {
            var userMsgs = MessageFilterByUserId(filterObj.UserId);
            
            var msgs = userMsgs.Where(u => u.Time == filterObj.Time);

            return Mapper.Map<IEnumerable<MessageBase>>(msgs);
        }

        // Get messages by identifiers, filtered by item
        public IEnumerable<MessageBase> MessageFilterByUserIdWithItem(MessageFilterByUserIdWithItem filterObj)
        {
            var userMsgs = MessageFilterByUserId(filterObj.UserId);            

            var msgs = userMsgs.Where(u => u.ItemId == filterObj.ItemId);

            return Mapper.Map<IEnumerable<MessageBase>>(msgs);
        }

    }
}