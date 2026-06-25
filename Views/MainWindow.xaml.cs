using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using WhatToEat.ViewModels.MainWindow;

namespace WhatToEat
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IServiceProvider _services;

        private readonly Dictionary<Type, Window> _openedWindows = new();

        public MainWindow(IServiceProvider services, MainVM mainVM)
        {
            InitializeComponent();
            _services = services;
            DataContext = mainVM;
        }

        private void AddItemClick(object sender, RoutedEventArgs e)
        {
            ShowWindow<AddRestaurantWindow>();
        }

        private void ModifyItemClick(object sender, RoutedEventArgs e)
        {
            ShowWindow<ModifyRestaurantWindow>();
        }

        private void QueryItemClick(object sender, RoutedEventArgs e)
        {
            ShowWindow<QueryRestaurantWindow>();
        }

        private void DeleteItemClick(object sender, RoutedEventArgs e)
        {
            ShowWindow<DeleteRestaurantWindow>();
        }

        private void ShowWindow<TWindow>()
            where TWindow : Window
        {
            var windowType = typeof(TWindow);

            if (_openedWindows.TryGetValue(windowType, out var existingWindow))
            {
                if (existingWindow.WindowState == WindowState.Minimized)
                {
                    existingWindow.WindowState = WindowState.Normal;
                }

                existingWindow.Activate();
                return;
            }

            var window = _services.GetRequiredService<TWindow>();
            window.Owner = this;

            window.Closed += (_, _) =>
            {
                _openedWindows.Remove(windowType);

                if (!IsVisible)
                {
                    return;
                }

                if (WindowState == WindowState.Minimized)
                {
                    WindowState = WindowState.Normal;
                }

                Activate();
                Focus();
            };

            _openedWindows[windowType] = window;
            window.Show();
        }
    }
}
