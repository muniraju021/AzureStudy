using Microsoft.EntityFrameworkCore;
using ProductsCrudApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductsCrudApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<ProductBrand> ProductBrands { get; set; }

        public DbSet<ProductItem> ProductItems { get; set; }
    }
}
