using System;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using WSAD_FinalProject.Models.Data;
using WSAD_FinalProject.Models.ViewModels.Account;

namespace WSAD_FinalProject.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return this.RedirectToAction("Login");
        }

        /// <summary>
        /// Supports New User Creation
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// POST - Commits the new user data to the database
        /// </summary>
        /// <param name="newUser"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(CreateUserViewModel newUser)
        {
            bool accountCreated = false;

            //ensures all the [Required] data is populated
            if (!ModelState.IsValid)
            {
                return View(newUser);
            }

            //Tally the password and the confirm password
            if (!newUser.UserPassword.Equals(newUser.UserPasswordConfirm))
            {
                ModelState.AddModelError("", "Passwords do not match!");
                return View(newUser);
            }

            using (WSADDbContext context = new WSADDbContext())
            {
                //Check to see if the email id already exists in the database
                if (context.Users.Any(row => row.UserEmailAddress.Equals(newUser.UserEmailAddress)))
                {
                    ModelState.AddModelError("", "An Account With " + newUser.UserEmailAddress + " Already Exists");
                    newUser.UserEmailAddress = " ";
                    return View(newUser);
                }

                //User DTO
                User newUserDto = new User()
                {
                    //viewModel.property = newUser.FirstName
                    UserFirstName = newUser.UserFirstName,
                    UserLastName = newUser.UserLastName,
                    UserEmailAddress = newUser.UserEmailAddress,
                    UserPassword = newUser.UserPassword,
                    UserCompany = newUser.UserCompany,
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
                //redirect to the login page
                return RedirectToAction("login");
            }
            return View(newUser);
        }

        /// <summary>
        /// GET - Login In Page
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        /// <summary>
        /// POST - User Authentication
        /// </summary>
        [HttpPost]
        public ActionResult Login(LoginUserViewModel loginUser)
        {
            if (loginUser == null)
            {
                ModelState.AddModelError("", "Login Required");
                return View();
            }

            if (string.IsNullOrWhiteSpace(loginUser.UserEmailAddress))
            {
                ModelState.AddModelError("", "Email Required");
                return View();
            }

            if (string.IsNullOrWhiteSpace(loginUser.UserPassword))
            {
                ModelState.AddModelError("", "Password Required");
                return View();
            }

            //Query DB for the set of username and password
            bool isValid = false;
            using (WSADDbContext context = new WSADDbContext())
            {
                //Query for the username and password
                if (context.Users.Any(row => row.UserEmailAddress.Equals(loginUser.UserEmailAddress) && row.UserPassword.Equals(loginUser.UserPassword)))
                {
                    isValid = true;
                }
            }

            //login failed
            if (!isValid)
            {
                ModelState.AddModelError("", "Inavlid Credentials!");
                return View();
            }
            else
            {
                //Validation passed - return user to home page
                FormsAuthentication.SetAuthCookie(loginUser.UserEmailAddress, loginUser.UserRememberMe); //saves the authentication info even after reboot of PC
                return Redirect(FormsAuthentication.GetRedirectUrl(loginUser.UserEmailAddress, loginUser.UserRememberMe)); //returns the user to the home page   
            }
        }

        /// <summary>
        /// Redirects user to login page
        /// </summary>
        /// <returns></returns>
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }

        /// <summary>
        /// Captures the logged in user
        /// </summary>
        /// <returns></returns>
        public ActionResult UserNavPartial()
        {
            //capture logged in user
            string emailAddress = this.User.Identity.Name; //user name that user uses to login
            UserNavPartialViewModel userNavVM;

            //get user information from the database
            using (WSADDbContext context = new WSADDbContext())
            {
                //query for the user
                User userDTO = context.Users.FirstOrDefault(x => x.UserEmailAddress == emailAddress);

                if (userDTO == null)
                {
                    return Content("");
                }

                //build the usernavpartialviewmodel   
                userNavVM = new UserNavPartialViewModel()
                {
                    UserFirstName = userDTO.UserFirstName,
                    UserLastName = userDTO.UserLastName,
                    UserId = userDTO.UserId
                };

            }
            //send the view model to the partial view
            return PartialView(userNavVM);
        }

        /// <summary>
        /// Shows the user profile view
        /// </summary>
        /// <returns></returns>
        public ActionResult UserProfile(int? id=null)
        {
            string emailAddress = User.Identity.Name;
            UserProfileViewModel profileVM;

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

                if (userDTO==null)
                {
                    return Content("Inavid Username");
                }

                profileVM = new UserProfileViewModel()
                {
                    UserId = userDTO.UserId,
                    UserFirstName = userDTO.UserFirstName,
                    UserLastName = userDTO.UserLastName,
                    UserEmailAddress = userDTO.UserEmailAddress,
                    UserCompany = userDTO.UserCompany,
                    UserIsAdmin = userDTO.UserIsAdmin,
                    UserDateCreated = userDTO.UserDateCreated
                };
            }
            //details view model
            return View(profileVM);
        }

        /// <summary>
        /// Shows the view where user can alter profile
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Account/edit</returns>
        [HttpGet]
        public ActionResult Edit(int id)
        {
            EditViewModel editVM;
            using (WSADDbContext context = new WSADDbContext())
            {
                User userDTO = context.Users.Find(id);

                if (userDTO == null)
                {
                    return Content("Invalid ID");
                }

                editVM = new EditViewModel()
                {
                    UserId = userDTO.UserId,
                    UserFirstName = userDTO.UserFirstName,
                    UserLastName = userDTO.UserLastName,
                    UserEmailAddress = userDTO.UserEmailAddress,
                    UserCompany = userDTO.UserCompany
                };
            }

            return View(editVM);
        }

        /// <summary>
        /// Saves profile alteration to db
        /// </summary>
        /// <param name="editVM"></param>
        /// <returns>userprofile</returns>
        [HttpPost]
        public ActionResult Edit(EditViewModel editVM)
        {

            bool passwordChanged = false;
            bool emailChanged = false;

            if (!ModelState.IsValid)
            {
                return View(editVM);
            }


            if (!string.IsNullOrWhiteSpace(editVM.UserPassword)) // we assume user changes password
            {
                if (!editVM.UserPassword.Equals(editVM.UserConfirmPassword))
                {
                    ModelState.AddModelError("", "Passwords must match");
                    return View(editVM);
                }
                else
                {
                    passwordChanged = true;
                }

            }

            using (WSADDbContext context = new WSADDbContext())
            {
                User userDTO = context.Users.Find(editVM.UserId);
                if (userDTO == null)
                {
                    return Content("Inavid User ID");
                }

                if (!userDTO.UserEmailAddress.Equals(editVM.UserEmailAddress))
                {
                    userDTO.UserEmailAddress = editVM.UserEmailAddress;
                    emailChanged = true;
                }

                userDTO.UserFirstName = editVM.UserFirstName;
                userDTO.UserLastName = editVM.UserLastName;
                userDTO.UserEmailAddress = editVM.UserEmailAddress;
                userDTO.UserCompany = editVM.UserCompany;
                userDTO.UserDateModified = DateTime.Now;

                if (passwordChanged)
                {
                    userDTO.UserPassword = editVM.UserPassword;
                }

                context.SaveChanges();
            }

            if (emailChanged || passwordChanged)
            {
                TempData["LogoutMessage"] = "Username & Password CHANGED. [Please login with updated credentials]";
                return RedirectToAction("Logout");
            }
            else
            {
                return RedirectToAction("UserProfile");
            }
        }
    }
}