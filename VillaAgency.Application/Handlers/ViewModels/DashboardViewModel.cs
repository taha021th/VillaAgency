using VillaAgency.Domain.Entities;

namespace VillaAgency.Application.Handlers.ViewModels
{
    public class DashboardViewModel
    {
        public int TotalProperties { get; set; }
        public int PropertiesRegisteredInPeriod { get; set; }

        public int TotalUsers { get; set; }
        public int UsersRegisteredInPeriod { get; set; }

        public int TotalVisitRequests { get; set; }
        public int VisitRequestsInPeriod { get; set; }
        public int PendingVisitRequests { get; set; }

        public int TotalSubmitProperties { get; set; }
        public int PendingSubmitProperties { get; set; }

        public int TotalRequestProperties { get; set; }
        public int PendingRequestProperties { get; set; }

        public int PeriodInDays { get; set; }
        public int TotalCategories { get; set; }
        public int UnreadMessages { get; set; }

        public List<VisitRequest> RecentVisitRequests { get; set; } = new();
        public List<Property> MostViewedProperties { get; set; } = new();
        public List<PropertyRequest> PropertyRequests { get; set; } = new();
        public List<PropertySubmission> PropertySubmissions { get; set; } = new();


    }
}
