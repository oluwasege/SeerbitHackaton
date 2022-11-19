
namespace SeerbitHackaton.Core.Entities
{
    public class FileUpload : FullAuditedEntity<Guid>
    {
        public string Name { get; set; }

        public string Path { get; set; }

        public string ContentType { get; set; }
    }
}
