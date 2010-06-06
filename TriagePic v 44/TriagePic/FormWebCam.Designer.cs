namespace TriagePic
{
    partial class FormWebCam
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormWebCam));
            this.buttonCapture = new System.Windows.Forms.Button();
            this.imageCapture = new DevEck.Devices.Video.ImageCaptureControl();
            this.comboBoxCaptureDevice = new System.Windows.Forms.ComboBox();
            this.buttonConfigure = new System.Windows.Forms.Button();
            this.chkHideAfterCapture = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // buttonCapture
            // 
            this.buttonCapture.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCapture.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonCapture.Location = new System.Drawing.Point(145, 315);
            this.buttonCapture.Name = "buttonCapture";
            this.buttonCapture.Size = new System.Drawing.Size(136, 31);
            this.buttonCapture.TabIndex = 1;
            this.buttonCapture.Text = "&Capture image";
            this.buttonCapture.UseVisualStyleBackColor = true;
            this.buttonCapture.Click += new System.EventHandler(this.buttonCapture_Click);
            // 
            // imageCapture
            // 
            this.imageCapture.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.imageCapture.BackColor = System.Drawing.Color.White;
            this.imageCapture.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.imageCapture.Device = null;
            this.imageCapture.Location = new System.Drawing.Point(12, 39);
            this.imageCapture.MaintainAspectRatio = false;
            this.imageCapture.Name = "imageCapture";
            this.imageCapture.Size = new System.Drawing.Size(364, 245);
            this.imageCapture.TabIndex = 4;
            // 
            // comboBoxCaptureDevice
            // 
            this.comboBoxCaptureDevice.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxCaptureDevice.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxCaptureDevice.FormattingEnabled = true;
            this.comboBoxCaptureDevice.Location = new System.Drawing.Point(12, 12);
            this.comboBoxCaptureDevice.Name = "comboBoxCaptureDevice";
            this.comboBoxCaptureDevice.Size = new System.Drawing.Size(364, 21);
            this.comboBoxCaptureDevice.TabIndex = 0;
            this.comboBoxCaptureDevice.SelectedIndexChanged += new System.EventHandler(this.comboBoxCaptureDevice_SelectedIndexChanged);
            // 
            // buttonConfigure
            // 
            this.buttonConfigure.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonConfigure.Location = new System.Drawing.Point(307, 315);
            this.buttonConfigure.Name = "buttonConfigure";
            this.buttonConfigure.Size = new System.Drawing.Size(64, 31);
            this.buttonConfigure.TabIndex = 3;
            this.buttonConfigure.Text = "Confi&gure";
            this.buttonConfigure.UseVisualStyleBackColor = true;
            this.buttonConfigure.Click += new System.EventHandler(this.buttonConfigure_Click);
            // 
            // chkHideAfterCapture
            // 
            this.chkHideAfterCapture.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.chkHideAfterCapture.AutoSize = true;
            this.chkHideAfterCapture.Location = new System.Drawing.Point(145, 292);
            this.chkHideAfterCapture.Name = "chkHideAfterCapture";
            this.chkHideAfterCapture.Size = new System.Drawing.Size(111, 17);
            this.chkHideAfterCapture.TabIndex = 2;
            this.chkHideAfterCapture.Text = "Hide &after capture";
            this.chkHideAfterCapture.UseVisualStyleBackColor = true;
            // 
            // FormWebCam
            // 
            this.AcceptButton = this.buttonCapture;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(388, 353);
            this.Controls.Add(this.chkHideAfterCapture);
            this.Controls.Add(this.buttonConfigure);
            this.Controls.Add(this.comboBoxCaptureDevice);
            this.Controls.Add(this.imageCapture);
            this.Controls.Add(this.buttonCapture);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(284, 287);
            this.Name = "FormWebCam";
            this.Text = "Web Cam";
            this.Load += new System.EventHandler(this.FormWebCam_Load);
            this.Shown += new System.EventHandler(this.FormWebCam_Shown);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormWebCam_FormClosed);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormWebCam_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonCapture;
        private DevEck.Devices.Video.ImageCaptureControl imageCapture;
        private System.Windows.Forms.ComboBox comboBoxCaptureDevice;
        private System.Windows.Forms.Button buttonConfigure;
        private System.Windows.Forms.CheckBox chkHideAfterCapture;
    }
}