#pragma warning disable

using System.ComponentModel.DataAnnotations.Schema;

namespace Spinfluence.Data
{
    public class Practice
    {
        public int Id { get; set; }
        public string Body { get; set; }

        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }

        public int AccountId { get; set; }
        public Account Account { get; set; }

        public int CompanyId { get; set; }
        public Company Company { get; set; }
    }
}
