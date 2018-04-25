using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net.Mail;

public partial class _Default : System.Web.UI.Page 
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    public void SendEmail(string from, string to, string bcc, string cc, string subject, string body, string mAttachment)
    {

        // Instantiate a new instance of MailMessage
        MailMessage NewEmail = new MailMessage();

        #region From
        // Set the sender address of the mail message
        NewEmail.From = new MailAddress(from,from);
        #endregion

        #region To
        // Set the recepient address of the mail message
        NewEmail.To.Add(new MailAddress(to));
        // You can also use NewEmail.To = new MailAddressCollection().Add(new MailAddress(to)); to set the collection
        #endregion

        #region BCC
        // Set the Bcc address of the mail message
        NewEmail.Bcc.Add(new MailAddress(bcc));
        #endregion
        
        #region CC
        // Check if the cc value is null or an empty value
        if ((cc != null) && (cc != string.Empty))
        {
            // Set the CC address of the mail message
            NewEmail.CC.Add(new MailAddress(cc));
        }
        #endregion

        #region Subject
        // Set the subject of the mail message
        NewEmail.Subject = subject;
        #endregion

        #region Body
        // Set the body of the mail message
        NewEmail.Body = body;
        #endregion

        #region Attachment
        Attachment MsgAttach = new Attachment((mAttachment));
        NewEmail.Attachments.Add(MsgAttach);
        #endregion

        #region Deployment
        // Secify the format of the body as HTML
        NewEmail.IsBodyHtml = true;
        // Set the priority of the mail message to normal
        NewEmail.Priority = MailPriority.Normal;
        #endregion


        // Instantiate a new instance of SmtpClient
        SmtpClient mSmtpClient = new SmtpClient();

        // Send the mail message
        mSmtpClient.Send(NewEmail);


    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        string from = "SenderAddress";
        string to = "RecepientAddress";
        string bcc = "BCCAddress";
        string cc = "CCAddress";
        string subject = "TestMail";
        string body = "TestBody";
        string mAttachment = "FileURL";
        SendEmail(from, to, bcc, cc, subject, body, mAttachment);


    }
}
