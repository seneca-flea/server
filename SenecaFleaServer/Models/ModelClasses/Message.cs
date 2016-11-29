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
            Time = DateTime.Now;
        }

        [Required]
        public int MessageId { get; set; }

        [Required, StringLength(1000)]
        public string Text { get; set; }
       
        [Required]
        public int SenderId { get; set; }
        
        [Required]
        public int ReceiverId { get; set; }

        //[Required]
        //public virtual User Sender { get; set; }

        //[Required]
        //public virtual User Receiver { get; set; }

        public DateTime Time { get; set; }

        public int ItemId { get; set; }


        public virtual Conversation Conversation { get; set; }
    }
}