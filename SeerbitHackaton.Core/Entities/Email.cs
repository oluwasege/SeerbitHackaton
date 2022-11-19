namespace SeerbitHackaton.Core.Entities
{
    public class Email : AuditedEntity<long>
    {
        public string Subject { get; set; }
        public string Body { get; set; }
        public string Address { get; set; }
        public bool Sent { get; set; }
        public string JsonReplacements { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Modified { get; set; }
    }
}
