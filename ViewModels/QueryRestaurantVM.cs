using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WhatToEat.Commands;
using WhatToEat.Data;
using WhatToEat.Helper;
using WhatToEat.Models;

namespace WhatToEat.ViewModels
{
    public class QueryRestaurantVM : ViewModelBase
    {
        private readonly RestaurantService _restaurantService;

        public ICommand RefreshCommand { get; }

        public QueryRestaurantVM(RestaurantService restaurantService)
        {
            _restaurantService = restaurantService;
            RefreshCommand = new RelayCommand(LoadRestaurants);
        }

        public ObservableCollection<RestaurantQueryItemVM> Restaurants { get; } = [];

        private RestaurantQueryItemVM? _selectedRestaurant;

        public RestaurantQueryItemVM? SelectedRestaurant
        {
            get => _selectedRestaurant;
            set
            {
                if (SetField(ref _selectedRestaurant, value))
                {
                    OnPropertyChanged(nameof(SelectedRestaurantHint));
                }
            }
        }

        public string RestaurantCountText => $"共 {Restaurants.Count} 筆餐廳";

        public string StatusMessage =>
            Restaurants.Count == 0 ? "目前沒有餐廳資料" : "點選餐廳可查看詳細營業時間";

        public string SelectedRestaurantHint =>
            SelectedRestaurant is null ? "尚未選取餐廳" : $"目前選取：{SelectedRestaurant.Name}";

        public void LoadRestaurants()
        {
            SelectedRestaurant = null;
            Restaurants.Clear();

            var restaurants = _restaurantService.GetAll();

            foreach (var restaurant in restaurants)
            {
                Restaurants.Add(ToQueryItem(restaurant));
            }

            OnPropertyChanged(nameof(RestaurantCountText));
            OnPropertyChanged(nameof(StatusMessage));
        }

        private RestaurantQueryItemVM ToQueryItem(Restaurant restaurant)
        {
            var item = new RestaurantQueryItemVM
            {
                Id = restaurant.Id,
                Name = restaurant.Name,
                PreferenceScore = restaurant.PreferenceScore,
                BusinessHoursSettingText = restaurant.HasBusinessHours ? "有設定" : "每日全天營業",
                BusinessHoursSummary = restaurant.HasBusinessHours
                    ? "點選查看詳細營業時間"
                    : "每日全天營業",
                BusinessHoursNote = restaurant.HasBusinessHours
                    ? "以下為此餐廳設定的營業時間"
                    : "此餐廳未設定營業時間，視為每日全天營業",
                ShowBusinessHoursDetails = restaurant.HasBusinessHours,
            };

            if (restaurant.HasBusinessHours)
            {
                foreach (var businessHour in restaurant.BusinessHours.OrderBy(b => b.DayOfWeek))
                {
                    item.BusinessHours.Add(
                        new BusinessHourQueryItemVM
                        {
                            DayName = TimeHelper.ToDayName(businessHour.DayOfWeek),
                            OpenStatusText = businessHour.IsOpen ? "營業" : "公休",
                            TimeRangeText = businessHour.IsOpen
                                ? TimeHelper.FormatTimeRange(
                                    businessHour.OpenTime,
                                    businessHour.CloseTime
                                )
                                : "-",
                        }
                    );
                }
            }

            return item;
        }
    }
}
