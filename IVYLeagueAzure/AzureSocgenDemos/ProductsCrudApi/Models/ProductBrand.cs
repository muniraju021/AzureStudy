using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProductsCrudApi.Models
{
    [Table("ProductBrand")]
    public class ProductBrand
    {
        [Key]
        public long Id { get; set; }
        public string Name { get; set; }
    }
}
