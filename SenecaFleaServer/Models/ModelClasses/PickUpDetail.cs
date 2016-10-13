using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SenecaFleaServer.Models
{
    public class PickUpDetail
    {
        // TODO: Miguel, Confirm these properties

        public PickUpDetail()
        {
            PickupDate = DateTime.Now.AddSeconds(1);
        }

        [Required]
        public int PickUpDetailId { get; set; }

        [Required]
        public Location PickupLocation { get; set; }

        [Required]
        public DateTime PickupDate { get; set; }
    }
}