//#define LOAD_OLD
using System;
using System.Drawing;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace TriagePicNamespace
{
	/// <summary>
	/// Summary description for Options.
	/// </summary>
	public class Options
	{
        private TriagePic parent;

        private int schemaRevision = 5; // Hard-coded.  Introduced TriagePic v 32.  Bumped to 4 for v 39, 5 for v 40.  How exactly should we test this?

        public int SchemaRevision
        {
            get { return schemaRevision; }
            set { schemaRevision = value; }
        }

        // CheckList/Staffing tab:
        private string eventNameText;

        public string EventNameText
        {
            get { return eventNameText; }
            set { eventNameText = value; }
        }
        private string eventTypeText;

        public string EventTypeText
        {
            get { return eventTypeText; }
            set { eventTypeText = value; }
        }
        private int eventRangeInt;

        public int EventRangeInt
        {
            get { return eventRangeInt; }
            set { eventRangeInt = value; }
        }

        private string stationPatientTrackingOfficerText;

        public string StationPatientTrackingOfficerText
        {
            get { return stationPatientTrackingOfficerText; }
            set { stationPatientTrackingOfficerText = value; }
        }
        private string stationTriagePhysicansOrRNsText;

        public string StationTriagePhysicansOrRNsText
        {
            get { return stationTriagePhysicansOrRNsText; }
            set { stationTriagePhysicansOrRNsText = value; }
        }
        private string stationOtherStaffText;

        public string StationOtherStaffText
        {
            get { return stationOtherStaffText; }
            set { stationOtherStaffText = value; }
        }
        private string stationPhotographersText;

        public string StationPhotographersText
        {
            get { return stationPhotographersText; }
            set { stationPhotographersText = value; }
        }

        // Main tab:
        private string patientNumberText;

        public string PatientNumberText
        {
            get { return patientNumberText; }
            set { patientNumberText = value; }
        }

        private string practicePatientNumberText; // Seen only in popup dialog, not tabbed pages

        public string PracticePatientNumberText
        {
            get { return practicePatientNumberText; }
            set { practicePatientNumberText = value; }
        }

        // Hospital tab:
        private string orgNameText;

        public string OrgNameText
        {
            get { return orgNameText; }
            set { orgNameText = value; }
        }
        private string orgAbbrOrShortNameText;

        public string OrgAbbrOrShortNameText
        {
            get { return orgAbbrOrShortNameText; }
            set { orgAbbrOrShortNameText = value; }
        }
        private string orgStreetAddress1Text;

        public string OrgStreetAddress1Text
        {
            get { return orgStreetAddress1Text; }
            set { orgStreetAddress1Text = value; }
        }
        private string orgStreetAddress2Text;

        public string OrgStreetAddress2Text
        {
            get { return orgStreetAddress2Text; }
            set { orgStreetAddress2Text = value; }
        }
        private string orgTownOrCityText;

        public string OrgTownOrCityText
        {
            get { return orgTownOrCityText; }
            set { orgTownOrCityText = value; }
        }
        private string orgCountyText;

        public string OrgCountyText
        {
            get { return orgCountyText; }
            set { orgCountyText = value; }
        }
        private string org2LetterStateText;

        public string Org2LetterStateText
        {
            get { return org2LetterStateText; }
            set { org2LetterStateText = value; }
        }
        private string orgZipcodeText;

        public string OrgZipcodeText
        {
            get { return orgZipcodeText; }
            set { orgZipcodeText = value; }
        }
        private string orgPhoneText;

        public string OrgPhoneText
        {
            get { return orgPhoneText; }
            set { orgPhoneText = value; }
        }
        private string orgFaxText;

        public string OrgFaxText
        {
            get { return orgFaxText; }
            set { orgFaxText = value; }
        }
        private string orgEmailText;

        public string OrgEmailText
        {
            get { return orgEmailText; }
            set { orgEmailText = value; }
        }
        private string orgWebSiteText;

        public string OrgWebSiteText
        {
            get { return orgWebSiteText; }
            set { orgWebSiteText = value; }
        }
        private string orgNPI_Text;

        public string OrgNPI_Text
        {
            get { return orgNPI_Text; }
            set { orgNPI_Text = value; }
        }
        private string orgPatientID_PrefixText;

        public string OrgPatientID_PrefixText
        {
            get { return orgPatientID_PrefixText; }
            set { orgPatientID_PrefixText = value; }
        }
        // Email Setting tab:
#if LOAD_OLD
         
        public string EmailFrom
        {
            get { return emailFrom[0]; }
            set { emailFrom[0] = value; }
        }

        public string EmailServer
        {
            get { return emailServer[0]; }
            set { emailServer[0] = value; }
        }

        public bool UseSSL_ForSMTP
        {
            get { return useSSL_ForSMTP[0]; }
            set { useSSL_ForSMTP[0] = value; }
        }

        public int PortForSMTP
        {
            get { return portForSMTP[0]; }
            set { portForSMTP[0] = value; }
        }

        public bool UseBasicAuthenticationForSMTP
        {
            get { return useBasicAuthenticationForSMTP[0]; }
            set { useBasicAuthenticationForSMTP[0] = value; }
        }

        public string BasicAuthName
        {
            get { return basicAuthName[0]; }
            set { basicAuthName[0] = value; }
        }

        public string BasicAuthPassword
        {
            get { return basicAuthPassword[0]; }
            set { basicAuthPassword[0] = value; }
        }
#endif
        private string[] emailProfileSelected = { "", "" };

        public string[] EmailProfileSelected
        {
            get { return emailProfileSelected; }
            set { emailProfileSelected = value; }
        }

#if SOON
// Use ArrayList instead of default array to allow adding/deleting elements in app
// One was is given by this example:
//    public class Group{
//    [XmlElement(Type = typeof(Employee)), XmlElement(Type = typeof(Manager))]
//    public ArrayList Info;
//}
// e.g.:
//    [XmlElement(Type = typeof(string))]
//    public ArrayList EmailProfileName;
// But this creates a flat array of a sequence of named elements, instead of <EmailProfileName>...contents....</EmailProfileName>
// Instead, use the attributes also used for arrays:
// [XmlArray("NameOfArrayElement")] and 1 or more [XmlArrayItem("NameOfType", typeof(YourTypes)]
// e.g.:
     private ArrayList emailProfileName;

    public ArrayList EmailProfileName
    {
        get { return emailProfileName; }
        set { emailProfileName = value; }
    }

#endif
        private string[] emailProfileName;

        public string[] EmailProfileName
        {
            get { return emailProfileName; }
            set { emailProfileName = value; }
        }
        private string [] emailFrom; // = { "", "" };
         
        public string [] EmailFrom
        {
            get { return emailFrom; }
            set { emailFrom = value; }
        }
        private string[] emailServer; // = { "", "" };

        public string [] EmailServer
        {
            get { return emailServer; }
            set { emailServer = value; }
        }
        private string[] emailProfileComments;

        public string[] EmailProfileComments
        {
            get { return emailProfileComments; }
            set { emailProfileComments = value; }
        }

        private bool[] useSSL_ForSMTP; // = { false, false };

        public bool [] UseSSL_ForSMTP
        {
            get { return useSSL_ForSMTP; }
            set { useSSL_ForSMTP = value; }
        }
        private int[] portForSMTP; // = { 0, 0 };

        public int [] PortForSMTP
        {
            get { return portForSMTP; }
            set { portForSMTP = value; }
        }
        private bool[] useBasicAuthenticationForSMTP; // = { false, false };

        public bool [] UseBasicAuthenticationForSMTP
        {
            get { return useBasicAuthenticationForSMTP; }
            set { useBasicAuthenticationForSMTP = value; }
        }
        private string[] basicAuthName; // = { "", "" };

        public string [] BasicAuthName
        {
            get { return basicAuthName; }
            set { basicAuthName = value; }
        }
        private string[] basicAuthPassword; // = { "", "" };

        public string [] BasicAuthPassword
        {
            get { return basicAuthPassword; }
            set { basicAuthPassword = value; }
        }

        private bool[] fetchBeforeSendAuthentication; // = { false, false };

        public bool[] FetchBeforeSendAuthentication
        {
            get { return fetchBeforeSendAuthentication; }
            set { fetchBeforeSendAuthentication = value; }
        }
        private string[] emailIncomingServer; // = { "", "" };

        public string[] EmailIncomingServer
        {
            get { return emailIncomingServer; }
            set { emailIncomingServer = value; }
        }

        private bool[] useSSL_ForIncoming; // = { false, false };

        public bool[] UseSSL_ForIncoming
        {
            get { return useSSL_ForIncoming; }
            set { useSSL_ForIncoming = value; }
        }
        private int[] portForIncoming; // = { 0, 0 };

        public int[] PortForIncoming
        {
            get { return portForIncoming; }
            set { portForIncoming = value; }
        }
        // Email Distribution tab:
        private string emailTo;

        public string EmailTo
        {
            get { return emailTo; }
            set { emailTo = value; }
        }
        private string emailCc;

        public string EmailCc
        {
            get { return emailCc; }
            set { emailCc = value; }
        }
        private string emailBcc;

        public string EmailBcc
        {
            get { return emailBcc; }
            set { emailBcc = value; }
        }
        private string anonymizedEmailTo;

        public string AnonymizedEmailTo
        {
            get { return anonymizedEmailTo; }
            set { anonymizedEmailTo = value; }
        }
        private string anonymizedEmailCc;

        public string AnonymizedEmailCc
        {
            get { return anonymizedEmailCc; }
            set { anonymizedEmailCc = value; }
        }
        private string anonymizedEmailBcc;

        public string AnonymizedEmailBcc
        {
            get { return anonymizedEmailBcc; }
            set { anonymizedEmailBcc = value; }
        }


//		public static string options_path = Environment.GetFolderPath(Environment.SpecialFolder.Personal)+"\\TriagePic.xml";
		public static string options_path = "TriagePicSharedSettings.xml";
		public Options()
		{
            // Could add initialization here:  this.PatientNumberText = "...";
		}

        public Options(TriagePic p)
        {
            parent = p;
        }

		public void CreateOptionsFile()
		{
			XmlSerializer ser = new XmlSerializer( typeof(Options));
			
			Options o = new Options();
            // Could add initialization here:  o.PatientNumberText = "...";
			StreamWriter wr = new StreamWriter( options_path );

			ser.Serialize(wr,(object)o);

			wr.Flush();
			wr.Close();
		}

		public Options ReadOptionsFromFile()
		{
            parent.newOptionsFileCreated = false; // Assume file exists until we know better
			if ( !File.Exists( options_path ))
			{
				//File.Create( options_path ); // If this happened, caller will see null default values, e.g., port for STMP will be 0
                parent.newOptionsFileCreated = true;
                WriteOptionsToFile(); // Write out class elements as initialized above.
			}
            XmlSerializer ser = new XmlSerializer(typeof(Options));
			return (Options)ser.Deserialize(new XmlTextReader(options_path));
            // Alternative 2 previous lines:
            //Options o = new Options();
            //{   // Try to prevent conflict over xml file access
            //    XmlSerializer ser = new XmlSerializer(typeof(Options));
            //    XmlTextReader xtr = new XmlTextReader(options_path);
            //    o = (Options)ser.Deserialize(xtr);
            //    xtr.Close();
            //}
            //return o;
        }

        public void WriteOptionsToFile()
        {
            XmlSerializer ser = new XmlSerializer(typeof(Options));
            StreamWriter wr = new StreamWriter(Options.options_path);
            ser.Serialize(wr, this);
            wr.Flush();
            wr.Close();
        }

#if ANOTHERVERSION
        public Options ReadOptionsFromFile2() // Glenn adds
        {
            if (!File.Exists(options_path))
            {
                File.Create(options_path);

                while (!File.Exists(options_path))
                {
                    // spin.  Have to wait before CreateOptionsFile call
                }
                CreateOptionsFile();
            }

            Options o = new Options();
            {   // Try to prevent conflict over xml file access
                XmlSerializer ser = new XmlSerializer(typeof(Options));
                StreamReader sr = new StreamReader(options_path);
                o = (Options)ser.Deserialize(sr); // Does StreamReader instead of XmlTextReader gives different form to color strings? nah
                sr.Close();
            }
            o.ForeColor = o.ForeColor.Substring(o.ForeColor.IndexOf(']') + 1);
            o.BgColor = o.BgColor.Substring(o.BgColor.IndexOf(']') + 1);
            return o;
        }
#endif

        // Could add:

		//public string Text
		//{
		//	get { return this.text; }
		//	set { this.text = value; }
		//}
	}
}
