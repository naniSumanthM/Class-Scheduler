﻿using System;

namespace WSAD_FinalProject.Areas.Admin.Models.ViewModels.ManageSession
{
    public class ManageSessionViewModel
    {
        public ManageSessionViewModel() { }

        public ManageSessionViewModel(WSAD_FinalProject.Models.Data.Session sessionDto)
        {
            SessionId = sessionDto.SessionId;
            SessionTitle = sessionDto.SessionTitle;
            SessionDescription = sessionDto.SessionDescription;
            SessionPresenter = sessionDto.SessionPresenter;
            SessionAddress = sessionDto.SessionAddress;
            SessionRoom = sessionDto.SessionRoom;
            SessionSeatsAvailable = sessionDto.SessionSeatsAvailable;
            SessionDateCreated = sessionDto.SessionDateCreated;
            SessionDateModified = sessionDto.SessionDateModified;
        }

        public int SessionId { get; set; }
        public string SessionTitle { get; set; }
        public string SessionDescription { get; set; }
        public string SessionPresenter { get; set; }
        public string SessionAddress { get; set; }
        public string SessionRoom { get; set; }
        public int SessionSeatsAvailable { get; set; }
        public DateTime SessionDateCreated { get; set; }
        public DateTime SessionDateModified { get; set; }

        //front-end evaluation
        public bool isSelected { get; set; }

    }
}