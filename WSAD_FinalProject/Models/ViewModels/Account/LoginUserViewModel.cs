using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WSAD_FinalProject.Models.ViewModels.Account
{
    public class LoginUserViewModel
    {
        [Required(ErrorMessage = "Cannot find @")]
        [Display(Name = "Email Address")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string UserEmailAddress { get; set; }
        public string UserPassword { get; set; }
        public bool UserRememberMe { get; set; }
    }
}