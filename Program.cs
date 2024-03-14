using Microsoft.EntityFrameworkCore;
using OnLineBankingApp.Models;

namespace OnLineBankingApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContextPool<AppDBContext>(

 options => options.UseSqlServer(builder.Configuration.GetConnectionString("MyDBConnection")));

            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(10);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            {
                builder.Services.AddCors(options =>
                {
                    options.AddPolicy("AllowAll", builder =>
                        builder.AllowAnyOrigin()
                               .AllowAnyMethod()
                               .AllowAnyHeader());
                });
            }


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();
            app.UseRouting();
            app.UseSession();
            app.UseCors("AllowAll");


            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=OnLineBankingPortal}/{action=Index}/{id?}");

            app.Run();
        }
    }
}