using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatToEat.Data;

namespace WhatToEat.Models
{
    public class RestaurantService
    {
        private readonly AppDbContext _db;

        public RestaurantService(AppDbContext db)
        {
            _db = db;
        }

        public bool NameExists(string name)
        {
            return _db.Restaurants.Any(r => r.Name == name);
        }

        public void Add(Restaurant restaurant)
        {
            _db.Restaurants.Add(restaurant);
            _db.SaveChanges();
        }
    }
}
