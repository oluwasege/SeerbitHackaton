namespace SeerbitHackaton.Core.ViewModels
{
    public class UserVM
    {
        public long Id { get; set; }

        public string FullName { get; set; }
        public string Email { get; set; }

        public static implicit operator UserVM(User model)
        {
            return model == null ? null : new UserVM
            {
                FullName = model.FullName,
                Email = model.Email,
                Id = model.Id
            };
        }
    }


}