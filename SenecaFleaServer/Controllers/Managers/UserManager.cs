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

        // Constructors
        public UserManager()
        {
            ds = new DataContext();
        }

        public UserManager(DataContext context)
        {
            ds = context;
        }

        // Get all users
        public IEnumerable<UserBase> UserGetAll()
        {
            var c = ds.Users.OrderBy(i => i.UserId).Take(100);

            return Mapper.Map<IEnumerable<UserBase>>(c);
        }

        // Get user by identifier
        public UserBase UserGetById(int id)
        {
            var o = ds.Users.SingleOrDefault(i => i.UserId == id);

            return Mapper.Map<UserBase>(o);
        }

        // Add user
        public UserBase UserAdd(UserAdd newItem)
        {
            if (newItem == null) { return null; }

            // Set id
            int? newId = ds.Users.Select(i => (int?)i.UserId).Max() + 1;
            if (newId == null) { newId = 1; }

            // Add item
            User addedItem = Mapper.Map<User>(newItem);
            addedItem.UserId = (int)newId;

            ds.Users.Add(addedItem);
            ds.SaveChanges();

            return Mapper.Map<UserBase>(addedItem);
        }

        // Edit user
        public UserBase UserEdit(UserEdit editedItem)
        {
            if (editedItem == null) { return null; }

            // Fetch the object
            //var storedItem = ds.Items.Find(editedItem.ItemId);
            var storedItem = ds.Users.First(i => i.UserId == editedItem.UserId);

            if (storedItem == null) { return null; }

            // Edit object
            ds.Entry(storedItem).CurrentValues.SetValues(editedItem);

            ds.SaveChanges();

            return Mapper.Map<UserBase>(storedItem);
        }

        // Edit user location
        public UserBase UserEditLocation(UserEditLocation editedItem)
        {
            if (editedItem == null) { return null; }

            // Fetch the object
            //var storedItem = ds.Items.Find(editedItem.ItemId);
            var storedItem = ds.Users.First(i => i.UserId == editedItem.UserId);

            if (storedItem == null) { return null; }

            // Edit object
            ds.Entry(storedItem).CurrentValues.SetValues(editedItem);

            ds.SaveChanges();

            return Mapper.Map<UserBase>(storedItem);
        }

        // Delete user
        public void UserDelete(int id)
        {
            var storedItem = ds.Users.Find(id);

            if (storedItem != null)
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

        // Get favorites from user
        public IEnumerable<ItemBase> UserGetFavorite(int userId)
        {
            var user = ds.Users.Find(userId);

            if (user == null)
            {
                return null;
            }
            else
            {
                var items = user.FavoriteItems;

                return Mapper.Map<IEnumerable<ItemBase>>(items);
            }
        }

        // Add item to a user's favorites
        public bool UserAddFavorite(int userId, int itemId)
        {
            var user = ds.Users.SingleOrDefault(i => i.UserId == userId);
            var item = ds.Items.SingleOrDefault(i => i.ItemId == itemId);

            if (user == null || item == null)
            {
                return false;
            }
            else
            {
                // Add favorite
                user.FavoriteItems.Add(item);
                ds.SaveChanges();

                return true;
            }
        }

        // Remove item from a user's favorites
        public bool UserRemoveFavorite(int userId, int itemId)
        {
            var user = ds.Users.SingleOrDefault(i => i.UserId == userId);
            var item = ds.Items.SingleOrDefault(i => i.ItemId == itemId);

            if (user == null || item == null)
            {
                return false;
            }
            else
            {
                // Remove favorite
                user.FavoriteItems.Remove(item);
                ds.SaveChanges();

                return true;
            }
        }
    }
}