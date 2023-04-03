namespace Spinfluence.Models
{
    public interface IAccountSettings
    {
        string host { get; set; }

        string port { get; set; }

        string client { get; set; }

        string secret { get; set; }
    }
}
