using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using SenecaFleaServer.Models;

namespace SenecaFleaServer.Controllers
{
    public class Manager
    {
        private DataContext ds = new DataContext();

        public Manager() { }
        public Manager(DataContext context) { ds = context; }

        // #############################################
        // Item
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
    }
}