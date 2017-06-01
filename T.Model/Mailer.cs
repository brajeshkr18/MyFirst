using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using T.Model;
using System.Configuration;
using System.IO;
using BizBrolly.Util;

using System.Net.Mail;

//using log4net;

namespace T.Model
{
    public class Mailer
    {
        public bool SendContactMailToTeam(string To, string Name, string RequesterMail, string subject, string from, string contact, string message, string CC, string BCC, bool enableSsl, string filePath)
        {
            try
            {
                ContactEmailDetails contactdetail = new ContactEmailDetails();
                
               // Emailer em = new Emailer();
                StreamReader reader = System.IO.File.OpenText(filePath);
                string body = reader.ReadToEnd();
                reader.Close();
                reader.Dispose();
                body = body.Replace("$NameOfReceiver", contactdetail.ContactNameOfReceiver);
                body = body.Replace("$message", contactdetail.ContactTemplateMessage);
                body = body.Replace("$Content", contactdetail.ContactTemplateContent);
                body = body.Replace("$Name", Name);
                body = body.Replace("$CurrentDate", DateTime.Now.ToShortDateString());
                body = body.Replace("$EMAIL", RequesterMail);
                body = body.Replace("$MOBILE", contact);
                body = body.Replace("$CMESSAGE", message);
               // em.SendMail(To, subject, body, from, CC, BCC, enableSsl);
                SendMail(To, subject, body, from, CC, BCC, enableSsl);
                return true;
            }
            catch (Exception ex)
            {
                string strEx = ex.Message;
                return false;
            }
        }
        public bool SendContactMailOnCallCreation(string To, string Name, string RequesterMail, string subject, string from, string contact, string message, string CC, string BCC, bool enableSsl, string filePath, clsActiveCall oclsActiveCall)
        {
            try
            {
                ContactOnCallDetails contactdetail = new ContactOnCallDetails();

                Emailer em = new Emailer();
                StreamReader reader = System.IO.File.OpenText(filePath);
                string body = reader.ReadToEnd();
                reader.Close();
                reader.Dispose();
                body = body.Replace("$message", message);

                body = body.Replace("$Content", contactdetail.ContactTemplateContent);
                body = body.Replace("$NameOfReceiver", Name);
                body = body.Replace("$PatientNumber", oclsActiveCall.PatientNumber);
                body = body.Replace("$RoomNumber", oclsActiveCall.RoomNumber);
                body = body.Replace("$RequestorName", RequesterMail);
                body = body.Replace("$TAT", oclsActiveCall.TAT);
                //body = body.Replace("$Name", Name);
                //body = body.Replace("$CurrentDate", DateTime.Now.ToShortDateString());
                //body = body.Replace("$EMAIL", RequesterMail);
                //body = body.Replace("$MOBILE", contact);
                //body = body.Replace("$CMESSAGE", message);
                em.SendMail(To, subject, body, from, CC, BCC, enableSsl);
                return true;
            }
            catch (Exception ex)
            {
                string strEx = ex.Message;
                return false;
            }
        }
        public void SendMail(string To, String subject, string body, string emailfrom, string CC, String BCC, bool enableSsl)
        {
            MailMessage msg = new MailMessage();
            try
            {
                if (To != null & To != "")
                    msg.To.Add(To);
                if (subject != null & subject != "")
                    msg.Subject = subject;
                if (body != null & body != "")
                    msg.Body = body;
                if (BCC != null & BCC != "")
                    msg.Bcc.Add(BCC);

                msg.BodyEncoding = System.Text.Encoding.UTF8;
                msg.From = new MailAddress(emailfrom, "Query");
                msg.IsBodyHtml = true;
                //msg.Priority = MailPriority.High;            
                SmtpClient client = new SmtpClient();
                client.EnableSsl = enableSsl;
                client.Send(msg);

            }
            catch (Exception ex)
            {
               // logger.Error(ex.Message);
                string strEx = ex.Message;
            }
            finally
            {
                msg.Dispose();
            }
        }
    }

}