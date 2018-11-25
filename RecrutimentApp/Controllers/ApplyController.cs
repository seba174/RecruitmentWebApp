using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RecrutimentApp.EntityFramework;
using RecrutimentApp.Models;
using RecrutimentApp.Utilities;

namespace RecrutimentApp.Controllers
{
    public class ApplyController : Controller
    {
        private readonly DataContext dataContext;

        public ApplyController(DataContext context) => dataContext = context;

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

            return View(new JobApplicationCreateModel()
            {
                JobOfferId = offerId.Value,
                OfferName = offer.JobTitle
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(JobApplicationCreateModel model)
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