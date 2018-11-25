using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RecrutimentApp.EntityFramework;
using RecrutimentApp.Models;
using RecrutimentApp.Utilities;

namespace RecrutimentApp.Controllers
{
    public class JobApplicationController : Controller
    {
        private readonly DataContext dataContext;

        public JobApplicationController(DataContext context) => dataContext = context;

        public async Task<ActionResult> Details(int id)
        {
            JobApplication jobApplication = await dataContext.JobApplications.FindAsync(id);
            if (jobApplication == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            JobOffer offer = await dataContext.JobOffers.FindAsync(jobApplication.JobOfferId);

            return View(new JobApplicationWithOfferName(jobApplication)
            {
                OfferName = offer.JobTitle
            });
        }

        public async Task<ActionResult> Create(int? offerId)
        {
            if (offerId is null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            JobOffer offer = await dataContext.JobOffers.FindAsync(offerId.Value);
            if (offer == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            return View(new JobApplicationWithOfferName()
            {
                JobOfferId = offerId.Value,
                OfferName = offer.JobTitle
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(JobApplicationWithOfferName model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            JobApplication jobApplication = new JobApplication
            {
                ContactAgreement = model.ContactAgreement,
                CvUrl = model.CvUrl,
                DateOfBirth = model.DateOfBirth,
                EmailAddress = model.EmailAddress,
                FirstName = model.FirstName,
                JobOfferId = model.JobOfferId,
                Lastname = model.Lastname,
                PhoneNumber = model.PhoneNumber
            };

            dataContext.JobApplications.Add(jobApplication);
            await dataContext.SaveChangesAsync();

            return RedirectToAction("Details", "JobOffer", new { id = model.JobOfferId });
        }
    }
}