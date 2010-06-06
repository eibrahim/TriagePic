using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using System.Collections;
using System.Design;
using System.ComponentModel.Design;
using System.Windows.Forms.Design;
using Filmstrip.Designer;


namespace Filmstrip
{

    /// <summary>
    /// The Filmstrip control.
    /// </summary>
    [ToolboxItem(true), 
    Description("A Filmstrip control for Windows forms."), 
    ToolboxBitmap(typeof(FilmstripControl), "FilmstripControl"), 
    DefaultEvent("OnSelectionChanged"), 
    DesignerAttribute(typeof(FilmstripControlDesigner), typeof(IDesigner))]
    public partial class FilmstripControl : UserControl
    {
        #region Constants

        // Constants
        internal const int scrollHoverDelayDefault = 500;
        internal const String CONTROL = "Control";
        internal const String DARKBLUE = "DarkBlue";
        internal const String STRETCH = "Stretch";
        internal const String CENTER = "Center";
        internal const String TOP = "Top";
        internal const String BOTTOM = "Bottom";
        internal const String BUTTON_RIGHT_TEXT = ">";
        internal const String BUTTON_LEFT_TEXT = "<";
        internal const String THUMB_PANEL_NAME = "PanelThumbnail";
        internal const String THUMB_NAME = "Thumbnail";
        internal const Keys CTRL_D = (Keys.Control | Keys.D);
        // Display strings should be in a resources file
        internal const String IMAGE_ERROR = "Image error";
        internal const String TIP_SCROLL_LEFT = "Scroll left";
        internal const String TIP_SCROLL_RIGHT = "Scroll right";
        internal const String TIP_SCROLLING_LEFT = "Scrolling left...";
        internal const String TIP_SCROLLING_RIGHT = "Scrolling right...";
        internal const String TIP_THUMB_EXTRA = "{0} (Image {1} of {2})";
        internal const String ERROR_CLICK_IMAGE_TEXT = "An error has occurred displaying the selected image.";
        internal const String ERROR_TITLE = "Error";
        internal const String MODIFY_DESCRIPTION = "\n[Double-click or {0} to edit.]";

        // The thumbnail size and seperator are fixed values...
        internal const int thumbnailSeperatorSize = 20;
        internal const int thumbnailPanelWidth = 75;
        internal const int thumbnailPanelMargin = 10;
        internal const int thumbnailImageMargin = 5;


        #endregion Constants

        #region Member variables

        internal static int imageIdCounter = 1;
        public static int NO_SELECTION_ID = -1;

        public enum StripLocation
        {
            Top = 1,
            Bottom = 2
        };

        // Members for properties
        private Color controlBackgroundColor;
        private ImageLayout mainImageLayout;
        private int scrollHoverDelay;
        private Image selectedImage;
        private int selectedImageID;
        private Image imageNavigationLeft;
        private Image imageNavigationRight;
        private ImageLayout imageNavigationLeftLayout;
        private ImageLayout imageNavigationRightLayout;
        private Color selectedHighlightColor;
        private StripLocation thumbsStripLocation;
        private Keys editDescriptionAccelerator;

        // Other members
        private int thumbsStartIndex;
        private double scaleRatio;
        bool loaded;

        private ToolTip toolTip;
        private Control hoverControl;
        private Timer hoverTimer;

        private List<int> keys;
        private Dictionary<int, FilmstripImage> imagesCollection;

        private List<Button> thumbnailControls;

        #endregion Member variables

        #region Properties

        /// <summary>
        /// The image to be used for the left navigation button. If no image is specified the button text will have  '&lt;'.
        /// </summary>
        [Category("Filmstrip properties")]
        [Description("The image to be used for the left navigation button. If no image is specified the button text will have '<'.")]
        [DefaultValue(null)]
        public Image ImageNavLeft
        {
            get { return imageNavigationLeft; }
            set
            {
                // Update the property
                imageNavigationLeft = value;
                // Update the button
                buttonLeft.BackgroundImage = value;
                // Set the button text if required
                if (null == value)
                {
                    buttonLeft.Text = BUTTON_LEFT_TEXT;
                }
                else
                {
                    buttonLeft.Text = String.Empty;
                }
            }
        }

        /// <summary>
        /// The image layout used for the left navigation button image, if an image is present. Default value is 'Stretch'.
        /// </summary>
        [Category("Filmstrip properties")]
        [Description("The image layout used for the left navigation button image, if an image is present. Default value is 'Stretch'.")]
        [DefaultValue(typeof(ImageLayout), STRETCH)]
        public ImageLayout ImageNavLeftLayout
        {
            get { return imageNavigationLeftLayout; }
            set
            {
                // Set the value
                imageNavigationLeftLayout = value;
                // Update the button property
                buttonLeft.BackgroundImageLayout = imageNavigationLeftLayout;
            }
        }

        /// <summary>
        /// The image to be used for the right navigation button. If no image is specified the button text will have '&gt;'.
        /// </summary>
        [Category("Filmstrip properties")]
        [Description("The image to be used for the right navigation button. If no image is specified the button text will have '>'.")]
        [DefaultValue(null)]
        public Image ImageNavRight
        {
            get { return imageNavigationRight; }
            set 
            {
                // Update the property
                imageNavigationRight = value;
                // Update the button
                buttonRight.BackgroundImage = value;
                // Set the button text if required
                if (null == value)
                {
                    buttonRight.Text = BUTTON_RIGHT_TEXT;
                }
                else
                {
                    buttonRight.Text = String.Empty;
                }
            }
        }

        /// <summary>
        /// The image layout used for the right navigation button image, if an image is present. Default value is 'Stretch'.
        /// </summary>
        [Category("Filmstrip properties")]
        [Description("The image layout used for the right navigation button image, if an image is present. Default value is 'Stretch'.")]
        [DefaultValue(typeof(ImageLayout), STRETCH)]
        public ImageLayout ImageNavRightLayout
        {
            get { return imageNavigationRightLayout; }
            set
            {
                // Set the value
                imageNavigationRightLayout = value;
                // Update the button property
                buttonRight.BackgroundImageLayout = imageNavigationRightLayout;
            }
        }

        /// <summary>
        /// The delay interval (in milliseconds) used before scolling the thumbnail view whenever the mouse hovers over a navigation button. Default value is 500ms.
        /// </summary>
        [Category("Filmstrip properties")]
        [Description("The delay interval (in milliseconds) used before scolling the thumbnail view whenever the mouse hovers over a navigation button. Default value is 500ms.")]
        [DefaultValue(500)]
        public int ScrollHoverDelay
        {
            get { return scrollHoverDelay; }
            set 
            { 
                // Set the value
                scrollHoverDelay = value;
                // Update our timer object
                hoverTimer.Interval = scrollHoverDelay;
            }
        }

        /// <summary>
        /// "he image layout for the image selected into the main viewing area. Default value is 'Center'.
        /// </summary>
        [Category("Filmstrip properties")]
        [Description("The image layout for the image selected into the main viewing area. Default value is 'Center'.")]
        [DefaultValue(typeof(ImageLayout), CENTER)]
        public ImageLayout MainImageLayout
        {
            get { return mainImageLayout; }
            set 
            { 
                // Set the value
                mainImageLayout = value;
                // Update the main picture panel
                mainPicture.BackgroundImageLayout = mainImageLayout;
            }
        }

        /// <summary>
        /// The background colour of all items on the filmstrip control. Default value is 'Control'.
        /// </summary>
        [Category("Filmstrip properties")]
        [Description("The background colour of all items on the filmstrip control. Default value is 'Control'.")]
        [DefaultValue(typeof(Color), CONTROL)]
        public Color ControlBackground
        {
            get { return controlBackgroundColor; }
            set
            {
                // Set the value
                controlBackgroundColor = value;
                // Update all child controls
                BackColor = controlBackgroundColor;
                mainPicture.BackColor = controlBackgroundColor;
                buttonLeft.BackColor = controlBackgroundColor;
                buttonRight.BackColor = controlBackgroundColor;
                panelNavigation.BackColor = controlBackgroundColor;
                // And the dynamically created thumbnails
                int selectedIndex = GetSelectedThumbIndex();
                for (int x = 0; x < thumbnailControls.Count; x++ )
                {
                    // Get this thumb
                    Button thumb = thumbnailControls[x];
                    // Update the background colour
                    thumb.BackColor = controlBackgroundColor;
                    // And update it's parent pabel, but not for the current selection
                    if (selectedIndex != x)
                    {
                        thumb.Parent.BackColor = controlBackgroundColor;
                    }
                }
            }
        }

        /// <summary>
        /// The colour of the highlight around the selected thumbnail image. Default value is 'DarkBlue'.
        /// </summary>
        [Category("Filmstrip properties")]
        [Description("The colour of the highlight around the selected thumbnail image. Default value is 'DarkBlue'.")]
        [DefaultValue(typeof(Color), DARKBLUE)]
        public Color SelectedHighlight
        {
            get { return selectedHighlightColor; }
            set
            {
                // Set the value
                selectedHighlightColor = value;
                // Do we have any current selection to worry about?
                int selectedIndex = GetSelectedThumbIndex();
                if (selectedIndex >= 0)
                {
                    // Yep... so update it.
                    Button selectedNail = thumbnailControls[selectedIndex];
                    selectedNail.Parent.BackColor = selectedHighlightColor;
                }
            }
        }

        /// <summary>
        /// The location of the strip of thumbnail images, either 'Top' or 'Bottom'. Default value is 'Bottom'.
        /// </summary>
        [Category("Filmstrip properties")]
        [Description("The location of the strip of thumbnail images, either 'Top' or 'Bottom'. Default value is 'Bottom'.")]
        [DefaultValue(typeof(FilmstripControl.StripLocation), BOTTOM)]
        public FilmstripControl.StripLocation ThumbsStripLocation
        {
            get { return thumbsStripLocation; }
            set
            {
                thumbsStripLocation = value;

                // Also change the dock syle of the thumbs strip and the main picture location
                switch (thumbsStripLocation)
                {
                    case FilmstripControl.StripLocation.Bottom:
                        panelNavigation.Dock = DockStyle.Bottom;
                        mainPicture.Top = 0;
                        break;
                    case FilmstripControl.StripLocation.Top:
                        panelNavigation.Dock = DockStyle.Top;
                        mainPicture.Top = panelNavigation.Bottom;
                        break;
                    default:
                        // Do nothing
                        break;
                }
            }
        }

        /// <summary>
        /// The description of the image currently selected in the control.
        /// </summary>
        [Category("Filmstrip properties")]
        [Description("The description of the image currently selected in the control.")]
        [Browsable(false)]
        [ReadOnly(true)]
        public String SelectedImageDescription
        {
            get 
            {
                // Do we have a valid selection?
                if (imagesCollection.ContainsKey(selectedImageID))
                {
                    // Get it!
                    return imagesCollection[selectedImageID].Description;
                }
                else
                {
                    // Nothing to see here...
                    return String.Empty;
                }
            }
            set
            {
                // Any selection?
                if (NO_SELECTION_ID == selectedImageID)
                { 
                    // Nope!
                    throw new InvalidOperationException("Cannot set the description when no image is selected");
                }

                // Yep... is it valid?
                if (imagesCollection.ContainsKey(selectedImageID))
                {
                    // Yep. :)
                    UpdateSelectedDescription(value);
                }
                else
                {
                    // Nope. :(
                    throw new InvalidOperationException(String.Format("Cannot set the description as selected image id ({0}) is invalid", selectedImageID));
                }
            }
        }

        /// <summary>
        /// The image currently selected in the control.
        /// </summary>
        [Category("Filmstrip properties")]
        [Description("The image currently selected in the control.")]
        [Browsable(false)]
        public Image SelectedImage
        {
            get { return selectedImage; }
        }

        /// <summary>
        /// The image id of the image currently selected in the control.
        /// If -1 is specified then any selection is cleared.
        /// </summary>
        /// <exception cref="System.SystemException.ArgumentOutOfRangeException">System.SystemException.ArgumentOutOfRangeException - 
        /// if the image id is not in the control's collection</exception>
        [Category("Filmstrip properties")]
        [Description("The image id of the image currently selected in the control.")]
        [Browsable(false)]
        [ReadOnly(true)]
        public int SelectedImageID
        {
            get { return selectedImageID; }
            set
            {
                // What selection?
                if (NO_SELECTION_ID == value)
                {
                    // None...
                    ClearSelection();
                }
                else if (imagesCollection.ContainsKey(value))
                {
                    // Valid inmage id
                    SelectThumbnail(value);
                }
                else
                {
                    // Ooops
                    throw new ArgumentOutOfRangeException();
                }
            }
        }

        /// <summary>
        /// The images collection. Setting this property will remove any images currently in the collection.
        /// </summary>
        /// <exception cref="System.SystemException.ArgumentException">System.SystemException.ArgumentException - if any of the 
        /// the image ids already exist in the collection</exception>
        /// <exception cref="System.SystemException.ArgumentOutOfRangeException">System.SystemException.ArgumentOutOfRangeException - 
        /// if any of the image ids are negative</exception>
        [Category("Filmstrip properties")]
        [Description("The images collection. Setting this property will remove any images currently in the collection.")]
        [Browsable(false)]
        [ReadOnly(true)]
        public FilmstripImage[] ImagesCollection
        {
            get
            {
                List<FilmstripImage> images = new List<FilmstripImage>();
                foreach (FilmstripImage thisOne in imagesCollection.Values)
                {
                    images.Add(thisOne);
                }
                return images.ToArray();
            }
            set
            {
                ClearAllImages();

                AddImageRange(value);
            }
        }

        /// <summary>
        /// Sets the accelerator key combination to display the edit description form for the currently selected image.
        /// Default is Ctrl+D.
        /// </summary>
        [Category("Filmstrip properties")]
        [Description("Sets the accelerator key combination to display the edit description form for the currently selected image. Default is Ctrl+D.")]
        [DefaultValue(CTRL_D)]
        public Keys EditDescriptionAccelerator
        {
            get { return editDescriptionAccelerator; }
            set { editDescriptionAccelerator = value; }
        }

        #endregion Properties

        #region Property Events

        /// <summary>
        /// Event handler fired when the image selected in the filmstrip control changes.
        /// </summary>
        [Category("Filmstrip events")]
        [Description("Event handler fired when the image selected in the filmstrip control changes.")]
        public event EventHandler SelectionChanged;

        /// <summary>
        /// Event handler fired when the description of the image selected in the filmstrip control changes.
        /// </summary>
        [Category("Filmstrip events")]
        [Description("Event handler fired when the description of the image selected in the filmstrip control changes.")]
        public event EventHandler SelectedImageDescriptionChanged;

        #endregion Property Events

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public FilmstripControl()
        {
            InitializeComponent();

            thumbsStartIndex = 0;
            scaleRatio = 1.0;

            keys = new List<int>();
            imagesCollection = new Dictionary<int, FilmstripImage>();

            thumbnailControls = new List<Button>();

            // Set default property values
            selectedImage = null;
            selectedImageID = NO_SELECTION_ID;
            ControlBackground = SystemColors.Control;
            selectedHighlightColor = Color.DarkBlue;
            scrollHoverDelay = scrollHoverDelayDefault;
            mainImageLayout = ImageLayout.Center;
            imageNavigationLeftLayout = ImageLayout.Stretch;
            imageNavigationRightLayout = ImageLayout.Stretch;
            thumbsStripLocation = FilmstripControl.StripLocation.Bottom;
            editDescriptionAccelerator = CTRL_D;

            toolTip = new ToolTip();

            hoverControl = null;

            hoverTimer = new Timer();
            hoverTimer.Interval = scrollHoverDelay;
            hoverTimer.Tick += new EventHandler(hoverTimer_Tick);

            loaded = false;
        }

        #endregion Constructor

        #region Events
        
        /// <summary>
        /// Form load event - populates the form with any initial data.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LayoutScreenshotFilmstrip_Load(object sender, EventArgs e)
        {
            // Populate ourselves
            Populate();

            // And now we're ready
            loaded = true;
        }

        /// <summary>
        /// Handler for when the control is resized
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FilmstripControl_Resize(object sender, EventArgs e)
        {
            // Only if we're done with loading
            if (loaded)
            {
                AdjustThumbnails();

                // Do we have a selection?
                if (null != selectedImage)
                {
                    // Yep... better resize the main image too
                    mainPicture.BackgroundImage = ScaleImage(selectedImage, mainPicture.Width, mainPicture.Height);
                }

                // Update the button states
                SetButtonStates();
            }
        }

        /// <summary>
        /// Handler for the when the right navigation button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonRight_Click(object sender, EventArgs e)
        {
            // Stop any timer currently running
            StopHoverTimer();
            
            // Shift the thumbnails to the right one place
            MoveThumbnailsRight(1);
        }

        /// <summary>
        /// Handler for the when the left navigation button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonLeft_Click(object sender, EventArgs e)
        {
            // Stop any timer currently running
            StopHoverTimer();

            // Shift the thumbnails to the left  one place
            MoveThumbnailsLeft(1);
        }

        /// <summary>
        /// Handler for the MouseHover event on either of the navigation buttons
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonNav_MouseHover(object sender, EventArgs e)
        {
            // Start the timer for this navigation button
            StartHoverTimer(sender);
        }

        /// <summary>
        /// Handler for the MouseLeave event for either of the navigation buttons
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonNav_MouseLeave(object sender, EventArgs e)
        {
            // Stop the timer
            StopHoverTimer();
        }

        /// <summary>
        /// Timer interval event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void hoverTimer_Tick(object sender, EventArgs e)
        {
            // Which button are we over?
            // Update the tooltip and move the correct way
            if (hoverControl == buttonLeft)
            {
                toolTip.SetToolTip(hoverControl, TIP_SCROLLING_LEFT);
                MoveThumbnailsLeft(1);
            }
            else if (hoverControl == buttonRight)
            {
                toolTip.SetToolTip(hoverControl, TIP_SCROLLING_RIGHT);
                MoveThumbnailsRight(1);
            }
        }

        /// <summary>
        /// Handler for when a thumbnail image is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Thumbnail_Click(object sender, EventArgs e)
        {
            // Get the image id off the control
            Button boxSender = sender as Button;
            // We populated this so the image id should be fine... but just in case.
            try
            {
                // Get the ID
                int imageID = Convert.ToInt32(boxSender.Tag);
                // Select this one
                SelectThumbnail(imageID);
            }
            catch (InvalidCastException)
            {
                // Should never get in here... but just in case
                MessageBox.Show(ERROR_CLICK_IMAGE_TEXT, ERROR_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Handler for when the main image is double-clicked.
        /// Displays the image desciption form to view/amend the description
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureMain_DoubleClick(object sender, EventArgs e)
        {
            EditSelectedDescription();
        }

        /// <summary>
        /// Handler for the KeyDown event for all controls with keyboard access.
        /// Edits the description of the selected image
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FilmstripControl_KeyDown(object sender, KeyEventArgs e)
        {
            Keys keyEvent = e.KeyCode | e.Modifiers;

            if (editDescriptionAccelerator == keyEvent)
            {
                EditSelectedDescription();
            }
            else
            {
                switch (keyEvent)
                {
                    case Keys.PageUp:
                        MoveThumbnailsLeft(ThumbsForWidth());
                        break;
                    case Keys.PageDown:
                        MoveThumbnailsRight(ThumbsForWidth());
                        break;
                    case Keys.Home:
                        EnsureThumbIsVisible(keys[0]);
                        break;
                    case Keys.End:
                        EnsureThumbIsVisible(keys[keys.Count - 1]);
                        break;
                    default:
                        // Do nothing
                        break;
                }
            }
        }

        #endregion Events

        #region Public Actions

        /// <summary>
        /// Adds an image to the collection
        /// </summary>
        /// <param name="id">The image id</param>
        /// <param name="image">The image</param>
        /// <param name="text">The image description</param>
        /// <returns>The ID value of the added image.
        /// If the orginally supplied id value was negative then a new ID value is created.</returns>
        /// <exception cref="System.SystemException.ArgumentException">System.SystemException.ArgumentException - 
        /// if the image id already exists in the collection</exception>
        /// <exception cref="System.SystemException.ArgumentOutOfRangeException">System.SystemException.ArgumentOutOfRangeException - 
        /// if the image id is -1</exception>
        public int AddImage(int id, Image image, String text)
        {
            FilmstripImage newImage = new FilmstripImage(id, image, text);
            return AddImage(newImage);
        }

        /// <summary>
        /// Adds an image to the collection
        /// </summary>
        /// <param name="image">The image</param>
        /// <param name="text">The image description</param>
        /// <returns>The ID value of the added image.
        /// If the orginally supplied id value was negative then a new ID value is created.</returns>
        /// <exception cref="System.SystemException.ArgumentException">System.SystemException.ArgumentException - 
        /// if the image id already exists in the collection</exception>
        public int AddImage(Image image, String text)
        {
            FilmstripImage newImage = new FilmstripImage(image, text);
            return AddImage(newImage);
        }

        /// <summary>
        /// Adds an image to the collection
        /// </summary>
        /// <param name="newImage">Images to add to the collection.</param>
        /// <returns>The ID value of the added image.
        /// If the orginally supplied id value was negative then a new ID value is created.</returns>
        /// <exception cref="System.SystemException.ArgumentException">System.SystemException.ArgumentException - 
        /// if any of the image ids already exist in the collection</exception>
        public int AddImage(FilmstripImage newImage)
        {
            // Create new ID if not supplied
            if (NO_SELECTION_ID == newImage.Id)
            {
                newImage.Id = GetUniqueImageID();
            }

            // Add to the collection - throws an exception if the image id already exists in the collection
            imagesCollection.Add(newImage.Id, newImage); 

            // Add the key
            keys.Add(newImage.Id);

            // Repopulate
            //PopulateThumbnails();
            Populate();

            return newImage.Id;
        }

        /// <summary>
        /// Adds a collection of images to the collection
        /// </summary>
        /// <param name="images">Array of images to add to the collection.</param>
        /// <exception cref="System.SystemException.ArgumentException">System.SystemException.ArgumentException - 
        /// if any of the image ids already exist in the collection</exception>
        public void AddImageRange(FilmstripImage[] images)
        {
            Cursor.Current = Cursors.WaitCursor;

            foreach (FilmstripImage image in images)
            {
                // Create new ID if not supplied
                if (NO_SELECTION_ID == image.Id)
                {
                    image.Id = GetUniqueImageID();
                }

                // Add to the collection - throws an exception if the image id already exists in the collection
                imagesCollection.Add(image.Id, image);

                // Add the key
                keys.Add(image.Id);
            }

            // Repopulate
            //PopulateThumbnails();
            Populate();

            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// Removes the specified image from the collection
        /// </summary>
        /// <param name="id">The id of the image to remove</param>
        /// <exception cref="System.SystemException.ArgumentException">System.SystemException.ArgumentException - 
        /// if the image id does not exist in the collection</exception>
        /// <exception cref="System.SystemException.ArgumentOutOfRangeException">System.SystemException.ArgumentOutOfRangeException - 
        /// if the image id is -1</exception>
        public void RemoveImage(int id)
        {
            if (NO_SELECTION_ID == id)
            {
                throw new ArgumentOutOfRangeException(String.Format("{0} is an invalid image id, it cannot be removed.", NO_SELECTION_ID.ToString()));
            }

            // Do we have this image id?
            if (imagesCollection.ContainsKey(id))
            {
                // Clear selection if needed
                if (id == selectedImageID)
                {
                    ClearSelection();
                }

                // Remove it from the collections
                imagesCollection.Remove(id);
                keys.Remove(id);

                // Repopulate
                //PopulateThumbnails();
                Populate();
            }
            else
            {
                throw new ArgumentException(String.Format("The supplied image id ({0}) does not exist in the collection.", id.ToString()));
            }
        }

        /// <summary>
        /// Clears all images from the form
        /// </summary>
        public void ClearAllImages()
        {
            // Reset selection
            ClearSelection();
            // Clear all our collections
            imagesCollection.Clear();
            keys.Clear();
            thumbsStartIndex = 0;
            // Repopulate the thumbs strip
            Populate();
        }

        /// <summary>
        /// Clear the current image selection
        /// </summary>
        public void ClearSelection()
        {
            foreach (Button thisNail in thumbnailControls)
            {
                thisNail.Parent.BackColor = ControlBackground;
            }

            // Reset the selection
            mainPicture.BackgroundImage = null;
            selectedImageID = NO_SELECTION_ID;
            selectedImage = null;
            toolTip.SetToolTip(mainPicture, String.Empty);
            // Fire the event to say it changed
            EventHandler handler = SelectionChanged;
            if (null != handler)
            {
                handler(this as object, new EventArgs());
            }
        }

        /// <summary>
        /// Ensures that the thumb for the specified image is visible.
        /// This does not affect the current selection in any way.
        /// </summary>
        /// <param name="imageID">The image id of the thumb to make visible</param>
        /// <exception cref="ArgumentException">ArgumentException if the image id is invalid</exception>
        public void EnsureThumbIsVisible(int imageID)
        {
            // Valid id?
            int keyIndex = keys.IndexOf(imageID);
            if ((NO_SELECTION_ID == imageID) || (keyIndex < 0))
            {
                throw new ArgumentException(String.Format("The supplied image id ({0}) does not exist in the collection.", imageID.ToString()));
            }

            // Move until it's visible
            while (!IsThumbVisible(imageID))
            {
                // Which way to move?
                int startIndex = keys.IndexOf(Convert.ToInt32(thumbnailControls[0].Tag));
                int endIndex = keys.IndexOf(Convert.ToInt32(thumbnailControls[thumbnailControls.Count - 1].Tag));
                if (keyIndex > endIndex)
                {
                    MoveThumbnailsRight(1);
                }
                else if (keyIndex < startIndex)
                {
                    MoveThumbnailsLeft(1);
                }
            }
        }

        #endregion Public Actions

        #region Private Actions

        /// <summary>
        /// Returns a new unique ID value.
        /// Uses our static counter
        /// </summary>
        /// <returns>Unique ID value for use as new image id</returns>
        private int GetUniqueImageID()
        {
            int newID = imageIdCounter++;
            while (keys.Contains(newID))
            {
                newID = imageIdCounter++;
            }
            return newID;
        }

        /// <summary>
        /// Displays the Description form for the currently selected image
        /// </summary>
        private void EditSelectedDescription()
        {
            // If we have no selection... nothing to do...
            if (NO_SELECTION_ID == SelectedImageID)
            {
                return;
            }

            Filmstrip.FormImageDescription formDescription = new Filmstrip.FormImageDescription();
            formDescription.ImageDescription = SelectedImageDescription;
            if (DialogResult.OK == formDescription.ShowDialog())
            {
                UpdateSelectedDescription(formDescription.ImageDescription);
            }
        }

        /// <summary>
        /// Updates the description on the currently selected image.
        /// Also updates the appropriate tooltips and fires the event.
        /// </summary>
        /// <param name="newDescription">The new description.</param>
        private void UpdateSelectedDescription(String newDescription)
        {
            // Set the new description
            imagesCollection[selectedImageID].Description = newDescription;

            // Now update the tootips...
            // Main picture
            toolTip.SetToolTip(mainPicture, newDescription + GetModifyDescriptionTooltip());
            // Thumb, if visible
            if (IsThumbVisible(selectedImageID))
            {
                int selectedIndex = GetSelectedThumbIndex();
                Button thisNail = thumbnailControls[GetSelectedThumbIndex()];
                // Add the text "(Image x of y.)" to the tooltip on the thumb
                String text = String.Format(TIP_THUMB_EXTRA, newDescription, selectedIndex + 1, imagesCollection.Count);
                toolTip.SetToolTip(thisNail, text);
            }

            // Fire the event
            EventHandler handler = SelectedImageDescriptionChanged;
            if (null != SelectedImageDescriptionChanged)
            {
                handler(this as object, new EventArgs());
            }
        }

        /// <summary>
        /// Formats the text used that is appended to the main image tooltip so that the current
        /// accelerator command is displayed.
        /// </summary>
        /// <returns>String representation of the current EditDescriptionAccelerator value</returns>
        private String GetModifyDescriptionTooltip()
        {
            KeysConverter converter = new KeysConverter();
            String acceleratorString = converter.ConvertToString(editDescriptionAccelerator);
            return String.Format(MODIFY_DESCRIPTION, acceleratorString);
        }

        /// <summary>
        /// Changes the thumbnails in the event of the control being resized
        /// </summary>
        private void AdjustThumbnails()
        {
            // Right what happened...?
            // Did we get bigger, and we have't yet created thumbs for all the images we have?
            if ((thumbnailControls.Count < ThumbsForWidth()) && (thumbnailControls.Count < imagesCollection.Count))
            {
                while ((thumbnailControls.Count < ThumbsForWidth()) && (thumbnailControls.Count < imagesCollection.Count))
                {
                    // Get the image id of the current last one
                    int imageIDOfLast = NO_SELECTION_ID;
                    if (thumbnailControls.Count > 0)
                    {
                        imageIDOfLast = Convert.ToInt32(thumbnailControls[thumbnailControls.Count - 1].Tag.ToString());
                    }
                    // Add a new one
                    int newIndex = PushThumbnail();
                    // If we're already at the right hand end, we need to shift all the image across
                    // (as if the left navigation button was clicked)
                    if ((NO_SELECTION_ID != imageIDOfLast) && (keys[keys.Count - 1] == imageIDOfLast))
                    {
                        MoveThumbnailsLeft(1);
                    }
                    else
                    {
                        // Set the image on this new thumb
                        PopulateThumbnail(newIndex);
                    }
                }
            }
            // Did the control get smaller?
            else if (thumbnailControls.Count > ThumbsForWidth())
            {
                while (thumbnailControls.Count > ThumbsForWidth())
                {
                    PopThumbnail();
                }
            }

            SetButtonStates();
        }

        /// <summary>
        /// Resets the tooltip control with the default tips.
        /// This is nedded beacuse when the thumbs are deleted and then re-created 
        /// the tooltip control gets confused and the tips do not appear.
        /// </summary>
        private void ResetTooltips()
        {
            toolTip.RemoveAll();
            // Nav buttons
            toolTip.SetToolTip(buttonLeft, TIP_SCROLL_LEFT);
            toolTip.SetToolTip(buttonRight, TIP_SCROLL_RIGHT);
        }

        /// <summary>
        /// Populates the filmstrip control
        /// </summary>
        private void Populate()
        {
            // Set the tooltips on the navigation buttons
            ResetTooltips();
            // Create the thumbnail list
            CreateFilmstripThumbnails();
            // Populate
            PopulateThumbnails();
            // Update the button states
            SetButtonStates();
        }

        /// <summary>
        /// Populates the thumbnails in the thumbnail stripand maintains the any current selection.
        /// </summary>
        private void PopulateThumbnails()
        {
            // If we have some images
            if (imagesCollection.Count > 0)
            {
                //Modify the start index if we've just removed from the end, if necessary of course
                while (((imagesCollection.Count - thumbsStartIndex) < thumbnailControls.Count) && (thumbsStartIndex > 0))
                {
                    thumbsStartIndex--;
                }

                // Add the images to the thumb controls, starting at our start index
                int thumbIndex = 0;
                for (int startIndex = thumbsStartIndex; (startIndex < imagesCollection.Count) && (thumbIndex < thumbnailControls.Count); startIndex++, thumbIndex++)
                {
                    // Populate the Button
                    Button thisNail = thumbnailControls[thumbIndex];
                    thisNail.Tag = keys[startIndex];
                    thisNail.Visible = true;

                    // Update any selection colour
                    if (keys[startIndex] == selectedImageID)
                    {
                        thisNail.Parent.BackColor = selectedHighlightColor;
                    }
                    else
                    {
                        thisNail.Parent.BackColor = controlBackgroundColor;
                    }

                    // Try to get the image
                    FilmstripImage image = null;
                    if (imagesCollection.TryGetValue(keys[startIndex], out image))
                    {
                        // Yay... make sure we have no error label
                        ClearThumbnailError(thumbIndex);

                        // Get the thumbnail for the image
                        thisNail.Image = image.Image.GetThumbnailImage(thisNail.Width, thisNail.Height, 
                                                                       ThumbnailCallback, IntPtr.Zero);
                    }
                    else
                    {
                        // We should never really get to this error state but just in case we create a label on this 
                        // thumb to indicate the problem
                        SetThumbnailError(thumbIndex);
                    }

                    // Set the Tooltip text
                    int imageID = Convert.ToInt32(thisNail.Tag);
                    String text = imagesCollection[imageID].Description;
                    // Add the text "(Image x of y.)" to the tooltip on the thumb
                    text = String.Format(TIP_THUMB_EXTRA, text, startIndex + 1, imagesCollection.Count);
                    toolTip.SetToolTip(thisNail, text);
                }
            }

            // Update the navigation button states
            SetButtonStates();
        }

        /// <summary>
        /// Removes the error info from the specified thumbnail
        /// </summary>
        /// <param name="thumbIndex">The index of the thumbnail</param>
        private void ClearThumbnailError(int thumbIndex)
        {
            // Remove any label that might be there
            Button thisNail = thumbnailControls[thumbIndex];
            if (thisNail.Controls.Count > 0)
            {
                thisNail.Controls.Clear();
            }
            thisNail.TabStop = true;
        }

        /// <summary>
        /// Sets error information on the thumbnail.
        /// Used when the image for the thumb cannot be loaded / retrieved.
        /// </summary>
        /// <param name="thumbIndex">The index of the thumbnail</param>
        private void SetThumbnailError(int thumbIndex)
        {
            Button thisNail = thumbnailControls[thumbIndex];

            // Remove any existing children first
            if (thisNail.Controls.Count > 0)
            {
                thisNail.Controls.Clear();
            }
            // We're not a tab stop any more
            thisNail.TabStop = false;

            // Create and add the label
            Label label = new Label();
            label.Text = IMAGE_ERROR;
            label.TextAlign = ContentAlignment.MiddleCenter;
            label.Dock = DockStyle.Fill;
            thisNail.Controls.Add(label);
        }

        /// <summary>
        /// Callback function required for the use of GetThumbnailImage function
        /// </summary>
        /// <returns></returns>
        private bool ThumbnailCallback()
        {
            // Nothing to do here.
            return false;
        }

        /// <summary>
        /// Selects the specified thumbnail
        /// </summary>
        /// <param name="thumbID">The image id</param>
        /// <exception cref="System.SystemException.ArgumentException">System.SystemException.ArgumentException - 
        /// if the image id does not exist in the collection</exception>
        /// <exception cref="System.SystemException.ArgumentOutOfRangeException">System.SystemException.ArgumentOutOfRangeException - 
        /// if the image id is -1</exception>
        private void SelectThumbnail(int imageID)
        {
            // Do we have a valid id?
            if (NO_SELECTION_ID == imageID)
            {
                // Nope... but we should never be in here as the id should have been checked previously
                throw new ArgumentOutOfRangeException(String.Format("{0} is an invalid image id, it cannot be selected.", imageID.ToString()));
            }
            if (!keys.Contains(imageID))
            {
                // Nope... but we should never be in here as the id should have been checked previously
                throw new ArgumentException(String.Format("The supplied image id ({0}) does not exist in the collection.", imageID.ToString()));
            }

            // Make sure it's visible before we select it
            EnsureThumbIsVisible(imageID);
         
            // Get the thumb control index for this image
            int thumbIndex = GetThumbIndex(imageID);

            // Remove any existing highlighting first
            int selectedIndex = GetSelectedThumbIndex();
            if ((selectedIndex >= 0) && (selectedIndex != thumbIndex))
            {
                // Yep... so remove its selected border
                thumbnailControls[selectedIndex].Parent.BackColor = ControlBackground;
            }

            // Get the new index of the thumb - it should be visible now
            thumbIndex = GetThumbIndex(imageID);
            
            // Is it the same as the last one selected
            if (imageID != selectedImageID)
            {
                // Nope... a new selection then... is the index valid
                if (thumbIndex >= 0)
                {
                    // A valid index... Set the selected border
                    thumbnailControls[thumbIndex].Parent.BackColor = SelectedHighlight;
                    // Get the image itself
                    Image imageSelected = imagesCollection[imageID].Image;
                    // Set it as the main picture
                    mainPicture.BackgroundImage = ScaleImage(imageSelected, mainPicture.Width, mainPicture.Height);
                    // Update the property members
                    selectedImage = imageSelected;
                    selectedImageID = imageID;

                    // Update the tooltip for the main picture
                    String text = imagesCollection[imageID].Description;
                    toolTip.SetToolTip(mainPicture, text + GetModifyDescriptionTooltip());

                    // Fire the event to say selection changed
                    EventHandler handler = SelectionChanged;
                    if (null != handler)
                    {
                        handler(this as object, new EventArgs());
                    }
                }
            }
        }

        /// <summary>
        /// Gets the thumb index of the selected thumb
        /// </summary>
        /// <returns>The selecvted thumb index or -1</returns>
        private int GetSelectedThumbIndex()
        {
            return GetThumbIndex(selectedImageID);
        }

        /// <summary>
        /// Scales the supplied image according to the supplied dimensions
        /// </summary>
        /// <param name="image">The image to scale</param>
        /// <param name="width">The new width in pixels</param>
        /// <param name="height">The new height in pixels</param>
        /// <returns>The scaled image</returns>
        private Image ScaleImage(Image image, int width, int height)
        {
            Image returnImage = null;
            // Only bother if the supplied dimensions differ from the actual image dimensions
            if ( (width != image.Width) || (height != image.Height) )
            {
                // Scaling ratios - assume 1 in case either width or height is 0.
                double widthRatio = 1;
                double heightRatio = 1;
                if (image.Width > 0)
                {
                    widthRatio = (double)width / (double)image.Width;
                }
                if (image.Height > 0)
                {
                    heightRatio = (double)height / (double)image.Height;
                }

                // Scale according to the correct ratio
                if (widthRatio < heightRatio)
                {
                    returnImage = image.GetThumbnailImage((int)(image.Width * widthRatio), (int)(image.Height * widthRatio), ThumbnailCallback, IntPtr.Zero);
                    scaleRatio = widthRatio;
                }
                else
                {
                    returnImage = image.GetThumbnailImage((int)(image.Width * heightRatio), (int)(image.Height * heightRatio), ThumbnailCallback, IntPtr.Zero);
                    scaleRatio = heightRatio;
                }
            }
            else
            {
                returnImage = image;
                scaleRatio = 1.0;
            }

            return returnImage;
        }

        /// <summary>
        /// Retrieve the index of the tumbnail control for the specified image
        /// </summary>
        /// <param name="imageID">The image id</param>
        /// <returns>0-based index into the thumbnail control collection, or -1</returns>
        private int GetThumbIndex(int imageID)
        {
            int index = -1;
            // Search the collection
            if (NO_SELECTION_ID != imageID)
            {
                for (int i = 0; i < thumbnailControls.Count; i++)
                {
                    // Is it this one
                    if (thumbnailControls[i].Tag.ToString().Equals(imageID.ToString()))
                    {
                        // Yep... :D
                        index = i;
                        break;
                    }
                }
            }
            return index;
        }

        /// <summary>
        /// Determines whether or not the specified image's thumbnail is currently visible
        /// </summary>
        /// <param name="imageID">The image id</param>
        /// <returns>true if the image's thumbnail is visible</returns>
        private bool IsThumbVisible(int imageID)
        {
            bool visible = false;
            // Check each of the picture box controls
            foreach (Button thumb in thumbnailControls)
            {
                // Is it this one?
                if ((null != thumb.Tag) && (thumb.Tag.ToString().Equals(imageID.ToString())))
                {
                    // Yep... :D
                    visible = true;
                    break;
                }
            }
            return visible;
        }

        /// <summary>
        /// Creates the collection of thumbnail controls that will be populated with the images 
        /// </summary>
        private void CreateFilmstripThumbnails()
        {
            // Clear out the old
            thumbnailControls.Clear();
            panelThumbs.Controls.Clear();

            // Work out the size of the picture boxes
            int numberThumbnailsToCreate = ThumbsForWidth();

            // Create each one
            // Each thumb is actually a Panel control with a Button control on top
            // The Button is inset by 5 pixels on each side so as to create a border which can 
            // be used to show selection
            for (int i = 0; i < numberThumbnailsToCreate; i++)
            {
                PushThumbnail();
            }
        }

        /// <summary>
        /// Pushes a new thumbnail button onto our collection
        /// Used to add a new thumbnail when the control is loaded or resized
        /// </summary>
        /// <returns>The index of the newly added thumb in the collection (i.e. the last one)</returns>
        private int PushThumbnail()
        {
            int index = thumbnailControls.Count;

            // We leave a 20 pixel gap inbetween each one, and make them just smaller than the container's height
            int nThumbnailTop = panelThumbs.Top;
            int nThumbnailHeight = panelThumbs.Height - thumbnailPanelMargin;
            // Work out how many we can have and get the modulus, so we can centre all the controls
            //int margin = (panelThumbnails.Width % (thumbnailPanelWidth + thumbnailSeperatorSize)) / 2;
            int margin = thumbnailSeperatorSize;

            // Create the panel
            Panel newPanel = new Panel();
            newPanel.Name = THUMB_PANEL_NAME + index.ToString();
            newPanel.BackColor = ControlBackground;
            newPanel.Top = nThumbnailTop;
            newPanel.Height = nThumbnailHeight;
            newPanel.Width = thumbnailPanelWidth;
            newPanel.Left = margin;
            // If this isn't the first one... shift it to the right the correct amount
            if (index > 0)
            {
                newPanel.Left += ((thumbnailPanelWidth + thumbnailSeperatorSize) * index);
            }

            // Create the Button
            Button newButton = new Button();
            newButton.Name = THUMB_NAME + index.ToString();
            // Make it 5 pixels in from it's Panel parent
            newButton.Top = thumbnailImageMargin;
            newButton.Height = nThumbnailHeight - thumbnailPanelMargin;
            newButton.Width = thumbnailPanelWidth - thumbnailPanelMargin;
            newButton.Left = thumbnailImageMargin;
            // Initially invisible (it has no image at the moment)
            newButton.Visible = false;
            newButton.FlatStyle = FlatStyle.Flat;
            newButton.FlatAppearance.BorderSize = 0;
            newButton.Click += new EventHandler(Thumbnail_Click);
            newButton.KeyDown += new KeyEventHandler(FilmstripControl_KeyDown);
            newButton.TabIndex = buttonRight.TabIndex;
            buttonRight.TabIndex++;

            // Add it to our thumb collection
            thumbnailControls.Add(newButton);
            // Add it to the parent panel
            newPanel.Controls.Add(newButton);

            // Add the panel to it's parent container
            panelThumbs.Controls.Add(newPanel);

            return thumbnailControls.Count - 1;
        }

        /// <summary>
        /// Pops the last thumb control from the collection
        /// </summary>
        private void PopThumbnail()
        {
            // Remove it
            thumbnailControls.RemoveAt(thumbnailControls.Count - 1);
            panelThumbs.Controls.RemoveAt(panelThumbs.Controls.Count - 1);
            // And update the tab index of the Right navigation button
            buttonRight.TabIndex--;
        }

        /// <summary>
        /// Populates the specified thumb
        /// </summary>
        /// <param name="index">The index of the thumb to populate in the thumbs collection</param>
        private void PopulateThumbnail(int index)
        {
            // Check the index is ok
            if ((index > thumbnailControls.Count) || (index < 0))
            {
                return;
            }

            // Get this one from the collection
            Button thisNail = thumbnailControls[index];

            Button previousNail = null;
            int imageID = NO_SELECTION_ID;
            Image thisImage = null;

            // Is this the first one?
            if (index > 0)
            {
                // Nope... so get the details of the previous one
                previousNail = thumbnailControls[index - 1];
                int previousImageID = Convert.ToInt32(previousNail.Tag);
                // Now work out the details for this one
                imageID = keys[keys.IndexOf(previousImageID) + 1];
                //imageCollection.TryGetValue(imageID, out thisImage);
                thisImage = imagesCollection[imageID].Image;
            }
            else
            {
                // Yep, so just take the first key in the list
                imageID = keys[0];
                thisImage = imagesCollection[imageID].Image;
            }

            // Populate
            thisNail.Tag = imageID;
            thisNail.Visible = true;

            // Right... did we get it?
            if (null != thisImage)
            {
                // Remove any label that might be there
                ClearThumbnailError(index);

                // Scale the image
                Image thumb = thisImage.GetThumbnailImage(thisNail.Width, thisNail.Height, ThumbnailCallback, IntPtr.Zero);
                thisNail.Image = thumb;
            }
            else
            {
                SetThumbnailError(index);
            }
            // Set the Tooltip text
            String text = imagesCollection[imageID].Description;
            // Add the text "(Image x of y.)" to the tooltip on the thumb
            text = String.Format(TIP_THUMB_EXTRA, text, imageID + 1, imagesCollection.Count);
            toolTip.SetToolTip(thisNail, text);

            // Was this image the selected one?
            if (selectedImageID == imageID)
            {
                // Set the selected border
                thisNail.Parent.BackColor = SelectedHighlight;
            }
        }

        /// <summary>
        /// Returns the number of thumbnail buttons possible for the current width of the control
        /// </summary>
        /// <returns>Returns the number of thumbnail buttons possible for the current width of the control</returns>
        private int ThumbsForWidth()
        {
            return panelThumbs.Width / (thumbnailPanelWidth + thumbnailSeperatorSize);
        }

        /// <summary>
        /// Updates the enablement of the navigation buttons depending on the state of the filmstrip
        /// </summary>
        private void SetButtonStates()
        {
            if ( thumbnailControls.Count >= imagesCollection.Count )
            {
                // There aren't enough images or just enough images to fill all the thumbnail controls
                buttonLeft.Enabled = false;
                buttonRight.Enabled = false;
            }
            else if (0 == thumbsStartIndex)
            {
                // We're at the start
                buttonLeft.Enabled = false;
                buttonRight.Enabled = true;
            }
            else if (imagesCollection.Count == (thumbsStartIndex + thumbnailControls.Count))
            {
                // We're at the end
                buttonLeft.Enabled = true;
                buttonRight.Enabled = false;
            }
            else
            {
                // We're in the middle
                buttonLeft.Enabled = true;
                buttonRight.Enabled = true;
            }
        }

        /// <summary>
        /// Moves all images shown in the thumbnails to the right (if possible)
        /// </summary>
        private void MoveThumbnailsRight(int placesToMove)
        {
            // Only if we're not already at the end
            int placesMoved = 0;

            while (((thumbsStartIndex + thumbnailControls.Count) < keys.Count) && (placesMoved < placesToMove) )
            {
                // Increment out thumbnail start index
                thumbsStartIndex++;
                placesMoved++;
            }

            // Repopulate them
            PopulateThumbnails();
            // Update the button states
            SetButtonStates();
        }

        /// <summary>
        /// Moves all images shown in the thumbnails to the left (if possible)
        /// </summary>
        private void MoveThumbnailsLeft(int placesToMove)
        {
            // Only if we're not already at the start
            int placesMoved = 0;

            while ((thumbsStartIndex > 0) && (placesMoved < placesToMove) )
            {
                // Decrement out thumbnail start index
                thumbsStartIndex --;
                placesMoved++;
            }

            // Repopulate them
            PopulateThumbnails();
            // Update the button states
            SetButtonStates();
        }

        /// <summary>
        /// Starts the hover timer
        /// </summary>
        /// <param name="sender"></param>
        private void StartHoverTimer(object sender)
        {
            hoverControl = sender as Control;
            hoverTimer.Start();
        }

        /// <summary>
        /// Stops the hover timer.
        /// </summary>
        private void StopHoverTimer()
        {
            hoverTimer.Stop();
            // Reset any tootip text
            if (hoverControl == buttonLeft)
            {
                toolTip.SetToolTip(hoverControl, TIP_SCROLL_LEFT);
            }
            else if (hoverControl == buttonRight)
            {
                toolTip.SetToolTip(hoverControl, TIP_SCROLL_RIGHT);
            }
            hoverControl = null;
        }

        #endregion Private Actions
    }

    /// <summary>
    /// Encapsulation of an image in the Filmstrip control.
    /// </summary>
    public class FilmstripImage
    {
        #region Member variables

        private int id;
        private Image image;
        private String description;

        #endregion Member variables

        #region Properties

        /// <summary>
        /// The Image ID.
        /// Cannot be -1.
        /// </summary>
        /// <exception cref="System.SystemException.ArgumentOutOfRangeException">System.SystemException.ArgumentOutOfRangeException - 
        /// when trying to set the image id to be -1</exception>
        public int Id
        {
            get { return id; }
            set 
            {
                if (FilmstripControl.NO_SELECTION_ID == value)
                {
                    throw new ArgumentOutOfRangeException();
                }
                id = value;
            }
        }

        /// <summary>
        /// The image itself.
        /// </summary>
        public Image Image
        {
            get { return image; }
            set { image = value; }
        }

        /// <summary>
        /// The description of the image.
        /// </summary>
        public String Description
        {
            get { return description; }
            set { description = value; }
        }

        #endregion Properties

        /// <summary>
        /// Default constructor
        /// </summary>
        public FilmstripImage()
        {
            id = FilmstripControl.NO_SELECTION_ID;
            image = null;
            description = String.Empty;
        }

        /// <summary>
        /// Overloaded constructor
        /// </summary>
        /// <param name="imageValue">The image itself</param>
        /// <param name="descriptionValue">The image description</param>
        public FilmstripImage(Image imageValue, String descriptionValue)
        {
            id = FilmstripControl.NO_SELECTION_ID;
            image = imageValue;
            description = descriptionValue;
        }

        /// <summary>
        /// Overloaded constructor
        /// </summary>
        /// <param name="idValue">The image ID - cannot be -1</param>
        /// <param name="imageValue">The image itself</param>
        /// <param name="descriptionValue">The image description</param>
        /// <exception cref="System.SystemException.ArgumentOutOfRangeException">System.SystemException.ArgumentOutOfRangeException - if the image id is -1</exception>
        public FilmstripImage(int idValue, Image imageValue, String descriptionValue)
        {
            if (FilmstripControl.NO_SELECTION_ID == idValue)
            {
                throw new ArgumentOutOfRangeException();
            }
            id = idValue;
            image = imageValue;
            description = descriptionValue;
        }
    }


    #region Designer support

    /// <summary>
    /// Designer namespace encapsualting all Designer functionality for the Filmstrip Control
    /// </summary>
    namespace Designer
    {
        /// <summary>
        /// Control designer class for the FilmstripControl
        /// </summary>
        internal class FilmstripControlDesigner : ControlDesigner
        {
            // The list of actions
            private DesignerActionListCollection actionList;

            /// <summary>
            /// Standard constructor
            /// </summary>
            public FilmstripControlDesigner()
            {
            }

            /// <summary>
            /// ActionsLists property - populated with what the Filmstrip Control needs
            /// </summary>
            public override DesignerActionListCollection ActionLists
            {
                get
                {
                    // Create a new collection if necessary
                    if (actionList == null)
                    {
                        actionList = new DesignerActionListCollection();
                        // We add _our_ actions
                        actionList.Add(new FilmstripControlDesignerActionList(this.Component));
                    }

                    return actionList;
                }
            }

        }

        /// <summary>
        /// Actions list calss specfically for the the Filmstrip Control.
        /// The list currently contains:
        ///     ControlBackground
        ///     SelectedHighlight
        ///     MainImageLayout
        ///     ThumbsStripLocation
        ///     Dock
        /// </summary>
        internal class FilmstripControlDesignerActionList : DesignerActionList
        {
            FilmstripControl usercontrol;
            private DesignerActionUIService service = null;

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="component">The control itself</param>
            public FilmstripControlDesignerActionList(IComponent component)
                : base(component)
            {
                usercontrol = component as FilmstripControl;
                this.service = GetService(typeof(DesignerActionUIService)) as DesignerActionUIService;
            }

            /// <summary>
            /// Retrieves the property descriptor for the named control property
            /// </summary>
            /// <param name="propName">The property name required</param>
            /// <returns>The property descriptor for the named control property</returns>
            /// <exception cref="ArgumentException">ArgumentException if the named property does not exist on the control</exception>
            private PropertyDescriptor GetPropertyByName(String propName)
            {
                PropertyDescriptor prop = null;
                prop = TypeDescriptor.GetProperties(usercontrol)[propName];
                if (null == prop)
                {
                    throw new ArgumentException("Matching property not found!", propName);
                }
                else
                {
                    return prop;
                }
            }

            /// <summary>
            /// Property for the Filmstrip Control's ControlBackground property
            /// </summary>
            public Color ControlBackground
            {
                get { return usercontrol.ControlBackground; }
                set { GetPropertyByName("ControlBackground").SetValue(usercontrol, value); }
            }

            /// <summary>
            /// Property for the Filmstrip Control's SelectedHighlight property
            /// </summary>
            public Color SelectedHighlight
            {
                get { return usercontrol.SelectedHighlight; }
                set { GetPropertyByName("SelectedHighlight").SetValue(usercontrol, value); }
            }

            /// <summary>
            /// Property for the Filmstrip Control's MainImageLayout property
            /// </summary>
            public ImageLayout MainImageLayout
            {
                get { return usercontrol.MainImageLayout; }
                set { GetPropertyByName("MainImageLayout").SetValue(usercontrol, value); }
            }

            /// <summary>
            /// Property for the Filmstrip Control's ThumbsStripLocation property
            /// </summary>
            public FilmstripControl.StripLocation ThumbsStripLocation
            {
                get { return usercontrol.ThumbsStripLocation; }
                set
                {
                    GetPropertyByName("ThumbsStripLocation").SetValue(usercontrol, value);
                    // We need to invalidate here otherwise the selection handles around the 
                    // control do not repaint correctly
                    usercontrol.Invalidate(true);
                }
            }

            /// <summary>
            /// Property for the Filmstrip Control's Dock property
            /// </summary>
            public DockStyle Dock
            {
                get { return usercontrol.Dock; }
                set { GetPropertyByName("Dock").SetValue(usercontrol, value); }
            }

            /// <summary>
            /// Poulates the list of available actions of the Filmstrip Control.
            /// </summary>
            /// <returns>The list of availabe actions for the Filmstrip Control</returns>
            public override DesignerActionItemCollection GetSortedActionItems()
            {
                DesignerActionItemCollection items = new DesignerActionItemCollection();
                items.Add(new DesignerActionPropertyItem("ControlBackground", "Control background colour", "Filmstrip properties"));
                items.Add(new DesignerActionPropertyItem("SelectedHighlight", "Selected highlight colour", "Filmstrip properties"));
                items.Add(new DesignerActionPropertyItem("MainImageLayout", "Main image layout style", "Filmstrip properties"));
                items.Add(new DesignerActionPropertyItem("ThumbsStripLocation", "Thumbs strip location", "Filmstrip properties"));
                items.Add(new DesignerActionPropertyItem("Dock", "Dock style", "Base properties"));
                return items;
            }
        }
    }

    #endregion Designer support

}
