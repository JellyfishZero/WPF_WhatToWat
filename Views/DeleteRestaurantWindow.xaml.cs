using System.Windows;
using WhatToEat.ViewModels;

namespace WhatToEat
{
    /// <summary>
    /// DeleteWindow.xaml 的互動邏輯
    /// </summary>
    public partial class DeleteRestaurantWindow : Window
    {
        private readonly DeleteRestaurantVM _deleteRestaurantVM;

        public DeleteRestaurantWindow(DeleteRestaurantVM deleteRestaurantVM)
        {
            InitializeComponent();
            _deleteRestaurantVM = deleteRestaurantVM;
            DataContext = _deleteRestaurantVM;
            _deleteRestaurantVM.DeleteRestaurantConfirmationRequested +=
                OnDeleteRestaurantConfirmationRequested;
            _deleteRestaurantVM.DeleteRestaurantCompleted += OnDeleteRestaurantCompleted;
        }

        private void OnDeleteRestaurantConfirmationRequested(
            object? sender,
            DeleteRestaurantConfirmationEventArgs e
        )
        {
            var result = MessageBox.Show(
                e.Message,
                e.IsDeleteAll ? "確認刪除全部" : "確認刪除",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning
            );

            e.IsConfirmed = result == MessageBoxResult.Yes;
        }

        private void OnDeleteRestaurantCompleted(
            object? sender,
            DeleteRestaurantCompletedEventArgs e
        )
        {
            MessageBox.Show(e.Message);
        }
    }
}
