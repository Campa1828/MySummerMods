﻿namespace AlivieskaGpsClient
{
	partial class MainForm
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
			this.zoomSlider = new System.Windows.Forms.TrackBar();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.gpsConnectionBox = new System.Windows.Forms.GroupBox();
			this.label6 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.gpsDataHeading = new System.Windows.Forms.TextBox();
			this.gpsDataSpeed = new System.Windows.Forms.TextBox();
			this.gpsDataY = new System.Windows.Forms.TextBox();
			this.gpsDataZ = new System.Windows.Forms.TextBox();
			this.gpsDataX = new System.Windows.Forms.TextBox();
			this.gpsConnectButton = new System.Windows.Forms.Button();
			this.mapImage = new System.Windows.Forms.PictureBox();
			((System.ComponentModel.ISupportInitialize)(this.zoomSlider)).BeginInit();
			this.gpsConnectionBox.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.mapImage)).BeginInit();
			this.SuspendLayout();
			// 
			// zoomSlider
			// 
			this.zoomSlider.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.zoomSlider.LargeChange = 100;
			this.zoomSlider.Location = new System.Drawing.Point(418, 12);
			this.zoomSlider.Maximum = 400;
			this.zoomSlider.Minimum = 100;
			this.zoomSlider.Name = "zoomSlider";
			this.zoomSlider.Orientation = System.Windows.Forms.Orientation.Vertical;
			this.zoomSlider.Size = new System.Drawing.Size(45, 400);
			this.zoomSlider.SmallChange = 25;
			this.zoomSlider.TabIndex = 1;
			this.zoomSlider.TickFrequency = 10;
			this.zoomSlider.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
			this.zoomSlider.Value = 100;
			this.zoomSlider.Scroll += new System.EventHandler(this.zoomSlider_Scroll);
			// 
			// textBox1
			// 
			this.textBox1.Location = new System.Drawing.Point(6, 41);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(158, 20);
			this.textBox1.TabIndex = 2;
			this.textBox1.Text = "http://localhost:8080/";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(3, 25);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(81, 13);
			this.label1.TabIndex = 3;
			this.label1.Text = "Server address:";
			// 
			// gpsConnectionBox
			// 
			this.gpsConnectionBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.gpsConnectionBox.Controls.Add(this.label6);
			this.gpsConnectionBox.Controls.Add(this.label5);
			this.gpsConnectionBox.Controls.Add(this.label4);
			this.gpsConnectionBox.Controls.Add(this.label3);
			this.gpsConnectionBox.Controls.Add(this.label2);
			this.gpsConnectionBox.Controls.Add(this.gpsDataHeading);
			this.gpsConnectionBox.Controls.Add(this.gpsDataSpeed);
			this.gpsConnectionBox.Controls.Add(this.gpsDataY);
			this.gpsConnectionBox.Controls.Add(this.gpsDataZ);
			this.gpsConnectionBox.Controls.Add(this.gpsDataX);
			this.gpsConnectionBox.Controls.Add(this.gpsConnectButton);
			this.gpsConnectionBox.Controls.Add(this.label1);
			this.gpsConnectionBox.Controls.Add(this.textBox1);
			this.gpsConnectionBox.Location = new System.Drawing.Point(469, 12);
			this.gpsConnectionBox.Name = "gpsConnectionBox";
			this.gpsConnectionBox.Size = new System.Drawing.Size(170, 230);
			this.gpsConnectionBox.TabIndex = 4;
			this.gpsConnectionBox.TabStop = false;
			this.gpsConnectionBox.Text = "Server connection";
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(6, 177);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(38, 13);
			this.label6.TabIndex = 14;
			this.label6.Text = "Speed";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(6, 203);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(47, 13);
			this.label5.TabIndex = 13;
			this.label5.Text = "Heading";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(6, 151);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(38, 13);
			this.label4.TabIndex = 12;
			this.label4.Text = "Height";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(6, 125);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(14, 13);
			this.label3.TabIndex = 11;
			this.label3.Text = "Y";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(6, 99);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(14, 13);
			this.label2.TabIndex = 5;
			this.label2.Text = "X";
			// 
			// gpsDataHeading
			// 
			this.gpsDataHeading.Location = new System.Drawing.Point(72, 200);
			this.gpsDataHeading.Name = "gpsDataHeading";
			this.gpsDataHeading.ReadOnly = true;
			this.gpsDataHeading.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.gpsDataHeading.Size = new System.Drawing.Size(92, 20);
			this.gpsDataHeading.TabIndex = 10;
			this.gpsDataHeading.Text = "012";
			// 
			// gpsDataSpeed
			// 
			this.gpsDataSpeed.Location = new System.Drawing.Point(72, 174);
			this.gpsDataSpeed.Name = "gpsDataSpeed";
			this.gpsDataSpeed.ReadOnly = true;
			this.gpsDataSpeed.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.gpsDataSpeed.Size = new System.Drawing.Size(92, 20);
			this.gpsDataSpeed.TabIndex = 9;
			this.gpsDataSpeed.Text = "012";
			// 
			// gpsDataY
			// 
			this.gpsDataY.Location = new System.Drawing.Point(72, 148);
			this.gpsDataY.Name = "gpsDataY";
			this.gpsDataY.ReadOnly = true;
			this.gpsDataY.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.gpsDataY.Size = new System.Drawing.Size(92, 20);
			this.gpsDataY.TabIndex = 8;
			this.gpsDataY.Text = "012";
			// 
			// gpsDataZ
			// 
			this.gpsDataZ.Location = new System.Drawing.Point(72, 122);
			this.gpsDataZ.Name = "gpsDataZ";
			this.gpsDataZ.ReadOnly = true;
			this.gpsDataZ.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.gpsDataZ.Size = new System.Drawing.Size(92, 20);
			this.gpsDataZ.TabIndex = 7;
			this.gpsDataZ.Text = "012";
			// 
			// gpsDataX
			// 
			this.gpsDataX.Location = new System.Drawing.Point(72, 96);
			this.gpsDataX.Name = "gpsDataX";
			this.gpsDataX.ReadOnly = true;
			this.gpsDataX.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.gpsDataX.Size = new System.Drawing.Size(92, 20);
			this.gpsDataX.TabIndex = 6;
			this.gpsDataX.Text = "012";
			// 
			// gpsConnectButton
			// 
			this.gpsConnectButton.Location = new System.Drawing.Point(6, 67);
			this.gpsConnectButton.Name = "gpsConnectButton";
			this.gpsConnectButton.Size = new System.Drawing.Size(158, 23);
			this.gpsConnectButton.TabIndex = 5;
			this.gpsConnectButton.Text = "button1";
			this.gpsConnectButton.UseVisualStyleBackColor = true;
			// 
			// mapImage
			// 
			this.mapImage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.mapImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.mapImage.Location = new System.Drawing.Point(12, 12);
			this.mapImage.Name = "mapImage";
			this.mapImage.Size = new System.Drawing.Size(400, 400);
			this.mapImage.TabIndex = 5;
			this.mapImage.TabStop = false;
			this.mapImage.Click += new System.EventHandler(this.mapImage_Click);
			this.mapImage.Paint += new System.Windows.Forms.PaintEventHandler(this.mapImage_Paint);
			this.mapImage.MouseDown += new System.Windows.Forms.MouseEventHandler(this.mapImage_MouseDown);
			this.mapImage.MouseMove += new System.Windows.Forms.MouseEventHandler(this.mapImage_MouseMove);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(651, 424);
			this.Controls.Add(this.mapImage);
			this.Controls.Add(this.gpsConnectionBox);
			this.Controls.Add(this.zoomSlider);
			this.MinimumSize = new System.Drawing.Size(667, 462);
			this.Name = "MainForm";
			this.Text = "Alivieska GPS client";
			this.Load += new System.EventHandler(this.MainForm_Load);
			this.Resize += new System.EventHandler(this.MainForm_Resize);
			this.Validated += new System.EventHandler(this.MainForm_Validated);
			((System.ComponentModel.ISupportInitialize)(this.zoomSlider)).EndInit();
			this.gpsConnectionBox.ResumeLayout(false);
			this.gpsConnectionBox.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.mapImage)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.TrackBar zoomSlider;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.GroupBox gpsConnectionBox;
		private System.Windows.Forms.Button gpsConnectButton;
		private System.Windows.Forms.TextBox gpsDataHeading;
		private System.Windows.Forms.TextBox gpsDataSpeed;
		private System.Windows.Forms.TextBox gpsDataY;
		private System.Windows.Forms.TextBox gpsDataZ;
		private System.Windows.Forms.TextBox gpsDataX;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.PictureBox mapImage;
	}
}

