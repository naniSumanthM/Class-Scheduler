using System;
using WSAD_FinalProject.Models.ViewModels.Session;

namespace WSAD_FinalProject.Models.ViewModels.SessionCart
{
    public class SessionCartViewModel
    {
        public SessionCartViewModel()
        {
        }

        public SessionCartViewModel(Data.SessionCart row)
        {
            this.SessionCartId = row.SessionCartId;
            this.UserId = row.UserId;
            this.SessionId = row.SessionId;
            this.Session = new SessionViewModel(row.Session);
        }

        public int SessionCartId { get; set; }
        public int UserId { get; set; }
        public int SessionId { get; set; }
        public SessionViewModel Session { get; set; }
        public bool isSelected { get; set; }
    }
}