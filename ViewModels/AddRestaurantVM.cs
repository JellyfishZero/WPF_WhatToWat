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
        InvalidBusinessHours,
    }

    public class AddRestaurantVM : ViewModelBase
    {
        public List<string> HourItems { get; } =
        [
            "00",
            "01",
            "02",
            "03",
            "04",
            "05",
            "06",
            "07",
            "08",
            "09",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "20",
            "21",
            "22",
            "23",
        ];

        public List<string> MinuteItems { get; } = ["00", "30"];

        public List<BusinessHourInputVM> BusinessHours { get; } =
        [
            new(DayOfWeek.Monday, "週一"),
            new(DayOfWeek.Tuesday, "週二"),
            new(DayOfWeek.Wednesday, "週三"),
            new(DayOfWeek.Thursday, "週四"),
            new(DayOfWeek.Friday, "週五"),
            new(DayOfWeek.Saturday, "週六"),
            new(DayOfWeek.Sunday, "週日"),
        ];

        private readonly RestaurantService _restaurantService;
        private string _restaurantName = "";
        private int _preferenceScore = 3;
        private bool _hasBusinessHours;
        private string _errorMessage = "";
        private string _defaultStartHour = "09";
        private string _defaultStartMinute = "00";
        private string _defaultEndHour = "21";
        private string _defaultEndMinute = "00";

        public AddRestaurantVM(RestaurantService restaurantService)
        {
            _restaurantService = restaurantService;
        }

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

        public string ErrorMessage
        {
            get => _errorMessage;
            private set => SetField(ref _errorMessage, value);
        }

        public string DefaultStartHour
        {
            get => _defaultStartHour;
            set => SetField(ref _defaultStartHour, value);
        }

        public string DefaultStartMinute
        {
            get => _defaultStartMinute;
            set => SetField(ref _defaultStartMinute, value);
        }

        public string DefaultEndHour
        {
            get => _defaultEndHour;
            set => SetField(ref _defaultEndHour, value);
        }

        public string DefaultEndMinute
        {
            get => _defaultEndMinute;
            set => SetField(ref _defaultEndMinute, value);
        }

        public AddRestaurantResult AddRestaurant()
        {
            ErrorMessage = "";

            string name = RestaurantName.Trim();

            if (string.IsNullOrWhiteSpace(name))
            {
                ErrorMessage = "請輸入餐廳名稱";
                return AddRestaurantResult.EmptyName;
            }

            if (_restaurantService.NameExists(name))
            {
                ErrorMessage = "店家名稱已存在";
                return AddRestaurantResult.DuplicatedName;
            }

            if (HasBusinessHours)
            {
                var invalidBusinessHours = BusinessHours
                    .Where(x => x.IsOpen && !x.IsTimeRangeValid())
                    .Select(x => x.DayName)
                    .ToList();

                if (invalidBusinessHours.Count > 0)
                {
                    ErrorMessage =
                        "以下營業日的開始時間必須早於結束時間："
                        + Environment.NewLine
                        + string.Join(Environment.NewLine, invalidBusinessHours);

                    return AddRestaurantResult.InvalidBusinessHours;
                }
            }

            var restaurant = new Restaurant
            {
                Name = name,
                PreferenceScore = PreferenceScore,
                HasBusinessHours = HasBusinessHours,
            };

            if (HasBusinessHours)
            {
                restaurant.BusinessHours.AddRange(BusinessHours.Select(x => x.ToBusinessHour()));
            }

            _restaurantService.Add(restaurant);

            return AddRestaurantResult.Success;
        }

        public void Reset()
        {
            ErrorMessage = "";
            RestaurantName = "";
            PreferenceScore = 3;
            HasBusinessHours = false;
            DefaultStartHour = "09";
            DefaultStartMinute = "00";
            DefaultEndHour = "21";
            DefaultEndMinute = "00";

            foreach (var businessHour in BusinessHours)
            {
                businessHour.Reset();
            }
        }

        public void ApplyDefaultBusinessHours(
            bool includeWeekend
        )
        {
            foreach (var businessHour in BusinessHours)
            {
                bool isWeekend =
                    businessHour.DayOfWeek == DayOfWeek.Saturday
                    || businessHour.DayOfWeek == DayOfWeek.Sunday;

                if (!includeWeekend && isWeekend)
                {
                    continue;
                }

                businessHour.IsOpen = true;
                businessHour.StartHour = DefaultStartHour;
                businessHour.StartMinute = DefaultStartMinute;
                businessHour.EndHour = DefaultEndHour;
                businessHour.EndMinute = DefaultEndMinute;
            }
        }
    }
}
