
namespace SeerbitHackaton.Core.Entities
{
    public class State
    {
        [Key]
        public long Id { get; set; }
        public string Name { get; set; }
        public string Capital { get; set; }
        public int StateCode { get; set; }
        public string Slogan { get; set; }
        public virtual ICollection<Lga> Lgas { get; set; }
    }
}
