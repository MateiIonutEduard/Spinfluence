namespace Spinfluence.Models
{
    public class PracticeEventSearchFilter
    {
        public string? EventName { get; set; }
        public bool IsCanceled { get; set; }
        public int PracticeStatus { get; set; }
    }
}
