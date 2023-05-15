﻿using Spinfluence.Data;
using Spinfluence.Models;

namespace Spinfluence.Services
{
    public interface IPracticeService
    {
        string GetContentType(string filePath);
        Task<string> GetPracticeAsync(int id, string? type);
        Task<PracticeEventModel[]> GetPracticesAsync(PracticeEventSearchFilter? filter, string token);
        Task<int> AddPracticeAsync(PracticeModel practiceModel, string token);
        Task<int> CancelPracticeEventAsync(int practiceId, string token);
    }
}
