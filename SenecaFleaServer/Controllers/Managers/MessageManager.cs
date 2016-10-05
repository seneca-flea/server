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

        public MessageManager()
        {
            ds = new DataContext();
        }

        public MessageManager(DataContext context)
        {
            ds = context;
        }

        public IEnumerable<MessageBase> MessageGetAll()
        {
            var fetchedObjs = ds.Messages.OrderBy(m => m.id);
            return (fetchedObjs == null) ? null : Mapper.Map<IEnumerable<MessageBase>>(fetchedObjs);            
        }

        public MessageBase MessageGetById(int id)
        {
            var fechedObj = ds.Messages.SingleOrDefault(m => m.id == id);
            return (fechedObj == null) ? null : Mapper.Map<MessageBase>(fechedObj);
        }

        public MessageBase MessageAdd(MessageAdd newItem)
        {
            // Set id
            int? newId = ds.Messages.Select(m => (int?)m.id).Max() + 1;
            if (newId == null) { newId = 1; }

            var addedItem = Mapper.Map<Message>(newItem);
            addedItem.id = (int)newId;
            ds.Messages.Add(addedItem);
            ds.SaveChanges();

            return (addedItem == null) ? null : Mapper.Map<MessageBase>(addedItem);
        }

        public void MessageDelete(int id)
        {
            var storedItem = ds.Messages.Find(id);

            if (storedItem == null)
            {
                //Throw an exception
                throw new NotImplementedException();
            }
            else
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