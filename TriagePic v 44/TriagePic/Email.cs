#define PFIF_TEST
#define SPRINT
//#define GMAIL
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D; // for clock
using System.Drawing.Imaging; // for clock
using System.Globalization; // for CultureInfo with DateTimeOffset
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Net.Mail;  // see www.aspnettutorials.com/tutorials/email/email-attach-aspnet2-csharp.aspx
using Microsoft.Win32; // for registry
using System.Xml; // For outbox
// using System.Security.Principal;
using System.Diagnostics;
using CodeProject.Dialog; // for AppBox
using LumiSoft.Net;
//using LumiSoft.Net.Log;
using LumiSoft.Net.POP3.Client;


namespace TriagePicNamespace
{
    public partial class MyEmail : Form
    {
        private TriagePic parent;
        // Stored in XML file:
//        private string emailServer;
//        private string emailFrom;
//        private string emailTo;
//        private string emailCc;
//        private string emailBcc;
//        private string anonymizedEmailTo;
//        private string anonymizedEmailCc;
//        private string anonymizedEmailBcc;

//        private DataSet outboxDataSet = null;

        public MyEmail()
        {
        }

        public MyEmail(TriagePic p)
        {
//            InitializeComponent();
            parent = p;
            parent.sendingLabel.Text = ""; // was commented out when using DEBUGPIC
        }

        public void ReadEmailSettings(Options op)
        {
            // Email Setup tab:
            try
            {
                parent.emailProfileSelected[0] = op.EmailProfileSelected[0];
                parent.emailProfileSelected[1] = op.EmailProfileSelected[1];
                if (parent.emailProfileSelected[0] == null || parent.emailProfileSelected[0].Length == 0)
                {
                    AppBox.Show(
                        "Please provide a value for the first 'EmailProfileSelected' field in the TriagePicSharedSettings.xml file");
                }
                if (parent.emailProfileSelected[0] == parent.emailProfileSelected[1])
                {
                    AppBox.Show(
                        "Please provide different first and second values for the 'EmailProfileSelected' field in the TriagePicSharedSettings.xml file");
                }
                // We'll leave items for most profiles snoozing in op unless/until we need them
                int k = 0;
                bool found1 = false;
                bool found2 = false;
                foreach (string s in op.EmailProfileName)
                {
                    parent.EmailProfileNameComboBox1.Items.Add(s);
                    parent.EmailProfileNameComboBox2.Items.Add(s);
                    if(s == op.EmailProfileSelected[0])
                    {
                        found1 = true;
                        parent.EmailProfileNameComboBox1.SelectedIndex = k;
                        SetPrimaryAndSecondaryEmailProfileItems(op, 0, k);
                    }
                    if(s == op.EmailProfileSelected[1])
                    {
                        found2 = true;
                        parent.EmailProfileNameComboBox2.SelectedIndex = k;
                        SetPrimaryAndSecondaryEmailProfileItems(op, 1, k);
                    }
                    k++;
                }
                if (!found1)
                {
                    ErrBox.Show(
                        "The requested email profile with name " + op.EmailProfileSelected[0] + " (given in 'EmailProfileSelected') was not found among choices in the 'EmailProfileName' field in the TriagePicSharedSettings.xml file");
                }
                if (!found2)
                {
                    ErrBox.Show(
                        "The requested email profile with name " + op.EmailProfileSelected[1] + " (given in 'EmailProfileSelected') was not found among choices in the 'EmailProfileName' field in the TriagePicSharedSettings.xml file");
                }


#if NOTQUITE
                bool found;
                for (int i = 0; i < 2; i++)
                {
                    found = false;
                    int k = 0;
                    foreach (string s in op.EmailProfileName)
                    {
                        if (s == op.EmailProfileSelected[i])
                        {
                            found = true;
                            if(i == 0)
                                parent.EmailProfileNameComboBox1.SelectedItem = s; // PROBLEM HERE, with setting text instead of index, causes problems elsewhere
                            else
                                parent.EmailProfileNameComboBox2.SelectedItem = s; 
                            SetPrimaryAndSecondaryEmailProfileItems(op, i, k);
                            break;
                        }
                        k++;
                    }
                    if (!found)
                    {
                        ErrBox.Show(
                            "The requested email profile with name " + op.EmailProfileName[i] + " (given in 'EmailProfileSelected') was not found among choices in the 'EmailName' field in the TriagePicSharedSettings.xml file");
                    }
                }
#endif
                parent.SeePrimaryEmailProfile(true);

                // Email Distribution tab:
                parent.FullRecordToListTextBox.Text = parent.emailTo = op.EmailTo;
                parent.FullRecordCcListTextBox.Text = parent.emailCc = op.EmailCc;
                parent.FullRecordBccListTextBox.Text = parent.emailBcc = op.EmailBcc;
                parent.AnonymizedToListTextBox.Text = parent.anonymizedEmailTo = op.AnonymizedEmailTo;
                parent.AnonymizedCcListTextBox.Text = parent.anonymizedEmailCc = op.AnonymizedEmailCc;
                parent.AnonymizedBccListTextBox.Text = parent.anonymizedEmailBcc = op.AnonymizedEmailBcc;
            }
            catch (Exception ex)
            {
                ErrBox.Show("Error in reading email settings within TriagePicSharedSettings.xml\n" + ex.ToString());
                return;
            }
            if (parent.emailServer[0] == null || parent.emailServer[0].Length == 0)
            {
                AppBox.Show(
                    "Please provide a value of the 'EmailServer' field in the TriagePicSharedSettings.xml file");
            }
            if (parent.emailFrom[0] == null || parent.emailFrom[0].Length == 0)
            {
                AppBox.Show(
                    "Please provide a value of the 'EmailFrom' field in the TriagePicSharedSettings.xml file");
            }
            // We are not necessarily requiring a 2nd SMTP Server
            if ((parent.emailTo == null || parent.emailTo.Length == 0) && (parent.anonymizedEmailTo == null || parent.anonymizedEmailTo.Length == 0))
            {
                AppBox.Show(
                    "Please provide a value of either the 'EmailTo' or 'AnonymizedEmailTo' fields, or both, in the TriagePicSharedSettings.xml file");
            }
            if (parent.useBasicAuthenticationForSMTP[0] && (parent.basicAuthName[0] == null || parent.basicAuthName[0].Length == 0))
            {
                AppBox.Show(
                    "Please provide a value of the 'EmailSMTPBasicAuthenicationName' field in the TriagePicSharedSettings.xml file");
            }
            if (parent.useBasicAuthenticationForSMTP[0] && (parent.basicAuthPassword[0] == null || parent.basicAuthPassword[0].Length == 0))
            {
                AppBox.Show(
                    "Please provide a value of the 'EmailSMTPBasicAuthenicationPassword' field in the TriagePicSharedSettings.xml file");
            }

        }

        public void SetPrimaryAndSecondaryEmailProfileItems(Options op, int primaryOrSecondary, int profileNum)
        {
            parent.emailProfileSelected[primaryOrSecondary] = op.EmailProfileName[profileNum];
            parent.emailFrom[primaryOrSecondary] = op.EmailFrom[profileNum];
            parent.emailServer[primaryOrSecondary] = op.EmailServer[profileNum];
            parent.emailProfileComments[primaryOrSecondary] = op.EmailProfileComments[profileNum];
            parent.useSSL_ForSMTP[primaryOrSecondary] = op.UseSSL_ForSMTP[profileNum];
            parent.portForSMTP[primaryOrSecondary] = op.PortForSMTP[profileNum].ToString();
            parent.useBasicAuthenticationForSMTP[primaryOrSecondary] = op.UseBasicAuthenticationForSMTP[profileNum];
            //if (op.UseBasicAuthenticationForSMTP[0])
            //    parent.Authenticate1RadioButton2_Click(null, null); // Sets parent.UseBasicAuthenticationForSMTP
            //else
            //    parent.Authenticate1RadioButton1_Click(null, null); // By default, prefer login credentials (network authentication)
            parent.basicAuthName[primaryOrSecondary] = op.BasicAuthName[profileNum];
            parent.basicAuthPassword[primaryOrSecondary] = op.BasicAuthPassword[profileNum];
            parent.fetchBeforeSendAuthentication[primaryOrSecondary] = op.FetchBeforeSendAuthentication[profileNum];
            parent.emailIncomingServer[primaryOrSecondary] = op.EmailIncomingServer[profileNum];
            parent.useSSL_ForIncoming[primaryOrSecondary] = op.UseSSL_ForIncoming[profileNum];
            parent.portForIncoming[primaryOrSecondary] = op.PortForIncoming[profileNum].ToString();
        }

        public void ReadEmailHistory()
        {
            // Read outbox.xml:
            parent.outboxDataSet = new DataSet();
            // no good outboxDataSet.SchemaSerializationMode = SchemaSerializationMode.ExcludeSchema;
            parent.outboxDataSet.ReadXml(Environment.CurrentDirectory + @"\outbox queue\outbox.xml");
            // later maybe outboxDataSet.WriteXmlSchema(Environment.CurrentDirectory + @"\Test.xsd");
            // or DataSet.Tables[0].WriteXml("test.xml", XmlWriteMode.WriteSchema);
            // or more likely with XmlWriteMode.IgnoreSchema
            //BindingSource outboxBindingSource = new BindingSource();
            // Don't need to bind if column headings are same as XML attributes, set at design time
            //outboxBindingSource.DataSource = outboxDataSet;
            //outboxDataGridView.DataSource = outboxBindingSource;


            // "WhenLocalTime" comes in as a string, but we want a DateTimeOffset object.
            // Attempts to do the conversion at the DataGridView were unsuccessful.  So let's carve on the DataTable instead:
            DataColumn whenDateTimeOffset = new DataColumn("When", typeof(DateTimeOffset));
            parent.outboxDataSet.Tables[0].Columns.Add(whenDateTimeOffset);
            parent.outboxDataSet.Tables[0].Columns["When"].SetOrdinal(0); // SetOrdinal makes it first col in table.  Alternative to later using DataGridView.DisplayIndex
            string s;
            CultureInfo provider = CultureInfo.InvariantCulture;
            for (int i = 0; i < parent.outboxDataSet.Tables[0].Rows.Count; i++)
            {
                s = parent.outboxDataSet.Tables[0].Rows[i]["WhenLocalTime"].ToString();
                parent.outboxDataSet.Tables[0].Rows[i]["When"] = DateTimeOffset.ParseExact(s, "yyyy-MM-dd HH:mm:ss K", provider);
                //"o" format would be "yyyy-MM-ddTHH:mm:ss.fffffffK", but we don't want fractional seconds;
            }
            // More trouble than it's worth: parent.outboxDataSet.Tables[0].Columns["When"].ReadOnly = true; // can do this only AFTER preceding loop.
            // Instead, make datagridview read-only when we need it to be.

            parent.outboxDataGridView.DataSource = parent.outboxDataSet.Tables[0];
            //outboxDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            parent.outboxDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            // Must be in DataBinding Complete handler: outboxDataGridView.Column[...].Width = ...
            parent.outboxDataGridView.RowHeadersWidth = 10;
            parent.outboxDataGridView.Columns["When"].MinimumWidth = 110;
            parent.outboxDataGridView.Columns["When"].DefaultCellStyle.Format = "MMM dd hh:mm tt"; // new v .42
            parent.outboxDataGridView.Columns["TZ"].MinimumWidth = 40; // but hidden.  Has timezone abbreviation (3 letter in US)
            parent.outboxDataGridView.Columns["PatientID"].MinimumWidth = 65;
            parent.outboxDataGridView.Columns["Sex"].MinimumWidth = 40; // called "Sex" instead of "Gender" cuz it's shorter
            parent.outboxDataGridView.Columns["First"].MinimumWidth = 70;
            parent.outboxDataGridView.Columns["Last"].MinimumWidth = 70;
            parent.outboxDataGridView.Columns["ImagePrefix"].Width = 0;
        }


        public void AppendEmailHistory(string patientNumberSent, string zone, string hasPicAbbr)
        {
            //outboxDataSet.Tables[0].NewRow();
            string genderAbbr;
            if (parent.GenderMaleCheckBox.Checked && !parent.GenderFemaleCheckBox.Checked)
                genderAbbr = "M";
            else if (!parent.GenderMaleCheckBox.Checked && parent.GenderFemaleCheckBox.Checked)
                genderAbbr = "F";
            else if (!parent.GenderMaleCheckBox.Checked && !parent.GenderFemaleCheckBox.Checked)
                genderAbbr = "";
            else
                genderAbbr = "M+F";

            string pedsAbbr;
            if (parent.PedsCheckBox.Checked)
                pedsAbbr = "Y";
            else
                pedsAbbr = "N";

            DataRow newRow = parent.outboxDataSet.Tables[0].NewRow();
            parent.outboxDataSet.Tables[0].Rows.Add(newRow);
            parent.outboxDataGridView.EndEdit(); // commit new row
            int i = parent.outboxDataSet.Tables[0].Rows.Count - 1;
                        // .Rows[0].Cells[2].Value = DateTime.Now;
            // .Rows[0].Cells[2].Style.Format = "d";
            // We might sometimes be off by a second with respect to DateTimeLabel.Text, but should we care?
            // For 24 time, use: HH:mm (without tt)
            foreach (DataGridViewBand band in parent.outboxDataGridView.Columns)
                band.ReadOnly = true; // Momentarily allow alteration to new row.
            CultureInfo provider = CultureInfo.InvariantCulture;
            int k = 0;
            parent.outboxDataGridView.Rows[i].Cells[k++].Value = DateTimeOffset.Now; // "When" [displayed format set elsewhere]
            parent.outboxDataGridView.Rows[i].Cells[k++].Value = DateTimeOffset.Now.ToString("yyyy-MM-dd HH:mm:ss K", provider);  // "WhenLocalTime" (hidden).
            // WhenLocalTime format here must be same as used in TriagePic.SumPatientArrivals.  It's what appears in outbox queue XML file.
            parent.outboxDataGridView.Rows[i].Cells[k++].Value = parent.GetTimeZoneAbbreviation(); // "TZ" (hidden)
            parent.outboxDataGridView.Rows[i].Cells[k++].Value = "Y"; // "Sent"
            // PatientNumber was bumped above during call to MySendMail, so we subtract 1
            parent.outboxDataGridView.Rows[i].Cells[k++].Value = /*patientID_Prefix + */patientNumberSent; // "PatientID"
            parent.outboxDataGridView.Rows[i].Cells[k++].Value = zone; // "Zone"
            parent.outboxDataGridView.Rows[i].Cells[k++].Value = genderAbbr; // "Sex"
            parent.outboxDataGridView.Rows[i].Cells[k++].Value = pedsAbbr; // "Peds"
            // Nickname?
            parent.outboxDataGridView.Rows[i].Cells[k++].Value = parent.FirstNameTextBox.Text + " " + parent.MiddleNameTextBox.Text; // "First"
            parent.outboxDataGridView.Rows[i].Cells[k++].Value = parent.LastNameTextBox.Text + " " + parent.NameSuffixTextBox.Text; // "Last"
            parent.outboxDataGridView.Rows[i].Cells[k++].Value = hasPicAbbr; // "Pic"
            parent.outboxDataGridView.Rows[i].Cells[k++].Value = parent.FormEventNameLabel.Text; // "Event" including specific suffix
            parent.outboxDataGridView.Rows[i].Cells[k++].Value = parent.patientID_PrefixAndNumber;
            parent.outboxDataGridView.EndEdit();
            foreach (DataGridViewBand band in parent.outboxDataGridView.Columns)
                band.ReadOnly = true;
            // Output update, but don't want "When" column in result, just "WhenLocalTime"
            DataSet temp = parent.outboxDataSet.Copy();
            temp.Tables[0].Columns.Remove("When");
            temp.WriteXml(Environment.CurrentDirectory + @"\outbox queue\outbox.xml"); //
        }


        public string FormatEmailBodyPlainText(string zone, bool anonymize)
        {
            char cLF = (char)0x0A;
            string LF = cLF.ToString();
            // string LF = "\r\n"; // previous was being translated to "\n", which wasn't reliable as line terminator with SmtpClient

            string gender;
            if (parent.GenderMaleCheckBox.Checked && !parent.GenderFemaleCheckBox.Checked)
                gender = "Male";
            else if (!parent.GenderMaleCheckBox.Checked && parent.GenderFemaleCheckBox.Checked)
                gender = "Female";
            else if (!parent.GenderMaleCheckBox.Checked && !parent.GenderFemaleCheckBox.Checked)
                gender = "Unspecified";
            else
                gender = "Complex";

            string peds;
            if (parent.PedsCheckBox.Checked)
                peds = "Yes";
            else
                peds = "No";

            string reasonNoPhoto = "";
            if (!anonymize)
            {
                // Probably should use radios here... just take the first one checked:
                if (parent.NoTimeForPhotoCheckBox.Checked)
                    reasonNoPhoto = "No time to take; fast arrival rate";
                else if (parent.ForgotPhotoCheckBox.Checked)
                    reasonNoPhoto = "Forgot to take; patient gone";
            }
            string body =
            "DON'T REPLY TO THIS ADDRESS." + LF;
            if (anonymize)
                body += "PUBLIC DISTRIBUTION - ANONYMIZED." + LF;
            else
                body += "DISTRIBUTION RESTRICTED." + LF;
            body +=
            "SEE ATTACHED DOCUMENT." + LF +
            "Suburban Hospital" + LF +
            "Bethesda, MD" + LF +
            "Disaster Triage Station" + LF +
            "Computer: " + parent.MachineNameFieldLabel.Text + LF +
            "Login: " + parent.UserNameFieldLabel.Text + LF;
            if (parent.PatientTrackingOfficerTextBox.Text.Length != 0)
                body += "Patient Tracking Officer: " + parent.PatientTrackingOfficerTextBox.Text + LF;
            if (parent.TriagePhysicansOrRNsTextBox.Text.Length != 0)
                body += "Triage Physicians or RNs: " + parent.TriagePhysicansOrRNsTextBox.Text + LF;
            if (parent.OtherStationStaff.Text.Length != 0)
                body += "Other Station Staff: " + parent.OtherStationStaff.Text + LF;
            if (!anonymize && parent.PhotographersTextBox.Text.Length != 0)
                body += "Photographers: " + parent.PhotographersTextBox.Text + LF;
            body +=
                "Event: " + parent.FormEventNameLabel.Text + LF +
                // was: DateTime.Now.ToString("f") + LF + // For en-US culture, this outputs, for example: Tuesday, April 10, 2001 3:51 PM  
                // Includes timezone abbreviation:
                parent.DateTimeLabel.Text + LF; // Because of Outlook's 40 char limit, LF following this disappears if concatentated.  Restart string concat...
            if (!anonymize)
                body += "Patient ID: " + parent.patientID_PrefixAndNumber + LF;
            body +=
                "Gender: " + gender + LF +
                "Peds: " + peds + LF;
            if (!anonymize)
            {
                if (parent.FirstNameTextBox.Text.Length != 0)
                    body += "First Name: " + parent.FirstNameTextBox.Text + LF;
                if (parent.MiddleNameTextBox.Text.Length != 0)
                    body += "Middle Name: " + parent.MiddleNameTextBox.Text + LF;
                if (parent.LastNameTextBox.Text.Length != 0)
                    body += "Last Name: " + parent.LastNameTextBox.Text + LF;
                if (parent.NameSuffixTextBox.Text.Length != 0)
                    body += "Suffix: " + parent.NameSuffixTextBox.Text + LF;
                if (parent.NickNameTextBox.Text.Length != 0)
                    body += "Nickname/Alias: " + parent.NickNameTextBox.Text + LF;
            }
            body +=
                "To Zone: " + zone + LF;
            if (!anonymize)
            {
                body += "Picture: ";
                if (parent.hasPrimary())
                {
                    body += "Yes" + LF;
                }
                else
                {
                    // Otherwise, we have no picture to send
                    body += "No" + LF +
                    "Why No Photo: " + reasonNoPhoto + LF;
                }
            }
            body = EmitExtraLineFeedIfLongLine(body);
            return body;
        }
#if MAYBENOT
        public static String RemoveDiacritics(String s)
        {
            // From: http://weblogs.asp.net/fmarguerie/archive/2006/10/30/removing-diacritics-accents-from-strings.aspx
            // http://blogs.msdn.com/michkap/archive/2005/02/19/376617.aspx
            String normalizedString = s.Normalize(NormalizationForm.FormD);
            StringBuilder stringBuilder = new StringBuilder();

            for (int i = 0; i < normalizedString.Length; i++)
            {
                Char c = normalizedString[i];
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                    stringBuilder.Append(c);
            }

            // return stringBuilder.ToString();
            // More from: http://blogs.msdn.com/michkap/archive/2007/05/14/2629747.aspx
            // Combining above method with converting Unicode to Western codepage, strips even more diarcritics... Like Polish L-slash
            // However it is still not 100%

            //string unicodeString = RemoveDiacritics(unicodeStringOrig);
            string unicodeString = stringBuilder;
            Encoding nonunicode = Encoding.GetEncoding(850);
            Encoding unicode = Encoding.Unicode;
            byte[] unicodeBytes = unicode.GetBytes(unicodeString);
            byte[] nonunicodeBytes = Encoding.Convert(unicode, nonunicode, unicodeBytes);
            char[] nonunicodeChars = new char[nonunicode.GetCharCount(nonunicodeBytes, 0, nonunicodeBytes.Length)];
            nonunicode.GetChars(nonunicodeBytes, 0, nonunicodeBytes.Length, nonunicodeChars, 0);
            string nonunicodeString = new string(nonunicodeChars);
            return nonunicodeString;
        }
#endif
 
// Adapted from http://msdn.microsoft.com/en-us/library/system.text.encoding.convert(VS.71).aspx
// using System; using System.Text;
// Whether this is really adequate, or more complex version above needs to be made working, perhaps time will tell.

        public static String ConvertToASCII(String s)
        {
            string unicodeString = s;

             // Create two different encodings.
             Encoding ascii = Encoding.ASCII;
             Encoding unicode = Encoding.Unicode;

             // Convert the string into a byte[].
             byte[] unicodeBytes = unicode.GetBytes(unicodeString);

             // Perform the conversion from one encoding to the other.
             byte[] asciiBytes = Encoding.Convert(unicode, ascii, unicodeBytes);
                
             // Convert the new byte[] into a char[] and then into a string.
             // Illustrates the use of GetCharCount/GetChars.
             char[] asciiChars = new char[ascii.GetCharCount(asciiBytes, 0, asciiBytes.Length)];
             ascii.GetChars(asciiBytes, 0, asciiBytes.Length, asciiChars, 0);
             string asciiString = new string(asciiChars);

             // Display the strings created before and after the conversion.
             // Console.WriteLine("Original string: {0}", unicodeString);
             // Console.WriteLine("Ascii converted string: {0}", asciiString);
            return asciiString;
        }


        public string FormatEmailBodyAsPFIF(string zone)
        {
            string body = FormatPFIF(zone);
            return EmitExtraLineFeedIfLongLine(body);
        }

        public string FormatEmailAttachmentAsPFIF(string zone)
        {
            // returns short name of file
            string body = FormatPFIF(zone);
            if (body.Length == 0) // Probably because of no patient name given
                return null;
            char cLF = (char)0x0A;
            string LF = cLF.ToString();
            body = body.Replace(LF, "\r\n"); // In attachments, CRLF is better than LF methinks
            string shortName = parent.patientID_PrefixAndNumber + " " + zone + ".pfif";
            string fileLoc = parent.dest1 + shortName;
            // Don't need this: body = EmitExtraLineFeedIfLongLine(body);
            // Create a Text File
            if (File.Exists(fileLoc))
            {
                File.Delete(fileLoc);
                AppBox.Show("Over-writing old PFIF-format file with same name.");
            }
            //if (!File.Exists(fileLoc))
            //{
             using (StreamWriter sw1 = new StreamWriter(fileLoc))
            {
                sw1.Write(body);                  
                sw1.Close();
            }
            //}
            return shortName;
        }

        private string FormatPFIF(string zone)
        {
            // Check mandatory fields:
            if (parent.PatientNumberTextBox.Text.Length == 0)
                return ""; // Check mandatory fields
            // There may be people with only 1 name (so we are relaxing PFIF spec here by not ORing first & last names)
            if (parent.FirstNameTextBox.Text.Length == 0 && parent.LastNameTextBox.Text.Length == 0 && parent.NickNameTextBox.Text.Length == 0)
                return "";
            // PatientNumberTextBox.Text is used to generate unique part of mandatory person_record_id & note_record_id
            // Other mandatory items in note: author_name, source_date, text.  But we can guarantee there contents here.
            
            char cLF = (char)0x0A;
            string LF = cLF.ToString();
            // string LF = "\r\n"; // previous was being translated to "\n", which wasn't reliable as line terminator with SmtpClient
            string txtPhotoAttachmentShortName = parent.patientID_PrefixAndNumber + " " + zone + ".jpg"; // we'll check later if photo actually exists

            string gender;
            if (parent.GenderMaleCheckBox.Checked && !parent.GenderFemaleCheckBox.Checked)
                gender = "Male";
            else if (!parent.GenderMaleCheckBox.Checked && parent.GenderFemaleCheckBox.Checked)
                gender = "Female";
            else if (!parent.GenderMaleCheckBox.Checked && !parent.GenderFemaleCheckBox.Checked)
                gender = "Unspecified";
            else
                gender = "Complex";

            string peds;
            if (parent.PedsCheckBox.Checked)
                peds = "Yes";
            else
                peds = "No";

            string reasonNoPhoto = "";
            // Probably should use radios here... just take the first one checked:
            if (parent.NoTimeForPhotoCheckBox.Checked)
                reasonNoPhoto = "No time to take; fast arrival rate";
            else if (parent.ForgotPhotoCheckBox.Checked)
                reasonNoPhoto = "Forgot to take; patient gone";

            string hospitalAuthor = parent.orgNameTextBox.Text + " - ";//"Suburban Hospital - ";
            if (parent.PatientTrackingOfficerTextBox.Text.Length != 0)
                hospitalAuthor += "Patient Tracking Officer: " + parent.PatientTrackingOfficerTextBox.Text;
            else if (parent.TriagePhysicansOrRNsTextBox.Text.Length != 0)
                hospitalAuthor += "Triage Physicians or RNs: " + parent.TriagePhysicansOrRNsTextBox.Text;
            else if (parent.OtherStationStaff.Text.Length != 0)
                hospitalAuthor += "Other Station Staff: " + parent.OtherStationStaff.Text;
            else if (parent.PhotographersTextBox.Text.Length != 0)
                hospitalAuthor += "Photographers: " + parent.PhotographersTextBox.Text;
            else
                hospitalAuthor += "Login name: " + parent.UserNameFieldLabel.Text;

            //string hospitalURL = "www.suburbanhospital.org";
            //string hospitalEmail = "info@suburbanhospital.org"; // General web site email address
            //string hospitalPhone = "(301) 896-3118";
            // PFIF specification requires these fields, intended for search matching, to be "all caps, no accents".
            // Whether this implies ASCII or "Windows" code page or something else, dunno. ASCII is most restrictive:
            string asciiFirstNameAllCaps = ConvertToASCII(parent.FirstNameTextBox.Text.ToUpper());
            // We extend PFIF spec by adding nickname/alias in parentheses
            if (parent.NickNameTextBox.Text.Length > 0)
                asciiFirstNameAllCaps += " (" + ConvertToASCII(parent.NickNameTextBox.Text.ToUpper()) + ")";
            if (parent.MiddleNameTextBox.Text.Length > 0)
                asciiFirstNameAllCaps += " " + ConvertToASCII(parent.MiddleNameTextBox.Text.ToUpper());
            string asciiLastNameAllCaps = ConvertToASCII(parent.LastNameTextBox.Text.ToUpper());
            if (parent.NameSuffixTextBox.Text.Length > 0)  // PFIF spec doesn't specifically say to do this, but makes sense.
                asciiLastNameAllCaps += " " + ConvertToASCII(parent.NameSuffixTextBox.Text.ToUpper());
            // PFIF spec requires this datetime format:
            string datePFIF = DateTime.UtcNow.ToString("yyyy-mm-ddThh:mm:ssZ"); // UTC date.  Trailing "Z" is std abbrev for "UTC".  Embedded "T" indicates start of time
            // For our purposes here, we don't need to distinguish between the person record's source_date and note's source_date
            // Corresponding entry_date's likewise, if not left blank

            string PFIF =
            "<pfif:pfif xsi:schemaLocation=\"http://zesty.ca/pfif/1.1 http://zesty.ca/pfif/1.1/pfif-1.1.xsd\">" + LF +
            // Spec says 1 or more instance of person.  Here, always exactly 1
            "<pfif:person>" + LF +
            "  <pfif:person_record_id>BHEPP Mass Disaster ID/" + parent.patientID_PrefixAndNumber + "</pfif:person_record_id>" + LF +
            "  <pfif:entry_date>" + datePFIF + "</pfif:entry_date>" + LF +  // This will be overwritten by recipient if stored in recipient's repository.  Unclear if it should be left blank on generation.
            "  <pfif:author_name>" + hospitalAuthor + "</pfif:author_name>" + LF +
            "  <pfif:author_email>" + parent.orgEmailTextBox.Text + "</pfif:author_email>" + LF +
            "  <pfif:author_phone>" + parent.orgPhoneTextBox.Text + "</pfif:author_phone>" + LF +
                // Suburban Hospital general  301.896.3939
                // Pastoral Care  301.896.3178
                // Patient Information 301.896.3118
                // Public Affairs 	301.896.2580
            "  <pfif:source_name>Possibly Lost Person Finder web site URL goes here</pfif:source_name>" + LF + // Name of home repository (PFIF or non-PFIF) of record
            "  <pfif:source_date>" + datePFIF + "</pfif:source_date>" + LF +
            "  <pfif:source_url>" + parent.orgWebSiteTextBox.Text + "</pfif:source_url>" + LF + // URL of record in home repository.
            "  <pfif:first_name>" + asciiFirstNameAllCaps + "</pfif:first_name>" + LF +
            "  <pfif:last_name>" + asciiLastNameAllCaps + "</pfif:last_name>" + LF +
            // app doesn't have this info available at time record is sent:
//            "  <pfif:home_city></pfif:home_city>" + LF +
//            "  <pfif:home_state></pfif:home_state>" + LF +
//            "  <pfif:home_neighborhood></pfif:home_neighborhood>" + LF +
//            "  <pfif:home_street></pfif:home_street>" + LF +
//            "  <pfif:home_zip></pfif:home_zip>" + LF +
            "  <pfif:photo_url>file:" + txtPhotoAttachmentShortName + "</pfif:photo_url>" + LF + // Because this is an attachment, don't use "//" after "file:"
            "  <pfif:other>" + LF +
                // Format is identifier<colon><space>short descriptor
                // if multiple lines, have identifier<colon> on separate line, intent each descriptor line typically 4 addl spaces
                // For data imported from other apps, use form <domain name>/<field name> as identifier.
            "    restrictions: " + LF +
            "        IF RECEIVED AS EMAIL, DON'T REPLY TO THIS ADDRESS." + LF +
            "        DISTRIBUTION RESTRICTED." + LF +
            "        SEE ATTACHED DOCUMENT." + LF +
            "    automated-pfif-author: NLM TriagePic Prototype" + LF + // recommended identifier
            "    on computer: " + parent.MachineNameFieldLabel.Text + LF +
            "    event: " + parent.FormEventNameLabel.Text + LF +
            "    description:" + LF; // recommended identifier
                // We could try to make this more human readable
            if (parent.NickNameTextBox.Text.Length != 0)
                PFIF += "      Nickname/Alias = " + parent.NickNameTextBox.Text + LF;
            PFIF +=
            "      Gender = " + gender + LF +
            "      Pediatric = " + peds + LF +
// do zone in note instead:           "      To Zone = " + zone + LF + 
            "      Picture = ";
            if (parent.hasPrimary())
            {
                PFIF += "Yes" + LF;
            }
            else
            {
                // Otherwise, we have no picture to send
                PFIF += "No" + LF +
                "      Why No Photo = " + reasonNoPhoto + LF;
            }
            //     Lots more to go here
            PFIF +=
            "  </pfif:other>" + LF +
            "</pfif:person>" + LF +
            "<pfif:note>" + LF +
            "  <pfif:note_record_id>Note 1, BHEPP Mass Disaster ID/" + parent.patientID_PrefixAndNumber + "</pfif:note_record_id>" + LF +
            "  <pfif:entry_date>" + datePFIF + "</pfif:entry_date>" + LF +  // This will be overwritten by recipient if stored in recipient's PFIF repository.  Unclear if it should be left blank on generation.
            "  <pfif:author_name>" + hospitalAuthor + "</pfif:author_name>" + LF +
            "  <pfif:author_email>" + parent.orgEmailTextBox.Text + "</pfif:author_email>" + LF +
            "  <pfif:author_phone>" + parent.orgPhoneTextBox.Text + "</pfif:author_phone>" + LF +
            "  <pfif:source_date>" + datePFIF + "</pfif:source_date>" + LF +
            "  <pfif:found>true</pfif:found>" + LF +
            "  <pfif:email_of_found_person>" + parent.orgEmailTextBox.Text + "</pfif:email_of_found_person>" + LF +
            "  <pfif:phone_of_found_person>" + parent.orgPhoneTextBox.Text + "</pfif:phone_of_found_person>" + LF +
            "  <pfif:last_known_location>" + LF +
            "    Disaster Triage Station, " + parent.orgNameTextBox.Text + LF +
            "    " + parent.orgStreetAddress1TextBox.Text + LF;
            if(parent.orgStreetAddress2TextBox.Text.Length > 0)
            PFIF +=
            "    " + parent.orgStreetAddress2TextBox.Text + LF;
            PFIF +=
            "    " + parent.orgTownOrCityTextBox.Text + ", " + parent.org2LetterStateTextBox.Text + " " + parent.orgZipcodeTextBox.Text + LF +
            "  </pfif:last_known_location>" + LF +
            // Some content in "text" is required.  We put the zone info here, because it is more transient and not so concerned about patient identification.
            "  <pfif:text>" + LF +
            "    Current condition:" + LF +
            "      Sent to '" + zone + "' treatment zone within hospital." + LF +
            "    Email and phone numbers given for found person are those of " + parent.orgNameTextBox.Text + " for general inquiries." + LF +
            "    See PFIF parent record for restrictions on distribution of information in this note." + LF +
            "  </pfif:text>" + LF +
            "</pfif:note>" + LF + 
            "</pfif:pfif>" + LF;

            return PFIF;
        }

        public string FormatEmailAttachmentAsEDXL_and_LPF(string zone)
        {
            return FormatEmailAttachmentAsEDXL_and_PTL_or_LPF(zone, true);
        }
        
        public string FormatEmailAttachmentAsEDXL_and_PTL(string zone)
        {
            return FormatEmailAttachmentAsEDXL_and_PTL_or_LPF(zone, false);
        }

        private string FormatEmailAttachmentAsEDXL_and_PTL_or_LPF(string zone, bool FormatAsLPF)
        {
            // returns short name of file
            string body = FormatEDXL_and_PTL_or_LPF(zone, FormatAsLPF);
            if (body.Length == 0) // Probably because of no patient name given
                return null;
            char cLF = (char)0x0A;
            string LF = cLF.ToString();
            body = body.Replace(LF, "\r\n"); // In attachments, CRLF is better than LF methinks
            string shortName = parent.patientID_PrefixAndNumber + " " + zone;
            if(FormatAsLPF)
                shortName += ".lpf";
            else
                shortName += ".ptl";
            string fileLoc = parent.dest1 + shortName;
            // Don't need this: body = EmitExtraLineFeedIfLongLine(body);
            // Create a Text File
            if (File.Exists(fileLoc))
            {
                File.Delete(fileLoc);
                if(FormatAsLPF)
                    AppBox.Show("Over-writing old LPF-format file with same name.");
                else
                    AppBox.Show("Over-writing old PTL-format file with same name.");
            }
            //if (!File.Exists(fileLoc))
            //{
            using (StreamWriter sw1 = new StreamWriter(/*parent.startupPath + */fileLoc))
            {
                sw1.Write(body);
                sw1.Close();
            }
            //}
            return shortName;
        }

        private string FormatEDXL_and_PTL_or_LPF(string zone, bool FormatAsLPF)
        {
            // EDXL is wrapper, PTL or LPF (based on PTL and TriagePic) is payload
            // PTL format from DRAFT PTL_FinalRpt_v1 0.doc, 9/4/2008, Scott Wetterhall et al, Research Triangle
            char cLF = (char)0x0A;
            string LF = cLF.ToString();

            // Don't know if adding middle name and/or suffix is legit in PTL, but do it here:
            // PTL maps from these standards:
            //   DEEDS 1.0: 1.02 Name (component 1: family name; component 2: given name),
            //   NEMSIS: E06_01 Last Name, E06_02 First Name
            //   VEDS: Name (Owner's Name, Primary Driver's Name, Subscriber's Name)
            string firstName = parent.FirstNameTextBox.Text;
            // PTL format didn't include alias, we'll add it in quotes (Sept 2009):
            if (parent.NickNameTextBox.Text.Length > 0)
                firstName += " \"" + parent.NickNameTextBox.Text + "\"";
            if (parent.MiddleNameTextBox.Text.Length > 0)
                firstName += " " + parent.MiddleNameTextBox.Text;
            string lastName = parent.LastNameTextBox.Text;
            if (parent.NameSuffixTextBox.Text.Length > 0)  // PFIF spec doesn't specifically say to do this, but makes sense.
                lastName += " " + parent.NameSuffixTextBox.Text;

            // PTL maps from these standards:
            //   DEEDS 1.0: 1.05 Sex
            //   NEMSIS: E06_11 Gender
            //   VEDS: Gender
            // Uses DEEDS encodings for values:
            string gender;
            if (parent.GenderMaleCheckBox.Checked && !parent.GenderFemaleCheckBox.Checked)
                gender = "M";
            else if (!parent.GenderMaleCheckBox.Checked && parent.GenderFemaleCheckBox.Checked)
                gender = "F";
            else if (!parent.GenderMaleCheckBox.Checked && !parent.GenderFemaleCheckBox.Checked)
                gender = "U";
            else // both boxes checked
                if (FormatAsLPF)
                    gender = "C"; // complex.  Non-DEEDS
                else 
                    gender = "U";
            // PTL cannot support "peds" tag, LPF can:
            string peds = null;
            if (FormatAsLPF)
            {
                if (parent.PedsCheckBox.Checked)
                    peds = "Y";
                else
                    peds = "N";
            }

            string distribStatus;
            // strings being matched may be by themselves or as suffix of string:
            if(parent.FormEventNameLabel.Text.IndexOf("TEST or DEMO") >= 0)
                distribStatus = "Test";
            else if (parent.FormEventNameLabel.Text.IndexOf("REAL - NOT A DRILL") >= 0) // was: IndexOf("REAL DISASTER - NOT A DRILL")
                distribStatus = "Actual";
            else
                distribStatus = "Exercise"; // our "DRILL"

            string dateEDXL = DateTime.UtcNow.ToString("yyyy-mm-ddThh:mm:ssZ"); // UTC date.  Trailing "Z" is std abbrev for "UTC".  Embedded "T" indicates start of time

            /* For REF:
            <xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema" xmlns:cap="urn:oasis:names:tc:emergency:cap:1.1" attributeFormDefault="unqualified" elementFormDefault="qualified" targetNamespace="urn:oasis:names:tc:emergency:cap:1.1" xmlns:xs="urn:oasis:names:tc:emergency:cap:1.1http://www.w3.org/2001/XMLSchema">
            <element name = "category" maxOccurs = "unbounded">
                            <simpleType>
                              <restriction base = "string">
                                <enumeration value = "Geo"/>
                                <enumeration value = "Met"/>
                                <enumeration value = "Safety"/>
                                <enumeration value = "Security"/>
                                <enumeration value = "Rescue"/>
                                <enumeration value = "Fire"/>
                                <enumeration value = "Health"/>
                                <enumeration value = "Env"/>
                                <enumeration value = "Transport"/>
                                <enumeration value = "Infra"/>
                                <enumeration value = "CBRNE"/>
                                <enumeration value = "Other"/>
                              </restriction>
                            </simpleType>
                          </element>
            (1) Code Values:
“Geo” - Geophysical (inc. landslide)
“Met” - Meteorological (inc. flood)
“Safety” - General emergency and public safety
“Security” - Law enforcement, military, homeland and local/private security
“Rescue” - Rescue and recovery
“Fire” - Fire suppression and rescue
“Health” - Medical and public health
“Env” - Pollution and other environmental
“Transport” - Public and private transportation
“Infra” - Utility, telecommunication, other non-transport infrastructure
“CBRNE” – Chemical, Biological, Radiological, Nuclear or High-Yield Explosive threat or attack
“Other” - Other events
(2) Multiple instances allowed

            */

            string results;
            // Begin EDXL wrapper
            results = "<EDXLDistribution xmlns=\"urn:oasis:names:tc:emergency:EDXL:DE:1.0\">" + LF +
            "<distributionID>NPI " + parent.orgNPI_TextBox.Text + " " + dateEDXL + "</distributionID>" + LF + // This can be any unique ID.  We'll make one up.
            "<senderID>" + parent.orgEmailTextBox.Text + "</senderID>" + LF + // must be of form actor@domainname
            "<dateTimeSent>" + dateEDXL + "</dateTimeSent>" + LF + // TO DO format: 2007-02-15T16:53:00-05:00
            "<distributionStatus>" + distribStatus + "</distributionStatus>" + LF +
            "<distributionType>Report</distributionType>" + LF + // other plausible choices are "Update"
                // Below value is default.  Alternatives to default not easily known.  Maybe use cap 1.1's <scope> of "Public", "Restricted", "Private"
                // latter could use explicitAddress blocks analogously to CAP 1.1's <addresses>
                // EDXL Spec says "combinded...", but probably a typo, spec examples say "combined..."
                // Spec example also contains "Unclassified" as value
            "<combinedConfidentiality>UNCLASSIFIED AND NOT SENSITIVE</combinedConfidentiality>" + LF + // [sic]
                // could have <senderRole> and/or recipientRole here, each with valueListUrn and value.
                // EDXL doc says examples of things <keyword> might be used to describe include event type, event cause, incident ID, and response type
                // Glenn says: for now, use CAP 1.1 as enumeration here:
            "<keyword>" + LF +
            "  <valueListUrn>urn:oasis:names:tc:emergency:cap:1.1</valueListUrn>" + LF +
            "  <value>Health</value>" + LF +
            "  <value>Rescue</value>" + LF + // Multiple values from same list OK
            "</keyword>" + LF +
                // Unclear if adding other keywords from list is good or bad idea.
                // Multiple keyword lists would be OK
                // more valueListUrn examples: urn:sandia:gov:sensors:keywords, http://www.niem.gov/EventTypeList

            // Use of "e-mail" and "DMIS COGs" are from EDXL spec example, but no real method set at time of spec.
                //"<explicitAddress>" + LF +
                //  "<explicitAddressScheme>e-mail</explicitAddressScheme>" + LF +
                //  "<explicitAddressValue>dellis@sandia.gov</explicitAddressValue>" + LF +
                //"</explicitAddress>" + LF +
                //"<explicitAddress>" + LF +
                //  "<explicitAddressScheme>DMIS COGs</explicitAddressScheme>" + LF +
                //  "<explicitAddressValue>1734</explicitAddressValue>" + LF +
                //"</explicitAddress>" + LF +
            "<targetArea>" + LF;
            // We are using the approximate center of the NIH campus, near buildings 12 & 13 and midway between Suburban Hospital & NNMC, as the
            // logical center of the BHEPP partnership: = 38.9992, -77.1024
            // (Actual - Suburban 38.9974, -77.1107
            // NNMC 39.0016, -77.0920
            // NIH CC, Patient transfer entrance 39.0022, -77.1056
            // Lister Hill 38.9937, -77.0990
            switch (parent.eventRange)
            {
                default: case 1:
                    results +=
            "  <circle>38.9992,-77.1024, 40.2</circle>" + LF; break; // latitude, longitude of Bethesda, 25 mile (40.2 km) radius
                case 2:
                    results +=
            "  <circle>38.9992,-77.1024, 80.5</circle>" + LF; break; // latitude, longitude of Bethesda, 50 mile (80.5 km) radius
                case 3:
                    results +=
            "  <circle>38.9992,-77.1024, 161</circle>" + LF; break; // latitude, longitude of Bethesda, 100 mile (161 km) radius
                case 4:
                    results +=
            "  <subdivision>US-MD</subdivision>" + LF + //ISO 3166-2 designation
            "  <subdivision>US-DC</subdivision>" + LF +
            "  <subdivision>US-VA</subdivision>" + LF +
            "  <subdivision>US-WV</subdivision>" + LF +
            "  <subdivision>US-DE</subdivision>" + LF +
            "  <subdivision>US-PA</subdivision>" + LF +
            "  <subdivision>US-NJ</subdivision>" + LF; break;
            }
            results +=
            "</targetArea>" + LF +
                // Transition from EDXL to PTL or LPF content
            "<contentObject>" + LF;
            if (FormatAsLPF)
            results +=
            "  <contentDescription>LPF notification - disaster victim arrives at hospital triage station</contentDescription>" + LF;
            else
            results +=
            "  <contentDescription>PTL add person notification</contentDescription>" + LF;

            results +=
            "  <xmlContent>" + LF +
            "    <embeddedXMLContent>" + LF;
            if (FormatAsLPF)
                results +=
            "      <lpfContent>" + LF +
            "        <version>1.2</version>" + LF; // continue versioning from where PTL left off
            else
                results +=
            "      <ptlContent>" + LF +
            "        <version>1.0</version>" + LF; // PTL rev 3 perhaps defines "version 1.1".

            results +=
            "        <login>" + LF +
            "          <username>" + parent.UserNameFieldLabel.Text + "</username>" + LF + // or we could fabricate hospitalAuthor as in FormatPFIF
                //          "          <password>password</password>  // dubious we'd want to transmit this
            "        </login>" + LF +
            "        <person>" + LF +
            "          <personId>" + parent.patientID_PrefixAndNumber + "</personId>" + LF +
                //            "          <ptlPersonId>45</ptlPersonId>" + LF +
            "          <eventName>" + parent.FormEventNameLabel.Text + "</eventName>" + LF +
                //            "          <ptlEventId>3</ptlEventId>" + LF +
            "          <organization>" + LF + // organization assigning personID, i.e., hospital
            "            <orgName>" + parent.orgNameTextBox.Text + "</orgName>" + LF +
            "            <orgId>" + parent.orgNPI_TextBox.Text + "</orgId>" + LF +
                //            "            <ptlOrgId>1</ptlOrgId>" + LF +
            "          </organization>" + LF +
            "          <lastName>" + lastName + "</lastName>" + LF +
            "          <firstName>" + firstName + "</firstName>" + LF +
            "          <gender>" + gender + "</gender>" + LF;
            if (FormatAsLPF)
            {
                results +=
            "          <genderEnum>M, F, U, C</genderEnum>" + LF +
            "          <genderEnumDesc>Male; Female; Unknown; Complex(M/F)</genderEnumDesc>" + LF;
            }
                //            "          <ethnicity>695</ethnicity>" + LF + // PTL as defined by OMB.  Uses NEMSIS encoding (Ethinicity) 690 = Hispanic or Latino, 695 = Not Hispanic or Latino
                //            "          <birthDate>1986-10-22</birthDate>" + LF +  // how to encoded Peds?
                //            "          <approxBirthDateFlag>false</approxBirthDateFlag>" + LF +
            if(FormatAsLPF)
            {
                results +=
            "          <peds>" + peds + "</peds>" + LF +  // PTL format doesn't support Peds, LPF does.
            "          <pedsEnum>Y,N</pedsEnum>" + LF +
            "          <pedsEnumDesc>Pediatric patient? Yes, No</pedsEnumDesc>" + LF;
            }
            results +=
                //            "          <race>680</race>" + LF +
                //            "          <eyeColor>BRO</eyeColor>" + LF +
                //            "          <hairColor>BLN</hairColor>" + LF +
                //            "          <distinguishingMarks></distinguishingMarks>" + LF +
                //            "          <personalBelongings></personalBelongings>" + LF +
            "          <triageCategory>" + zone + "</triageCategory>" + LF; // PTL says: red, yellow, green, black, white.  Suburban uses gray and not white
            if(FormatAsLPF)
            results +=
            "          <triageCategoryEnum>Green, BH Green, Yellow, Red, Gray, Black</triageCategoryEnum>" + LF +
            "          <triageCategoryEnumDesc>Treat eventually if needed; Treat for behavioral health; Treat soon; Treat immediately; Cannot be saved; Deceased</triageCategoryEnumDesc>" + LF;
                //            "          <emailAddress></emailAddress>" + LF +
                // In PTL rev 3 example xml, there is emergencyContact.  The design for replacing this with an EmergencyContact class is provided.
                //            "          <emergencyContact>Jack Smith, father, 919-233-4567</emergencyContact>" + LF +
                //            "          <specialNeed>2600</specialNeed>" + LF + // NEEMS encoding, Barriers to Patient Care
                //            "          <specialNeed>2620</specialNeed>" + LF +
                // Not shown: Patient's home address, personal phone numbers
                // Not shown: Location to which patient is released, transferred, or sent to when deceased
                // Begin transition back to EDXL wrapper:
            results +=
            "        </person>" + LF;
            if(FormatAsLPF)
            results +=
            "      </lpfContent>" + LF;
            else
            results +=
            "      </ptlContent>" + LF;

            results +=
            "    </embeddedXMLContent>" + LF +
            "  </xmlContent>" + LF +
            "</contentObject>" + LF +
            "</EDXLDistribution>" + LF;

            return results;
        }

        /* Example for reference:
<EDXLDistribution xmlns="urn:oasis:names:tc:emergency:EDXL:DE:1.0">
<distributionID></distributionID>
<senderID></senderID>
<dateTimeSent>2007-02-15T16:53:00-05:00</dateTimeSent>
<distributionStatus>Test</distributionStatus>
<distributionType>Update</distributionType>
<keyword></keyword>
<targetArea></targetArea>
<contentObject>
  <contentDescription>PTL add person notification</contentDescription>
  <xmlContent>
    <embeddedXMLContent>
      <ptlContent>
        <version>1.0</version>
        <login>
          <username>ptl</username>
          <password>password</password>
        </login>
        <person>
          <personId>23</personId>
          <ptlPersonId>45</ptlPersonId>
          <eventName>Hurricane Bruce</eventName>
          <ptlEventId>3</ptlEventId>
          <organization>
            <orgName>Memorial Hospital</orgName>
            <orgId></orgId>
            <ptlOrgId>1</ptlOrgId>
          </organization>
          <lastName>Smith</lastName>
          <firstName>Melinda</firstName>
          <gender>F</gender>
          <ethnicity>695</ethnicity>
          <birthDate>1986-10-22</birthDate>
          <approxBirthDateFlag>false</approxBirthDateFlag>
          <race>680</race>
          <eyeColor>BRO</eyeColor>
          <hairColor>BLN</hairColor>
          <distinguishingMarks></distinguishingMarks>
          <personalBelongings></personalBelongings>
          <triageCategory>white</triageCategory>
          <emailAddress></emailAddress>
          <emergencyContact>Jack Smith, father, 919-233-4567</emergencyContact>
          <specialNeed>2600</specialNeed>
          <specialNeed>2620</specialNeed>
          <phone>
            <phoneType>home</phoneType>
            <phoneNbr>919-123-4567</phoneNbr>
          </phone>
          <phone>
            <phoneType>cell</phoneType>
            <phoneNbr>919-999-9999</phoneNbr>
          </phone>
          <address>
            <addressType>home</addressType>
            <addressLine1>100 Main St.</addressLine1>
            <addressLine2>Apartment 3B</addressLine2>
            <city>Tampa</city>
            <county>Hillsborough</county>
            <state>NC</state>
            <postalCode>27705</postalCode>
          </address>
          <location>
            <name>Park Shelter</name>
            <startDate>2007-02-09</startDate>
            <endDate>2007-02-10</endDate>
            <disposition>1</disposition>
            <organization>
              <orgName>Durham City Parks</orgName>
              <ptlOrgId>2</ptlOrgId>
            </organization>
            <address>
              <addressType>home</addressType>
              <addressLine1>123 Elm St.</addressLine1>
              <city>Tampa</city>
              <county>Hillsborough</county>
              <state>NC</state>
              <postalCode></postalCode>
            </address>
          </location>
        </person>
      </ptlContent>
    </embeddedXMLContent>
  </xmlContent>
</contentObject>
</EDXLDistribution>
 */



        private string EmitExtraLineFeedIfLongLine(string bodyIn)
        {
            // If a line is 40 characters or more, emit extra LF for Outlook's benefit:

            // Email sent as plain text, as here, can run into problems with Outlook removing "extra" line breaks and screwing up the format.
            // The Outlook user can turn off this removal (per message or sticky), but it's better not to generate it in the first place.
            // Workarounds are:
            //   use Rich Text or HTML instead of plain text (I'd rather not)
            //   Keep the line length to under 40 characters (I found this experimentally, with Outlook 2003 SP3)
            //   Put 2 spaces at the start of the line (reportedly, but didn't work for me)
            //   Use two linebreaks in a row (but this double spaces, so not ideal)

            // FYI, to change Outlook default behavior so that line break removal is disabled, the user can do something like (depending on version):
            // Tools/Options/"Preferences" tab/"Email Options" button:
            //   Uncheck the box marked "Remove extra line breaks in plain text messages" (4th from the top)
            //   "OK","Apply", "OK" to close out the options window

            // FYI, to change this default behavior at the policy level, see ADM Template for Group Policy for Outlook 2003:
            // PART "Automatically clean up plain text messages" CHECKBOX
            // KEYNAME Software\Policies\Microsoft\Office\11.0\Outlook\Options\Mail
            // VALUENAME AutoFormatPlainText
            // VALUEON NUMERIC 1
            // VALUEOFF NUMERIC 0
            char cLF = (char)0x0A;
            string LF = cLF.ToString();
            string bodyOut = "";
            string[] bodyInArray = bodyIn.Split(cLF);
            foreach (string line in bodyInArray)
            {
                bodyOut += line + LF;
                if (line.Length >= 40)
                    bodyOut += LF;
            }
            return bodyOut;
        }

        private bool FetchBeforeSendAuthentication(int server)
        {
            // Formally, this is POP before SMTP.  POP is not covered by .Net.  Uses Lumisoft DLL.
            // Code here based very loosely on Lumisoft POP client sample.
            Debug.Assert(server == 0 || server == 1);
            Debug.Assert(parent.useBasicAuthenticationForSMTP[server]);
            Debug.Assert(parent.fetchBeforeSendAuthentication[server]);
            string s = "Can't do fetch-before-send authentication of email service by ";
            if (server == 0)
                s += "first";
            else
                s += "second";
            s += " server profile, because under the 'Email Setup' tab, ";
            if (parent.basicAuthName[server] == "")
            {
                ErrBox.Show(s + "the 'Name' field with Basic Authentication is empty");
                return false;
            }
            if (parent.basicAuthPassword[server] == "")
            {
                ErrBox.Show(s + "the 'Password' field with Basic Authentication is empty");
                return false;
            }

            POP3_Client pop3 = new POP3_Client();
            try
            {
                //pop3.Logger = new Logger();
                //pop3.Logger.WriteLog += m_pLogCallback;
                pop3.Connect(parent.emailIncomingServer[server], Convert.ToInt32(parent.portForIncoming[server]), parent.useSSL_ForIncoming[server]);
                // Glenn tried, but doesn't work, because empty strings are returned (security)
                // m_pUserName.Text = System.Net.CredentialCache.DefaultNetworkCredentials.UserName; // Glenn adds
                // m_pPassword.Text = System.Net.CredentialCache.DefaultNetworkCredentials.Password; // Glenn adds
                pop3.Authenticate(parent.basicAuthName[server], parent.basicAuthPassword[server], true);

                //m_pPop3 = pop3;
                // Should we do a fetch here?  Maybe we have to, to get pop before smtp to work.  Just accessing Messages does fetch.
                int count = pop3.Messages.Count;
                // We could instead parse & display them (see lumisoft sample code), but nahh
                pop3.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                ErrBox.Show("During attempt at fetch-before-send authentication,\nPOP3 email server reported:\n" + ex.Message);
                pop3.Dispose();
                return false;
            }
        }

        public void MySendMail(
            string [] txtFrom, string txtTo, string txtcc, string txtbcc,
            string txtSubject, string txtBody,
            string[] txtAttachmentsPath, string[] txtAttachmentsShortName)
        {
            ValidateMailServer(0); // based on parent variable values
            if (txtTo == null || txtFrom == null)
                return;
            if (txtAttachmentsPath.Length != txtAttachmentsShortName.Length)
            {
                ErrBox.Show("Internal error.  Inconsistent number of items in arrays of email attachments.");
                return;
            }
            int serverToTry = 0;

            SmtpClient emailClient;
            parent.sendingLabel.Text = "Preparing Email Message...";
            parent.sendingLabel.Refresh();
            MailMessage MyMessage = new MailMessage();
            // If we ever want to use HTML mail instead:  MyMessage.IsBodyHtml = true;
            try
            {
                MailAddress SendFrom = new MailAddress(txtFrom[serverToTry]);// could use MailAddress(txtFrom, txtNicerName)
                // For a nicer non-ASCII name, use a constructor that accepts an encoding that matches the text of the name
                // mail.From = new MailAddress("me@mycompany.com", "Steve Øbirk", Encoding.GetEncoding("iso-8859-1"));
                // Same could also be done for to, cc, bcc.

                MyMessage.From = SendFrom;

                string[] addresses = txtTo.Split(';');
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
                        MailAddress recip = new MailAddress(address);
                        MyMessage.To.Add(recip);
                    }
                    else
                    {
                        string nice = address.Substring(0, n);
                        nice = nice.Trim();
                        string unnice = address.Substring(n + 1); // skip '<'.
                        unnice = unnice.Trim();
                        unnice = unnice.Remove(unnice.Length - 1); //  Assume last char is '>', skip it too.
                        MailAddress recip = new MailAddress(unnice, nice);
                        MyMessage.To.Add(recip);
                    }
                }

                if (txtcc != null && txtcc.Length > 0)
                {
                    addresses = txtcc.Split(';');
                    foreach (string address2 in addresses)
                    {
                        // Add a carbon copy recipient.
                        address = address2.Trim();
                        if (address.Length == 0)
                            continue; // tolerates semicolon at end of last name in email list, or duplicate semicolons in a row
                        int n = address.IndexOf('<');
                        if (n < 0)
                        {
                            MailAddress recip = new MailAddress(address);
                            MyMessage.CC.Add(recip);
                        }
                        else
                        {
                            string nice = address.Substring(0, n);
                            nice = nice.Trim();
                            string unnice = address.Substring(n + 1); // skip '<'.
                            unnice = unnice.Trim();
                            unnice = unnice.Remove(unnice.Length - 1); //  Assume last char is '>', skip it too.
                            MailAddress recip = new MailAddress(unnice, nice);
                            MyMessage.CC.Add(recip);
                        }
                    }
                }
                if (txtbcc != null && txtbcc.Length > 0)
                {
                    addresses = txtbcc.Split(';');
                    foreach (string address2 in addresses)
                    {
                        // Add a blind carbon copy recipient.
                        address = address2.Trim();
                        if (address.Length == 0)
                            continue; // tolerates semicolon at end of last name in email list, or duplicate semicolons in a row
                        int n = address.IndexOf('<');
                        if (n < 0)
                        {
                            MailAddress recip = new MailAddress(address);
                            MyMessage.Bcc.Add(recip);
                        }
                        else
                        {
                            string nice = address.Substring(0, n);
                            nice = nice.Trim();
                            string unnice = address.Substring(n + 1); // skip '<'.
                            unnice = unnice.Trim();
                            unnice = unnice.Remove(unnice.Length - 1); //  Assume last char is '>', skip it too.
                            MailAddress recip = new MailAddress(unnice, nice);
                            MyMessage.Bcc.Add(recip);
                        }
                    }
                }

                MyMessage.Subject = txtSubject;
                MyMessage.Body = txtBody;

                // Attach legalese, photos, PFIF, EDXL_and_LPF (or EDXL_and_PTL)
                // Note: legalese attachment may be null during debug, but should always have it in real system, except when sending to LPF
                for (int i = 0; i < txtAttachmentsShortName.Length; i++)
                {
                    Attachment a = new Attachment(txtAttachmentsPath[i]);
                    a.Name = txtAttachmentsShortName[i];
                    MyMessage.Attachments.Add(a);
                }

                emailClient = InitEmailClient(serverToTry);
            }
            catch (Exception ex)
            {
                ErrBox.Show("Error in preparing mail\n" + ex.ToString());
                parent.sendingLabel.Text = "";
                parent.sendingLabel.Refresh();
                // Maybe replace later with:
                // sendingLabel.Text = "Error in sending mail - " + ex.ToString();
                // counterForSentMessage = 10;
                // ... or some variation when outbox queueing is created.
                MyMessage.Dispose();
                return;
            }
            if (parent.useBasicAuthenticationForSMTP[serverToTry] &&
                parent.fetchBeforeSendAuthentication[serverToTry])
            {
                parent.sendingLabel.Text = "Authenticating...";
                parent.sendingLabel.Refresh();

                if (!FetchBeforeSendAuthentication(serverToTry))
                {
                    if (!ValidateMailServer(1))
                        return;
                    TrySecondMailServer(txtFrom[1], MyMessage);
                    return;
                } // else continue
            }
            try
            {
                // There was delay seen when sending from M.Gill's laptop.  Mail would appear to send from TriagePic, but
                // wouldn't actually leave the laptop until TriagePic was closed.
                // This was discussed at http://social.msdn.microsoft.com/Forums/en-US/netfxnetcom/thread/6ce868ba-220f-4ff1-b755-ad9eb2e2b13d
                // Diagnosis using an SMTP server log seems to indicate that the .NET framework does not send a 'QUIT' message, and so it
                // does not send the mail until the connection times out (e.g., until the Socket Dispose()).
                // Fix reported by jptros :
                //   The ServicePoint member contains a member called MaxIdleTime. Setting this value to 0 seems to have the same affect as Timeout.Infinite
                //   (which presumably is also 0). Setting it to 1 caused my email to be sent out immediately.  See this link:
                //      http://msdn2.microsoft.com/en-us/library/system.net.servicepoint.maxidletime.aspx
                // To which damienb added:
                // [This works but] after sending the CPU was permanently at 50%, and looking at the processes it was 'aspnet_wp.exe'.
                // I couldn't see what it was doing though. This did not happen before I added the line to set MaxIdleTime to 1.
                // Since MaxIdleTime is in milliseconds I set it to 1000 (1 second) and now it all seems to work fine.
                // Mail gets sent within 1 second, and aspnet_wp.exe returns to negligible CPU after.

                emailClient.ServicePoint.MaxIdleTime = 1000;
                // Same thread reported additional problems with Norton, Symantec antivirus holding up outbound email until antivirus is turned off.
                // Supposedly these help (also check if antivirus has whitelist of email-sending apps)
                emailClient.ServicePoint.ConnectionLimit = 1;
                emailClient.ServicePoint.UseNagleAlgorithm = true; // reqd with Symantec Endpoint
                emailClient.Send(MyMessage);

                // MessageBox.Show(sendMsg);
                parent.sendingLabel.Text = "Email Sent:  " + txtSubject;
                parent.sendingLabel.Refresh();
                parent.counterForSentMessage = 10;
                MyMessage.Dispose();
                // CAN'T MOVE FILES TO 'SENT' FOLDER HERE, BECAUSE YOU'LL GET 'FILES IN USE BY OTHER PROCESS' ERROR DUE TO SMPTCLIENT
                // Instead, let's wait for counter to zero.
                // Maybe MyMessage.Dispose will fix it.

            }
/*
            catch (InvalidOperationException ex)
            {
                   // SMTP server wasn't set.  But we can check for that directly.
            }
*/
            // In .Net subsequent to 2.0, MS documentation seems to prefer that user catches per-addressee SmtpFailedRecipientException,
            // newly introduced in 2.0, over the all-addressee SmtpFailedRecipientsException
            catch (SmtpFailedRecipientsException ex)
            {
                // More than one email address couldn't be found; or is it one or more?
                for (int i = 0; i < ex.InnerExceptions.Length; i++)
                {
                    SmtpStatusCode status = ex.InnerExceptions[i].StatusCode;
                    if (status == SmtpStatusCode.MailboxBusy ||
                        status == SmtpStatusCode.MailboxUnavailable)
                    {
                        //Console.WriteLine("Delivery failed - retrying in 5 seconds.");
                        //System.Threading.Thread.Sleep(5000);
                        if(ValidateMailServer(1))
                            TrySecondMailServer(txtFrom[1], MyMessage);
                    }
                    else
                    {
                        AppBox.Show("Failed to deliver message to " + ex.InnerExceptions[i].FailedRecipient);
                        parent.sendingLabel.Text = "";
                        parent.sendingLabel.Refresh();
                        // Need outbox queueing here
                    }
                }
            }
//            catch (SmtpFailedRecipientException ex)
//            {
//                // One email address couldn't be found
//                if (ex.StatusCode == SmtpStatusCode.MailboxUnavailable)
//                {
//                    //mailbox is unavailable
//                }
//            }
            catch (SmtpException ex)
            {
                /* SETASIDE
                                if (// Can't get syntax right here:  ex.InnerException.GetType() == System.Net.WebException || // SMTP sever couldn't be found
                                    ex.StatusCode == SmtpStatusCode.InsufficientStorage ||  // InsufficientStorage idea if mass email - suggestions from http://forums.asp.net/t/1241365.aspx
                                    ex.StatusCode == SmtpStatusCode.ServiceNotAvailable)
                                {
                                    //Console.WriteLine("Delivery failed - retrying in 5 seconds.");
                                    //System.Threading.Thread.Sleep(5000);
                                    TrySecondMailServer(txtFrom[1], 1, MyMessage);
                                }
                                else
                                {
                                    MessageBox.Show("Error in sending mail\n" + ex.ToString());
                                    parent.sendingLabel.Text = "";
                                    parent.sendingLabel.Refresh();

                                }
                 */
                if (ValidateMailServer(1))
                    TrySecondMailServer(txtFrom[1], MyMessage);
                else
                {
                    ErrBox.Show("Error in sending mail (SMTP Exception)\n" + ex.ToString());
                    parent.sendingLabel.Text = "";
                    parent.sendingLabel.Refresh();
                }

                // Maybe replace later with:
                // sendingLabel.Text = "Error in sending mail - " + ex.ToString();
                // counterForSentMessage = 10;
                // ... or some variation when outbox queueing is created.
                MyMessage.Dispose();
            }
            catch (Exception ex)
            {
                ErrBox.Show("General error when sending mail\n" + ex.ToString());
                parent.sendingLabel.Text = "";
                parent.sendingLabel.Refresh();
                // Maybe replace later with:
                // sendingLabel.Text = "Error in sending mail - " + ex.ToString();
                // counterForSentMessage = 10;
                // ... or some variation when outbox queueing is created.
                MyMessage.Dispose();
            }
    }
           
            
        public SmtpClient InitEmailClient(int serverToTry)
        {
            Debug.Assert(serverToTry == 0 || serverToTry == 1);
            // See external doc for suggested email settings for various vendor servers
            SmtpClient emailClient = new SmtpClient(parent.emailServer[serverToTry]);
            emailClient.EnableSsl = parent.useSSL_ForSMTP[serverToTry];
            emailClient.Port = Convert.ToInt32(parent.portForSMTP[serverToTry]); //  Default: 25.
            //System.Net.NetworkCredential SMTPUserInfo = null;
            if (parent.useBasicAuthenticationForSMTP[serverToTry])
            {
                //SMTPUserInfo = new System.Net.NetworkCredential(parent.BasicAuthName[serverToTry], parent.BasicAuthPassword[serverToTry]);
                //emailClient.Credentials = SMTPUserInfo; // THIS DIDN'T WORK, Credentials were NULL after assignment
                emailClient.Credentials = new System.Net.NetworkCredential(parent.basicAuthName[serverToTry], parent.basicAuthPassword[serverToTry]);
                // One post says to avoid Authentication problems (which we're seeing), either set boolean to false BEFORE setting Credentials, or skip entirely.  We'll do latter instead of: emailClient.UseDefaultCredentials = false;
            }
            else
            {
                // Include credentials if the server requires them.
                emailClient.Credentials = System.Net.CredentialCache.DefaultNetworkCredentials; // Domain, Name, Password will appear blank in debugger.  Evidently a security feature; something to do with read access to environment.
                emailClient.UseDefaultCredentials = true;
            }
            // Possible idea for future (tho more applicable to web apps):
            // Writing email to the IIS Server's SMTP service pickup directory is another feature of System.Net.Mail.
            // The SMTP pickup directory is a special directory used by Microsoft's SMTP service to send email.
            // Any email files found in that directory are processed and delivered over SMTP. If the delivery process fails,
            // the files are stored in a queue directory for delivery at another time. If a fatal error occurs (such as a DNS
            // resolution error), the files are moved to the Badmail directory.  So, if we are using the IIS SMTP Service,
            // we can write the message directly to the PickupDirectory, and bypass the Network layer thusly:
            //   emailClient.DeliveryMethod = SmtpDeliveryMethod.PickupDirectoryFromIis;

            return emailClient;
        }

        public bool ValidateMailServer(int server)
        {
            Debug.Assert(server == 0 || server == 1);
            string s = "Can't send email message by ";
            if (server == 0)
                s += "first";
            else
                s += "second";
            s += " server, because ";
            if (parent.emailFrom[server] == null || parent.emailFrom[server].Length == 0)
            {
                if(server == 0)
                    ErrBox.Show(s + "its 'From' field under the 'Email Setup' tab is empty.");
                // else fail quietly.  No optional 2nd email server
                return false;
            }
            if (parent.emailServer[server] == null || parent.emailServer[server].Length == 0)
            {
                if (server == 0)
                    ErrBox.Show(s + "its 'SMTP Server' field under the 'Email Setup' tab is empty.");
                // else fail quietly.  No optional 2nd email server
                return false;
            }

            // Fail noisily for either server in remaining cases.  If we get this far for second server, it's no longer optional.
            if (parent.emailFrom[server].LastIndexOf('@') < 0) // Cheesy validation
            {
                ErrBox.Show(s + "its 'From' field under the 'Email Setup' tab is invalid.");
                return false;
            }
            if (parent.emailServer[server].IndexOf('.') < 0) // Cheesy validation
            {
                ErrBox.Show(s + "its 'SMTP Server' field under the 'Email Setup' tab is invalid.");
                return false;
            }
            if (parent.portForSMTP[server] == null || parent.portForSMTP[server].Length == 0 || parent.portForSMTP[server] == "0") // Cheesy validation
            {
                ErrBox.Show(s + "its 'Port' field under the 'Email Setup' tab is invalid (must be integer and greater than zero).");
                return false;
            }
            if (parent.useBasicAuthenticationForSMTP[server])
            {
                if (parent.basicAuthName[server] == null || parent.basicAuthName[server].Length == 0 ||
                    parent.basicAuthPassword[server] == null || parent.basicAuthPassword[server].Length == 0)
                    {
                        ErrBox.Show(s + "under the 'Email Setup' tab, authentication by name and password was requested, but not supplied.");
                        return false;
                    }
            }
            return true;
        }

        public bool TrySecondMailServer(string from, MailMessage MyMessage)
        {
            if (parent.useBasicAuthenticationForSMTP[1] &&
                parent.fetchBeforeSendAuthentication[1])
            {
                parent.sendingLabel.Text = "Authenticating by 2nd server profile...";
                parent.sendingLabel.Refresh();

                if (!FetchBeforeSendAuthentication(1))
                    return false; // failed
            }

            MailAddress SendFrom = new MailAddress(from);
            MyMessage.From = SendFrom;
            SmtpClient emailClient2 = InitEmailClient(1);
            try
            {
                emailClient2.ServicePoint.MaxIdleTime = 1000; // Flush connection in 1 second
                emailClient2.ServicePoint.ConnectionLimit = 1;
                emailClient2.ServicePoint.UseNagleAlgorithm = true; // Hacks to prevent antivirus software from holding up send
                emailClient2.Send(MyMessage);
            }
            catch (Exception ex)
            {
                ErrBox.Show("General error in sending mail by 2nd server\n" + ex.ToString());
                parent.sendingLabel.Text = "";
                parent.sendingLabel.Refresh();
                // Maybe replace later with:
                // sendingLabel.Text = "Error in sending mail - " + ex.ToString();
                // counterForSentMessage = 10;
                // ... or some variation when outbox queueing is created.
                // Caller is doing this MyMessage.Dispose();
                return false;
            }
            parent.sendingLabel.Text = "Email Sent via 2nd Route:  " + MyMessage.Subject;
            parent.sendingLabel.Refresh();
            parent.counterForSentMessage = 10;
            return true;
        }


/* SMTPStatusEnum, and corresponding explanations from MSDN, is:
    SystemStatus	 A system status or system Help reply.
	HelpMessage	A Help message was returned by the service.
	ServiceReady	The SMTP service is ready.
	ServiceClosingTransmissionChannel	The SMTP service is closing the transmission channel.
	Ok	The email was successfully sent to the SMTP service.
	UserNotLocalWillForward	The user mailbox is not located on the receiving server; the server forwards the e-mail.
	CannotVerifyUserWillAttemptDelivery	The specified user is not local, but the receiving SMTP service accepted the message and attempted to deliver it. This status code is defined in RFC 1123, which is available at http://www.ietf.org.
	StartMailInput	The SMTP service is ready to receive the e-mail content.
	ServiceNotAvailable	The SMTP service is not available; the server is closing the transmission channel.
	MailboxBusy	The destination mailbox is in use.
	LocalErrorInProcessing	The SMTP service cannot complete the request. This error can occur if the client's IP address cannot be resolved (that is, a reverse lookup failed). You can also receive this error if the client domain has been identified as an open relay or source for unsolicited e-mail (spam). For details, see RFC 2505, which is available at http://www.ietf.org.
	InsufficientStorage	The SMTP service does not have sufficient storage to complete the request.
	ClientNotPermitted	The client was not authenticated or is not allowed to send mail using the specified SMTP host.
	CommandUnrecognized	The SMTP service does not recognize the specified command.
	SyntaxError	The syntax used to specify a command or parameter is incorrect.
	CommandNotImplemented	The SMTP service does not implement the specified command.
	BadCommandSequence	The commands were sent in the incorrect sequence.
	MustIssueStartTlsFirst	The SMTP server is configured to accept only TLS connections, and the SMTP client is attempting to connect by using a non-TLS connection. The solution is for the user to set EnableSsl=true on the SMTP Client.
	CommandParameterNotImplemented	The SMTP service does not implement the specified command parameter.
	MailboxUnavailable	The destination mailbox was not found or could not be accessed.
	UserNotLocalTryAlternatePath	The user mailbox is not located on the receiving server. You should resend using the supplied address information.
	ExceededStorageAllocation	The message is too large to be stored in the destination mailbox.
	MailboxNameNotAllowed	The syntax used to specify the destination mailbox is incorrect.
	TransactionFailed	The transaction failed.
	GeneralFailure	The transaction could not occur. You receive this error when the specified SMTP host cannot be found. 
 */


        public void WriteEmailSettings(Options op)
        {
            // Items under Email Setup tab are written to op when change occurs, so don't have to be refreshed here.

            // Email Distribution tab:
            if (parent.emailTo == null)
                parent.emailTo = "Glenn_Pearson@nlm.nih.gov";
            op.EmailTo = parent.emailTo;
            op.EmailCc = parent.emailCc;
            op.EmailBcc = parent.emailBcc;
            op.AnonymizedEmailTo = parent.anonymizedEmailTo;
            op.AnonymizedEmailCc = parent.anonymizedEmailCc;
            op.AnonymizedEmailBcc = parent.anonymizedEmailBcc;
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // MyEmail
            // 
            this.ClientSize = new System.Drawing.Size(292, 266);
            this.Enabled = false;
            this.Name = "MyEmail";
            this.ResumeLayout(false);

        }

    }
}