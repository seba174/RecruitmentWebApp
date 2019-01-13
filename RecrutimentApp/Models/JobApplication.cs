using System;
using System.ComponentModel.DataAnnotations;
using RecrutimentApp.Utilities;

namespace RecrutimentApp.Models
{
    public class JobApplication
    {
        public int Id { get; set; }

        public virtual int JobOfferId { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string Lastname { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email address")]
        public string EmailAddress { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Adult]
        [Display(Name = "Date of birth")]
        public DateTime? DateOfBirth { get; set; }

        public bool ContactAgreement { get; set; }

        [DataType(DataType.Url)]
        [Display(Name = "Curriculum vitae")]
        public string CvUrl { get; set; }
    }
}
