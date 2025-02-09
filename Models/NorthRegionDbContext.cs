using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace NorthRegion.Models
{
    public class NorthRegionDbContext: DbContext
    {
        public NorthRegionDbContext(DbContextOptions<NorthRegionDbContext> options): base(options) {}

        public DbSet<NorthRegionViewModel> NorthRegion {get; set;}
    }
}