
namespace SeerbitHackaton.Core.ViewModels
{
    public class SendEmailApiVM
    {
        public SendEmailApiVM()
        {
            Sender = null;
            AttachmentPaths = null;
        }

        public string[] EmailAddresses { get; set; }
        public EmailType EmailType { get; set; }
        public Dictionary<string, string> Replacements { get; set; }
        public string Sender { get; set; }
        public IEnumerable<string> AttachmentPaths { get; set; }
    }
}
