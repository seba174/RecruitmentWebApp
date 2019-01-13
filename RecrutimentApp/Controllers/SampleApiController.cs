using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecrutimentApp.EntityFramework;
using RecrutimentApp.Models;
using RecrutimentApp.Utilities;

namespace RecrutimentApp.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    public class SampleApiController : Controller
    {
        private readonly DataContext dataContext;

        public SampleApiController(DataContext dataContext)
        {
            this.dataContext = dataContext ?? throw new ArgumentNullException(nameof(dataContext));
        }

        /// <summary>
        /// Gets job offers which title contains given string (provided page)
        /// </summary>
        /// <remarks>
        /// If searchString is null or empty, all job offers are returned. 
        /// </remarks>
        /// <param name="searchString">Search string which will be used to filter job offers</param>
        /// <param name="pageNo">Page number</param>
        /// <returns>Job offers which title contains given string</returns>
        [HttpGet]
        public async Task<JobOfferPagingViewModel> GetJobOffers(string searchString, int pageNo = 1)
        {
            int totalRecord, pageSize = 10;

            IEnumerable<JobOffer> offers = null;
            if (string.IsNullOrEmpty(searchString))
            {
                totalRecord = dataContext.JobOffers.Count();
                offers = await dataContext.JobOffers
                .Join(
                    dataContext.Companies,
                    o => o.CompanyId,
                    c => c.Id,
                    (o, c) => new JobOffer(o, c))
                .OrderBy(o => o.JobTitle)
                .Skip((pageNo - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            }
            else
            {
                totalRecord = dataContext.JobOffers.Where(o => o.JobTitle.ToLower().Contains(searchString.ToLower())).Count();
                offers = await dataContext.JobOffers.Where(o => o.JobTitle.ToLower().Contains(searchString.ToLower()))
                .Join(
                    dataContext.Companies,
                    o => o.CompanyId,
                    c => c.Id,
                    (o, c) => new JobOffer(o, c))
                .OrderBy(o => o.JobTitle)
                .Skip((pageNo - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            }

            int totalPage = (totalRecord / pageSize) + ((totalRecord % pageSize) > 0 ? 1 : 0);

            return new JobOfferPagingViewModel
            {
                JobOffers = offers,
                TotalPage = totalPage
            };
        }

        /// <summary>
        /// Gets job applications which are associated with job offer which id is passed
        /// </summary>
        /// <remarks>
        /// If no offer with given id is found, empty list is returned.
        /// </remarks>
        /// <param name="id">Id of job offer</param>
        /// <returns>Job applications which are associated with job offer which id is passed</returns>
        [HttpGet]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IEnumerable<JobApplication>> GetJobApplications(int id)
        {
            return await dataContext.JobApplications.Where(ja => ja.JobOfferId == id).ToListAsync();
        }

        /// <summary>
        /// Gets companies which name contains given string
        /// </summary>
        /// <remarks>
        /// If searchString is null or empty, all companies are returned. 
        /// </remarks>
        /// <param name="searchString">Search string which will be used to filter companies</param>
        /// <returns>Companies which name contains given string</returns>
        [HttpGet]
        public async Task<IEnumerable<Company>> GetCompanies(string searchString)
        {
            if (string.IsNullOrEmpty(searchString))
            {
                return await dataContext.Companies.ToListAsync();
            }

            return await dataContext.Companies.Where(o => o.Name.ToLower().Contains(searchString.ToLower())).ToListAsync();
        }
    }
}