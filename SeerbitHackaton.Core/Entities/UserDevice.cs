namespace Shared.Entities
{
    public class UserDevice : FullAuditedEntity<long>
    {
        public long GraduateId { get; set; }
        public string RegistrationToken { get; set; }
    }
}
