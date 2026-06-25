using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WhatToEat.ViewModels;

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
    }
}
