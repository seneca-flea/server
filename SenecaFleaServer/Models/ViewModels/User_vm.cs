using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SenecaFleaServer.Models
{
    public class UserAdd
    {
        [Required, StringLength(100)]
        public string FirstName { get; set; }

        [Required, StringLength(100)]
        public string LastName { get; set; }

        [StringLength(100)]
        [EmailAddress]
        public string Email { get; set; }

        public string PhoneNumber { get; set; }
    }

    public class UserBase : UserAdd
    {
        [Key]
        public int UserId { get; set; }

        public bool IsLogged { get; set; }
    }

    public class UserWithLocation : UserBase
    {
        public virtual Location PreferableLocation { get; set; }
    }

    public class UserEdit
    {
        [Key]
        public int UserId { get; set; }

        //these properties below are not allowed to edit
        //FirstName, LastName, Email

        public string PhoneNumber { get; set; }
    }

    public class UserEditLocation
    {
        [Key]
        public int UserId { get; set; }

        public virtual Location PreferableLocation { get; set; }
    }

    public class UserWithAllInfo : UserWithLocation
    {
        public virtual ICollection<Item> FavoriteItems { get; set; }

        public virtual ICollection<Message> Messages { get; set; }

        public virtual ICollection<PurchaseHistory> PurchaseHistories { get; set; }
    }
}