using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SenecaFleaServer.Models
{
    public class Image
    {
        [Required]
        public int ImageId { get; set; }

        [Required, StringLength(50)]
        public string ContentType { get; set; }

        [Required]
        public byte[] Photo { get; set; }        
    }
}