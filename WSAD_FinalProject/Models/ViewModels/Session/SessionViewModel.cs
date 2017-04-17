namespace WSAD_FinalProject.Models.ViewModels.Session
{
    public class SessionViewModel
    {
        public SessionViewModel()
        {
                
        }

        public SessionViewModel(Data.Session row)
        {
            //row.SessionID will be null if the session is deleted
            this.SessionId = row.SessionId;
            this.SessionTitle = row.SessionTitle;
            this.SessionDescription = row.SessionDescription;
            this.SessionPresenter = row.SessionPresenter;
            this.SessionAddress = row.SessionAddress;
            this.SessionRoom = row.SessionRoom;
            this.SessionSeatsAvailable = row.SessionSeatsAvailable;
        }

        public int SessionId { get; set; }
        public string SessionTitle { get; set; }
        public string SessionDescription { get; set; }
        public string SessionPresenter { get; set; }
        public string SessionAddress { get; set; }
        public string SessionRoom { get; set; }
        public int SessionSeatsAvailable { get; set; }

        public bool isSelected { get; set; }
    }
}