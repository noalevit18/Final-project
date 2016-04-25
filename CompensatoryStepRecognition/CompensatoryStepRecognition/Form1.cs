using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CompensatoryStepRecognition
{
    

    public partial class Form1 : Form
    {
        /// <summary> Active Kinect sensor </summary>
        private KinectSensor kinectSensor = null;
        
        /// <summary> Array for the bodies (Kinect will track up to 6 people simultaneously) </summary>
        private Body[] bodies = null;

        /// <summary> Reader for body frames </summary>
        private BodyFrameReader bodyFrameReader = null;

        /// <summary>
        /// Width of display (depth space)
        /// </summary>
        private int displayWidth;

        /// <summary>
        /// Height of display (depth space)
        /// </summary>
        private int displayHeight;

        /// <summary>
        /// Coordinate mapper to map one type of point to another
        /// </summary>
        private CoordinateMapper coordinateMapper = null;

        private int jointCircleSize = 8;
        
        public Form1()
        {
            InitializeComponent();
            initializeKinect();
            lbl_patientName.Text = "";
        }

        private void initializeKinect()
        {
            kinectSensor = KinectSensor.GetDefault();
            if (kinectSensor != null)
                kinectSensor.Open();
            kinectSensor.IsAvailableChanged += Sensor_IsAvailableChanged;

            bodyFrameReader = this.kinectSensor.BodyFrameSource.OpenReader();
            if (bodyFrameReader != null)
                bodyFrameReader.FrameArrived += this.Reader_BodyFrameArrived;

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
            ChooseNameMessage chooseNameMessage = new ChooseNameMessage();
            chooseNameMessage.formClosing += chooseNameMessageClosed;
            chooseNameMessage.ShowDialog(); 
        }

        protected void chooseNameMessageClosed(string patientName){
            if (patientName != ""){
                lbl_patientName.Text = patientName;
                lbl_patientName.Left = this.Width / 2 - lbl_patientName.Width / 2;
                btn_start.Visible = false;
                pic_kinect.Visible = true;
                btn_stop.Visible = true;
                pic_kinect.Visible = true;
            }
        }

        private void btn_start_MouseHover(object sender, EventArgs e)
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
        }

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


    }
}
