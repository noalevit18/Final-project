﻿namespace Kinect_Image_Recorder
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lbl_outputPath = new System.Windows.Forms.Label();
            this.lbl_kinectDetection = new System.Windows.Forms.Label();
            this.txt_outputPath = new System.Windows.Forms.TextBox();
            this.btn_browse = new System.Windows.Forms.Button();
            this.lbl_patientName = new System.Windows.Forms.Label();
            this.btn_stop = new System.Windows.Forms.Button();
            this.pic_kinect = new System.Windows.Forms.PictureBox();
            this.btn_start = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.lbl_pertubation = new System.Windows.Forms.Label();
            this.rb_leftRight = new System.Windows.Forms.RadioButton();
            this.gb_pertubation = new System.Windows.Forms.GroupBox();
            this.rb_BackFront = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.pic_kinect)).BeginInit();
            this.gb_pertubation.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbl_outputPath
            // 
            this.lbl_outputPath.AutoSize = true;
            this.lbl_outputPath.Font = new System.Drawing.Font("Candara", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_outputPath.Location = new System.Drawing.Point(12, 54);
            this.lbl_outputPath.Name = "lbl_outputPath";
            this.lbl_outputPath.Size = new System.Drawing.Size(182, 19);
            this.lbl_outputPath.TabIndex = 0;
            this.lbl_outputPath.Text = "Choose Output Directory:";
            // 
            // lbl_kinectDetection
            // 
            this.lbl_kinectDetection.AutoSize = true;
            this.lbl_kinectDetection.Font = new System.Drawing.Font("Candara", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_kinectDetection.Location = new System.Drawing.Point(12, 404);
            this.lbl_kinectDetection.Name = "lbl_kinectDetection";
            this.lbl_kinectDetection.Size = new System.Drawing.Size(139, 19);
            this.lbl_kinectDetection.TabIndex = 1;
            this.lbl_kinectDetection.Text = "No Kinect Detected";
            // 
            // txt_outputPath
            // 
            this.txt_outputPath.Font = new System.Drawing.Font("Candara", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_outputPath.Location = new System.Drawing.Point(200, 51);
            this.txt_outputPath.Name = "txt_outputPath";
            this.txt_outputPath.Size = new System.Drawing.Size(477, 27);
            this.txt_outputPath.TabIndex = 2;
            // 
            // btn_browse
            // 
            this.btn_browse.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_browse.Location = new System.Drawing.Point(692, 52);
            this.btn_browse.Name = "btn_browse";
            this.btn_browse.Size = new System.Drawing.Size(78, 25);
            this.btn_browse.TabIndex = 3;
            this.btn_browse.Text = "Browse";
            this.btn_browse.UseVisualStyleBackColor = true;
            this.btn_browse.Click += new System.EventHandler(this.btn_browse_Click);
            // 
            // lbl_patientName
            // 
            this.lbl_patientName.AutoSize = true;
            this.lbl_patientName.BackColor = System.Drawing.Color.Transparent;
            this.lbl_patientName.Font = new System.Drawing.Font("Candara", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_patientName.Location = new System.Drawing.Point(248, 81);
            this.lbl_patientName.Name = "lbl_patientName";
            this.lbl_patientName.Size = new System.Drawing.Size(295, 59);
            this.lbl_patientName.TabIndex = 5;
            this.lbl_patientName.Text = "patient name";
            // 
            // btn_stop
            // 
            this.btn_stop.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btn_stop.Location = new System.Drawing.Point(331, 162);
            this.btn_stop.Name = "btn_stop";
            this.btn_stop.Size = new System.Drawing.Size(81, 35);
            this.btn_stop.TabIndex = 7;
            this.btn_stop.Text = "Stop";
            this.btn_stop.UseVisualStyleBackColor = true;
            this.btn_stop.Visible = false;
            this.btn_stop.Click += new System.EventHandler(this.btn_stop_Click);
            // 
            // pic_kinect
            // 
            this.pic_kinect.BackColor = System.Drawing.Color.Transparent;
            this.pic_kinect.ErrorImage = null;
            this.pic_kinect.InitialImage = null;
            this.pic_kinect.Location = new System.Drawing.Point(431, 143);
            this.pic_kinect.Name = "pic_kinect";
            this.pic_kinect.Size = new System.Drawing.Size(339, 280);
            this.pic_kinect.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pic_kinect.TabIndex = 6;
            this.pic_kinect.TabStop = false;
            // 
            // btn_start
            // 
            this.btn_start.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btn_start.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btn_start.ImageIndex = 0;
            this.btn_start.Location = new System.Drawing.Point(244, 162);
            this.btn_start.Name = "btn_start";
            this.btn_start.Size = new System.Drawing.Size(81, 35);
            this.btn_start.TabIndex = 4;
            this.btn_start.Text = "Start";
            this.btn_start.UseVisualStyleBackColor = true;
            this.btn_start.Click += new System.EventHandler(this.btn_start_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(40, 289);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 20);
            this.textBox1.TabIndex = 8;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(40, 315);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(100, 20);
            this.textBox2.TabIndex = 9;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 292);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(14, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "Z";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 318);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(14, 13);
            this.label2.TabIndex = 11;
            this.label2.Text = "Y";
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(177, 289);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(100, 20);
            this.textBox3.TabIndex = 12;
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(177, 315);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(100, 20);
            this.textBox4.TabIndex = 13;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(157, 292);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(14, 13);
            this.label3.TabIndex = 14;
            this.label3.Text = "Z";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(157, 318);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(14, 13);
            this.label4.TabIndex = 15;
            this.label4.Text = "Y";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(80, 273);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(25, 13);
            this.label5.TabIndex = 16;
            this.label5.Text = "Left";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(197, 273);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(32, 13);
            this.label6.TabIndex = 17;
            this.label6.Text = "Right";
            // 
            // lbl_pertubation
            // 
            this.lbl_pertubation.AutoSize = true;
            this.lbl_pertubation.Font = new System.Drawing.Font("Candara", 12F);
            this.lbl_pertubation.Location = new System.Drawing.Point(3, 16);
            this.lbl_pertubation.Name = "lbl_pertubation";
            this.lbl_pertubation.Size = new System.Drawing.Size(213, 19);
            this.lbl_pertubation.TabIndex = 18;
            this.lbl_pertubation.Text = "Choose Pertubation Direction:";
            // 
            // rb_leftRight
            // 
            this.rb_leftRight.AutoSize = true;
            this.rb_leftRight.Checked = true;
            this.rb_leftRight.Font = new System.Drawing.Font("Candara", 12F);
            this.rb_leftRight.Location = new System.Drawing.Point(232, 14);
            this.rb_leftRight.Name = "rb_leftRight";
            this.rb_leftRight.Size = new System.Drawing.Size(95, 23);
            this.rb_leftRight.TabIndex = 19;
            this.rb_leftRight.TabStop = true;
            this.rb_leftRight.Text = "Left/Right";
            this.rb_leftRight.UseVisualStyleBackColor = true;
            // 
            // gb_pertubation
            // 
            this.gb_pertubation.Controls.Add(this.rb_BackFront);
            this.gb_pertubation.Controls.Add(this.lbl_pertubation);
            this.gb_pertubation.Controls.Add(this.rb_leftRight);
            this.gb_pertubation.Location = new System.Drawing.Point(9, 0);
            this.gb_pertubation.Name = "gb_pertubation";
            this.gb_pertubation.Size = new System.Drawing.Size(531, 45);
            this.gb_pertubation.TabIndex = 20;
            this.gb_pertubation.TabStop = false;
            // 
            // rb_BackFront
            // 
            this.rb_BackFront.AutoSize = true;
            this.rb_BackFront.Font = new System.Drawing.Font("Candara", 12F);
            this.rb_BackFront.Location = new System.Drawing.Point(354, 14);
            this.rb_BackFront.Name = "rb_BackFront";
            this.rb_BackFront.Size = new System.Drawing.Size(101, 23);
            this.rb_BackFront.TabIndex = 20;
            this.rb_BackFront.Text = "Front/Back";
            this.rb_BackFront.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Beige;
            this.ClientSize = new System.Drawing.Size(793, 435);
            this.Controls.Add(this.gb_pertubation);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBox4);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.btn_stop);
            this.Controls.Add(this.pic_kinect);
            this.Controls.Add(this.lbl_patientName);
            this.Controls.Add(this.btn_start);
            this.Controls.Add(this.btn_browse);
            this.Controls.Add(this.txt_outputPath);
            this.Controls.Add(this.lbl_kinectDetection);
            this.Controls.Add(this.lbl_outputPath);
            this.Name = "Form1";
            this.Text = "Compensatory Step Recognizer";
            ((System.ComponentModel.ISupportInitialize)(this.pic_kinect)).EndInit();
            this.gb_pertubation.ResumeLayout(false);
            this.gb_pertubation.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbl_outputPath;
        private System.Windows.Forms.Label lbl_kinectDetection;
        private System.Windows.Forms.TextBox txt_outputPath;
        private System.Windows.Forms.Button btn_browse;
        private System.Windows.Forms.Button btn_start;
        private System.Windows.Forms.Label lbl_patientName;
        private System.Windows.Forms.Button btn_stop;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lbl_pertubation;
        private System.Windows.Forms.RadioButton rb_leftRight;
        private System.Windows.Forms.GroupBox gb_pertubation;
        private System.Windows.Forms.RadioButton rb_BackFront;
        private System.Windows.Forms.PictureBox pic_kinect;
    }
}

