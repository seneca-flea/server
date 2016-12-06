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

        private User GetUserByUserID(int userID)
        {
            var user = ds.Users.SingleOrDefault(u => u.UserId == userID);
            return user;
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


        // Get all conversations by userId
        public IEnumerable<ConversationBase> ConversationGetAllByCurrentUser()
        {            
            User cUser = GetCurrentUser();
            if (cUser == null) { return null; }

            // Set HasNewMessage of cUser to false
            cUser.HasNewMessage = false;

            // Get current user's conversations, ordered by Time
            var cc = ds.Conversations
                .Where(c => c.user1 == cUser.UserId | c.user2 == cUser.UserId)
                .OrderByDescending(c => c.Time);

            IEnumerable<ConversationBase> cList = Mapper.Map<IEnumerable<ConversationBase>>(cc);

            // Add Receiver First Name and Last Name
            foreach (ConversationBase c in cList)
            {
                User receiver = null;
                if (c.User1 == cUser.UserId)
                {
                    //Get User2
                    receiver = GetUserByUserID(c.User2);
                    c.UserFirstName = receiver.FirstName;
                    c.UserLastName = receiver.LastName;
                }
                else if (c.User2 == cUser.UserId)
                {
                    //Get User1
                    receiver = GetUserByUserID(c.User1);
                    c.UserFirstName = receiver.FirstName;
                    c.UserLastName = receiver.LastName;
                }

                // Get the latest message that talk to the current user
                var msg = ds.Messages
                    .Where(m => m.SenderId == receiver.UserId | m.ReceiverId == receiver.UserId)
                    .OrderByDescending(t => t.Time).Take(1);

                IEnumerable<MessageBase> recentMsg = Mapper.Map<IEnumerable<MessageBase>>(msg);
                foreach (MessageBase message in recentMsg)
                {
                    c.recentMessage = message;
                }
            }

            // save updated current user's HasNewMessage value
            ds.SaveChanges();

            return cList;


            //// Get current user's msgs
            //User currentUser = GetCurrentUser();
            //if (currentUser == null) { return null; }

            ////var cc = ds.Conversations.Where(c => c.user1 == currentUser.UserId | c.user2 == currentUser.UserId);

            //var cc = ds.Conversations.Include("Message").Where(c => c.user1 == currentUser.UserId | c.user2 == currentUser.UserId);

            //// Ordered by Time
            //cc.OrderByDescending(c => c.Time);
            //IEnumerable<ConversationWithMessage> cList = Mapper.Map<IEnumerable<ConversationWithMessage>>(cc);            

            //// Add Receiver First Name and Last Name
            //foreach (ConversationWithMessage c in cList)
            //{
            //    c.Messages.OrderByDescending(t => t.Time);

            //    User receiver;
            //    if (c.User1 == currentUser.UserId)
            //    {
            //        //Get User2
            //        receiver = GetUserByUserID(c.User2);
            //        c.UserFirstName = receiver.FirstName;
            //        c.UserLastName = receiver.LastName;
            //    }
            //    else if (c.User2 == currentUser.UserId)
            //    {
            //        //Get User1
            //        receiver = GetUserByUserID(c.User1);
            //        c.UserFirstName = receiver.FirstName;
            //        c.UserLastName = receiver.LastName;
            //    }
            //}

            //return cList;

        }

        // Get a conversation by a receiver
        public ConversationBase ConversationGetByReceiver(int receiverId)
        {
            // Get current user's msgs
            User cUser = GetCurrentUser();
            if (cUser == null) { return null; }

            // get conversation, ordered by Time
            var cc = ds.Conversations
                .Where(c => c.user1 == cUser.UserId | c.user2 == cUser.UserId)
                .OrderByDescending(c => c.Time);
                        
            // within current user's conversations, only take the conversation matched with receiverId
            ConversationBase re = new ConversationBase();
            IEnumerable<ConversationBase> cList = Mapper.Map<IEnumerable<ConversationBase>>(cc);            
            foreach (ConversationBase c in cList) {                
                User receiver;
                if (c.User1 == cUser.UserId && c.User2 == receiverId)
                {
                    //Get User2
                    receiver = GetUserByUserID(c.User2);
                    c.UserFirstName = receiver.FirstName;
                    c.UserLastName = receiver.LastName;
                    re = c;
                }
                else if (c.User2 == cUser.UserId && c.User1 == receiverId)
                {
                    //Get User1
                    receiver = GetUserByUserID(c.User1);
                    c.UserFirstName = receiver.FirstName;
                    c.UserLastName = receiver.LastName;
                    re = c;
                }

                //// Get the latest message that talk to receiverId
                //var msg = ds.Messages
                //    .Where(m => m.SenderId == cUser.UserId || m.ReceiverId == cUser.UserId)
                //    .Where(m => m.SenderId == receiverId || m.ReceiverId == receiverId)
                //    .OrderByDescending(t => t.Time).Take(1);

                //IEnumerable<MessageBase> recentMsg = Mapper.Map<IEnumerable<MessageBase>>(msg);
                //foreach (MessageBase message in recentMsg)
                //{
                //    re.recentMessage = message;
                //}
            }
            return (re == null) ? null : re;                        
        }


        // Get a conversation with its messages by a receiver
        internal object ConversationGetWithMessagesByReceiver(int receiverId)
        {            
            // CurrentUser exists?
            User cUser = GetCurrentUser();
            if (cUser == null) { return null; }

            if(cUser.UserId == receiverId) { return null; }

            // Receiver exists?
            User receiver = ds.Users.Find(receiverId);
            if (receiver == null) { return null; }

            // Shouldn't be CurrentUser = Receiver
            if (cUser == receiver) return null;

            // Set HasNewMessage of cUser to false
            cUser.HasNewMessage = false;

            // Get current user's conversations, ordered by Time
            var cc = ds.Conversations.Include("Messages")
                .Where(c => c.user1 == cUser.UserId || c.user2 == cUser.UserId)
                .Where(c => c.user1 == receiverId || c.user2 == receiverId)
                .OrderByDescending(u => u.Time);

            // Prepared the result
            ConversationWithMessage re = new ConversationWithMessage(); ;
            IEnumerable<ConversationWithMessage> cList = Mapper.Map<IEnumerable<ConversationWithMessage>>(cc);
            foreach (ConversationWithMessage cm in cList) {                
                // Add Receiver First Name and Last Name
                if (cm.User1 == cUser.UserId)
                {
                    //Get User2
                    receiver = GetUserByUserID(cm.User2);
                    cm.UserFirstName = receiver.FirstName;
                    cm.UserLastName = receiver.LastName;
                    re = cm;
                }
                else if (cm.User2 == cUser.UserId)
                {
                    //Get User1
                    receiver = GetUserByUserID(cm.User1);
                    cm.UserFirstName = receiver.FirstName;
                    cm.UserLastName = receiver.LastName;
                    re = cm;
                }

                //// Get current user's msgs that talk to receiverId
                //var msg = ds.Messages
                //    .Where(c => c.SenderId == cUser.UserId || c.ReceiverId == cUser.UserId)
                //    .Where(c => c.SenderId == receiverId || c.ReceiverId == receiverId)
                //    .OrderByDescending(t => t.Time).Take(1);

                //IEnumerable<MessageBase> recentMsg = Mapper.Map<IEnumerable<MessageBase>>(msg);
                //foreach (MessageBase message in recentMsg)
                //{
                //    re.recentMessage = message;
                //}

            }
            return (re == null) ? null : re;
        }

        //// Get a conversation with its messages by a receiver
        //public ConversationWithMessage ConversationFilterByReceiverWithMessages(int receiverId)
        //{
        //    var co = ds.Conversations.Include("Messages").SingleOrDefault(c => c.user2 == receiverId);            

        //    return (co == null) ? null : Mapper.Map<ConversationWithMessage>(co);
        //}       

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
            User cUser = GetCurrentUser();
            if (cUser == null) { return; }

            if(cUser.UserId == receiverId) { return; }

            // Check if the user exists
            var receiver = ds.Users.Find(receiverId);
            // Continue?
            if (receiver == null) { return; }

            try
            {
                // Get current user's msgs
                var cc = ds.Conversations.Include("Messages")
                .Where(c => c.user1 == cUser.UserId || c.user2 == cUser.UserId)
                .Where(c => c.user1 == receiverId || c.user2 == receiverId);

                var msgs = ds.Messages
                    .Where(c => c.SenderId == cUser.UserId || c.ReceiverId == cUser.UserId)
                    .Where(c => c.SenderId == receiverId || c.ReceiverId == receiverId);

                if (cc.Count() == 0 || msgs.Count() == 0) return;

                var cList = Mapper.Map<IEnumerable<Conversation>>(cc);
                var mList = Mapper.Map<IEnumerable<Message>>(msgs);

                ds.Conversations.Remove(cList.First());
                ds.Messages.RemoveRange(mList);
                ds.SaveChanges();
            }
            catch (Exception e)
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
            User cUser = GetCurrentUser();

            var c = ds.Messages
                .Where(u => u.SenderId == cUser.UserId | u.ReceiverId == cUser.UserId)
                .OrderBy(m => m.MessageId);

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
            User cUser = GetCurrentUser();
            if (cUser.UserId != newItem.SenderId) { return null; }

            // Check for matching users
            var sender = ds.Users.SingleOrDefault(u => u.UserId == newItem.SenderId);
            var receiver = ds.Users.SingleOrDefault(u => u.UserId == newItem.ReceiverId);

            // Continue?
            if (sender == null || receiver == null) { return null; }

            // Check for matching item if the item field is not empty
            if (newItem.ItemId != null)
            {
                var itemId = ds.Items.SingleOrDefault(i => i.ItemId == newItem.ItemId);
                if (itemId == null) { return null; }
            }

            // Set id
            int? newId = ds.Messages.Select(m => (int?)m.MessageId).Max() + 1;
            if (newId == null) { newId = 1; }

            var addedItem = Mapper.Map<Message>(newItem);
            addedItem.MessageId = (int)newId;

            var cc = ds.Conversations
                .Where(c => c.user1 == newItem.SenderId || c.user2 == newItem.SenderId)
                .Where(c => c.user1 == newItem.ReceiverId || c.user2 == newItem.ReceiverId);           

            IEnumerable<Conversation> cList = Mapper.Map<IEnumerable<Conversation>>(cc);

            if (cList.Count() == 0)    //new conversation
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
                addedItem.Conversation = cList.First();
                ds.Messages.Add(addedItem);
            }
            //set HasNewMessage to receiver
            receiver.HasNewMessage = true;

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
            if (sender == null || receiver == null) { return; }

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
            User cUser = GetCurrentUser();
            if (cUser == null) return;

            var storedItem = ds.Messages.Find(id);

            if ((storedItem.SenderId != cUser.UserId) && (storedItem.ReceiverId != cUser.UserId))
                return;

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
            msgs.OrderByDescending(t=>t.Time);

            return Mapper.Map<IEnumerable<MessageBase>>(msgs);
        }

        // Get messages by identifiers, filted by a receiver
        public IEnumerable<MessageBase> MessageFilterByUserIdWithReceiver(int receiverId)
        {
            var userMsgs = MessageFilterByUserId(GetCurrentUser().UserId);
                        
            var msgs = userMsgs.Where(u => u.ReceiverId == receiverId);

            msgs.OrderByDescending(u => u.Time);

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