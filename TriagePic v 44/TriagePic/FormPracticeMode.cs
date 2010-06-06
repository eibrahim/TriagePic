using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using CodeProject.Dialog; // for AppBox, ErrBox, MsgBox

namespace TriagePicNamespace
{
    public partial class FormPracticeMode : Form
    {
        TriagePic parent;

        public FormPracticeMode(TriagePic p, int nPracticePatientID)
        {
            InitializeComponent();
            parent = p;
            // This confirmation ONLY gets called if Practice Mode checkbox is checked
            radioButtonPractice1.Checked = true; // Discard is default
            radioButtonPractice2.Checked = radioButtonPractice3.Checked = false;
            PracticeID_TextBox.Text = nPracticePatientID.ToString();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (radioButtonPractice1.Checked) // Discard
                parent.practiceModeChoice = 0;
            else if (radioButtonPractice2.Checked) // Send but with Practice ID
            {
                parent.practiceModeChoice = 1;
                parent.practicePatientID = Convert.ToInt32(PracticeID_TextBox.Text);
            }
            else if (radioButtonPractice3.Checked) // Send normally
                parent.practiceModeChoice = 2;
            else
                ErrBox.Show("Internal error");

            Close();   
        }
    }
}