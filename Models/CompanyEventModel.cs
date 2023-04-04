#pragma warning disable

namespace Spinfluence.Models
{
    public class CompanyEventModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }
        public int TotalSeats { get; set; }
    }
}
