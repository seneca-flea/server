using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SenecaFleaServer.Models
{
    public class User
    {
        public User()
        {
            PreferableLocations = new HashSet<Location>();
            FavoriteItems = new HashSet<Item>();
            Conversations = new HashSet<Conversation>();
            PurchaseHistories = new HashSet<PurchaseHistory>();
        }

        [Required]
        public int UserId { get; set; }

        //public bool IsLogged { get; set; }

        //[Required, StringLength(100)]
        //public string FirstName { get; set; }

        //[Required, StringLength(100)]
        //public string LastName { get; set; }

        //public string PhoneNumber { get; set; }

        [StringLength(100)]
        [EmailAddress]
        public string Email { get; set; }

        public virtual ICollection<Location> PreferableLocations { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Item> FavoriteItems { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Conversation> Conversations { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PurchaseHistory> PurchaseHistories { get; set; }
    }
}