using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WSAD_FinalProject.Models.Data
{
    [Table("tblRole")]
    public class Role
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public bool isAdmin { get; set; }

    }
}