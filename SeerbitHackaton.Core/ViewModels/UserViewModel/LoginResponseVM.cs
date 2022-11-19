namespace SeerbitHackaton.Core.ViewModels.UserViewModel
{
    public class LoginResponseVM
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
        public IList<string> Roles { get; set; }
    }
}
