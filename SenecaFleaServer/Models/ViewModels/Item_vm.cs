using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SenecaFleaServer.Models
{
    // TODO: Complete ItemAdd and ItemEdit
    public class ItemAdd
    {
        public string Title { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
    }

    public class ItemEdit
    {
        [Key]
        public int ItemId { get; set; }

        public string Title { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
    }

    public class ItemBase : ItemAdd
    {
        [Key]
        public int ItemId { get; set; }
    }

    public class ItemWithMedia : ItemBase 
    {
        public IEnumerable<Image> Images { get; set; }
    }
}