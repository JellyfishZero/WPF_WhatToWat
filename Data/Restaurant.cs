using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatToEat.Data
{
    /// <summary>
    /// 餐廳資料格式
    /// </summary>
    public class Restaurant
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 名稱
        /// </summary>
        public string Name { get; set; } = "";

        /// <summary>
        /// 偏好分數
        /// </summary>
        public int PreferenceScore { get; set; }

        /// <summary>
        /// 需要紀錄營業時間；不紀錄則視為一週七天全天營業
        /// </summary>
        public bool HasBusinessHours { get; set; }

        public List<BusinessHour> BusinessHours { get; set; } = [];
    }

    /// <summary>
    /// 營業時間資料格式
    /// </summary>
    public class BusinessHour
    {
        public int Id { get; set; }
        public int RestaurantId { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
        public bool IsOpen { get; set; }
        public TimeSpan? OpenTime { get; set; }
        public TimeSpan? CloseTime { get; set; }
    }
}
