using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using WhatToEat.ViewModels.Restaurants;

namespace WhatToEat
{
    public partial class QueryRestaurantWindow : Window
    {
        private readonly QueryRestaurantVM _queryRestaurantVM;

        public QueryRestaurantWindow(QueryRestaurantVM queryRestaurantVM)
        {
            InitializeComponent();
            _queryRestaurantVM = queryRestaurantVM;
            DataContext = _queryRestaurantVM;

            // 載入資料
            _queryRestaurantVM.LoadRestaurants();
        }

        #region 營業時間 DataGrid 捲動優化
        /// <summary>
        /// 使營業時間子表不會消耗父表的滑鼠滾輪事件，避免滑鼠滾輪在子表上時，父表不會跟著滾動。
        /// </summary>
        private void OnBusinessHoursDataGridPreviewMouseWheel(
            object sender,
            MouseWheelEventArgs e
        )
        {
            e.Handled = true;

            // 找到父層的 ScrollViewer，並將滾動事件傳遞給它
            var scrollViewer = FindVisualChild<ScrollViewer>(RestaurantDataGrid);
            if (scrollViewer is null)
            {
                return;
            }

            scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset - e.Delta);
        }

        /// <summary>
        /// 這個方法是用來在視覺樹中尋找指定類型的子元素。
        /// 它會遞迴地遍歷所有子元素，直到找到第一個符合條件的子元素為止。
        /// </summary>
        private static T? FindVisualChild<T>(DependencyObject parent)
            where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);

                if (child is T target)
                {
                    return target;
                }

                var descendant = FindVisualChild<T>(child);
                if (descendant is not null)
                {
                    return descendant;
                }
            }

            return null;
        }
        #endregion
    }
}

