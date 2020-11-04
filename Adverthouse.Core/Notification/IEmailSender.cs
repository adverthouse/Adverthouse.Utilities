namespace Adverthouse.Core.Notification
{
    public interface IEmailSender
    {
        bool SendError { get; set; }
        void SendMail(EmailData emailData, bool sendAsync = true);
    }
}