using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RecrutimentApp.Utilities;

namespace RecrutimentApp.Models
{
    public class JobOffer
    {
        public JobOffer()
        {
        }

        public JobOffer(JobOffer other, Company company)
        {
            Id = other.Id;
            JobTitle = other.JobTitle;
            Company = company;
            CompanyId = company.Id;
            SalaryFrom = other.SalaryFrom;
            SalaryTo = other.SalaryTo;
            Created = other.Created;
            Location = other.Location;
            Description = other.Description;
            ValidUntil = other.ValidUntil;
            JobApplications = other.JobApplications;
        }

        public int Id { get; set; }

        [Display(Name = "Job title")]
        [Required]
        public string JobTitle { get; set; }

        public virtual Company Company { get; set; }

        public virtual int CompanyId { get; set; }

        [Display(Name = "Salary from")]
        [GreaterThanZero]
        [MoneyNotGreaterThan(nameof(SalaryTo))]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? SalaryFrom { get; set; }

        [Display(Name = "Salary to")]
        [GreaterThanZero]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? SalaryTo { get; set; }

        public DateTime Created { get; set; }

        [Required]
        public string Location { get; set; }

        [Required]
        [MinLength(100)]
        public string Description { get; set; }

        [Display(Name = "Valid until")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-DD}")]
        [NotPastDate]
        public DateTime? ValidUntil { get; set; }

        public List<JobApplication> JobApplications { get; set; } = new List<JobApplication>();
    }
}
