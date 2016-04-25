using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kinect_Image_Recorder
{
    public class Logger
    {
        private static string path = "Log.txt";

        public static void writeToLog(string message)
        {
            try
            {
                if (!File.Exists(path))
                {
                    using (StreamWriter sw = File.CreateText(path))
                    {
                        sw.Write("");
                    }
                }
                using (StreamWriter sw = File.AppendText(path))
                {
                    sw.WriteLine(DateTime.Now + " " + message);
                }
            }
            catch (Exception ex)
            {

            }
        }

        public static void deleteLog()
        {
            if (File.Exists(path))
                File.Delete(path);
        }
    }
}
