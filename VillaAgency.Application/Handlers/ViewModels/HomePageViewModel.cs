using VillaAgency.Domain.Entities;

namespace VillaAgency.Application.Handlers.ViewModels
{
    public class HomePageViewModel
    {
        public IEnumerable<Property> SliderProperties { get; set; } = new List<Property>();
        public Property? FeaturedProperty { get; set; }
        public int TotalPropertiesCount { get; set; }
        public int TotalAgentsCount { get; set; }
        public IEnumerable<Property> LatestProperties { get; set; } = new List<Property>();
        public Property? ApartmentDeals { get; set; } = new Property();
        public Property? VillaDeals { get; set; } = new Property();
        public Property? PenthouseDeals { get; set; } = new Property();


    }
}
