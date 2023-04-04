#pragma warning disable

using Spinfluence.Data;

namespace Spinfluence.Models
{
    public class CompanyDetailsModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string LogoImage { get; set; }
        public string PosterImage { get; set; }
        public int CompanyEvents { get; set; }
        public List<CompanyEvent>? CompanyEventList { get; set; } 
    }
}
