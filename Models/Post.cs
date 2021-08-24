using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BlogZemoga.Models
{
    public class Post
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name ="Author")]
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser ApplicationUser { get; set; }

        [Required]
        [Display(Name ="Post")]
        public string Posts { get; set; }

        [Required]
        public string Status { get; set; }

        [Display(Name ="Approval Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]

        public DateTime ApprovalDate { get; set; }

        [Display(Name ="Register Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime RegisterDate { get; set; }
    }
}
