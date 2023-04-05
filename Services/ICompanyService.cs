using Spinfluence.Data;
using Spinfluence.Models;

namespace Spinfluence.Services
{
    public interface ICompanyService
    {
        Task<CompanyDetailsModel[]> GetCompaniesAsync();
        Task<CompanyDetailsModel?> GetCompanyAsync(int id);
        Task<int> CreateCompanyAsync(CompanyModel model);
        Task<bool> RemoveCompanyAsync(int id);
    }
}
