using System.Windows;
using System.Windows.Controls;
using WhatToEat.Helper;
using WhatToEat.Models;
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
                    MessageBox.Show("請輸入餐廳名稱");
                    return;

                case AddRestaurantResult.DuplicatedName:
                    MessageBox.Show("店家名稱已存在");
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

            DefaultStartHourComboBox.SelectedIndex = 9;
            DefaultStartMinuteComboBox.SelectedIndex = 0;
            DefaultEndHourComboBox.SelectedIndex = 21;
            DefaultEndMinuteComboBox.SelectedIndex = 0;
        }

        private void OnApplyWeekdaysBusinessHoursClicked(object sender, RoutedEventArgs e)
        {
            ApplyDefaultBusinessHours(includeWeekend: false);
        }

        private void OnApplyAllBusinessHoursClicked(object sender, RoutedEventArgs e)
        {
            ApplyDefaultBusinessHours(includeWeekend: true);
        }

        private void ApplyDefaultBusinessHours(bool includeWeekend)
        {
            _addRestaurantVM.ApplyDefaultBusinessHours(
                includeWeekend,
                (string)DefaultStartHourComboBox.SelectedItem,
                (string)DefaultStartMinuteComboBox.SelectedItem,
                (string)DefaultEndHourComboBox.SelectedItem,
                (string)DefaultEndMinuteComboBox.SelectedItem
            );
        }
    }
}
