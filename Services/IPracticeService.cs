using Spinfluence.Data;
using Spinfluence.Models;

namespace Spinfluence.Services
{
    public interface IPracticeService
    {
        Task<Practice[]> GetPracticesAsync(string token);
        Task<bool?> AddPracticeAsync(PracticeModel practiceModel, string token);
        Task<int> CancelPracticeEventAsync(int practiceId, string token);
    }
}
