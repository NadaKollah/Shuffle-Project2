using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace shuffle2.Entity
{
    [Table("User")]
    public class User
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }

        [Required]
        [Column("Name")]
        public string Name { get; set; }
        
        [Required]
        [Column("Surname")]
        public string Surname { get; set; }

        [Required]
        [Column("Email")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "Invalid Email")]
        public string Email { get; set; }
    }
}
