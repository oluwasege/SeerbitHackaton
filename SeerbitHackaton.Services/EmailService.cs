global using SeerbitHackaton.Core.Utils;
using Microsoft.Extensions.Options;
using System.Net.Mail;
using System.Net.Mime;
using System.Net;
using System.Text;


namespace SeerbitHackaton.Services
{
    public class EmailService : IEmailService
    {



        /// <summary>
        /// the smtp Config settings
        /// </summary>
        private readonly SmtpConfigSettings _smtpConfigSettings;


        /// <summary>
        /// Initializes a new instance of the <see cref="EmailService"/> class.
        /// </summary>
        /// <param name="converter">The converter.</param>
        /// <param name="emailLink">The email link.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="env">The env.</param>
        /// <param name="smtpConfigSettings">The SMTP configuration settings.</param>
        public EmailService(IOptions<SmtpConfigSettings> smtpConfigSettings)
        {
            _smtpConfigSettings = smtpConfigSettings.Value;
        }

        /// <summary>
        /// Sends the mail.
        /// </summary>
        /// <param name="recipient">The recipient.</param>
        /// <param name="replacements">The replacements.</param>
        /// <param name="subject">The subject.</param>
        /// <param name="emailTemplatePath">The email template path.</param>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>
        public async Task<bool> SendMail(List<string> recipient, string subject, string body)
         => await BuildMailBody(recipient, subject, body);

        /// <summary>
        /// Builds the mail body.
        /// </summary>
        /// <param name="destination">The destination.</param>
        /// <param name="replacements">The replacements.</param>
        /// <param name="subject">The subject.</param>
        /// <param name="templateUrl">The template URL.</param>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>
        private async Task<bool> BuildMailBody(List<string> destination, string subject, string body)
        {
            //var emailTemplatePath = Path.Combine(_env.ContentRootPath, templateUrl);

            var msg = new MailRequest
            {
                Recipient = destination,
                Subject = subject,
                IsHtmlBody = true,
                Body = body
            };

            return await SendEmail(msg);
        }

        /// <summary>
        /// Generates the email HTML body.
        /// </summary>
        /// <param name="replacements">The replacements.</param>
        /// <param name="path">The path.</param>
        /// <returns>System.String.</returns>
        /// <exception cref="NullReferenceException">Email Template not found for {path}</exception>

        /// <summary>
        /// Sends the email.
        /// </summary>
        /// <param name="mailRequest">The mail request.</param>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>
        public async Task<bool> SendEmail(MailRequest mailRequest)
        {

            try
            {
                if (!string.IsNullOrEmpty(mailRequest.Body) && !string.IsNullOrEmpty(mailRequest.Subject) && mailRequest.Recipient.Count > 0)
                {
                    var mailMessage = new MailMessage();

                    for (int i = 0; i < mailRequest.Recipient.Count; i++)
                    {
                        mailMessage.To.Add(new MailAddress(mailRequest.Recipient[i]));
                    }

                    if (mailRequest?.CC != null && mailRequest.CC.Count > 0)
                    {
                        for (int i = 0; i < mailRequest.CC.Count; i++)
                        {
                            mailMessage.To.Add(new MailAddress(mailRequest.CC[i]));
                        }
                    }

                    if (mailRequest?.BCC != null && mailRequest.BCC.Count > 0)
                    {
                        for (int i = 0; i < mailRequest.BCC.Count; i++)
                        {
                            mailMessage.To.Add(new MailAddress(mailRequest.BCC[i]));
                        }
                    }

                    mailMessage.Subject = mailRequest.Subject;
                    mailMessage.Priority = MailPriority.High;
                    mailMessage.Body = mailRequest.Body;
                    //if (mailMessage.Subject == EmailSubject.Request)
                    //{
                    //    var file = HtmlToPdfHelper.ConvertToPDFBytes(_converter, mailMessage.Body);
                    //    var stream = new MemoryStream(file);
                    //    Attachment data = new Attachment(stream, $"{mailMessage.Subject}.pdf", "application/pdf");
                    //    System.Net.Mime.ContentDisposition disposition = data.ContentDisposition;
                    //    disposition.CreationDate = DateTime.Now;
                    //    disposition.ModificationDate = DateTime.Now;
                    //    disposition.DispositionType = DispositionTypeNames.Attachment;
                    //    mailMessage.Attachments.Add(data);//Attach the file  

                    //}
                    //AlternateView view = AlternateView.CreateAlternateViewFromString(mailMessage.Body, null, MediaTypeNames.Text.Html);
                    //LinkedResource resource = new LinkedResource(Path.Combine(_env.ContentRootPath, @"Filestore\EmailTemplate\images\logo.png"));
                    //resource.ContentId = "id1";
                    //mailMessage.AlternateViews.Add(view);


                    /*
                    BodyBuilder builder = new BodyBuilder()
                    {
                        HtmlBody = mailRequest.Body
                    };

                    if (mailRequest.Attachments != null)
                    {
                        byte[] fileBytes;
                        foreach (var file in mailRequest.Attachments)
                        {
                            if (file.Length > 0)
                            {
                                using (var ms = new MemoryStream())
                                {
                                    file.CopyTo(ms);
                                    fileBytes = ms.ToArray();
                                }
                                builder.Attachments.Add(file.FileName, fileBytes, MimeKit.ContentType.Parse(file.ContentType));
                            }
                        }
                    }
                    */
                    mailMessage.From = new MailAddress(_smtpConfigSettings.Mail, _smtpConfigSettings.DisplayName);
                    mailMessage.IsBodyHtml = true;

                    var smtpClient = new SmtpClient();

                    var credential = new NetworkCredential(_smtpConfigSettings.UserName, _smtpConfigSettings.Password);
                    smtpClient.Host = _smtpConfigSettings.Host;
                    smtpClient.Port = _smtpConfigSettings.Port;
                    smtpClient.UseDefaultCredentials = true;
                    smtpClient.Credentials = credential;
                    // Create the HTML view
                    AlternateView htmlView = AlternateView.CreateAlternateViewFromString(
                                                                 mailMessage.Body,
                                                                 Encoding.UTF8,
                                                                 MediaTypeNames.Text.Html);
                    // Create a plain text message for client that don't support HTML

                    //string mediaType = MediaTypeNames.Image.Jpeg;
                    //LinkedResource img = new LinkedResource(Path.Combine(_env.ContentRootPath, @"Filestore\EmailTemplate\images\logo.png"), mediaType);
                    //// Make sure you set all these values!!!
                    //img.ContentId = "EmbeddedContent_1";
                    //img.ContentType.MediaType = mediaType;
                    //img.TransferEncoding = TransferEncoding.Base64;
                    //img.ContentType.Name = img.ContentId;
                    //img.ContentLink = new Uri("cid:" + img.ContentId);
                    //htmlView.LinkedResources.Add(img);
                    //LinkedResource img2 = new LinkedResource(Path.Combine(_env.ContentRootPath, @"Filestore\EmailTemplate\images\hero.png"), mediaType);
                    //// Make sure you set all these values!!!
                    //img2.ContentId = "EmbeddedContent_2";
                    //img2.ContentType.MediaType = mediaType;
                    //img2.TransferEncoding = TransferEncoding.Base64;
                    //img2.ContentType.Name = img2.ContentId;
                    //img2.ContentLink = new Uri("cid:" + img2.ContentId);
                    //htmlView.LinkedResources.Add(img2);
                    //mailMessage.AlternateViews.Add(htmlView);
                    //LinkedResource img3 = new LinkedResource(Path.Combine(_env.ContentRootPath, @"Filestore\EmailTemplate\images\facebook.png"), mediaType);
                    //// Make sure you set all these values!!!
                    //img3.ContentId = "EmbeddedContent_f";
                    //img3.ContentType.MediaType = mediaType;
                    //img2.TransferEncoding = TransferEncoding.Base64;
                    //img3.ContentType.Name = img3.ContentId;
                    //img3.ContentLink = new Uri("cid:" + img3.ContentId);
                    //htmlView.LinkedResources.Add(img3);
                    //LinkedResource img4 = new LinkedResource(Path.Combine(_env.ContentRootPath, @"Filestore\EmailTemplate\images\instagram.png"), mediaType);
                    //// Make sure you set all these values!!!
                    //img4.ContentId = "EmbeddedContent_i";
                    //img3.ContentType.MediaType = mediaType;
                    //img4.TransferEncoding = TransferEncoding.Base64;
                    //img4.ContentType.Name = img4.ContentId;
                    //img4.ContentLink = new Uri("cid:" + img4.ContentId);
                    //htmlView.LinkedResources.Add(img4);
                    //LinkedResource img5 = new LinkedResource(Path.Combine(_env.ContentRootPath, @"Filestore\EmailTemplate\images\twitter.png"), mediaType);
                    //// Make sure you set all these values!!!
                    //img5.ContentId = "EmbeddedContent_t";
                    //img5.ContentType.MediaType = mediaType;
                    //img5.TransferEncoding = TransferEncoding.Base64;
                    //img5.ContentType.Name = img5.ContentId;
                    //img5.ContentLink = new Uri("cid:" + img5.ContentId);
                    //htmlView.LinkedResources.Add(img5);
                    //mailMessage.AlternateViews.Add(htmlView);

                    //_logger.LogInformation("Sending email notification....");

                    await smtpClient.SendMailAsync(mailMessage);

                    return true;
                }
                return false;

            }
            catch (Exception)
            {
                //_logger.LogError(ex.StackTrace, ex, "An error occured while sending message");
                return false;
            }

        }

    }
}
