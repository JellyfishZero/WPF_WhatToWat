using System.Windows;
using System.Windows.Controls;
using WhatToEat.Helper;
using WhatToEat.Models;
using WhatToEat.ViewModels;

namespace WhatToEat
{
    /// <summary>
    /// AddItemWindow.xaml 的互動邏輯
    /// </summary>
    public partial class AddRestaurantWindow : Window
    {
        private readonly AddRestaurantVM _addRestaurantVM;

        public AddRestaurantWindow(AddRestaurantVM addRestaurantVM)
        {
            InitializeComponent();
            _addRestaurantVM = addRestaurantVM;
            DataContext = _addRestaurantVM;
        }

        private void OnAddNewRestaurantBtnClicked(object sender, RoutedEventArgs e)
        {
            string name = RestaurantNameTextBox.Text.Trim();
            int preferenceScore = Convert.ToInt32(PreferenceScoreSlider.Value);
            bool hasBusinessHours = HasBusinessHoursCheckBox.IsChecked == true;

            List<Data.BusinessHour> businessHours = hasBusinessHours
                ? CreateBusinessHours()
                : [];

            var result = _addRestaurantVM.AddRestaurant(
                name,
                preferenceScore,
                hasBusinessHours,
                businessHours
            );

            switch (result)
            {
                case AddRestaurantResult.EmptyName:
                    MessageBox.Show("請輸入餐廳名稱");
                    return;

                case AddRestaurantResult.DuplicatedName:
                    MessageBox.Show("店家名稱已存在");
                    return;

                case AddRestaurantResult.Success:
                    MessageBox.Show("餐廳已新增");
                    ClearForm();
                    RestaurantNameTextBox.Focus();
                    return;
            }
        }

        private void ClearForm()
        {
            RestaurantNameTextBox.Clear();
            PreferenceScoreSlider.Value = 3;
            HasBusinessHoursCheckBox.IsChecked = false;
            DefaultStartHourComboBox.SelectedIndex = 9;
            DefaultStartMinuteComboBox.SelectedIndex = 0;
            DefaultEndHourComboBox.SelectedIndex = 21;
            DefaultEndMinuteComboBox.SelectedIndex = 0;

            ClearBusinessHours();
        }

        private void OnApplyWeekdaysBusinessHoursClicked(object sender, RoutedEventArgs e)
        {
            ApplyDefaultBusinessHours(includeWeekend: false);
        }

        private void OnApplyAllBusinessHoursClicked(object sender, RoutedEventArgs e)
        {
            ApplyDefaultBusinessHours(includeWeekend: true);
        }

        private void ApplyDefaultBusinessHours(bool includeWeekend)
        {
            string startHour = (string)DefaultStartHourComboBox.SelectedItem;
            string startMinute = (string)DefaultStartMinuteComboBox.SelectedItem;
            string endHour = (string)DefaultEndHourComboBox.SelectedItem;
            string endMinute = (string)DefaultEndMinuteComboBox.SelectedItem;

            SetBusinessHour(
                MondayOpenCheckBox,
                MondayStartHourComboBox,
                MondayStartMinuteComboBox,
                MondayEndHourComboBox,
                MondayEndMinuteComboBox,
                startHour,
                startMinute,
                endHour,
                endMinute
            );

            SetBusinessHour(
                TuesdayOpenCheckBox,
                TuesdayStartHourComboBox,
                TuesdayStartMinuteComboBox,
                TuesdayEndHourComboBox,
                TuesdayEndMinuteComboBox,
                startHour,
                startMinute,
                endHour,
                endMinute
            );

            SetBusinessHour(
                WednesdayOpenCheckBox,
                WednesdayStartHourComboBox,
                WednesdayStartMinuteComboBox,
                WednesdayEndHourComboBox,
                WednesdayEndMinuteComboBox,
                startHour,
                startMinute,
                endHour,
                endMinute
            );

            SetBusinessHour(
                ThursdayOpenCheckBox,
                ThursdayStartHourComboBox,
                ThursdayStartMinuteComboBox,
                ThursdayEndHourComboBox,
                ThursdayEndMinuteComboBox,
                startHour,
                startMinute,
                endHour,
                endMinute
            );

            SetBusinessHour(
                FridayOpenCheckBox,
                FridayStartHourComboBox,
                FridayStartMinuteComboBox,
                FridayEndHourComboBox,
                FridayEndMinuteComboBox,
                startHour,
                startMinute,
                endHour,
                endMinute
            );

            if (!includeWeekend)
            {
                return;
            }

            SetBusinessHour(
                SaturdayOpenCheckBox,
                SaturdayStartHourComboBox,
                SaturdayStartMinuteComboBox,
                SaturdayEndHourComboBox,
                SaturdayEndMinuteComboBox,
                startHour,
                startMinute,
                endHour,
                endMinute
            );

            SetBusinessHour(
                SundayOpenCheckBox,
                SundayStartHourComboBox,
                SundayStartMinuteComboBox,
                SundayEndHourComboBox,
                SundayEndMinuteComboBox,
                startHour,
                startMinute,
                endHour,
                endMinute
            );
        }

        private static void SetBusinessHour(
            CheckBox openCheckBox,
            ComboBox startHourComboBox,
            ComboBox startMinuteComboBox,
            ComboBox endHourComboBox,
            ComboBox endMinuteComboBox,
            string startHour,
            string startMinute,
            string endHour,
            string endMinute
        )
        {
            openCheckBox.IsChecked = true;
            startHourComboBox.SelectedItem = startHour;
            startMinuteComboBox.SelectedItem = startMinute;
            endHourComboBox.SelectedItem = endHour;
            endMinuteComboBox.SelectedItem = endMinute;
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
