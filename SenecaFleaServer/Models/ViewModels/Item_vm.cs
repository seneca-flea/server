using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SenecaFleaServer.Models
{
    // TODO: Complete ItemAdd and ItemEdit
    public class ItemAdd
    {
        [Required, StringLength(50, MinimumLength = 3)]
        public string Title { get; set; }

        [Required]
        public string Status { get; set; }

        [Required]
        public int SellerId { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public string Type { get; set; }

        public string BookTitle { get; set; }

        public int BookYear { get; set; }

        public string BookPublisher { get; set; }

        public string BookAuthor { get; set; }
    }

    public class ItemEdit
    {
        [Key]
        public int ItemId { get; set; }

        public string Title { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
    }

    public class ItemBase : ItemAdd
    {
        [Key]
        public int ItemId { get; set; }

        /// <summary>
        /// Number of images the item contains
        /// </summary>
        public int ImagesCount { get; set; }
    }

    public class ItemWithMedia : ItemBase 
    {
        public IEnumerable<Image> Images { get; set; }
    }

    public class ImageAdd
    {
        public string ContentType { get; set; }
        public byte[] Photo { get; set; }
    }
}