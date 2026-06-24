using System.Windows;

namespace WhatToEat
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly AddRestaurantWindow _addWindow;
        private readonly ModifyRestaurantWindow _modifyWindow;
        private readonly QueryRestaurantWindow _queryWindow;
        private readonly DeleteRestaurantWindow _deleteWindow;

        public MainWindow(AddRestaurantWindow addWindow, ModifyRestaurantWindow modifyWindow, QueryRestaurantWindow queryWindow, DeleteRestaurantWindow deleteWindow)
        {
            InitializeComponent();
            _addWindow = addWindow;
            _modifyWindow = modifyWindow;
            _queryWindow = queryWindow;
            _deleteWindow = deleteWindow;
        }

        private void AddItemClick(object sender, RoutedEventArgs e)
        {
            _addWindow.Show();
        }

        private void ModifyItemClick(object sender, RoutedEventArgs e)
        {
            _modifyWindow.Show();
        }

        private void QueryItemClick(object sender, RoutedEventArgs e)
        {
            _queryWindow.Show();
        }

        private void DeleteItemClick(object sender, RoutedEventArgs e)
        {
            _deleteWindow.Show();
        }
    }
}
