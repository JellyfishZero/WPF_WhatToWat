using System.Configuration;
using System.Data;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WhatToEat.Data;
using WhatToEat.Models;
using WhatToEat.ViewModels;

namespace WhatToEat
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IServiceProvider Services { get; private set; } = null!;

        protected override void OnStartup(System.Windows.StartupEventArgs e)
        {
            base.OnStartup(e);

            Services = new ServiceCollection()
                // db
                .AddTransient<AppDbContext>()
                // model
                .AddTransient<RestaurantService>()
                // view model
                .AddTransient<AddRestaurantVM>()
                // view
                .AddTransient<MainWindow>()
                .AddTransient<AddRestaurantWindow>()
                .AddTransient<ModifyRestaurantWindow>()
                .AddTransient<DeleteRestaurantWindow>()
                .AddTransient<QueryRestaurantWindow>()
                .BuildServiceProvider();

            // 連接或產生SQLite DB
            using var db = Services.GetRequiredService<AppDbContext>();
            db.Database.Migrate();

            var mainWindow = Services.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }
    }
}
