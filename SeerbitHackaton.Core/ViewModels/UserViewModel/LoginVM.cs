namespace SeerbitHackaton.Core.ViewModels.UserViewModel
{
    public class LoginVM
    {
        [Required(ErrorMessage = "Email is required")]
        public string EmailAddress { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
}
