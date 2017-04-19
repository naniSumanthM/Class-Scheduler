using WSAD_FinalProject.Models.Data;
using WSAD_FinalProject.Models.ViewModels.Account;

namespace WSAD_FinalProject.Areas.Admin.Models.ViewModels.ManageSession
{
    public class UserEnrollmentViewModel
    {
        public UserEnrollmentViewModel()
        {

        }
        public UserEnrollmentViewModel(SessionCart row)
        {
            this.SessionCartId = row.SessionCartId;
            this.UserId = row.UserId;
            this.User = new UserViewModel(row.Customer);
        }

        public int SessionCartId { get; set; }
        public int UserId { get; set; }
        public UserViewModel User { get; set; }
        public bool isSelected { get; set; }

    }
}