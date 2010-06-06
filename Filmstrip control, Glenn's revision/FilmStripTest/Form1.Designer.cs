namespace FilmStripTest
{
    partial class FormFilmstripTest
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
            this.label1 = new System.Windows.Forms.Label();
            this.labelSelectedID = new System.Windows.Forms.Label();
            this.pictureSelected = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
            this.comboImages = new System.Windows.Forms.ComboBox();
            this.buttonClose = new System.Windows.Forms.Button();
            this.buttonAdd = new System.Windows.Forms.Button();
            this.buttonRemove = new System.Windows.Forms.Button();
            this.buttonClear = new System.Windows.Forms.Button();
            this.groupBoxImages = new System.Windows.Forms.GroupBox();
            this.buttonAddDefault = new System.Windows.Forms.Button();
            this.groupBoxFilmstrip = new System.Windows.Forms.GroupBox();
            this.filmstripControl = new Filmstrip.FilmstripControl();
            this.groupBoxSelection = new System.Windows.Forms.GroupBox();
            this.buttonUpdateDesc = new System.Windows.Forms.Button();
            this.textSelectedDesc = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.groupBoxControlProperties = new System.Windows.Forms.GroupBox();
            this.comboHighlightColour = new OfficePickers.ColorPicker.ComboBoxColorPicker();
            this.comboBackgroundColour = new OfficePickers.ColorPicker.ComboBoxColorPicker();
            this.numericHoverDelay = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.comboMainImageLayout = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.comboThumbsLocation = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureSelected)).BeginInit();
            this.groupBoxImages.SuspendLayout();
            this.groupBoxFilmstrip.SuspendLayout();
            this.groupBoxSelection.SuspendLayout();
            this.groupBoxControlProperties.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericHoverDelay)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 85);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Selected Image ID";
            // 
            // labelSelectedID
            // 
            this.labelSelectedID.AutoSize = true;
            this.labelSelectedID.Location = new System.Drawing.Point(127, 85);
            this.labelSelectedID.Name = "labelSelectedID";
            this.labelSelectedID.Size = new System.Drawing.Size(18, 13);
            this.labelSelectedID.TabIndex = 3;
            this.labelSelectedID.Text = "ID";
            // 
            // pictureSelected
            // 
            this.pictureSelected.Location = new System.Drawing.Point(6, 103);
            this.pictureSelected.Name = "pictureSelected";
            this.pictureSelected.Size = new System.Drawing.Size(134, 84);
            this.pictureSelected.TabIndex = 3;
            this.pictureSelected.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 27);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(108, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Set selected image id";
            // 
            // comboImages
            // 
            this.comboImages.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboImages.FormattingEnabled = true;
            this.comboImages.Location = new System.Drawing.Point(6, 43);
            this.comboImages.Name = "comboImages";
            this.comboImages.Size = new System.Drawing.Size(134, 21);
            this.comboImages.TabIndex = 1;
            this.comboImages.SelectedIndexChanged += new System.EventHandler(this.comboImages_SelectedIndexChanged);
            // 
            // buttonClose
            // 
            this.buttonClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonClose.Location = new System.Drawing.Point(711, 531);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(75, 23);
            this.buttonClose.TabIndex = 4;
            this.buttonClose.Text = "Close";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // buttonAdd
            // 
            this.buttonAdd.Location = new System.Drawing.Point(6, 49);
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.Size = new System.Drawing.Size(75, 23);
            this.buttonAdd.TabIndex = 1;
            this.buttonAdd.Text = "Add...";
            this.buttonAdd.UseVisualStyleBackColor = true;
            this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click);
            // 
            // buttonRemove
            // 
            this.buttonRemove.AutoSize = true;
            this.buttonRemove.Location = new System.Drawing.Point(6, 78);
            this.buttonRemove.Name = "buttonRemove";
            this.buttonRemove.Size = new System.Drawing.Size(131, 23);
            this.buttonRemove.TabIndex = 2;
            this.buttonRemove.Text = "Remove selected image";
            this.buttonRemove.UseVisualStyleBackColor = true;
            this.buttonRemove.Click += new System.EventHandler(this.buttonRemove_Click);
            // 
            // buttonClear
            // 
            this.buttonClear.Location = new System.Drawing.Point(5, 107);
            this.buttonClear.Name = "buttonClear";
            this.buttonClear.Size = new System.Drawing.Size(75, 23);
            this.buttonClear.TabIndex = 3;
            this.buttonClear.Text = "Clear all images";
            this.buttonClear.UseVisualStyleBackColor = true;
            this.buttonClear.Click += new System.EventHandler(this.buttonClear_Click);
            // 
            // groupBoxImages
            // 
            this.groupBoxImages.Controls.Add(this.buttonAddDefault);
            this.groupBoxImages.Controls.Add(this.buttonAdd);
            this.groupBoxImages.Controls.Add(this.buttonClear);
            this.groupBoxImages.Controls.Add(this.buttonRemove);
            this.groupBoxImages.Location = new System.Drawing.Point(13, 13);
            this.groupBoxImages.Name = "groupBoxImages";
            this.groupBoxImages.Size = new System.Drawing.Size(147, 142);
            this.groupBoxImages.TabIndex = 0;
            this.groupBoxImages.TabStop = false;
            this.groupBoxImages.Text = "Images";
            // 
            // buttonAddDefault
            // 
            this.buttonAddDefault.AutoSize = true;
            this.buttonAddDefault.Location = new System.Drawing.Point(6, 20);
            this.buttonAddDefault.Name = "buttonAddDefault";
            this.buttonAddDefault.Size = new System.Drawing.Size(107, 23);
            this.buttonAddDefault.TabIndex = 0;
            this.buttonAddDefault.Text = "Add default images";
            this.buttonAddDefault.UseVisualStyleBackColor = true;
            this.buttonAddDefault.Click += new System.EventHandler(this.buttonAddDefault_Click);
            // 
            // groupBoxFilmstrip
            // 
            this.groupBoxFilmstrip.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxFilmstrip.Controls.Add(this.filmstripControl);
            this.groupBoxFilmstrip.Location = new System.Drawing.Point(167, 118);
            this.groupBoxFilmstrip.Name = "groupBoxFilmstrip";
            this.groupBoxFilmstrip.Size = new System.Drawing.Size(619, 398);
            this.groupBoxFilmstrip.TabIndex = 3;
            this.groupBoxFilmstrip.TabStop = false;
            this.groupBoxFilmstrip.Text = "Filmstrip control";
            // 
            // filmstripControl
            // 
            this.filmstripControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.filmstripControl.BackColor = System.Drawing.SystemColors.Control;
            this.filmstripControl.Location = new System.Drawing.Point(6, 19);
            this.filmstripControl.Name = "filmstripControl";
            this.filmstripControl.Size = new System.Drawing.Size(607, 373);
            this.filmstripControl.TabIndex = 0;
            this.filmstripControl.SelectionChanged += new System.EventHandler(this.filmstripControl_OnSelectionChanged);
            this.filmstripControl.SelectedImageDescriptionChanged += new System.EventHandler(this.filmstripControl_OnSelectedImageDescriptionChanged);
            // 
            // groupBoxSelection
            // 
            this.groupBoxSelection.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBoxSelection.Controls.Add(this.buttonUpdateDesc);
            this.groupBoxSelection.Controls.Add(this.textSelectedDesc);
            this.groupBoxSelection.Controls.Add(this.label4);
            this.groupBoxSelection.Controls.Add(this.label1);
            this.groupBoxSelection.Controls.Add(this.labelSelectedID);
            this.groupBoxSelection.Controls.Add(this.pictureSelected);
            this.groupBoxSelection.Controls.Add(this.comboImages);
            this.groupBoxSelection.Controls.Add(this.label2);
            this.groupBoxSelection.Location = new System.Drawing.Point(13, 161);
            this.groupBoxSelection.Name = "groupBoxSelection";
            this.groupBoxSelection.Size = new System.Drawing.Size(148, 355);
            this.groupBoxSelection.TabIndex = 2;
            this.groupBoxSelection.TabStop = false;
            this.groupBoxSelection.Text = "Selection details";
            // 
            // buttonUpdateDesc
            // 
            this.buttonUpdateDesc.Location = new System.Drawing.Point(6, 295);
            this.buttonUpdateDesc.Name = "buttonUpdateDesc";
            this.buttonUpdateDesc.Size = new System.Drawing.Size(75, 23);
            this.buttonUpdateDesc.TabIndex = 6;
            this.buttonUpdateDesc.Text = "Update";
            this.buttonUpdateDesc.UseVisualStyleBackColor = true;
            this.buttonUpdateDesc.Click += new System.EventHandler(this.buttonUpdateDesc_Click);
            // 
            // textSelectedDesc
            // 
            this.textSelectedDesc.Location = new System.Drawing.Point(6, 207);
            this.textSelectedDesc.Multiline = true;
            this.textSelectedDesc.Name = "textSelectedDesc";
            this.textSelectedDesc.Size = new System.Drawing.Size(134, 81);
            this.textSelectedDesc.TabIndex = 5;
            this.textSelectedDesc.Text = "desc";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 190);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(60, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "Description";
            // 
            // openFileDialog
            // 
            this.openFileDialog.Filter = "Image files (*.bmp; *.jpg; *.jpeg; *.gif; *.png)|*.bmp; *.jpg; *.jpeg; *.gif; *.p" +
                "ng|All files (*.*)|*.*";
            this.openFileDialog.Multiselect = true;
            this.openFileDialog.Title = "Select images";
            // 
            // groupBoxControlProperties
            // 
            this.groupBoxControlProperties.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxControlProperties.Controls.Add(this.comboHighlightColour);
            this.groupBoxControlProperties.Controls.Add(this.comboBackgroundColour);
            this.groupBoxControlProperties.Controls.Add(this.numericHoverDelay);
            this.groupBoxControlProperties.Controls.Add(this.label9);
            this.groupBoxControlProperties.Controls.Add(this.comboMainImageLayout);
            this.groupBoxControlProperties.Controls.Add(this.label8);
            this.groupBoxControlProperties.Controls.Add(this.comboThumbsLocation);
            this.groupBoxControlProperties.Controls.Add(this.label7);
            this.groupBoxControlProperties.Controls.Add(this.label6);
            this.groupBoxControlProperties.Controls.Add(this.label5);
            this.groupBoxControlProperties.Location = new System.Drawing.Point(168, 13);
            this.groupBoxControlProperties.Name = "groupBoxControlProperties";
            this.groupBoxControlProperties.Size = new System.Drawing.Size(618, 99);
            this.groupBoxControlProperties.TabIndex = 1;
            this.groupBoxControlProperties.TabStop = false;
            this.groupBoxControlProperties.Text = "Control properties";
            // 
            // comboHighlightColour
            // 
            this.comboHighlightColour.Color = System.Drawing.Color.DarkBlue;
            this.comboHighlightColour.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.comboHighlightColour.DropDownHeight = 1;
            this.comboHighlightColour.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboHighlightColour.DropDownWidth = 1;
            this.comboHighlightColour.FormattingEnabled = true;
            this.comboHighlightColour.IntegralHeight = false;
            this.comboHighlightColour.ItemHeight = 16;
            this.comboHighlightColour.Items.AddRange(new object[] {
            "Color",
            "Color",
            "Color",
            "Color",
            "Color",
            "Color",
            "Color",
            "Color",
            "Color",
            "Color",
            "Color"});
            this.comboHighlightColour.Location = new System.Drawing.Point(455, 17);
            this.comboHighlightColour.Name = "comboHighlightColour";
            this.comboHighlightColour.Size = new System.Drawing.Size(154, 22);
            this.comboHighlightColour.TabIndex = 7;
            this.comboHighlightColour.SelectedColorChanged += new System.EventHandler(this.comboHighlightColour_SelectedColorChanged);
            // 
            // comboBackgroundColour
            // 
            this.comboBackgroundColour.Color = System.Drawing.SystemColors.Control;
            this.comboBackgroundColour.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.comboBackgroundColour.DropDownHeight = 1;
            this.comboBackgroundColour.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBackgroundColour.DropDownWidth = 1;
            this.comboBackgroundColour.FormattingEnabled = true;
            this.comboBackgroundColour.IntegralHeight = false;
            this.comboBackgroundColour.ItemHeight = 16;
            this.comboBackgroundColour.Items.AddRange(new object[] {
            "Color",
            "Color",
            "Color",
            "Color",
            "Color",
            "Color",
            "Color",
            "Color",
            "Color",
            "Color",
            "Color"});
            this.comboBackgroundColour.Location = new System.Drawing.Point(127, 17);
            this.comboBackgroundColour.Name = "comboBackgroundColour";
            this.comboBackgroundColour.Size = new System.Drawing.Size(154, 22);
            this.comboBackgroundColour.TabIndex = 1;
            this.comboBackgroundColour.SelectedColorChanged += new System.EventHandler(this.comboBackgroundColour_SelectedColorChanged);
            // 
            // numericHoverDelay
            // 
            this.numericHoverDelay.Location = new System.Drawing.Point(127, 68);
            this.numericHoverDelay.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.numericHoverDelay.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericHoverDelay.Name = "numericHoverDelay";
            this.numericHoverDelay.Size = new System.Drawing.Size(77, 20);
            this.numericHoverDelay.TabIndex = 5;
            this.numericHoverDelay.Value = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.numericHoverDelay.ValueChanged += new System.EventHandler(this.numericHoverDelay_ValueChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(7, 71);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(113, 13);
            this.label9.TabIndex = 4;
            this.label9.Text = "Scroll hover delay time";
            // 
            // comboMainImageLayout
            // 
            this.comboMainImageLayout.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboMainImageLayout.FormattingEnabled = true;
            this.comboMainImageLayout.Location = new System.Drawing.Point(455, 42);
            this.comboMainImageLayout.Name = "comboMainImageLayout";
            this.comboMainImageLayout.Size = new System.Drawing.Size(154, 21);
            this.comboMainImageLayout.TabIndex = 9;
            this.comboMainImageLayout.SelectedIndexChanged += new System.EventHandler(this.comboMainImageLayout_SelectedIndexChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(326, 45);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(116, 13);
            this.label8.TabIndex = 8;
            this.label8.Text = "Main image layout style";
            // 
            // comboThumbsLocation
            // 
            this.comboThumbsLocation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboThumbsLocation.FormattingEnabled = true;
            this.comboThumbsLocation.Location = new System.Drawing.Point(127, 42);
            this.comboThumbsLocation.Name = "comboThumbsLocation";
            this.comboThumbsLocation.Size = new System.Drawing.Size(154, 21);
            this.comboThumbsLocation.TabIndex = 3;
            this.comboThumbsLocation.SelectedIndexChanged += new System.EventHandler(this.comboThumbsLocation_SelectedIndexChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(7, 45);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(107, 13);
            this.label7.TabIndex = 2;
            this.label7.Text = "Thumbs strip location";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(326, 20);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(123, 13);
            this.label6.TabIndex = 6;
            this.label6.Text = "Selected highlight colour";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(7, 20);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(97, 13);
            this.label5.TabIndex = 0;
            this.label5.Text = "Background colour";
            // 
            // FormFilmstripTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(798, 566);
            this.Controls.Add(this.groupBoxControlProperties);
            this.Controls.Add(this.groupBoxSelection);
            this.Controls.Add(this.groupBoxFilmstrip);
            this.Controls.Add(this.groupBoxImages);
            this.Controls.Add(this.buttonClose);
            this.Name = "FormFilmstripTest";
            this.ShowIcon = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.Text = "Filmstrip control test harness";
            this.Load += new System.EventHandler(this.FormFilmstripTest_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureSelected)).EndInit();
            this.groupBoxImages.ResumeLayout(false);
            this.groupBoxImages.PerformLayout();
            this.groupBoxFilmstrip.ResumeLayout(false);
            this.groupBoxSelection.ResumeLayout(false);
            this.groupBoxSelection.PerformLayout();
            this.groupBoxControlProperties.ResumeLayout(false);
            this.groupBoxControlProperties.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericHoverDelay)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Filmstrip.FilmstripControl filmstripControl;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelSelectedID;
        private System.Windows.Forms.PictureBox pictureSelected;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboImages;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.Button buttonAdd;
        private System.Windows.Forms.Button buttonRemove;
        private System.Windows.Forms.Button buttonClear;
        private System.Windows.Forms.GroupBox groupBoxImages;
        private System.Windows.Forms.GroupBox groupBoxFilmstrip;
        private System.Windows.Forms.GroupBox groupBoxSelection;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBoxControlProperties;
        private System.Windows.Forms.NumericUpDown numericHoverDelay;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox comboMainImageLayout;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox comboThumbsLocation;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private OfficePickers.ColorPicker.ComboBoxColorPicker comboHighlightColour;
        private OfficePickers.ColorPicker.ComboBoxColorPicker comboBackgroundColour;
        private System.Windows.Forms.Button buttonAddDefault;
        private System.Windows.Forms.Button buttonUpdateDesc;
        private System.Windows.Forms.TextBox textSelectedDesc;
    }
}

