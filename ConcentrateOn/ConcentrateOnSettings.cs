using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Windows.Storage;

namespace ConcentrateOn
{

    [Serializable]
    public class ConcentrateOnSettings
    {
        public int break_counter = 0;
        public int seconds_countdown_counter = 0;
        public bool pause_is_on = false;
        public bool couter_is_started = false;
        public bool is_working_time = true;
        public bool is_short_rest = false;
        public bool is_long_rest = false;
        public long _startTime_unix_milliseconds = 0;
        public int WorkTimePeriodMinutes = 25;
        public int IntervalForLongRest = 4;
        public int LongRestMinutes = 15;
        public int ShortRestMinutes = 5;
        public bool settings_send_notifications = true;
        public int current_animation_id = 0;
        public bool animations_working_state = false;
        public bool notifications_with_sound = true;



        const string settingsFilsName = "ConcentrateOnSettings.xml";

        public void SetStartTime(DateTimeOffset startTime)
        {
            _startTime_unix_milliseconds = startTime.ToUnixTimeMilliseconds();
        }

        public DateTimeOffset GetStartTime()
        {
            return DateTimeOffset.FromUnixTimeMilliseconds(_startTime_unix_milliseconds);
        }

        private static string GetSettingsLocation()
        {
            //StorageFolder installedLocation = Windows.ApplicationModel.Package.Current.InstalledLocation;
            StorageFolder localFolder = ApplicationData.Current.LocalFolder;
            return Path.Combine(localFolder.Path, settingsFilsName);
        }

        public static ConcentrateOnSettings ReadSettings()
        {
            if (!File.Exists(GetSettingsLocation()))
            {
                ConcentrateOnSettings mergeSpiderOutputSettings = new ConcentrateOnSettings();
                SaveSettings(mergeSpiderOutputSettings);
            }
            return ReadSettings(GetSettingsLocation());
        }

        public static ConcentrateOnSettings ReadSettings(string path_to_read_settings)
        {
            ConcentrateOnSettings ret = new ConcentrateOnSettings();
            XmlSerializer xmlSerializer = new XmlSerializer(ret.GetType());
            Stream stream = new FileStream(path_to_read_settings, FileMode.Open, FileAccess.Read);
            ret = (ConcentrateOnSettings)xmlSerializer.Deserialize(stream);//   .Serialize(stream, settings);
            stream.Close();
            return ret;
        }

        public static void SaveSettings(ConcentrateOnSettings settings)
        {
            SaveSettings(GetSettingsLocation(), settings);
        }

        public static void SaveSettings(string path_to_save_settings, ConcentrateOnSettings settings)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(settings.GetType());
            Stream stream = new FileStream(path_to_save_settings, FileMode.Create, FileAccess.Write);
            xmlSerializer.Serialize(stream, settings);
            stream.Close();
        }


    }
}
