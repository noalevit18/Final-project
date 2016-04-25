using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kinect_Image_Recorder
{
    public partial class Form1 : Form , INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        enum PERTUBATION_DIRECTION { LEFT_RIGHT, FRONT_BACK };

        private int pertubation_type;
        private bool isPlaying;

        //Active Kinect sensor
        private KinectSensor kinectSensor = null;
        
        //Array for the bodies (Kinect will track up to 6 people simultaneously)
        private Body[] bodies = null;

        //Reader for body frames
        private BodyFrameReader bodyFrameReader = null;

        private ColorFrameReader colorFrameReader = null;

        private MultiSourceFrameReader multiSourceFrameReader = null;
        
        // Width of display (depth space)
        private int displayWidth;

        // Height of display (depth space)
        private int displayHeight;

        // Coordinate mapper to map one type of point to another
        private CoordinateMapper coordinateMapper = null;

        private ImageExporter imageExporter;

        private int jointCircleSize = 8;

        private VideoManager videoManager;

        private DateTime start;
        private int counter = 0;
        private int dequeueCounter = 0;
        private Queue<Byte[]> colorFrames = new Queue<Byte[]>();
        

        private Thread refreshPictureBoxThread; 

        private Bitmap kinectImage;
        public Bitmap KinectImage
        {
            get { return kinectImage; }
            set 
            { 
                kinectImage = value;
                pic_kinect.Image = value;
                pic_kinect.Refresh();
                // Call OnPropertyChanged whenever the property is updated
                //OnPropertyChanged("kinectImage");
            }
        }

        public Form1()
        {
            InitializeComponent();
            lbl_patientName.Text = "";
            isPlaying = false;
            kinectImage = null;
            //refreshPictureBoxThread = new Thread(() => refreshKinectImage());
            //Pertubation type is required in order to analyse the Bamper movement
            if (rb_leftRight.Checked)
                pertubation_type = (int)PERTUBATION_DIRECTION.LEFT_RIGHT;
            else
                pertubation_type = (int)PERTUBATION_DIRECTION.FRONT_BACK;
        }

        private void initializeKinect()
        {
            kinectSensor = KinectSensor.GetDefault();
            if (kinectSensor != null)
                kinectSensor.Open();
            kinectSensor.IsAvailableChanged += Sensor_IsAvailableChanged;
            start = DateTime.Now;
            /*bodyFrameReader = this.kinectSensor.BodyFrameSource.OpenReader();
            if (bodyFrameReader != null)
                bodyFrameReader.FrameArrived += this.Reader_BodyFrameArrived;*/
            //colorFrameReader = kinectSensor.ColorFrameSource.OpenReader();
            //if (colorFrameReader != null)
            multiSourceFrameReader = kinectSensor.OpenMultiSourceFrameReader(FrameSourceTypes.Color | FrameSourceTypes.Body);
            if (multiSourceFrameReader != null)
                multiSourceFrameReader.MultiSourceFrameArrived += Reader_MultiSourceFrameArrived;

            // get the coordinate mapper
            this.coordinateMapper = kinectSensor.CoordinateMapper;

            // get the depth (display) extents
            FrameDescription frameDescription = kinectSensor.DepthFrameSource.FrameDescription;

            // get size of joint space
            this.displayWidth = frameDescription.Width;
            this.displayHeight = frameDescription.Height;
                
        }

        private void Sensor_IsAvailableChanged(object sender, IsAvailableChangedEventArgs e)
        {
            lbl_kinectDetection.Text = kinectSensor.IsAvailable ? "Kinect Detected" : "No Kinect Detected";
        }

        private void Reader_MultiSourceFrameArrived(object sender, MultiSourceFrameArrivedEventArgs e)
        {
            // Get a reference to the multi-frame
            if (isPlaying)
            {
                var reference = e.FrameReference.AcquireFrame();

                //Bitmap bitmap = null;
                // Open color frame
                using (ColorFrame colorFrame = reference.ColorFrameReference.AcquireFrame())
                {
                    if (colorFrame != null)
                    {
                        //ColorFrame frame = reference.ColorFrameReference.AcquireFrame();
                        int width = colorFrame.FrameDescription.Width;
                        int height = colorFrame.FrameDescription.Height;

                        byte[] pixeldata = new byte[width * height * ((32 + 7) / 8)]; //32 bits per pixel format
                        if (colorFrame.RawColorImageFormat == ColorImageFormat.Bgra)
                        {
                            colorFrame.CopyRawFrameDataToArray(pixeldata);
                        }
                        else
                        {
                            colorFrame.CopyConvertedFrameDataToArray(pixeldata, ColorImageFormat.Bgra);
                        }
                        colorFrames.Enqueue(pixeldata);
                        
                        if (counter < 60)
                            Logger.writeToLog("Form1: Reader_MultiSourceFrameArrived: frame # " + counter);
                        if (colorFrames.Count % 6 == 0)
                            refreshKinectImage(pixeldata);
                        if (colorFrames.Count % 30 == 0)
                        {
                            textBox1.Text = colorFrames.Count.ToString();
                            Process currProcess = Process.GetCurrentProcess();
                            textBox2.Text = (currProcess.PrivateMemorySize64 / 1000000).ToString();
                        }
                        counter++;
                        
                        //Thread myNewThread = new Thread(() => refreshKinectImage(colorFrame));
                        //myNewThread.Start();
                        /*try
                        {
                            int width = colorFrame.FrameDescription.Width;
                            int height = colorFrame.FrameDescription.Height;
                            
                            byte[] pixeldata = new byte[width * height * ((32 + 7) / 8)]; //32 bits per pixel format
                            if (colorFrame.RawColorImageFormat == ColorImageFormat.Bgra)
                            {
                                colorFrame.CopyRawFrameDataToArray(pixeldata);
                            }
                            else
                            {
                                colorFrame.CopyConvertedFrameDataToArray(pixeldata, ColorImageFormat.Bgra);
                            }
                            Bitmap bmap = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
                            BitmapData bmapdata = bmap.LockBits(
                                new Rectangle(0, 0, width, height),
                                ImageLockMode.ReadWrite,
                                bmap.PixelFormat);
                            IntPtr ptr = bmapdata.Scan0;
                            Marshal.Copy(pixeldata, 0, ptr, pixeldata.Length);

                            bmap.UnlockBits(bmapdata);

                            //colorBitmaps.Enqueue(bmap);

                            pic_kinect.Image = bmap;
                            this.Invalidate();

                            //Bitmap bit = new Bitmap(pic_kinect.Image, 720, 480);

                            //Thread myNewThread = new Thread(() => videoManager.addFrame(bit));
                            //myNewThread.Start();
                            //videoManager.addFrame(bit);
                            counter++;
                        }
                        catch (Exception ex)
                        {
                            Logger.writeToLog("Form: refreshKinectImage: " + ex.Message + ". counter = " + counter);
                            btn_stop.PerformClick();
                        }*/
                    }
                    }

                    //Open body frame
                    /*using (BodyFrame bodyFrame = reference.BodyFrameReference.AcquireFrame())
                    {
                        if (bodyFrame != null)
                        {
                            if (this.bodies == null)
                                this.bodies = new Body[bodyFrame.BodyCount];

                            // The first time GetAndRefreshBodyData is called, Kinect will allocate each Body in the array.
                            // As long as those body objects are not disposed and not set to null in the array,
                            // those body objects will be re-used.
                            bodyFrame.GetAndRefreshBodyData(this.bodies);

                            //Logger.writeToLog("Reader_BodyFrameArrived: Data recieved");
                            foreach (Body body in bodies)
                            {
                                if (body.IsTracked)
                                {
                                    //Logger.writeToLog("Reader_BodyFrameArrived: Body is tracked");
                                    advanceKinectFrame(body);
                                    exportFrameToCSV(body);
                                }
                            }
                        }
                    }*/
                }
            
        }

        public static Bitmap getBitmapFromColorFrame (Byte[] frame)
        {
            try
            {
                //ColorFrame colorFrame = colorFrames.Dequeue();
                /*int width = colorFrame.FrameDescription.Width;
                int height = colorFrame.FrameDescription.Height;

                byte[] pixeldata = new byte[width * height * ((32 + 7) / 8)]; //32 bits per pixel format
                if (colorFrame.RawColorImageFormat == ColorImageFormat.Bgra)
                {
                    colorFrame.CopyRawFrameDataToArray(pixeldata);
                }
                else
                {
                    colorFrame.CopyConvertedFrameDataToArray(pixeldata, ColorImageFormat.Bgra);
                }*/
                int width = 1920, height = 1080;

                Bitmap bmap = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
                //bmap.SetResolution()
                BitmapData bmapdata = bmap.LockBits(
                    new Rectangle(0, 0, width, height),
                    ImageLockMode.ReadWrite,
                    bmap.PixelFormat);
                IntPtr ptr = bmapdata.Scan0;
                Marshal.Copy(frame, 0, ptr, frame.Length);

                bmap.UnlockBits(bmapdata);
                return bmap;
            }
            catch (Exception e)
            {
                Logger.writeToLog("Form: getBitmapFromColorFrame: " + e.Message);
                return null;
            }
        }

        private void refreshKinectImage(Byte[] colorFrame)
        {
            //while (isPlaying)
            //{
                //if (colorFrames.Count > 0)
                //{
                    try
                    {
                        //ColorFrame colorFrame = colorFrames.Dequeue();
                        /*int width = colorFrame.FrameDescription.Width;
                        int height = colorFrame.FrameDescription.Height;

                        byte[] pixeldata = new byte[width * height * ((32 + 7) / 8)]; //32 bits per pixel format
                        if (colorFrame.RawColorImageFormat == ColorImageFormat.Bgra)
                        {
                            colorFrame.CopyRawFrameDataToArray(pixeldata);
                        }
                        else
                        {
                            colorFrame.CopyConvertedFrameDataToArray(pixeldata, ColorImageFormat.Bgra);
                        }
                        Bitmap bmap = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
                        BitmapData bmapdata = bmap.LockBits(
                            new Rectangle(0, 0, width, height),
                            ImageLockMode.ReadWrite,
                            bmap.PixelFormat);
                        IntPtr ptr = bmapdata.Scan0;
                        Marshal.Copy(pixeldata, 0, ptr, pixeldata.Length);

                        bmap.UnlockBits(bmapdata);*/


                        pic_kinect.Image = getBitmapFromColorFrame (colorFrame) ;
                        pic_kinect.Invalidate();
                        //textBox4.Text = colorFrame.RelativeTime.ToString();
                        //Bitmap bit = new Bitmap(pic_kinect.Image, 720, 480);

                        //Thread myNewThread = new Thread(() => videoManager.addFrame(bit));
                        //myNewThread.Start();
                        //videoManager.addFrame(bit);
                        counter++;
                    }
                    catch (Exception ex)
                    {
                        Logger.writeToLog("Form: refreshKinectImage: " + ex.Message + ". counter = " + counter);
                        btn_stop.PerformClick();
                    }
                //}
            //}
        }

        private void Reader_BodyFrameArrived(object sender, BodyFrameArrivedEventArgs e)
        {
            bool dataReceived = false;

            using (BodyFrame bodyFrame = e.FrameReference.AcquireFrame())
            {
                if (bodyFrame != null)
                {
                    if (this.bodies == null)
                        this.bodies = new Body[bodyFrame.BodyCount];

                    // The first time GetAndRefreshBodyData is called, Kinect will allocate each Body in the array.
                    // As long as those body objects are not disposed and not set to null in the array,
                    // those body objects will be re-used.
                    bodyFrame.GetAndRefreshBodyData(this.bodies);
                    dataReceived = true;
                }
            }

            if (dataReceived)
            {
                Logger.writeToLog("Reader_BodyFrameArrived: Data recieved");
                foreach (Body body in bodies)
                {
                    if (body.IsTracked)
                    {
                        Logger.writeToLog("Reader_BodyFrameArrived: Body is tracked");
                        advanceKinectFrame(body);
                        exportFrameToCSV(body);
                    }
                }
            }
        }

        private void btn_browse_Click(object sender, EventArgs e){
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            dlg.SelectedPath = Properties.Settings.Default.LastPath;

            dlg.Description = "Choose Output Destination";
            DialogResult res = dlg.ShowDialog();
            if (res == DialogResult.OK)
            {
                Properties.Settings.Default.LastPath = dlg.SelectedPath;
                Properties.Settings.Default.Save();
                try{
                    txt_outputPath.Text = dlg.SelectedPath;
                }
                catch (IOException ex){
                    MessageBox.Show(e.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
        }

        private void btn_start_Click(object sender, EventArgs e)
        {
            if (txt_outputPath.Text != "")
            {
                ChooseNameMessage chooseNameMessage = new ChooseNameMessage();
                chooseNameMessage.formClosing += chooseNameMessageClosed;
                chooseNameMessage.ShowDialog();
                counter = 0;
            }
            else
                MessageBox.Show("Please enter the desired exporting path.");
        }

        protected void chooseNameMessageClosed(string patientName){
            if (patientName != ""){
                isPlaying = true;
                initializeKinect();
                lbl_patientName.Text = patientName;
                lbl_patientName.Left = this.Width / 2 - lbl_patientName.Width / 2;
                btn_start.Visible = false;
                pic_kinect.Visible = true;
                btn_stop.Visible = true;
                pic_kinect.Visible = true;
                imageExporter = new ImageExporter(txt_outputPath.Text, patientName);
                videoManager = new VideoManager(patientName.Trim(), txt_outputPath.Text.Trim());
                //refreshPictureBoxThread.Start();
            }
        }

        /*private void btn_start_MouseHover(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.Hand;
            btn_start.BackgroundImage = ((System.Drawing.Image)(Properties.Resources.pushed_start));
        }

        private void btn_start_MouseLeave(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.Arrow;
            btn_start.BackgroundImage = ((System.Drawing.Image)(Properties.Resources.start));
        }

        private void btn_stop_MouseHover(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.Hand;
            btn_stop.BackgroundImage = ((System.Drawing.Image)(Properties.Resources.new_pushed_stop));
        }

        private void btn_stop_MouseLeave(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.Arrow;
            btn_stop.BackgroundImage = ((System.Drawing.Image)(Properties.Resources.new_stop));
        }*/

        private void DrawPoint(Joint joint, Graphics g, Brush b)
        {
            try
            {
                DepthSpacePoint depthPoint = coordinateMapper.MapCameraPointToDepthSpace(joint.Position);
                float x = depthPoint.X / displayWidth * pic_kinect.Width;
                float y = depthPoint.Y / displayHeight * pic_kinect.Height;
                g.FillPie(b, x - jointCircleSize / 2, y - jointCircleSize / 2, jointCircleSize, jointCircleSize, 0, 360);
            }
            catch (Exception e)
            {
                Logger.writeToLog("Error in DrawPoint: Could not draw the joint '" + joint.JointType.ToString() + "'." + e.Message.ToString());
            }
        }

        private void DrawLine(Joint joint1, Joint joint2, Graphics g)
        {
            try
            {
                if (joint1 == null || joint2 == null)
                    return;
                DepthSpacePoint depthPoint1 = coordinateMapper.MapCameraPointToDepthSpace(joint1.Position);
                DepthSpacePoint depthPoint2 = coordinateMapper.MapCameraPointToDepthSpace(joint2.Position);
                float x1 = depthPoint1.X / displayWidth * pic_kinect.Width;
                float y1 = depthPoint1.Y / displayHeight * pic_kinect.Height;
                float x2 = depthPoint2.X / displayWidth * pic_kinect.Width;
                float y2 = depthPoint2.Y / displayHeight * pic_kinect.Height;

                System.Drawing.PointF pStart = new System.Drawing.PointF(x1, y1);
                System.Drawing.PointF pEnd = new System.Drawing.PointF(x2, y2);
                Pen pen = new Pen(Color.Black, 3.5F);
                g.DrawLine(pen, pStart, pEnd);
            }
            catch (Exception e)
            {
                Logger.writeToLog("Error in DrawLine: Could not draw the bone '" + joint1.JointType.ToString() + "' - '" + 
                    joint1.JointType.ToString() +"'." + e.Message.ToString());
            }
        }

        private void advanceKinectFrame(Body body)
        {
            IReadOnlyDictionary<JointType, Joint> joints = body.Joints;
            Bitmap bmp = new Bitmap(pic_kinect.Width, pic_kinect.Height);
            Graphics g = Graphics.FromImage(bmp);
            Brush b = Brushes.Violet;
            //Draw joints
            foreach (Joint joint in joints.Values)
            {
                if (joint.JointType != JointType.HandTipLeft && joint.JointType != JointType.HandTipRight && joint.JointType != JointType.HandTipLeft && joint.JointType != JointType.ThumbLeft && joint.JointType != JointType.ThumbRight
                    && joint.JointType != JointType.HandLeft && joint.JointType != JointType.HandRight && joint.JointType != JointType.FootLeft
                    && joint.JointType != JointType.FootRight)
                    DrawPoint(joint, g, b);                    
            }
            //Draw bones - legs
            DrawLine(joints[JointType.AnkleLeft], joints[JointType.KneeLeft], g);
            DrawLine(joints[JointType.AnkleRight], joints[JointType.KneeRight], g);
            DrawLine(joints[JointType.KneeLeft], joints[JointType.HipLeft], g);
            DrawLine(joints[JointType.KneeRight], joints[JointType.HipRight], g);
            DrawLine(joints[JointType.HipLeft], joints[JointType.SpineBase], g);
            DrawLine(joints[JointType.HipRight], joints[JointType.SpineBase], g);
            //Spine
            DrawLine(joints[JointType.SpineBase], joints[JointType.SpineMid], g);
            DrawLine(joints[JointType.SpineMid], joints[JointType.SpineShoulder], g);
            DrawLine(joints[JointType.SpineShoulder], joints[JointType.Neck], g);
            DrawLine(joints[JointType.Neck], joints[JointType.Head], g);
            //Hands
            DrawLine(joints[JointType.SpineShoulder], joints[JointType.ShoulderLeft], g);
            DrawLine(joints[JointType.SpineShoulder], joints[JointType.ShoulderRight], g);
            DrawLine(joints[JointType.ShoulderLeft], joints[JointType.ElbowLeft], g);
            DrawLine(joints[JointType.ShoulderRight], joints[JointType.ElbowRight], g);
            DrawLine(joints[JointType.ElbowLeft], joints[JointType.WristLeft], g);
            DrawLine(joints[JointType.ElbowRight], joints[JointType.WristRight], g);
            //Refresh picture-box
            pic_kinect.Image = bmp;
            pic_kinect.Refresh();
        }

        private void exportFrameToCSV(Body body)
        {
            IReadOnlyDictionary<JointType, Joint> joints = body.Joints;
            StringBuilder frameInfo = new StringBuilder();
            //Left foot
            frameInfo.Append(joints[JointType.AnkleLeft].Position.X * 100).Append(",");
            frameInfo.Append(joints[JointType.AnkleLeft].Position.Y * 100).Append(",");
            frameInfo.Append(joints[JointType.AnkleLeft].Position.Z * 100).Append(",");
            //Right foot
            frameInfo.Append(joints[JointType.AnkleRight].Position.X * 100).Append(",");
            frameInfo.Append(joints[JointType.AnkleRight].Position.Y * 100).Append(",");
            frameInfo.Append(joints[JointType.AnkleRight].Position.Z * 100).Append(",");

            imageExporter.addFrame(frameInfo.ToString());
        }

        private void btn_stop_Click(object sender, EventArgs e)
        {
            Logger.writeToLog("Form1: btn_stop_Click: start. counter = " + counter);
            //refreshPictureBoxThread.Abort();
            isPlaying = false;
            lbl_patientName.Text = "" ;
            lbl_patientName.Visible = false;
            btn_stop.Visible = false;
            pic_kinect.Visible = false;
            btn_start.Visible = true;
            //videoManager.exportVideo(colorFrames);
            videoManager = null;
            imageExporter.closeExporter();
            Logger.writeToLog("Form1: btn_stop_Click: end");
        }

        // Create the OnPropertyChanged method to raise the event
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
