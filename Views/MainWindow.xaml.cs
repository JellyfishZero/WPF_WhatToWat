using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace WhatToEat
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IServiceProvider _services;

        public MainWindow(IServiceProvider services)
        {
            InitializeComponent();
            _services = services;
        }

        private void AddItemClick(object sender, RoutedEventArgs e)
        {
            var window = _services.GetRequiredService<AddRestaurantWindow>();
            window.Owner = this;
            window.Show();
        }

        private void ModifyItemClick(object sender, RoutedEventArgs e)
        {
            var window = _services.GetRequiredService<ModifyRestaurantWindow>();
            window.Owner = this;
            window.Show();
        }

        private void QueryItemClick(object sender, RoutedEventArgs e)
        {
            var window = _services.GetRequiredService<QueryRestaurantWindow>();
            window.Owner = this;
            window.Show();
        }

        private void DeleteItemClick(object sender, RoutedEventArgs e)
        {
            var window = _services.GetRequiredService<DeleteRestaurantWindow>();
            window.Owner = this;
            window.Show();
        }
    }
}
