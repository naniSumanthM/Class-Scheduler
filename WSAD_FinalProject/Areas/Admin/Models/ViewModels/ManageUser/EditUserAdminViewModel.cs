using System.ComponentModel.DataAnnotations;

namespace WSAD_FinalProject.Areas.Admin.Models.ViewModels.ManageUser
{
    public class EditUserAdminViewModel
    {
        //admin can edit any part of the user inclduing the password and the admin status.
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

        [Required]
        public bool UserIsActive { get; set; }

        [Required]
        public bool UserIsAdmin { get; set; }


    }
}