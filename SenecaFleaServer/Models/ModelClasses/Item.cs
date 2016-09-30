using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SenecaFleaServer.Models.ModelClasses
{
    public class Item
    {
        public int ItemId { get; set; }

        [StringLength(50)]
        public string Title { get; set; }

        public decimal Price { get; set; }

        public int CourseId { get; set; }

        public int PublisherId { get; set; }

        [StringLength(65)]
        public string PhotoPath { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        public DateTime PickUpDate { get; set; }

        public int StreetNumber { get; set; }

        public string StreetName { get; set; }

        [StringLength(20)]
        public string City { get; set; }

        [StringLength(7)]
        public string PostalCode { get; set; }

        public int StatusId { get; set; }

        public int CategoryId { get; set; }

        public int ProgramId { get; set; }

        public virtual CourseItem Course { get; set; }

        public virtual PublisherItem Publisher { get; set; }

        public virtual ItemStatus Status { get; set; }

        public virtual Category Category { get; set; }

        public virtual ProgramItem Program { get; set; }
    }
}