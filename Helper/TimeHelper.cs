namespace WhatToEat.Helper
{
    public static class TimeHelper
    {
        public static string FormatTime(TimeSpan time)
        {
            return $"{time.Hours:00}:{time.Minutes:00}";
        }

        public static string FormatTime(TimeSpan? time)
        {
            return time is null ? "-" : FormatTime(time.Value);
        }

        public static string FormatTimeRange(TimeSpan? openTime, TimeSpan? closeTime)
        {
            if (openTime is null || closeTime is null)
            {
                return "-";
            }

            return $"{FormatTime(openTime.Value)} ~ {FormatTime(closeTime.Value)}";
        }

        public static string ToDayName(DayOfWeek dayOfWeek)
        {
            return dayOfWeek switch
            {
                DayOfWeek.Monday => "星期一",
                DayOfWeek.Tuesday => "星期二",
                DayOfWeek.Wednesday => "星期三",
                DayOfWeek.Thursday => "星期四",
                DayOfWeek.Friday => "星期五",
                DayOfWeek.Saturday => "星期六",
                DayOfWeek.Sunday => "星期日",
                _ => ""
            };
        }
    }
}
