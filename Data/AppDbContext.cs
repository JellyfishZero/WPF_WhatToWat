using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace WhatToEat.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Restaurant> Restaurants => Set<Restaurant>();
        public DbSet<BusinessHour> BusinessHours => Set<BusinessHour>();

        protected override void OnConfiguring(
            Microsoft.EntityFrameworkCore.DbContextOptionsBuilder optionsBuilder
        )
        {
            var folder = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "JellyfishZero_WhatToEat"
            );

            Directory.CreateDirectory(folder);

            var dbPath = Path.Combine(folder, "WhatToEat.db");

            optionsBuilder.UseSqlite($"Data Source={dbPath}");
        }
    }
}
