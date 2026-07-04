using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Encryptor
{
    class Program
    {
        public static string key_GoToRest_mp4 = "2jd*A-PdQP2RWDvZL#?!8bHU";
        public static string key_GoToRestV_mp4 = "D!baR?8RQ2=Pm4k*QaUr8-x$";
        public static string key_GoToWork_mp4 = "$5n5Cd#$B4ftqyGr$#7kGH4z";
        public static string key_GoToWorkV_mp4 = "zvKX-xjkGq?pr3zupx3qVsV^";
        public static string key_Resting_mp4 = "s&Wzfb%KJtMm2bkT^kh6&ZcP";
        public static string key_RestingV_mp4 = "fH+5f-rTke?Yn+G9RCZMzkBW";
        public static string key_Working_mp4 = "#4VT*FctcGbt*VkTp*Mk=XV-";
        public static string key_WorkingV_mp4 = "#us3HQ%=quU4=S-J%6jv^Gy#";

        static void Main(string[] args)
        {
            Console.WriteLine("Start...");

            //Encryption();
            //   C:\source\repos\ConcentrateOn\ConcentrateOn\Animations\AncientMan
            WriteDefaultValues(@"C:\source\repos\ConcentrateOn\ConcentrateOn\Animations\AncientMan\pause.dat");
            DisplayValues(@"C:\source\repos\ConcentrateOn\ConcentrateOn\Animations\AncientMan\pause.dat");

            Console.WriteLine("... finish. Press any key.");
            Console.ReadKey();
        }


        public static void WriteDefaultValues(string fileName)
        {
            using (BinaryWriter writer = new BinaryWriter(File.Open(fileName, FileMode.Create)))
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append(key_GoToRest_mp4);
                stringBuilder.Append(key_GoToRestV_mp4);
                stringBuilder.Append(key_GoToWork_mp4);
                stringBuilder.Append(key_GoToWorkV_mp4);
                stringBuilder.Append(key_Resting_mp4);
                stringBuilder.Append(key_RestingV_mp4);
                stringBuilder.Append(key_Working_mp4);
                stringBuilder.Append(key_WorkingV_mp4);

                writer.Write(stringBuilder.ToString());
            }
        }


        public static void DisplayValues(string fileName)
        {
            if (File.Exists(fileName))
            {
                using (BinaryReader reader = new BinaryReader(File.Open(fileName, FileMode.Open)))
                {
                    string s = reader.ReadString();
                    key_GoToRest_mp4 = s.Substring(0, 24);
                    key_GoToRestV_mp4 = s.Substring(24, 24);
                    key_GoToWork_mp4 = s.Substring(48, 24);
                    key_GoToWorkV_mp4 = s.Substring(72, 24);
                    key_Resting_mp4 = s.Substring(96, 24);
                    key_RestingV_mp4 = s.Substring(120, 24);
                    key_Working_mp4 = s.Substring(144, 24);
                    key_WorkingV_mp4 = s.Substring(168, 24);
                }

                Console.WriteLine("key_GoToRest_mp4 = " + key_GoToRest_mp4);
                Console.WriteLine("key_GoToRestV_mp4 = " + key_GoToRestV_mp4);
                Console.WriteLine("key_GoToWork_mp4 = " + key_GoToWork_mp4);
                Console.WriteLine("key_GoToWorkV_mp4 = " + key_GoToWorkV_mp4);
                Console.WriteLine("key_Resting_mp4 = " + key_Resting_mp4);
                Console.WriteLine("key_RestingV_mp4 = " + key_RestingV_mp4);
                Console.WriteLine("key_Working_mp4 = " + key_Working_mp4);
                Console.WriteLine("key_WorkingV_mp4 = " + key_WorkingV_mp4);
            }
        }


        private static void Encryption()
        {
            List<Encrypt_item> encrypt_Items = new List<Encrypt_item>();
            encrypt_Items.Add(new Encrypt_item() { input_path = @"C:\source\repos\ConcentrateOn\ConcentrateOn\Animations\AncientMan\GoToRest.mp4", output_path = @"C:\source\repos\ConcentrateOn\ConcentrateOn\Animations\AncientMan\GoToRest", key = key_GoToRest_mp4 });
            encrypt_Items.Add(new Encrypt_item() { input_path = @"C:\source\repos\ConcentrateOn\ConcentrateOn\Animations\AncientMan\GoToRestV.mp4", output_path = @"C:\source\repos\ConcentrateOn\ConcentrateOn\Animations\AncientMan\GoToRestV", key = key_GoToRestV_mp4 });
            encrypt_Items.Add(new Encrypt_item() { input_path = @"C:\source\repos\ConcentrateOn\ConcentrateOn\Animations\AncientMan\GoToWork.mp4", output_path = @"C:\source\repos\ConcentrateOn\ConcentrateOn\Animations\AncientMan\GoToWork", key = key_GoToWork_mp4 });
            encrypt_Items.Add(new Encrypt_item() { input_path = @"C:\source\repos\ConcentrateOn\ConcentrateOn\Animations\AncientMan\GoToWorkV.mp4", output_path = @"C:\source\repos\ConcentrateOn\ConcentrateOn\Animations\AncientMan\GoToWorkV", key = key_GoToWorkV_mp4 });
            encrypt_Items.Add(new Encrypt_item() { input_path = @"C:\source\repos\ConcentrateOn\ConcentrateOn\Animations\AncientMan\Resting.mp4", output_path = @"C:\source\repos\ConcentrateOn\ConcentrateOn\Animations\AncientMan\Resting", key = key_Resting_mp4 });
            encrypt_Items.Add(new Encrypt_item() { input_path = @"C:\source\repos\ConcentrateOn\ConcentrateOn\Animations\AncientMan\RestingV.mp4", output_path = @"C:\source\repos\ConcentrateOn\ConcentrateOn\Animations\AncientMan\RestingV", key = key_RestingV_mp4 });
            encrypt_Items.Add(new Encrypt_item() { input_path = @"C:\source\repos\ConcentrateOn\ConcentrateOn\Animations\AncientMan\Working.mp4", output_path = @"C:\source\repos\ConcentrateOn\ConcentrateOn\Animations\AncientMan\Working", key = key_Working_mp4 });
            encrypt_Items.Add(new Encrypt_item() { input_path = @"C:\source\repos\ConcentrateOn\ConcentrateOn\Animations\AncientMan\WorkingV.mp4", output_path = @"C:\source\repos\ConcentrateOn\ConcentrateOn\Animations\AncientMan\WorkingV", key = key_WorkingV_mp4 });

            List<Task> tasks = new List<Task>();
            foreach (var item in encrypt_Items)
            {
                tasks.Add(Task.Run(() =>
                {
                    Encryptor.Encryption.FileEncrypt(item.input_path, item.output_path, item.key);
                }));
            }
            Task.WaitAll(tasks.ToArray());
        }
    }

    public class Encrypt_item
    {
        public string key;
        public string input_path;
        public string output_path;
    }

}
