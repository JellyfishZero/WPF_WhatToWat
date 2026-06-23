using System.Windows;

namespace WhatToEat
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void AddItemClick(object sender, RoutedEventArgs e)
        {
            AddRestaurantWindow addWindow = new AddRestaurantWindow();
            addWindow.Show();
        }

        private void ModifyItemClick(object sender, RoutedEventArgs e)
        {
            ModifyRestaurantWindow modifyWindow = new ModifyRestaurantWindow();
            modifyWindow.Show();
        }

        private void QueryItemClick(object sender, RoutedEventArgs e)
        {
            QueryRestaurantWindow queryWindow = new QueryRestaurantWindow();
            queryWindow.Show();
        }

        private void DeleteItemClick(object sender, RoutedEventArgs e)
        {
            DeleteRestaurantWindow deleteWindow = new DeleteRestaurantWindow();
            deleteWindow.Show();
        }
    }
}
