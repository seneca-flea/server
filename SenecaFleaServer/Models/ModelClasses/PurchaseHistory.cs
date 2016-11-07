using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SenecaFleaServer.Models
{
    public class PurchaseHistory
    {
        public PurchaseHistory()
        {
            PurchaseDate = DateTime.Now;
        }

        [Required]
        public int Id { get; set; }

        [Required]
        public Item Item { get; set; }

        [Required]
        public User Seller { get; set; }

        [Required]
        public DateTime PurchaseDate { get; set; }
    }
}