namespace TriagePicNamespace
{
    partial class FormPracticeMode
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPracticeMode));
            this.buttonOK = new System.Windows.Forms.Button();
            this.radioButtonPractice1 = new System.Windows.Forms.RadioButton();
            this.radioButtonPractice2 = new System.Windows.Forms.RadioButton();
            this.radioButtonPractice3 = new System.Windows.Forms.RadioButton();
            this.PracticeID_Label = new System.Windows.Forms.Label();
            this.PracticeID_TextBox = new System.Windows.Forms.TextBox();
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
            // radioButtonPractice1
            // 
            this.radioButtonPractice1.Location = new System.Drawing.Point(43, 24);
            this.radioButtonPractice1.Margin = new System.Windows.Forms.Padding(4);
            this.radioButtonPractice1.Name = "radioButtonPractice1";
            this.radioButtonPractice1.Size = new System.Drawing.Size(285, 32);
            this.radioButtonPractice1.TabIndex = 1;
            this.radioButtonPractice1.TabStop = true;
            this.radioButtonPractice1.Text = "Discard  - Leave Patient ID unchanged.";
            this.radioButtonPractice1.UseVisualStyleBackColor = true;
            // 
            // radioButtonPractice2
            // 
            this.radioButtonPractice2.AutoSize = true;
            this.radioButtonPractice2.Location = new System.Drawing.Point(43, 69);
            this.radioButtonPractice2.Margin = new System.Windows.Forms.Padding(4);
            this.radioButtonPractice2.Name = "radioButtonPractice2";
            this.radioButtonPractice2.Size = new System.Drawing.Size(246, 21);
            this.radioButtonPractice2.TabIndex = 2;
            this.radioButtonPractice2.TabStop = true;
            this.radioButtonPractice2.Text = "Send, but use separate Practice ID";
            this.radioButtonPractice2.UseVisualStyleBackColor = true;
            // 
            // radioButtonPractice3
            // 
            this.radioButtonPractice3.AutoSize = true;
            this.radioButtonPractice3.Location = new System.Drawing.Point(43, 136);
            this.radioButtonPractice3.Margin = new System.Windows.Forms.Padding(4);
            this.radioButtonPractice3.Name = "radioButtonPractice3";
            this.radioButtonPractice3.Size = new System.Drawing.Size(308, 21);
            this.radioButtonPractice3.TabIndex = 3;
            this.radioButtonPractice3.TabStop = true;
            this.radioButtonPractice3.Text = "Normal - Send by email, increment Patient ID";
            this.radioButtonPractice3.UseVisualStyleBackColor = true;
            // 
            // PracticeID_Label
            // 
            this.PracticeID_Label.AutoSize = true;
            this.PracticeID_Label.Location = new System.Drawing.Point(58, 104);
            this.PracticeID_Label.Name = "PracticeID_Label";
            this.PracticeID_Label.Size = new System.Drawing.Size(64, 17);
            this.PracticeID_Label.TabIndex = 4;
            this.PracticeID_Label.Text = "Practice-";
            // 
            // PracticeID_TextBox
            // 
            this.PracticeID_TextBox.Location = new System.Drawing.Point(128, 104);
            this.PracticeID_TextBox.Name = "PracticeID_TextBox";
            this.PracticeID_TextBox.Size = new System.Drawing.Size(100, 23);
            this.PracticeID_TextBox.TabIndex = 5;
            // 
            // FormPracticeMode
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(369, 219);
            this.ControlBox = false;
            this.Controls.Add(this.PracticeID_TextBox);
            this.Controls.Add(this.PracticeID_Label);
            this.Controls.Add(this.radioButtonPractice3);
            this.Controls.Add(this.radioButtonPractice2);
            this.Controls.Add(this.radioButtonPractice1);
            this.Controls.Add(this.buttonOK);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FormPracticeMode";
            this.Text = "Practice Mode - Extra Choice";
            this.Enter += new System.EventHandler(this.buttonOK_Click);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.RadioButton radioButtonPractice1;
        private System.Windows.Forms.RadioButton radioButtonPractice2;
        private System.Windows.Forms.RadioButton radioButtonPractice3;
        private System.Windows.Forms.Label PracticeID_Label;
        private System.Windows.Forms.TextBox PracticeID_TextBox;
    }
}