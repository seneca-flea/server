﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SenecaFleaServer.Models
{
    public class LocationAdd
    {
        [StringLength(100)]
        public string Address { get; set; }

        [StringLength(20)]
        public string City { get; set; }

        [StringLength(20)]
        public string Province { get; set; }

        [StringLength(20)]
        public string Country { get; set; }

        [StringLength(10)]
        public string PostalCode { get; set; }

        [Column(TypeName = "numeric")]
        public decimal latitude { get; set; }

        [Column(TypeName = "numeric")]
        public decimal longitude { get; set; }
    }

    public class LocationBase : LocationAdd
    {
        [Key]
        public int LocationId { get; set; }
    }

    //public class LocationWithGoogleMap : LocationAdd
    //{
    //    [Required]
    //    public GoogleMap map { get; set; }
    //}
}