using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SenecaFleaServer.Models
{
    public class PurchaseHistory
    {
        // TODO: Miguel, Confirm these properties

        public PurchaseHistory()
        {
            PurchaseDate = DateTime.Now.AddSeconds(1);
        }

        [Required]
        public int Id { get; set; }

        [Required]
        public Item item { get; set; }

        [Required]
        public User Seller { get; set; }

        [Required]
        public DateTime PurchaseDate { get; set; }
    }
}