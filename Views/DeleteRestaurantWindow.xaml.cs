using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WhatToEat
{
    /// <summary>
    /// DeleteWindow.xaml 的互動邏輯
    /// </summary>
    public partial class DeleteRestaurantWindow : Window
    {
        public DeleteRestaurantWindow()
        {
            InitializeComponent();
        }

        private void OnDeleteRestaurantButtonClick(object sender, RoutedEventArgs e)
        {
            if (RestaurantComboBox.SelectedItem == null)
            {
                MessageBox.Show(
                    "請先選擇要刪除的餐廳。",
                    "尚未選擇",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
                return;
            }

            var confirmResult = MessageBox.Show(
                "刪除後資料將無法直接復原，請確認是否要繼續。",
                "確認刪除",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (confirmResult != MessageBoxResult.Yes)
            {
                return;
            }
        }

        private void OnDeleteAllRestaurantsButtonClick(object sender, RoutedEventArgs e)
        {
            var confirmResult = MessageBox.Show(
                "此操作會刪除全部餐廳資料，刪除後資料將無法直接復原，請確認是否要繼續。",
                "確認刪除全部",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (confirmResult != MessageBoxResult.Yes)
            {
                return;
            }
        }
    }
}
