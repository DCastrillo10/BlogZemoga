using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BlogZemoga.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int PostId { get; set; }

        [ForeignKey("PostId")]
        public Post Post { get; set; }

        [Required]
        public string Comments { get; set; }

        public bool Status { get; set; }

        [Display(Name ="Register Date")]
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:dd-MM-yyyy HH:mm}")]
        public DateTime RegisterDate { get; set; }

    }
}
