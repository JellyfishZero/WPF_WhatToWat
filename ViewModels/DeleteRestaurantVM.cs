using System.Collections.ObjectModel;
using System.Windows.Input;
using WhatToEat.Commands;
using WhatToEat.Data;
using WhatToEat.Models;

namespace WhatToEat.ViewModels
{
    public enum DeleteRestaurantResult
    {
        Success,
        EmptySelection,
        NoRestaurants,
    }

    /// <summary>
    /// 刪除完成事件的事件參數，包含刪除結果以及訊息。
    /// </summary>
    public class DeleteRestaurantCompletedEventArgs : EventArgs
    {
        public DeleteRestaurantCompletedEventArgs(DeleteRestaurantResult result, string message)
        {
            Result = result;
            Message = message;
        }

        public DeleteRestaurantResult Result { get; }
        public string Message { get; }
    }

    /// <summary>
    /// 確認事件的事件參數，包含訊息、是否刪除全部以及使用者是否確認刪除。
    /// </summary>
    public class DeleteRestaurantConfirmationEventArgs : EventArgs
    {
        public DeleteRestaurantConfirmationEventArgs(string message, bool isDeleteAll)
        {
            Message = message;
            IsDeleteAll = isDeleteAll;
        }

        public string Message { get; }
        public bool IsDeleteAll { get; }
        public bool IsConfirmed { get; set; }
    }

    public class DeleteRestaurantVM : ViewModelBase
    {
        public event EventHandler<DeleteRestaurantConfirmationEventArgs>? DeleteRestaurantConfirmationRequested;
        public event EventHandler<DeleteRestaurantCompletedEventArgs>? DeleteRestaurantCompleted;

        public ICommand DeleteSelectedRestaurantCommand { get; }
        public ICommand DeleteAllRestaurantsCommand { get; }

        private readonly RestaurantService _restaurantService;

        public DeleteRestaurantVM(RestaurantService restaurantService)
        {
            _restaurantService = restaurantService;
            DeleteSelectedRestaurantCommand = new RelayCommand(RequestDeleteSelectedRestaurant);
            DeleteAllRestaurantsCommand = new RelayCommand(RequestDeleteAllRestaurants);

            LoadRestaurants();
        }

        public ObservableCollection<Restaurant> Restaurants { get; } = [];

        private Restaurant? _selectedRestaurant = null;

        public Restaurant? SelectedRestaurant
        {
            get => _selectedRestaurant;
            set => SetField(ref _selectedRestaurant, value);
        }

        private string _statusMessage = "";

        public string StatusMessage
        {
            get => _statusMessage;
            private set => SetField(ref _statusMessage, value);
        }

        private void RequestDeleteSelectedRestaurant()
        {
            if (SelectedRestaurant == null)
            {
                RaiseCompleted(DeleteRestaurantResult.EmptySelection, "請先選擇要刪除的餐廳");
                return;
            }

            var args = new DeleteRestaurantConfirmationEventArgs(
                $"確定要刪除「{SelectedRestaurant.Name}」嗎？",
                false
            );

            DeleteRestaurantConfirmationRequested?.Invoke(this, args);

            if (!args.IsConfirmed)
            {
                return;
            }

            DeleteSelectedRestaurant();
        }

        private void RaiseCompleted(DeleteRestaurantResult result, string message)
        {
            DeleteRestaurantCompleted?.Invoke(
                this,
                new DeleteRestaurantCompletedEventArgs(result, message)
            );
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

        private void DeleteSelectedRestaurant()
        {
            if (SelectedRestaurant == null)
            {
                return;
            }

            _restaurantService.Delete(SelectedRestaurant);
            LoadRestaurants();

            RaiseCompleted(DeleteRestaurantResult.Success, "餐廳已刪除");
        }

        private void RequestDeleteAllRestaurants()
        {
            if (Restaurants.Count == 0)
            {
                RaiseCompleted(DeleteRestaurantResult.NoRestaurants, "目前沒有餐廳可以刪除");
                return;
            }

            var args = new DeleteRestaurantConfirmationEventArgs(
                "確定要刪除全部餐廳嗎？此操作無法直接復原。",
                true
            );

            DeleteRestaurantConfirmationRequested?.Invoke(this, args);

            if (!args.IsConfirmed)
            {
                return;
            }

            DeleteAllRestaurants();
        }

        private void DeleteAllRestaurants()
        {
            _restaurantService.DeleteAll();
            LoadRestaurants();

            RaiseCompleted(DeleteRestaurantResult.Success, "全部餐廳已刪除");
        }
    }
}
