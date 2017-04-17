using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WSAD_FinalProject.Models.Data
{

    [Table("tblUser")]
    public class User
    {
        [Key]
        public int UserId { get; set; }
        public string UserFirstName { get; set; }
        public string UserLastName { get; set; }
        public string UserEmailAddress { get; set; }
        public string UserPassword { get; set; }
        public string UserCompany { get; set; }
        public bool UserIsActive { get; set; }
        public bool UserIsAdmin { get; set; }
        public DateTime UserDateCreated { get; set; }
        public DateTime UserDateModified { get; set; }
    }
}