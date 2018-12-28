
using System;
using System.ComponentModel.DataAnnotations;

namespace RecrutimentApp.Models
{
    public class Company
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Headquater location")]
        public string HeadquaterLocation { get; set; }

        [Required]
        [DataType(DataType.Url)]
        [Display(Name = "Website")]
        public string WebSiteUrl { get; set; }
    }
}
