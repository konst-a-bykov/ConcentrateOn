using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ConcentrateOn
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SettingsPage : Page
    {
        public ObservableCollection<ListOfItims> IntValues { get; set; }
        ConcentrateOnSettings concentrateOnSettings;
        App app;
        Core core;

        public SettingsPage()
        {
            this.InitializeComponent();
            IntValues = new ObservableCollection<ListOfItims>();
            for (int i = 1; i <= 100; i++)
                IntValues.Add(new ListOfItims() { IntVal = i, StrVal = i.ToString() });
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            SetFlowDirection();
            app = (App)e.Parameter;
            concentrateOnSettings = app.core.concentrateOnSettings;
            core = app.core;
            ReflectSettings();
        }


        private void SetFlowDirection()
        {
            var flowDirectionSetting = Windows.ApplicationModel.Resources.Core.ResourceContext.GetForCurrentView().QualifierValues["LayoutDirection"];
            if (flowDirectionSetting == "LTR")
            {
                this.FlowDirection = Windows.UI.Xaml.FlowDirection.LeftToRight;
            }
            else
            {
                this.FlowDirection = Windows.UI.Xaml.FlowDirection.RightToLeft;
            }
        }


        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
        }


        private void ReflectSettings()
        {
            cmbx_WorkTimePeriodMinutes.SelectedItem = IntValues.Where(x=>x.IntVal==concentrateOnSettings.WorkTimePeriodMinutes).FirstOrDefault();
            cmbx_ShortRestMinutes.SelectedItem = IntValues.Where(x=>x.IntVal==concentrateOnSettings.ShortRestMinutes).FirstOrDefault();
            cmbx_LongRestMinutes.SelectedItem = IntValues.Where(x=>x.IntVal==concentrateOnSettings.LongRestMinutes).FirstOrDefault();
            cmbx_IntervalForLongRest.SelectedItem = IntValues.Where(x=>x.IntVal==concentrateOnSettings.IntervalForLongRest).FirstOrDefault();
            toggle_send_notificaions.IsOn = concentrateOnSettings.settings_send_notifications;
            toggle_NotificationsWithSound.IsOn = concentrateOnSettings.notifications_with_sound;
        }


        private void cmbx_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            //if (comboBox.SelectedIndex == 0)
            //    comboBox.SelectedIndex = 1;

            ListOfItims selectedItem = (ListOfItims)comboBox.SelectedItem;
            if(comboBox.Name == cmbx_WorkTimePeriodMinutes.Name)
                concentrateOnSettings.WorkTimePeriodMinutes = ((ListOfItims)cmbx_WorkTimePeriodMinutes.SelectedItem).IntVal;
            if (comboBox.Name == cmbx_ShortRestMinutes.Name)
                concentrateOnSettings.ShortRestMinutes = ((ListOfItims)cmbx_ShortRestMinutes.SelectedItem).IntVal;
            if (comboBox.Name == cmbx_LongRestMinutes.Name)
                concentrateOnSettings.LongRestMinutes = ((ListOfItims)cmbx_LongRestMinutes.SelectedItem).IntVal;
            if (comboBox.Name == cmbx_IntervalForLongRest.Name)
                concentrateOnSettings.IntervalForLongRest = ((ListOfItims)cmbx_IntervalForLongRest.SelectedItem).IntVal;
        }

        private void toggle_send_notificaions_Toggled(object sender, RoutedEventArgs e)
        {
            concentrateOnSettings.settings_send_notifications = toggle_send_notificaions.IsOn;
        }

        private void toggle_NotificationsWithSound_Toggled(object sender, RoutedEventArgs e)
        {
            concentrateOnSettings.notifications_with_sound = toggle_NotificationsWithSound.IsOn;
        }
    }

    public class ListOfItims
    {
        public ListOfItims()
        {
        }

        public int IntVal { get; set; }
        public string StrVal { get; set; }
    }
}
