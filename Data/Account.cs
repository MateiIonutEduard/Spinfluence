using Microsoft.AspNetCore.Mvc.ViewEngines;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
#pragma warning disable

namespace Spinfluence.Data
{
    public class Account
    {
        public int Id { get; set; }

        [StringLength(80)]
        public string username { get; set; }

        [StringLength(64)]
        public string password { get; set; }

        public string address { get; set; }

        public string logo { get; set; }

        public string token { get; set; }

        [ForeignKey("AccountId")]
        public virtual ICollection<Practice> Practices { get; set; }

        public bool admin { get; set; }
    }
}
