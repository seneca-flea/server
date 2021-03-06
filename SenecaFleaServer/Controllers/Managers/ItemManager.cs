﻿using System;
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
        public ItemWithMedia ItemGetById(int id) 
        {
            var result = ds.Items
                .Include("Book").Include("Course").Include("Images")
                .SingleOrDefault(i => i.ItemId == id);      

            return Mapper.Map<ItemWithMedia>(result);      
        }

        // Add item
        public ItemBase ItemAdd(ItemAdd newItem)
        {
            if (newItem == null) { return null; }

            // Check for matching user
            var userId = ds.Users.SingleOrDefault(i => i.UserId == newItem.SellerId);

            if (userId == null) { return null; }

            // Set id
            int? newId = ds.Items.Select(m => (int?)m.ItemId).Max() + 1;
            if (newId == null) { newId = 1; }

            // Add item
            Item addedItem = Mapper.Map<Item>(newItem);
            addedItem.ItemId = (int)newId;

            // Course details
            var course = new Course()
            {
                Name = newItem.CourseName,
                Program = newItem.CourseProgram
            };
            addedItem.Course = course;

            // Book details
            if (newItem.Type == "Book")
            {
                var book = new Book()
                {
                    Title = newItem.BookTitle,
                    Author = newItem.BookAuthor,
                    Publisher = newItem.BookPublisher,
                    Year = newItem.BookYear
                };

                addedItem.Book = book;
            }

            ds.Items.Add(addedItem);
            ds.SaveChanges();

            return Mapper.Map<ItemBase>(addedItem);
        }

        // Add image to item
        public bool ItemAddPhoto(int id, string contentType, byte[] photo)
        {
            if (string.IsNullOrEmpty(contentType) | photo == null) { return false; }

            // Find matching object
            var storedItem = ds.Items.Find(id);

            if (storedItem == null) { return false; }

            // Save the photo
            var image = new ImageAdd
            {
                ContentType = contentType,
                Photo = photo
            };

            storedItem.Images.Add(Mapper.Map<Image>(image));

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

            ds.SaveChanges();

            return Mapper.Map<ItemBase>(storedItem);
        }

        // Delete item
        public void ItemDelete(int id)
        {
            var storedItem = ds.Items.SingleOrDefault(c => c.ItemId == id);

            if (storedItem != null)
            {
                ds.Items.Remove(storedItem);
                ds.SaveChanges();
            }
        }

        // Get items by user id
        public IEnumerable<ItemBase> FilterByUser(int id)
        {
            var items = ds.Items
                .Include("Book").Include("Course")
                .Where(c => c.SellerId == id)
                .OrderByDescending(x => x.ItemId);

            return Mapper.Map<IEnumerable<ItemBase>>(items);
        }

        // Get items by title
        public IEnumerable<ItemBase> FilterByTitle(string title)
        {
            var items = ds.Items.Include("Images").Include("Course")
                .Where(c => c.Title.Contains(title));

            return Mapper.Map<IEnumerable<ItemBase>>(items);
        }

        // Get items by category
        public IEnumerable<ItemBase> FilterByStatus(string status)
        {
            var items = ds.Items.Include("Images").Include("Course")
                .Where(c => c.Status == status);

            return Mapper.Map<IEnumerable<ItemBase>>(items);
        }

        // Get items by course name
        public IEnumerable<ItemBase> FilterByCourseName(string courseName)
        {
            // Find if course exists
            var course = ds.Courses.SingleOrDefault(c => c.Name == courseName);
            if (course == null) { return null; }

            var items = ds.Items.Include("Images").Include("Course")
                .Where(i => i.Course == course);

            return Mapper.Map<IEnumerable<ItemBase>>(items);
        }

        // Get items by course code
        public IEnumerable<ItemBase> FilterByCourseCode(string courseCode)
        {
            // Find if course exists
            var course = ds.Courses.SingleOrDefault(c => c.Code == courseCode);
            if (course == null) { return null; }

            var items = ds.Items.Include("Images").Include("Course")
                .Where(i => i.Course == course);

            return Mapper.Map<IEnumerable<ItemBase>>(items);
        }

        // Get items within price range
        public IEnumerable<ItemBase> FilterByPriceRange(decimal min, decimal max)
        {
            var items = ds.Items.Include("Images")
                .Where(d => d.Price >= min && d.Price <= max).Take(100);

            return Mapper.Map<IEnumerable<ItemBase>>(items);
        }
    }
}