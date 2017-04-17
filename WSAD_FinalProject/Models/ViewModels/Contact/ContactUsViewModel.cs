using System;
using System.ComponentModel.DataAnnotations;

namespace WSAD_FinalProject.Models.ViewModels.Contact
{
    public class ContactUsViewModel
    {

        [Required]
        public string ContactName { get; set; }

        [Required(ErrorMessage = "Cannot find @")]
        [Display(Name = "Email Address")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string ContactEmail { get; set; }

        [Required]
        public string ContactReason { get; set; }

        [Required]
        public string ContactDetail { get; set; }

    }
}