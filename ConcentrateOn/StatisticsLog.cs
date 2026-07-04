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
    public class StatisticsLog
    {
        public List<StatisticsLogItem> statisticsLogItems = new List<StatisticsLogItem>();
        public StatisticsLogItem current_statisticsLogItem = new StatisticsLogItem();


        const string statisticsLogFileName = "ConcentrateOnStatisticsLog.xml";

        private static string GetStatisticsLogLocation()
        {
            StorageFolder localFolder = ApplicationData.Current.LocalFolder;
            return Path.Combine(localFolder.Path, statisticsLogFileName);
        }


        public static StatisticsLog ReadStatisticsLog(string path_to_read_StatisticsLog)
        {
            StatisticsLog ret = new StatisticsLog();
            XmlSerializer xmlSerializer = new XmlSerializer(ret.GetType());
            Stream stream = new FileStream(path_to_read_StatisticsLog, FileMode.Open, FileAccess.Read);
            ret = (StatisticsLog)xmlSerializer.Deserialize(stream);
            stream.Close();
            return ret;
        }


        public static StatisticsLog ReadStatisticsLog()
        {
            if (!File.Exists(GetStatisticsLogLocation()))
            {
                StatisticsLog statisticsLog = new StatisticsLog();
                SaveStatisticsLog(statisticsLog);
            }
            return ReadStatisticsLog(GetStatisticsLogLocation());
        }


        public static void SaveStatisticsLog(string path_to_save_StatisticsLog, StatisticsLog statisticsLog)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(statisticsLog.GetType());
            Stream stream = new FileStream(path_to_save_StatisticsLog, FileMode.Create, FileAccess.Write);
            xmlSerializer.Serialize(stream, statisticsLog);
            stream.Close();
        }


        public static void SaveStatisticsLog(StatisticsLog statisticsLog)
        {
            SaveStatisticsLog(GetStatisticsLogLocation(), statisticsLog);
        }


    }


    [Serializable]
    public enum ActivityType
    {
        WorkingTime,
        ShortRest,
        LongRest
    }


    [Serializable]
    public class StatisticsLogItem
    {
        public long _startTime_unix_milliseconds = 0;
        public int action_duration_seconds = 0;
        public ActivityType activityType;


        public void SetStartTime(DateTimeOffset startTime)
        {
            _startTime_unix_milliseconds = startTime.ToUnixTimeMilliseconds();
        }

        public DateTimeOffset GetStartTime()
        {
            return DateTimeOffset.FromUnixTimeMilliseconds(_startTime_unix_milliseconds);
        }


    }
}
