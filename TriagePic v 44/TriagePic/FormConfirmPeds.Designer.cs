namespace TriagePicNamespace
{
    partial class FormConfirmPeds
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormConfirmPeds));
            this.buttonOK = new System.Windows.Forms.Button();
            this.radioButtonPeds1 = new System.Windows.Forms.RadioButton();
            this.radioButtonPeds2 = new System.Windows.Forms.RadioButton();
            this.radioButtonPeds3 = new System.Windows.Forms.RadioButton();
            this.radioButtonPeds4 = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // buttonOK
            // 
            this.buttonOK.Location = new System.Drawing.Point(136, 184);
            this.buttonOK.Margin = new System.Windows.Forms.Padding(4);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(100, 28);
            this.buttonOK.TabIndex = 0;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // radioButtonPeds1
            // 
            this.radioButtonPeds1.AutoSize = true;
            this.radioButtonPeds1.Location = new System.Drawing.Point(43, 15);
            this.radioButtonPeds1.Margin = new System.Windows.Forms.Padding(4);
            this.radioButtonPeds1.Name = "radioButtonPeds1";
            this.radioButtonPeds1.Size = new System.Drawing.Size(58, 21);
            this.radioButtonPeds1.TabIndex = 1;
            this.radioButtonPeds1.TabStop = true;
            this.radioButtonPeds1.Text = "Peds";
            this.radioButtonPeds1.UseVisualStyleBackColor = true;
            // 
            // radioButtonPeds2
            // 
            this.radioButtonPeds2.AutoSize = true;
            this.radioButtonPeds2.Location = new System.Drawing.Point(43, 45);
            this.radioButtonPeds2.Margin = new System.Windows.Forms.Padding(4);
            this.radioButtonPeds2.Name = "radioButtonPeds2";
            this.radioButtonPeds2.Size = new System.Drawing.Size(58, 21);
            this.radioButtonPeds2.TabIndex = 2;
            this.radioButtonPeds2.TabStop = true;
            this.radioButtonPeds2.Text = "Adult";
            this.radioButtonPeds2.UseVisualStyleBackColor = true;
            // 
            // radioButtonPeds3
            // 
            this.radioButtonPeds3.AutoSize = true;
            this.radioButtonPeds3.Location = new System.Drawing.Point(43, 74);
            this.radioButtonPeds3.Margin = new System.Windows.Forms.Padding(4);
            this.radioButtonPeds3.Name = "radioButtonPeds3";
            this.radioButtonPeds3.Size = new System.Drawing.Size(293, 21);
            this.radioButtonPeds3.TabIndex = 3;
            this.radioButtonPeds3.TabStop = true;
            this.radioButtonPeds3.Text = "Unspecified (both checkboxes unchecked)";
            this.radioButtonPeds3.UseVisualStyleBackColor = true;
            // 
            // radioButtonPeds4
            // 
            this.radioButtonPeds4.AutoSize = true;
            this.radioButtonPeds4.Location = new System.Drawing.Point(43, 104);
            this.radioButtonPeds4.Margin = new System.Windows.Forms.Padding(4);
            this.radioButtonPeds4.Name = "radioButtonPeds4";
            this.radioButtonPeds4.Size = new System.Drawing.Size(194, 21);
            this.radioButtonPeds4.TabIndex = 4;
            this.radioButtonPeds4.TabStop = true;
            this.radioButtonPeds4.Text = "Both checkboxes checked:";
            this.radioButtonPeds4.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(72, 128);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(232, 56);
            this.label1.TabIndex = 5;
            this.label1.Text = "Older adolescent (may be seen by pediatrician or adult practitioner); or pregnant" +
                "";
            // 
            // FormConfirmPeds
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(369, 227);
            this.ControlBox = false;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.radioButtonPeds4);
            this.Controls.Add(this.radioButtonPeds3);
            this.Controls.Add(this.radioButtonPeds2);
            this.Controls.Add(this.radioButtonPeds1);
            this.Controls.Add(this.buttonOK);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FormConfirmPeds";
            this.Text = "Confirm Peds vs. Adult Choice";
            this.Enter += new System.EventHandler(this.buttonOK_Click);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.RadioButton radioButtonPeds1;
        private System.Windows.Forms.RadioButton radioButtonPeds2;
        private System.Windows.Forms.RadioButton radioButtonPeds3;
        private System.Windows.Forms.RadioButton radioButtonPeds4;
        private System.Windows.Forms.Label label1;
    }
}