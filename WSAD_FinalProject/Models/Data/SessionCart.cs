using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WSAD_FinalProject.Models.Data
{
    [Table("tblSessionCart")]
    public class SessionCart
    {
        public SessionCart()
        {

        }

        [Key]
        public int SessionCartId { get; set; }
        public int UserId { get; set; }
        public int SessionId { get; set; }

        [ForeignKey("SessionId")]
        public virtual Session Session { get; set; }

        [ForeignKey("UserId")]
        public virtual User Customer { get; set; }
    }
}