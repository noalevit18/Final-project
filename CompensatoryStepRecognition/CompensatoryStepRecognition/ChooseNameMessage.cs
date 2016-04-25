using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CompensatoryStepRecognition
{
    public delegate void closing(string s);

    public partial class ChooseNameMessage : Form
    {
        public event closing formClosing;

        public ChooseNameMessage()
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
                MessageBox.Show("Patients` name can not be empty");
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
                    btn_ok.PerformClick();
                    break;
                case (char)Keys.Escape:
                    btn_cancel.PerformClick();
                    break;
            }
        }


    }
}
