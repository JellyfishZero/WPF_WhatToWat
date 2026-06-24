using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatToEat.Data;
using WhatToEat.Models;

namespace WhatToEat.ViewModels
{
    public enum AddRestaurantResult
    {
        Success,
        EmptyName,
        DuplicatedName,
    }

    public class AddRestaurantVM
    {
        private readonly RestaurantService _restaurantService;

        public AddRestaurantVM(RestaurantService restaurantService)
        {
            _restaurantService = restaurantService;
        }

        public AddRestaurantResult AddRestaurant(
            string name,
            int preferenceScore,
            bool hasBusinessHours,
            List<BusinessHour> businessHours)
        {
            name = name.Trim();

            if (string.IsNullOrWhiteSpace(name))
            {
                return AddRestaurantResult.EmptyName;
            }

            if (_restaurantService.NameExists(name))
            {
                return AddRestaurantResult.DuplicatedName;
            }

            var restaurant = new Restaurant
            {
                Name = name,
                PreferenceScore = preferenceScore,
                HasBusinessHours = hasBusinessHours,
            };

            if (hasBusinessHours)
            {
                restaurant.BusinessHours.AddRange(businessHours);
            }

            _restaurantService.Add(restaurant);

            return AddRestaurantResult.Success;
        }
    }
}
