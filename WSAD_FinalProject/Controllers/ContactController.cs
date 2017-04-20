using System;
using System.Data.Entity.Infrastructure;
using System.Net.Mail;
using System.Web.Mvc;
using WSAD_FinalProject.Models.Data;
using WSAD_FinalProject.Models.ViewModels.Contact;

namespace WSAD_FinalProject.Controllers
{
    public class ContactController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.ContactReason = new SelectList(new[]
            {
                "Request Admin Account", "Forgot Password", "General Inquiry"
            });

            return View();
        }

        [HttpPost]
        public ActionResult Index(ContactUsViewModel newContact)
        {
            bool contactStatus = false;
            ViewBag.ContactReason = new SelectList(new[]
            {
                "Request Admin Account", "Forgot Password", "General Inquiry"
            });

            if (newContact == null)
            {
                ModelState.AddModelError("", "No Message Provided");
                return View();
            }

            if (string.IsNullOrWhiteSpace(newContact.ContactName) ||
               string.IsNullOrWhiteSpace(newContact.ContactEmail) ||
               string.IsNullOrWhiteSpace(newContact.ContactReason)||
               string.IsNullOrWhiteSpace(newContact.ContactDetail))
            {
                ModelState.AddModelError("", "All Fields Required!");
                return View();
            }


            MailMessage email = new MailMessage();
            email.To.Add("sumanth083@gmail.com");
            email.From = new MailAddress(newContact.ContactEmail);
            email.Subject = "Email Inquiry";
            email.Body = string.Format("Name: {0} \r\nMessage {1} \r\nEmail {2}", newContact.ContactName, newContact.ContactDetail, newContact.ContactEmail);
            email.IsBodyHtml = false;

            SmtpClient client = new SmtpClient();
            client.Host = "mail.twc.com";
            try
            {
                client.Send(email);
            }
            catch (SmtpException)
            {
                throw new SmtpException();
            }


            using (WSADDbContext context = new WSADDbContext())
            {
                Contact newContactDto = new Contact()
                {
                    ContactName = newContact.ContactName,
                    ContactEmail = newContact.ContactEmail,
                    ContactReason = newContact.ContactReason,
                    ContactDetail = newContact.ContactDetail
                };

                newContactDto = context.Contacts.Add(newContactDto);
                
                /*
                if (newContact.ContactReason.Equals("Request Admin Account"))
                {
                    //send email to the user saying account access granted
                    //need to access the user account table and change the bit field of the user
                    //need to access the edit user view model first
                }
                else if (newContact.ContactReason.Equals("Forgot Password"))
                {
                    //send the password as an email to the user
                    //need to query for the password
                }
                else
                {
                    //general inquiry
                    
                }
                */

            
                try
                {
                    context.SaveChanges();
                    contactStatus = true;
                }
                catch (DbUpdateException d)
                {
                    Console.WriteLine(d.Message);
                }
            }

            if (contactStatus)
            {
                TempData["contact"] = "Thank You- We will get in touch via Email";
                //redirect to the login page
                return View();
            }
            return View();
        }
    }
}