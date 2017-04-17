using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WSAD_FinalProject.Models.Data
{
    [Table("tblSession")]

    public class Session
    {
        [Key]
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