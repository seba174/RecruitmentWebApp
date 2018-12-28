using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using RecrutimentApp.Controllers;
using RecrutimentApp.EntityFramework;
using RecrutimentApp.Models;
using Xunit;

namespace RecrutimentApp.UnitTests
{
    public class SampleApiTests
    {
        public class SampleApiConstructor
        {
            [Fact]
            public void ConstructorShouldThrowIfDataContextIsNull()
            {
                Assert.Throws<ArgumentNullException>(() => new SampleApiController(null));
            }
        }

        public class GetJobOffersMethod : IDisposable
        {
            private readonly DbContextOptions<DataContext> options;
            private readonly SampleApiController apiController;

            public GetJobOffersMethod()
            {
                options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(nameof(GetJobOffersMethod))
                .Options;

                apiController = new SampleApiController(new DataContext(options));
            }

            public void Dispose()
            {
                using (var dataContext = new DataContext(options))
                {
                    dataContext.Database.EnsureDeleted();
                }
            }

            [Theory]
            [InlineData(null)]
            [InlineData("")]
            [InlineData("Backend")]
            public async void WhenThereAreNoJobOffersInDatabaseAndAnySearchStringIsProvidedShouldReturnEmptyEnumerable(string searchString)
            {
                var jobOffers = await apiController.GetJobOffers(searchString);

                Assert.Empty(jobOffers.JobOffers);
            }

            [Theory]
            [InlineData("Backend")]
            [InlineData("Frontend")]
            public async void ShouldReturnOneOfferIfSearchStringMatchesOnlyOneOffer(string searchString)
            {
                AddTwoOffersAndThreeCompaniesToDatabase();

                var jobOffers = await apiController.GetJobOffers(searchString);

                Assert.True(jobOffers.JobOffers.Count() == 1);
            }

            [Theory]
            [InlineData("")]
            [InlineData(null)]
            public async void ShouldReturnAllOffersFromDatabaseIfSearchStringIsNullOrEmpty(string searchString)
            {
                AddTwoOffersAndThreeCompaniesToDatabase();

                var jobOffers = await apiController.GetJobOffers(searchString);

                Assert.True(jobOffers.JobOffers.Count() == 2);
            }

            private void AddTwoOffersAndThreeCompaniesToDatabase()
            {
                using (var dataContext = new DataContext(options))
                {
                    dataContext.Companies.AddRange(
                        new Company() { Name = "Predica" },
                        new Company() { Name = "Microsoft" },
                        new Company() { Name = "Github" });
                    dataContext.SaveChanges();
                    dataContext.JobOffers.AddRange(
                        new JobOffer
                        {
                            JobTitle = "Backend developer",
                            CompanyId = dataContext.Companies.FirstOrDefault(c => c.Name == "Predica")?.Id ?? 0
                        },
                        new JobOffer
                        {
                            JobTitle = "Frontend developer",
                            CompanyId = dataContext.Companies.FirstOrDefault(c => c.Name == "Microsoft")?.Id ?? 0
                        });

                    dataContext.SaveChanges();
                }
            }
        }

        public class GetJobApplicationsMethod : IDisposable
        {
            private readonly DbContextOptions<DataContext> options;
            private readonly SampleApiController apiController;

            public GetJobApplicationsMethod()
            {
                options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(nameof(GetJobApplicationsMethod))
                .Options;

                apiController = new SampleApiController(new DataContext(options));
            }

            public void Dispose()
            {
                using (var dataContext = new DataContext(options))
                {
                    dataContext.Database.EnsureDeleted();
                }
            }

            [Theory]
            [InlineData(0)]
            [InlineData(100)]
            public async void SearchingForIdWithEmptyDatabaseShouldReturnEmptyEnumerable(int id)
            {
                var jobApplications = await apiController.GetJobApplications(id);

                Assert.Empty(jobApplications);
            }

            [Theory]
            [InlineData(10)]
            [InlineData(0)]
            [InlineData(-1)]
            public async void SearchingForNonExsitingIdShouldReturnEmptyEnumerable(int id)
            {
                AddTwoJobApplicationsReferencingTheSameJobOffer();

                var jobApplications = await apiController.GetJobApplications(id);

                Assert.Empty(jobApplications);
            }

            [Fact]
            public async void ShouldReturnAllJobApplicationsFromDatabase()
            {
                AddTwoJobApplicationsReferencingTheSameJobOffer();
                int id = 1;

                var jobApplications = await apiController.GetJobApplications(id);

                Assert.True(jobApplications.Count() == 2);
            }

            private void AddTwoJobApplicationsReferencingTheSameJobOffer()
            {
                using (var dataContext = new DataContext(options))
                {
                    dataContext.Companies.Add(
                        new Company()
                        {
                            Id = 1
                        }
                    );
                    dataContext.SaveChanges();

                    dataContext.JobOffers.Add(
                        new JobOffer
                        {
                            Id = 1,
                            JobTitle = "Backend developer",
                            CompanyId = 1
                        }
                    );
                    dataContext.SaveChanges();

                    dataContext.JobApplications.AddRange(
                        new JobApplication
                        {
                            JobOfferId = 1
                        },
                        new JobApplication
                        {
                            JobOfferId = 1
                        }
                    );
                    dataContext.SaveChanges();
                }
            }
        }
    }
}
