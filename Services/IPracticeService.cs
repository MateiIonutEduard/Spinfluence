using Spinfluence.Data;
using Spinfluence.Models;

namespace Spinfluence.Services
{
    public interface IPracticeService
    {
        Task<PracticeEventModel[]> GetPracticesAsync(string token);
        Task<int> AddPracticeAsync(PracticeModel practiceModel, string token);
        Task<int> CancelPracticeEventAsync(int practiceId, string token);
    }
}
