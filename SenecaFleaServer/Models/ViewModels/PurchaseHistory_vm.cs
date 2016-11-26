using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SenecaFleaServer.Models
{
    public class PurchaseHistoryBase
    {
        public PurchaseHistoryBase()
        {
            PurchaseDate = DateTime.Now;
        }

        public int Id { get; set; }

        [Required]
        public ItemBase Item { get; set; }

        [Required]
        public UserBase Seller { get; set; }

        [Required]
        public DateTime PurchaseDate { get; set; }
    }

    public class PurchaseHistoryAdd
    {
        [Required]
        public int ItemId { get; set; }

        [Required]
        public int SellerId { get; set; }
    }
}