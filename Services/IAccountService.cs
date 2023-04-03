namespace Spinfluence.Services
{
    public interface IAccountService
    {
        Task<string> Login(string address, string password);
        Task<string> Signup(string username, string password, string address, IFormFile logo, bool admin);
        Task<string> RefreshToken(string token);
        dynamic About(string token);
        bool Recover(string address);
        Task<bool> UpdatePassword(string token, string key);
        string EncryptString(string data);
        void SendEmail(string to, string subject, string body);
        string DecryptString(string data);
    }
}
