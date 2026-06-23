using System.Configuration;
using System.Data;
using System.Windows;

namespace WhatToEat
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(System.Windows.StartupEventArgs e)
        {
            base.OnStartup(e);

            using var db = new Data.AppDbContext();
            db.Database.EnsureCreated();
        }
    }
}
