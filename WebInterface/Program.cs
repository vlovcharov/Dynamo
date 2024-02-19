using Serilog;
using WebInterface.DTOs;
using WebInterface.Services;

namespace WebInterface
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.            
            builder.Services.AddScoped<IFileParseService, FileParseService>();
            builder.Services.AddScoped<ICoinLoreService, CoinLoreService>();
            builder.Services.AddScoped<IPortfolioCalculationService, PortfolioCalculationService>();
            builder.Services.AddControllersWithViews();
            builder.Services.AddMemoryCache();

            builder.Services.Configure<Settings>(builder.Configuration.GetSection("Settings"));

            builder.Host.UseSerilog((context, configuration) =>
                configuration.ReadFrom.Configuration(context.Configuration));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
