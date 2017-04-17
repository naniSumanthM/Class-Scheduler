using System.Linq;
using System.Security.Principal;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using WSAD_FinalProject.Models.Data;

namespace WSAD_FinalProject
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_AuthenticateRequest()
        {
            if (Context.User == null)
            {
                return;
            } //Exit if the userObj = null

            //get the current user reference
            string emailAddress = Context.User.Identity.Name;
            string[] roles = new string[1];

            using (WSADDbContext context = new WSADDbContext())
            {
                //get the user based on the email address of the current user
                User userDTO = context.Users.FirstOrDefault(row => row.UserEmailAddress == emailAddress);

                //Add Roles to the IPrinicipal Objecct
                if (userDTO != null)
                {
                    roles = context.UserRoles.Where(row => row.UserId == userDTO.UserId)
                        .Select(row => row.role.Name) //fkRole in tblRole
                        .ToArray();
                }
            }

            //Build IPrinicpal Object
            IIdentity userIdentity = new GenericIdentity(emailAddress);
            IPrincipal newUserObj = new System.Security.Principal.GenericPrincipal(userIdentity, roles);

            //Update Context.User with IPrinicpal Obj
            Context.User = newUserObj;
        }
    }
}
