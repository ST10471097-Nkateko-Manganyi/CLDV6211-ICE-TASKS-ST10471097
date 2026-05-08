using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CloudDevProj2.Models
{
    public class Notes
    {
        [Key]

        public int NoteId { get; set; }

        [Required(ErrorMessage = "Person is required")]

        public int PersonId { get; set; }

        [Required(ErrorMessage = "Note cannot be empty")]

        [StringLength(500, ErrorMessage = "Note cannot exceed 500 characters")]

        public string NoteText { get; set; }

        [ForeignKey("PersonId")]

        public Person Person { get; set; }
    }
}
