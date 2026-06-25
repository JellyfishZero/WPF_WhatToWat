using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatToEat.Models;

namespace WhatToEat.ViewModels
{
    public class ModifyRestaurantVM
    {
        private readonly RestaurantService _restaurantService;

        public ModifyRestaurantVM(RestaurantService restaurantService)
        {
            _restaurantService = restaurantService;
        }
    }
}
