using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace back.Models.DB
{
    [Index(nameof(Email), IsUnique = true)]
    public class User
    {
        [Key]
        public int IdUser { get; set; }

        [Column("Name")]
        public string Name { get; set; }

        [Column("Email")]
        public string Email { get; set; }

        [Column("Password")]
        public string Password { get; set; }

        [Column("HashPassword")]
        public string HashPassword { get; set; }
    }
}
