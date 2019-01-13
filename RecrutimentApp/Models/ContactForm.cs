using System.ComponentModel.DataAnnotations;

namespace RecrutimentApp.Models
{
    public class ContactForm
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email address")]
        public string EmailAddress { get; set; }

        [Required]
        [MaxLength(250)]
        public string Description { get; set; }
    }
}
