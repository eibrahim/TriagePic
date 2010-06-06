namespace TriagePicNamespace
{
    partial class FormConfirmGender
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormConfirmGender));
            this.buttonOK = new System.Windows.Forms.Button();
            this.radioButtonGender1 = new System.Windows.Forms.RadioButton();
            this.radioButtonGender2 = new System.Windows.Forms.RadioButton();
            this.radioButtonGender3 = new System.Windows.Forms.RadioButton();
            this.radioButtonGender4 = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // buttonOK
            // 
            this.buttonOK.Location = new System.Drawing.Point(136, 177);
            this.buttonOK.Margin = new System.Windows.Forms.Padding(4);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(100, 28);
            this.buttonOK.TabIndex = 0;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // radioButtonGender1
            // 
            this.radioButtonGender1.AutoSize = true;
            this.radioButtonGender1.Location = new System.Drawing.Point(43, 39);
            this.radioButtonGender1.Margin = new System.Windows.Forms.Padding(4);
            this.radioButtonGender1.Name = "radioButtonGender1";
            this.radioButtonGender1.Size = new System.Drawing.Size(56, 21);
            this.radioButtonGender1.TabIndex = 1;
            this.radioButtonGender1.TabStop = true;
            this.radioButtonGender1.Text = "Male";
            this.radioButtonGender1.UseVisualStyleBackColor = true;
            // 
            // radioButtonGender2
            // 
            this.radioButtonGender2.AutoSize = true;
            this.radioButtonGender2.Location = new System.Drawing.Point(43, 69);
            this.radioButtonGender2.Margin = new System.Windows.Forms.Padding(4);
            this.radioButtonGender2.Name = "radioButtonGender2";
            this.radioButtonGender2.Size = new System.Drawing.Size(72, 21);
            this.radioButtonGender2.TabIndex = 2;
            this.radioButtonGender2.TabStop = true;
            this.radioButtonGender2.Text = "Female";
            this.radioButtonGender2.UseVisualStyleBackColor = true;
            // 
            // radioButtonGender3
            // 
            this.radioButtonGender3.AutoSize = true;
            this.radioButtonGender3.Location = new System.Drawing.Point(43, 98);
            this.radioButtonGender3.Margin = new System.Windows.Forms.Padding(4);
            this.radioButtonGender3.Name = "radioButtonGender3";
            this.radioButtonGender3.Size = new System.Drawing.Size(293, 21);
            this.radioButtonGender3.TabIndex = 3;
            this.radioButtonGender3.TabStop = true;
            this.radioButtonGender3.Text = "Unspecified (both checkboxes unchecked)";
            this.radioButtonGender3.UseVisualStyleBackColor = true;
            // 
            // radioButtonGender4
            // 
            this.radioButtonGender4.AutoSize = true;
            this.radioButtonGender4.Location = new System.Drawing.Point(43, 128);
            this.radioButtonGender4.Margin = new System.Windows.Forms.Padding(4);
            this.radioButtonGender4.Name = "radioButtonGender4";
            this.radioButtonGender4.Size = new System.Drawing.Size(256, 21);
            this.radioButtonGender4.TabIndex = 4;
            this.radioButtonGender4.TabStop = true;
            this.radioButtonGender4.Text = "Complex (both checkboxes checked)";
            this.radioButtonGender4.UseVisualStyleBackColor = true;
            // 
            // FormConfirmGender
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(369, 219);
            this.ControlBox = false;
            this.Controls.Add(this.radioButtonGender4);
            this.Controls.Add(this.radioButtonGender3);
            this.Controls.Add(this.radioButtonGender2);
            this.Controls.Add(this.radioButtonGender1);
            this.Controls.Add(this.buttonOK);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FormConfirmGender";
            this.Text = "Confirm Gender Choice";
            this.Enter += new System.EventHandler(this.buttonOK_Click);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.RadioButton radioButtonGender1;
        private System.Windows.Forms.RadioButton radioButtonGender2;
        private System.Windows.Forms.RadioButton radioButtonGender3;
        private System.Windows.Forms.RadioButton radioButtonGender4;
    }
}