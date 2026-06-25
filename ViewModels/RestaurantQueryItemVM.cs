using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatToEat.ViewModels
{
    public class RestaurantQueryItemVM
    {
        public string Name { get; set; } = "";
        public int PreferenceScore { get; set; }
        public string BusinessHoursSettingText { get; set; } = "";
        public string BusinessHoursSummary { get; set; } = "";
        public string BusinessHoursNote { get; set; } = "";
        public bool ShowBusinessHoursDetails { get; set; }
        public ObservableCollection<BusinessHourQueryItemVM> BusinessHours { get; } = [];
    }
}
