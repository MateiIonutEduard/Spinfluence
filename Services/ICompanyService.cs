using Spinfluence.Data;
using Spinfluence.Models;

namespace Spinfluence.Services
{
    public interface ICompanyService
    {
        Task<Company[]> GetCompaniesAsync();
        Task<Company?> GetCompanyAsync(int id);
        Task<bool> CreateCompanyAsync(CompanyModel model);
        Task<bool> RemoveCompanyAsync(int id);
    }
}
