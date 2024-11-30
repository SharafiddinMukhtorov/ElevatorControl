using ActualLab.Fusion.UI;
using ActualLab.Fusion;
using Elevator.Components;
using Microsoft.EntityFrameworkCore;
using ActualLab.Fusion.Extensions;
using Elevator.Infrastructure;

namespace Elevator
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add Razor Components with Interactive Server Components support
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();

            // Configure Fusion - add Fusion service and FusionTime for automatic updates
            var fusion = builder.Services.AddFusion();
            fusion.AddFusionTime();
            fusion.AddService<ElevatorService>();

            // Register ElevatorService as IComputeService for Fusion
            builder.Services.AddScoped<ElevatorService>(); // Add ElevatorService for Fusion

            // Register UICommander for managing UI commands
            builder.Services.AddScoped<UICommander>();

            // Add HttpContextAccessor for session management or request context
            builder.Services.AddHttpContextAccessor();

            // Configure DbContext with PostgreSQL connection string from app settings
            var connectionString = builder.Configuration.GetConnectionString("ElevatorDb");
            builder.Services.AddDbContext<ElevatorControlDbContext>(options =>
                options.UseNpgsql(connectionString));

            // Add Antiforgery and HTTPS redirection for security
            builder.Services.AddAntiforgery(options => options.HeaderName = "X-XSRF-TOKEN");

            // Build Application
            var app = builder.Build();

            // Configure middleware (error handling, static files, etc.)
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts(); // Enforce HTTP Strict Transport Security (HSTS)
            }

            app.UseHttpsRedirection(); // Enforce HTTPS
            app.UseStaticFiles(); // Serve static files (CSS, JS, images, etc.)
            app.UseAntiforgery(); // Enable antiforgery tokens for security

            // Map Razor Components and enable Fusion interactive rendering
            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode(); // Enable interactive server-side rendering for Fusion

            // Run the application
            app.Run();
        }
    }
}
