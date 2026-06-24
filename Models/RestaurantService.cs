using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatToEat.Data;

namespace WhatToEat.Models
{
    class RestaurantService
    {
        public bool NameExists(string name)
        {
            using var db = new AppDbContext();
            return db.Restaurants.Any(r => r.Name == name);
        }

        public void Add(Restaurant restaurant)
        {
            using var db = new AppDbContext();
            db.Restaurants.Add(restaurant);
            db.SaveChanges();
        }
    }
}
