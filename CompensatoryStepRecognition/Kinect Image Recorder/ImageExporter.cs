using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kinect_Image_Recorder
{
    class ImageExporter
    {
        private StringBuilder stringToExport;
        private StreamWriter sw;
        private string filePath;
        private string patientName;
        private int frameCounter;

        private const int FRAMES_LIMIT = 150; //when frameCounter reaches this number, write to file.
        private const string BETWEEN_PERTUBATIONS_SEPERATOR = "=====================================\n";

        public ImageExporter(string _filePath, string _patientName)
        {
            filePath = _filePath;
            patientName = _patientName;
            stringToExport = new StringBuilder();
            frameCounter = 0;
            sw = new StreamWriter(filePath + "/" + patientName + ".csv");
            sw.WriteLine("AnkleLeft.X, AnkleLeft.Y, AnkleLeft.Z, AnkleRight.X, AnkleRight.Y, AnkleRight.Z");
        }

        public void addFrame (string frameInfo)
        {
            stringToExport.AppendLine(frameInfo);
            frameCounter++;
            if (frameCounter == FRAMES_LIMIT)
            {
                sw.Write(stringToExport.ToString());
                frameCounter = 0;
                stringToExport.Clear();
            }
            else
            {
                frameCounter++;
            }
        }

        public void closeExporter()
        {
            if (stringToExport.Length > 0)
            {
                sw.Write(stringToExport.ToString());
                stringToExport.Clear();
            }
            sw.Close();
        }

    }
}
