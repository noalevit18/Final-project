using AForge.Video.FFMPEG;
using AviFile;
using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Kinect_Image_Recorder
{
    class VideoManager
    {
        private Queue<Byte[]> frames;
        private string fullExportPath;
        private bool firstFrame;
        private AviManager aviManager;
        private VideoStream aviStream;
        private int finalNumberOfFrames;

        private const int QUEUE_MAX_SIZE = 30;
        private const int FRAMES_LIMIT = 30 * 60 * 2; //Export frames every 2 minutes

        public VideoManager(string patientName, string exportPath)
        {
            setFullExportPath(patientName, exportPath);
            firstFrame = true;
            frames = new Queue<Byte[]>();
            aviManager = new AviManager(fullExportPath, false);
        }

        private void setFullExportPath(string patientName, string exportPath)
        {
            int count = 1;
            try
            {
                string originalPath = exportPath + "/" + patientName;
                StringBuilder tempPath = new StringBuilder();
                tempPath.Append(originalPath).Append(".avi");
                while (File.Exists(tempPath.ToString()))
                {
                    tempPath.Clear();
                    tempPath.Append(originalPath).Append(count).Append(".avi");
                    count++;
                }
                fullExportPath = tempPath.ToString();
                Logger.writeToLog("VideoManager: setFullExportPath: Video export path = " + fullExportPath);
            } 
            catch (Exception e)
            {
                StringBuilder error = new StringBuilder();
                error.Append("VideoManager: setFullExportPath: Error in export path - chosen directory path = ").Append(exportPath).Append(", patient name = ")
                    .Append(patientName).Append(", count = ").Append(count);
                Logger.writeToLog(error.ToString());
            }
        } 

        public void addFrame (Bitmap frame)
        {
            if (frame != null)
            {
                /*if (firstFrame)
                {
                    aviStream = aviManager.AddVideoStream(true, 2, frame);
                    firstFrame = false;
                }
                else
                {
                    aviStream.AddFrame(frame);
                }*/
                frames.Add(frame);
                /*if (frames.Count == FRAMES_LIMIT)
                {
                    exportFrames();
                    frames.Clear();
                }*/
            }
            else  
            {
                Logger.writeToLog("VideoManager: addFrame: frame is null");
            }
        }

        public void flushQueueToDisk ()
        {

        }

        //public void set

        private void fillAviStreamWithBitmaps(Queue<Byte[]> colorFrames)
        {
            int transferedBitmaps = 0;
            try{

                //int frameSizeInBytes = frames[0].Size.Height * frames[0].Size.Width * 2;
                Byte[] dequeue = colorFrames.Dequeue();
                Bitmap frame = Form1.getBitmapFromColorFrame(colorFrames.Dequeue());
                aviStream = aviManager.AddVideoStream(true, 28, frame);
                //aviStream = aviManager.AddVideoStream(true, 30, frameSizeInBytes, 720, 480, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
                transferedBitmaps = 1;
                foreach (Byte[] colorFrame in colorFrames)
                {
                    frame = Form1.getBitmapFromColorFrame(colorFrame);
                    if (frame != null)
                    {
                        aviStream.AddFrame(frame);
                        transferedBitmaps++;
                        Logger.writeToLog("VideoManager: fillAviStreamWithBitmaps: transferedBitmaps = " + transferedBitmaps);
                    }
                }
            } catch(Exception e){
                Logger.writeToLog("VideoManager: fillAviStreamWithBitmaps: Error loading bitmaps- " + e.Message);
            }
            StringBuilder message = new StringBuilder();
            message.Append("VideoManager: fillAviStreamWithBitmaps: ").Append(transferedBitmaps).Append(" out of ").Append(frames.Count).Append(" Bitmaps loaded successfuly.");
            Logger.writeToLog(message.ToString());
        }

        public void exportVideo (Queue<Byte[]> colorFrames)
        {
            try
            {
                //int width = 320;
                //int height = 240;
                // create instance of video writer
                /*VideoFileWriter writer = new VideoFileWriter();
                // create new video file
                //writer.Open("test.avi", width, height, 25, VideoCodec.MPEG4);
                ColorFrame colorFrame = colorFrames.Dequeue();
                Bitmap image = Form1.getBitmapFromColorFrame(colorFrame);
                writer.Open(fullExportPath, image.Width, image.Height);
                writer.WriteVideoFrame(image, colorFrame.RelativeTime);

                foreach (ColorFrame colorFrame1 in colorFrames)
                {
                    // create a bitmap to save into the video file
                    image = Form1.getBitmapFromColorFrame (colorFrame);
                    // write 1000 video frames
                    //for (int i = 0; i < 1000; i++)
                    //{
                        //image.SetPixel(i % width, i % height, Color.Red);
                        writer.WriteVideoFrame(image, colorFrame.RelativeTime);
                    //}
                }
                writer.Close();*/

                //while (frames.Count < _finalNumberOfFrames){}

                fillAviStreamWithBitmaps(colorFrames);
                Avi.AVICOMPRESSOPTIONS_CLASS opts = new Avi.AVICOMPRESSOPTIONS_CLASS();
                //for video streams
                opts.fccType = (UInt32)Avi.mmioStringToFOURCC("vids", 0);
                opts.fccHandler = (UInt32)Avi.mmioStringToFOURCC("CVID", 0);
                //export the stream
                GCHandle handle1 = GCHandle.Alloc(aviStream);
                IntPtr aviStreamAddress = (IntPtr)handle1;
                Avi.AVISaveV(fullExportPath, 0, 0, 1, ref aviStreamAddress, ref opts);
                Logger.writeToLog("VideoManager: exportVideo: Video was exported successfuly.");
                aviManager.Close();
            } 
            catch (Exception e)
            {
                Logger.writeToLog("VideoManager: exportVideo: Error exporting video - " + e.Message);
            }
        }

        


    }
}
