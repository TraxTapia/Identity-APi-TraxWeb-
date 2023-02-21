
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Trax.Models.Generic.Mail;
using Trax.Models.Generic.OperationResult;

namespace Trax.Framework.Generic.Email
{
    public class Email : IIdentityMessageService
    {
        private MailConfiguration MailConfiguration;

        public Email(MailConfiguration MailConfiguration)
        {
            this.MailConfiguration = MailConfiguration;
        }
        public Email()
        { }

        public Task SendAsync(IdentityMessage message)
        {
            MailMessage _MailMessage = new MailMessage();
            _MailMessage.From = new MailAddress(this.MailConfiguration.SMTPFromMail, "Aplicativos MAC");
            List<string> _To = message.Destination.Split(',').ToList();
            _To.ForEach(x => { _MailMessage.To.Add(x); });
            _MailMessage.Subject = message.Subject;
            _MailMessage.IsBodyHtml = true;
            _MailMessage.Body = message.Body;
            SmtpClient _SmtpClient = new SmtpClient();
            NetworkCredential credentials = new NetworkCredential(this.MailConfiguration.SMTPFromMail, this.MailConfiguration.SMTPPassword);
            _SmtpClient.UseDefaultCredentials = false;
            _SmtpClient.Credentials = credentials;
            _SmtpClient.Host = this.MailConfiguration.SMTPHost;
            _SmtpClient.Port = this.MailConfiguration.SMTPPort;
            _SmtpClient.EnableSsl = this.MailConfiguration.SMTPEnableSSL;
            _SmtpClient.Timeout = this.MailConfiguration.SMTPTimeOut;
            _SmtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
            return Task.Run(() => _SmtpClient.SendAsync(_MailMessage, null));
        }

        public OperationResult SendMail(MailDTO MailDTO)
        {
            OperationResult _OperationResult = new OperationResult();
            try
            {
                SmtpClient _SmtpClient = new SmtpClient();
                _SmtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                _SmtpClient.EnableSsl = this.MailConfiguration.SMTPEnableSSL;
                _SmtpClient.Timeout = this.MailConfiguration.SMTPTimeOut;
                _SmtpClient.Host = this.MailConfiguration.SMTPHost;
                _SmtpClient.Port = this.MailConfiguration.SMTPPort;
                NetworkCredential credentials = new NetworkCredential(this.MailConfiguration.SMTPUserName, this.MailConfiguration.SMTPPassword);
                _SmtpClient.UseDefaultCredentials = false;
                _SmtpClient.Credentials = credentials;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
                MailMessage _MailMessage = new MailMessage();
                _MailMessage.IsBodyHtml = true;
                _MailMessage.From = new MailAddress(this.MailConfiguration.SMTPFromMail);
                if (MailDTO.To != null && MailDTO.To.Length > 0)
                {
                    foreach (string s in MailDTO.To)
                        _MailMessage.To.Add(new MailAddress(s));
                }
                if (MailDTO.CC != null && MailDTO.CC.Length > 0)
                {
                    foreach (string s in MailDTO.CC)
                        _MailMessage.CC.Add(new MailAddress(s));
                }
                if (MailDTO.CCO != null && MailDTO.CCO.Length > 0)
                {
                    foreach (string s in MailDTO.CCO)
                        _MailMessage.Bcc.Add(new MailAddress(s));
                }
                if (MailDTO.Attached != null && MailDTO.Attached.Length > 0)
                {
                    foreach (string s in MailDTO.Attached)
                        if (System.IO.File.Exists(s))
                            _MailMessage.Attachments.Add(new Attachment(s));
                }
                _MailMessage.Subject = MailDTO.Subject;
                _MailMessage.Body = MailDTO.Body;
                _SmtpClient.Send(_MailMessage);
            }
            catch (Exception ex)
            {
                _OperationResult.SetStatusCode(OperationResult.StatusCodesEnum.INTERNAL_SERVER_ERROR);
                _OperationResult.AddException(ex);
            }
            return _OperationResult;
        }
        public OperationResult SendMail(string To, string Subject, string Body)
        {
            OperationResult _OperationResult = new OperationResult();
            try
            {
                SmtpClient _SmtpClient = new SmtpClient();
                _SmtpClient.EnableSsl = this.MailConfiguration.SMTPEnableSSL;
                _SmtpClient.Timeout = this.MailConfiguration.SMTPTimeOut;
                _SmtpClient.Host = this.MailConfiguration.SMTPHost;
                _SmtpClient.Port = this.MailConfiguration.SMTPPort;
                NetworkCredential credentials = new NetworkCredential(this.MailConfiguration.SMTPUserName, this.MailConfiguration.SMTPPassword);
                _SmtpClient.UseDefaultCredentials = false;
                _SmtpClient.Credentials = credentials;
                _SmtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
                MailMessage _MailMessage = new MailMessage();
                _MailMessage.From = new MailAddress(this.MailConfiguration.SMTPFromMail);
                _MailMessage.To.Add(new MailAddress(To));
                _MailMessage.Subject = Subject;
                _MailMessage.IsBodyHtml = true;
                _MailMessage.Body = Body;
                _SmtpClient.Send(_MailMessage);
            }
            catch (Exception ex)
            {
                _OperationResult.SetStatusCode(OperationResult.StatusCodesEnum.INTERNAL_SERVER_ERROR);
                _OperationResult.AddException(ex);
            }
            return _OperationResult;
        }
        public OperationResult SendMail(EmailRequestDTO Request)
        {
            OperationResult _OperationResult = new OperationResult();
            try
            {
                MailMessage _MailMessage = new MailMessage();
                _MailMessage.From = new MailAddress(this.MailConfiguration.SMTPFromMail, "Sistemas");
                if (Request.IsBodyHtml)
                {
                    Request.Body = this.AcentuaString(Request.Body);
                }
                if (Request.To != null && Request.To.Count > 0)
                {
                    Request.To.ForEach(x => { _MailMessage.To.Add(new MailAddress(x)); });
                }
                if (Request.CC != null && Request.CC.Count > 0)
                {
                    Request.CC.ForEach(x => { _MailMessage.CC.Add(new MailAddress(x)); });
                }
                if (Request.CCO != null && Request.CCO.Count > 0)
                {
                    Request.CCO.ForEach(x => { _MailMessage.Bcc.Add(new MailAddress(x)); });
                }
                if (Request.Attached != null && Request.Attached.Count > 0)
                {
                    Request.Attached.ForEach(x =>
                    {
                        _MailMessage.Attachments.Add(new Attachment(new MemoryStream(x.Content.ToArray()), x.FileName, "application/pdf"));
                    });
                }
                _MailMessage.Subject = Request.Subject;
                _MailMessage.IsBodyHtml = Request.IsBodyHtml;
                _MailMessage.Body = Request.Body;
                NetworkCredential credentials = new NetworkCredential(this.MailConfiguration.SMTPFromMail, this.MailConfiguration.SMTPPassword);
                using (SmtpClient _SmtpClient = new SmtpClient())
                {
                    _SmtpClient.UseDefaultCredentials = false;
                    _SmtpClient.Credentials = credentials;
                    _SmtpClient.Host = this.MailConfiguration.SMTPHost;
                    _SmtpClient.Port = this.MailConfiguration.SMTPPort;
                    _SmtpClient.EnableSsl = this.MailConfiguration.SMTPEnableSSL;
                    _SmtpClient.Timeout = this.MailConfiguration.SMTPTimeOut;
                    _SmtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls12;
                    _SmtpClient.ServicePoint.MaxIdleTime = 1;
                    _SmtpClient.Send(_MailMessage);
                };
            }
            catch (Exception ex)
            {
                _OperationResult.SetStatusCode(OperationResult.StatusCodesEnum.INTERNAL_SERVER_ERROR);
                _OperationResult.AddException(ex);
            }
            return _OperationResult;
        }

        public string AcentuaString(string cadena)
        {
            cadena = cadena.Replace("á", "&aacute;");
            cadena = cadena.Replace("é", "&eacute;");
            cadena = cadena.Replace("í", "&iacute;");
            cadena = cadena.Replace("ó", "&oacute;");
            cadena = cadena.Replace("ú", "&uacute;");
            cadena = cadena.Replace("Á", "&Aacute;");
            cadena = cadena.Replace("É", "&Eacute;");
            cadena = cadena.Replace("Í", "&Iacute;");
            cadena = cadena.Replace("Ó", "&Oacute;");
            cadena = cadena.Replace("Ú", "&Uacute;");
            cadena = cadena.Replace("¡", "&iexcl;");
            cadena = cadena.Replace("ñ", "&ntilde;");
            cadena = cadena.Replace("Ñ", "&Ntilde;");
            return cadena;
        }
    }
}
