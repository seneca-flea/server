using AutoMapper;
using SenecaFleaServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SenecaFleaServer.Controllers
{
    public class UserManager
    {
        private DataContext ds;

        public UserManager() { ds = new DataContext(); }

        public UserManager(DataContext context) { ds = context; }


        public IEnumerable<UserBase> UserGetAll()
        {
            var c = ds.Users.OrderBy(i => i.Id);

            return Mapper.Map<IEnumerable<UserBase>>(c);
        }

        public UserBase UserGetById(int id)
        {
            var o = ds.Users.SingleOrDefault(i => i.Id == id);

            return Mapper.Map<UserBase>(o);
        }

        public UserBase UserAdd(UserAdd newItem)
        {
            if (newItem == null) { return null; }

            // Set id
            int? newId = ds.Users.Select(i => (int?)i.Id).Max() + 1;
            if (newId == null) { newId = 1; }

            // Add item
            User addedItem = Mapper.Map<User>(newItem);
            addedItem.Id = (int)newId;

            ds.Users.Add(addedItem);
            ds.SaveChanges();

            return Mapper.Map<UserBase>(addedItem);
        }

        public UserBase UserEdit(UserEdit editedItem)
        {
            if (editedItem == null) { return null; }

            // Fetch the object
            //var storedItem = ds.Items.Find(editedItem.ItemId);
            var storedItem = ds.Users.First(i => i.Id == editedItem.Id);

            if (storedItem == null) { return null; }

            // Edit object
            ds.Entry(storedItem).CurrentValues.SetValues(editedItem);

            ds.SaveChanges();

            return Mapper.Map<UserBase>(storedItem);
        }

        public UserBase UserEditLocation(UserEditLocation editedItem)
        {
            if (editedItem == null) { return null; }

            // Fetch the object
            //var storedItem = ds.Items.Find(editedItem.ItemId);
            var storedItem = ds.Users.First(i => i.Id == editedItem.Id);

            if (storedItem == null) { return null; }

            // Edit object
            ds.Entry(storedItem).CurrentValues.SetValues(editedItem);

            ds.SaveChanges();

            return Mapper.Map<UserBase>(storedItem);
        }

        public void UserDelete(int id)
        {
            var storedItem = ds.Users.Find(id);

            if (storedItem == null)
            {
                //Throw an exception
                throw new NotImplementedException();
            }
            else
            {
                try
                {
                    ds.Users.Remove(storedItem);
                    ds.SaveChanges();
                }
                catch (Exception)
                {
                    //log and handle it
                }
            }
        }
    }
}