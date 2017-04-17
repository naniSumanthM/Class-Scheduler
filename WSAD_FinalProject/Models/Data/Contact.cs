using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WSAD_FinalProject.Models.Data
{
    [Table("tblContact")]
    public class Contact
    {
        [Key]
        public int ContactId { get; set; } 
        public string ContactName { get; set; }
        public string ContactEmail { get; set; }
        public string ContactReason { get; set; }
        public string ContactDetail { get; set; }
    }
}