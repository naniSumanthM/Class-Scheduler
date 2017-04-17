using WSAD_FinalProject.Models.ViewModels.Account;

namespace WSAD_FinalProject.Areas.Admin.Models.ViewModels.ManageSession
{
    public class UsersEnrolledBySessionViewModel
    {
        public UsersEnrolledBySessionViewModel()
        {
                
        }

        public UsersEnrolledBySessionViewModel(WSAD_FinalProject.Models.Data.SessionCart row)
        {
            this.SessionCartId = SessionCartId;
            this.UserId = UserId;
            this.SessionId = SessionId;
            this.User = new UserViewModel(row.Customer);
        }

        public int SessionCartId { get; set; }
        public int UserId { get; set; }
        public int SessionId { get; set; }
        public UserViewModel User { get; set; }
        public bool isSelected { get; set; }

    }
}