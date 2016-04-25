using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace CompensatoryStepRecognition_WPF
{
    public delegate void closing(string s);

    /// <summary>
    /// Interaction logic for ChooseNameMessageBox.xaml
    /// </summary>
    public partial class ChooseNameMessageBox : Window
    {
        public event closing formClosing;

        public ChooseNameMessageBox()
        {
            InitializeComponent();
        }

        private void btn_ok_Click(object sender, EventArgs e)
        {
            if (txt_patientName.Text.Length > 0)
            {
                formClosing(txt_patientName.Text);
                this.Close();
            }
            else
                System.Windows.MessageBox.Show("Patients` name can not be empty");
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            formClosing("");
            this.Close();
        }

        private void txt_patientName_KeyPress(object sender, KeyPressEventArgs e)
        {
            switch (e.KeyChar)
            {
                case (char)Keys.Enter:
                    btn_ok.RaiseEvent(new RoutedEventArgs(System.Windows.Controls.Button.ClickEvent));
                    break;
                case (char)Keys.Escape:
                    btn_cancel.RaiseEvent(new RoutedEventArgs(System.Windows.Controls.Button.ClickEvent));
                    break;
            }
        }
    }
}
