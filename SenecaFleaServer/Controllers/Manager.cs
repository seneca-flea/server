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

        // #############################################
        // Item
        public ItemBase ItemGetById(int id)
        {
            var result = ds.Items.SingleOrDefault(i => i.ItemId == id);

            return Mapper.Map<ItemBase>(result);
        }
    }
}