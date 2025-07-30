using MongoDB.Driver;
using VillaAgency.Domain.Entities;
using VillaAgency.Domain.Entities.Users;

namespace VillaAgency.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        IMongoCollection<Property> Properties { get; }
        IMongoCollection<ContactMessage> ContactMessages { get; }
        IMongoCollection<ApplicationUser> Users { get; }
        IMongoCollection<ApplicationRole> Roles { get; }
        IMongoCollection<Category> Categories { get; }
        IMongoCollection<VisitRequest> VisitRequests { get; }
        IMongoCollection<PropertySubmission> PropertySubmissions { get; }
    }
}
