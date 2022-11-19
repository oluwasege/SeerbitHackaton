
global using System.ComponentModel.DataAnnotations;

namespace SeerbitHackaton.Core.Entities
{
    public class Lga
    {
        [Key]
        public long Id { get; set; }
        public string Name { get; set; }
        public long StateId { get; set; }
        [ForeignKey(nameof(StateId))]
        public State State { get; set; }
    }
}
