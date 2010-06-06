using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Filmstrip
{
    internal partial class FormImageDescription : Form
    {
        private String imageDescription;
        public String ImageDescription
        {
            get { return imageDescription; }
            set { imageDescription = value; }
        }

        public FormImageDescription()
        {
            imageDescription = String.Empty;

            InitializeComponent();
        }

        private void FormImageDescription_Load(object sender, EventArgs e)
        {
            textDescription.Text = imageDescription;
            SetButtonStates();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            imageDescription = textDescription.Text;
        }

        private void textDescription_TextChanged(object sender, EventArgs e)
        {
            SetButtonStates();
        }

        private void SetButtonStates()
        {
            buttonCancel.Enabled = true;
            buttonOK.Enabled = imageDescription.Equals(textDescription.Text) ? false : true;
        }
    }
}