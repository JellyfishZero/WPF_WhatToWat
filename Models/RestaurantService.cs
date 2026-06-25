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

            return db.Restaurants.Include(r => r.BusinessHours).OrderBy(r => r.Id).ToList();
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

        /// <summary>
        /// 允許維持自己的原名，但不能改成其他餐廳已使用的名稱。
        /// </summary>
        public bool NameExistsExceptId(string name, int excludedRestaurantId)
        {
            using var db = _dbFactory.CreateDbContext();

            return db.Restaurants.Any(r => r.Id != excludedRestaurantId && r.Name == name);
        }

        public bool Update(
            int id,
            string name,
            int preferenceScore,
            bool hasBusinessHours,
            IEnumerable<BusinessHour> businessHours
        )
        {
            using var db = _dbFactory.CreateDbContext();

            var target = db
                .Restaurants.Include(r => r.BusinessHours)
                .FirstOrDefault(r => r.Id == id);

            if (target == null)
            {
                return false;
            }

            target.Name = name;
            target.PreferenceScore = preferenceScore;
            target.HasBusinessHours = hasBusinessHours;

            target.BusinessHours.Clear();

            if (hasBusinessHours)
            {
                target.BusinessHours.AddRange(businessHours);
            }

            db.SaveChanges();
            return true;
        }
    }
}
