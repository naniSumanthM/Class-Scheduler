﻿using System;

namespace WSAD_FinalProject.Areas.Admin.Models.ViewModels.ManageSession
{
    public class SessionProfileViewModel
    {
        public int SessionId { get; set; }
        public string SessionTitle { get; set; }
        public string SessionDescription { get; set; }
        public string SessionPresenter { get; set; }
        public string SessionAddress { get; set; }
        public string SessionRoom { get; set; }
        public int SessionSeatsAvailable { get; set; }
        public DateTime SessionDateCreated { get; set; }
        public DateTime SessionDateModified { get; set; }
    }
}