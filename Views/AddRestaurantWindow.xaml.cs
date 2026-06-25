using System.Windows;
using WhatToEat.ViewModels;

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
        }

        private void OnAddNewRestaurantBtnClicked(object sender, RoutedEventArgs e)
        {
            var result = _addRestaurantVM.AddRestaurant();

            switch (result)
            {
                case AddRestaurantResult.EmptyName:
                case AddRestaurantResult.DuplicatedName:
                case AddRestaurantResult.InvalidBusinessHours:
                    MessageBox.Show(_addRestaurantVM.ErrorMessage);
                    return;

                case AddRestaurantResult.Success:
                    MessageBox.Show("餐廳已新增");
                    ClearForm();
                    RestaurantNameTextBox.Focus();
                    return;
            }
        }

        private void ClearForm()
        {
            _addRestaurantVM.Reset();
        }
    }
}
