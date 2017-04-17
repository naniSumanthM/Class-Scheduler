namespace WSAD_FinalProject.Models.ViewModels.Account
{
    public class UserViewModel
    {
        public UserViewModel()
        {
            
        }

        public UserViewModel(Data.User rowUser)
        {
            //row.UserID will be null if deleted
            this.UserId = rowUser.UserId;
            this.UserFirstName = rowUser.UserFirstName;
            this.UserLastName = rowUser.UserLastName;
            this.UserEmailAddress = rowUser.UserEmailAddress;
            this.UserCompnay = rowUser.UserCompany;
        }

        public int UserId { get; set; }
        public string UserFirstName { get; set; }
        public string UserLastName { get; set; }
        public string UserEmailAddress { get; set; }
        public string UserCompnay { get; set; }

        //extra bool property
        public bool isSelected { get; set; }

    }
}