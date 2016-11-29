using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace SenecaFleaServer.Models
{
    public class Conversation
    {
        public Conversation()
        {
            Time = DateTime.Now;
            Messages = new HashSet<Message>();
        }

        [Required]
        public int ConversationId { get; set; }

        [Required]
        public int user1 { get; set; }

        [Required]
        public int user2 { get; set; }

        public DateTime Time { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Message> Messages { get; set; }
    }
}