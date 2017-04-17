using WSAD_FinalProject.Models.ViewModels.Account;
using WSAD_FinalProject.Models.ViewModels.Session;

namespace WSAD_FinalProject.Areas.Admin.Models.ViewModels.ManageUser
{
    public class SessionsEnrolledByUserViewModel
    {
        public SessionsEnrolledByUserViewModel()
        {
            
        }

        public SessionsEnrolledByUserViewModel(WSAD_FinalProject.Models.Data.SessionCart row)
        {
            this.SessionCartId = SessionCartId;
            this.UserId = UserId;
            this.SessionId = SessionId;
            this.Session = new SessionViewModel(row.Session);
        }

        public int SessionCartId { get; set; }
        public int UserId { get; set; }
        public int SessionId { get; set; }    
        public SessionViewModel Session { get; set; }  
        public bool isSelected { get; set; }
    }
}
