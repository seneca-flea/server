using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using SenecaFleaServer.Models;

namespace SenecaFleaServer.Controllers
{
    public class AdminManager
    {
        private DataContext ds;

        public AdminManager()
        {
            ds = new DataContext();
        }

        // Get items; only used for Item Administration 
        public IEnumerable<ItemBase> ItemGet()
        {
            var items = ds.Items.OrderBy(i => i.Title);

            return Mapper.Map<IEnumerable<ItemBase>>(items);
        }

        public ItemWithMedia ItemGetByIdWithMedia(int id)
        {
            var result = ds.Items.Include("Images")
                .SingleOrDefault(i => i.ItemId == id);

            return Mapper.Map<ItemWithMedia>(result);
        }

        public Image ItemImageGetById(int id)
        {
            var o = ds.Images.Find(id);
            return (o == null) ? null : Mapper.Map<Image>(o);
        }

        // Delete item
        public void ItemDelete(int id)
        {
            var storedItem = ds.Items.Find(id);

            if (storedItem != null)
            {
                ds.Items.Remove(storedItem);
                ds.SaveChanges();
            }
        }

        //public ItemPhotos VehiclePhotoGetById(int id)
        //{
        //    var o = ds.Items.Find(id);

        //    return (o == null) ? null : Mapper.Map<ItemPhotos>(o);
        //}

    }
}