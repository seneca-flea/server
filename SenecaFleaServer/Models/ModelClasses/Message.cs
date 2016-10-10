using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SenecaFleaServer.Models
{
    public class Message
    {
        public Message()
        {
            time = DateTime.Now;
        }

        [Required]
        public int MessageId { get; set; }

        [Required, StringLength(1000)]
        public string text { get; set; }

        [Required]
        public virtual User sender { get; set; }

        [Required]
        public virtual User receiver { get; set; }

        public DateTime time { get; set; }

        public int ItemId { get; set; }
    }
}