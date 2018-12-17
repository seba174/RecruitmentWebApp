using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecrutimentApp.EntityFramework;
using RecrutimentApp.Models;

namespace RecrutimentApp.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    public class SampleApiController : Controller
    {
        private readonly DataContext dataContext;

        public SampleApiController(DataContext dataContext) => this.dataContext = dataContext;

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

        [HttpGet("{id}")]
        public async Task<IEnumerable<JobApplication>> GetJobApplications(int id)
        {
            return await dataContext.JobApplications.Where(ja => ja.JobOfferId == id).ToListAsync();
        }

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