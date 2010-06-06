using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using CodeProject.Dialog; // for AppBox, MsgBox, ErrBox

namespace TriagePicNamespace
{
    public partial class FormConfirmPeds : Form
    {
        TriagePic parent;

        public FormConfirmPeds(TriagePic p)
        {
            InitializeComponent();
            parent = p;
            // This confirmation ONLY gets called if both checkboxes are unchecked or both are checked.
            radioButtonPeds1.Checked = radioButtonPeds2.Checked = radioButtonPeds3.Checked = radioButtonPeds4.Checked = false;
            if (!parent.PedsCheckBox.Checked && !parent.AdultCheckBox.Checked)
                radioButtonPeds3.Checked = true;
            else if (parent.PedsCheckBox.Checked && parent.AdultCheckBox.Checked)
                radioButtonPeds4.Checked = true;
            else
                ErrBox.Show("Internal error");
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (radioButtonPeds1.Checked) // Male
            {
                parent.PedsCheckBox.Checked = true;
                parent.AdultCheckBox.Checked = false;

            }
            else if (radioButtonPeds2.Checked) // Female
            {
                parent.PedsCheckBox.Checked = false;
                parent.AdultCheckBox.Checked = true;

            }
            else if (radioButtonPeds3.Checked) // Unknown (both checkboxes unchecked)
            {
                parent.PedsCheckBox.Checked = false;
                parent.AdultCheckBox.Checked = false;

            }
            else if (radioButtonPeds4.Checked) // Complex (both checkboxes checked)
            {
                parent.PedsCheckBox.Checked = true;
                parent.AdultCheckBox.Checked = true;
            }
            else
                ErrBox.Show("Internal error");

            Close();   
        }
    }
}