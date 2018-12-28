using System.Collections.Generic;

namespace RecrutimentApp.Models
{
    public class JobOfferPagingViewModel
    {
        public IEnumerable<JobOffer> JobOffers { get; set; }

        public int TotalPage { get; set; }
    }
}
