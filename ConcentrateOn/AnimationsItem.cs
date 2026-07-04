using System;
using System.IO;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.Storage;
using Windows.Storage.Streams;
using System.Threading.Tasks;

namespace ConcentrateOn
{
    public class AnimationsItem
    {
        public string uri_rest_image;
        public string uri_work_image;

        public string uri_GoToRest_mp4;
        public string uri_GoToRestV_mp4;
        public string uri_GoToWork_mp4;
        public string uri_GoToWorkV_mp4;
        public string uri_Resting_mp4;
        public string uri_RestingV_mp4;
        public string uri_Working_mp4;
        public string uri_WorkingV_mp4;

        public string animation_name_resource_ID;

        public MediaPlaybackList playbackList_horizontal;
        public MediaPlaybackList playbackList_vertical;

        bool is_going_to_work_horizontal = false;
        bool is_going_to_rest_horizontal = false;
        bool is_working_now_horizontal = false;

        bool is_going_to_work_vertical = false;
        bool is_going_to_rest_vertical = false;
        bool is_working_now_vertical = false;

        public double video_Width_horizontal;
        public double video_Height_horizontal;
        public double video_Width_vertical;
        public double video_Height_vertical;



        public void MakePlaylists(bool working_state = false)
        {
            MakeHorizontalPlaylist(working_state);
            MakeVerticalPlaylist(working_state);

            playbackList_horizontal.CurrentItemChanged += PlaybackList_horizontal_CurrentItemChanged;
            playbackList_vertical.CurrentItemChanged += PlaybackList_veritcal_CurrentItemChanged;
        }

        private void PlaybackList_veritcal_CurrentItemChanged(MediaPlaybackList sender, CurrentMediaPlaybackItemChangedEventArgs args)
        {
            if (is_working_now_vertical == false && is_going_to_work_vertical && playbackList_vertical.CurrentItemIndex == 2)
            {//  have got to work
                playbackList_vertical.Items[0].IsDisabledInPlaybackList = true;
                playbackList_vertical.Items[1].IsDisabledInPlaybackList = true;
                playbackList_vertical.Items[2].IsDisabledInPlaybackList = false;
                playbackList_vertical.Items[3].IsDisabledInPlaybackList = true;
                is_going_to_work_vertical = false;
                is_working_now_vertical = true;
            }

            if (is_working_now_vertical && is_going_to_rest_vertical && playbackList_vertical.CurrentItemIndex == 0)
            {//  have got to rest
                playbackList_vertical.Items[0].IsDisabledInPlaybackList = false;
                playbackList_vertical.Items[1].IsDisabledInPlaybackList = true;
                playbackList_vertical.Items[2].IsDisabledInPlaybackList = true;
                playbackList_vertical.Items[3].IsDisabledInPlaybackList = true;
                is_going_to_rest_vertical = false;
                is_working_now_vertical = false;
            }
        }

        

       

        private void PlaybackList_horizontal_CurrentItemChanged(MediaPlaybackList sender, CurrentMediaPlaybackItemChangedEventArgs args)
        {
            if (is_working_now_horizontal == false && is_going_to_work_horizontal && playbackList_horizontal.CurrentItemIndex == 2)
            {//  have got to work
                playbackList_horizontal.Items[0].IsDisabledInPlaybackList = true;
                playbackList_horizontal.Items[1].IsDisabledInPlaybackList = true;
                playbackList_horizontal.Items[2].IsDisabledInPlaybackList = false;
                playbackList_horizontal.Items[3].IsDisabledInPlaybackList = true;
                is_going_to_work_horizontal = false;
                is_working_now_horizontal = true;
            }

            if (is_working_now_horizontal && is_going_to_rest_horizontal && playbackList_horizontal.CurrentItemIndex == 0)
            {//  have got to rest
                playbackList_horizontal.Items[0].IsDisabledInPlaybackList = false;
                playbackList_horizontal.Items[1].IsDisabledInPlaybackList = true;
                playbackList_horizontal.Items[2].IsDisabledInPlaybackList = true;
                playbackList_horizontal.Items[3].IsDisabledInPlaybackList = true;
                is_going_to_rest_horizontal = false;
                is_working_now_horizontal = false;
            }
        }


        public void GoToWork()
        {
            GoToWorkHorizontal();
            GoToWorkVertical();
        }



        private void GoToWorkHorizontal()
        {
            if (is_working_now_horizontal == false)
            {
                is_going_to_work_horizontal = true;
                playbackList_horizontal.Items[0].IsDisabledInPlaybackList = false;
                playbackList_horizontal.Items[1].IsDisabledInPlaybackList = false;
                playbackList_horizontal.Items[2].IsDisabledInPlaybackList = false;
                playbackList_horizontal.Items[3].IsDisabledInPlaybackList = true;
            }
        }

        private void GoToWorkVertical()
        {
            if (is_working_now_vertical == false)
            {
                is_going_to_work_vertical = true;
                playbackList_vertical.Items[0].IsDisabledInPlaybackList = false;
                playbackList_vertical.Items[1].IsDisabledInPlaybackList = false;
                playbackList_vertical.Items[2].IsDisabledInPlaybackList = false;
                playbackList_vertical.Items[3].IsDisabledInPlaybackList = true;
            }
        }


        public void GoToRest()
        {
            GoToRestHorizontal();
            GoToRestVertical();
        }


        private void GoToRestHorizontal()
        {
            if (is_working_now_horizontal)
            {
                is_going_to_rest_horizontal = true;
                playbackList_horizontal.Items[0].IsDisabledInPlaybackList = false;
                playbackList_horizontal.Items[1].IsDisabledInPlaybackList = true;
                playbackList_horizontal.Items[2].IsDisabledInPlaybackList = false;
                playbackList_horizontal.Items[3].IsDisabledInPlaybackList = false;
            }
        }


        private void GoToRestVertical()
        {
            if (is_working_now_vertical)
            {
                is_going_to_rest_vertical = true;
                playbackList_vertical.Items[0].IsDisabledInPlaybackList = false;
                playbackList_vertical.Items[1].IsDisabledInPlaybackList = true;
                playbackList_vertical.Items[2].IsDisabledInPlaybackList = false;
                playbackList_vertical.Items[3].IsDisabledInPlaybackList = false;
            }
        }




        public void MakeHorizontalPlaylist(bool working_state = false)
        {
            playbackList_horizontal = new MediaPlaybackList();
            playbackList_horizontal.Items.Add(new MediaPlaybackItem(MediaSource.CreateFromUri(new Uri(uri_Resting_mp4))));
            playbackList_horizontal.Items.Add(new MediaPlaybackItem(MediaSource.CreateFromUri(new Uri(uri_GoToWork_mp4))));
            playbackList_horizontal.Items.Add(new MediaPlaybackItem(MediaSource.CreateFromUri(new Uri(uri_Working_mp4))));
            playbackList_horizontal.Items.Add(new MediaPlaybackItem(MediaSource.CreateFromUri(new Uri(uri_GoToRest_mp4))));

            playbackList_horizontal.MaxPlayedItemsToKeepOpen = 2;
            playbackList_horizontal.Items[0].IsDisabledInPlaybackList = false;
            playbackList_horizontal.Items[1].IsDisabledInPlaybackList = true;
            playbackList_horizontal.Items[2].IsDisabledInPlaybackList = true;
            playbackList_horizontal.Items[3].IsDisabledInPlaybackList = true;
            if (working_state)
            {
                playbackList_horizontal.Items[0].IsDisabledInPlaybackList = true;
                playbackList_horizontal.Items[1].IsDisabledInPlaybackList = true;
                playbackList_horizontal.Items[2].IsDisabledInPlaybackList = false;
                playbackList_horizontal.Items[3].IsDisabledInPlaybackList = true;
                is_working_now_horizontal = true;
            }
            playbackList_horizontal.AutoRepeatEnabled = true;
        }


        public void MakeVerticalPlaylist(bool working_state = false)
        {
            playbackList_vertical = new MediaPlaybackList();
            playbackList_vertical.Items.Add(new MediaPlaybackItem(MediaSource.CreateFromUri(new Uri(uri_RestingV_mp4))));
            playbackList_vertical.Items.Add(new MediaPlaybackItem(MediaSource.CreateFromUri(new Uri(uri_GoToWorkV_mp4))));
            playbackList_vertical.Items.Add(new MediaPlaybackItem(MediaSource.CreateFromUri(new Uri(uri_WorkingV_mp4))));
            playbackList_vertical.Items.Add(new MediaPlaybackItem(MediaSource.CreateFromUri(new Uri(uri_GoToRestV_mp4))));

            playbackList_vertical.MaxPlayedItemsToKeepOpen = 2;
            playbackList_vertical.Items[0].IsDisabledInPlaybackList = false;
            playbackList_vertical.Items[1].IsDisabledInPlaybackList = true;
            playbackList_vertical.Items[2].IsDisabledInPlaybackList = true;
            playbackList_vertical.Items[3].IsDisabledInPlaybackList = true;
            if (working_state)
            {
                playbackList_vertical.Items[0].IsDisabledInPlaybackList = true;
                playbackList_vertical.Items[1].IsDisabledInPlaybackList = true;
                playbackList_vertical.Items[2].IsDisabledInPlaybackList = false;
                playbackList_vertical.Items[3].IsDisabledInPlaybackList = true;
                is_working_now_vertical = true;
            }
            playbackList_vertical.AutoRepeatEnabled = true;
        }
    }
}