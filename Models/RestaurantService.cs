using System.Linq;
using Microsoft.EntityFrameworkCore;
using WhatToEat.Data;

namespace WhatToEat.Models
{
    public class RestaurantService
    {
        private readonly IDbContextFactory<AppDbContext> _dbFactory;

        public RestaurantService(IDbContextFactory<AppDbContext> dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public bool NameExists(string name)
        {
            using var db = _dbFactory.CreateDbContext();

            return db.Restaurants.Any(r => r.Name == name);
        }

        public void Add(Restaurant restaurant)
        {
            using var db = _dbFactory.CreateDbContext();

            db.Restaurants.Add(restaurant);
            db.SaveChanges();
        }
    }
}
