#pragma warning disable

namespace Spinfluence.Models
{
    public class PracticeEventModel
    {
        public int PracticeId { get; set; }
        public string Body { get; set; }

        public string Name { get; set; }
        public DateTime BeginDate { get; set; }

        public DateTime EndDate { get; set; }
        public string CompanyName { get; set; }
        public int Seats { get; set; }
    }
}
