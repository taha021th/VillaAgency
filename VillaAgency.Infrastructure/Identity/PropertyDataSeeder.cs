//using Microsoft.AspNetCore.Identity;
//using Microsoft.Extensions.DependencyInjection;
//using MongoDB.Driver;
//using VillaAgency.Application.Common.Interfaces;
//using VillaAgency.Domain.Entities;
//using VillaAgency.Domain.Entities.Users;

//namespace VillaAgency.Infrastructure.Identity
//{
//    public static class PropertyDataSeeder
//    {
//        public static async Task SeedPropertiesAsync(IServiceProvider serviceProvider)
//        {
//            var context = serviceProvider.GetRequiredService<IApplicationDbContext>();
//            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

//            // اگر از قبل ملکی در دیتابیس وجود داشت، کاری انجام نده
//            if (await context.Properties.CountDocumentsAsync(_ => true) > 0)
//            {
//                return;
//            }

//            // دریافت لیست دسته‌بندی‌ها و مشاوران موجود
//            var categories = await context.Categories.Find(_ => true).ToListAsync();
//            var agents = await userManager.GetUsersInRoleAsync("Agent");

//            if (!categories.Any() || !agents.Any())
//            {
//                // اگر هیچ دسته‌بندی یا مشاوری وجود نداشت، نمی‌توان ملک نمونه ساخت
//                return;
//            }

//            var random = new Random();
//            var properties = new List<Property>();

//            // **اصلاح شد:** لیست تصاویر نمونه اکنون شامل مسیر کامل وب است
//            var sampleImages = new List<string>
//            {
//                "/images/property-01.jpg", "/images/property-02.jpg", "/images/property-03.jpg",
//                "/images/property-04.jpg", "/images/property-05.jpg", "/images/property-06.jpg"
//            };

//            for (int i = 1; i <= 10000; i++)
//            {
//                var randomCategory = categories[random.Next(categories.Count)];
//                var randomAgent = agents[random.Next(agents.Count)];

//                var property = new Property
//                {
//                    Id = Guid.NewGuid(),
//                    Title = $"ملک شماره {i} در {randomCategory.Name}",
//                    Description = "این یک توضیح نمونه برای ملک است که به صورت خودکار ایجاد شده. این ملک دارای ویژگی‌های منحصر به فرد و موقعیت مکانی عالی می‌باشد.",
//                    Address = $"پردیس، {randomCategory.Name}، خیابان نمونه، پلاک {i}",
//                    Price = random.Next(1, 10) * 500_000_000M,
//                    Area = random.Next(70, 250),
//                    Bedrooms = random.Next(1, 5),
//                    Floor = random.Next(1, 15),
//                    FloorsCount = 15,
//                    Unit = random.Next(1, 60),
//                    UnitsCountInFloor = 4,
//                    BuildDate = "1403/01/02",
//                    CreatedAt = DateTime.UtcNow.AddDays(-random.Next(0, 366)),

//                    // **اصلاح شد:** اکنون آدرس کامل تصویر در دیتابیس ذخیره می‌شود
//                    ImageUrls = new List<string> { sampleImages[random.Next(sampleImages.Count)] },

//                    CategoryId = randomCategory.Id,
//                    CategoryName = randomCategory.Name,
//                    AgentId = randomAgent.Id,
//                    AgentName = randomAgent.FullName
//                };
//                properties.Add(property);
//            }

//            if (properties.Any())
//            {
//                await context.Properties.InsertManyAsync(properties);
//            }
//        }
//    }
//}
