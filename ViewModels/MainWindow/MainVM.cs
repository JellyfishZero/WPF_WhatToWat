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

            var currentTime = dateTime.TimeOfDay;

            var todayBusinessHour = restaurant.BusinessHours.FirstOrDefault(x =>
                x.DayOfWeek == dateTime.DayOfWeek
            );

            // 先檢查今天設定的營業時間是否包含現在。
            // 同日營業例如 10:00 ~ 21:00；跨日營業例如 22:00 ~ 02:00 的晚間時段。
            if (
                todayBusinessHour is not null
                && IsBusinessHourOpenAt(todayBusinessHour, currentTime)
            )
            {
                return true;
            }

            var yesterday = GetPreviousDay(dateTime.DayOfWeek);
            var yesterdayBusinessHour = restaurant.BusinessHours.FirstOrDefault(x =>
                x.DayOfWeek == yesterday
            );

            // 再檢查昨天是否有跨日營業延伸到今天凌晨。
            // 例如週一 22:00 ~ 02:00，週二 01:00 仍應算作營業中。
            return yesterdayBusinessHour is not null
                && IsPreviousDayOvernightBusinessHourOpenAt(yesterdayBusinessHour, currentTime);
        }

        private static bool IsBusinessHourOpenAt(BusinessHour businessHour, TimeSpan currentTime)
        {
            if (!businessHour.IsOpen)
            {
                return false;
            }

            if (businessHour.OpenTime is null || businessHour.CloseTime is null)
            {
                return false;
            }

            var openTime = businessHour.OpenTime.Value;
            var closeTime = businessHour.CloseTime.Value;

            if (openTime < closeTime)
            {
                // 同日區間：現在時間必須被開始與結束時間夾住。
                return openTime <= currentTime && currentTime < closeTime;
            }

            // 跨日區間：今天只負責開始時間到午夜，隔天凌晨由昨天的營業時間判斷。
            return currentTime >= openTime;
        }

        private static bool IsPreviousDayOvernightBusinessHourOpenAt(
            BusinessHour businessHour,
            TimeSpan currentTime
        )
        {
            if (!businessHour.IsOpen)
            {
                return false;
            }

            if (businessHour.OpenTime is null || businessHour.CloseTime is null)
            {
                return false;
            }

            return businessHour.OpenTime > businessHour.CloseTime
                && currentTime < businessHour.CloseTime.Value;
        }

        private static DayOfWeek GetPreviousDay(DayOfWeek dayOfWeek)
        {
            return dayOfWeek == DayOfWeek.Sunday ? DayOfWeek.Saturday : dayOfWeek - 1;
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
