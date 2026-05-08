using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CloudDevProj2.Models
{
    public class Skill
    {
        [Key]

        public int SkillId { get; set; }

        [Required(ErrorMessage = "Person is required")]

        public int PersonId { get; set; }

        [Required(ErrorMessage = "Skill name is required")]

        [StringLength(100, ErrorMessage = "Skill cannot exceed 100 characters")]

        public string SkillName { get; set; }

        [ForeignKey("PersonId")]

        public Person Person { get; set; }
    }
}
