using System.Collections.Generic;
using WSAD_FinalProject.Models.ViewModels.SessionCart;

namespace WSAD_FinalProject.Models.ViewModels.Session
{
    public class SessionIndexViewModel
    {
        //list of all the sessions
        public List<SessionViewModel> Sessions { get; set; }

        //list of all the user registered sessions
        public List<SessionCartViewModel> RegisteredSessions { get; set; }

    }
}