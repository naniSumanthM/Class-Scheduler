
using System.ComponentModel.DataAnnotations;

namespace WSAD_FinalProject.Models.ViewModels.Account
{
    public class CreateUserViewModel
    {
        [Required]
        public string UserFirstName { get; set; }

        [Required]
        public string UserLastName { get; set; }

        [Required(ErrorMessage = "Cannot find @")]
        [Display(Name = "Email Address")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string UserEmailAddress { get; set; }

        [Required]
        public string UserCompany { get; set; }

        [Required]
        public string UserPassword { get; set; }

        [Required]
        public string UserPasswordConfirm { get; set; }
    }
}