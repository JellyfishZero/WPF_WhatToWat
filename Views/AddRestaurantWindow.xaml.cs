using System.Windows;
using WhatToEat.ViewModels.Restaurants;

namespace WhatToEat
{
    /// <summary>
    /// AddItemWindow.xaml 的互動邏輯
    /// </summary>
    public partial class AddRestaurantWindow : Window
    {
        private readonly AddRestaurantVM _addRestaurantVM;

        public AddRestaurantWindow(AddRestaurantVM addRestaurantVM)
        {
            InitializeComponent();
            _addRestaurantVM = addRestaurantVM;
            DataContext = _addRestaurantVM;
            _addRestaurantVM.AddRestaurantCompleted += OnAddRestaurantCompleted;
        }

        private void OnAddRestaurantCompleted(object? sender, AddRestaurantCompletedEventArgs e)
        {
            MessageBox.Show(e.Message);

            if (e.Result == AddRestaurantResult.Success)
            {
                ClearForm();
                RestaurantNameTextBox.Focus();
            }
        }

        private void ClearForm()
        {
            _addRestaurantVM.Reset();
        }
    }
}
