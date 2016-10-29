using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SenecaFleaServer.Models
{
    public class MessageAdd
    {
        [Required, StringLength(1000)]
        public string Text { get; set; }

        [Required]
        public int SenderId { get; set; }

        [Required]
        public int ReceiverId { get; set; }

        public DateTime Time { get; set; }

        public int? ItemId { get; set; }
    }


    public class MessageBase : MessageAdd
    {
        [Key]
        public int MessageId { get; set; }
    }

    //
    public class MessageWithItem : MessageBase
    {
        public Item Item { get; set; }
    }


    #region Filter

    public class MessageFilterByUserIdWithReceiverId
    {
        public int UserId { get; set; }

        public int ReceiverId { get; set; }
    }


    public class MessageFilterByUserIdWithTime
    {
        public int UserId { get; set; }

        public DateTime Time { get; set; }
    }

    public class MessageFilterByUserIdWithItem
    {
        public int UserId { get; set; }

        public int ItemId { get; set; }

        //public Item Item { get; set; }
    }

    #endregion Filter

}