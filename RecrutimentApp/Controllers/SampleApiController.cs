using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecrutimentApp.EntityFramework;
using RecrutimentApp.Models;

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
        /// Gets job offers which title contains given string
        /// </summary>
        /// <remarks>
        /// If searchString is null or empty, all job offers are returned. 
        /// </remarks>
        /// <param name="searchString">Search string which will be used to filter job offers</param>
        /// <returns>Job offers which title contains given string</returns>
        [HttpGet]
        [HttpGet("{searchString}")]
        public async Task<IEnumerable<JobOffer>> GetJobOffers(string searchString)
        {
            if (string.IsNullOrEmpty(searchString))
            {
                return await dataContext.JobOffers.Join(
                    dataContext.Companies,
                    o => o.CompanyId,
                    c => c.Id,
                    (o, c) => new JobOffer(o, c)).ToListAsync();
            }

            return await dataContext.JobOffers.Where(o => o.JobTitle.ToLower().Contains(searchString.ToLower())).Join(
                dataContext.Companies,
                o => o.CompanyId,
                c => c.Id,
                (o, c) => new JobOffer(o, c)).ToListAsync();
        }

        /// <summary>
        /// Gets job applications which are associated with job offer which id is passed
        /// </summary>
        /// <remarks>
        /// If no offer with given id is found, empty list is returned.
        /// </remarks>
        /// <param name="id">Id of job offer</param>
        /// <returns>Job applications which are associated with job offer which id is passed</returns>
        [HttpGet("{id}")]
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
        [HttpGet("{searchString}")]
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