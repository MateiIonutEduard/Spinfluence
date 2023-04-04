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
                    PosterImage = posterImage,
                    LogoImage = logoImage
                };

                db.Company.Add(company);
                await db.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<CompanyDetailsModel[]> GetCompaniesAsync()
        {
            List<CompanyDetailsModel> companies = 
            (
             from c in await db.Company.ToListAsync()
             let counter = (from e in db.CompanyEvent.ToList() where e.CompanyId == c.Id select e).Count()
             select new CompanyDetailsModel
             {
                 Id = c.Id,
                 Name = c.Name,
                 Description = c.Description,
                 LogoImage = c.LogoImage,
                 PosterImage = c.PosterImage,
                 CompanyEvents = counter
             }
            ).ToList();

            return companies.ToArray();
        }

        public async Task<CompanyDetailsModel?> GetCompanyAsync(int id)
        {
            Company? company = await db.Company.FirstOrDefaultAsync(c => c.Id == id);

            if (company != null)
            {
                var companyEvents = (from e in await db.CompanyEvent.Where(e => e.CompanyId == id)
                    .ToListAsync() join p in await db.Practice.ToListAsync() on e.Id equals p.CompanyEventId
                    let seatEvents = db.Practice.Where(p => p.CompanyEventId == e.Id).Count()
                    select new CompanyEventModel
                    {
                        Id = e.Id,
                        Name = e.Name,
                        BeginDate = e.BeginDate,
                        EndDate = e.EndDate,
                        TotalSeats = e.Seats - seatEvents
                    }
                ).ToList();

                int TotalSeats = companyEvents.Sum(e => e.TotalSeats);

                CompanyDetailsModel companyDetailsModel = new CompanyDetailsModel
                {
                    Id = company.Id,
                    Name = company.Name,
                    Description = company.Description,
                    LogoImage = company.LogoImage,
                    PosterImage = company.PosterImage,
                    CompanyEventList = companyEvents,
                    CompanyEvents = TotalSeats
                };

                return companyDetailsModel;
            }

            return null;
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
