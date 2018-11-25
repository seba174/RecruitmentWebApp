using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RecrutimentApp.Models;

namespace RecrutimentApp.Controllers
{
    public class CompanyController : Controller
    {
        private static List<Company> companies = new List<Company>
        {
            new Company() { Id = 1, Name = "Predica"},
            new Company() { Id = 2, Name = "Microsoft"},
            new Company() { Id = 3, Name = "Github"}
        };

        public IActionResult Index()
        {
            return View(companies);
        }
    }
}