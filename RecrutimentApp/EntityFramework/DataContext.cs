using Microsoft.EntityFrameworkCore;
using RecrutimentApp.Models;

namespace RecrutimentApp.EntityFramework
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<JobApplication> JobApplications { get; set; }

        public DbSet<JobOffer> JobOffers { get; set; }

        public DbSet<Company> Companies { get; set; }
    }
}
