using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RecrutimentApp.Models;
using RecrutimentApp.Utilities;

namespace RecrutimentApp.Controllers
{
    public class JobOfferController : Controller
    {
        private static List<Company> companies = new List<Company>
        {
            new Company() { Id = 1, Name = "Predica"},
            new Company() { Id = 2, Name = "Microsoft"},
            new Company() { Id = 3, Name = "Github"}
        };

        private static List<JobOffer> jobOffers = new List<JobOffer>()
        {
            new JobOffer
            {
                Id = 1,
                JobTitle = "Backend developer",
                Company = companies.FirstOrDefault(c => c.Name == "Predica"),
                Created = DateTime.Now.AddDays(-2),
                Description = "Backend C# developer with intrests about IoT solutions.",
                Location = "Poland",
                SalaryFrom = 2000,
                SalaryTo = 10000,
                ValidUntil = DateTime.Now.AddDays(20)
            },
            new JobOffer
            {
                Id = 2,
                JobTitle = "Frontend developer",
                Company = companies.FirstOrDefault(c => c.Name == "Microsoft"),
                Created = DateTime.Now.AddDays(-2),
                Description = "Developing Office 365 front end interface.",
                Location = "Poland",
                SalaryFrom = 2000,
                SalaryTo = 10000,
                ValidUntil = DateTime.Now.AddDays(20)
            }
        };

        public IActionResult Index([FromQuery(Name = "search")] string searchString)
        {
            if (string.IsNullOrEmpty(searchString))
            {
                return View(jobOffers);
            }

            List<JobOffer> searchResult = jobOffers.FindAll(o => o.JobTitle.Contains(searchString));
            return View(searchResult);
        }

        public IActionResult Details(int id)
        {
            JobOffer jobOffer = jobOffers.Where(j => j.Id == id).FirstOrDefault();
            if (jobOffer is null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            return View(jobOffer);
        }

        public ActionResult Edit(int? id)
        {
            if (id is null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var offer = jobOffers.Find(o => o.Id == id);
            if (offer is null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            return View(offer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(JobOffer model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var offer = jobOffers.Find(o => o.Id == model.Id);
            offer.JobTitle = model.JobTitle;
            offer.Description = model.Description;

            return RedirectToAction("Details", new { id = model.Id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id is null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            jobOffers.RemoveAll(j => j.Id == id);
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Create()
        {
            var model = new JobOfferCreateView
            {
                Companies = companies
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(JobOfferCreateView model)
        {
            if (!ModelState.IsValid)
            {
                model.Companies = companies;
                return View(model);
            }

            var id = jobOffers.Max(j => j.Id) + 1;
            jobOffers.Add(new JobOffer
            {
                Id = id,
                CompanyId = model.CompanyId,
                Company = companies.FirstOrDefault(c => c.Id == model.CompanyId),
                Description = model.Description,
                JobTitle = model.JobTitle,
                Location = model.Location,
                SalaryFrom = model.SalaryFrom,
                SalaryTo = model.SalaryTo,
                ValidUntil = model.ValidUntil,
                Created = DateTime.Now
            });
            return RedirectToAction("index");
        }
    }
}