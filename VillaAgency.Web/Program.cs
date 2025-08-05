using VillaAgency.Application;
using VillaAgency.Infrastructure;




var builder = WebApplication.CreateBuilder(args);

// فراخوانی متد ثبت سرویس‌ها از لایه‌های دیگر

builder.Services.AddApplicationServices();

builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath="/Login";
    options.AccessDeniedPath="/AccessDenied";
});




// اضافه کردن سرویس‌های خود لایه Web
builder.Services.AddControllers();
builder.Services.AddRazorPages();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var rediConnection = builder.Configuration.GetConnectionString("Redis");
builder.Services.AddSingleton<StackExchange.Redis.IConnectionMultiplexer>(sp =>
{
    return StackExchange.Redis.ConnectionMultiplexer.Connect(rediConnection);
});


var app = builder.Build();


//using (var scope = app.Services.CreateScope())
//{
//    var serviceProvider = scope.ServiceProvider;
//    try
//    {
//        await VillaAgency.Infrastructure.Identity.IdentityDataSeeder.SeedRolesAndAdminUserAsync(serviceProvider);

//        await VillaAgency.Infrastructure.Identity.PropertyDataSeeder.SeedPropertiesAsync(serviceProvider);
//    }
//    catch (Exception ex)
//    {
//        var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
//        logger.LogError(ex, "An error occurred while seeding the database.");

//    }
//}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapRazorPages();
app.MapControllers();

app.Run();