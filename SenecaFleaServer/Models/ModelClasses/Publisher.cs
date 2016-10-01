﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SenecaFleaServer.Models
{
    public class Publisher
    {
        public int PublisherId { get; set; }

        [StringLength(25)]
        public string Name { get; set; }
    }

    public class PublisherItem
    {
    }
}