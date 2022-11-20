using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeerbitHackaton.Services.Interfaces
{
    public interface IEmailService
    {
        Task<bool> SendMail(List<string> destination, string subject, string body);
        Task<bool> SendEmail(MailRequest mailRequest);
    }
}
