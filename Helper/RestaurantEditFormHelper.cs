using WhatToEat.ViewModels;

namespace WhatToEat.Helper
{
    public static class RestaurantEditFormHelper
    {
        public static List<string> CreateHourItems()
        {
            return Enumerable.Range(0, 24).Select(i => i.ToString("D2")).ToList();
        }

        public static List<string> CreateMinuteItems()
        {
            return ["00", "30"];
        }

        public static List<BusinessHourInputVM> CreateBusinessHours()
        {
            return
            [
                new(DayOfWeek.Monday, "週一"),
                new(DayOfWeek.Tuesday, "週二"),
                new(DayOfWeek.Wednesday, "週三"),
                new(DayOfWeek.Thursday, "週四"),
                new(DayOfWeek.Friday, "週五"),
                new(DayOfWeek.Saturday, "週六"),
                new(DayOfWeek.Sunday, "週日"),
            ];
        }

        public static List<string> GetInvalidBusinessHourDayNames(
            IEnumerable<BusinessHourInputVM> businessHours
        )
        {
            return businessHours
                .Where(x => x.IsOpen && !x.IsTimeRangeValid())
                .Select(x => x.DayName)
                .ToList();
        }

        public static string CreateInvalidBusinessHoursMessage(IEnumerable<string> dayNames)
        {
            return "以下營業日的開始時間必須早於結束時間："
                + Environment.NewLine
                + string.Join(Environment.NewLine, dayNames);
        }

        public static void ApplyDefaultBusinessHours(
            IEnumerable<BusinessHourInputVM> businessHours,
            bool includeWeekend,
            string defaultStartHour,
            string defaultStartMinute,
            string defaultEndHour,
            string defaultEndMinute
        )
        {
            foreach (var businessHour in businessHours)
            {
                bool isWeekend =
                    businessHour.DayOfWeek == DayOfWeek.Saturday
                    || businessHour.DayOfWeek == DayOfWeek.Sunday;

                if (!includeWeekend && isWeekend)
                {
                    continue;
                }

                businessHour.IsOpen = true;
                businessHour.StartHour = defaultStartHour;
                businessHour.StartMinute = defaultStartMinute;
                businessHour.EndHour = defaultEndHour;
                businessHour.EndMinute = defaultEndMinute;
            }
        }

        public static void ResetBusinessHours(IEnumerable<BusinessHourInputVM> businessHours)
        {
            foreach (var businessHour in businessHours)
            {
                businessHour.Reset();
            }
        }
    }
}
