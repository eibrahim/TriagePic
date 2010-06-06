using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using System.Windows.Forms;
using DevEck.Devices.Video;

namespace TriagePic
{
    public partial class FormWebCam : Form
    {
        private static long _counter = 1;
        private static FormWebCam _form;

        internal static void Start()
        {
            if (_form == null)
            {
                _form = new FormWebCam();
            }
            _form.WindowState = FormWindowState.Normal;
            _form.Show();
            _form.BringToFront();
        }
        public FormWebCam()
        {
            InitializeComponent();
        }


        private void UpdateDeviceList()
        {
            try
            {
                comboBoxCaptureDevice.BeginUpdate();
                comboBoxCaptureDevice.Items.Clear();

                foreach (Device device in Device.FindDevices())
                    comboBoxCaptureDevice.Items.Add(device);

               
            }
            finally
            {
                comboBoxCaptureDevice.EndUpdate();
            }
        }

        private void buttonCapture_Click(object sender, EventArgs e)
        {
            if (imageCapture.Device == null)
                MessageBox.Show("No Webcam selected!", "Problem", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                Bitmap captureImage = imageCapture.Capture();

                try
                {
                    string filename = string.Format("{0}WEBCAM{1:0000}.jpg",Constants.source, _counter++);
                    captureImage.Save(filename, ImageFormat.Jpeg);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Error saving image: " + ex.ToString());
                    MessageBox.Show("Error saving image", "Problem", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                if (chkHideAfterCapture.Checked)
                {
                    Close();
                }
            }
        }

        private void FormWebCam_Load(object sender, EventArgs e)
        {

        }

        private void FormWebCam_Shown(object sender, EventArgs e)
        {
            UpdateDeviceList();
        }

        private void comboBoxCaptureDevice_SelectedIndexChanged(object sender, EventArgs e)
        {
            Device selectedDevice = comboBoxCaptureDevice.SelectedItem as Device;

            imageCapture.Device = selectedDevice;
            imageCapture.Start();
            
        }

        private void FormWebCam_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                this.Hide();
            }
        }

        private void FormWebCam_FormClosed(object sender, FormClosedEventArgs e)
        {
            imageCapture.Stop();
            _form = null;
        }

        private void buttonConfigure_Click(object sender, EventArgs e)
        {
            try
            {
                imageCapture.DisplayProperties();
            }
            catch (Exception exception)
            {
                MessageBox.Show("Could not open camera configuration.", "Error", MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
        }
    }
}
