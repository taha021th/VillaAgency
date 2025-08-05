using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using VillaAgency.Application.Common.Interfaces;
using VillaAgency.Domain.Entities;
using VillaAgency.Domain.Entities.Enums;
using VillaAgency.Domain.Entities.Users;

namespace VillaAgency.Infrastructure.Identity
{
    public static class PropertyDataSeeder
    {
        public static async Task SeedPropertiesAsync(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<IApplicationDbContext>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            if (await context.Properties.CountDocumentsAsync(_ => true) > 0)
            {
                return;
            }

            var categories = await context.Categories.Find(_ => true).ToListAsync();
            var agents = await userManager.GetUsersInRoleAsync("Agent");

            if (!categories.Any() || !agents.Any())
            {
                return;
            }

            var random = new Random();
            var properties = new List<Property>();
            var propertyTypes = Enum.GetValues(typeof(PropertyType)).Cast<PropertyType>().ToList();

            var sampleImages = new List<string>
            {
                "/images/property-01.jpg", "/images/property-02.jpg", "/images/property-03.jpg",
                "/images/property-04.jpg", "/images/property-05.jpg", "/images/property-06.jpg"
            };

            for (int i = 1; i <= 10000; i++)
            {
                var randomCategory = categories[random.Next(categories.Count)];
                var randomAgent = agents[random.Next(agents.Count)];

                var property = new Property
                {
                    Id = Guid.NewGuid(),
                    Title = $"ملک شماره {i} در {randomCategory.Name}",
                    // ... سایر انتساب‌ها ...
                    CreatedAt = DateTime.UtcNow.AddDays(-random.Next(0, 366)),
                    ImageUrls = new List<string> { sampleImages[random.Next(sampleImages.Count)] },
                    CategoryId = randomCategory.Id,
                    CategoryName = randomCategory.Name,
                    AgentId = randomAgent.Id,
                    AgentName = randomAgent.FullName,

                    // **انتساب نوع ملک به صورت تصادفی**

                };
                properties.Add(property);
            }

            if (properties.Any())
            {
                await context.Properties.InsertManyAsync(properties);
            }
        }
    }
}
