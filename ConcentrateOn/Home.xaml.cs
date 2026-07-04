using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ConcentrateOn
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Home : Page
    {
        Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
        private double tx_counter_content_Horisontal_FontSize = 55;
        private double tx_counter_content_Vertical_FontSize = 40;
        public Core core;
        public MediaPlayerElement media;

        public double video_Width_horizontal = 500;
        public double video_Height_horizontal = 500;
        public double video_Width_vertical = 500;
        public double video_Height_vertical = 500;
        double video_Width = 500;
        double video_Height = 500;

        App app;
        public bool is_HorisontalMode = true;





        public Home()
        {
            this.InitializeComponent();

            //ReadStatusStrings();
        }

        

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            SetFlowDirection();
            app = (App)e.Parameter;
            core = app.core;
            core.home = this;
            app.home = this;
            RestoreButtonsState(core.concentrateOnSettings);
            core.StatusUpdate();

            if (core.current_animation != null)
            {
                video_Width_horizontal = core.current_animation.video_Width_horizontal;
                video_Height_horizontal = core.current_animation.video_Height_horizontal;
                video_Width_vertical = core.current_animation.video_Width_vertical;
                video_Height_vertical = core.current_animation.video_Height_vertical; 
            }

            is_HorisontalMode = this.ActualWidth > this.ActualHeight;
            media = is_HorisontalMode ? mediaElement_horizontal : mediaElement_vertical;
            HorisnotalModeIsChanged();
            ResizeMedia();


            if (core != null && core.current_animation != null)
            {
                _ = this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    //  video installation  horizontal
                    mediaElement_horizontal.SetMediaPlayer(core._mediaPlayer_horizontal);
                    core._mediaPlayer_horizontal.Play();
                    //  vertical
                    mediaElement_vertical.SetMediaPlayer(core._mediaPlayer_vertical);
                    core._mediaPlayer_vertical.Play();
                });
            }

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



        public void ColorizeCounter(bool is_working_time)
        {
            if(is_working_time)
            {// red
                tx_counter.BorderBrush = new SolidColorBrush(Colors.DarkRed);
                tx_counter_content.Foreground = new SolidColorBrush(Colors.DarkRed);
            }
            else
            {// green
                tx_counter.BorderBrush = new SolidColorBrush(Colors.DarkGreen);
                tx_counter_content.Foreground = new SolidColorBrush(Colors.DarkGreen);
            }
        }


        public void RestoreButtonsState(ConcentrateOnSettings concentrateOnSettings)
        {
            _ = this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                if (concentrateOnSettings.couter_is_started)
                {
                    bt_start.Visibility = Visibility.Collapsed;
                    bt_pause.Visibility = Visibility.Visible;
                }
                else
                {
                    bt_start.Visibility = Visibility.Visible;
                    bt_pause.Visibility = Visibility.Collapsed;
                }
                if (concentrateOnSettings.pause_is_on)
                {
                    bt_start.Visibility = Visibility.Collapsed;
                    bt_pause.Visibility = Visibility.Collapsed;
                    buttons_stack.Visibility = Visibility.Visible;
                }
                else
                {
                    buttons_stack.Visibility = Visibility.Collapsed;
                }
            });
        }

       


        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (this.ActualHeight > this.ActualWidth)
            {
                //   Vertical
                if(is_HorisontalMode)
                {
                    is_HorisontalMode = false;
                    HorisnotalModeIsChanged();
                }

                bt_start.VerticalAlignment = VerticalAlignment.Bottom;
                bt_start.HorizontalAlignment = HorizontalAlignment.Center;
                buttons_stack.Orientation = Orientation.Horizontal;
                bt_continue.Margin = new Thickness(0, 0, 5, 0);
                bt_stop.Margin = new Thickness(5, 0, 0, 0);

                wrap_panel.Orientation = Orientation.Vertical;
                tx_counter.VerticalAlignment = VerticalAlignment.Top;
                tx_counter.HorizontalAlignment = HorizontalAlignment.Center;
                tx_counter_content.FontSize = tx_counter_content_Vertical_FontSize;
            }
            else
            {
                //    Horisontal
                if (is_HorisontalMode == false)
                {
                    is_HorisontalMode = true;
                    HorisnotalModeIsChanged();
                }

                bt_start.VerticalAlignment = VerticalAlignment.Center;
                bt_start.HorizontalAlignment = HorizontalAlignment.Right;
                buttons_stack.Orientation = Orientation.Vertical;
                bt_continue.Margin = new Thickness(0, 0, 0, 5);
                bt_stop.Margin = new Thickness(0, 5, 0, 0);

                wrap_panel.Orientation = Orientation.Horizontal;
                tx_counter.VerticalAlignment = VerticalAlignment.Top;
                tx_counter.HorizontalAlignment = HorizontalAlignment.Right;
                tx_counter_content.FontSize = tx_counter_content_Horisontal_FontSize;
            }
            ResizeMedia();
        }

        public void HorisnotalModeIsChanged()
        {
            if (is_HorisontalMode)
            {
                video_Width = video_Width_horizontal;
                video_Height = video_Height_horizontal;
            }
            else
            {
                video_Width = video_Width_vertical;
                video_Height = video_Height_vertical;
            }
            media = is_HorisontalMode ? mediaElement_horizontal : mediaElement_vertical;

            _ = this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                if(is_HorisontalMode)
                {
                    mediaElement_horizontal.Visibility = Visibility.Visible;
                    mediaElement_vertical.Visibility = Visibility.Collapsed;
                }
                else
                {
                    mediaElement_horizontal.Visibility = Visibility.Collapsed;
                    mediaElement_vertical.Visibility = Visibility.Visible;
                }
            });
        }



        public void HorisnotalModeIsChangedSameThread()
        {
            is_HorisontalMode = this.ActualWidth > this.ActualHeight;
            if (is_HorisontalMode)
            {
                video_Width = video_Width_horizontal;
                video_Height = video_Height_horizontal;
            }
            else
            {
                video_Width = video_Width_vertical;
                video_Height = video_Height_vertical;
            }
            media = is_HorisontalMode ? mediaElement_horizontal : mediaElement_vertical;
            if (is_HorisontalMode)
            {
                mediaElement_horizontal.Visibility = Visibility.Visible;
                mediaElement_vertical.Visibility = Visibility.Collapsed;
            }
            else
            {
                mediaElement_horizontal.Visibility = Visibility.Collapsed;
                mediaElement_vertical.Visibility = Visibility.Visible;
            }
        }



        public void ResizeMedia()
        {
            double horiz = this.ActualWidth / video_Width;
            double vert = this.ActualHeight / video_Height;
            double actualHeight = this.ActualHeight;
            double actualWidth = this.ActualWidth;

            _ = this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                if (media != null)
                {
                    if (horiz > vert)
                    {
                        media.Height = actualHeight;
                        media.Width = actualHeight / video_Height * video_Width;
                    }
                    else
                    {
                        media.Width = actualWidth;
                        media.Height = actualWidth / video_Width * video_Height;
                    } 
                }
            });
        }



        public void ResizeMediaSameThread()
        {
            double horiz = this.ActualWidth / video_Width;
            double vert = this.ActualHeight / video_Height;
            double actualHeight = this.ActualHeight;
            double actualWidth = this.ActualWidth;
            if (media != null)
            {
                if (horiz > vert)
                {
                    media.Height = actualHeight;
                    media.Width = actualHeight / video_Height * video_Width;
                }
                else
                {
                    media.Width = actualWidth;
                    media.Height = actualWidth / video_Width * video_Height;
                } 
            }
        }



        private void bt_start_Click(object sender, RoutedEventArgs e)
        {
            if (core != null)
            {
                FadeOutOneStart.Begin();
                bt_start.Visibility = Visibility.Collapsed;
                bt_pause.Visibility = Visibility.Visible;
                FadeInOnePause.Begin();

                core.bt_start_Click();
            }
        }

        



        private void bt_pause_Click(object sender, RoutedEventArgs e)
        {
            if (core != null)
            {
                FadeOutOnePause.Begin();
                bt_pause.Visibility = Visibility.Collapsed;
                buttons_stack.Visibility = Visibility.Visible;
                FadeIn.Begin();

                core.bt_pause_Click();
            }
        }

        private void bt_continue_Click(object sender, RoutedEventArgs e)
        {
            if (core != null)
            {
                FadeOut.Begin();
                buttons_stack.Visibility = Visibility.Collapsed;
                bt_pause.Visibility = Visibility.Visible;
                FadeInOnePause.Begin();

                core.bt_continue_Click();
            }
        }

        private void bt_stop_Click(object sender, RoutedEventArgs e)
        {
            if (core != null)
            {
                FadeOut.Begin();
                buttons_stack.Visibility = Visibility.Collapsed;
                bt_start.Visibility = Visibility.Visible;
                FadeInOneStart.Begin();

                core.bt_stop_Click();
            }
        }

        
    }
}
