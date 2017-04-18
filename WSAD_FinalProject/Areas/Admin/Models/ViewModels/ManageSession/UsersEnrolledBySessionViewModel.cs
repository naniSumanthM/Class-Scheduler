using System.Collections.Generic;
using System.Linq;
using WSAD_FinalProject.Models.Data;
using WSAD_FinalProject.Models.ViewModels.Account;

namespace WSAD_FinalProject.Areas.Admin.Models.ViewModels.ManageSession
{
    public class UsersEnrolledBySessionViewModel
    {
        public UsersEnrolledBySessionViewModel()
        {

        }

        public UsersEnrolledBySessionViewModel(int sessionId, IEnumerable<SessionCart> sessionCartDTOs)
        {
            //Capture session -- even if there are no enrollments
            this.SessionId = sessionId;

            //If there are enrollments, generate userEnrollment View Model and put it in Enrollments
            Enrollments = sessionCartDTOs.Select(x => new UserEnrollmentViewModel((x))).ToList();

        }

        public int SessionId { get; set; }
        public List<UserEnrollmentViewModel> Enrollments { get; set; }
    }

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