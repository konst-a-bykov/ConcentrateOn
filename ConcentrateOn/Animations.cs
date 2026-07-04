using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.Core;
using Windows.Media.Playback;

namespace ConcentrateOn
{
    public class Animations
    {
        public Dictionary<int,AnimationsItem> animationsItems;

        public Animations(bool working_state = false)
        {
            animationsItems = new Dictionary<int, AnimationsItem>();
            AddAnimations(working_state);
        }

        void AddAnimations(bool working_state = false)
        {
            AddAnimation0(working_state);
        }

        private void AddAnimation0(bool working_state)
        {
            AnimationsItem animationsItem = new AnimationsItem();
            animationsItem.animation_name_resource_ID = "animation_AncientMan";

            animationsItem.uri_GoToRest_mp4 = "ms-appx:///Animations/AncientMan/GoToRest.mp4";
            animationsItem.uri_GoToRestV_mp4 = "ms-appx:///Animations/AncientMan/GoToRestV.mp4";
            animationsItem.uri_GoToWork_mp4 = "ms-appx:///Animations/AncientMan/GoToWork.mp4";
            animationsItem.uri_GoToWorkV_mp4 = "ms-appx:///Animations/AncientMan/GoToWorkV.mp4";
            animationsItem.uri_Resting_mp4 = "ms-appx:///Animations/AncientMan/Resting.mp4";
            animationsItem.uri_RestingV_mp4 = "ms-appx:///Animations/AncientMan/RestingV.mp4";
            animationsItem.uri_Working_mp4 = "ms-appx:///Animations/AncientMan/Working.mp4";
            animationsItem.uri_WorkingV_mp4 = "ms-appx:///Animations/AncientMan/WorkingV.mp4";

            animationsItem.uri_rest_image = "ms-appx:///Animations/AncientMan/RestImage.png";
            animationsItem.uri_work_image = "ms-appx:///Animations/AncientMan/WorkImage.png";
            animationsItem.video_Width_horizontal = 1280;
            animationsItem.video_Height_horizontal = 720;
            animationsItem.video_Width_vertical = 720;
            animationsItem.video_Height_vertical = 1280;
            animationsItem.MakePlaylists(working_state);
            animationsItems.Add(0, animationsItem);
            
        }
    }
}
