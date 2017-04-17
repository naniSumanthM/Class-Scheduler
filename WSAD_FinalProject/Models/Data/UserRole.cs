using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WSAD_FinalProject.Models.Data
{
    [Table("tblUserRole")]
    public class UserRole
    {
        //tells Entity Framework about our composite primary key

        [Key(), Column(Order = 0)]
        public int UserId { get; set; }
        [Key(), Column(Order=1)]
        public int RoleId { get; set; }

        //Many To Many 
        [ForeignKey("UserId")]
        public virtual User user { get; set; }

        [ForeignKey("RoleId")]
        public virtual Role role { get; set; }
    }
}