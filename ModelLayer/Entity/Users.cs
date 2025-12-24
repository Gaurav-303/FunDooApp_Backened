using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.Entity
{
    [Table("Users")]
    public class Users
    {   
       
            [Key]
            public int UserId { get; set; }

            [Required]
            [MaxLength(100)]
            public string FirstName { get; set; }

            [Required]
            [MaxLength(100)]
            public string LastName { get; set; }

            [Required]
            [EmailAddress]
            [MaxLength(255)]
            public string Email { get; set; }

            [Required]
            [MaxLength(255)]
            public string Password { get; set; }

            [DataType(DataType.DateTime)]
            public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

            [DataType(DataType.DateTime)]
            public DateTime ChangedAt { get; set; } = DateTime.UtcNow;


        
    }
}
