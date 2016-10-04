using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using SenecaFleaServer.Models;
using System.Data.Entity;

namespace SenecaFleaServer.Controllers
{
    public class ItemManager
    {
        private DataContext ds;

        public ItemManager() { ds = new DataContext(); }
        public ItemManager(DataContext context) { ds = context; }

        public ItemBase ItemGetById(int id)
        {
            var result = ds.Items.SingleOrDefault(i => i.ItemId == id);

            return Mapper.Map<ItemBase>(result);
        }

        public ItemBase ItemAdd(ItemAdd newItem)
        {
            if (newItem == null) { return null; }

            // Set id
            int? newId = ds.Items.Select(m => (int?)m.ItemId).Max() + 1;
            if (newId == null) { newId = 1; }

            // Add item
            Item addedItem = Mapper.Map<Item>(newItem);
            addedItem.ItemId = (int)newId;

            ds.Items.Add(addedItem);
            ds.SaveChanges();

            return Mapper.Map<ItemBase>(addedItem);
        }

        public ItemBase ItemEdit(ItemEdit editedItem)
        {
            if (editedItem == null) { return null; }

            // Fetch the object
            //var storedItem = ds.Items.Find(editedItem.ItemId);
            var storedItem = ds.Items.First(i => i.ItemId == editedItem.ItemId);

            if (storedItem == null) { return null; }

            // Edit object
            ds.Entry(storedItem).CurrentValues.SetValues(editedItem);

            // For unit test (broken)
            //storedItem = Mapper.Map<Item>(editedItem);
            //ds.MarkAsModified(storedItem);

            ds.SaveChanges();

            return Mapper.Map<ItemBase>(storedItem);
        }

        public void ItemDelete(int id)
        {
            var storedItem = ds.Items.Find(id);

            if (storedItem != null)
            {
                ds.Items.Remove(storedItem);
                ds.SaveChanges();
            }
        }

        public IEnumerable<ItemBase> FilterByCourse(int id)
        {
            var course = ds.Courses.SingleOrDefault(c => c.CourseId == id);

            if (course == null) { return null; }

            var items = ds.Items.Where(
                i => i.Courses.FirstOrDefault(c => c.CourseId == id) == course);

            return Mapper.Map<IEnumerable<ItemBase>>(items);
        }
    }
}