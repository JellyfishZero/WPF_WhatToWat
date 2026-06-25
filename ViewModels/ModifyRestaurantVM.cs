using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WhatToEat.Commands;
using WhatToEat.Data;
using WhatToEat.Models;

namespace WhatToEat.ViewModels
{
    public class ModifyRestaurantVM : ViewModelBase
    {
        private readonly RestaurantService _restaurantService;

        public ICommand LoadSelectedRestaurantCommand { get; }
        public ICommand ApplyWeekdaysBusinessHoursCommand { get; }
        public ICommand ApplyAllBusinessHoursCommand { get; }
        public ICommand ResetChangesCommand { get; }
        public ICommand UpdateRestaurantCommand { get; }

        public ModifyRestaurantVM(RestaurantService restaurantService)
        {
            _restaurantService = restaurantService;

            LoadSelectedRestaurantCommand = new RelayCommand(LoadSelectedRestaurant);
            ApplyWeekdaysBusinessHoursCommand = new RelayCommand(() =>
                ApplyDefaultBusinessHours(false)
            );
            ApplyAllBusinessHoursCommand = new RelayCommand(() => ApplyDefaultBusinessHours(true));
            ResetChangesCommand = new RelayCommand(ResetChanges);
            UpdateRestaurantCommand = new RelayCommand(UpdateRestaurant);

            LoadRestaurants();
        }

        public ObservableCollection<Restaurant> Restaurants { get; } =
            new ObservableCollection<Restaurant>();
        public List<string> HourItems { get; } =
            Enumerable.Range(0, 24).Select(i => i.ToString("D2")).ToList();
        public List<string> MinuteItems { get; } = new List<string> { "00", "30" };
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

        private Restaurant? _selectedRestaurant;
        private string _restaurantName = "";
        private int _preferenceScore = 3;
        private bool _hasBusinessHours;
        private string _statusMessage = "";
        private bool _isEditorEnabled;

        private string _defaultStartHour = "09";
        private string _defaultStartMinute = "00";
        private string _defaultEndHour = "21";
        private string _defaultEndMinute = "00";

        public Restaurant? SelectedRestaurant
        {
            get => _selectedRestaurant;
            set
            {
                if (SetField(ref _selectedRestaurant, value))
                {
                    IsEditorEnabled = false;
                }
            }
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

        public string StatusMessage
        {
            get => _statusMessage;
            private set => SetField(ref _statusMessage, value);
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

        public bool IsEditorEnabled
        {
            get => _isEditorEnabled;
            private set => SetField(ref _isEditorEnabled, value);
        }

        private void LoadRestaurants()
        {
            Restaurants.Clear();

            foreach (var restaurant in _restaurantService.GetAll())
            {
                Restaurants.Add(restaurant);
            }

            SelectedRestaurant = null;

            StatusMessage =
                Restaurants.Count == 0
                    ? "目前沒有餐廳資料"
                    : $"目前共有 {Restaurants.Count} 間餐廳";
        }

        private void LoadSelectedRestaurant()
        {
            if (SelectedRestaurant == null)
            {
                IsEditorEnabled = false;
                StatusMessage = "請先選擇要修改的餐廳";
                return;
            }

            RestaurantName = SelectedRestaurant.Name;
            PreferenceScore = SelectedRestaurant.PreferenceScore;
            HasBusinessHours = SelectedRestaurant.HasBusinessHours;
            LoadBusinessHoursFromRestaurant(SelectedRestaurant);
            IsEditorEnabled = true;

            StatusMessage = $"已載入「{SelectedRestaurant.Name}」";
        }

        private void LoadBusinessHoursFromRestaurant(Restaurant restaurant)
        {
            ResetBusinessHours();

            if (!restaurant.HasBusinessHours)
            {
                return;
            }

            foreach (var businessHourInput in BusinessHours)
            {
                var businessHour = restaurant.BusinessHours.FirstOrDefault(x =>
                    x.DayOfWeek == businessHourInput.DayOfWeek
                );

                if (businessHour == null)
                {
                    continue;
                }

                businessHourInput.IsOpen = businessHour.IsOpen;

                if (businessHour.OpenTime.HasValue)
                {
                    businessHourInput.StartHour = businessHour.OpenTime.Value.Hours.ToString("D2");
                    businessHourInput.StartMinute = businessHour.OpenTime.Value.Minutes.ToString(
                        "D2"
                    );
                }

                if (businessHour.CloseTime.HasValue)
                {
                    businessHourInput.EndHour = businessHour.CloseTime.Value.Hours.ToString("D2");
                    businessHourInput.EndMinute = businessHour.CloseTime.Value.Minutes.ToString(
                        "D2"
                    );
                }
            }
        }

        private void ResetBusinessHours()
        {
            foreach (var businessHour in BusinessHours)
            {
                businessHour.Reset();
            }
        }

        private void ApplyDefaultBusinessHours(bool includeWeekend)
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

        private void ResetChanges()
        {
            if (SelectedRestaurant == null)
            {
                StatusMessage = "請先選擇要復原的餐廳";
                return;
            }

            LoadSelectedRestaurant();
            StatusMessage = $"已復原「{SelectedRestaurant.Name}」的修改";
        }

        private void UpdateRestaurant()
        {
            if (SelectedRestaurant == null)
            {
                IsEditorEnabled = false;
                StatusMessage = "請先選擇要修改的餐廳";
                return;
            }

            string name = RestaurantName.Trim();

            if (string.IsNullOrWhiteSpace(name))
            {
                StatusMessage = "請輸入餐廳名稱";
                return;
            }

            if (_restaurantService.NameExistsExceptId(name, SelectedRestaurant.Id))
            {
                StatusMessage = "店家名稱已存在";
                return;
            }

            if (HasBusinessHours)
            {
                var invalidBusinessHours = BusinessHours
                    .Where(x => x.IsOpen && !x.IsTimeRangeValid())
                    .Select(x => x.DayName)
                    .ToList();

                if (invalidBusinessHours.Count > 0)
                {
                    StatusMessage =
                        "以下營業日的開始時間必須早於結束時間："
                        + Environment.NewLine
                        + string.Join(Environment.NewLine, invalidBusinessHours);

                    return;
                }
            }

            int updatedRestaurantId = SelectedRestaurant.Id;

            bool isUpdated = _restaurantService.Update(
                updatedRestaurantId,
                name,
                PreferenceScore,
                HasBusinessHours,
                HasBusinessHours
                    ? BusinessHours.Select(x => x.ToBusinessHour()).ToList()
                    : []
            );

            if (!isUpdated)
            {
                IsEditorEnabled = false;
                LoadRestaurants();
                StatusMessage = "找不到要修改的餐廳，請重新載入資料";
                return;
            }

            LoadRestaurants();

            SelectedRestaurant = Restaurants.FirstOrDefault(x => x.Id == updatedRestaurantId);

            if (SelectedRestaurant != null)
            {
                LoadSelectedRestaurant();
            }

            StatusMessage = "餐廳已修改";
        }
    }
}
