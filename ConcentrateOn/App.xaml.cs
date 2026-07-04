using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
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

namespace ConcentrateOn
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        /// 

        //public ConcentrateOnSettings concentrateOnSettings;
        public Home home;
        public SettingsPage settingsPage;
        public Core core;

        public App()
        {
            this.InitializeComponent();

            core = new Core();
            core.home = home;
            
            this.Suspending += OnSuspendingAsync;
            this.LeavingBackground += App_LeavingBackground;
            this.EnteredBackground += App_EnteredBackground;
        }

        
        private void App_EnteredBackground(object sender, EnteredBackgroundEventArgs e)
        {
            SaveSettings();
            core.ScheduleNotifications();
        }

        private void App_LeavingBackground(object sender, LeavingBackgroundEventArgs e)
        {
            RestoreSettings();
            core.RestoreState();
            Task.Factory.StartNew(() =>
            {
                Task.Delay(300);
                core.SetUpAnimation(core.concentrateOnSettings.is_working_time && core.concentrateOnSettings.couter_is_started);
            });
            Task.Factory.StartNew(() => core.RemoveScheduledNotifications());
        }

        public void SaveSettings()
        {
            ConcentrateOnSettings.SaveSettings(core.concentrateOnSettings);
            //StatisticsLog.SaveStatisticsLog(core.statisticsLog);
        }

        public void RestoreSettings()
        {
            core.concentrateOnSettings = ConcentrateOnSettings.ReadSettings();
            //Task.Factory.StartNew(() =>
            //    core.statisticsLog = StatisticsLog.ReadStatisticsLog());
        }

        protected override void OnActivated(IActivatedEventArgs e)
        {
            //base.OnActivated(args);

            Frame rootFrame = Window.Current.Content as Frame;
            if (rootFrame == null)
            {
                rootFrame = new Frame();
                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    RestoreSettings();
                    core.RestoreState();
                    core.RemoveScheduledNotifications();
                }
                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
            {
                if (e.PreviousExecutionState == ApplicationExecutionState.ClosedByUser)
                {
                    core.concentrateOnSettings.pause_is_on = false;
                    core.concentrateOnSettings.couter_is_started = false;
                    core.concentrateOnSettings.is_working_time = true;
                    core.concentrateOnSettings.is_short_rest = false;
                    core.concentrateOnSettings.is_long_rest = false;
                    core.concentrateOnSettings.break_counter = 0;
                }
                rootFrame.Navigate(typeof(MainPage), this);
            }
            Window.Current.Activate();
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            // CoreApplication.EnablePrelaunch was introduced in Windows 10 version 1607
            bool canEnablePrelaunch = Windows.Foundation.Metadata.ApiInformation.IsMethodPresent("Windows.ApplicationModel.Core.CoreApplication", "EnablePrelaunch");

            // NOTE: Only enable this code if you are targeting a version of Windows 10 prior to version 1607
            // and you want to opt-out of prelaunch.
            // In Windows 10 version 1511, all UWP apps were candidates for prelaunch.
            // Starting in Windows 10 version 1607, the app must opt-in to be prelaunched.
            //if ( !canEnablePrelaunch && e.PrelaunchActivated == true)
            //{
            //    return;
            //}

            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                    RestoreSettings();
                    core.RestoreState();
                    core.RemoveScheduledNotifications();
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (e.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                {
                    // On Windows 10 version 1607 or later, this code signals that this app wants to participate in prelaunch
                    if (canEnablePrelaunch)
                    {
                        TryEnablePrelaunch();
                    }

                    // When the navigation stack isn't restored navigate to the first page,
                    // configuring the new page by passing required information as a navigation
                    // parameter
                    if (e.PreviousExecutionState == ApplicationExecutionState.ClosedByUser)
                    {
                        core.concentrateOnSettings.pause_is_on = false;
                        core.concentrateOnSettings.couter_is_started = false;
                        core.concentrateOnSettings.is_working_time = true;
                        core.concentrateOnSettings.is_short_rest = false;
                        core.concentrateOnSettings.is_long_rest = false;
                        core.concentrateOnSettings.break_counter = 0;
                    }
                    rootFrame.Navigate(typeof(MainPage), this);
                }
                // Ensure the current window is active
                Window.Current.Activate();
            }
        }


        private void TryEnablePrelaunch()
        {
            Windows.ApplicationModel.Core.CoreApplication.EnablePrelaunch(true);
        }


        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspendingAsync(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }

    }
}
