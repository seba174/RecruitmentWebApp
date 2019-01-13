using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using RecrutimentApp.EntityFramework;
using RecrutimentApp.Models;
using RecrutimentApp.Utilities;

namespace RecrutimentApp.Controllers
{
    public class JobApplicationController : Controller
    {
        private readonly DataContext dataContext;
        private readonly CloudBlobContainer blobContainer;

        public JobApplicationController(DataContext context, IConfiguration Configuration)
        {
            dataContext = context;

            string connectionString = Configuration.GetConnectionString("BlobStorageAccount");
            string containerName = Configuration.GetSection("BlobStorageAccountSettings").GetValue<string>("ContainerName");

            var storageAccount = CloudStorageAccount.Parse(connectionString);
            CloudBlobClient cloudBlobClient = storageAccount.CreateCloudBlobClient();
            blobContainer = cloudBlobClient.GetContainerReference(containerName);
        }

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

        [Authorize]
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
                OfferName = offer.JobTitle,
                FirstName = UserInformations.GetUserGivenName(User),
                Lastname = UserInformations.GetUserSurname(User),
                EmailAddress = UserInformations.GetUserEmail(User)
            });
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(JobApplicationWithOfferName model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            string nameToSave = Guid.NewGuid().ToString() + DateTime.Now.ToString("_yyyy-MM-dd-HH-mm-ss_") + model.File.FileName;
            CloudBlockBlob blockBlob = blobContainer.GetBlockBlobReference(nameToSave);

            using (var memoryStream = new MemoryStream())
            {
                model.File.CopyTo(memoryStream);
                byte[] file = memoryStream.ToArray();
                await blockBlob.UploadFromByteArrayAsync(file, 0, file.Length);
            }

            string url = blockBlob.Uri.AbsoluteUri;

            JobApplication jobApplication = new JobApplication
            {
                ContactAgreement = model.ContactAgreement,
                CvUrl = url,
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