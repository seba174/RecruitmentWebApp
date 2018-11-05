using System.Collections.Generic;

namespace RecrutimentApp.Models
{
    public class JobOfferCreateView : JobOffer
    {
        public IEnumerable<Company> Companies { get; set; }
    }
}
