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
    public class SessionCartController : Controller
    {
        /// <summary>
        /// Returns a list of sessions the user has registered for
        /// </summary>
        [HttpGet]
        public ActionResult Index()
        {
            List<SessionCartViewModel> enrollmentList;

            using (WSADDbContext context = new WSADDbContext())
            {
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
            return View(enrollmentList);
        }

        /// <summary>
        /// Post back for the sessions registered by the user
        /// </summary>
        /// <param name="sessionsToAdd"></param>
        [HttpPost]
        public ActionResult AddToSession(List<SessionViewModel> sessions)
        {
            //Verify that sessionToAdd is not null
            if (sessions == null)
            {
                return RedirectToAction("Index");
            }

            //Capture Sessions to Add (filter by isSelected)
            sessions = sessions.Where(p => p.isSelected).ToList();

            //If there are no sessions to add, then redirect to sessionCart index
            if (!(sessions.Count > 0))
            {
                return RedirectToAction("Index");
            }

            //get user from user.identity.name
            string emailAddress = User.Identity.Name;

            //get user from the database -- we need their user id
            using (WSADDbContext context = new WSADDbContext())
            {
                //Get user info
                //Get user id from DB
                int userId = context.Users
                    .Where(row => row.UserEmailAddress == emailAddress)
                    .Select(row => row.UserId)
                    .FirstOrDefault();


                foreach (SessionViewModel sessionVM in sessions)
                {
                    //check to see if the user, session combo already exists, then we say already enrolled
                    //does not make sense to do it here, since we do not have quantity
                    //create session cart dto

                    SessionCart sessionCartDTO = new SessionCart()
                    {
                        //add product id and user id to dto
                        UserId = userId,
                        SessionId = sessionVM.SessionId
                    };
                    //add dto to db conetext
                    context.SessionCartItems.Add(sessionCartDTO);
                }

                context.SaveChanges();
            }

            //redirect to shopping cart index
            return RedirectToAction("Index");
        }

        /// <summary>
        /// User Can View the details of a session
        /// </summary>
        /// <param name="id"></param>
        [HttpGet]
        public ActionResult SessionProfile(int? id = null)
        {
            string sessionId = Session.SessionID;
            SessionCartProfileViewModel sessionCartProfileVM;

            using (WSADDbContext context = new WSADDbContext())
            {
                Session sessionDTO;
                if (id.HasValue)
                {
                    sessionDTO = context.Sessions.Find(id.Value);
                }
                else
                {
                    sessionDTO = context.Sessions.FirstOrDefault(row => row.SessionTitle == sessionId);
                }
                if (sessionDTO == null)
                {
                    return Content("Session Invalid");
                }

                sessionCartProfileVM = new SessionCartProfileViewModel()
                {
                    SessionId = sessionDTO.SessionId,
                    SessionTitle = sessionDTO.SessionTitle,
                    SessionDescription = sessionDTO.SessionDescription,
                    SessionPresenter = sessionDTO.SessionPresenter,
                    SessionAddress = sessionDTO.SessionAddress,
                    SessionRoom = sessionDTO.SessionRoom,
                    SessionSeatsAvailable = sessionDTO.SessionSeatsAvailable,
                    SessionDateCreated = sessionDTO.SessionDateCreated,
                    SessionDateModified = sessionDTO.SessionDateModified
                };
            }
            return View(sessionCartProfileVM);
        }

        /// <summary>
        /// User can remove session from cart
        /// </summary>
        [HttpPost]
        public ActionResult Delete(List<SessionCartViewModel> collectionOfCartItems)
        {
            //Filter collection of Sessions to find the selected items only
            var cartItemsToDelete = collectionOfCartItems.Where(x => x.isSelected == true);

            //Get reference to the database
            using (WSADDbContext context = new WSADDbContext())
            {
                //iterate and delete each object
                foreach (var sessionItem in cartItemsToDelete)
                {
                    var dtoToDelete = context.SessionCartItems.FirstOrDefault(row => row.SessionCartId == sessionItem.SessionCartId);
                    context.SessionCartItems.Remove(dtoToDelete);
                }
                context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

    }
}


