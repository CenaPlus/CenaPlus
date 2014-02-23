using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Mail;

namespace CenaPlus.Cloud.Web.Bll
{
    public static class SMTPHelper
    {
        public static string ServerHost;
        public static int Port;
        static SmtpClient SmtpClient = null;
        public static void Send(string Target, string Title, string Content)
        {
            System.Net.Mail.SmtpClient client = new SmtpClient("smtp.exmail.qq.com");//请更换为自己邮箱smtp

            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential("noreply@cenaplus.org", "yuno123");
            client.DeliveryMethod = SmtpDeliveryMethod.Network;

            MailAddress addressFrom = new MailAddress("noreply@cenaplus.org", "Cena+");
            MailAddress addressTo = new MailAddress(Target, "");

            System.Net.Mail.MailMessage message = new MailMessage(addressFrom, addressTo);
            message.Sender = new MailAddress("noreply@cenaplus.org");//请更换为自己的邮箱
            message.BodyEncoding = System.Text.Encoding.UTF8;
            message.IsBodyHtml = true;
            message.Subject = Title;
            message.Body = Content;

            client.Send(message);
        }
    }
}