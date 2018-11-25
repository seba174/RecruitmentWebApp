using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecrutimentApp.EntityFramework;
using RecrutimentApp.Models;
using RecrutimentApp.Utilities;

namespace RecrutimentApp.Controllers
{
    public class CompanyController : Controller
    {
        private readonly DataContext dataContext;

        public CompanyController(DataContext context)
        {
            dataContext = context;
        }

        public async Task<ActionResult> Index([FromQuery(Name = "search")] string searchString)
        {
            if (string.IsNullOrEmpty(searchString))
            {
                return View(await dataContext.Companies.ToListAsync());
            }

            List<Company> searchResult = await dataContext.Companies.Where(o => o.Name.ToLower().Contains(searchString.ToLower())).ToListAsync();
            return View(searchResult);
        }

        public async Task<ActionResult> Details(int id)
        {
            Company company = await dataContext.Companies.FindAsync(id);
            if (company == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            return View(company);
        }

        public async Task<ActionResult> Edit(int id)
        {
            Company company = await dataContext.Companies.FindAsync(id);
            if (company == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            return View(company);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Company model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var company = await dataContext.Companies.FindAsync(model.Id);
            company.Name = model.Name;
            company.HeadquaterLocation = model.HeadquaterLocation;

            await dataContext.SaveChangesAsync();

            return RedirectToAction("Details", new { model.Id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                dataContext.Companies.Remove(new Company { Id = id });
                await dataContext.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                return View("Error", new ErrorViewModel { RequestId = "Can not delete company, because there are JobOffers which reference to it." });
            }

            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Create()
        {
            return View(new Company());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Company model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await dataContext.Companies.AddAsync(model);
            await dataContext.SaveChangesAsync();

            return RedirectToAction("index");
        }
    }
}