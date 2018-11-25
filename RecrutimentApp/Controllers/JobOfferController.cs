﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecrutimentApp.EntityFramework;
using RecrutimentApp.Models;
using RecrutimentApp.Utilities;
using static Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState;

namespace RecrutimentApp.Controllers
{
    public class JobOfferController : Controller
    {
        private readonly DataContext dataContext;

        public JobOfferController(DataContext context)
        {
            dataContext = context;
            //if (dataContext.Companies.Count() < 3)
            //{
            //    dataContext.Companies.AddRange(
            //        new Company() { Name = "Predica", HeadquaterLocation = "Warsaw" },
            //        new Company() { Name = "Microsoft", HeadquaterLocation = "Redmond" },
            //        new Company() { Name = "Github", HeadquaterLocation = "San Francisco" });
            //    dataContext.SaveChanges();
            //}

            //if (dataContext.JobOffers.Count() < 2)
            //{
            //    dataContext.JobOffers.AddRange(
            //    new JobOffer
            //    {
            //        JobTitle = "Backend developer",
            //        CompanyId = dataContext.Companies.FirstOrDefault(c => c.Name == "Predica")?.Id ?? 0,
            //        Created = DateTime.Now.AddDays(-2),
            //        Description = "Backend C# developer with intrests about IoT solutions. The main task would be building API which expose data from phisical devices. Description need to have at least 100 characters so I am adding some. In test case I reccomend you to use Lorem Impsum.",
            //        Location = "Poland",
            //        SalaryFrom = 2000,
            //        SalaryTo = 10000,
            //        ValidUntil = DateTime.Now.AddDays(20).Date
            //    },
            //    new JobOffer
            //    {
            //        JobTitle = "Frontend developer",
            //        CompanyId = dataContext.Companies.FirstOrDefault(c => c.Name == "Microsoft")?.Id ?? 0,
            //        Created = DateTime.Now.AddDays(-2),
            //        Description = "Developing Office 365 front end interface.",
            //        Location = "Poland",
            //        SalaryFrom = 2000,
            //        SalaryTo = 10000,
            //        ValidUntil = DateTime.Now.AddDays(20).Date
            //    });

            //    dataContext.SaveChanges();
            //}
        }

        public async Task<ActionResult> Index([FromQuery(Name = "search")] string searchString)
        {
            if (string.IsNullOrEmpty(searchString))
            {
                return View(await dataContext.JobOffers.Join(
                    dataContext.Companies,
                    o => o.CompanyId,
                    c => c.Id,
                    (o, c) => new JobOffer(o, c)).ToListAsync());
            }

            List<JobOffer> searchResult = await dataContext.JobOffers.Where(o => o.JobTitle.ToLower().Contains(searchString.ToLower())).ToListAsync();
            return View(searchResult);
        }

        public async Task<ActionResult> Details(int id)
        {
            JobOffer jobOffer = await dataContext.JobOffers.Where(j => j.Id == id).FirstOrDefaultAsync();
            if (jobOffer is null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            jobOffer.JobApplications = await dataContext.JobApplications.Where(ja => ja.JobOfferId == jobOffer.Id).ToListAsync();

            return View(new JobOffer(jobOffer, await dataContext.Companies.FindAsync(jobOffer.CompanyId)));
        }

        public async Task<ActionResult> Edit(int? id)
        {
            if (id is null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            JobOffer offer = await dataContext.JobOffers.FindAsync(id.Value);
            if (offer is null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            return View(new JobOffer(offer, await dataContext.Companies.FindAsync(offer.CompanyId)));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(JobOffer model)
        {
            var t = ModelState.IsValid;

            if (ModelState.GetFieldValidationState(nameof(JobOffer.JobTitle)) != Valid || ModelState.GetFieldValidationState(nameof(JobOffer.Description)) != Valid)
            {
                return View();
            }

            var offer = await dataContext.JobOffers.FindAsync(model.Id);
            offer.JobTitle = model.JobTitle;
            offer.Description = model.Description;

            await dataContext.SaveChangesAsync();

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

            dataContext.JobOffers.Remove(new JobOffer { Id = id.Value });
            await dataContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Create()
        {
            var model = new JobOfferCreateView
            {
                Companies = await dataContext.Companies.ToListAsync()
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(JobOfferCreateView model)
        {
            if (!ModelState.IsValid)
            {
                model.Companies = await dataContext.Companies.ToListAsync();
                return View(model);
            }

            JobOffer jobOffer = new JobOffer
            {
                CompanyId = model.CompanyId,
                Description = model.Description,
                JobTitle = model.JobTitle,
                Location = model.Location,
                SalaryFrom = model.SalaryFrom,
                SalaryTo = model.SalaryTo,
                ValidUntil = model.ValidUntil,
                Created = DateTime.Now
            };

            await dataContext.JobOffers.AddAsync(jobOffer);
            await dataContext.SaveChangesAsync();

            return RedirectToAction("index");
        }
    }
}