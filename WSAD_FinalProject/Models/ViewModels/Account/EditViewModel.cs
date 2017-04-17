using System.ComponentModel.DataAnnotations;

namespace WSAD_FinalProject.Models.ViewModels.Account
{
    public class EditViewModel
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        public string UserFirstName { get; set; }

        [Required]
        public string UserLastName { get; set; }

        [Required]
        public string UserEmailAddress { get; set; }

        public string UserPassword { get; set; }

        public string UserConfirmPassword { get; set; }

        [Required]
        public string UserCompany { get; set; }
    }
}