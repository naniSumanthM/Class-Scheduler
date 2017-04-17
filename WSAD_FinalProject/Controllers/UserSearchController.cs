using System.Collections.Generic;
using System.Web.Http;
using WSAD_FinalProject.Models.Data;
using WSAD_FinalProject.Models.ViewModels.UserSearch;
using System.Linq;

namespace WSAD_FinalProject.Controllers
{
    public class UserSearchController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<UserSearchViewModel> Get(string term)
        {
            using (WSADDbContext context = new WSADDbContext())
            {
                IQueryable<User> matches;
                List<UserSearchViewModel> usVM = new List<UserSearchViewModel>();

                if (string.IsNullOrWhiteSpace(term))
                {
                    matches = context.Users.AsQueryable();
                }
                else
                {
                    matches = context.Users
                        .Where(row => row.UserFirstName.StartsWith(term));
                }

                foreach (var userDTO in matches)
                {
                    usVM.Add(new UserSearchViewModel(userDTO));
                }

                return usVM;
            }
        }

        #region Unused
        //// GET api/<controller>/5
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST api/<controller>
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT api/<controller>/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE api/<controller>/5
        //public void Delete(int id)
        //{
        //} 
        #endregion
    }
}