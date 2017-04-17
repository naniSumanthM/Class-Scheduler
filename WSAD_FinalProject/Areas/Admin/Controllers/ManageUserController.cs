using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web.Mvc;
using WSAD_FinalProject.Areas.Admin.Models.ViewModels.ManageUser;
using WSAD_FinalProject.Models.Data;

namespace WSAD_FinalProject.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ManageUserController : Controller
    {
        /// <summary>
        /// Raw Dump of all the users in the system
        /// </summary>
        [HttpGet]
        public ActionResult Index()
        {
            List<ManageUserViewModel> collectionOfUserVM = new List<ManageUserViewModel>();
            //setup db context
            using (WSADDbContext context = new WSADDbContext())
            {
                //retreive all users from db
                var dbUsers = context.Users;

                //move users into ViewModel obj
                foreach (var userDTO in dbUsers)
                {
                    collectionOfUserVM.Add(new ManageUserViewModel(userDTO));
                }
            }
            //send ViewModel Collecton to the View
            return View(collectionOfUserVM);
        }

        /// <summary>
        /// Returns a detailed view of the user profile, so user fields are omitted from the table for an eye friendly view
        /// </summary>
        [HttpGet]
        public ActionResult UserProfile(int? id = null)
        {
            string emailAddress = User.Identity.Name;
            UserProfileAdminViewModel profileVM;

            using (WSADDbContext context = new WSADDbContext())
            {
                User userDTO;

                if (id.HasValue)
                {
                    userDTO = context.Users.Find(id.Value);
                }
                else
                {
                    userDTO = context.Users.FirstOrDefault(row => row.UserEmailAddress == emailAddress);
                }

                if (userDTO == null)
                {
                    return Content("Inavid Username");
                }

                profileVM = new UserProfileAdminViewModel()
                {
                    UserId = userDTO.UserId,
                    UserFirstName = userDTO.UserFirstName,
                    UserLastName = userDTO.UserLastName,
                    UserEmailAddress = userDTO.UserEmailAddress,
                    UserCompany = userDTO.UserCompany,
                    UserIsAdmin = userDTO.UserIsAdmin,
                    UserDateCreated = userDTO.UserDateCreated,
                    UserDateModified = userDTO.UserDateModified,
                    UserIsActive = userDTO.UserIsActive
                };
            }
            return View(profileVM);
        }

        /// <summary>
        /// Renders a view, with a new user registration form
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AddUser()
        {
            return View();
        }

        /// <summary>
        /// Admin has the ability to add users to the system
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddUser(AddUserAdminViewModel newAdminCreatedUser)
        {
            bool accountCreated = false;

            //ensures all the [Required] data is populated
            if (!ModelState.IsValid)
            {
                return View(newAdminCreatedUser);
            }

            //Tally the password and the confirm password
            if (!newAdminCreatedUser.UserPassword.Equals(newAdminCreatedUser.UserPasswordConfirm))
            {
                ModelState.AddModelError("", "Passwords do not match!");
                return View(newAdminCreatedUser);
            }

            using (WSADDbContext context = new WSADDbContext())
            {
                //Check to see if the email id already exists in the database
                if (context.Users.Any(row => row.UserEmailAddress.Equals(newAdminCreatedUser.UserEmailAddress)))
                {
                    ModelState.AddModelError("", "An Account With " + newAdminCreatedUser.UserEmailAddress + " Already Exists");
                    newAdminCreatedUser.UserEmailAddress = " ";
                    return View(newAdminCreatedUser);
                }

                //User DTO
                User newUserDto = new User()
                {
                    //viewModel.property = newUser.FirstName
                    UserFirstName = newAdminCreatedUser.UserFirstName,
                    UserLastName = newAdminCreatedUser.UserLastName,
                    UserEmailAddress = newAdminCreatedUser.UserEmailAddress,
                    UserPassword = newAdminCreatedUser.UserPassword,
                    UserCompany = newAdminCreatedUser.UserCompany,
                    UserIsActive = true,
                    UserIsAdmin = false,
                    UserDateCreated = DateTime.Now,
                    UserDateModified = DateTime.Now
                };

                //add it to the db context
                newUserDto = context.Users.Add(newUserDto);

                //Save & commit changes to db
                try
                {
                    context.SaveChanges();
                    accountCreated = true;
                }
                catch (DbUpdateException d)
                {
                    Console.WriteLine(d.Message);
                }
            }

            if (accountCreated)
            {
                TempData["AccountCreatedMessage"] = "Account Created Sucessfully";
            }
            return View(newAdminCreatedUser);
        }

        /// <summary>
        /// Renders a View, allowing admin to edit a user profile
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EditUser(int id)
        {
            EditUserAdminViewModel editUserAdminViewModel;
            using (WSADDbContext context = new WSADDbContext())
            {
                User userDTO = context.Users.Find(id);

                if (userDTO == null)
                {
                    return Content("Invalid ID");
                }

                editUserAdminViewModel = new EditUserAdminViewModel()
                {
                    UserId = userDTO.UserId,
                    UserFirstName = userDTO.UserFirstName,
                    UserLastName = userDTO.UserLastName,
                    UserEmailAddress = userDTO.UserEmailAddress,
                    UserCompany = userDTO.UserCompany,
                    UserIsAdmin = userDTO.UserIsAdmin,
                    UserIsActive = userDTO.UserIsActive
                };
            }
            return View(editUserAdminViewModel);
        }

        /// <summary>
        /// Admin has the ability to edit a user
        /// This should solve the admin access, rather than automating a user from gaining admin access programatically
        /// </summary>
        [HttpPost]
        public ActionResult EditUser(EditUserAdminViewModel editUserAdminViewModel)
        {
            bool passwordChanged = false;
            bool emailChanged = false;

            if (!ModelState.IsValid)
            {
                return View(editUserAdminViewModel);
            }

            //admin resets the password
            if (!string.IsNullOrWhiteSpace(editUserAdminViewModel.UserPassword))
            {
                if (!editUserAdminViewModel.UserPassword.Equals(editUserAdminViewModel.UserConfirmPassword))
                {
                    ModelState.AddModelError("", "Passwords must match");
                    return View(editUserAdminViewModel);
                }
                else
                {
                    passwordChanged = true;
                }
            }

            using (WSADDbContext context = new WSADDbContext())
            {
                User userDTO = context.Users.Find(editUserAdminViewModel.UserId);
                if (userDTO == null)
                {
                    return Content("Inavid User ID");
                }

                if (!userDTO.UserEmailAddress.Equals(editUserAdminViewModel.UserEmailAddress))
                {
                    userDTO.UserEmailAddress = editUserAdminViewModel.UserEmailAddress;
                    emailChanged = true;
                }

                userDTO.UserFirstName = editUserAdminViewModel.UserFirstName;
                userDTO.UserLastName = editUserAdminViewModel.UserLastName;
                userDTO.UserEmailAddress = editUserAdminViewModel.UserEmailAddress;
                userDTO.UserCompany = editUserAdminViewModel.UserCompany;
                userDTO.UserIsActive = editUserAdminViewModel.UserIsActive;
                userDTO.UserIsAdmin = editUserAdminViewModel.UserIsAdmin;
                userDTO.UserDateModified = DateTime.Now;

                if (passwordChanged)
                {
                    userDTO.UserPassword = editUserAdminViewModel.UserPassword;
                }

                context.SaveChanges();
            }

            if (emailChanged || passwordChanged)
            {
                TempData["AdminChange"] = "User Credentials Changed";
            }
            return View(editUserAdminViewModel);
        }

        /// <summary>
        /// Admin Can Peek at sessions a user is enrolled to
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetEnrolledSessions(int id)
        {
            //id == UserId

            List<SessionsEnrolledByUserViewModel> enrollmentList;

            using (WSADDbContext context = new WSADDbContext())
            {
                User userDTO = context.Users.Find(id);

                if (userDTO == null)
                {
                    return Content("Invalid ID");
                }

                //SELECT * FROM SesionCart where UserID = x
                int userId = context.Users
                    .Where(x => x.UserEmailAddress == userDTO.UserEmailAddress)
                    .Select(x => x.UserId)
                    .FirstOrDefault();

                //Get enrollment sessions
                enrollmentList = context.SessionCartItems.Where(x => x.UserId == userId)
                    .ToArray()
                    .Select(x => new SessionsEnrolledByUserViewModel(x))
                    .ToList();
            }
            return View(enrollmentList);
        }
    }
}