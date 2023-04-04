#pragma warning disable

using System.ComponentModel.DataAnnotations.Schema;

namespace Spinfluence.Data
{
    public class CompanyEvent
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Seats { get; set; }

        [ForeignKey("CompanyEventId")]
        public virtual ICollection<Practice> Practices { get; set; }

        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }
    }
}
