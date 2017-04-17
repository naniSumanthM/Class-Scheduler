using System.ComponentModel.DataAnnotations;

namespace WSAD_FinalProject.Areas.Admin.Models.ViewModels.ManageSession
{
    public class CreateSessionViewModel
    {
        [Required]
        public string SessionTitle { get; set; }

        [Required]
        public string SessionDescription { get; set; }

        [Required]
        public string SessionPresenter { get; set; }

        [Required]
        public string SessionAddress { get; set; }

        [Required]
        public string SessionRoom { get; set; }

        [Required]
        public int SessionSeatsAvailable { get; set; }
    }
}