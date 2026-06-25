using System.Windows.Input;
using WhatToEat.Commands;
using WhatToEat.Data;
using WhatToEat.Models;
using WhatToEat.ViewModels.Common;

namespace WhatToEat.ViewModels.MainWindow
{
    public class MainVM : ViewModelBase
    {
        public ICommand DrawRestaurantCommand { get; }

        private readonly RestaurantService _restaurantService;

        private string _currentTimeText = DateTime.Now.ToString("HH:mm");
        public string CurrentTimeText
        {
            get => _currentTimeText;
            set => SetField(ref _currentTimeText, value);
        }

        private string _statusMessage = "按下抽選後，會依喜好程度作為權重挑選現在可以吃的店。";
        public string StatusMessage
        {
            get => _statusMessage;
            set => SetField(ref _statusMessage, value);
        }

        private string _selectedRestaurantName = "還沒決定";
        public string SelectedRestaurantName
        {
            get => _selectedRestaurantName;
            set => SetField(ref _selectedRestaurantName, value);
        }

        private string _selectedRestaurantNote =
            "資料庫為空時會提醒新增餐廳；沒有營業中的店時會顯示「哭哭，沒有開著的店」。";
        public string SelectedRestaurantNote
        {
            get => _selectedRestaurantNote;
            set => SetField(ref _selectedRestaurantNote, value);
        }

        public MainVM(RestaurantService restaurantService)
        {
            _restaurantService = restaurantService;
            DrawRestaurantCommand = new RelayCommand(DrawRestaurant);
        }

        private void DrawRestaurant()
        {
            var now = DateTime.Now;
            CurrentTimeText = now.ToString("HH:mm");

            var restaurants = _restaurantService.GetAll();
            if (restaurants.Count == 0)
            {
                StatusMessage = "目前還沒有餐廳資料。";
                SelectedRestaurantName = "請先新增餐廳";
                SelectedRestaurantNote = "請從工具 > 新增餐廳 建立第一筆資料。";
                return;
            }

            var openRestaurants = restaurants
                .Where(restaurant => IsRestaurantOpenAt(restaurant, now))
                .ToList();

            if (openRestaurants.Count == 0)
            {
                StatusMessage = "現在沒有營業中的餐廳。";
                SelectedRestaurantName = "哭哭，沒有開著的店";
                SelectedRestaurantNote = "可以晚點再試，或新增其他營業時間的餐廳。";
                return;
            }

            var selectedRestaurant = DrawByPreference(openRestaurants);

            StatusMessage = $"從 {openRestaurants.Count} 間營業中的餐廳抽出結果。";
            SelectedRestaurantName = selectedRestaurant.Name;
            SelectedRestaurantNote = $"喜好程度：{selectedRestaurant.PreferenceScore}";
        }

        private static bool IsRestaurantOpenAt(Restaurant restaurant, DateTime dateTime)
        {
            if (!restaurant.HasBusinessHours)
            {
                return true;
            }

            var businessHour = restaurant.BusinessHours.FirstOrDefault(x =>
                x.DayOfWeek == dateTime.DayOfWeek
            );

            if (businessHour is null || !businessHour.IsOpen)
            {
                return false;
            }

            if (businessHour.OpenTime is null || businessHour.CloseTime is null)
            {
                return false;
            }

            var currentTime = dateTime.TimeOfDay;

            return businessHour.OpenTime <= currentTime && currentTime <= businessHour.CloseTime;
        }

        private static Restaurant DrawByPreference(List<Restaurant> restaurants)
        {
            var totalWeight = restaurants.Sum(x => Math.Max(1, x.PreferenceScore));
            var ticket = Random.Shared.Next(1, totalWeight + 1);

            var currentWeight = 0;

            foreach (var restaurant in restaurants)
            {
                currentWeight += Math.Max(1, restaurant.PreferenceScore);

                if (ticket <= currentWeight)
                {
                    return restaurant;
                }
            }

            return restaurants[^1];
        }
    }
}
