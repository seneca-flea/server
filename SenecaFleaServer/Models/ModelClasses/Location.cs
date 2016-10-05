using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SenecaFleaServer.Models
{
    public class Location
    {
        [StringLength(100)]
        public string Street { get; set; }

        [StringLength(20)]
        public string City { get; set; }

        [StringLength(10)]
        public string Province { get; set; }

        [StringLength(6)]
        public string PostalCode { get; set; }
        
        [Required]
        public virtual GoogleMap map { get; set; }
    }
}