namespace Filmstrip
{
    partial class FilmstripControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.mainPicture = new System.Windows.Forms.PictureBox();
            this.panelThumbs = new System.Windows.Forms.Panel();
            this.buttonRight = new System.Windows.Forms.Button();
            this.buttonLeft = new System.Windows.Forms.Button();
            this.panelNavigation = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.mainPicture)).BeginInit();
            this.panelNavigation.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainPicture
            // 
            this.mainPicture.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.mainPicture.BackColor = System.Drawing.Color.White;
            this.mainPicture.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.mainPicture.Location = new System.Drawing.Point(0, 0);
            this.mainPicture.Name = "mainPicture";
            this.mainPicture.Size = new System.Drawing.Size(450, 320);
            this.mainPicture.TabIndex = 0;
            this.mainPicture.TabStop = false;
            this.mainPicture.DoubleClick += new System.EventHandler(this.pictureMain_DoubleClick);
            // 
            // panelThumbs
            // 
            this.panelThumbs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.panelThumbs.Location = new System.Drawing.Point(35, 6);
            this.panelThumbs.Margin = new System.Windows.Forms.Padding(0);
            this.panelThumbs.Name = "panelThumbs";
            this.panelThumbs.Size = new System.Drawing.Size(380, 70);
            this.panelThumbs.TabIndex = 1;
            this.panelThumbs.TabStop = true;
            // 
            // buttonRight
            // 
            this.buttonRight.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonRight.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.buttonRight.Location = new System.Drawing.Point(420, 5);
            this.buttonRight.Name = "buttonRight";
            this.buttonRight.Size = new System.Drawing.Size(25, 72);
            this.buttonRight.TabIndex = 2;
            this.buttonRight.Text = ">";
            this.buttonRight.UseVisualStyleBackColor = true;
            this.buttonRight.MouseLeave += new System.EventHandler(this.buttonNav_MouseLeave);
            this.buttonRight.Click += new System.EventHandler(this.buttonRight_Click);
            this.buttonRight.KeyUp += new System.Windows.Forms.KeyEventHandler(this.FilmstripControl_KeyDown);
            this.buttonRight.MouseHover += new System.EventHandler(this.buttonNav_MouseHover);
            // 
            // buttonLeft
            // 
            this.buttonLeft.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonLeft.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.buttonLeft.Location = new System.Drawing.Point(5, 5);
            this.buttonLeft.Name = "buttonLeft";
            this.buttonLeft.Size = new System.Drawing.Size(25, 72);
            this.buttonLeft.TabIndex = 0;
            this.buttonLeft.Text = "<";
            this.buttonLeft.UseVisualStyleBackColor = true;
            this.buttonLeft.MouseLeave += new System.EventHandler(this.buttonNav_MouseLeave);
            this.buttonLeft.Click += new System.EventHandler(this.buttonLeft_Click);
            this.buttonLeft.KeyUp += new System.Windows.Forms.KeyEventHandler(this.FilmstripControl_KeyDown);
            this.buttonLeft.MouseHover += new System.EventHandler(this.buttonNav_MouseHover);
            // 
            // panelNavigation
            // 
            this.panelNavigation.BackColor = System.Drawing.Color.White;
            this.panelNavigation.Controls.Add(this.buttonLeft);
            this.panelNavigation.Controls.Add(this.buttonRight);
            this.panelNavigation.Controls.Add(this.panelThumbs);
            this.panelNavigation.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelNavigation.Location = new System.Drawing.Point(0, 318);
            this.panelNavigation.Name = "panelNavigation";
            this.panelNavigation.Size = new System.Drawing.Size(450, 82);
            this.panelNavigation.TabIndex = 1;
            this.panelNavigation.TabStop = true;
            // 
            // FilmstripControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.panelNavigation);
            this.Controls.Add(this.mainPicture);
            this.Name = "FilmstripControl";
            this.Size = new System.Drawing.Size(450, 400);
            this.Load += new System.EventHandler(this.LayoutScreenshotFilmstrip_Load);
            this.Resize += new System.EventHandler(this.FilmstripControl_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.mainPicture)).EndInit();
            this.panelNavigation.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox mainPicture;
        private System.Windows.Forms.Panel panelThumbs;
        private System.Windows.Forms.Button buttonRight;
        private System.Windows.Forms.Button buttonLeft;
        private System.Windows.Forms.Panel panelNavigation;
    }
}
