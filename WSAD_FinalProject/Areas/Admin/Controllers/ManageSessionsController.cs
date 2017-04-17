using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Web.Mvc;
using WSAD_FinalProject.Areas.Admin.Models.ViewModels.ManageSession;
using WSAD_FinalProject.Models.Data;

namespace WSAD_FinalProject.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ManageSessionsController : Controller
    {
        /// <summary>
        /// Return a tabular format of sessions that are available for sign up (USER)
        /// </summary>
        public ActionResult Index()
        {
            List<ManageSessionViewModel> collectionOfSessionVM = new List<ManageSessionViewModel>();
            //setup db context
            using (WSADDbContext context = new WSADDbContext())
            {
                //retreive all sessions from database
                var dbSessions = context.Sessions;

                //move users into ViewModel obj
                foreach (var sessionDTO in dbSessions)
                {
                    collectionOfSessionVM.Add(new ManageSessionViewModel(sessionDTO));
                }
            }
            //send ViewModel Collecton to the View
            return View(collectionOfSessionVM);
        }

        [HttpGet]
        public ActionResult CreateSession()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateSession(CreateSessionViewModel newSession)
        {
            bool sessionCreated = false;

            //admin has the right to populate this, and commit changes to the database
            if (newSession == null)
            {
                ModelState.AddModelError("", "No Message Provided");
                return View();
            }

            //form verification
            if (string.IsNullOrWhiteSpace(newSession.SessionTitle) ||
                string.IsNullOrWhiteSpace(newSession.SessionDescription) ||
                string.IsNullOrWhiteSpace(newSession.SessionPresenter) ||
                string.IsNullOrWhiteSpace(newSession.SessionAddress) ||
                string.IsNullOrWhiteSpace(newSession.SessionRoom))
            {
                ModelState.AddModelError("", "All Fields Required!");
                return View();
            }

            //Seats Verification
            if (newSession.SessionSeatsAvailable == null || newSession.SessionSeatsAvailable < 1)
            {
                ModelState.AddModelError("", "Session Cannot Be Added");
                return View();
            }

            //TODO: Loop through all the user emails and notify about new session

            //store the session into the database
            using (WSADDbContext context = new WSADDbContext())
            {
                //Check to see if a session already exists
                if (context.Sessions.Any(row => row.SessionTitle.Equals(newSession.SessionTitle, StringComparison.OrdinalIgnoreCase)))
                {
                    ModelState.AddModelError("", "A Session with the Title " + newSession.SessionTitle + " Already Exists");
                    newSession.SessionTitle = " ";
                    return View(newSession);
                }

                Session newSessionDto = new Session()
                {
                    SessionTitle = newSession.SessionTitle,
                    SessionDescription = newSession.SessionDescription,
                    SessionPresenter = newSession.SessionPresenter,
                    SessionAddress = newSession.SessionAddress,
                    SessionRoom = newSession.SessionRoom,
                    SessionSeatsAvailable = newSession.SessionSeatsAvailable,
                    SessionDateCreated = DateTime.Now,
                    SessionDateModified = DateTime.Now
                };

                newSessionDto = context.Sessions.Add(newSessionDto);

                //commit changes to the database
                try
                {
                    context.SaveChanges();
                    sessionCreated = true;
                }
                catch (DbException d)
                {
                    return Content(d.Message);
                }
            }

            //toast - session created sucessfully
            if (sessionCreated)
            {
                TempData["SessionSuccess"] = "Session Created Successfully";
                return View();
            }

            return View();
        }

        /// <summary>
        /// Admin can view the details of a session
        /// </summary>
        /// <param name="id"></param>
        /// <returns>SessionProfileView</returns>
        public ActionResult SessionProfile(int? id = null)
        {
            //get the sessionTitle from the database
            //https://msdn.microsoft.com/en-us/library/system.security.principal.iidentity(v=vs.110).aspx
            string sessionId = Session.SessionID;
            SessionProfileViewModel sessionProfileVM;

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

                sessionProfileVM = new SessionProfileViewModel()
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
            //create details view
            return View(sessionProfileVM);
            //TODO: Admin needs to be presented with a list of all the users enrolled in a session
        }

        /// <summary>
        /// Redirect the admin to a view where he can edit the session upon a button click
        /// </summary>
        /// <returns>View To Help Admin Edit Sessions</returns>
        [HttpGet]
        public ActionResult Edit(int id)
        {
            EditSessionViewModel editSessionVM;
            using (WSADDbContext context = new WSADDbContext())
            {
                Session sessionDTO = context.Sessions.Find(id);
                if (sessionDTO == null)
                {
                    return Content("Invalid Session ID");
                }

                editSessionVM = new EditSessionViewModel()
                {
                    SessionId = sessionDTO.SessionId,
                    SessionTitle = sessionDTO.SessionTitle,
                    SessionDescription = sessionDTO.SessionDescription,
                    SessionPresenter = sessionDTO.SessionPresenter,
                    SessionAddress = sessionDTO.SessionAddress,
                    SessionRoom = sessionDTO.SessionRoom,
                    SessionSeatsAvailable = sessionDTO.SessionSeatsAvailable,
                };
            }
            //passing the viewModel data to the view with edit template
            return View(editSessionVM);
        }

        /// <summary>
        /// Gives the admin the ability to edit one session from the list of sessions
        /// </summary>
        /// <param name="editSessionVM"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit(EditSessionViewModel editSessionVM)
        {
            if (!ModelState.IsValid)
            {
                return View(editSessionVM);
            }

            using (WSADDbContext context = new WSADDbContext())
            {
                Session sessionDTO = context.Sessions.Find(editSessionVM.SessionId);
                if (sessionDTO == null)
                {
                    return Content("Inavid Session ID");
                }

                sessionDTO.SessionTitle = editSessionVM.SessionTitle;
                sessionDTO.SessionDescription = editSessionVM.SessionDescription;
                sessionDTO.SessionPresenter = editSessionVM.SessionPresenter;
                sessionDTO.SessionAddress = editSessionVM.SessionAddress;
                sessionDTO.SessionRoom = editSessionVM.SessionRoom;
                sessionDTO.SessionSeatsAvailable = editSessionVM.SessionSeatsAvailable;

                context.SaveChanges();
            }

            // Toast to show the session edited
            return View();
        }

        /// <summary>
        /// Gives the admin the ability to delete multiple sessions from the session collection
        /// </summary>
        /// <param name="collectionOfSessionsToDelete"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete(List<ManageSessionViewModel> collectionOfSessionsToDelete)
        {
            //Filter collection of Sessions, and seperate the isSelected items only
            var fileteredCollectionOfSessionsToDelete = collectionOfSessionsToDelete.Where(x => x.isSelected == true);

            //Get reference to the database
            using (WSADDbContext context = new WSADDbContext())
            {
                //iterate and delete each object
                foreach (var vmItems in fileteredCollectionOfSessionsToDelete)
                {
                    var dtoToDelete = context.Sessions.FirstOrDefault(row => row.SessionId == vmItems.SessionId);
                    context.Sessions.Remove(dtoToDelete);

                    //delete the rows in the sessionCart table (null pointer prevented) on user cart GET

                    var sessionCartDtoToDelete = context.SessionCartItems.FirstOrDefault(row => row.SessionId == vmItems.SessionId);
                    if (sessionCartDtoToDelete != null)
                    {
                        context.SessionCartItems.Remove(sessionCartDtoToDelete);
                    }
                }
                context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult GetEnrolledUsers(int? id = null)
        {
            //select userID from sessionCart where sessionId = id;

            //id == sessionID
            string sessionId = Session.SessionID;
            List<UsersEnrolledBySessionViewModel> enrollmentList;

            using (WSADDbContext context = new WSADDbContext())
            {
                //get a list of users according to the session id that has been passed in

                //select userID from sessionCart where sessionID = id;
                Session sessionDTO;
                int sessionID;

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

                sessionID = sessionDTO.SessionId;

                //Get enrollment users
                enrollmentList = context.SessionCartItems.Where(x => x.SessionId == sessionID)
                    .ToArray()
                    .Select(x => new UsersEnrolledBySessionViewModel(x))
                    .ToList();
            }
            return View(enrollmentList);
        }

        [HttpPost]
        public ActionResult DeleteUsersFromSession(List<UsersEnrolledBySessionViewModel> collectionOfUsersToDeleteFromSession)
        {
            IEnumerable<UsersEnrolledBySessionViewModel> filteredCollectionsOfUsersToDelete = collectionOfUsersToDeleteFromSession.Where(x => x.isSelected == true);
            bool usersDeleted = false;

            using (WSADDbContext context = new WSADDbContext())
            {
                foreach (var userItems in filteredCollectionsOfUsersToDelete)
                {
                    SessionCart dtoToDelete = context.SessionCartItems.FirstOrDefault(row => row.UserId == userItems.User.UserId);
                    
                    if (dtoToDelete != null)
                    {
                        context.SessionCartItems.Remove(dtoToDelete);
                        usersDeleted = true;
                    }
                }
                context.SaveChanges();
            }

            //TODO: Stay on same view
            if (usersDeleted)
            {
                TempData["UsersRemoved"] = "Users Removed!";
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }

        public ActionResult AddUserToSession(int sessionId, int userId)
        {
            bool userAdded = false;
            //Check for valid session and user id's
            if (sessionId <= 0 || userId <=0)
            {
                return this.HttpNotFound("Invalid Input Parameters");
            }

            using (WSADDbContext context = new WSADDbContext())
            {

                Session sessionDTO = context.Sessions.FirstOrDefault(x => x.SessionId == sessionId);
                User userDTO = context.Users.FirstOrDefault(x => x.UserId == userId);

                if (sessionDTO == null || userDTO ==null)
                {
                    return this.HttpNotFound("Invalid Input Parameters");
                }

                //Check to see if the user+session Combination already exists, if not add it

                SessionCart sessionCartDto =
                    context.SessionCartItems.FirstOrDefault(
                        row => row.UserId == userDTO.UserId && sessionDTO.SessionId == sessionId);
               
                //sessionCart DTO == null -> then add since the combination does not exist

                if (sessionCartDto == null)
                {
                    SessionCart sessionCartItemToAdd = new SessionCart()
                    {
                        UserId = userId,
                        SessionId = sessionId
                    };

                    context.SessionCartItems.Add(sessionCartItemToAdd);
                    userAdded = true;
                }
                else
                {
                    //if (!userAdded)
                    //{
                    //    TempData["NoDuplicates"] = "User Already Registered to this session";
                    //    return RedirectToAction("GetEnrolledUsers", new { sessionId });
                    //}
                }
                context.SaveChanges();
            }

            //return RedirectToAction("GetEnrolledUsers", new {sessionId});
            return RedirectToAction("Index");
        }
    }
}
