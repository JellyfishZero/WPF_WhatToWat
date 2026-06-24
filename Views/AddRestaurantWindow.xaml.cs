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
using WhatToEat.Helper;

namespace WhatToEat
{
    /// <summary>
    /// AddItemWindow.xaml 的互動邏輯
    /// </summary>
    public partial class AddRestaurantWindow : Window
    {
        public AddRestaurantWindow()
        {
            InitializeComponent();
        }

        private void OnAddNewRestaurantBtnClicked(object sender, RoutedEventArgs e)
        {
            string name = RestaurantNameTextBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("請輸入餐廳名稱");
                return;
            }

            int preferenceScore = Convert.ToInt32(PreferenceScoreSlider.Value);

            var restaurant = new Data.Restaurant
            {
                Name = name,
                PreferenceScore = preferenceScore,
                HasBusinessHours = HasBusinessHoursCheckBox.IsChecked == true,
            };

            using var db = new Data.AppDbContext();

            bool nameExists = db.Restaurants.Any(r => r.Name == name);

            if (nameExists)
            {
                MessageBox.Show("店家名稱已存在");
                return;
            }

            db.Restaurants.Add(restaurant);
            if (restaurant.HasBusinessHours)
            {
                restaurant.BusinessHours.AddRange(CreateBusinessHours());
            }
            db.SaveChanges();

            MessageBox.Show("餐廳已新增");
            ClearForm();
            RestaurantNameTextBox.Focus();
        }

        private void ClearForm()
        {
            RestaurantNameTextBox.Clear();
            PreferenceScoreSlider.Value = 3;
            HasBusinessHoursCheckBox.IsChecked = false;

            ClearBusinessHours();
        }

        private Data.BusinessHour CreateBusinessHour(
            DayOfWeek dayOfWeek,
            CheckBox openCheckBox,
            ComboBox startHourComboBox,
            ComboBox startMinuteComboBox,
            ComboBox endHourComboBox,
            ComboBox endMinuteComboBox
        )
        {
            bool isOpen = openCheckBox.IsChecked == true;

            return new Data.BusinessHour
            {
                DayOfWeek = dayOfWeek,
                IsOpen = isOpen,
                OpenTime = isOpen
                    ? TimeHelper.GetTime(startHourComboBox, startMinuteComboBox)
                    : null,
                CloseTime = isOpen ? TimeHelper.GetTime(endHourComboBox, endMinuteComboBox) : null,
            };
        }

        private List<Data.BusinessHour> CreateBusinessHours()
        {
            return
            [
                CreateBusinessHour(
                    DayOfWeek.Monday,
                    MondayOpenCheckBox,
                    MondayStartHourComboBox,
                    MondayStartMinuteComboBox,
                    MondayEndHourComboBox,
                    MondayEndMinuteComboBox
                ),
                CreateBusinessHour(
                    DayOfWeek.Tuesday,
                    TuesdayOpenCheckBox,
                    TuesdayStartHourComboBox,
                    TuesdayStartMinuteComboBox,
                    TuesdayEndHourComboBox,
                    TuesdayEndMinuteComboBox
                ),
                CreateBusinessHour(
                    DayOfWeek.Wednesday,
                    WednesdayOpenCheckBox,
                    WednesdayStartHourComboBox,
                    WednesdayStartMinuteComboBox,
                    WednesdayEndHourComboBox,
                    WednesdayEndMinuteComboBox
                ),
                CreateBusinessHour(
                    DayOfWeek.Thursday,
                    ThursdayOpenCheckBox,
                    ThursdayStartHourComboBox,
                    ThursdayStartMinuteComboBox,
                    ThursdayEndHourComboBox,
                    ThursdayEndMinuteComboBox
                ),
                CreateBusinessHour(
                    DayOfWeek.Friday,
                    FridayOpenCheckBox,
                    FridayStartHourComboBox,
                    FridayStartMinuteComboBox,
                    FridayEndHourComboBox,
                    FridayEndMinuteComboBox
                ),
                CreateBusinessHour(
                    DayOfWeek.Saturday,
                    SaturdayOpenCheckBox,
                    SaturdayStartHourComboBox,
                    SaturdayStartMinuteComboBox,
                    SaturdayEndHourComboBox,
                    SaturdayEndMinuteComboBox
                ),
                CreateBusinessHour(
                    DayOfWeek.Sunday,
                    SundayOpenCheckBox,
                    SundayStartHourComboBox,
                    SundayStartMinuteComboBox,
                    SundayEndHourComboBox,
                    SundayEndMinuteComboBox
                ),
            ];
        }

        private void ClearBusinessHours()
        {
            ResetBusinessHour(
                MondayOpenCheckBox,
                MondayStartHourComboBox,
                MondayStartMinuteComboBox,
                MondayEndHourComboBox,
                MondayEndMinuteComboBox
            );

            ResetBusinessHour(
                TuesdayOpenCheckBox,
                TuesdayStartHourComboBox,
                TuesdayStartMinuteComboBox,
                TuesdayEndHourComboBox,
                TuesdayEndMinuteComboBox
            );

            ResetBusinessHour(
                WednesdayOpenCheckBox,
                WednesdayStartHourComboBox,
                WednesdayStartMinuteComboBox,
                WednesdayEndHourComboBox,
                WednesdayEndMinuteComboBox
            );

            ResetBusinessHour(
                ThursdayOpenCheckBox,
                ThursdayStartHourComboBox,
                ThursdayStartMinuteComboBox,
                ThursdayEndHourComboBox,
                ThursdayEndMinuteComboBox
            );

            ResetBusinessHour(
                FridayOpenCheckBox,
                FridayStartHourComboBox,
                FridayStartMinuteComboBox,
                FridayEndHourComboBox,
                FridayEndMinuteComboBox
            );

            ResetBusinessHour(
                SaturdayOpenCheckBox,
                SaturdayStartHourComboBox,
                SaturdayStartMinuteComboBox,
                SaturdayEndHourComboBox,
                SaturdayEndMinuteComboBox
            );

            ResetBusinessHour(
                SundayOpenCheckBox,
                SundayStartHourComboBox,
                SundayStartMinuteComboBox,
                SundayEndHourComboBox,
                SundayEndMinuteComboBox
            );
        }

        private void ResetBusinessHour(
            CheckBox openCheckBox,
            ComboBox startHourComboBox,
            ComboBox startMinuteComboBox,
            ComboBox endHourComboBox,
            ComboBox endMinuteComboBox
        )
        {
            openCheckBox.IsChecked = false;
            startHourComboBox.SelectedIndex = 0;
            startMinuteComboBox.SelectedIndex = 0;
            endHourComboBox.SelectedIndex = 0;
            endMinuteComboBox.SelectedIndex = 0;
        }
    }
}
