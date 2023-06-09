﻿#pragma warning disable

namespace Spinfluence.Models
{
    public class PracticeEventModel
    {
        public int? AccountId { get; set; }
        public string? ApplicantName { get; set; }
        public int PracticeId { get; set; }
        public string Body { get; set; }
        public string Name { get; set; }
        public DateTime BeginDate { get; set; }
        public string Resume { get; set; }
        public string CoverLetter { get; set; }
        public bool? IsApproved { get; set; }
        public bool IsCanceled { get; set; }
        public DateTime EndDate { get; set; }
        public string CompanyName { get; set; }
        public int Seats { get; set; }
    }
}
