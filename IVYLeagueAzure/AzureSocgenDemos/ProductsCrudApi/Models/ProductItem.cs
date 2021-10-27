using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProductsCrudApi.Models
{
    [Table("ProductItem")]
    public class ProductItem
    {
        [Key]
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public long ProductBrandId { get; set; }
        public string ImagePath { get; set; }
        public string ThumbnailPath { get; set; }
    }
}
