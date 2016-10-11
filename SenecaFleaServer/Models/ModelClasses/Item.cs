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
            Courses = new HashSet<Course>();
        }

        [Required]
        public int ItemId { get; set; }

        [Required, StringLength(50)]
        public string Title { get; set; }

        [Required]
        public decimal Price { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        [StringLength(35)]
        public string Status { get; set; }

        public ICollection<Course> Courses { get; set; }

        public ICollection<Image> ImageSet { get; set; }

        public virtual PickUpDetail PickUp { get; set; }

        public virtual User Seller { get; set; }
    }
}