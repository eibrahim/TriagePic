using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace TriagePic
{
    public partial class FormSentImages : Form
    {
        public FormSentImages()
        {
            InitializeComponent();
        }

        public FormSentImages(string[] images, string name)
        {
            InitializeComponent();
            for (int i = 0; i < images.Length; i++)
            {
                filmstripControl.AddImage(i,new Bitmap(images[i]), "");
            }
            filmstripControl.SelectedImageID = 0;

            this.Text = name;
        }

        private void FormSentImages_Load(object sender, EventArgs e)
        {
        }
    }
}
