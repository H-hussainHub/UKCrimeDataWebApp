namespace UKCrimeDataWebApp.Models
{
    public class Crime
    {
        public string Category { get; set; }
    }

    public class CrimeSummary
    {
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Date { get; set; }
        public Dictionary<string, int> CrimesByCategory { get; set; } = new Dictionary<string, int>();
    }
}
