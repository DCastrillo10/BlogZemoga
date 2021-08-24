using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BlogZemoga.Models
{
    public class ApplicationUser: IdentityUser
    {
        [Required]
        public string Names { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public bool Status { get; set; }

        [Required]
        public DateTime RegisterDate { get; set; }

        [NotMapped]
        public string Role { get; set; }

    }
}
