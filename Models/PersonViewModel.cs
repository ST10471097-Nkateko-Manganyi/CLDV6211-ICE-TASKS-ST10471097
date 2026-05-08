namespace CloudDevProj2.Models
{
    public class PersonViewModel
    {
        public int PersonId { get; set; }
        public string FirstName { get; set; }
        public string Lastname { get; set; }
        public string MiddleName { get; set; }
        public DateOnly Title { get; set; }
        public string Gender { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        //     >>>> (?) <<<< means this field is nullable     /////
        public string? ImageURL { get; set; }

        public IFormFile? ImageFile { get; set; }
    }
}
