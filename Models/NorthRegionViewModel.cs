using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace NorthRegion.Models
{
    public class NorthRegionViewModel
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public float Price { get; set; }
        public string? ExpiredDate{get; set; }
        [Column("ImageFilename")] //refer to real name of database table.
        [DisplayName("Image")] //the one displaying on web page
        public string? ImageFileName { get; set; }
        [DisplayName("Source of Image")]
        public string? Source { get; set; }
    }
}