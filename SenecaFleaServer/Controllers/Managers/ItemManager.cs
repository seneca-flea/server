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

        // Constructors
        public ItemManager()
        {
            ds = new DataContext();
        }

        public ItemManager(DataContext context)
        {
            ds = context;
        }

        // Get item by identifier
        public ItemBase ItemGetById(int id)
        {
            var result = ds.Items.Include("Courses")
                .SingleOrDefault(i => i.ItemId == id);

            return Mapper.Map<ItemBase>(result);
        }

        public ItemWithMedia ItemGetByIdWithMedia(int id) 
        {
            var result = ds.Items.Include("Courses").Include("Images")
                .SingleOrDefault(i => i.ItemId == id);      

            return Mapper.Map<ItemWithMedia>(result);      
        }

        // Add item
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

        // Add Image
        public bool ItemSetPhoto(int id, string contentType, byte[] photo)
        {
            if (string.IsNullOrEmpty(contentType) | photo == null) { return false; }

            // Find matching object
            var storedItem = ds.Items.Find(id);

            if (storedItem == null) { return false; }

            // TODO: Save the photo

            return (ds.SaveChanges() > 0) ? true : false;
        }

        // Edit item
        public ItemBase ItemEdit(ItemEdit editedItem)
        {
            if (editedItem == null) { return null; }

            // Fetch the object
            var storedItem = ds.Items.SingleOrDefault(c => c.ItemId == editedItem.ItemId);

            if (storedItem == null) { return null; }

            // Edit object
            ds.Entry(storedItem).CurrentValues.SetValues(editedItem);

            // For unit test (broken)
            //storedItem = Mapper.Map<Item>(editedItem);
            //ds.MarkAsModified(storedItem);

            ds.SaveChanges();

            return Mapper.Map<ItemBase>(storedItem);
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

        // Get items by category
        public IEnumerable<ItemBase> FilterByStatus(string status)
        {
            var items = ds.Items.Where(c => c.Status == status);

            return Mapper.Map<IEnumerable<ItemBase>>(items);
        }

        // Get items by course name
        public IEnumerable<ItemBase> FilterByCourseName(string courseName)
        {
            // Find if course exists
            var course = ds.Courses.SingleOrDefault(c => c.Name == courseName);
            if (course == null) { return null; }

            var items = ds.Items.Where(
                i => i.Courses.FirstOrDefault(c => c.Name == courseName) == course);

            return Mapper.Map<IEnumerable<ItemBase>>(items);
        }

        // Get items by course code
        public IEnumerable<ItemBase> FilterByCourseCode(string courseCode)
        {
            // Find if course exists
            var course = ds.Courses.SingleOrDefault(c => c.Code == courseCode);
            if (course == null) { return null; }

            var items = ds.Items.Where(
                i => i.Courses.FirstOrDefault(c => c.Code == courseCode) == course);

            return Mapper.Map<IEnumerable<ItemBase>>(items);
        }
    }
}