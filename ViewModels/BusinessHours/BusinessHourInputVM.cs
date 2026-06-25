using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using WhatToEat.Data;
using WhatToEat.ViewModels.Common;

namespace WhatToEat.ViewModels.BusinessHours
{
    public class BusinessHourInputVM : ViewModelBase
    {
        private bool _isOpen;
        private string _startHour = "00";
        private string _startMinute = "00";
        private string _endHour = "00";
        private string _endMinute = "00";

        public DayOfWeek DayOfWeek { get; }
        public string DayName { get; }

        public bool IsOpen
        {
            get => _isOpen;
            set => SetField(ref _isOpen, value);
        }

        public string StartHour
        {
            get => _startHour;
            set => SetField(ref _startHour, value);
        }

        public string StartMinute
        {
            get => _startMinute;
            set => SetField(ref _startMinute, value);
        }

        public string EndHour
        {
            get => _endHour;
            set => SetField(ref _endHour, value);
        }

        public string EndMinute
        {
            get => _endMinute;
            set => SetField(ref _endMinute, value);
        }

        public BusinessHourInputVM(DayOfWeek dayOfWeek, string dayName)
        {
            DayOfWeek = dayOfWeek;
            DayName = dayName;
        }

        public void Reset()
        {
            IsOpen = false;
            StartHour = "00";
            StartMinute = "00";
            EndHour = "00";
            EndMinute = "00";
        }

        public bool IsTimeRangeValid()
        {
            return GetStartTime() < GetEndTime();
        }

        public BusinessHour ToBusinessHour()
        {
            return new BusinessHour
            {
                DayOfWeek = DayOfWeek,
                IsOpen = IsOpen,
                OpenTime = IsOpen ? GetStartTime() : null,
                CloseTime = IsOpen ? GetEndTime() : null,
            };
        }

        private TimeSpan GetStartTime()
        {
            return new TimeSpan(int.Parse(StartHour), int.Parse(StartMinute), 0);
        }

        private TimeSpan GetEndTime()
        {
            return new TimeSpan(int.Parse(EndHour), int.Parse(EndMinute), 0);
        }
    }
}
