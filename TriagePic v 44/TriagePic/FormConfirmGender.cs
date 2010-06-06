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
    public partial class FormConfirmGender : Form
    {
        TriagePic parent;

        public FormConfirmGender(TriagePic p)
        {
            InitializeComponent();
            parent = p;
            // This confirmation ONLY gets called if both checkboxes are unchecked or both are checked.
            radioButtonGender1.Checked = radioButtonGender2.Checked = radioButtonGender3.Checked = radioButtonGender4.Checked = false;
            if (!parent.GenderMaleCheckBox.Checked && !parent.GenderFemaleCheckBox.Checked)
                radioButtonGender3.Checked = true;
            else if (parent.GenderMaleCheckBox.Checked && parent.GenderFemaleCheckBox.Checked)
                radioButtonGender4.Checked = true;
            else
                ErrBox.Show("Internal error");
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (radioButtonGender1.Checked) // Male
            {
                parent.GenderMaleCheckBox.Checked = true;
                parent.GenderFemaleCheckBox.Checked = false;

            }
            else if (radioButtonGender2.Checked) // Female
            {
                parent.GenderMaleCheckBox.Checked = false;
                parent.GenderFemaleCheckBox.Checked = true;

            }
            else if (radioButtonGender3.Checked) // Unknown (both checkboxes unchecked)
            {
                parent.GenderMaleCheckBox.Checked = false;
                parent.GenderFemaleCheckBox.Checked = false;

            }
            else if (radioButtonGender4.Checked) // Complex (both checkboxes checked)
            {
                parent.GenderMaleCheckBox.Checked = true;
                parent.GenderFemaleCheckBox.Checked = true;
            }
            else
                ErrBox.Show("Internal error");

            Close();   
        }
    }
}