using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Spinfluence.Data;
using Spinfluence.Infrastructure;
using Spinfluence.Models;
using Spinfluence.Services;
using System.Text;

namespace Spinfluence
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if(args.Length > 0)
            {
                // execute in terminal
                CliUtils.ExecuteCommand(args);
                Environment.Exit(0);
            }

            var builder = WebApplication.CreateBuilder(args);
            var config = builder.Configuration;

            string connectionString = config.GetConnectionString("SpinFluenceDB");
            ServerVersion serverVersion = ServerVersion.AutoDetect(connectionString);

            builder.Services.AddDbContext<SpinContext>(options =>
                options.UseMySql(connectionString, serverVersion));

            builder.Services.AddSingleton<IAccountSettings>(sp =>
                sp.GetRequiredService<IOptions<AccountSettings>>().Value);

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = false,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = config["JwtSettings:Issuer"],
                        ValidAudience = config["JwtSettings:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Convert.FromBase64String(config["JwtSettings:Key"]))
                    };
                });

            // Add services to the container.
            builder.Services.AddTransient<IAccountService, AccountService>();
            builder.Services.AddTransient<IPracticeService, PracticeService>();
            builder.Services.AddTransient<ICompanyService, CompanyService>();
            builder.Services.AddControllersWithViews(); 
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

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}