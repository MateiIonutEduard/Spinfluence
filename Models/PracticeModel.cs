#pragma warning disable

namespace Spinfluence.Models
{
    public class PracticeModel
    {
        public int CompanyEventId { get; set; }
        public string Body { get; set; }
        public IFormFile? Resume { get; set; }
        public IFormFile? CoverLetter { get; set; }
    }
}
