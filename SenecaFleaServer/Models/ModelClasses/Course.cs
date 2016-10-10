using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SenecaFleaServer.Models
{
    public class Course
    {
        public int CourseId { get; set; }

        [StringLength(25)]
        public string Name { get; set; }

        [StringLength(6)]
        public string Code { get; set; }
    }
}