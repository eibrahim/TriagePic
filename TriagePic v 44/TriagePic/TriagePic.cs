#define PFIF_TEST
#define DEBUGPIX
//#define READREG
// #define PTL
using System;
using System.Collections; // Array List
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D; // for clock
using System.Drawing.Imaging; // for clock
using System.Text;
using System.Text.RegularExpressions; // email verification
using System.Windows.Forms;
using System.IO;
using System.Net.Mail;  // see www.aspnettutorials.com/tutorials/email/email-attach-aspnet2-csharp.aspx
using Microsoft.Win32; // for registry
using System.Xml; // For outbox
// using System.Security.Principal;
using Filmstrip;
using CodeProject.Dialog;
using TriagePic;
using TriagePic.gov.nih.nlm.hepldemo1;

// DialogBox.dll; for AppBox, MsgBox, ErrBox


namespace TriagePicNamespace
{
    public partial class TriagePic : Form
    {
        #region Variables
        //public string startupPath = Application.StartupPath + "/"; /* constant */
        private FileInfo noPictureYetImageFileInfo = new FileInfo("images/special/no-images.jpg");
        private FileInfo simulatedCameraImageFileInfo = new FileInfo("images/special/img_0001.jpg");
        private FileInfo legaleseFileInfo = new FileInfo("text attachments/restrictions.txt");
        private FileInfo anonymizedLegaleseFileInfo = new FileInfo("text attachments/anonymized.txt");
// WAS:        public FileInfo imageShownFileInfo = null;
// WAS:        private Image imageShown;
        // public string source = "C:/Documents and Settings/aamgill/My Documents/Bluetooth Exchange Folder/"; // constant
        public string justTaken = "images/just taken/"; /* constant */ // Moved from bluetooth folder
        //public string justTaken = "images\\just taken\\"; /* constant */  // Form that File.Move wants
        public string dest1 = "images/processed/"; /* constant */
        public string dest2 = "images/sent/"; /* constant */
        private string simulatedCameraBrowseFolder = Application.StartupPath + "\\images\\special"; // Can change during session, after each browse.
        public string patientID_PrefixAndNumber; // Either normal or practice
        // public string patientID_NumberOnly; // short form for outbox
        public string patientID_OutboxForm; // v 41 - for normal, is patient number only; for practice, also has outbox prefix
        public string normalPatientID_Prefix;
        public string practicePatientID_Prefix = "Practice-"; // hardcode for now.
        public string practicePatientID_OutboxPrefix = "Prac-"; // shorter
        public int practicePatientID = 0;
        private ToolTip toolTip1 = new ToolTip();
        public int practiceModeChoice = 2; // Normal send
        // private MailMessage msg = new MailMessage();
        Timer t = new Timer();
        Rectangle rec = new Rectangle(50, 50, 100, 100);
        public SplashScreen ms_frmSplash = null; // For hints, see www.codeproject.com/KB/cs/prettygoodsplashscreen.aspx
        /* Foregoing was static, but that causes problems with centering */
        public int counterForSplash = 3; // NAH:  7; // If you change this, change fade in/out values elsewhere too.
        private int previousMinuteShown;
        public int counterForSentMessage = 0;
        MyEmail myEmail = null;
        public bool newOptionsFileCreated; // File that following items are stored in.  Test this after ReadOptionsFromFile.
        // Stored in XML file:
        public string emailTo;
        public string emailCc;
        public string emailBcc;
        public string anonymizedEmailTo;
        public string anonymizedEmailCc;
        public string anonymizedEmailBcc;
        // also nextPatientID, shown in PatientNumberTextBox.Text
        private string eventSuffix; // See Checklist tab.  Stored in .xml as Event Type
        private string eventName;
        private string patientTrackingOfficer;
        private string triagePhysiciansOrRNs;
        private string otherStationStaff;
        private string photographers;
        public DataSet outboxDataSet = null;
        public int eventRange;
// WAS:        private string lastSentPic = null;
        private ArrayList lastSentPics = new ArrayList();
        private ArrayList deleteJustTakenPics = new ArrayList(); // delayed purge of pics in "just taken" folder.  Same images, but different filenames as lastSentPics.
        private string lastSentPFIF = null;
#if PTL
        private string lastSentEDXL_and_PTL = null;
#else
        private string lastSentEDXL_and_LPF = null;
#endif
        // Email Setup:
        // Allow one main SMTP server, one backup SMTP server:
        public bool suppressEmailSetupUpdate = true; // Ignore initial loads of Email Setup fields
        public bool suppressShownEmailProfileTextChangedEvents = false;
        public bool suppressEmailProfileNameComboSelectedIndexChanged = false;
        public string[] emailProfileSelected = { null, null };
        public string[] emailServer = {null, null};
        public string[] emailFrom = {null, null};
        public string[] emailProfileComments = { null, null };
        public string[] portForSMTP = {null, null};
        public bool  [] useBasicAuthenticationForSMTP = {false, false};
        public bool  [] useSSL_ForSMTP = {false, false};
        public string[] basicAuthName = {null, null};
        public string[] basicAuthPassword = {null, null};
        public bool  [] fetchBeforeSendAuthentication = {false, false};
        public string[] emailIncomingServer = {null, null};
        public bool  [] useSSL_ForIncoming = {false, false};
        public string[] portForIncoming = {null, null};
        // Main Tab:
        private const String NO_SELECTION = "No selection"; // For filmstrip
        public List<InBoxData> inBoxDataList = new List<InBoxData>();
        private bool ignoreCaptionExtraTextChangedEvent = false;

        Options op = null;
        #endregion
        #region Startup and Timers

        public TriagePic()
        {
            InitializeComponent();

            //** NOTE - This code won't work until the web service is linked in. When the web service is working the list 
            //** of events should show up.
            WebServices service = new WebServices();
            Basic[] events = service.shn_pls_get_incident_list();

            if (events == null)
            {
                events = new Basic[0];
            }

            List<Basic> eventList = new List<Basic>();
            for (int i = 0; i < events.Length; i++)
            {
                eventList.Add(events[i]);
            }

            if (eventList.Count > 0)
            {
                EventNameComboBox.DataSource = eventList;
                EventNameComboBox.DisplayMember = "name";   
            }
            EventNameComboBox.Text = "";

            // Too soon, can't do math on this.Location yet:  ShowSplash();
            this.tabControl1.SelectedTab = this.tabPageChecklist;
#if EXPERIMENT_TO_WORDWRAP_TABS
            // From Mick Doherty in 2005 at http://www.pcreview.co.uk/forums/thread-2006689.php
            // Works, but lame padding causes a blank line at top of each tab, so not great.
            // (That is, for every n lines of real wrapped text in tab, you'd need to pad n-1 lines, causing n-1 blank lines at tab top)
            // Also looks better if centered horizontally, faked here by adding blanks.
            tabControl1.Padding = new Point(6, tabControl1.Font.Height + 4);
            tabControl1.TabPages[0].Text = "Main\n\r Info"; // done programmatically to get wordwrap (XP or later w. Visual Styles enabled)
            tabControl1.TabPages[2].Text = "Can't\n\rSnap";
            tabControl1.TabPages[3].Text = "   Checklist\n\r(Event, Staff)";
            tabControl1.TabPages[6].Text = "Email\n\rSetup";

            // Other ways to go: owner draw (ugh), use images (containing text) on tabs
#endif
            // System.Reflection.Assembly.GetExecutingAssembly().GetName().Version
// MOVE:
//            this.Text = "TriagePic v " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version +
//                " - BHEPP Prototype, Built at NLM, Deployed at " + orgAbbrOrShortNameTextBox.Text;

            sendingLabel.Text = "";
            photoRoleComboBox.SelectedItem = ImageRole.Unassigned.ToString();
            SetButtonStates();
            // Collect info for report:
            MachineNameFieldLabel.Text = Environment.MachineName;
            /* Other equivalent methods:
            ... = System.Net.Dns.GetHostName();
            ... = System.Windows.Forms.SystemInformation.ComputerName;
            ... = System.Environment.GetEnvironmentVariable("COMPUTERNAME");
             */
            // Via System.Environment to get login name with the domain name:
            UserNameFieldLabel.Text = System.Environment.UserDomainName + "\\" + System.Environment.UserName;
            /* Other equivalent methods:
            // using System.Security
            WindowsPrincipal wp = new WindowsPrincipal(WindowsIdentity.GetCurrent());
            ... = wp.Identity.Name;
            // Via SystemInformation
            ... = System.Windows.Forms.SystemInformation.UserDomainName + "\\" + System.Windows.Forms.SystemInformation.UserName;
            */
//            string zoneTip = "Practicing?  Control+Zone deletes patient (after gender/ped verify).  No email sent, no ID increment.";
            string zoneTip = "Verify, send, then prep form for next arrival (or more options if Practice Mode)";
            this.toolTip1.SetToolTip(GreenSendPhotoButton, zoneTip);
            this.toolTip1.SetToolTip(BHGreenSendPhotoButton, zoneTip);
            this.toolTip1.SetToolTip(YellowSendPhotoButton, zoneTip);
            this.toolTip1.SetToolTip(RedSendPhotoButton, zoneTip);
            this.toolTip1.SetToolTip(GraySendPhotoButton, zoneTip);
            this.toolTip1.SetToolTip(BlackSendPhotoButton, zoneTip);
            /// See If Folder Is Empty

            if(!Directory.Exists(Constants.source))
                Directory.CreateDirectory(Constants.source);
            string[] fileEntries = Directory.GetFiles(Constants.source);
            if (fileEntries.Length > 0)
            {
                string phrase1 = fileEntries.Length.ToString() + " unprocessed photographs were found)";
                string phrase2 = "Delete them?";
                string phrase3 = "Delete old photos?";
                if (fileEntries.Length == 1)
                {
                    phrase1 = "An unprocessed photograph was found";
                    phrase2 = "Delete it?";
                    phrase2 = "Delete old photo?";
                }
                DialogResult dlgResult = MsgBox.Show(phrase1 +
                    " (perhaps sent from the camera a moment ago, or leftover from earlier TriagePic crash).  " +
                    phrase2, phrase3, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dlgResult == DialogResult.Yes)
                {
                    foreach (string fi in fileEntries)
                    //foreach (File fi in Directory.GetFiles(source))
                        File.Delete(fi); // Don't need recursive delete here
                }
                //else if (dlgResult == DialogResult.No) { ...}
            }
            EmptyDirectoryOnStartup(justTaken);
            EmptyDirectoryOnStartup(dest1);
            // clock:

            t.Interval = 1000;
            t.Tick += new EventHandler(OncePerSecond);
            t.Enabled = true;
            myEmail = new MyEmail(this);

            op = new Options(this);


            op = op.ReadOptionsFromFile();
            // ErrBox.Show("Test"); -- Note that this comes up in screen center, because parent window is not fully instantiated yet.
            // Likewise all other AppBox and ErrBox calls in this function.
            if(newOptionsFileCreated) // Indication no file found, so empty one created.
            {
                AppBox.Show(
                    "No pre-existing file " + Options.options_path + " found in same directory as TriagePic.exe,\n" +
                    "so one was created with empty content.  Either exit here and replace it with one with real content,\n" +
                    "or fill in the following and then exit to save:\n" +
                    "  Email Settings tab - all fields\n" +
                    "  Hospital tab - all fields\n" +
                    "  Checklist tab - event type & optional names\n" +
                    "  Main tab - patient number.");
            }
            myEmail.ReadEmailSettings(op); // transfer from op to local variables, GUI fields
            suppressEmailSetupUpdate = false; // If Email Setup fields get edited, save them immediately to op object.

            try
            {
                PatientTrackingOfficerTextBox.Text = patientTrackingOfficer = op.StationPatientTrackingOfficerText;
                TriagePhysicansOrRNsTextBox.Text = triagePhysiciansOrRNs = op.StationTriagePhysicansOrRNsText;
                OtherStationStaff.Text = otherStationStaff = op.StationOtherStaffText;
                PhotographersTextBox.Text = photographers = op.StationPhotographersText;

                EventNameComboBox.Text = eventName = op.EventNameText;
                eventSuffix = op.EventTypeText;
                eventRange = op.EventRangeInt;
            }
            catch (Exception ex)
            {
                ErrBox.Show("Error in reading checklist settings within TriagePicSharedSettings.xml\n" + ex.ToString());
                return;
            }

            if(eventSuffix == "TEST or DEMO")
            {
                EventTypeRadio1.Checked = true;
                EventTypeRadio1_Click(null, null);
            }
            else if (eventSuffix == "DRILL")
            {
                EventTypeRadio2.Checked = true;
                EventTypeRadio2_Click(null, null);
            }
            else if (eventSuffix == "REAL - NOT A DRILL")// was: "REAL DISASTER - NOT A DRILL")
            {
                EventTypeRadio3.Checked = true;
                EventTypeRadio3_Click(null, null);
            }
            else
                ErrBox.Show("Bad value in " + Options.options_path + " for Event Type"); 
            switch (eventRange)
            {
                default: // don't bother reporting as error, just go with default
                case 1: EventRangeRadioButton1.Checked = true; break;
                case 2: EventRangeRadioButton2.Checked = true; break;
                case 3: EventRangeRadioButton3.Checked = true; break;
                case 4: EventRangeRadioButton4.Checked = true; break;
            }

            comboBoxOutboxID_Type.SelectedIndex = 0; // set default

            try
            {

                PatientNumberTextBox.Text = op.PatientNumberText;
                practicePatientID = Convert.ToInt32(op.PracticePatientNumberText);

                orgNameTextBox.Text = op.OrgNameText;
                orgAbbrOrShortNameTextBox.Text = op.OrgAbbrOrShortNameText;
                orgStreetAddress1TextBox.Text = op.OrgStreetAddress1Text;
                orgStreetAddress2TextBox.Text = op.OrgStreetAddress2Text;
                orgTownOrCityTextBox.Text = op.OrgTownOrCityText;
                orgCountyTextBox.Text = op.OrgCountyText;
                org2LetterStateTextBox.Text = op.Org2LetterStateText;
                orgZipcodeTextBox.Text = op.OrgZipcodeText;
                orgPhoneTextBox.Text = op.OrgPhoneText;
                orgFaxTextBox.Text = op.OrgFaxText;
                orgEmailTextBox.Text = op.OrgEmailText;
                orgWebSiteTextBox.Text = op.OrgWebSiteText;
                orgNPI_TextBox.Text = op.OrgNPI_Text;
                PatientNumberPrefix.Text = orgPatientID_Prefix.Text = op.OrgPatientID_PrefixText; //PatientNumberPrefix is read-only as far as user is concerned.
            }
            catch (Exception ex)
            {
                ErrBox.Show("Error in reading next-patient number or hospital settings within TriagePicSharedSettings.xml\n" + ex.ToString());
                return;
            }

            myEmail.ReadEmailHistory();

            // Set main header text, now that we have short name:
            // "g" format is short date & short time
            System.Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            this.Text = "TriagePic " + version.Major.ToString() + "." + version.Minor.ToString() + " of " + BuildDate().ToString("g") +
                " - BHEPP Prototype, Built at NLM, Deployed at " + orgAbbrOrShortNameTextBox.Text;
        }

        private System.DateTime BuildDate()
        {
            // Function reported by "Wes", http://stackoverflow.com/questions/324245/aspnet-show-application-build-dateinfo-at-the-bottom-of-the-screen
            // See also:
            //   http://msdn.microsoft.com/en-us/library/system.reflection.assemblyversionattribute.assemblyversionattribute.aspx
            //   http://dotnetfreak.co.uk/blog/archive/2004/07/08/determining-the-build-date-of-an-assembly.aspx?CommentPosted=true#commentmessage
            // This ONLY works if the assembly was built using VS.NET and the assembly version attribute is set to something like "<major>.<minor>.*".
            // The asterisk (*) is the important part, as if present, VS.NET generates both the build and revision numbers automatically.
            // Note for app the version is set by opening the 'My Project' file and clicking on the 'assembly information' button.
            // An alternative method is to simply read the last time the file was written, using something similar to:
            //   Return System.IO.File.GetLastWriteTime(System.Reflection.Assembly.GetExecutingAssembly.Location)
            // Build dates start from 01/01/2000

            System.DateTime result = DateTime.Parse("1/1/2000");
            //Retrieve the version information from the assembly from which this code is being executed
            System.Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            //Add the number of days (build)
            result = result.AddDays(version.Build);
            //Add the number of seconds since midnight (revision) multiplied by 2
            result = result.AddSeconds(version.Revision * 2);
            //If we're currently in daylight saving time add an extra hour.  [This strikes Glenn as dubious treatment.]
            if (TimeZone.IsDaylightSavingTime(System.DateTime.Now, TimeZone.CurrentTimeZone.GetDaylightChanges(System.DateTime.Now.Year)))
            { result = result.AddHours(1); }

            return result;

        }
        // Alternative, from same listing, but Jørn Jensen:  Use file time of .exe:
        // lblVersion.Text = String.Format("Version: {0}<br>Dated: {1}",
        // System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString(),
        // System.IO.File.GetLastWriteTime(System.Reflection.Assembly.GetExecutingAssembly().Location).ToShortDateString());

        public void EmptyDirectoryOnStartup(string d)
        {
            DirectoryInfo di = new DirectoryInfo(d);
            FileInfo[] rgFiles = null;
            try
            {
                rgFiles = di.GetFiles("*.*");
            }
            catch (Exception ex)
            {
                ErrBox.Show(ex.Source + ":  " + ex.Message);
                ErrBox.Show("Required Triage Pic subdirectory " + d + " was not found; Incomplete installation.");
                Application.Exit();
                return;
            }
            foreach (FileInfo fi2 in rgFiles)
                fi2.Delete();
        }

        public void OncePerSecond(object ob, EventArgs a)
        {
            if (counterForSplash > 0)
            {
// Fade in/out too clunky if only doing it by 1-second intervals.  Not worth creating a thread to do it smoothly
 //               if (counterForSplash > 5)  // if counterForSplash initializes as 7
 //                   ms_frmSplash.Opacity += 0.50;
 //               if (counterForSplash < 3)
 //                   ms_frmSplash.Opacity -= 0.50;
                counterForSplash--;
                if (counterForSplash == 2)
                    ShowSplash();
                else if (counterForSplash == 0)
                    CloseSplash();
            }   
            FindCameraImage();
            if (counterForSentMessage > 0)
                counterForSentMessage--;
            if (counterForSentMessage == 0)
            {
                // startupPath needed because Browse dialog in simulateShot can globally change inferred path of File objects, we need to be explicit.
                sendingLabel.Text = ""; // clear for next patient // was commented out when using DEBUGPIX

                // Do delayed archive move of files associated with last sent message
                // If lastSentPics is 0, then assume it's because practiceMode was set and choice 0 (delete) was taken.
                // (but user may have changed it in the last few seconds, so don't check it here)
                if (lastSentPics.Count != deleteJustTakenPics.Count && lastSentPics.Count > 0)
                    ErrBox.Show("Internal error.  Mismatch of counts of pictures just sent.");
                if (lastSentPics.Count > 0)
                {
                    string [] lsp = new string[lastSentPics.Count];
                    lsp = (string[])lastSentPics.ToArray(typeof(string));
                    string destination;
                    foreach (string lastSentPic in lsp)
                    {
                        destination = lastSentPic.Replace(dest1, dest2);
                        // e.g., image/processed/911-1234 Red.jpg --> image/sent/911-1234 Red.jpg
                        //File.Move(startupPath + lastSentPic, startupPath + destination);
                        File.Move(lastSentPic, destination);
                    }
                    lastSentPics.Clear();
                }
                if (deleteJustTakenPics.Count > 0)
                {
                    string[] djtp = new string[deleteJustTakenPics.Count];
                    djtp = (string[])deleteJustTakenPics.ToArray(typeof(string));
                    foreach (string deleteJustTakenPic in djtp)
                    {
                        // e.g., image/just taken/RIMG0001.JPG
                        //File.Delete(startupPath + deleteJustTakenPic);
                        File.Delete(deleteJustTakenPic);
                    }
                    deleteJustTakenPics.Clear();
                }
                if (lastSentPFIF != null)
                {
                    string destination = lastSentPFIF.Replace(dest1, dest2);
                    //File.Move(startupPath + lastSentPFIF, startupPath + destination);
                    File.Move(lastSentPFIF, destination);
                    lastSentPFIF = null;
                }
#if PTL
                if (lastSentEDXL_and_PTL != null)
                {
                    string destination = lastSentEDXL_and_PTL.Replace(dest1, dest2);
                    File.Move(lastSentEDXL_and_PTL, destination);
                    lastSentEDXL_and_PTL = null;
                }
#else
                if (lastSentEDXL_and_LPF != null)
                {
                    string destination = lastSentEDXL_and_LPF.Replace(dest1, dest2);
                    //File.Move(startupPath + lastSentEDXL_and_LPF, startupPath + destination);
                    File.Move(lastSentEDXL_and_LPF, destination);
                    lastSentEDXL_and_LPF = null;
                }
#endif
            }
            // update clock if needed:
            if (DateTime.Now.Minute == previousMinuteShown)
                return; // avoid unnecessary refreshes, minimize flashing
            previousMinuteShown = DateTime.Now.Minute;
            string tzabbr = GetTimeZoneAbbreviation();
            // For culture-invariant, DateTime.Now.ToString("f") format is like "dddd, dd MMMM yyyy HH:mm" if culture-invariant, or "dddd, MMMM dd yyyy HH:mm tt" for en-US. 
            // See http://blogs.msdn.com/kathykam/archive/2006/09/29/.NET-Format-String-102_3A00_-DateTime-Format-String.aspx
            // However, we need to keep string length under 40 characters to avoid losing terminating linebreak if email read by Outlook.
            // So shorten month using "MMM".  Add a little punctuation
            // For 24 time, use: HH:mm (without tt)
            DateTimeLabel.Text = DateTime.Now.ToString("dddd, MMM. dd yyyy hh:mm tt") + " " + tzabbr; // For en-US culture, this outputs, for example: Tuesday, April 10, 2001 3:51 PM EST 
        }

        public string GetTimeZoneAbbreviation() // New v .42
        {
            // There are no international standards for timezone abbreviation.
            // Nor does .Net 2.0 or 3.5 supply abbreviations.
            // The usually recommended hack (at least for US timezones) is to take the first letter of each word as shown below.
            // .Net 2.0: TimeZone timeZone = System.TimeZone.CurrentTimeZone; Console.WriteLine(timeZone.StandardName);
            string tzname;
            if (System.TimeZone.CurrentTimeZone.IsDaylightSavingTime(DateTime.Now))
                tzname = System.TimeZone.CurrentTimeZone.DaylightName;
            else
                tzname = System.TimeZone.CurrentTimeZone.StandardName;
            // .Net 3.5: TimeZoneInfo hwZone = TimeZoneInfo.FindSystemTimeZoneById("Hawaiian Standard Time"); TimeZoneInfo info = TimeZoneInfo.Local;
            // Hack to convert time zone name to 3 letter abbreviation by just taking the first letter of each word.
            // This will not work for 100% of timezones worldwide (and can be 4 letters elsewhere), but is this program ever to be used outside Washington DC area?
            // For a somewhat-full list, see www.timeanddate.com/library/abbreviations/timezones/
            string[] words = tzname.Split(" ".ToCharArray());
            string tzabbr = "";
            foreach (string word in words)
                tzabbr += word[0];
            return tzabbr;
        }

        public void ShowSplash()
        {
            ms_frmSplash = new SplashScreen();
            ms_frmSplash.Owner = this; // Needed to make CenterParent property work
            ms_frmSplash.ShowInTaskbar = false;
            // CenterParent not working for a ".Show" form,  so set position manually.
            ms_frmSplash.Location = new System.Drawing.Point(
                this.Location.X + (this.Width /2) - (ms_frmSplash.Width/2),
                this.Location.Y + (this.Height/2) - (ms_frmSplash.Height/2)
                );
            ms_frmSplash.Show(this);
        }

        public void CloseSplash()
        {
            ms_frmSplash.Close();
        }
        #endregion
        #region "Main" Tab
        private void SimulateCameraShotButton_Click(object sender, EventArgs e)
        {
            // Sticks copy of stock image into bluetooth folder.
            // Stock picture is "img_0001.jpg".  We may make multiple instances here, so have to bump number.
            // Handle up to 99.  Stock picture name MUST contain "01" for this to work.

            // This will work as long as there's no name conflict between simulated pics and real ones.
            // Since we're using "img_", and Ricoh uses "RIMG", OK so far.
            // If both used the same prefix, might have to look at more than 100 slots (like FindCameraImage function)
            simulateShotImpl(simulatedCameraImageFileInfo.FullName);
        }

        private void buttonBrowseForSimulatedShot_Click(object sender, EventArgs e)
        {
            // Sticks copy of browsed image into bluetooth folder.
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "JPEG files (*.jpg)|*.jpg";// + "|" + "All files (*.*)|*.*";
            dlg.CheckFileExists = true;
            dlg.InitialDirectory = simulatedCameraBrowseFolder; //Path.GetDirectoryName(simulatedCameraImageFileInfo.FullName); //Application.StartupPath;

            if (dlg.ShowDialog() != DialogResult.OK)
                return;
            simulatedCameraBrowseFolder = Path.GetDirectoryName(dlg.FileName); // Remember for next time this session.  Not yet across sesssions.
            Environment.CurrentDirectory = Application.StartupPath; // This helps with problems elsewhere induced by browsing changing the default dir.  Less need for startupPath.
            
            simulateShotImpl(dlg.FileName);            // dlg.FileName has the full path.
        }

        private void simulateShotImpl(string fileToCopy)
        {
            // fileToCopy has full path.
            string fullPathToTarget;
            string target = simulatedCameraImageFileInfo.Name; // We use "img_0001" for both default image and starting place for browse image rename; we could make them different, tho, e.g., "sim_0001"
            // We ignore original name, rename copy to "img_00dd.jpg", where dd are digits.
            int n = 1;
            while (n < 100) // only doing up to 100 for now
            {
                fullPathToTarget = Constants.source + target; // "source" is bluetooth directory, which is the target from point of view of this function.
                if (!File.Exists(fullPathToTarget))
                {
                    File.Copy(fileToCopy, fullPathToTarget);
                    break;
                }
                target = target.Replace(n.ToString("d2"), (++n).ToString("d2")); // replacing 2 digits
            }
        }


        private void FindCameraImage() // Rewritten v 27 for multiple images
        {
            // Looks into bluetooth folder, moves new image (once fully arrived) to "just taken" folder
            // See previous function for name format if simulated image.
            // For real imageRicoh 500se camera uses following conventions:
            // "RIMG0001" is first name when flash card is empty.
            // Additional images bump up 4 digit values (based on max value on memory card; doesn't fill in deleted numbers [well maybe it does if card is full]
            // When sent, bluetooth driver will append suffix if there's a conflict with pre-existing file on laptop, of form e.g.
            // "RIMG0006_2"
            DirectoryInfo di = new DirectoryInfo(Constants.source);
            List<FileInfo> rgFiles = new List<FileInfo>(di.GetFiles());
            rgFiles.Sort(new FileComparer());  // See helper class at end of this file.  Compares by creation date, sorts from oldest to newest
            foreach (FileInfo fi in rgFiles)
            {
                if (!fi.Name.EndsWith(".jpg") && (!fi.Name.EndsWith(".JPG"))) // Ricoh 500se camera uses .JPG
                    continue;
                if (fi.Name == "no-images.jpg")
                    continue;
                FileStream fs = null;
                try
                {
                    fs = new FileStream(fi.FullName, FileMode.Open, FileAccess.Read);
                }
                catch (Exception) ///*Exception ex*/
                {
                    // Assume we are trying to read a file that is still in the process of being written (i.e., sent wirelessly from camera)
                    // So just ignore it this time, we'll get around to it on a future call to here.
                    continue;
                }
                if (fs != null)
                {
                    InBoxData inBox = new InBoxData();
                    inBox.justTakenFileName = fi.Name;
                    inBox.justTakenFilePath = /*startupPath + */justTaken + fi.Name;
                    fs.Close(); // Need to close this in order for File.Move to work.
                    try
                    {
                        // Stock simulated picture is "img_0001.jpg", but we may have already bumped it up in SimulateCameraShot_Click.
                        // We may make multiple instances here, so have to bump number.
                        // Handle up to 9999

                        int ndx = fi.Name.IndexOf('.');
                        if (ndx < 2)
                            ErrBox.Show("Bad simulated image name"); // not real good here
                        // Start n with value given in incoming file
                        string fourdigits;
                        if (fi.Name[ndx - 2] == '_') // assume we're unlikely to encounter suffix with > 1 digit
                            ndx = ndx - 2; // Ignore any suffix that bluetooth driver on laptop added
                        fourdigits = fi.Name.Substring(ndx-4, 4);
                        int n = Convert.ToInt32(fourdigits);
                        while (n < 10000)
                        {
                            if (!File.Exists(inBox.justTakenFilePath))
                            {
                                File.Move(fi.FullName, inBox.justTakenFilePath);
                                break;
                            }
                            // otherwise:
                            inBox.justTakenFileName = inBox.justTakenFileName.Replace(n.ToString("d4"), (++n).ToString("d4")); // replacing 2 digits
                            inBox.justTakenFilePath = /*startupPath + */justTaken + inBox.justTakenFileName;
                        }
                    }
                    catch (Exception ex)
                    {
                        ErrBox.Show("Photo just received wirelessly could not be moved to 'just taken' folder.  Details:\n" + ex.ToString());
                        //MessageBox.Show(ex.Source + ":  " + ex.Message);
                        ErrBox.Show(ex.ToString());
                        break; // Avoid endless loop
                    }
                    try
                    {
                        if (inBoxDataList.Count == 0)
                            inBox.imageRole = ImageRole.Primary;
                        else
                            inBox.imageRole = ImageRole.Unassigned;
                        //inBox.filmstripID = AddToFilmstrip(inBox.justTakenFilePath);
                        AddToFilmstrip(inBox); // reads justTakenFilePath, sets filmstripID, fileStream.
                    }
                    catch (Exception ex)
                    {
                        ErrBox.Show("Photo could not be added to filmstrip; File may be corrupt.  Details follow.");
                        ErrBox.Show(ex.Source + ":  " + ex.Message);
                        break; // Avoid endless loop
                    }
                    inBoxDataList.Add(inBox);
                    if (inBoxDataList.Count == 1)
                    {
                        filmstripControl.SelectedImageID = inBox.filmstripID;
                        photoRoleComboBox.SelectedItem = ImageRole.Primary.ToString();
                        // This will trigger event that will do:
                        // inBoxDataList[0].imageRole = ImageRole.Primary;
                    }
                }
           }// for each
        }

        private void DeleteBadPhotoButton_Click(object sender, EventArgs e) // Rewritten v 27 for multiple images
        {
            int n = filmstripControl.SelectedImageID;
            if (n == FilmstripControl.NO_SELECTION_ID || inBoxDataList.Count == 0)
                return;
            bool found = false;
            foreach (InBoxData ibd in inBoxDataList)
            {
                if (n == ibd.filmstripID)
                {
                    ibd.fileStream.Close(); // Hope this works here, otherwise we'll have to cache filestream, try again after RemoveImage
                    inBoxDataList.Remove(ibd); // remove selected image data
                    found = true;
                    break;
                }
            }
            if (!found)
                ErrBox.Show("Internal error.  Image to be deleted not found in data list.");
            filmstripControl.RemoveImage(filmstripControl.SelectedImageID);
            SelectAndMakePrimaryLeftmostImage();
        }

        private void SelectAndMakePrimaryLeftmostImage()
        {
            // Make the leftmost image (e.g., smallest ID value) the selected one, if there are any images.
            int SENTINAL = 999999;
            int n = SENTINAL;
            bool alreadyAPrimary = false; // Also check if there's already a primary
            foreach (InBoxData ibd in inBoxDataList)
            {
                if (ibd.filmstripID < n)
                    n = ibd.filmstripID;
                if (ibd.imageRole == ImageRole.Primary)
                    alreadyAPrimary = true;
            }
            // n now has minimum ID value.
            if (n != SENTINAL)
            {
                filmstripControl.SelectedImageID = n;
                if (!alreadyAPrimary)
                {
                    inBoxDataList[0].imageRole = ImageRole.Primary;
                    photoRoleComboBox.SelectedItem = inBoxDataList[0].imageRole.ToString();
                }
            }
            SetButtonStates();
        }


        private void GreenSendPhotoButton_Click(object sender, EventArgs e)
        {
            SendPhotoButton("Green");
        }

        private void BHGreenSendPhotoButton_Click(object sender, EventArgs e)
        {
            SendPhotoButton("BH Green");
        }

        private void YellowSendPhotoButton_Click(object sender, EventArgs e)
        {
            SendPhotoButton("Yellow");
        }

        private void RedSendPhotoButton_Click(object sender, EventArgs e)
        {
            SendPhotoButton("Red");
        }

        private void GraySendPhotoButton_Click(object sender, EventArgs e)
        {
            SendPhotoButton("Gray");
        }

        private void BlackSendPhotoButton_Click(object sender, EventArgs e)
        {
            SendPhotoButton("Black");
        }


        private void SendPhotoButton(string zone)  // Rewritten v 27 for multiple images per send
        {
            if (!VerifyImageRoles())
                return;
#if NO
            // If the CTRL key is pressed when the control is clicked, don't actually send email, nor increment patient ID
            bool suppressSendAndIncrement = (bool)(Control.ModifierKeys == Keys.Control);
            // Test it here, but for sake of versimilude during practice, don't make it effective until after gender, ped verifications.
#endif
            // Confirm gender, but ONLY if both checkboxes are unchecked.
            if ((!GenderMaleCheckBox.Checked && !GenderFemaleCheckBox.Checked))
            {
                FormConfirmGender confirmGen = new FormConfirmGender(this); // function call might adjust checkmarks
                confirmGen.Owner = this; // Needed to make CenterParent property work
                confirmGen.ShowInTaskbar = false;
                confirmGen.StartPosition = FormStartPosition.CenterParent;
                confirmGen.ShowDialog(this);
                // need to sleep here to give modal dialog time to go away, redraw main window
                this.Refresh();
            }
            // Confirm peds vs. adult, but ONLY if both checkboxes are unchecked.
            if ((!PedsCheckBox.Checked && !AdultCheckBox.Checked))
            {
                FormConfirmPeds confirmPeds = new FormConfirmPeds(this); // function call might adjust checkmarks
                confirmPeds.Owner = this; // Needed to make CenterParent property work
                confirmPeds.ShowInTaskbar = false;
                confirmPeds.StartPosition = FormStartPosition.CenterParent;
                confirmPeds.ShowDialog(this);
                // need to sleep here to give modal dialog time to go away, redraw main window
                this.Refresh();
            }

            practiceModeChoice = 2; // Normal (i.e., send email, use & increment Patient #)
            patientID_PrefixAndNumber = normalPatientID_Prefix + PatientNumberTextBox.Text; // Normal, until we know otherwise
            // patientID_NumberOnly = PatientNumberTextBox.Text; // short form for outbox
            patientID_OutboxForm = PatientNumberTextBox.Text; // short form for outbox. v .41
            if (checkBoxPracticeMode.Checked)
            {
                FormPracticeMode practiceMode = new FormPracticeMode(this, practicePatientID); // function call might adjust value of practiceModeChoice, practiceID
                practiceMode.Owner = this; // Needed to make CenterParent property work
                practiceMode.ShowInTaskbar = false;
                practiceMode.StartPosition = FormStartPosition.CenterParent;
                practiceMode.ShowDialog(this);
                // need to sleep here to give modal dialog time to go away, redraw main window
                this.Refresh();
            }

/// NO:     if (suppressSendAndIncrement)
            if (practiceModeChoice == 0)
            {
                // Delete
                // Don't bump patient ID counter
                // Don't archive image
                DeleteCurrentPatientPhotosFromFilmstripAndJustTakenFolder();
                ReinitializeFields();
                return;
            }
            if (practiceModeChoice == 1)
            {
                // Substitute Practice ID #, send, then bump practice ID counter
                // Don't bump patient ID counter
                patientID_PrefixAndNumber = practicePatientID_Prefix + practicePatientID;
                //patientID_NumberOnly = practicePatientID.ToString(); // short form for outbox
                patientID_OutboxForm = practicePatientID_OutboxPrefix + practicePatientID;
            }

            // collect info as dynamic arrays:
            ArrayList attachmentsListShortFileName = new ArrayList();
            ArrayList attachmentsListFullFileName = new ArrayList();
            ArrayList anonAttachmentsListShortFileName = new ArrayList();
            ArrayList anonAttachmentsListFullFileName = new ArrayList();
            //string from = "Glenn_Pearson@nlm.nih.gov";
            //string to = "Glenn_Pearson@nlm.nih.gov";
            string subject = "New Disaster Patient #" + patientID_PrefixAndNumber + " Arrived at " + orgAbbrOrShortNameTextBox.Text;
            string anonymizedSubject = "New Disaster Patient Arrived at " + orgAbbrOrShortNameTextBox.Text;
//#if PFIF_TEST
//            string body = FormatEmailBodyAsPFIF(zone);
//#else
            string body = myEmail.FormatEmailBodyPlainText(zone, false); // anonymized?
            string anonymizedBody = myEmail.FormatEmailBodyPlainText(zone, true); // anonymized?
//#endif
            string shortNamePFIF = myEmail.FormatEmailAttachmentAsPFIF(zone);
            string fullNamePFIF = null;
            if (shortNamePFIF != null) // May be null if no patient name given
                fullNamePFIF = dest1 + shortNamePFIF;
#if PTL
            string shortNameEDXL_and_PTL = myEmail.FormatEmailAttachmentAsEDXL_and_PTL(zone);
            string fullNameEDXL_and_PTL = null;
            if (shortNameEDXL_and_PTL != null)
                fullNameEDXL_and_PTL = dest1 + shortNameEDXL_and_PTL;
#else
            string shortNameEDXL_and_LPF = myEmail.FormatEmailAttachmentAsEDXL_and_LPF(zone);
            string fullNameEDXL_and_LPF = null;
            if (shortNameEDXL_and_LPF != null)
                fullNameEDXL_and_LPF = dest1 + shortNameEDXL_and_LPF;
#endif
            string hasPicAbbr;
//            string destinationPicPath = null;
//            string shortPicName = null;
//            if (imageShownFileInfo == null || imageShownFileInfo.Name == "")
            if(!hasPrimary())
            {
                hasPicAbbr = "N";
/* MOVED:
                if (!NoTimeForPhotoCheckBox.Checked && !ForgotPhotoCheckBox.Checked) // more to come
                {
                    MessageBox.Show("Indicate why no photo, then send again");
                    return;
                }
 */
            }
            else
            {
                // Otherwise, we have a picture (or pictures) to send.
                // Move them to processed folder with renaming, and add to attachments list.
                hasPicAbbr = "Y";
                string shortPicName = null;
                int countSecondaryPics = 0;
                for (int i = 0; i < inBoxDataList.Count; i++)
                {
                    if (inBoxDataList[i].imageRole == ImageRole.Unassigned)
                        continue;

                    shortPicName = patientID_PrefixAndNumber + " " + zone;
                    if (inBoxDataList[i].imageRole == ImageRole.Secondary)
                        shortPicName += " s" + (++countSecondaryPics).ToString();
                    if (inBoxDataList[i].captionExtra != null && inBoxDataList[i].captionExtra.Length > 0)
                        shortPicName += " - " + inBoxDataList[i].captionExtra;

                    shortPicName += ".jpg";

                    inBoxDataList[i].processedFileName = shortPicName;
                    inBoxDataList[i].processedFilePath = /*startupPath +*/ dest1 + shortPicName;

                    // Problem here if image handle not closed
                    // System.IO.File.Move(inBoxDataList[i].justTakenFilePath, inBoxDataList[i].processedFilePath);
                    // Instead copy, delete original later
                    File.Copy(inBoxDataList[i].justTakenFilePath, inBoxDataList[i].processedFilePath);

                    attachmentsListFullFileName.Add(inBoxDataList[i].processedFilePath);
                    attachmentsListShortFileName.Add(inBoxDataList[i].processedFileName);
                }     
            }
            // Send anonymized first, so red Sent message at bottom of "Main" tab will persist with more informative full record subject
            // emailFrom is an array of strings, 1 per server to try
            if (anonymizedEmailTo != null && anonymizedEmailTo.Length > 0)
            {

                /* WAS:
                                myEmail.MySendMail(emailFrom, anonymizedEmailTo, anonymizedEmailCc, anonymizedEmailBcc, anonymizedSubject, anonymizedBody, null, null,
                                    anonymizedLegaleseFileInfo.FullName, anonymizedLegaleseFileInfo.Name, null, null, null, null);
                 */
                //                myEmail.MySendMail(emailFrom, anonymizedEmailTo, anonymizedEmailCc, anonymizedEmailBcc, subject, body, destination, shortName,
                //                    legaleseFileInfo.FullName, legaleseFileInfo.Name, fullNamePFIF, shortNamePFIF, fullNameEDXL_and_PTL, shortNameEDXL_and_PTL);
                if (anonymizedLegaleseFileInfo.Name != null) // sanity check
                {
                    anonAttachmentsListFullFileName.Add(anonymizedLegaleseFileInfo.FullName);
                    anonAttachmentsListShortFileName.Add(anonymizedLegaleseFileInfo.Name);
                }
                string[] anonAttachmentsShortFileName = new string[1];
                string[] anonAttachmentsFullFileName = new string[1]; // [anonAttachmentsListFullFileName.Count];
                anonAttachmentsShortFileName = (string[])anonAttachmentsListShortFileName.ToArray(typeof(string));
                anonAttachmentsFullFileName = (string[])anonAttachmentsListFullFileName.ToArray(typeof(string));

                myEmail.MySendMail(emailFrom, anonymizedEmailTo, anonymizedEmailCc, anonymizedEmailBcc,
                    anonymizedSubject, anonymizedBody, anonAttachmentsFullFileName, anonAttachmentsShortFileName);
            }

            if (emailTo.Length > 0)  // And this should always be true, because we at least send to LPF
            {
                // Pictures were added to these lists above.
                if (legaleseFileInfo.Name != null) // sanity check
                {
                    attachmentsListFullFileName.Add(legaleseFileInfo.FullName);
                    attachmentsListShortFileName.Add(legaleseFileInfo.Name);
                }
                if (shortNamePFIF != null) // If no patient name given, suppress PFIF
                {
                    attachmentsListFullFileName.Add(fullNamePFIF);
                    attachmentsListShortFileName.Add(shortNamePFIF);
                }
#if PTL
                if (shortNameEDXL_and_PTL != null)
                {
                    attachmentsListFullFileName.Add(fullNameEDXL_and_PTL);
                    attachmentsListShortFileName.Add(shortNameEDXL_and_PTL);
                }
#else
                if (shortNameEDXL_and_LPF != null)
                {
                    attachmentsListFullFileName.Add(fullNameEDXL_and_LPF);
                    attachmentsListShortFileName.Add(shortNameEDXL_and_LPF);
                }
#endif

                string[] attachmentsShortFileName = new string[attachmentsListShortFileName.Count];
                string[] attachmentsFullFileName = new string[attachmentsListFullFileName.Count];
                attachmentsShortFileName = (string[])attachmentsListShortFileName.ToArray(typeof(string));
                attachmentsFullFileName = (string[])attachmentsListFullFileName.ToArray(typeof(string));

                myEmail.MySendMail(emailFrom, emailTo, emailCc, emailBcc, subject, body, attachmentsFullFileName, attachmentsShortFileName);
#if PTL
// WAS:                myEmail.MySendMail(emailFrom, emailTo, emailCc, emailBcc, subject, body, destinationPicPath, shortPicName,
// WAS:                    legaleseFileInfo.FullName, legaleseFileInfo.Name, fullNamePFIF, shortNamePFIF, fullNameEDXL_and_PTL, shortNameEDXL_and_PTL);
                lastSentEDXL_and_PTL = fullNameEDXL_and_PTL;
#else
// WAS:                myEmail.MySendMail(emailFrom, emailTo, emailCc, emailBcc, subject, body, destinationPicPath, shortPicName,
// WAS:                    legaleseFileInfo.FullName, legaleseFileInfo.Name, fullNamePFIF, shortNamePFIF, fullNameEDXL_and_LPF, shortNameEDXL_and_LPF);
                lastSentEDXL_and_LPF = fullNameEDXL_and_LPF;
#endif
                lastSentPFIF = fullNamePFIF;
// WAS:                lastSentPic = destinationPicPath;
                foreach (string attach in attachmentsFullFileName)
                {
                    if (attach.Contains(".jpg"))
                        lastSentPics.Add(attach); // For benefit of momentarily delayed image archiving
                }
                DeleteCurrentPatientPhotosFromFilmstripAndJustTakenFolder();
                attachmentsListFullFileName.Clear();
                attachmentsListShortFileName.Clear();
            }
            myEmail.AppendEmailHistory(/*patientID_NumberOnly*/ patientID_OutboxForm, zone, hasPicAbbr);
            // Bump counter:
            // Never get here if practiceModeChoice == 0)
            if (practiceModeChoice == 1)
                practicePatientID++;
            if (practiceModeChoice == 2)
                PatientNumberTextBox.Text = (Convert.ToInt32(PatientNumberTextBox.Text) + 1).ToString();
            ReinitializeFields();
        }


        private void DeleteCurrentPatientPhotosFromFilmstripAndJustTakenFolder()
        {
            for (int i = 0; i < inBoxDataList.Count; i++)
            {
                if (inBoxDataList[i].imageRole == ImageRole.Unassigned)
                    continue;

                filmstripControl.RemoveImage(inBoxDataList[i].filmstripID);
                inBoxDataList[i].fileStream.Close(); //  This might fix problems below
                // Now that filmstripControl no longer has handle to 'just taken' file, delete it (but copy persists in 'processed' [except for Delete of practice mode])
                // Still doesn't work: File.Delete(inBoxDataList[i].justTakenFilePath);
                deleteJustTakenPics.Add(inBoxDataList[i].justTakenFilePath); // so schedule it for shortly later.
            }
            SetButtonStates();
            // Do this as a separate loop to avoid screwing up index in preceding loop.
            // In reverse order so that removed items don't affect remaining items to be inspected.
            for (int i = inBoxDataList.Count - 1; i >= 0; i--)
            {
                if (inBoxDataList[i].imageRole == ImageRole.Unassigned)
                    continue;

                inBoxDataList.RemoveAt(i);
            }
            SelectAndMakePrimaryLeftmostImage();
        }


        private void ReinitializeFields()
        {
            // PatientNumberTextBox is intentionally handled elsewhere.
            GenderMaleCheckBox.Checked = false;
            GenderFemaleCheckBox.Checked = false;
            PedsCheckBox.Checked = false;
            AdultCheckBox.Checked = false;
            NoTimeForPhotoCheckBox.Checked = false;
            ForgotPhotoCheckBox.Checked = false;
            FirstNameTextBox.Text = "";
            MiddleNameTextBox.Text = "";
            LastNameTextBox.Text = "";
            NameSuffixTextBox.Text = "";
            NickNameTextBox.Text = "";
        }

        #endregion
        #region Shut down
        public void TriagePhoto_FormClosed(object sender, FormClosedEventArgs e)
        {
            myEmail.WriteEmailSettings(op);

            op.StationPatientTrackingOfficerText = patientTrackingOfficer;
            op.StationTriagePhysicansOrRNsText = triagePhysiciansOrRNs;
            op.StationOtherStaffText = otherStationStaff;
            op.StationPhotographersText = photographers;

//          op.EventNameText = EventName;
            op.EventTypeText = eventSuffix;
            op.EventRangeInt = eventRange;

            op.PatientNumberText = PatientNumberTextBox.Text;
            op.PracticePatientNumberText = practicePatientID.ToString(); 

            op.OrgNameText = orgNameTextBox.Text;
            op.OrgAbbrOrShortNameText = orgAbbrOrShortNameTextBox.Text;
            op.OrgStreetAddress1Text = orgStreetAddress1TextBox.Text;
            op.OrgStreetAddress2Text = orgStreetAddress2TextBox.Text;
            op.OrgTownOrCityText = orgTownOrCityTextBox.Text;
            op.OrgCountyText = orgCountyTextBox.Text;
            op.Org2LetterStateText = org2LetterStateTextBox.Text;
            op.OrgZipcodeText = orgZipcodeTextBox.Text;
            op.OrgPhoneText = orgPhoneTextBox.Text;
            op.OrgFaxText = orgFaxTextBox.Text;
            op.OrgEmailText = orgEmailTextBox.Text;
            op.OrgWebSiteText = orgWebSiteTextBox.Text;
            op.OrgNPI_Text = orgNPI_TextBox.Text;
            op.OrgPatientID_PrefixText = orgPatientID_Prefix.Text;

            op.WriteOptionsToFile();
        }
        #endregion
        // ========= OTHER SIMPLE ON-CLICK AND ON-TEXT-CHANGED HANDLERS ==============
        #region "Checklist" Tab
        // === "Checklist" Tab:

        private void EventNameComboBox_TextChanged(object sender, EventArgs e)
        {
            if (EventNameComboBox.Text.Length == 0)
                FormEventNameLabel.Text = eventSuffix;
            else
                FormEventNameLabel.Text = EventNameComboBox.Text + " - " + eventSuffix;

        }
        
        private void EventTypeRadio1_Click(object sender, EventArgs e)
        {
            eventSuffix = "TEST or DEMO";
            EventNameComboBox_TextChanged(sender, e);
        }

        private void EventTypeRadio2_Click(object sender, EventArgs e)
        {
            eventSuffix = "DRILL";
            EventNameComboBox_TextChanged(sender, e);
        }

        private void EventTypeRadio3_Click(object sender, EventArgs e)
        {
            eventSuffix = "REAL - NOT A DRILL"; // Before v 41, was: "REAL DISASTER - NOT A DRILL", but shortened to fit better at top
            EventNameComboBox_TextChanged(sender, e);
        }

        // To make radius pic, site http://www.freemaptools.com/radius-around-point.htm was used.
        private void EventRangeRadioButton1_Click(object sender, EventArgs e)
        {
            eventRange = 1; // 25 miles from Bethesda
        }

        private void EventRangeRadioButton2_Click(object sender, EventArgs e)
        {
            eventRange = 2; // 50 miles from Bethesda
        }
        private void EventRangeRadioButton3_Click(object sender, EventArgs e)
        {
            eventRange = 3; // 100 miles from Bethesda
        }

        private void EventRangeRadioButton4_Click(object sender, EventArgs e)
        {
            eventRange = 4; // MD, DC, VA, WV, DE, PA, NJ
        }
        private void PatientTrackingOfficerTextBox_TextChanged(object sender, EventArgs e)
        {
            patientTrackingOfficer = PatientTrackingOfficerTextBox.Text;
        }

        private void TriagePhysicansOrRNsTextBox_TextChanged(object sender, EventArgs e)
        {
            triagePhysiciansOrRNs = TriagePhysicansOrRNsTextBox.Text;
        }

        private void OtherStationStaff_TextChanged(object sender, EventArgs e)
        {
            otherStationStaff = OtherStationStaff.Text;
        }

        private void PhotographersTextBox_TextChanged(object sender, EventArgs e)
        {
            photographers = PhotographersTextBox.Text;
        }
        #endregion
        #region "Distribution" Tab
        // === "Distribution" Tab:

        
        private void FullRecordToListTextBox_TextChanged(object sender, EventArgs e)
        {
            emailTo = FullRecordToListTextBox.Text;
        }

        private void FullRecordCcListTextBox_TextChanged(object sender, EventArgs e)
        {
            emailCc = FullRecordCcListTextBox.Text;
        }

        private void FullRecordBccListTextBox_TextChanged(object sender, EventArgs e)
        {
            emailBcc = FullRecordBccListTextBox.Text;
        }

        private void AnonymizedToListTextBox_TextChanged(object sender, EventArgs e)
        {
            anonymizedEmailTo = AnonymizedToListTextBox.Text;
        }

        private void AnonymizedCcListTextBox_TextChanged(object sender, EventArgs e)
        {
            anonymizedEmailCc = AnonymizedCcListTextBox.Text;
        }

        private void AnonymizedBccListTextBox_TextChanged(object sender, EventArgs e)
        {
            anonymizedEmailBcc = AnonymizedBccListTextBox.Text;
        }

        private void FullRecordToListTextBox_Validating(object sender, CancelEventArgs e)
        {
            // emailTo or anonymizedEmailTo might be null if not read in from XML.
            if ((emailTo == null || emailTo.Length == 0) &&
                (anonymizedEmailTo == null || anonymizedEmailTo.Length == 0))
            {
                MessageBox.Show("One of the 'To' fields must be filled in.", "Error");
                e.Cancel = true;
            }
            ValidateAddressList(emailTo, "Full-Record 'To'", e);
        }

        private void FullRecordCcListTextBox_Validating(object sender, CancelEventArgs e)
        {
            ValidateAddressList(emailCc, "Full-Record 'CC'", e);
        }

        private void FullRecordBccListTextBox_Validating(object sender, CancelEventArgs e)
        {
            ValidateAddressList(emailBcc, "Full-Record 'BCC'", e);
        }

        private void AnonymizedToListTextBox_Validating(object sender, CancelEventArgs e)
        {
            // emailTo or anonymizedEmailTo might be null if not read in from XML.
            if ((emailTo == null || emailTo.Length == 0) &&
                (anonymizedEmailTo == null || anonymizedEmailTo.Length == 0))
            {
                MessageBox.Show("One of the 'To' fields must be filled in.", "Error");
                e.Cancel = true;
            }
            ValidateAddressList(anonymizedEmailTo, "Anonymized 'To'", e);
        }

        private void AnonymizedCcListTextBox_Validating(object sender, CancelEventArgs e)
        {
            ValidateAddressList(anonymizedEmailCc, "Anonymized 'CC'", e);
        }

        private void AnonymizedBccListTextBox_Validating(object sender, CancelEventArgs e)
        {
            ValidateAddressList(anonymizedEmailBcc, "Anonymized 'BCC'", e);
        }

        private void ValidateAddressList(string addressList, string FieldDescriptionOnError, CancelEventArgs e)
        {
            if (addressList == null || addressList.Length == 0)
                return; // Handles cases where email address is optional.  Caller can do cases where it's not.
            string[] addresses = addressList.Split(';');
            string address; // temp used, cause can't write to foreach variable
            foreach (string address2 in addresses)
            {
                // Add a recipient.
                address = address2.Trim();
                if (address.Length == 0)
                    continue; // tolerates semicolon at end of last name in email list, or duplicate semicolons in a row
                int n = address.IndexOf('<');
                if (n < 0)
                {
                    if (IsValidEmail(address))
                        continue;
                    // Here on error
                    MessageBox.Show("The " + FieldDescriptionOnError + " field has an entry with bad syntax or unknown domain: " + address, "Error");
                    e.Cancel = true;
                    return;
                }
                else
                {
                    string nice = address.Substring(0, n);
                    nice = nice.Trim();
                    string unnice = address.Substring(n + 1); // skip '<'.
                    unnice = unnice.Trim();
                    if (unnice[unnice.Length - 1] != '>')
                    {
                        // Here on error
                        MessageBox.Show("The " + FieldDescriptionOnError + " field has an entry with missing '>': " + nice, "Error");
                        e.Cancel = true;
                        return;
                    }
                    unnice = unnice.Remove(unnice.Length - 1); //  Since last char is '>', skip it too.
                    if (!IsValidEmail(unnice))
                    {
                        // Here on error
                        MessageBox.Show("The " + FieldDescriptionOnError + " field has an entry with bad syntax or unknown domain: " + unnice, "Error");
                        e.Cancel = true;
                        return;
                    }
                }
            }
        }

        private bool IsValidEmail(string s)
        {
            if (s == null || s.Length == 0)
                return (true); // Handles cases where email address is optional.  Caller can do cases where it's not.
            // For best discussion of possible email address regular expresssions, see Jan Goyverts' site, specifically
            // www.regular-expressions.info/email.html.  The version used here is "rfc 2822" without deprecated syntax
            // but with additional top-level domains enumerated (and ordered by length and alpha order).
            // For the code body, see article "Effective Email Address Validation" by Vasudevan Deepak Kumar
            // (www.codeproject.com/KB/validation/Valid_Email_Addresses....)
            // To see the regular expression below as a human-understandable tree structure, copy it, remove the C# string syntax, paste it into
            // a regular expression analyzer like http://xenon.stanford.edu/~xusch/regexp/analyzer.html
            // For top-level domains, this accepts any 2-letter code (rather than enumerate about 200 country codes), plus specific domains defined as
            // of Dec. 2009 (found in wikipedia) with 3-6 letters.

            string strRegex =
            @"^[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+" +
            @"(?:[A-Z]{2}|biz|cat|com|edu|gov|int|org|mil|net|pro|tel|aero|asia|coop|info|jobs|mobi|name|travel|museum)$"; // should we include 'arpa' ?
            Regex re = new Regex(strRegex, RegexOptions.IgnoreCase);
            if (re.IsMatch(s))
                return (true);
            return (false);
        }
        #endregion
        #region "Hospital" Tab
        // === "Hospital" tab:

        private void orgPatientID_Prefix_TextChanged(object sender, EventArgs e)
        {
            normalPatientID_Prefix = PatientNumberPrefix.Text = orgPatientID_Prefix.Text;
        }
        #endregion
        #region "Outbox" Tab
        // === "Outbox" Tab:

        private void outboxDataGridView_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            outboxDataGridView.Columns["WhenLocalTime"].Visible = false; // This is what's serialized to XML, and it's a string.
            // The user will manipulate "When" instead, which is a DateTimeOffset object with better sortability.
            // Just hide WhenLocalTime, don't remove.  Whenever a new patient causes a new row to be added, we must programmatically add When and WhenLocalTime.
            outboxDataGridView.Columns["TZ"].Visible = false; // Ditto for timezone abbreviation
            //outboxDataGridView.AutoResizeColumns();
            outboxDataGridView.Columns["Sent"].Width = 40;
            outboxDataGridView.Columns["Zone"].Width = 50;
            outboxDataGridView.Columns["Sex"].Width = 40;
            outboxDataGridView.Columns["Peds"].Width = 45;
            outboxDataGridView.Columns["Pic"].Width = 30;
            outboxDataGridView.Columns["Event"].Width = 45; // was: "Other", width = 45

            // Column # & name in data table    Visible in grid?
            // =============================    ================
            // [0] When (DateTimeOffset)        Yes
            // [1] WhenLocalTime (string)       No
            // [2] TZ                           No
            // [3] Sent                         Yes
            // [4] Zone                         Yes
            // [5] Sex                          Yes
            // [6] Peds                         Yes
            // [7] Pic                          Yes
            // [8] Event                        Yes
            foreach (DataGridViewBand band in outboxDataGridView.Columns)
                band.ReadOnly = true;
            SumPatientArrivals();
        }

        private void outboxDataGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            string colr;
            if (outboxDataGridView.Columns[e.ColumnIndex].Name == "Zone")
            {
//            for (int k = 0; k < outboxDataSet.Tables[0].Rows.Count; k++)
//            {
                // Cell[3] is "Zone"
                colr = e.Value.ToString(); //outboxDataGridView.Rows[k].Cells[3].Value.ToString();
                switch (colr)
                {
                        // matches color buttons on main page
                    case "Green":
                        e.CellStyle.BackColor = /* outboxDataGridView.Rows[k].Cells[3].Style.BackColor = */ Color.Lime; break;
                    case "BH Green":
                        e.CellStyle.BackColor = /* outboxDataGridView.Rows[k].Cells[3].Style.BackColor = */ Color.PaleGreen; break;
                    case "Yellow":
                        e.CellStyle.BackColor = /* outboxDataGridView.Rows[k].Cells[3].Style.BackColor = */ Color.Yellow; break;
                    case "Red":
                        e.CellStyle.BackColor = /* outboxDataGridView.Rows[k].Cells[3].Style.BackColor = */ Color.Red; break;
                    case "Gray":
                        e.CellStyle.BackColor = /* outboxDataGridView.Rows[k].Cells[3].Style.BackColor = */ Color.DarkGray; break;
                    case "Black":
                        e.CellStyle.BackColor = /* outboxDataGridView.Rows[k].Cells[3].Style.BackColor = */ Color.Black;
                        e.CellStyle.ForeColor = Color.White; break;
                    default: break;
                }
            }
        }


        private void outboxDataGridView_CellEndEdit(object sender, DataGridViewCellEventArgs e) // Do we need this?
        {
            SumPatientArrivals();
        }

        private void outboxDataGridView_Validated(object sender, EventArgs e) // Do we need this?
        {
            SumPatientArrivals();
        }

        private void comboBoxOutboxID_Type_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (outboxDataSet != null) // suppress until further in the process the first time
                SumPatientArrivals();
        }

        private void SumPatientArrivals()
        {
            bool today, last24hours, thisEvent;
            labelTodaysTotalAll.Text = "0"; label24HrsTotalAll.Text = "0"; labelEventTotalAll.Text = "0"; labelShownTotalAll.Text = "0";

            labelTodaysTotalGreen.Text = "0"; labelTodaysTotalBHGreen.Text = "0"; labelTodaysTotalYellow.Text = "0";
            labelTodaysTotalRed.Text = "0"; labelTodaysTotalGray.Text = "0"; labelTodaysTotalBlack.Text = "0";
            labelTodaysTotalMales.Text = "0"; labelTodaysTotalFemales.Text = "0"; labelTodaysTotalUnknownOrComplexGender.Text = "0";
            labelTodaysTotalPeds.Text = "0"; labelTodaysTotalAdults.Text = "0"; labelTodaysTotalPedsNotGiven.Text = "0";
            label24HrsTotalGreen.Text = "0"; label24HrsTotalBHGreen.Text = "0"; label24HrsTotalYellow.Text = "0";
            label24HrsTotalRed.Text = "0"; label24HrsTotalGray.Text = "0"; label24HrsTotalBlack.Text = "0";
            label24HrsTotalMales.Text = "0"; label24HrsTotalFemales.Text = "0"; label24HrsTotalUnknownOrComplexGender.Text = "0";
            label24HrsTotalPeds.Text = "0"; label24HrsTotalAdults.Text = "0"; label24HrsTotalPedsNotGiven.Text = "0";
            labelEventTotalGreen.Text = "0"; labelEventTotalBHGreen.Text = "0"; labelEventTotalYellow.Text = "0";
            labelEventTotalRed.Text = "0"; labelEventTotalGray.Text = "0"; labelEventTotalBlack.Text = "0";
            labelEventTotalMales.Text = "0"; labelEventTotalFemales.Text = "0"; labelEventTotalUnknownOrComplexGender.Text = "0";
            labelEventTotalPeds.Text = "0"; labelEventTotalAdults.Text = "0"; labelEventTotalPedsNotGiven.Text = "0";
            labelShownTotalGreen.Text = "0"; labelShownTotalBHGreen.Text = "0"; labelShownTotalYellow.Text = "0";
            labelShownTotalRed.Text = "0"; labelShownTotalGray.Text = "0"; labelShownTotalBlack.Text = "0";
            labelShownTotalMales.Text = "0"; labelShownTotalFemales.Text = "0"; labelShownTotalUnknownOrComplexGender.Text = "0";
            labelShownTotalPeds.Text = "0"; labelShownTotalAdults.Text = "0"; labelShownTotalPedsNotGiven.Text = "0";
            for (int k = 0; k < outboxDataSet.Tables[0].Rows.Count; k++)
            {
                if (outboxDataGridView.Rows[k].Cells[0].Value.ToString() == null || outboxDataGridView.Rows[k].Cells[0].Value.ToString().Length == 0)
                    continue; // kludge to ignore extra event calls

                if (comboBoxOutboxID_Type.SelectedIndex == 0) // official diaster patient ID; default normal setting
                {
                    string id = outboxDataGridView.Rows[k].Cells[4].Value.ToString();
                    if (id.Length > practicePatientID_OutboxPrefix.Length)
                    {
                        // Might be practice patient... check further:
                        id = id.Substring(0, practicePatientID_OutboxPrefix.Length);
                        if (id == practicePatientID_OutboxPrefix)
                            continue; // skip practice patients
                    }
                }

                if (comboBoxOutboxID_Type.SelectedIndex == 1) // TriagePic practice patients
                {
                    string id = outboxDataGridView.Rows[k].Cells[4].Value.ToString();
                    if (id.Length <= practicePatientID_OutboxPrefix.Length)
                        continue; // must be non-practice patient, so skip
                    id = id.Substring(0, practicePatientID_OutboxPrefix.Length);
                    if (id != practicePatientID_OutboxPrefix)
                        continue; // skip non-practice patients
                }
                // if (comboBoxOutboxID_Type.SelectedIndex == 2), show both types of patients.  No action needed here.

                // Parse will fill in missing year with current year.  Not always what we want
                DateTime timestamp;
                DateTimeOffset dto;
                dto = (DateTimeOffset)outboxDataGridView.Rows[k].Cells["When"].Value; // not hidden column "WhenLocalTime".  "When" is a DateTimeOffset
                // Although the user doesn't see the year displayed in the grid (because of our choice of column formatting),
                // it is contained in the value, and used in comparisons below.
                timestamp = dto.DateTime; // TO DO: use DateTimeOffset below instead of DateTime

                today = (bool)(timestamp.ToLongDateString() == System.DateTime.Now.ToLongDateString());
                if (today)
                {
                    BumpCount(labelTodaysTotalAll);

                    if (outboxDataGridView.Rows[k].Cells[3].Value.ToString() == "Green")
                    {
                        BumpCount(labelTodaysTotalGreen);
                    }
                    if (outboxDataGridView.Rows[k].Cells[3].Value.ToString() == "BH Green")
                    {
                        BumpCount(labelTodaysTotalBHGreen);
                    }
                    if (outboxDataGridView.Rows[k].Cells[3].Value.ToString() == "Yellow")
                    {
                        BumpCount(labelTodaysTotalYellow);
                    }
                    if (outboxDataGridView.Rows[k].Cells[3].Value.ToString() == "Red")
                    {
                        BumpCount(labelTodaysTotalRed);
                    }
                    if (outboxDataGridView.Rows[k].Cells[3].Value.ToString() == "Gray")
                    {
                        BumpCount(labelTodaysTotalGray);
                    }
                    if (outboxDataGridView.Rows[k].Cells[3].Value.ToString() == "Black")
                    {
                        BumpCount(labelTodaysTotalBlack);
                    }

                    if (outboxDataGridView.Rows[k].Cells[4].Value.ToString() == "M")
                    {
                        BumpCount(labelTodaysTotalMales);
                    }
                    if (outboxDataGridView.Rows[k].Cells[4].Value.ToString() == "F")
                    {
                        BumpCount(labelTodaysTotalFemales);
                    }
                    if (outboxDataGridView.Rows[k].Cells[4].Value.ToString() == "C" || outboxDataGridView.Rows[k].Cells[4].Value.ToString() == "")
                    {
                        BumpCount(labelTodaysTotalUnknownOrComplexGender);
                    }

                    if (outboxDataGridView.Rows[k].Cells[5].Value.ToString() == "Y")
                    {
                        BumpCount(labelTodaysTotalPeds);
                    }
                    if (outboxDataGridView.Rows[k].Cells[5].Value.ToString() == "N")
                    {
                        BumpCount(labelTodaysTotalAdults);
                    }
                    if (outboxDataGridView.Rows[k].Cells[5].Value.ToString() == "")
                    {
                        BumpCount(labelTodaysTotalPedsNotGiven);
                    }
                }
                last24hours = (bool)(DateTime.Compare(timestamp, System.DateTime.Now.AddHours(-24)) >= 0);
                if (last24hours)
                {
                    BumpCount(label24HrsTotalAll);

                    if (outboxDataGridView.Rows[k].Cells[3].Value.ToString() == "Green")
                    {
                        BumpCount(label24HrsTotalGreen);
                    }
                    if (outboxDataGridView.Rows[k].Cells[3].Value.ToString() == "BH Green")
                    {
                        BumpCount(label24HrsTotalBHGreen);
                    }
                    if (outboxDataGridView.Rows[k].Cells[3].Value.ToString() == "Yellow")
                    {
                        BumpCount(label24HrsTotalYellow);
                    }
                    if (outboxDataGridView.Rows[k].Cells[3].Value.ToString() == "Red")
                    {
                        BumpCount(label24HrsTotalRed);
                    }
                    if (outboxDataGridView.Rows[k].Cells[3].Value.ToString() == "Gray")
                    {
                        BumpCount(label24HrsTotalGray);
                    }
                    if (outboxDataGridView.Rows[k].Cells[3].Value.ToString() == "Black")
                    {
                        BumpCount(label24HrsTotalBlack);
                    }

                    if (outboxDataGridView.Rows[k].Cells[4].Value.ToString() == "M")
                    {
                        BumpCount(label24HrsTotalMales);
                    }
                    if (outboxDataGridView.Rows[k].Cells[4].Value.ToString() == "F")
                    {
                        BumpCount(label24HrsTotalFemales);
                    }
                    if (outboxDataGridView.Rows[k].Cells[4].Value.ToString() == "C" || outboxDataGridView.Rows[k].Cells[4].Value.ToString() == "")
                    {
                        BumpCount(label24HrsTotalUnknownOrComplexGender);
                    }

                    if (outboxDataGridView.Rows[k].Cells[5].Value.ToString() == "Y")
                    {
                        BumpCount(label24HrsTotalPeds);
                    }
                    if (outboxDataGridView.Rows[k].Cells[5].Value.ToString() == "N")
                    {
                        BumpCount(label24HrsTotalAdults);
                    }
                    if (outboxDataGridView.Rows[k].Cells[5].Value.ToString() == "")
                    {
                        BumpCount(label24HrsTotalPedsNotGiven);
                    }
                }
                thisEvent = (bool)(FormEventNameLabel.Text == outboxDataGridView.Rows[k].Cells[9].Value.ToString());
                if (thisEvent)
                {
                    BumpCount(labelEventTotalAll);

                    if (outboxDataGridView.Rows[k].Cells[3].Value.ToString() == "Green")
                    {
                        BumpCount(labelEventTotalGreen);
                    }
                    if (outboxDataGridView.Rows[k].Cells[3].Value.ToString() == "BH Green")
                    {
                        BumpCount(labelEventTotalBHGreen);
                    }
                    if (outboxDataGridView.Rows[k].Cells[3].Value.ToString() == "Yellow")
                    {
                        BumpCount(labelEventTotalYellow);
                    }
                    if (outboxDataGridView.Rows[k].Cells[3].Value.ToString() == "Red")
                    {
                        BumpCount(labelEventTotalRed);
                    }
                    if (outboxDataGridView.Rows[k].Cells[3].Value.ToString() == "Gray")
                    {
                        BumpCount(labelEventTotalGray);
                    }
                    if (outboxDataGridView.Rows[k].Cells[3].Value.ToString() == "Black")
                    {
                        BumpCount(labelEventTotalBlack);
                    }

                    if (outboxDataGridView.Rows[k].Cells[4].Value.ToString() == "M")
                    {
                        BumpCount(labelEventTotalMales);
                    }
                    if (outboxDataGridView.Rows[k].Cells[4].Value.ToString() == "F")
                    {
                        BumpCount(labelEventTotalFemales);
                    }
                    if (outboxDataGridView.Rows[k].Cells[4].Value.ToString() == "C" || outboxDataGridView.Rows[k].Cells[4].Value.ToString() == "")
                    {
                        BumpCount(labelEventTotalUnknownOrComplexGender);
                    }

                    if (outboxDataGridView.Rows[k].Cells[5].Value.ToString() == "Y")
                    {
                        BumpCount(labelEventTotalPeds);
                    }
                    if (outboxDataGridView.Rows[k].Cells[5].Value.ToString() == "N")
                    {
                        BumpCount(labelEventTotalAdults);
                    }
                    if (outboxDataGridView.Rows[k].Cells[5].Value.ToString() == "")
                    {
                        BumpCount(labelEventTotalPedsNotGiven);
                    }
                }
                // All events shown:
                BumpCount(labelShownTotalAll);

                if (outboxDataGridView.Rows[k].Cells[3].Value.ToString() == "Green")
                {
                    BumpCount(labelShownTotalGreen);
                }
                if (outboxDataGridView.Rows[k].Cells[3].Value.ToString() == "BH Green")
                {
                    BumpCount(labelShownTotalBHGreen);
                }
                if (outboxDataGridView.Rows[k].Cells[3].Value.ToString() == "Yellow")
                {
                    BumpCount(labelShownTotalYellow);
                }
                if (outboxDataGridView.Rows[k].Cells[3].Value.ToString() == "Red")
                {
                    BumpCount(labelShownTotalRed);
                }
                if (outboxDataGridView.Rows[k].Cells[3].Value.ToString() == "Gray")
                {
                    BumpCount(labelShownTotalGray);
                }
                if (outboxDataGridView.Rows[k].Cells[3].Value.ToString() == "Black")
                {
                    BumpCount(labelShownTotalBlack);
                }

                if (outboxDataGridView.Rows[k].Cells[4].Value.ToString() == "M")
                {
                    BumpCount(labelShownTotalMales);
                }
                if (outboxDataGridView.Rows[k].Cells[4].Value.ToString() == "F")
                {
                    BumpCount(labelShownTotalFemales);
                }
                if (outboxDataGridView.Rows[k].Cells[4].Value.ToString() == "C" || outboxDataGridView.Rows[k].Cells[4].Value.ToString() == "")
                {
                    BumpCount(labelShownTotalUnknownOrComplexGender);
                }

                if (outboxDataGridView.Rows[k].Cells[5].Value.ToString() == "Y")
                {
                    BumpCount(labelShownTotalPeds);
                }
                if (outboxDataGridView.Rows[k].Cells[5].Value.ToString() == "N")
                {
                    BumpCount(labelShownTotalAdults);
                }
                if (outboxDataGridView.Rows[k].Cells[5].Value.ToString() == "")
                {
                    BumpCount(labelShownTotalPedsNotGiven);
                }

            }
        }

        private void BumpCount(Label L)
        {
            L.Text = Convert.ToString(Convert.ToInt32(L.Text) + 1);
        }

        #endregion
        #region "Email Setup" Tab
        // "Email Setup" tab:
        private void EmailProfileNameComboBox1_MouseClick(object sender, MouseEventArgs e)
        {
            seeEditProfileRadio1_Click(sender, null);
        }
        
        private void EmailProfileNameComboBox2_MouseClick(object sender, MouseEventArgs e)
        {
            seeEditProfileRadio2_Click(sender, null);
        }

        private void EmailProfileNameComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (suppressEmailProfileNameComboSelectedIndexChanged)
                return;
            if (EmailProfileNameComboBox1.SelectedIndex == -1)
                return; // Ignore initial deselected state of combo
            // TO DO: Verify 1st & 2nd servers are different
            op.EmailProfileSelected[0] = emailProfileSelected[0] = EmailProfileNameComboBox1.SelectedItem.ToString();
            LoadEmailProfile(0, EmailProfileNameComboBox1.SelectedIndex);
            suppressShownEmailProfileTextChangedEvents = true;
            SeePrimaryEmailProfile(true); // Force view to 1st server
            suppressShownEmailProfileTextChangedEvents = false;
        }

        private void EmailProfileNameComboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (suppressEmailProfileNameComboSelectedIndexChanged)
                return;
            if (EmailProfileNameComboBox2.SelectedIndex == -1)
                return; // Ignore initial deselected state of combo
            // TO DO: Save Verify 1st & 2nd servers are different
            op.EmailProfileSelected[1] = emailProfileSelected[1] = EmailProfileNameComboBox2.SelectedItem.ToString();
            LoadEmailProfile(1, EmailProfileNameComboBox2.SelectedIndex);
            suppressShownEmailProfileTextChangedEvents = true;
            SeePrimaryEmailProfile(false); // Force view to 2nd server
            suppressShownEmailProfileTextChangedEvents = false;
        }

        private void LoadEmailProfile(int m, int i)
        {
            bool suppressedStateWhenCalled = suppressEmailSetupUpdate;
            suppressEmailSetupUpdate = true;
            myEmail.SetPrimaryAndSecondaryEmailProfileItems(op, m, i);
            suppressEmailSetupUpdate = suppressedStateWhenCalled; // restored
        }

        public void seeEditProfileRadio1_Click(object sender, EventArgs e)  //also called from Email.cs
        {
            suppressShownEmailProfileTextChangedEvents = true;
            SeePrimaryEmailProfile(true);
            suppressShownEmailProfileTextChangedEvents = false;
        }

        public void seeEditProfileRadio2_Click(object sender, EventArgs e)
        {
            suppressShownEmailProfileTextChangedEvents = true;
            SeePrimaryEmailProfile(false);
            suppressShownEmailProfileTextChangedEvents = false;
        }

        public void SeePrimaryEmailProfile(bool b) // also called from Email.cs
        {
            seeEditProfileRadio1.Checked = b; // These 2 lines are now probably redundant, given mouse click handlers on combo boxes
            seeEditProfileRadio2.Checked = !b;
            ShowSelectedEmailProfile();
        }

        public void ShowSelectedEmailProfile()
        {
            int m = (seeEditProfileRadio1.Checked) ? 0 : 1; // primary or secondary
            suppressShownEmailProfileTextChangedEvents = true;
            EmailProfileTextBox.Text = emailProfileSelected[m];
            EmailFromTextBox.Text = emailFrom[m];
            SMTP_ServerTextBox.Text = emailServer[m];
            EmailProfileCommentsTextBox.Text = emailProfileComments[m];
            UseSSL_CheckBox.Checked = useSSL_ForSMTP[m];
            PortForSMTP_TextBox.Text = portForSMTP[m];
            if (useBasicAuthenticationForSMTP[m])
                BasicAuthentication(true); // Sets UseBasicAuthenticationForSMTP
            else
                BasicAuthentication(false); // By default, prefer login credentials (network authentication)
            BasicAuthNameTextBox.Text = basicAuthName[m];
            BasicAuthPasswordTextBox.Text = basicAuthPassword[m];
            FetchBeforeSendCheckBox.Checked = fetchBeforeSendAuthentication[m];
            POP_ServerTextBox.Text = emailIncomingServer[m];
            SecurePOP_CheckBox.Checked = useSSL_ForIncoming[m];
            PortForPOP_TextBox.Text = portForIncoming[m];
            suppressShownEmailProfileTextChangedEvents = false;
        }

        private void EmailProfileTextBox_TextChanged(object sender, EventArgs e)
        {
            if (suppressShownEmailProfileTextChangedEvents)
                return;
            int m = (seeEditProfileRadio1.Checked) ? 0 : 1;
            string prevEmailProfileSelected = op.EmailProfileSelected[m];
            op.EmailProfileSelected[m] = emailProfileSelected[m] = EmailProfileTextBox.Text;
            // Now update name in BOTH combo boxes too:
            int k1, k2; //temp

            //EmailProfileNameComboBox1.Items.Clear;
            k1 = EmailProfileNameComboBox1.SelectedIndex;
            suppressEmailProfileNameComboSelectedIndexChanged = true;
            EmailProfileNameComboBox1.Items.RemoveAt(k1);
            EmailProfileNameComboBox1.Items.Insert(k1, EmailProfileTextBox.Text);
            EmailProfileNameComboBox1.SelectedIndex = k1; // re-establish index
            suppressEmailProfileNameComboSelectedIndexChanged = false;

            //EmailProfileNameComboBox2.Items.Clear;
            k2 = EmailProfileNameComboBox2.SelectedIndex;
            suppressEmailProfileNameComboSelectedIndexChanged = true;
            EmailProfileNameComboBox2.Items.RemoveAt(k2);
            EmailProfileNameComboBox2.Items.Insert(k2, EmailProfileTextBox.Text);
            EmailProfileNameComboBox2.SelectedIndex = k2; // re-establish index
            suppressEmailProfileNameComboSelectedIndexChanged = false;
            // Pathological case: if both combo boxes have the same selection (i.e., both k instances were the same above), update the name
            // of the other selection too in the data structures:
            int n = (m == 0) ? 1 : 0;
            if (k1 == k2) // e.g., emailProfileSelected[n] == prevEmailProfileSelected)
                op.EmailProfileSelected[n] = emailProfileSelected[n] = EmailProfileTextBox.Text;

            // Instead of EmailSetupUpdateOpObject(), need our own version using pre-change email profile name for search
            int i = 0;
            foreach (string s in op.EmailProfileName)
            {
                // if op.EmailProfileSelected[m] changed, it was handled earlier elsewhere.
                if (s == prevEmailProfileSelected)
                {
                    op.EmailProfileName[i] = EmailProfileTextBox.Text;
                    break;
                }
                i++;
            }

        }

        private void EmailFromTextBox_TextChanged(object sender, EventArgs e)
        {
            if (suppressShownEmailProfileTextChangedEvents)
                return;
            int m = (seeEditProfileRadio1.Checked) ? 0 : 1;
            emailFrom[m] = EmailFromTextBox.Text;
            EmailSetupUpdateOpObject();
        }

        private void SMTP_ServerTextBox_TextChanged(object sender, EventArgs e)
        {
            if (suppressShownEmailProfileTextChangedEvents)
                return; 
            int m = (seeEditProfileRadio1.Checked) ? 0 : 1;
            emailServer[m] = SMTP_ServerTextBox.Text;
            EmailSetupUpdateOpObject();
        }


        private void EmailProfileComments_TextChanged(object sender, EventArgs e)
        {
            if (suppressShownEmailProfileTextChangedEvents)
                return;
            int m = (seeEditProfileRadio1.Checked) ? 0 : 1;
            emailProfileComments[m] = EmailProfileCommentsTextBox.Text;
            EmailSetupUpdateOpObject();
        }

        private void PortForSMTPTextBox_TextChanged(object sender, EventArgs e)
        {
            if (suppressShownEmailProfileTextChangedEvents)
                return;
            int m = (seeEditProfileRadio1.Checked) ? 0 : 1;
            portForSMTP[m] = PortForSMTP_TextBox.Text;
            EmailSetupUpdateOpObject();
        }
        
        private void UseSSL_CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (suppressShownEmailProfileTextChangedEvents)
                return;
            int m = (seeEditProfileRadio1.Checked) ? 0 : 1;
            PortForSMTP_TextBox.Text = UseSSL_ForSMTP_Helper(m, UseSSL_CheckBox.Checked);
            EmailSetupUpdateOpObject();
        }

        private string UseSSL_ForSMTP_Helper(int whichServer, bool isSSL)
        {
            // If START TLS/SSL check changes, put well-known default value in port box
            useSSL_ForSMTP[whichServer] = isSSL;
            if (useSSL_ForSMTP[whichServer])
                portForSMTP[whichServer] = "587"; // recommended.  Older alternative: 465
            else
                portForSMTP[whichServer] = "25";
            // User can subsequently overwrite these values
            return portForSMTP[whichServer];
        }
        
        public void AuthenticateRadioButton1_Click(object sender, EventArgs e)
        {
            BasicAuthentication(false);
            EmailSetupUpdateOpObject(); // Don't have this in "BasicAuthentication" cuz latter may be called elsewhere & shouldn't have update.
        }

        public void AuthenticateRadioButton2_Click(object sender, EventArgs e)
        {
            BasicAuthentication(true);
            EmailSetupUpdateOpObject(); // Don't have this in "BasicAuthentication" cuz latter may be called elsewhere & shouldn't have update.
        }

        private void BasicAuthentication(bool b)
        {
            int m = (seeEditProfileRadio1.Checked) ? 0 : 1;
            AuthenticateRadioButton1.Checked = !b;
            AuthenticateRadioButton2.Checked = b;
            useBasicAuthenticationForSMTP[m] = b;
            BasicAuthNameTextBox.Enabled = b;
            BasicAuthPasswordTextBox.Enabled = b;
            EmailSetupTabPasswordLabel.Enabled = b;
            EmailSetupTabCautionLabel.Enabled = b;
            FetchBeforeSendCheckBox.Enabled = b;  // POP3
            //Guarantee other fields are consistent viz a viz enabling:
            EnableFetchBeforeSendFields(b && FetchBeforeSendCheckBox.Checked);
        }

        private void BasicAuthNameTextBox_TextChanged(object sender, EventArgs e)
        {
            if (suppressShownEmailProfileTextChangedEvents)
                return;
            int m = (seeEditProfileRadio1.Checked) ? 0 : 1;
            basicAuthName[m] = BasicAuthNameTextBox.Text;
            EmailSetupUpdateOpObject();
        }

        private void BasicAuthPasswordTextBox_TextChanged(object sender, EventArgs e)
        {
            if (suppressShownEmailProfileTextChangedEvents)
                return;
            int m = (seeEditProfileRadio1.Checked) ? 0 : 1;
            basicAuthPassword[m] = BasicAuthPasswordTextBox.Text;
            EmailSetupUpdateOpObject();
        }

        private void POP_ServerTextBox_TextChanged(object sender, EventArgs e)
        {
            if (suppressShownEmailProfileTextChangedEvents)
                return; 
            int m = (seeEditProfileRadio1.Checked) ? 0 : 1;
            emailIncomingServer[m] = POP_ServerTextBox.Text;
            EmailSetupUpdateOpObject();
        }

        private void PortForPOP_TextBox_TextChanged(object sender, EventArgs e)
        {
            if (suppressShownEmailProfileTextChangedEvents)
                return;
            int m = (seeEditProfileRadio1.Checked) ? 0 : 1;
            portForIncoming[m] = PortForPOP_TextBox.Text;
            EmailSetupUpdateOpObject();
        }

        private void EmailSetupUpdateOpObject()
        {
            if (suppressEmailSetupUpdate)
                return;
            // Because the op object has more info than parent, we need to update it here, and not wait until app closes like we do with other tab fields
            // Email Setup tab:
            //if (parent.emailFrom[0] == null)
            //    parent.emailFrom[0] = "Glenn_Pearson@nlm.nih.gov";
            //if (parent.emailServer[0] == null)
            //    parent.emailServer[0] = "mailfwd.nih.gov"; // default for prototype

            int m = (seeEditProfileRadio1.Checked) ? 0 : 1;

            int i = 0;
            foreach (string s in op.EmailProfileName)
            {
                // if op.EmailProfileSelected[m] changed, it was handled earlier elsewhere.
                if (s == op.EmailProfileSelected[m])
                {
                    op.EmailProfileName[i] = EmailProfileTextBox.Text;
                    op.EmailFrom[i] = EmailFromTextBox.Text;
                    op.EmailServer[i] = SMTP_ServerTextBox.Text;
                    op.EmailProfileComments[i] = EmailProfileCommentsTextBox.Text;
                    op.UseSSL_ForSMTP[i] = UseSSL_CheckBox.Checked;
                    op.PortForSMTP[i] = Convert.ToInt32(PortForSMTP_TextBox.Text);
                    op.UseBasicAuthenticationForSMTP[i] = AuthenticateRadioButton2.Checked;
                    op.BasicAuthName[i] = BasicAuthNameTextBox.Text;
                    op.BasicAuthPassword[i] = BasicAuthPasswordTextBox.Text;
                    op.FetchBeforeSendAuthentication[i] = FetchBeforeSendCheckBox.Checked;
                    op.EmailIncomingServer[i] = POP_ServerTextBox.Text;
                    op.UseSSL_ForIncoming[i] = SecurePOP_CheckBox.Checked;
                    op.PortForIncoming[i] = Convert.ToInt32(PortForPOP_TextBox.Text);
                    break;
                }
                i++;
            }
        }

        // In the _Leave handlers next, if Trim() removes any whitespace, the corresponding _TextChanged handler
        // will get called, and update array, so _Leave doesn't have to.
        private void EmailProfileTextBox_Leave(object sender, EventArgs e)
        {
            EmailProfileTextBox.Text = EmailProfileTextBox.Text.Trim();
        }

        private void BasicAuthNameTextBox_Leave(object sender, EventArgs e)
        {
            BasicAuthNameTextBox.Text = BasicAuthNameTextBox.Text.Trim();
        }

        private void EmailFromTextBox_Leave(object sender, EventArgs e)
        {
            EmailFromTextBox.Text = EmailFromTextBox.Text.Trim();
        }

        private void SMTP_ServerTextBox_Leave(object sender, EventArgs e)
        {
            SMTP_ServerTextBox.Text = SMTP_ServerTextBox.Text.Trim();
        }
        private void EmailProfileCommentsTextBox_Leave(object sender, EventArgs e)
        {
            EmailProfileCommentsTextBox.Text = EmailProfileCommentsTextBox.Text.Trim();
        }

        private void POP_ServerTextBox_Leave(object sender, EventArgs e)
        {
            POP_ServerTextBox.Text = POP_ServerTextBox.Text.Trim();
        }

        private void FetchBeforeSendCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            int m = (seeEditProfileRadio1.Checked) ? 0 : 1;
            fetchBeforeSendAuthentication[m] = FetchBeforeSendCheckBox.Checked;
            EnableFetchBeforeSendFields(FetchBeforeSendCheckBox.Checked);
        }

        private void EnableFetchBeforeSendFields(bool b)
        {
            POP_ServerTextBox.Enabled = b;
            EmailSetupTabPOP3Label.Enabled = b;
            SecurePOP_CheckBox.Enabled = b;
            PortForPOP_TextBox.Enabled = b;
            EmailSetupTabPortForPOPLabel.Enabled = b;
        }

        private void SecurePOP_CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            // If START TLS/SSL check changes, put well-known default value in port box
            int m = (seeEditProfileRadio1.Checked) ? 0 : 1;
            useSSL_ForIncoming[m] = SecurePOP_CheckBox.Checked;
            if (useSSL_ForIncoming[m])
                portForIncoming[m] = "995"; // For POP3
            else
                portForIncoming[m] = "110"; // For POP3
            // User can subsequently overwrite these values
            PortForPOP_TextBox.Text = portForIncoming[m];
        }

        private void PortForSMTP_TextBox_Leave(object sender, EventArgs e)
        {
            VerifyAndCleanupPortValue(PortForSMTP_TextBox);
        }

        private void PortForPOP_TextBox_Leave(object sender, EventArgs e)
        {
            VerifyAndCleanupPortValue(PortForPOP_TextBox);
        }

        private void VerifyAndCleanupPortValue(TextBox t)
        {
            try
            {
                // Roundtrip it through int conversion now, to verify format
                t.Text = Convert.ToInt32(t.Text.Trim()).ToString();
            }
            catch (Exception e)
            {
                ErrBox.Show("Bad Format for Port Number\n" + e.Message);
            }
        }

        private void DeleteSelectedProfileButton_Click(object sender, EventArgs e)
        {

        }

        private void NewProfileBasedOnSelected_Click(object sender, EventArgs e)
        {

        }

        private void NewEmptyProfileButton_Click(object sender, EventArgs e)
        {
            /* NOT YET
            int len = op.EmailProfileName.GetLength();
            op.EmailProfileName.[i] = EmailProfileTextBox.Text;
            op.EmailFrom[i] = EmailFromTextBox.Text;
            op.EmailServer[i] = SMTP_ServerTextBox.Text;
            op.UseSSL_ForSMTP[i] = UseSSL_CheckBox.Checked;
            op.PortForSMTP[i] = Convert.ToInt32(PortForSMTP_TextBox.Text);
            op.UseBasicAuthenticationForSMTP[i] = AuthenticateRadioButton2.Checked;
            op.BasicAuthName[i] = BasicAuthNameTextBox.Text;
            op.BasicAuthPassword[i] = BasicAuthPasswordTextBox.Text;
            op.FetchBeforeSendAuthentication[i] = FetchBeforeSendCheckBox.Checked;
            op.EmailIncomingServer[i] = POP_ServerTextBox.Text;
            op.UseSSL_ForIncoming[i] = SecurePOP_CheckBox.Checked;
            op.PortForIncoming[i] = Convert.ToInt32(PortForPOP_TextBox.Text);
             */
        }
        private void SwapProfilesButton_Click(object sender, EventArgs e)
        {
            // Swap
            int wasPrimary   = EmailProfileNameComboBox1.SelectedIndex;
            int wasSecondary = EmailProfileNameComboBox2.SelectedIndex;
            // Changing the indices here will call other handlers to do rest of work.
            // Do in such an order that state of seeEditProfileRadios will end up same as when started.
            suppressEmailSetupUpdate = true;
            if (seeEditProfileRadio1.Checked)
            {
                EmailProfileNameComboBox2.SelectedIndex = wasPrimary;   // now secondary
                EmailProfileNameComboBox1.SelectedIndex = wasSecondary; // now primary
            }
            else
            {
                EmailProfileNameComboBox1.SelectedIndex = wasSecondary; // now primary
                EmailProfileNameComboBox2.SelectedIndex = wasPrimary;   // now secondary
            }
            suppressEmailSetupUpdate = false;
        }

        #endregion
        #region "Main" Tab, Filmstrip Control
        // ---- "Main" tab, Filmstrip control


        private void FormFilmstripTest_Load(object sender, EventArgs e)
        {
            // Dunno if this function is useful
            SetButtonStates();
            UpdateSelectedInfo();
        }

        private void filmstripControl_OnSelectionChanged(object sender, EventArgs e)
        {
            UpdateSelectedInfo();
        }

        private void photoRoleComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            int n = filmstripControl.SelectedImageID;
            ImageRole role = (ImageRole)Enum.Parse(typeof(ImageRole), photoRoleComboBox.SelectedItem.ToString());
            SetImageRole(n, role);
        }

        private void SetImageRole(int ID, ImageRole role)
        {
            if (ID == FilmstripControl.NO_SELECTION_ID || inBoxDataList.Count == 0)
                return; // sanity check

            InBoxData ibd;
            for (int i = 0; i < inBoxDataList.Count; i++)
            {
                ibd = inBoxDataList[i];
                if (ID == ibd.filmstripID)
                {
                    ibd.imageRole = role;
                    inBoxDataList[i] = ibd;
                    break;
                }
            }
        }

        private bool VerifyImageRoles()
        {
            if (inBoxDataList.Count == 0)
            {
                if (transientReasonForNoPhotoSent() > 0)
                    return true; // TO DO: persistent reasons
                
//                MessageBox.Show("Indicate why no photo, then send again");
                AppBox.Show("Indicate why no photo, then send again");
                
                return false;
            }

            int primaryFound = 0;
            int secondaryFound = 0;
            for (int i = 0; i < inBoxDataList.Count; i++)
            {
                if(inBoxDataList[i].imageRole == ImageRole.Primary)
                    primaryFound++;
                if(inBoxDataList[i].imageRole == ImageRole.Secondary)
                    secondaryFound++;
            }

            if (primaryFound == 0 && secondaryFound == 0)
            {
                if (transientReasonForNoPhotoSent() > 0)
                    return true; // All photos are marked 'Unassigned'

                AppBox.Show(
                    "All photos above are marked 'Unassigned', but you didn't indicate why no photo for this individual.\n" +
                    "Correct this, then send again");
                return false;
            }
            if (primaryFound == 0 && secondaryFound > 1)
            {
                string msg;
                if (secondaryFound == 1)
                    msg = "There is a photo";
                else
                    msg = "There are " + secondaryFound.ToString() + " photos";
                AppBox.Show(
                     msg + " above marked 'Secondary', but no photo marked 'Primary'.\n" +
                    "Correct this, then send again.");
                    // If transientReasonForNoPhotoSent > 0, then the correction involves changing all 'Secondary' to 'Unassigned'.
                    // Probably don't have to be more specific here.
                return false;
            }
            if (primaryFound > 1)
            {
                AppBox.Show(
                    "There are " + primaryFound.ToString() + " photos above marked 'Primary'.\n" +
                    "Revise so that is at most 1, then send again.");
                return false;
            }
            return true;
        }

        private int transientReasonForNoPhotoSent()
        {
            // transient reasons are cleared after every send; persistent reasons are not
            if (NoTimeForPhotoCheckBox.Checked)
                return 1;
            if (ForgotPhotoCheckBox.Checked)
                return 2;
            return 0;
        }
        // TO DO: persistentReasonForNoPhotoSent()

        public bool hasPrimary()
        {
            // We've already verified inBoxDataList before we get here.
            if (inBoxDataList.Count == 0)
                return false;

            for (int i = 0; i < inBoxDataList.Count; i++)
            {
                if (inBoxDataList[i].imageRole == ImageRole.Primary)
                    return true; // Don't need to look further, since we earlier verified that there's only 1
            }
            return false;
        }

        private void UpdateSelectedInfo()
        {
///            sendingLabel.Text = "Selected Photo, Internal List ID: " + filmstripControl.SelectedImageID.ToString(); // FOR DEBUG
            ignoreCaptionExtraTextChangedEvent = true;
            textSelectedDesc.Text = filmstripControl.SelectedImageDescription;
            ignoreCaptionExtraTextChangedEvent = false;
            int n = filmstripControl.SelectedImageID;
            foreach (InBoxData ibd in inBoxDataList)
            {
                if (n == ibd.filmstripID)
                {
                    photoRoleComboBox.SelectedItem = ibd.imageRole.ToString();
                    break;
                }
            }          
            SetButtonStates();
        }

        private void SetButtonStates()
        {
            DeleteBadPhotoButton.Enabled = (filmstripControl.SelectedImageID != FilmstripControl.NO_SELECTION_ID) ? true : false;
            //buttonUpdateDesc.Enabled = (filmstripControl.SelectedImageID != FilmstripControl.NO_SELECTION_ID) ? true : false;
            textSelectedDesc.Enabled = (filmstripControl.SelectedImageID != FilmstripControl.NO_SELECTION_ID) ? true : false; 
        }

        /* first attempt:
        private int AddToFilmstrip(string pixfile)
        {
            Stream fs = new FileStream(pixfile, FileMode.Open, FileAccess.Read);
            int n = filmstripControl.AddImage(new FilmstripImage(System.Drawing.Image.FromStream(fs), ""));
            // Can't do this here, because filmstrip will reload image from file if selection changes: fs.Close();
            // Instead, do when image deleted.

            // Image.FromFile() has known problem [MS Article ID 311754], locks file until app exits:
            // int n = filmstripControl.AddImage(new FilmstripImage(Image.FromFile(pixfile), "")); //pixfile));
            SetButtonStates();
            return n;
        } */

        private void AddToFilmstrip(InBoxData ib)
        {
            ib.fileStream = new FileStream(ib.justTakenFilePath, FileMode.Open, FileAccess.Read);
            ib.filmstripID = filmstripControl.AddImage(new FilmstripImage(System.Drawing.Image.FromStream(ib.fileStream), ""));
            // Can't do this here, because filmstrip will reload image from file if selection changes: ib.fileStream.Close();
            // Instead, do when image deleted.

            // Image.FromFile() has known problem [MS Article ID 311754], locks file until app exits:
            // int n = filmstripControl.AddImage(new FilmstripImage(Image.FromFile(pixfile), "")); //pixfile));
            SetButtonStates();
            return;
        }

        private void filmstripControl_OnSelectedImageDescriptionChanged(object sender, EventArgs e)
        {
            textSelectedDesc.Text = filmstripControl.SelectedImageDescription;
            //MessageBox.Show("Selected image description has been updated.");
            // Don't need to save in inBoxDataList[i].captionExtra here.  We do that only through user keystrokes.
        }

//        private void buttonUpdateDesc_Click(object sender, EventArgs e)
//        {
//            filmstripControl.SelectedImageDescription = textSelectedDesc.Text;
//            SetButtonStates();
//        }

        private void textSelectedDesc_TextChanged(object sender, EventArgs e)
        {
            if (ignoreCaptionExtraTextChangedEvent)
                return; // prevent endless loop when selected image changes
            if (filmstripControl.SelectedImageID == FilmstripControl.NO_SELECTION_ID)
                return;
            filmstripControl.SelectedImageDescription = textSelectedDesc.Text; // We'll get called after every letter typed, but so what?
            InBoxData ibd;
            for (int i = 0; i < inBoxDataList.Count; i++)
            {
                ibd = inBoxDataList[i];
                if (filmstripControl.SelectedImageID == ibd.filmstripID)
                {
                    inBoxDataList[i].captionExtra = textSelectedDesc.Text;
                    break;
                }
            }
            // Probably don't need: SetButtonStates();
        }
        #endregion
        #region Newly generated handlers, to be relocated
        /* === Below (after dummy function) are new handlers to be moved to some category above:*/
        private void dummy() { }

        #endregion

        private void buttonShowWebCam_Click(object sender, EventArgs e)
        {
            FormWebCam.Start();
        }

        private void outboxDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string[] imageFiles;
            imageFiles = Directory.GetFiles(dest2, outboxDataGridView.Rows[e.RowIndex].Cells["ImagePrefix"].Value + "*.jpg", SearchOption.TopDirectoryOnly);

            string name = outboxDataGridView.Rows[e.RowIndex].Cells["First"].Value + " " + outboxDataGridView.Rows[e.RowIndex].Cells["Last"].Value;

            FormSentImages frm = new FormSentImages(imageFiles, name);
            frm.StartPosition = FormStartPosition.CenterParent;
            frm.ShowDialog(this);
        }


    }
    #region Auxillary Classes
    public enum ImageRole
    {
        Unassigned, Primary, Secondary
    }
    public class InBoxData
    {
        public int filmstripID;
        public string justTakenFilePath; // after potential renaming and placing in "just taken" directory
        public string justTakenFileName; // ditto
        public ImageRole imageRole;
        public string captionExtra; // This is a clone of image description, used so we can pull descriptions without having to select each image first
        public string processedFilePath; // after renaming with patient ID and placing in "processed" directory
        public string processedFileName; // ditto
        public Stream fileStream; // hold on to this so we can call fs.Close later
    }

    // Helper class, used with Camera
    public class FileComparer : IComparer<FileInfo>
    {
        public int Compare(FileInfo x, FileInfo y)
        {
            return x.CreationTime.CompareTo(y.CreationTime);
        }
    }
    #endregion
}