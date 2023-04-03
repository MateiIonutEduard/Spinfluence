﻿#pragma warning disable

using System.ComponentModel.DataAnnotations.Schema;

namespace Spinfluence.Data
{
    public class Practice
    {
        public int Id { get; set; }
        public string Body { get; set; }
        public bool IsCanceled { get; set; }

        public int AccountId { get; set; }
        public Account Account { get; set; }

        public int CompanyEventId { get; set; }
        public CompanyEvent CompanyEvent { get; set; }
    }
}
