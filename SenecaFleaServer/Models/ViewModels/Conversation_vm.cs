using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SenecaFleaServer.Models
{
    public class ConversationAdd
    {
        public ConversationAdd()
        {
            Time = DateTime.Now;
        }

        [Required]
        public int User1 { get; set; }

        [Required]
        public int User2 { get; set; }

        public DateTime? Time { get; set; }
    }

    public class ConversationBase : ConversationAdd
    {
        [Key]
        public int ConversationId { get; set; }
    }

    public class ConversationWithMessage : ConversationBase
    {
        public virtual ICollection<Message> Messages { get; set; }
    }
    

    public class ConversationFilterByReceiverWithDate
    {
        public int UserId { get; set; }

        public DateTime Time { get; set; }
    }
}
