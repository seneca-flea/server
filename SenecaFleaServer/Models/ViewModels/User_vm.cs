using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SenecaFleaServer.Models
{
    public class UserAdd
    {
        //[Required, StringLength(100)]
        //public string FirstName { get; set; }

        //[Required, StringLength(100)]
        //public string LastName { get; set; }

        [StringLength(100)]
        [EmailAddress]
        public string Email { get; set; }

        //public string PhoneNumber { get; set; }
    }

    public class UserBase : UserAdd
    {
        [Key]
        public int UserId { get; set; }        
    }

    public class UserWithLocation : UserBase
    {
        public IEnumerable<LocationBase> PreferableLocations { get; set; }
    }

    //public class UserEdit
    //{
    //    [Key]
    //    public int UserId { get; set; }
    //}

    public class UserEditLocation
    {
        [Key]
        public int UserId { get; set; }

        public LocationBase Location { get; set; }
    }

    public class UserWithAllInfo : UserBase
    {
        public virtual IEnumerable<ItemBase> FavoriteItems { get; set; }

        public virtual IEnumerable<MessageBase> Messages { get; set; }

        public virtual IEnumerable<PurchaseHistoryBase> PurchaseHistories { get; set; }
    }
}