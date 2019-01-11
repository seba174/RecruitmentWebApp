using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecrutimentApp.EntityFramework;
using RecrutimentApp.Models;
using RecrutimentApp.Utilities;
using static Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState;

namespace RecrutimentApp.Controllers
{
    [Authorize]
    public class JobOfferController : Controller
    {
        private readonly DataContext dataContext;

        public JobOfferController(DataContext context)
        {
            dataContext = context;
        }

        public ActionResult Index()
        {
            return View();
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

        [Authorize(Roles = Roles.Admin)]
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
        [Authorize(Roles = Roles.Admin)]
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

            return RedirectToAction(nameof(Details), new { id = model.Id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Roles.Admin)]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id is null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            dataContext.JobOffers.Remove(new JobOffer { Id = id.Value });
            await dataContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = Roles.Admin)]
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
        [Authorize(Roles = Roles.Admin)]
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

            return RedirectToAction(nameof(Index));
        }
    }
}