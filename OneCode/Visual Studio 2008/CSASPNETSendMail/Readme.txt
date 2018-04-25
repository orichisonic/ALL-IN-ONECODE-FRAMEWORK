========================================================================
    ASP.Net APPLICATION : CSASPNETSendMail Project Overview
========================================================================

/////////////////////////////////////////////////////////////////////////////
Use:

The CSASPNETSendMail sample demonstrates how to send mail via System.Net.Mail
in ASP.Net Ajax.


/////////////////////////////////////////////////////////////////////////////
Code Logic:

System.Web.Mail and System.Net.Mail are both built-in libraries provided by 
Microsoft in .NET framework. System.Web.Mail is supported by all versions of 
.Net Frameworks at the moment. However, System.Web.Mail is obsolete since 
.Net 2.0 could be removed from the class library in the future. In .Net 2.0, 
the new ¡®System.Net.Mail¡¯ namespace is intrdocued. It is recommend against 
¡®System.Web.Mail¡¯, if you are developing applications targeting .Net 2.0 and later.

1. Create an instance of MailMessage.
	MailMessage NewEmail = new MailMessage();
2. Set the sender address of the mail message.
	NewEmail.From = new MailAddress(address,displayname);
3. Set the recepient address of the mail message.
   You can use NewEmail.To.Add(new MailAddress(to)) or 
   NewEmail.To = new MailAddressCollection().Add(new MailAddress(to)); 
to define the single recepient address or a collection of recepient address.
4. Set the Bcc address and CC address
	NewEmail.Bcc.Add(new MailAddress(bcc));
	NewEmail.CC.Add(new MailAddress(cc));
5. Set the subject of the mail message
	NewEmail.Subject = subject;
6. Set the body of the mail message
	NewEmail.Body = body;
7. Set the attachment of mail message
	Attachment MsgAttach = new Attachment((mAttachment));
        NewEmail.Attachments.Add(MsgAttach);
8. Set if the mail body is made in HTML
	NewEmail.IsBodyHtml = true;
9. Set the priority state of mail message
	NewEmail.Priority = MailPriority.Normal;
10. Instantiate a new instance of SmtpClient
        SmtpClient mSmtpClient = new SmtpClient();
11. Send the mail
        // Send the mail message
        mSmtpClient.Send(NewEmail);
12. Deploy the mail setting in web.config
  <system.net>
    <mailSettings>
      <smtp>
        <network host="smtp host" port="25" userName="username" password="password"/>
      </smtp>
    </mailSettings>

  </system.net>

/////////////////////////////////////////////////////////////////////////////
References:

Send Mail in ASP.Net Fourm FAQ
http://forums.asp.net/t/1352706.aspx#_What_are_the_2

/////////////////////////////////////////////////////////////////////////////


