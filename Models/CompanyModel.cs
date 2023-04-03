﻿#pragma warning disable

namespace Spinfluence.Models
{
    public class CompanyModel
    {
        public string name { get; set; }
        public string description { get; set; }
        public string seats { get; set; }
        public IFormFile logoImage { get; set; }
        public IFormFile posterImage { get; set; }
    }
}
