#pragma warning disable

using System.ComponentModel.DataAnnotations.Schema;

namespace Spinfluence.Data
{
    public class Company
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Seats { get; set; }
        public string LogoImage { get; set; }
        public string PosterImage { get; set; }
    }
}
