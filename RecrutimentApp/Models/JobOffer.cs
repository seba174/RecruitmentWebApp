﻿using System;
using System.ComponentModel.DataAnnotations;

namespace RecrutimentApp.Models
{
    public class JobOffer
    {
        public int Id { get; set; }

        [Display(Name = "Job title")]
        [Required]
        public string JobTitle { get; set; }

        public virtual Company Company { get; set; }

        public virtual int CompanyId { get; set; }

        [Display(Name = "Salary from")]
        public decimal? SalaryFrom { get; set; }

        [Display(Name = "Salary to")]
        public decimal? SalaryTo { get; set; }

        public DateTime Created { get; set; }

        public string Location { get; set; }

        [Required]
        [MinLength(100)]
        public string Description { get; set; }

        [Display(Name = "Valid until")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyy-MM-dd}")]
        public DateTime? ValidUntil { get; set; }
    }
}
