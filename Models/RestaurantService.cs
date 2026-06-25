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

        public List<Restaurant> GetAll()
        {
            using var db = _dbFactory.CreateDbContext();

            return db.Restaurants
                .Include(r => r.BusinessHours)
                .OrderBy(r => r.Id)
                .ToList();
        }

        public void Delete(Restaurant restaurant)
        {
            using var db = _dbFactory.CreateDbContext();

            var target = db.Restaurants.FirstOrDefault(r => r.Id == restaurant.Id);

            if (target == null)
            {
                return;
            }

            db.Restaurants.Remove(target);
            db.SaveChanges();
        }

        public void DeleteAll()
        {
            using var db = _dbFactory.CreateDbContext();

            var restaurants = db.Restaurants.ToList();

            db.Restaurants.RemoveRange(restaurants);
            db.SaveChanges();
        }
    }
}
