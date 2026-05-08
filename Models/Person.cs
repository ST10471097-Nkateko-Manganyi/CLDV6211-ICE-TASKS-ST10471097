using System.ComponentModel.DataAnnotations;

namespace CloudDevProj2.Models

{
    public class Person
    {
        public int PersonId {  get; set; }

        [Required(ErrorMessage = "Name is required")]

        public string Name { get; set; }

        public List<Skill> Skills { get; set; } = new List<Skill>();

        public List<Notes> Notes { get; set; } = new List<Notes>();


        public string FirstName { get; set; }
        public string Lastname { get; set; }
        public string MiddleName { get; set; }

        public DateOnly Title { get; set; }
        public string Gender { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        //     >>>> (?) <<<< means this field is nullable     /////
        public string? ImageURL { get; set; }    
    }
}
