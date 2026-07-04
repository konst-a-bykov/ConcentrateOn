using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Windows.Media.Playback;
using Windows.UI.Core;
using Windows.UI.Notifications;
using Windows.UI.Xaml;

namespace ConcentrateOn
{
    public class Core
    {
        public Home home;
        public Statistics statistics;
        DispatcherTimer dispatcherTimer;
        DispatcherTimer animationTimer;
        public ConcentrateOnSettings concentrateOnSettings;
        string notifications_group_name = "ConcentrateOnScheduledNotifications";
        int hours_ahead_to_schedule_notifications = 24;
        private Animations animations;
        public MediaPlayer _mediaPlayer_horizontal;
        public MediaPlayer _mediaPlayer_vertical;
        string uri_rest_image = "ms-appx:///animations/RestImage.png";
        string uri_work_image = "ms-appx:///animations/WorkImage.png";
        string uri_bell_sound = "ms-appx:///animations/bell.wav";
        string uri_bell_finish_sound = "ms-appx:///animations/bellFinish.wav";
        public AnimationsItem current_animation;

        public string concentrate_on_text;
        public string short_rest_text;
        public string long_rest_text;
        public string one_of_four_text;

        public Core()
        {
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += dispatcherTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 1000 - DateTimeOffset.UtcNow.Millisecond);

            animationTimer = new DispatcherTimer();
            animationTimer.Tick += AnimationTimer_Tick; ;
            animationTimer.Interval = new TimeSpan(0, 0, 0, 0, 500);
            animationTimer.Start();

            concentrateOnSettings = ConcentrateOnSettings.ReadSettings();
            ReadStatusStrings();
            _mediaPlayer_horizontal = new MediaPlayer();
            _mediaPlayer_vertical = new MediaPlayer();
        }


        private void ReadStatusStrings()
        {
            var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForViewIndependentUse();
            concentrate_on_text = resourceLoader.GetString("concentrate_on_text");
            short_rest_text = resourceLoader.GetString("short_rest_text"); ;
            long_rest_text = resourceLoader.GetString("long_rest_text");
            one_of_four_text = resourceLoader.GetString("one_of_four_text");
        }


        public void SetUpAnimation(bool working_state = false)
        {
            animations = new Animations(working_state);
            current_animation = animations.animationsItems[0];
            uri_rest_image = current_animation.uri_rest_image;
            uri_work_image = current_animation.uri_work_image;

            //  set up horizontal mode
            _mediaPlayer_horizontal.Source = current_animation.playbackList_horizontal;
            _mediaPlayer_horizontal.AudioCategory = MediaPlayerAudioCategory.GameMedia;
            _mediaPlayer_horizontal.IsMuted = true;
            //  set up vertical mode
            _mediaPlayer_vertical.Source = current_animation.playbackList_vertical;
            _mediaPlayer_vertical.AudioCategory = MediaPlayerAudioCategory.GameMedia;
            _mediaPlayer_vertical.IsMuted = true;
            if (home != null)
            {
                home.video_Width_horizontal = current_animation.video_Width_horizontal;
                home.video_Height_horizontal = current_animation.video_Height_horizontal;
                home.video_Width_vertical = current_animation.video_Width_vertical;
                home.video_Height_vertical = current_animation.video_Height_vertical;
                _ = home.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    //home.HorisnotalModeIsChangedSameThread();
                    home.HorisnotalModeIsChanged();
                    //home.ResizeMediaSameThread();
                    home.ResizeMedia();
                    //  video installation  horizontal
                    home.mediaElement_horizontal.SetMediaPlayer(_mediaPlayer_horizontal);
                    _mediaPlayer_horizontal.Play();
                    //  vertical
                    home.mediaElement_vertical.SetMediaPlayer(_mediaPlayer_vertical);
                    _mediaPlayer_vertical.Play();
                });
            }
        }

        private void AnimationTimer_Tick(object sender, object e)
        {
            //  Changing animation state
            if (concentrateOnSettings != null)
            {
                if (concentrateOnSettings.is_working_time && concentrateOnSettings.couter_is_started)
                {
                    concentrateOnSettings.animations_working_state = true;
                    if (current_animation != null)
                        current_animation.GoToWork();
                }
                else
                {
                    concentrateOnSettings.animations_working_state = false;
                    if (current_animation != null)
                        current_animation.GoToRest();
                }
                if (concentrateOnSettings.pause_is_on)
                {
                    if (_mediaPlayer_horizontal.PlaybackSession.PlaybackState != MediaPlaybackState.Paused &&
                        _mediaPlayer_horizontal.PlaybackSession.CanPause)
                    {
                        _mediaPlayer_horizontal.Pause();
                    }
                    if (_mediaPlayer_vertical.PlaybackSession.PlaybackState != MediaPlaybackState.Paused &&
                        _mediaPlayer_vertical.PlaybackSession.CanPause)
                    {
                        _mediaPlayer_vertical.Pause();
                    }
                }
                else
                {
                    if (_mediaPlayer_horizontal.PlaybackSession.PlaybackState == MediaPlaybackState.Paused)
                    {
                        _mediaPlayer_horizontal.Play();
                    }
                    if (_mediaPlayer_vertical.PlaybackSession.PlaybackState == MediaPlaybackState.Paused)
                    {
                        _mediaPlayer_vertical.Play();
                    }
                } 
            }
        }

        private void dispatcherTimer_Tick(object sender, object e)
        {
            TickAction();
        }

        private void TickAction()
        {
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 1000 - DateTimeOffset.UtcNow.Millisecond);

            if (concentrateOnSettings.seconds_countdown_counter <= 0)
            {
                ChangeActivityPeriod();
                StatusUpdate();
            }
            concentrateOnSettings.seconds_countdown_counter--;
            if (concentrateOnSettings.pause_is_on)
            {
                concentrateOnSettings.seconds_countdown_counter++;
            }
            else
            {
                StatusUpdate();
            }
        }

        public void RestoreState()
        {
            if (concentrateOnSettings.couter_is_started && concentrateOnSettings.pause_is_on == false)
            {
                DateTimeOffset startTime = concentrateOnSettings.GetStartTime();
                DateTimeOffset nowTime = DateTimeOffset.UtcNow;
                int seconds_to_count = (int)(nowTime - startTime).TotalSeconds;
                // Set start position
                concentrateOnSettings.break_counter = 0;
                concentrateOnSettings.seconds_countdown_counter = concentrateOnSettings.WorkTimePeriodMinutes * 60;
                concentrateOnSettings.pause_is_on = false;
                concentrateOnSettings.couter_is_started = true;
                concentrateOnSettings.is_working_time = true;
                concentrateOnSettings.is_short_rest = false;
                concentrateOnSettings.is_long_rest = false;
                for (int i = 0; i <= seconds_to_count; i++)
                {
                    // emulate ticker
                    if (concentrateOnSettings.seconds_countdown_counter == 0)
                    {
                        ChangeActivityPeriod(false);
                    }
                    concentrateOnSettings.seconds_countdown_counter--;
                }
                // updating status
                if (home != null)
                {
                    home.RestoreButtonsState(concentrateOnSettings);
                }
                StatusUpdate();
                if (concentrateOnSettings.couter_is_started && dispatcherTimer.IsEnabled == false)
                    dispatcherTimer.Start();
            }
            else
            {
                if (home != null)
                {
                    home.RestoreButtonsState(concentrateOnSettings);
                }
                StatusUpdate();
            }
        }

        

        void ScheduleOneNotification(string text, DateTimeOffset deliveryTime, string tag, string group, bool is_rest_notification = true)
        {
            if (is_rest_notification)
            {
                ToastContentBuilder toastContentBuilder = new ToastContentBuilder();
                toastContentBuilder.AddArgument("action", "viewItemsDueToday");
                toastContentBuilder.AddText(text);
                toastContentBuilder.AddInlineImage(new Uri(uri_rest_image));
                if (concentrateOnSettings.notifications_with_sound)
                    toastContentBuilder.AddAudio(new Uri(uri_bell_finish_sound));
                toastContentBuilder.Schedule(deliveryTime, toast =>
                    {
                        toast.Tag = tag;
                        toast.Group = group;
                    });
            }
            else
            {
                ToastContentBuilder toastContentBuilder = new ToastContentBuilder();
                toastContentBuilder.AddArgument("action", "viewItemsDueToday");
                toastContentBuilder.AddText(text);
                toastContentBuilder.AddInlineImage(new Uri(uri_work_image));
                if (concentrateOnSettings.notifications_with_sound)
                    toastContentBuilder.AddAudio(new Uri(uri_bell_sound));
                toastContentBuilder.Schedule(deliveryTime, toast =>
                    {
                        toast.Tag = tag;
                        toast.Group = group;
                    });
            }
        }


        internal void ScheduleNotifications()
        {
            ConcentrateOnSettings concentrateOnSettingsLocal = Helper.CreateDeepCopy<ConcentrateOnSettings>(concentrateOnSettings);
            
            if (concentrateOnSettingsLocal.couter_is_started && concentrateOnSettingsLocal.pause_is_on == false &&
                concentrateOnSettingsLocal.settings_send_notifications)
            {
                DateTimeOffset nowTime = DateTimeOffset.Now;
                int seconds_to_count = hours_ahead_to_schedule_notifications * 60 * 60;
                for (int i = 0; i <= seconds_to_count; i++)
                {
                    // emulate ticker
                    if (concentrateOnSettingsLocal.seconds_countdown_counter == 0)
                    {
                        DateTimeOffset deliveryTime = nowTime.AddSeconds((double)i);
                        ChangeActivityPeriod_ScheduleNotification(concentrateOnSettingsLocal, deliveryTime, 
                            deliveryTime.ToUnixTimeMilliseconds().ToString());
                    }
                    concentrateOnSettingsLocal.seconds_countdown_counter--;
                }
            }
        }

        private void ChangeActivityPeriod_ScheduleNotification(ConcentrateOnSettings concentrateOnSettingsLocal, 
            DateTimeOffset deliveryTime, string notificationId)
        {
            if (concentrateOnSettingsLocal.is_working_time && concentrateOnSettingsLocal.couter_is_started)
            {
                concentrateOnSettingsLocal.is_working_time = false;
                concentrateOnSettingsLocal.break_counter = concentrateOnSettingsLocal.break_counter + 1;
                if (concentrateOnSettingsLocal.break_counter >= concentrateOnSettingsLocal.IntervalForLongRest)
                {
                    concentrateOnSettingsLocal.break_counter = 0;
                    concentrateOnSettingsLocal.is_short_rest = false;
                    concentrateOnSettingsLocal.is_long_rest = true;
                    concentrateOnSettingsLocal.seconds_countdown_counter = concentrateOnSettingsLocal.LongRestMinutes * 60;
                    if (concentrateOnSettingsLocal.settings_send_notifications)
                        ScheduleOneNotification(long_rest_text,deliveryTime, notificationId, notifications_group_name);
                }
                else
                {
                    concentrateOnSettingsLocal.is_short_rest = true;
                    concentrateOnSettingsLocal.seconds_countdown_counter = concentrateOnSettingsLocal.ShortRestMinutes * 60;
                    StringBuilder str = new StringBuilder(this.short_rest_text);
                    var addon = string.Format(one_of_four_text,
                        (concentrateOnSettingsLocal.break_counter).ToString(),
                        (concentrateOnSettingsLocal.IntervalForLongRest - 1).ToString());
                    str.Append(" ");
                    str.Append(addon);
                    string short_rest_text = str.ToString();
                    if (concentrateOnSettingsLocal.settings_send_notifications)
                        ScheduleOneNotification(short_rest_text, deliveryTime, notificationId, notifications_group_name);
                    concentrateOnSettingsLocal.is_long_rest = false;
                }
            }
            else if (concentrateOnSettingsLocal.couter_is_started &&
                (concentrateOnSettingsLocal.is_short_rest || concentrateOnSettingsLocal.is_long_rest))
            {
                concentrateOnSettingsLocal.is_working_time = true;
                concentrateOnSettingsLocal.seconds_countdown_counter = concentrateOnSettingsLocal.WorkTimePeriodMinutes * 60;
                if (concentrateOnSettingsLocal.settings_send_notifications)
                    ScheduleOneNotification(concentrate_on_text, deliveryTime, notificationId, notifications_group_name,false);
                concentrateOnSettingsLocal.is_short_rest = false;
                concentrateOnSettingsLocal.is_long_rest = false;
            }
        }




        internal void RemoveScheduledNotifications()
        {
            Task.Factory.StartNew(() =>
            {
                ToastNotifierCompat notifier = ToastNotificationManagerCompat.CreateToastNotifier();
                IReadOnlyList<ScheduledToastNotification> scheduledToasts = notifier.GetScheduledToastNotifications();
                var toRemove = scheduledToasts.Where(i => i.Group == notifications_group_name);
                if (toRemove != null)
                {
                    foreach (var item in toRemove)
                        notifier.RemoveFromSchedule(item);
                }
            });
        }




        public void SendNotification(string text, bool is_rest_notification = true)
        {
            if (is_rest_notification)
            {
                ToastContentBuilder toastContentBuilder = new ToastContentBuilder();
                toastContentBuilder.AddArgument("action", "viewConversation");
                toastContentBuilder.AddArgument("conversationId", 9813);
                toastContentBuilder.AddText(text);
                toastContentBuilder.AddInlineImage(new Uri(uri_rest_image));
                if (concentrateOnSettings.notifications_with_sound)
                    toastContentBuilder.AddAudio(new Uri(uri_bell_finish_sound));
                toastContentBuilder.Show();
            }
            else
            {
                ToastContentBuilder toastContentBuilder = new ToastContentBuilder();
                toastContentBuilder.AddArgument("action", "viewConversation");
                toastContentBuilder.AddArgument("conversationId", 9813);
                toastContentBuilder.AddText(text);
                toastContentBuilder.AddInlineImage(new Uri(uri_work_image));
                if (concentrateOnSettings.notifications_with_sound)
                    toastContentBuilder.AddAudio(new Uri(uri_bell_sound));
                toastContentBuilder.Show();
            }
        }



        private void ChangeActivityPeriod(bool is_allow_notifications = true)
        {
            if (concentrateOnSettings.is_working_time && concentrateOnSettings.couter_is_started)
            {
                concentrateOnSettings.is_working_time = false;
                concentrateOnSettings.break_counter = concentrateOnSettings.break_counter + 1;
                if (concentrateOnSettings.break_counter >= concentrateOnSettings.IntervalForLongRest)
                {
                    concentrateOnSettings.break_counter = 0;
                    concentrateOnSettings.is_short_rest = false;
                    concentrateOnSettings.is_long_rest = true;
                    concentrateOnSettings.seconds_countdown_counter = concentrateOnSettings.LongRestMinutes * 60;
                    if(is_allow_notifications && concentrateOnSettings.settings_send_notifications)
                        SendNotification(long_rest_text);
                }
                else
                {
                    concentrateOnSettings.is_short_rest = true;
                    concentrateOnSettings.seconds_countdown_counter = concentrateOnSettings.ShortRestMinutes * 60;
                    StringBuilder str = new StringBuilder(this.short_rest_text);
                    var addon = string.Format(one_of_four_text,
                        (concentrateOnSettings.break_counter).ToString(),
                        (concentrateOnSettings.IntervalForLongRest - 1).ToString());
                    str.Append(" ");
                    str.Append(addon);
                    string short_rest_text = str.ToString();
                    if (is_allow_notifications && concentrateOnSettings.settings_send_notifications)
                        SendNotification(short_rest_text);
                    concentrateOnSettings.is_long_rest = false;
                }
            }
            else if (concentrateOnSettings.couter_is_started &&
                (concentrateOnSettings.is_short_rest || concentrateOnSettings.is_long_rest))
            {
                concentrateOnSettings.is_working_time = true;
                concentrateOnSettings.seconds_countdown_counter = concentrateOnSettings.WorkTimePeriodMinutes * 60;
                if (is_allow_notifications && concentrateOnSettings.settings_send_notifications)
                    SendNotification(concentrate_on_text,false);
                concentrateOnSettings.is_short_rest = false;
                concentrateOnSettings.is_long_rest = false;
            }
        }


        public void StatusUpdate()
        {
            if (home!=null)
            {
                _ = home.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                   {
                       TimeSpan time = TimeSpan.FromSeconds((double)concentrateOnSettings.seconds_countdown_counter);
                       if (concentrateOnSettings.couter_is_started == false)
                           time = TimeSpan.FromSeconds((double)concentrateOnSettings.WorkTimePeriodMinutes * 60);
                       home.tx_counter_content.Text = time.Hours >= 1 ? time.ToString(@"h\:mm\:ss") : time.ToString(@"mm\:ss");
                       home.ColorizeCounter(concentrateOnSettings.is_working_time);
                       if (concentrateOnSettings.is_working_time)
                           home.tx_status.Text = concentrate_on_text;
                       if (concentrateOnSettings.is_short_rest)
                       {
                           StringBuilder str = new StringBuilder(short_rest_text);
                           var addon = string.Format(one_of_four_text,
                               (concentrateOnSettings.break_counter).ToString(),
                               (concentrateOnSettings.IntervalForLongRest - 1).ToString());
                           str.Append(" ");
                           str.Append(addon);
                           home.tx_status.Text = str.ToString();
                       }
                       if (concentrateOnSettings.is_long_rest)
                           home.tx_status.Text = long_rest_text;
                   }); 
            }
        }


        public void bt_start_Click()
        {
            concentrateOnSettings.break_counter = 0;
            concentrateOnSettings.seconds_countdown_counter = concentrateOnSettings.WorkTimePeriodMinutes * 60;
            concentrateOnSettings.pause_is_on = false;
            concentrateOnSettings.couter_is_started = true;
            concentrateOnSettings.is_working_time = true;
            concentrateOnSettings.is_short_rest = false;
            concentrateOnSettings.is_long_rest = false;
            concentrateOnSettings.SetStartTime(DateTimeOffset.UtcNow);
            StatusUpdate();
            if (dispatcherTimer.IsEnabled == false)
                dispatcherTimer.Start();
        }


        public void bt_pause_Click()
        {
            concentrateOnSettings.pause_is_on = true;
        }


        public void bt_continue_Click()
        {
            concentrateOnSettings.pause_is_on = false;
            if (dispatcherTimer.IsEnabled == false)
                dispatcherTimer.Start();
        }


        public void bt_stop_Click()
        {
            concentrateOnSettings.pause_is_on = false;
            concentrateOnSettings.couter_is_started = false;
            concentrateOnSettings.is_working_time = true;
            concentrateOnSettings.is_short_rest = false;
            concentrateOnSettings.is_long_rest = false;
            concentrateOnSettings.break_counter = 0;
            if (dispatcherTimer.IsEnabled)
                dispatcherTimer.Stop();
            StatusUpdate();
        }

        

    }

    public static class Helper
    {
        public static T CreateDeepCopy<T>(T obj)
        {
            using (var ms = new MemoryStream())
            {
                XmlSerializer serializer = new XmlSerializer(obj.GetType());
                serializer.Serialize(ms, obj);
                ms.Seek(0, SeekOrigin.Begin);
                return (T)serializer.Deserialize(ms);
            }
        }
    }
}
