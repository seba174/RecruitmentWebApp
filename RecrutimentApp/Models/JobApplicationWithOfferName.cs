
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace RecrutimentApp.Models
{
    public class JobApplicationWithOfferName : JobApplication
    {
        public JobApplicationWithOfferName() { }

        public JobApplicationWithOfferName(JobApplication jobApplication)
        {
            Id = jobApplication.Id;
            FirstName = jobApplication.FirstName;
            DateOfBirth = jobApplication.DateOfBirth;
            EmailAddress = jobApplication.EmailAddress;
            ContactAgreement = jobApplication.ContactAgreement;
            CvUrl = jobApplication.CvUrl;
            Lastname = jobApplication.Lastname;
            JobOfferId = jobApplication.JobOfferId;
            PhoneNumber = jobApplication.PhoneNumber;
        }

        public string OfferName { get; set; }

        [Required]
        [Display(Name = "Curriculum vitae")]
        [DataType(DataType.Upload)]
        public IFormFile File { get; set; }
    }
}