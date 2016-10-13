using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SenecaFleaServer.Models
{
    public class Book
    {
        // TODO: Miguel's work - I have added some you might need to change them later on

        [Required]
        public int BookId { get; set; }


        [Required, StringLength(100)]
        public string Title { get; set; }

        [StringLength(40)]
        public string Author { get; set; }

        [StringLength(40)]
        public string Publisher { get; set; }
        
        public int PublishedYear { get; set; }
    }
}