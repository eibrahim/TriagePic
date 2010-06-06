/* From "Skittles.CBDotNet" at http://dotnetdilemmas.blogspot.com/2008/11/pop-before-smtp.html
   (See also: http://stackoverflow.com/questions/282369/pop-before-smtp)
Friday, November 14, 2008
POP-Before-SMTP
Dot .Net Dilemmas

I recently had to move one of my web applications off one hosting provider (that changed their trust levels without informing anyone)
and on to a new hosting provider where I could set the trust level in my web.config file. I wanted to keep the web and mail service
on the old provider as transferring the domain to the new provider could have meant even more downtime for my client and she really
couldn't afford that (who could at the moment?).

Anyhow, all was ok until I was testing the application on the new server, went to purchases some sessions and BANG! Another issue raised its ugly head; 
"The server rejected one or more recipient addresses. The server response was: 450 : Recipient address rejected: Greylisted for 5 minutes"

I contacted my old hosting provider and the response I got back was;
"If a valid POP login is not received before sending mail through the server, then the mail is greylisted and held for 5 minutes before a retry"

To the point but, it didn’t actually explain how I might do this. They actually suggested that;
“The easiest way would be to click on "Send/Receive" in the email software before sending the email."

Even though they knew that the email was being created from within code.
I posted a message on Stackoverflow, and found out that I would basically have to write the code myself, as ‘POP3 is not built into the .NET framework’.

Below is the code that I put in place. Basically, I do a POP-Before-SMTP. This logs me into the POP3 server, my credentials are authenticated and I
can send the email that I need to send, immediately after the call. (I tried to put snapshots in here but the quality was awful.)

Ultimately, I will move everything over the new hosting provider (and hopefully talk my client into moving to a dedicated server)
but for now, this works perfectly well.
*/
using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace Pop3
{
    public class Pop3Client
    {
        private Pop3Credential m_credential;
        private const int m_pop3port = 110;
        private const int MAX_BUFFER_READ_SIZE = 256;
        private long m_inboxPosition = 0;
        private long m_directPosition = -1;
        private Socket m_socket = null;
        private Pop3Message m_pop3Message = null;

        public Pop3Credential UserDetails
        {
            set { m_credential = value; }
            get { return m_credential; }
        }

        public string From
            {get { return m_pop3Message.From; }}

        public string To
            {get { return m_pop3Message.To; }}

        public string Subject
            {get { return m_pop3Message.Subject; }}

        public string Body
            {get { return m_pop3Message.Body; }}

        public IEnumerator MultipartEnumerator
            {get { return m_pop3Message.MultipartEnumerator; }}

        public bool IsMultipart
            {get { return m_pop3Message.IsMultipart; }}


        public Pop3Client(string user, string pass, string server)
        {
            m_credential = new Pop3Credential(user,pass,server);
        }

        private Socket GetClientSocket()
        {
            Socket s = null;

            try
            {
                IPHostEntry hostEntry = null;

                // Get host related information.

                hostEntry = Dns.GetHostEntry(m_credential.Server);

                // Loop through the AddressList to obtain the supported AddressFamily. This is to avoid an exception that
                // occurs when the host IP Address is not compatible with the address family (typical in the IPv6 case).

                foreach(IPAddress address in hostEntry.AddressList)
                {
                    IPEndPoint ipe = new IPEndPoint(address, m_pop3port);

                    Socket tempSocket =
                    new Socket(ipe.AddressFamily,
                    SocketType.Stream, ProtocolType.Tcp);

                    tempSocket.Connect(ipe);

                    if(tempSocket.Connected)
                    {
                        // we have a connection.
                        // return this socket ...
                        s = tempSocket;
                        break;
                    }
                    else
                    {
                        continue;
                    }
                }
            }
            catch(Exception e)
            {
                throw new Pop3ConnectException(e.ToString());
            }
            // throw exception if can't connect ...
            if(s == null)
            {
                throw new Pop3ConnectException("Error : connecting to " + m_credential.Server);
            }
            return s;
        }

        private string GetPop3String()
        {
            if(m_socket == null)
            {
                throw new Pop3MessageException("Connection to POP3 server is closed");
            }

            byte[] buffer = new byte[MAX_BUFFER_READ_SIZE];
            string line = null;

            try
            {
                int byteCount = m_socket.Receive(buffer,buffer.Length,0);
                line = Encoding.ASCII.GetString(buffer, 0, byteCount);
            }
            catch(Exception e)
            {
            throw new Pop3ReceiveException(e.ToString());
            }

            return line;
        }

        private void LoginToInbox()
        {
            string returned;

            // send username ...
            Send("user " + m_credential.User);

            // get response ...
            returned = GetPop3String();

            if( !returned.Substring(0,3).Equals("+OK") )
            {
                throw new Pop3LoginException("login not excepted");
            }

            // send password ...
            Send("pass " + m_credential.Pass);

            // get response ...
            returned = GetPop3String();

            if( !returned.Substring(0,3).Equals("+OK") )
            {
                throw new Pop3LoginException("login/password not accepted");
            }
        }


        public void CloseConnection()
        {
            Send("quit");

            m_socket = null;
            m_pop3Message = null;
        }

       public void OpenInbox()
       {
            // get a socket ...
            m_socket = GetClientSocket();

            // get initial header from POP3 server ...
            string header = GetPop3String();

            if( !header.Substring(0,3).Equals("+OK") )
            {
                throw new Exception("Invalid initial POP3 response");
            }

            // send login details ...
            LoginToInbox();
        }
    }
}

/*
Login to the POP server just before you try to send the email.. 

p = new Pop3Client("user","password","server");
if(p != null)
{
p.OpenInbox();
// ...Send Email.
}
*/