using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Services;
using NLog;
using NLog.Web;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using WebApplication1.Models;

namespace WebApplication1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews(options => 
            {
                options.Filters.Add<WebApplication1.Filters.GlobalExceptionFilter>();
                options.Filters.Add<WebApplication1.Filters.AddCustomHeaderResultFilter>();
            });
            
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => {
                options.SignIn.RequireConfirmedAccount = false;
            })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Account/Login";
                options.AccessDeniedPath = "/Account/AccessDenied";
            });

            builder.Services.AddAuthentication()
                .AddGoogle(options =>
                {
                    var clientId = builder.Configuration["Authentication:Google:ClientId"];
                    var clientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
                    
                    if (string.IsNullOrEmpty(clientId) || string.IsNullOrEmpty(clientSecret)
                        || clientId.StartsWith("PLACE_YOUR") || clientSecret.StartsWith("PLACE_YOUR"))
                    {
                        Console.WriteLine("WARNING: Google OAuth credentials are not configured. Google login will not work.");
                        Console.WriteLine("Run: dotnet user-secrets set \"Authentication:Google:ClientId\" \"YOUR_ID\"");
                        Console.WriteLine("Run: dotnet user-secrets set \"Authentication:Google:ClientSecret\" \"YOUR_SECRET\"");
                    }
                    
                    options.ClientId = clientId ?? "NOT_CONFIGURED";
                    options.ClientSecret = clientSecret ?? "NOT_CONFIGURED";
                });
            
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            builder.Services.AddScoped<IFileUploadService, FileUploadService>();

            builder.Services.AddKeyedScoped<INotificationService, EmailNotificationService>("email");
            builder.Services.AddKeyedScoped<INotificationService, SmsNotificationService>("sms");
            builder.Services.AddScoped(typeof(WebApplication1.Repositories.IRepository<>), typeof(WebApplication1.Repositories.Repository<>));

            builder.Services.AddScoped<WebApplication1.Filters.ValidateLocationFilter>();
            builder.Services.AddScoped<WebApplication1.Filters.StudentHeaderAuthorizationFilter>();

            builder.Services.AddAutoMapper(cfg => cfg.AddMaps(typeof(Program).Assembly));

            builder.Logging.ClearProviders();
            builder.Host.UseNLog();

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<AppDbContext>();
                SeedData.Initialize(context);

                try 
                {
                    SeedData.SeedRolesAndAdminAsync(services).GetAwaiter().GetResult();
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred seeding the DB.");
                }
            }

            app.UseMiddleware<WebApplication1.Middleware.ExceptionHandlingMiddleware>();

            if (!app.Environment.IsDevelopment())
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSession();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
