using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Background;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace ConcentrateOn
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        App app;

        public MainPage()
        {
            this.InitializeComponent();

            // получаем ссылку на внешний вид приложения
            Windows.UI.ViewManagement.ApplicationView appView = Windows.UI.ViewManagement.ApplicationView.GetForCurrentView();
            // минимальный размер 300х250
            appView.SetPreferredMinSize(new Size(300, 250));
            
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            SetFlowDirection();
            app = (App)e.Parameter;
            myFrame.Navigate(typeof(Home), app);
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



        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (home.IsSelected)
            {
                myFrame.Navigate(typeof(Home), app);
                mySplitView.DisplayMode = SplitViewDisplayMode.Overlay;
                mySplitView.IsPaneOpen = false;
            }
            if(menu_hamburger_btn.IsSelected)
            {
                mySplitView.DisplayMode = SplitViewDisplayMode.Overlay;
                mySplitView.IsPaneOpen = false;
            }
            if (settings.IsSelected)
            {
                myFrame.Navigate(typeof(SettingsPage), app);
                mySplitView.DisplayMode = SplitViewDisplayMode.CompactOverlay;
                mySplitView.IsPaneOpen = false;
            }
            if (about.IsSelected)
            {
                myFrame.Navigate(typeof(About), app);
                mySplitView.DisplayMode = SplitViewDisplayMode.CompactOverlay;
                mySplitView.IsPaneOpen = false;
            }
            if (donate.IsSelected)
            {
                //OpenDonatePage();
                myFrame.Navigate(typeof(DonatePage), app);
                mySplitView.DisplayMode = SplitViewDisplayMode.CompactOverlay;
                mySplitView.IsPaneOpen = false;
            }
            //if (statistics.IsSelected)
            //{
            //    myFrame.Navigate(typeof(Statistics), app);
            //    mySplitView.DisplayMode = SplitViewDisplayMode.CompactOverlay;
            //    mySplitView.IsPaneOpen = false;
            //}
        }

        private async void OpenDonatePage()
        {
            string uriToLaunch = @"https://www.paypal.com/paypalme/Konstantin374";
            var uri = new Uri(uriToLaunch);
            var success = await Windows.System.Launcher.LaunchUriAsync(uri);
            if (success)
            {
                // URI launched
            }
            else
            {
                // URI launch failed
            }
        }

        private void HamburgerButton_Click(object sender, RoutedEventArgs e)
        {
            mySplitView.IsPaneOpen = !mySplitView.IsPaneOpen;
        }

        private void HamburgerButton_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            //mySplitView.IsPaneOpen = true;
        }

    }
}
