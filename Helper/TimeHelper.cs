using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WhatToEat.Helper
{
    public static class TimeHelper
    {
        public static TimeSpan GetTime(ComboBox hourComboBox, ComboBox minuteComboBox)
        {
            int hour = int.Parse((string)hourComboBox.SelectedItem);
            int minute = int.Parse((string)minuteComboBox.SelectedItem);

            return new TimeSpan(hour, minute, 0);
        }
    }
}
