using System.IO;
using Microsoft.EntityFrameworkCore;

namespace WhatToEat.Data
{
    public class AppDbContext : DbContext
    {
        private const string FOLDER_NAME = "JellyfishZero_WhatToEat";
        private const string DATABASE_NAME = "WhatToEat.db";

        public DbSet<Restaurant> Restaurants => Set<Restaurant>();
        public DbSet<BusinessHour> BusinessHours => Set<BusinessHour>();

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public static string GetDatabasePath()
        {
            var folder = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                FOLDER_NAME
            );

            Directory.CreateDirectory(folder);

            return Path.Combine(folder, DATABASE_NAME);
        }
    }
}


