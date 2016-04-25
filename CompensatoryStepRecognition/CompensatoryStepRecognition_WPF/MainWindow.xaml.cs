using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CompensatoryStepRecognition_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary> Active Kinect sensor </summary>
        private KinectSensor kinectSensor = null;

        /// <summary> Array for the bodies (Kinect will track up to 6 people simultaneously) </summary>
        private Body[] bodies = null;

        /// <summary> Reader for body frames </summary>
        private BodyFrameReader bodyFrameReader = null;

        /// <summary> KinectBodyView object which handles drawing the Kinect bodies to a View box in the UI </summary>
        private KinectBodyView kinectBodyView = null;

        /// <summary> List of gesture detectors, there will be one detector created for each potential body (max of 6) </summary>
        private List<GestureDetector> gestureDetectorList = null;


        public MainWindow()
        {
            InitializeComponent();

            kinectSensor = KinectSensor.GetDefault();
            kinectSensor.IsAvailableChanged += Sensor_IsAvailableChanged;
            this.kinectSensor.Open();
            lbl_kinectDetection.Content = kinectSensor.IsAvailable ? "Kinect Detected" : "No Kinect Detected";
            bodyFrameReader = this.kinectSensor.BodyFrameSource.OpenReader();
            bodyFrameReader.FrameArrived += this.Reader_BodyFrameArrived;
            kinectBodyView = new KinectBodyView(kinectSensor);
            gestureDetectorList = new List<GestureDetector>();

            lbl_patientName.Content = "";

            // create a gesture detector for each body (6 bodies => 6 detectors) and create content controls to display results in the UI
            int col0Row = 0;
            int col1Row = 0;
            int maxBodies = this.kinectSensor.BodyFrameSource.BodyCount;
            for (int i = 0; i < maxBodies; ++i)
            {
                GestureResultView result = new GestureResultView(i, false, false, 0.0f);
                GestureDetector detector = new GestureDetector(this.kinectSensor, result);
                this.gestureDetectorList.Add(detector);

                // split gesture results across the first two columns of the content grid
                ContentControl contentControl = new ContentControl();
                contentControl.Content = this.gestureDetectorList[i].GestureResultView;

                if (i % 2 == 0)
                {
                    // Gesture results for bodies: 0, 2, 4
                    Grid.SetColumn(contentControl, 0);
                    Grid.SetRow(contentControl, col0Row);
                    ++col0Row;
                }
                else
                {
                    // Gesture results for bodies: 1, 3, 5
                    Grid.SetColumn(contentControl, 1);
                    Grid.SetRow(contentControl, col1Row);
                    ++col1Row;
                }

                //this.contentGrid.Children.Add(contentControl);
            }
        }

        private void Sensor_IsAvailableChanged(object sender, IsAvailableChangedEventArgs e)
        {
            lbl_kinectDetection.Content = kinectSensor.IsAvailable ? "Kinect Detected" : "No Kinect Detected";
        }

        private void Reader_BodyFrameArrived(object sender, BodyFrameArrivedEventArgs e)
        {
            bool dataReceived = false;

            using (BodyFrame bodyFrame = e.FrameReference.AcquireFrame())
            {
                if (bodyFrame != null)
                {
                    if (this.bodies == null)
                    {
                        // creates an array of 6 bodies, which is the max number of bodies that Kinect can track simultaneously
                        this.bodies = new Body[bodyFrame.BodyCount];
                    }

                    // The first time GetAndRefreshBodyData is called, Kinect will allocate each Body in the array.
                    // As long as those body objects are not disposed and not set to null in the array,
                    // those body objects will be re-used.
                    bodyFrame.GetAndRefreshBodyData(this.bodies);
                    dataReceived = true;
                }
            }

            if (dataReceived)
            {
                // visualize the new body data
                this.kinectBodyView.UpdateBodyFrame(this.bodies);

                // we may have lost/acquired bodies, so update the corresponding gesture detectors
                if (this.bodies != null)
                {
                    // loop through all bodies to see if any of the gesture detectors need to be updated
                    int maxBodies = this.kinectSensor.BodyFrameSource.BodyCount;
                    for (int i = 0; i < maxBodies; ++i)
                    {
                        Body body = this.bodies[i];
                        ulong trackingId = body.TrackingId;

                        // if the current body TrackingId changed, update the corresponding gesture detector with the new value
                        if (trackingId != this.gestureDetectorList[i].TrackingId)
                        {
                            this.gestureDetectorList[i].TrackingId = trackingId;

                            // if the current body is tracked, unpause its detector to get VisualGestureBuilderFrameArrived events
                            // if the current body is not tracked, pause its detector so we don't waste resources trying to get invalid gesture results
                            this.gestureDetectorList[i].IsPaused = trackingId == 0;
                        }
                    }
                }
            }
        }

        private void btn_browse_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            dlg.SelectedPath = Properties.Settings.Default.LastPath;

            dlg.Description = "Choose Output Destination";
            DialogResult res = dlg.ShowDialog();
            if (res.ToString().Equals("OK"))
            {
                Properties.Settings.Default.LastPath = dlg.SelectedPath;
                Properties.Settings.Default.Save();
                try
                {
                    txt_outputPath.Text = dlg.SelectedPath;
                }
                catch (IOException ex)
                {
                    System.Windows.MessageBox.Show(e.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
        }

        private void btn_start_Click(object sender, RoutedEventArgs e)
        {
            ChooseNameMessageBox chooseNameMessage = new ChooseNameMessageBox();
            chooseNameMessage.formClosing += chooseNameMessageClosed;
            chooseNameMessage.ShowDialog();
        }

        protected void chooseNameMessageClosed(string patientName)
        {
            if (patientName != "")
            {
                lbl_patientName.Content = patientName;
                Thickness margin = lbl_patientName.Margin;
                margin.Left = this.Width / 2 - lbl_patientName.Width / 2;
                lbl_patientName.Margin = margin;
                btn_start.Visibility = Visibility.Hidden;
                img_kinect.Visibility = Visibility.Visible;
                btn_stop.Visibility = Visibility.Visible;
            }
        }
    }
}
