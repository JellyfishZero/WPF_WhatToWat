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
            AddWindow addWindow = new AddWindow();
            addWindow.Show();
        }

        private void ModifyItemClick(object sender, RoutedEventArgs e)
        {
            ModifyWindow modifyWindow = new ModifyWindow();
            modifyWindow.Show();
        }

        private void QueryItemClick(object sender, RoutedEventArgs e)
        {
            QueryWindow queryWindow = new QueryWindow();
            queryWindow.Show();
        }

        private void DeleteItemClick(object sender, RoutedEventArgs e)
        {
            DeleteWindow deleteWindow = new DeleteWindow();
            deleteWindow.Show();
        }
    }
}
