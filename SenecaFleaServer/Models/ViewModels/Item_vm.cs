﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SenecaFleaServer.Models
{
    public class ItemAdd
    {
        public string Title { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
    }

    public class ItemEdit
    {
        public int ItemId { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
    }

    public class ItemBase : ItemAdd
    {
        public int ItemId { get; set; }
    }
}