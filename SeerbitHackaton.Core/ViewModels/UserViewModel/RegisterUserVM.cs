using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeerbitHackaton.Core.ViewModels.UserViewModel
{
    public class RegisterUserVM
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string State { get; set; }
        [Required]
        public Gender Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        [Required]
        public string Role { get; set; }
    }
}
