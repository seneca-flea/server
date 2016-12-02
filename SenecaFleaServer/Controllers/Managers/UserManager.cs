using AutoMapper;
using SenecaFleaServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Http;

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
            var c = ds.Users.OrderBy(i => i.UserId);

            return Mapper.Map<IEnumerable<UserBase>>(c);
        }
        
        // Get current User info
        public UserBase GetCurrentUser()
        {
            var u = HttpContext.Current.User as ClaimsPrincipal;
            if (!HttpContext.Current.User.Identity.IsAuthenticated)
                throw new HttpResponseException(System.Net.HttpStatusCode.Unauthorized);
            // Fetch the object
            var currentUser = ds.Users.SingleOrDefault(i => i.Email == u.Identity.Name);
            return Mapper.Map<UserBase>(currentUser);
        }

        // Get user by identifier
        public UserBase UserGetById(int id)
        {
            var o = ds.Users.SingleOrDefault(i => i.UserId == id);
            
            return Mapper.Map<UserBase>(o);
        }

        // Get user by identifier
        public UserWithLocation UserUserWithLocationById(int id)
        {
            var o = ds.Users.SingleOrDefault(i => i.UserId == id);

            return Mapper.Map<UserWithLocation>(o);
        }

        // Get user by identifier
        public UserWithAllInfo UserGetWithAllInfoById(int id)
        {
            var o = ds.Users.SingleOrDefault(i => i.UserId == id);
            //var oMap = Mapper.Map<UserWithAllInfo>(o);

            return Mapper.Map<UserWithAllInfo>(o);
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

        //// Edit user
        //public UserBase UserEdit(UserEdit editedItem)
        //{
        //    if (editedItem == null) { return null; }

        //    // Fetch the object
        //    //var storedItem = ds.Items.Find(editedItem.ItemId);
        //    var storedItem = ds.Users.First(i => i.UserId == editedItem.UserId);

        //    if (storedItem == null) { return null; }

        //    // Edit object
        //    ds.Entry(storedItem).CurrentValues.SetValues(editedItem);

        //    ds.SaveChanges();

        //    return Mapper.Map<UserBase>(storedItem);
        //}


        // Edit user location
        public UserBase UserEditLocation(UserEditLocation editedItem)
        {
            //var u = HttpContext.Current.User as ClaimsPrincipal;
            //if (!HttpContext.Current.User.Identity.IsAuthenticated)
            //    throw new HttpResponseException(System.Net.HttpStatusCode.Unauthorized);

            //// Fetch the object
            //var storedItem = ds.Users.SingleOrDefault(i => i.Email == u.Identity.Name);

            var storedItem = ds.Users.SingleOrDefault(i => i.UserId == editedItem.UserId);
            if (storedItem == null) { return null; }

            // Edit object
            if (editedItem.Location.LocationId != 0)
            {
                var loc = ds.Locations.SingleOrDefault(e => e.LocationId == editedItem.Location.LocationId);
                ds.Entry(loc).CurrentValues.SetValues(editedItem.Location);
            }
            else
            {
                var location = Mapper.Map<Location>(editedItem.Location);
                storedItem.PreferableLocations.Add(location);
            }

            ds.SaveChanges();

            return Mapper.Map<UserBase>(storedItem);
        }

        // Delete a user
        public void UserDelete(int id)
        {
            UserBase cUser = GetCurrentUser();
            if(cUser.UserId != id) { return;  }

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

        // Get user's purchase history
        public IEnumerable<PurchaseHistoryBase> UserGetHistory(int id)
        {
            var user = ds.Users
                .Include("PurchaseHistories")
                .Include("PurchaseHistories.Item")
                .SingleOrDefault(i => i.UserId == id);

            if (user == null)
            {
                return null;
            }
            else
            {
                var items = user.PurchaseHistories;

                return Mapper.Map<IEnumerable<PurchaseHistoryBase>>(items);
            }
        }

        // Add to user's purchase history
        public bool UserAddHistory(int userId, PurchaseHistoryAdd obj)
        {
            var user = ds.Users.SingleOrDefault(i => i.UserId == userId);
            var seller = ds.Users.SingleOrDefault(i => i.UserId == obj.SellerId);
            var item = ds.Items.SingleOrDefault(i => i.ItemId == obj.ItemId);

            if (user == null || seller == null || item == null)
            {
                return false;
            }
            else
            {
                // Set item status
                item.Status = "Unavailable";

                // Add to history
                var history = Mapper.Map<PurchaseHistory>(obj);
                history.Item = item;

                ds.PurchaseHistories.Add(history);
                user.PurchaseHistories.Add(history);
                ds.SaveChanges();

                return true;
            }
        }

        // Remove form user's purchase history
        public bool UserDeleteHistory(int userId, int historyId)
        {
            var user = ds.Users.SingleOrDefault(i => i.UserId == userId);
            var history = ds.PurchaseHistories.Include("Item")
                .SingleOrDefault(i => i.Id == historyId);

            if (user == null || history == null)
            {
                return false;
            }
            else
            {
                // Remove from history
                history.Item.Status = "Available";
                user.PurchaseHistories.Remove(history);
                ds.PurchaseHistories.Remove(history);
                ds.SaveChanges();

                return true;
            }
        }
    }
}