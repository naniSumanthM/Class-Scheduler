using System;
using WSAD_FinalProject.Models.Data;

namespace WSAD_FinalProject.Areas.Admin.Models.ViewModels.ManageUser
{
    public class ManageUserViewModel
    {
        public ManageUserViewModel()
        {
                
        }

        public ManageUserViewModel(User userDto)
        {
            UserId = userDto.UserId;
            UserFirstName = userDto.UserFirstName;
            UserLastName = userDto.UserLastName;
            UserEmailAddress = userDto.UserEmailAddress;
            UserCompany = userDto.UserCompany;
            UserIsActive = userDto.UserIsActive;
            UserIsAdmin = userDto.UserIsAdmin;
            UserDateCreated = userDto.UserDateCreated;
            UserDateModified = userDto.UserDateModified;
        }

        public int UserId { get; set; }
        public string UserFirstName { get; set; }
        public string UserLastName { get; set; }
        public string UserEmailAddress { get; set; }
        public string UserCompany { get; set; }
        public bool UserIsActive { get; set; }
        public bool UserIsAdmin { get; set; }
        public DateTime UserDateCreated { get; set; }
        public DateTime UserDateModified { get; set; }

        public bool isSelected { get; set; }
    }
}