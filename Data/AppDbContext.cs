using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatToEat.Models;

namespace WhatToEat.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Restaurant> Restaurants => Set<Restaurant>();

        protected override void OnConfiguring(Microsoft.EntityFrameworkCore.DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=WhatToEat.db");
        }
    }
}
