using System.Windows;
using WhatToEat.ViewModels.Restaurants;

namespace WhatToEat
{
    /// <summary>
    /// ModifyWindow.xaml 的互動邏輯
    /// </summary>
    public partial class ModifyRestaurantWindow : Window
    {
        private readonly ModifyRestaurantVM _modifyRestaurantVM;

        public ModifyRestaurantWindow(ModifyRestaurantVM modifyRestaurantVM)
        {
            InitializeComponent();
            _modifyRestaurantVM = modifyRestaurantVM;
            DataContext = _modifyRestaurantVM;
            _modifyRestaurantVM.ModifyRestaurantCompleted += OnModifyRestaurantCompleted;
        }

        private void OnModifyRestaurantCompleted(
            object? sender,
            ModifyRestaurantCompletedEventArgs e
        )
        {
            MessageBox.Show(this, e.Message);
        }
    }
}

