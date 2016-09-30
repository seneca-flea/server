using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SenecaFleaServer.Models.ModelClasses
{
    public class Category
    {
        public int CategoryId { get; set; }

        [StringLength(25)]
        public string Name { get; set; }
    }
}