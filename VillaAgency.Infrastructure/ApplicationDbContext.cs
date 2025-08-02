using Microsoft.Extensions.Options;
using MongoDB.Driver;
using VillaAgency.Application.Common.Interfaces;
using VillaAgency.Domain.Entities;
using VillaAgency.Domain.Entities.Users;
using VillaAgency.Infrastructure.Settings;

namespace VillaAgency.Infrastructure
{
    public class ApplicationDbContext : IApplicationDbContext
    {
        private readonly IMongoDatabase _database;
        public ApplicationDbContext(IOptions<DatabaseSettings> dbSettings)
        {
            var client = new MongoClient(dbSettings.Value.ConnectionString);
            _database=client.GetDatabase(dbSettings.Value.DatabaseName);
        }
        public IMongoCollection<Property> Properties => _database.GetCollection<Property>("Properties");

        public IMongoCollection<ApplicationUser> Users => _database.GetCollection<ApplicationUser>("users");
        public IMongoCollection<ApplicationRole> Roles => _database.GetCollection<ApplicationRole>("roles");

        public IMongoCollection<ContactMessage> ContactMessages => _database.GetCollection<ContactMessage>("contactMessages");
        public IMongoCollection<Category> Categories => _database.GetCollection<Category>("catories");
        public IMongoCollection<VisitRequest> VisitRequests => _database.GetCollection<VisitRequest>("visitRequests");
        public IMongoCollection<PropertySubmission> PropertySubmissions => _database.GetCollection<PropertySubmission>("propertySubmissions");
        public IMongoCollection<PropertyRequest> PropertyRequests => _database.GetCollection<PropertyRequest>("propertyRequests");



    }
}
