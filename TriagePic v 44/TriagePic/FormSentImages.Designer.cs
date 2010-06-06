namespace TriagePic
{
    partial class FormSentImages
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSentImages));
            this.filmstripControl = new Filmstrip.FilmstripControl();
            this.SuspendLayout();
            // 
            // filmstripControl
            // 
            this.filmstripControl.BackColor = System.Drawing.Color.Transparent;
            this.filmstripControl.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("filmstripControl.BackgroundImage")));
            this.filmstripControl.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.filmstripControl.ControlBackground = System.Drawing.Color.Transparent;
            this.filmstripControl.Location = new System.Drawing.Point(14, 14);
            this.filmstripControl.Margin = new System.Windows.Forms.Padding(5);
            this.filmstripControl.Name = "filmstripControl";
            this.filmstripControl.Size = new System.Drawing.Size(668, 422);
            this.filmstripControl.TabIndex = 34;
            // 
            // FormSentImages
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(626, 450);
            this.Controls.Add(this.filmstripControl);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormSentImages";
            this.ShowIcon = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "FormSentImages";
            this.Load += new System.EventHandler(this.FormSentImages_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private Filmstrip.FilmstripControl filmstripControl;


    }
}