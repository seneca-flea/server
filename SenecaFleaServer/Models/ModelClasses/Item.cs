using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SenecaFleaServer.Models
{
    public class Item
    {
        public Item()
        {
            Images = new HashSet<Image>();
            Timestamp = DateTime.Now;
        }

        [Required]
        public int ItemId { get; set; }

        [Required, StringLength(50)]
        public string Title { get; set; }

        [Required]
        public decimal Price { get; set; }

        [StringLength(1500)]
        public string Description { get; set; }

        [StringLength(35)]
        public string Status { get; set; }

        public string Type { get; set; }

        public Course Course { get; set; }

        public ICollection<Image> Images { get; set; }

        public virtual PickUpDetail PickUp { get; set; }

        public virtual Book Book { get; set; }

        public DateTime Timestamp { get; set; }

        public int SellerId { get; set; }
    }
}