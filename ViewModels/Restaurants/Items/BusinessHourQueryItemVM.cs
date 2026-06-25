using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatToEat.ViewModels.Restaurants.Items
{
    public class BusinessHourQueryItemVM
    {
        public int DaySortOrder { get; set; }
        public string DayName { get; set; } = "";
        public string OpenStatusText { get; set; } = "";
        public string TimeRangeText { get; set; } = "";
    }
}
