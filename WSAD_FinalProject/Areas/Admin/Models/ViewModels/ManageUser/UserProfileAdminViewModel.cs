using System;

namespace WSAD_FinalProject.Areas.Admin.Models.ViewModels.ManageUser
{
    public class UserProfileAdminViewModel
    {
        public int UserId { get; set; }
        public string UserFirstName { get; set; }
        public string UserLastName { get; set; }
        public string UserEmailAddress { get; set; }
        public string UserCompany { get; set; }
        public bool UserIsActive { get; set; }
        public bool UserIsAdmin { get; set; }
        public DateTime UserDateCreated { get; set; }
        public DateTime UserDateModified { get; set; }
    }
}