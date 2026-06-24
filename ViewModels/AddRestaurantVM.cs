using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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

    public class AddRestaurantVM : INotifyPropertyChanged
    {
        private readonly RestaurantService _restaurantService;
        private string _restaurantName = "";
        private int _preferenceScore = 3;
        private bool _hasBusinessHours;

        public AddRestaurantVM(RestaurantService restaurantService)
        {
            _restaurantService = restaurantService;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public string RestaurantName
        {
            get => _restaurantName;
            set => SetField(ref _restaurantName, value);
        }

        public int PreferenceScore
        {
            get => _preferenceScore;
            set => SetField(ref _preferenceScore, value);
        }

        public bool HasBusinessHours
        {
            get => _hasBusinessHours;
            set => SetField(ref _hasBusinessHours, value);
        }

        public AddRestaurantResult AddRestaurant(List<BusinessHour> businessHours)
        {
            string name = RestaurantName.Trim();

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
                PreferenceScore = PreferenceScore,
                HasBusinessHours = HasBusinessHours,
            };

            if (HasBusinessHours)
            {
                restaurant.BusinessHours.AddRange(businessHours);
            }

            _restaurantService.Add(restaurant);

            return AddRestaurantResult.Success;
        }

        public void Reset()
        {
            RestaurantName = "";
            PreferenceScore = 3;
            HasBusinessHours = false;
        }

        private void SetField<T>(
            ref T field,
            T value,
            [CallerMemberName] string? propertyName = null
        )
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
            {
                return;
            }

            field = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
