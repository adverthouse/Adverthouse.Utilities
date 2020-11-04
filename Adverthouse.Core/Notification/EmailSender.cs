using Adverthouse.Core.Configuration;
using System;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Adverthouse.Core.Notification
{
    public class EmailSender : IEmailSender
    {
        private readonly SMTPConfig _smtpConfig;
        public bool SendError { get; set; }
        public EmailSender(AppSettings appSettings)
        {
            _smtpConfig = appSettings.SMTPConfig;
            SendError = false;
        }

        public void SendMail(EmailData emailData, bool sendAsync = true)
        {
            string emailBody = emailData.EmailBody;
            var msg = new MailMessage
            {
                From = new MailAddress(_smtpConfig.FromAddress, (emailData.IsTestMode ? "Test - " : "") + emailData.EmailSubject)
            };
            if (!String.IsNullOrWhiteSpace(emailData.CCAddresses))
            {
                foreach (string pCC in emailData.CCAddresses.Split(';'))
                {
                    msg.CC.Add(pCC);
                }
            }
            if (emailData.IsTestMode)
            {
                emailBody = emailBody.Replace("<!-- MailInfo -->", "<br>to: " + emailData.ToAddress + "<br>cc: " + emailData.CCAddresses + "<br> bcc_to:" + _smtpConfig.BCCAddresses);
                string testReveivers = _smtpConfig.TestReceiver;
                foreach (string pto in testReveivers.Split(';'))
                {
                    msg.To.Add(pto);
                }
            }
            else
            {
                if (!String.IsNullOrWhiteSpace(emailData.ToAddress))
                {
                    foreach (string pto in emailData.ToAddress.Split(';'))
                    {
                        msg.To.Add(pto);
                    }
                }
                if (!string.IsNullOrWhiteSpace(emailData.CCAddresses))
                {
                    msg.CC.Add(emailData.CCAddresses);
                }
                if (_smtpConfig.IsBCCEnabled)
                {
                    if (String.IsNullOrWhiteSpace(_smtpConfig.BCCAddresses))
                    {
                        foreach (string pbCC in _smtpConfig.BCCAddresses.Split(';'))
                        {
                            msg.Bcc.Add(pbCC);
                        }
                    }
                }
            }
            var smtp = new SmtpClient();
            smtp.Host = _smtpConfig.SMTPServer;
            smtp.Port = Convert.ToInt32(_smtpConfig.SMTPPort);
            smtp.EnableSsl = _smtpConfig.EnableSSL;
            switch (_smtpConfig.Deliverymethod)
            {
                case "Network":
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    break;
                case "PickupDirectoryFromIis":
                    smtp.DeliveryMethod = SmtpDeliveryMethod.PickupDirectoryFromIis;
                    break;
                case "SpecifiedPickupDirectory":
                    smtp.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                    break;
            } 
            smtp.UseDefaultCredentials = !_smtpConfig.IsAuthenticationEnabled;
            if (_smtpConfig.IsAuthenticationEnabled)
            {
                smtp.Credentials = new NetworkCredential(_smtpConfig.User, _smtpConfig.Password);
            }
            var encoding = _smtpConfig.Encoding;
            msg.SubjectEncoding = Encoding.GetEncoding(encoding);
            msg.BodyEncoding = Encoding.GetEncoding(encoding);
            msg.Body = emailBody;
            msg.Subject = (emailData.IsTestMode ? "Test - " : "") + emailData.EmailSubject;
            msg.IsBodyHtml = true;
            if (!String.IsNullOrWhiteSpace(emailData.Attachment)) msg.Attachments.Add(new Attachment(emailData.Attachment));
            msg.Priority = MailPriority.Normal;

            try
            {
                if (sendAsync)
                {
                    Task.Run(() => smtp.SendMailAsync(msg));
                }
                else
                {
                    smtp.Send(msg);
                }
            }
            catch (Exception ex)
            {
                SendError = true;
            }
        }


    }
}
