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
                .AddDbContextFactory<AppDbContext>(options =>
                    options.UseSqlite($"Data Source={AppDbContext.GetDatabasePath()}")
                )
                // model
                .AddTransient<RestaurantService>()
                // view model
                .AddTransient<AddRestaurantVM>()
                .AddTransient<QueryRestaurantVM>()
                .AddTransient<DeleteRestaurantVM>()
                .AddTransient<ModifyRestaurantVM>()
                // view
                .AddTransient<MainWindow>()
                .AddTransient<AddRestaurantWindow>()
                .AddTransient<ModifyRestaurantWindow>()
                .AddTransient<DeleteRestaurantWindow>()
                .AddTransient<QueryRestaurantWindow>()
                .BuildServiceProvider(
                    new ServiceProviderOptions
                    {
                        ValidateOnBuild = true,  // 檢查使否忘記註冊
                        ValidateScopes = true, // 取用範圍檢查
                    }
                );

            // Connect to or create the SQLite DB.
            var dbFactory = Services.GetRequiredService<IDbContextFactory<AppDbContext>>();
            using var db = dbFactory.CreateDbContext();
            db.Database.Migrate();

            // 開啟主要視窗
            var mainWindow = Services.GetRequiredService<MainWindow>();
            Current.MainWindow = mainWindow;
            ShutdownMode = ShutdownMode.OnMainWindowClose;
            mainWindow.Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            // 確保DI容器所有需要釋放的東西都會正確釋放
            if (Services is IDisposable disposable)
            {
                disposable.Dispose();
            }

            base.OnExit(e);
        }
    }
}
