using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SenecaFleaServer.Models
{
    public class GoogleMap
    {
        // ATTENTION: Fill this
        public GoogleMap()
        {

        }

        [Required]
        public int GoogleMapId { get; set; }

        [Column(TypeName = "numeric")]
        public decimal latitude { get; set; }

        [Column(TypeName = "numeric")]
        public decimal longitude { get; set; }
    }
}