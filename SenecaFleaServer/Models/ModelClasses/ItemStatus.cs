using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SenecaFleaServer.Models.ModelClasses
{
    public class ItemStatus
    {
        public int StatusId { get; set; }

        [StringLength(35)]
        public string StatusName { get; set; }
    }
}