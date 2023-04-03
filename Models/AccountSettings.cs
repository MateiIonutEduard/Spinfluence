namespace Spinfluence.Models
{
    public class AccountSettings : IAccountSettings
    {
        public string host { get; set; }

        public string port { get; set; }

        public string client { get; set; }

        public string secret { get; set; }
    }
}
