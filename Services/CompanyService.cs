using Microsoft.EntityFrameworkCore;
using Spinfluence.Data;
using Spinfluence.Models;
using System;

namespace Spinfluence.Services
{
    public class CompanyService : ICompanyService
    {
        SpinContext db;

        public CompanyService(SpinContext db)
        { this.db = db; }

        public async Task<bool> CreateCompanyAsync(CompanyModel model)
        {
            string logoImage = string.Empty;
            string posterImage = string.Empty;

            if (model.logoImage != null)
            {
                logoImage = $"./Storage/companies/logo/{model.logoImage.FileName}";
                var ms = new MemoryStream();
                await model.logoImage.CopyToAsync(ms);
                File.WriteAllBytes(logoImage, ms.ToArray());
            }

            if (model.posterImage != null)
            {
                posterImage = $"./Storage/companies/poster/{model.posterImage.FileName}";
                var ms = new MemoryStream();
                await model.posterImage.CopyToAsync(ms);
                File.WriteAllBytes(posterImage, ms.ToArray());
            }

            if(!string.IsNullOrEmpty(logoImage) && !string.IsNullOrEmpty(posterImage))
            {
                Company company = new Company
                {
                    Name = model.name,
                    Description = model.description,
                    Seats = Convert.ToInt32(model.seats),
                    PosterImage = posterImage,
                    LogoImage = logoImage
                };

                db.Company.Add(company);
                await db.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<Company[]> GetCompaniesAsync()
        {
            List<Company> companies = await db.Company.ToListAsync();
            return companies.ToArray();
        }

        public async Task<Company?> GetCompanyAsync(int id)
        {
            Company? company = await db.Company.FirstOrDefaultAsync(c => c.Id == id);
            return company;
        }

        public async Task<bool> RemoveCompanyAsync(int id)
        {
            Company? company = await db.Company.FirstOrDefaultAsync(c => c.Id == id);
            
            if(company != null)
            {
                db.Company.Remove(company);
                await db.SaveChangesAsync();
                return true;
            }

            return false;
        }
    }
}
