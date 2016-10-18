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

        [Required, StringLength(25)]
        public string Name { get; set; }

        [Required, StringLength(25)]
        public string ProgramName { get; set; }

        [StringLength(7)]
        public string Code { get; set; }
    }
}