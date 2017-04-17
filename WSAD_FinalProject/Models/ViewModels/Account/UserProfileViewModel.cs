using System;

namespace WSAD_FinalProject.Models.ViewModels.Account
{
    public class UserProfileViewModel
    {
        public int UserId { get; set; }

        public string UserFirstName { get; set; }

        public string UserLastName { get; set; }

        public string UserEmailAddress { get; set; }

        public string UserCompany { get; set; }

        public bool UserIsAdmin { get; set; }

        public DateTime UserDateCreated { get; set; }

    }
}