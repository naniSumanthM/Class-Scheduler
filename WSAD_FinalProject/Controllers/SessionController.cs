using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WSAD_FinalProject.Models.Data;
using WSAD_FinalProject.Models.ViewModels.Session;
using WSAD_FinalProject.Models.ViewModels.SessionCart;

namespace WSAD_FinalProject.Controllers
{
    [Authorize]
    public class SessionController : Controller
    {
        public ActionResult Index()
        {
            List<SessionViewModel> sessionVm;
            List<SessionCartViewModel> enrollmentList;
            SessionIndexViewModel model = new SessionIndexViewModel();

            using (WSADDbContext context = new WSADDbContext())
            {
                sessionVm = context.Sessions
                    .ToArray()
                    .Select(x => new SessionViewModel(x))
                    .ToList();

                //Get user info
                string emailAddress = User.Identity.Name;

                //Get user id from DB
                int userId = context.Users
                    .Where(x => x.UserEmailAddress == emailAddress)
                    .Select(x => x.UserId)
                    .FirstOrDefault();

                //Get enrollment sessions
                //Generate EnrollmentViewModel
                enrollmentList = context.SessionCartItems.Where(x => x.UserId == userId)
                    .ToArray()
                    .Select(x => new SessionCartViewModel(x))
                    .ToList();
            }
            model.Sessions = sessionVm;
            model.RegisteredSessions = enrollmentList;

            return View(model);
        }
    }
}


