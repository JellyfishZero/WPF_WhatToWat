using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WhatToEat.Commands;
using WhatToEat.Data;
using WhatToEat.Helper;
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

    /// <summary>
    /// 新增餐廳完成事件的參數類別
    /// </summary>
    public class AddRestaurantCompletedEventArgs : EventArgs
    {
        public AddRestaurantCompletedEventArgs(AddRestaurantResult result, string message)
        {
            Result = result;
            Message = message;
        }

        public AddRestaurantResult Result { get; }
        public string Message { get; }
    }

    public class AddRestaurantVM : ViewModelBase
    {
        public ICommand ApplyWeekdaysBusinessHoursCommand { get; }
        public ICommand ApplyAllBusinessHoursCommand { get; }
        public ICommand AddRestaurantCommand { get; }

        public event EventHandler<AddRestaurantCompletedEventArgs>? AddRestaurantCompleted;

        public List<string> HourItems { get; } = RestaurantEditFormHelper.CreateHourItems();

        public List<string> MinuteItems { get; } = RestaurantEditFormHelper.CreateMinuteItems();

        public List<BusinessHourInputVM> BusinessHours { get; } =
            RestaurantEditFormHelper.CreateBusinessHours();

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

            ApplyWeekdaysBusinessHoursCommand = new RelayCommand(() =>
                ApplyDefaultBusinessHours(false)
            );
            ApplyAllBusinessHoursCommand = new RelayCommand(() => ApplyDefaultBusinessHours(true));
            AddRestaurantCommand = new RelayCommand(AddRestaurantByCommand);
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

        private void AddRestaurantByCommand()
        {
            var result = AddRestaurant();

            string message = result == AddRestaurantResult.Success
                ? "餐廳已新增"
                : ErrorMessage;

            AddRestaurantCompleted?.Invoke(
                this,
                new AddRestaurantCompletedEventArgs(result, message)
            );
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
                var invalidBusinessHours =
                    RestaurantEditFormHelper.GetInvalidBusinessHourDayNames(BusinessHours);

                if (invalidBusinessHours.Count > 0)
                {
                    ErrorMessage =
                        RestaurantEditFormHelper.CreateInvalidBusinessHoursMessage(
                            invalidBusinessHours
                        );

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

            RestaurantEditFormHelper.ResetBusinessHours(BusinessHours);
        }

        public void ApplyDefaultBusinessHours(bool includeWeekend)
        {
            RestaurantEditFormHelper.ApplyDefaultBusinessHours(
                BusinessHours,
                includeWeekend,
                DefaultStartHour,
                DefaultStartMinute,
                DefaultEndHour,
                DefaultEndMinute
            );
        }
    }
}
