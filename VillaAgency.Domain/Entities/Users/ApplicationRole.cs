using AspNetCore.Identity.MongoDbCore.Models;
using MongoDbGenericRepository.Attributes;


namespace VillaAgency.Domain.Entities.Users
{
    [CollectionName("roles")]

    public class ApplicationRole : MongoIdentityRole<Guid>
    {
    }
}
