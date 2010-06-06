using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Filmstrip;

namespace FilmStripTest
{
    public partial class FormFilmstripTest : Form
    {
        private const String NO_SELECTION = "No selection";

        public FormFilmstripTest()
        {
            InitializeComponent();
        }

        private void FormFilmstripTest_Load(object sender, EventArgs e)
        {
            PopulateImagesCombo();

            PopulateMainImageLayoutCombo();
            PopulateThumbsLocationCombo();

            comboThumbsLocation.Text = filmstripControl.ThumbsStripLocation.ToString();
            comboMainImageLayout.Text = filmstripControl.MainImageLayout.ToString();

            comboHighlightColour.SelectedItem = filmstripControl.SelectedHighlight;
            comboBackgroundColour.SelectedItem = filmstripControl.ControlBackground;
            numericHoverDelay.Value = filmstripControl.ScrollHoverDelay;

            UpdateSelectedInfo();
        }

        private void AddDefaultImages()
        {
            filmstripControl.AddImage(new FilmstripImage(Properties.Resources.MantaRay as Image, "Manta ray"));
            filmstripControl.AddImage(new FilmstripImage(Properties.Resources.CayoCoco as Image, "Cayo Coco beach, Cuba"));
            filmstripControl.AddImage(new FilmstripImage(Properties.Resources.SpitzkoppeDawn as Image, "Dawn at Spitzkoppe, Namibia"));
            filmstripControl.AddImage(new FilmstripImage(Properties.Resources.Zugspitze as Image, "Zugspitze, Germany"));
            filmstripControl.AddImage(Properties.Resources.ChobeSunset as Image, "Sunset at Chobe National Park, Zambia");
            filmstripControl.AddImage(Properties.Resources.OperaHouse as Image, "Sydney Opera House");
            filmstripControl.AddImage(Properties.Resources.TowerBridge as Image, "Tower Bridge, London");
            filmstripControl.AddImage(Properties.Resources.YosemiteFalls as Image, "Yosemite Falls, CA");

            SetButtonStates();
        }

        private void PopulateThumbsLocationCombo()
        {
            comboThumbsLocation.Items.Clear();

            foreach (String name in Enum.GetNames(typeof(FilmstripControl.StripLocation)))
            {
                comboThumbsLocation.Items.Add(name);
            }
        }

        private void PopulateMainImageLayoutCombo()
        {
            comboMainImageLayout.Items.Clear();

            foreach (String name in Enum.GetNames(typeof(ImageLayout)))
            {
                comboMainImageLayout.Items.Add(name);
            }
        }

        private void PopulateImagesCombo()
        {
            comboImages.Items.Clear();
            
            // Add 'No selection' to clear selection
            comboImages.Items.Add(NO_SELECTION);
            
            foreach (FilmstripImage image in filmstripControl.ImagesCollection)
            {
                comboImages.Items.Add(image.Id);
            }
        }

        private void filmstripControl_OnSelectionChanged(object sender, EventArgs e)
        {
            UpdateSelectedInfo();
        }

        private void UpdateSelectedInfo()
        {
            labelSelectedID.Text = filmstripControl.SelectedImageID.ToString();
            Image selection = filmstripControl.SelectedImage;
            if (null != selection)
            {
                pictureSelected.Image = filmstripControl.SelectedImage.GetThumbnailImage(pictureSelected.Width, pictureSelected.Height, null, IntPtr.Zero);
            }
            else
            {
                pictureSelected.Image = selection;
            }

            textSelectedDesc.Text = filmstripControl.SelectedImageDescription;

            if ((FilmstripControl.NO_SELECTION_ID == filmstripControl.SelectedImageID) && (!comboImages.Text.Equals(NO_SELECTION)))
            {
                comboImages.Text = NO_SELECTION;
            }
            else
            {
                comboImages.Text = filmstripControl.SelectedImageID.ToString();
            }

            SetButtonStates();
        }

        private void SetButtonStates()
        {
            buttonRemove.Enabled = (filmstripControl.SelectedImageID != FilmstripControl.NO_SELECTION_ID) ? true : false;
            buttonClear.Enabled = (filmstripControl.ImagesCollection.Length > 0) ? true : false;
            buttonUpdateDesc.Enabled = (filmstripControl.SelectedImageID != FilmstripControl.NO_SELECTION_ID) ? true : false;
        }

        private void comboImages_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectionID = FilmstripControl.NO_SELECTION_ID;

            if (!comboImages.SelectedItem.ToString().Equals(NO_SELECTION))
            {
                selectionID = Convert.ToInt32(comboImages.SelectedItem);
            }

            try
            {
                filmstripControl.SelectedImageID = selectionID;
            }
            catch
            {
                MessageBox.Show("Invalid image ID");
            }

            SetButtonStates();
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            filmstripControl.ClearAllImages();
            PopulateImagesCombo();
            SetButtonStates();
        }

        private void buttonRemove_Click(object sender, EventArgs e)
        {
            comboImages.Items.Remove(filmstripControl.SelectedImageID);
            filmstripControl.RemoveImage(filmstripControl.SelectedImageID);

            PopulateImagesCombo();
            SetButtonStates();
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            if (DialogResult.OK == openFileDialog.ShowDialog())
            {
                List<FilmstripImage> images = new List<FilmstripImage>();
                foreach (String file in openFileDialog.FileNames)
                {
                    Image thisImage = Image.FromFile(file);
                    FilmstripImage newImageObject = new FilmstripImage(thisImage, file);
                    images.Add(newImageObject);
                }
                filmstripControl.AddImageRange(images.ToArray());
                PopulateImagesCombo();
                SetButtonStates();
            }
        }

        private void filmstripControl_OnSelectedImageDescriptionChanged(object sender, EventArgs e)
        {
            textSelectedDesc.Text = filmstripControl.SelectedImageDescription;
            MessageBox.Show("Selected image description has been updated.");
        }

        private void numericHoverDelay_ValueChanged(object sender, EventArgs e)
        {
            filmstripControl.ScrollHoverDelay = (int)numericHoverDelay.Value;
        }

        private void comboThumbsLocation_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilmstripControl.StripLocation location =
                (FilmstripControl.StripLocation)Enum.Parse(typeof(FilmstripControl.StripLocation), comboThumbsLocation.SelectedItem.ToString());
            filmstripControl.ThumbsStripLocation = location;
            SetButtonStates();
        }

        private void comboMainImageLayout_SelectedIndexChanged(object sender, EventArgs e)
        {
            ImageLayout layout = (ImageLayout)Enum.Parse(typeof(ImageLayout), comboMainImageLayout.SelectedItem.ToString());
            filmstripControl.MainImageLayout = layout;
            SetButtonStates();
        }

        private void comboBackgroundColour_SelectedColorChanged(object sender, EventArgs e)
        {
            filmstripControl.ControlBackground = comboBackgroundColour.Color;
        }

        private void comboHighlightColour_SelectedColorChanged(object sender, EventArgs e)
        {
            filmstripControl.SelectedHighlight = comboHighlightColour.Color;
        }

        private void buttonAddDefault_Click(object sender, EventArgs e)
        {
            AddDefaultImages();
            PopulateImagesCombo();
        }

        private void buttonUpdateDesc_Click(object sender, EventArgs e)
        {
            filmstripControl.SelectedImageDescription = textSelectedDesc.Text;
            SetButtonStates();
        }

    }
}